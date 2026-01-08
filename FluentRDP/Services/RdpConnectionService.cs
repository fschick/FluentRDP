using AxMSTSCLib;
using FluentRDP.Configuration;
using FluentRDP.Configuration.Enums;
using FluentRDP.Events;
using FluentRDP.Extensions;
using MSTSCLib;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace FluentRDP.Services;

internal sealed class RdpConnectionService : IDisposable
{
    private const uint DEVICE_SCALE_FACTOR = 100;
    private const string RDP_CONTROL_NAME = "rdpControl";
    private const string DESKTOP_SCALE_FACTOR_PROPERTY = "DesktopScaleFactor";
    private const string DEVICE_SCALE_FACTOR_PROPERTY = "DeviceScaleFactor";

    private readonly SemaphoreSlim _connectionSemaphore = new(1, 1);
    private readonly Control _container;
    private ConnectionSettings? _connectionSettings;
    private AxMsRdpClient10NotSafeForScripting? _rdpControl;
    private MsRdpClient10NotSafeForScripting? _rdpClient;
    private bool _fullScreen;

    public bool FullScreen { get => _fullScreen; set => SetFullScreen(value, false); }

    public event EventHandler? Connected;
    public event EventHandler<DisconnectedEventArgs>? Disconnected;
    public event EventHandler? Minimized;
    public event EventHandler<FullScreenEventArgs>? FullScreenChanged;

    public RdpConnectionService(Control container)
    {
        _container = container;

        var form = container.FindForm();
        if (form == null)
            throw new ArgumentException("The provided container is not part of a Form.", nameof(container));
    }

    public void Connect(ConnectionSettings connectionSettings)
    {
        if (_rdpControl != null)
            Disconnect(true);

        _connectionSettings = connectionSettings.Clone();

        SetupConnection();

        var fullScreen = _connectionSettings.ScreenMode == ScreenMode.FullScreen;
        var useAllMonitors = _connectionSettings.UseAllMonitors == true;
        if (fullScreen || useAllMonitors)
            SetFullScreen(true, true);

        if (FullScreen)
            _rdpControl.FullScreen = true;

        _rdpControl.Connect();
    }

    public void Reconnect(ConnectionSettings? connectionSettings = null)
    {
        if (_rdpControl == null || _rdpClient == null || _connectionSettings == null)
            return;

        var updatedSettings = connectionSettings ?? _connectionSettings;
        var sessionCanBeUpdated = updatedSettings
            .EqualsExceptBy(
                _connectionSettings,
                nameof(ConnectionSettings.Width),
                nameof(ConnectionSettings.Height),
                nameof(ConnectionSettings.ScaleFactor),
                nameof(ConnectionSettings.AutoResize)
             );

        if (sessionCanBeUpdated)
            UpdateSessionDisplaySettings(updatedSettings);
        else
            Connect(updatedSettings);
    }

    public void Disconnect()
        => Disconnect(false);

    private void Disconnect(bool isReconnect)
    {
        _rdpControl?.Disconnect();
        CleanupConnection(DisconnectedEventArgs.LOCAL_NOT_ERROR, true);
    }

    [MemberNotNull(nameof(_rdpControl), nameof(_rdpClient))]
    private void SetupConnection()
    {
        _connectionSemaphore.Wait();

        try
        {
            if (_rdpControl != null)
                throw new InvalidOperationException("RDP connection is already established.");

            CreateRdpControlAndClient();
            WireRdpControlEvents();
            ApplySettingsToRdpClient();
        }
        finally
        {
            _connectionSemaphore.Release();
        }
    }

    private void CleanupConnection(int reasonCode, bool isReconnect)
    {
        _connectionSemaphore.Wait();

        var reason = GetErrorDescription(reasonCode);

        try
        {
            if (_rdpControl == null)
                return;

            SetFullScreen(false, true);
            UnwireRdpControlEvents();
            RemoveRdpControlAndClient();
        }
        finally
        {
            _connectionSemaphore.Release();
        }

        Disconnected?.Invoke(this, new DisconnectedEventArgs(reasonCode, reason, isReconnect));
    }

    [MemberNotNull(nameof(_rdpControl), nameof(_rdpClient))]
    private void CreateRdpControlAndClient()
    {
        Debug.Assert(_rdpControl is null);
        Debug.Assert(_rdpClient is null);

        _rdpControl = new AxMsRdpClient10NotSafeForScripting();

        _rdpControl.BeginInit();
        _rdpControl.Name = RDP_CONTROL_NAME;
        _rdpControl.Dock = DockStyle.Fill;
        _rdpControl.EndInit();

        _container.Controls.Add(_rdpControl);

        var rdpClient = (MsRdpClient10NotSafeForScripting?)_rdpControl.GetOcx();
        _rdpClient = rdpClient ?? throw new InvalidOperationException("Failed to create RDP client control.");
        _rdpClient.AdvancedSettings9.ContainerHandledFullScreen = 1;
    }

    private void RemoveRdpControlAndClient()
    {
        Debug.Assert(_rdpControl is not null);
        Debug.Assert(_rdpClient is not null);

        _container.Controls.Remove(_rdpControl);

        _rdpControl.Dispose();
        _rdpControl = null;
        _rdpClient = null;
    }

    private void WireRdpControlEvents()
    {
        Debug.Assert(_rdpControl is not null);

        _rdpControl.OnConnected += OnConnected;
        _rdpControl.OnDisconnected += OnDisconnected;
        _rdpControl.OnConfirmClose += OnConfirmClose;
        _rdpControl.OnRequestContainerMinimize += OnRequestContainerMinimize;
        _rdpControl.OnRequestLeaveFullScreen += OnRequestLeaveFullScreen;
    }

    private void UnwireRdpControlEvents()
    {
        Debug.Assert(_rdpControl is not null);

        _rdpControl.OnConnected -= OnConnected;
        _rdpControl.OnDisconnected -= OnDisconnected;
        _rdpControl.OnConfirmClose -= OnConfirmClose;
        _rdpControl.OnRequestContainerMinimize -= OnRequestContainerMinimize;
        _rdpControl.OnRequestLeaveFullScreen -= OnRequestLeaveFullScreen;
    }

    private void ApplySettingsToRdpClient()
    {
        Debug.Assert(_connectionSettings is not null);
        Debug.Assert(_rdpClient is not null);

        var useAllMonitors = _connectionSettings.UseAllMonitors == true;
        var fixedSize = useAllMonitors || _connectionSettings.AutoResize == false;
        var extendedSettings = (IMsRdpExtendedSettings)_rdpClient;

        #region Connection options
        var (host, port) = ParseHostAndPort(_connectionSettings.Hostname);

        _rdpClient.Server = host;

        if (!string.IsNullOrWhiteSpace(port))
            _rdpClient.AdvancedSettings9.RDPPort = int.Parse(port);

        if (_connectionSettings.Domain != null)
            _rdpClient.Domain = _connectionSettings.Domain;

        if (_connectionSettings.Username != null)
            _rdpClient.UserName = _connectionSettings.Username;

        if (_connectionSettings.Password != null)
            _rdpClient.AdvancedSettings9.ClearTextPassword = _connectionSettings.Password;

        if (_connectionSettings.EnableCredSsp != null)
            _rdpClient.AdvancedSettings9.EnableCredSspSupport = _connectionSettings.EnableCredSsp.Value;

        if (_connectionSettings.AuthenticationLevel != null && _connectionSettings.AuthenticationLevel != AuthenticationLevel.NoRequirement)
            _rdpClient.AdvancedSettings9.AuthenticationLevel = (uint)_connectionSettings.AuthenticationLevel.Value;

        if (_connectionSettings.KeepAliveInterval != null)
            _rdpClient.AdvancedSettings2.keepAliveInterval = _connectionSettings.KeepAliveInterval.Value;

        if (_connectionSettings.MaxReconnectAttempts != null)
            _rdpClient.AdvancedSettings9.MaxReconnectAttempts = _connectionSettings.MaxReconnectAttempts.Value;
        #endregion Connection options

        #region Display options
        if (fixedSize && _connectionSettings.Width != null)
            _rdpClient.DesktopWidth = _connectionSettings.Width.Value;

        if (fixedSize && _connectionSettings.Height != null)
            _rdpClient.DesktopHeight = _connectionSettings.Height.Value;

        if (_connectionSettings.ColorDepth != null)
            _rdpClient.ColorDepth = _connectionSettings.ColorDepth.Value;

        var scaleFactor = _connectionSettings.ScaleFactor ?? _container.DeviceZoom;
        extendedSettings.set_Property(DESKTOP_SCALE_FACTOR_PROPERTY, scaleFactor);
        extendedSettings.set_Property(DEVICE_SCALE_FACTOR_PROPERTY, DEVICE_SCALE_FACTOR);

        if (_connectionSettings.SmartSizing != null)
            _rdpClient.AdvancedSettings9.SmartSizing = _connectionSettings.SmartSizing.Value;

        ((IMsRdpClientNonScriptable5)_rdpClient).UseMultimon = useAllMonitors;

        if (_connectionSettings.EnableCompression != null)
            _rdpClient.AdvancedSettings9.Compress = 1;

        if (_connectionSettings.EnableBitmapPersistence != null)
            _rdpClient.AdvancedSettings9.BitmapPersistence = 1;
        #endregion Display options

        #region Redirection options
        if (_connectionSettings.RedirectAudioCapture != null)
            _rdpClient.AdvancedSettings9.AudioCaptureRedirectionMode = _connectionSettings.RedirectAudioCapture.Value;

        if (_connectionSettings.AudioPlaybackMode != null)
            _rdpClient.SecuredSettings3.AudioRedirectionMode = (int)_connectionSettings.AudioPlaybackMode.Value;

        if (_connectionSettings.RedirectCameras != null)
            ApplyCameraRedirection(_connectionSettings.RedirectCameras);

        if (_connectionSettings.RedirectClipboard != null)
            _rdpClient.AdvancedSettings9.RedirectClipboard = _connectionSettings.RedirectClipboard.Value;

        if (_connectionSettings.RedirectComPorts != null)
            _rdpClient.AdvancedSettings9.RedirectPorts = _connectionSettings.RedirectComPorts.Value;

        if (_connectionSettings.RedirectPnpDevices != null)
            ApplyPnpDeviceRedirection(_connectionSettings.RedirectPnpDevices);

        if (_connectionSettings.RedirectPrinters != null)
            _rdpClient.AdvancedSettings9.RedirectPrinters = _connectionSettings.RedirectPrinters.Value;

        if (_connectionSettings.RedirectDrives != null)
            ApplyDriveRedirection(_connectionSettings.RedirectDrives);

        if (_connectionSettings.RedirectLocation != null)
        {
            object redirectLocation = _connectionSettings.RedirectLocation.Value;
            extendedSettings.set_Property("EnableLocationRedirection", ref redirectLocation);
        }

        if (_connectionSettings.RedirectSmartCards != null)
            _rdpClient.AdvancedSettings9.RedirectSmartCards = _connectionSettings.RedirectSmartCards.Value;

        if (_connectionSettings.RedirectWebAuthn != null)
            _rdpClient.AdvancedSettings9.PluginDlls = "WebAuthn.dll";

        if (_connectionSettings.KeyboardMode != null)
            _rdpClient.SecuredSettings3.KeyboardHookMode = (int)_connectionSettings.KeyboardMode.Value;
        #endregion

        #region Experience options
        if (_connectionSettings.DisplayConnectionBar != null)
            _rdpClient.AdvancedSettings9.DisplayConnectionBar = _connectionSettings.DisplayConnectionBar.Value;

        if (_connectionSettings.PinConnectionBar != null)
            _rdpClient.AdvancedSettings9.PinConnectionBar = _connectionSettings.PinConnectionBar.Value;
        #endregion Experience options
    }

    private static (string Host, string Port) ParseHostAndPort(string? hostname)
    {
        var hostPortMatch = Regex.Match(hostname ?? string.Empty, @"^(?<host>(?:\[([0-9a-fA-F:]+)\])|([^:\[\]]+))(?::(?<port>\d+))?$");
        if (!hostPortMatch.Success)
            throw new ArgumentException("Invalid hostname / IP address format.", nameof(hostname));

        var host = hostPortMatch.Groups["host"].Value;
        var port = hostPortMatch.Groups["port"].Value;
        if (string.IsNullOrWhiteSpace(host))
            throw new ArgumentException("No hostname / IP address specified.", nameof(hostname));

        return (host, port);
    }

    private void ApplyCameraRedirection(string camerasToRedirect)
    {
        Debug.Assert(_rdpClient is not null);

        // ReSharper disable once SuspiciousTypeConversion.Global
        var cameraCollection = ((IMsRdpClientNonScriptable8)_rdpClient).CameraRedirConfigCollection;

        if (camerasToRedirect == "*")
        {
            for (uint i = 0; i < cameraCollection.Count; i++)
                cameraCollection.ByIndex[i].Redirected = true;
        }
        else
        {
            var requestedCameras = camerasToRedirect
                .Split([';', ','], StringSplitOptions.RemoveEmptyEntries)
                .Select(camera => camera.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            for (uint i = 0; i < cameraCollection.Count; i++)
            {
                var camera = cameraCollection.ByIndex[i];
                camera.Redirected = requestedCameras.Contains(camera.SymbolicLink);
            }
        }
    }

    private void ApplyPnpDeviceRedirection(string devicesToRedirect)
    {
        Debug.Assert(_rdpClient is not null);

        var deviceCollection = ((IMsRdpClientNonScriptable3)_rdpClient).DeviceCollection;

        if (devicesToRedirect.Equals("DynamicDevices", StringComparison.OrdinalIgnoreCase))
        {
            _rdpClient.AdvancedSettings6.RedirectDevices = true;
        }
        else if (devicesToRedirect == "*")
        {
            for (uint i = 0; i < deviceCollection.DeviceCount; i++)
                deviceCollection.DeviceByIndex[i].RedirectionState = true;
        }
        else
        {
            var requestedDevices = devicesToRedirect
                .Split([';', ','], StringSplitOptions.RemoveEmptyEntries)
                .Select(camera => camera.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            for (uint i = 0; i < deviceCollection.DeviceCount; i++)
            {
                var device = deviceCollection.DeviceByIndex[i];
                var isRedirected = requestedDevices.Contains(device.FriendlyName) || requestedDevices.Contains(device.DeviceInstanceId);
                device.RedirectionState = isRedirected;
            }
        }
    }

    private void ApplyDriveRedirection(string drivesToRedirect)
    {
        Debug.Assert(_rdpClient is not null);

        var driveCollection = ((IMsRdpClientNonScriptable3)_rdpClient).DriveCollection;

        if (drivesToRedirect.Equals("DynamicDrives", StringComparison.OrdinalIgnoreCase))
        {
            _rdpClient.AdvancedSettings9.RedirectDrives = true;
        }
        else if (drivesToRedirect == "*")
        {
            for (uint i = 0; i < driveCollection.DriveCount; i++)
                driveCollection.DriveByIndex[i].RedirectionState = true;
        }
        else
        {
            var requestedDrives = drivesToRedirect
                .Split([';', ','], StringSplitOptions.RemoveEmptyEntries)
                .Select(drive => drive.Trim().TrimEnd('\\', ':'))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            for (uint i = 0; i < driveCollection.DriveCount; i++)
            {
                var drive = driveCollection.DriveByIndex[i];
                var driveLetter = drive.Name[..1];
                drive.RedirectionState = requestedDrives.Contains(driveLetter);
            }
        }
    }

    private void UpdateSessionDisplaySettings(ConnectionSettings connectionSettings)
    {
        Debug.Assert(_rdpClient is not null);

        _connectionSettings = connectionSettings.Clone();

        var isAutoSize = _connectionSettings.AutoResize != false;
        var disconnected = _rdpClient.Connected != 1;
        var useAllMonitors = _connectionSettings.UseAllMonitors == true;
        if (disconnected || useAllMonitors)
            return;

        var currentWidth = (uint)_rdpClient.DesktopWidth;
        var currentHeight = (uint)_rdpClient.DesktopHeight;
        var updatedWidth = isAutoSize ? (uint)_container.Size.Width : (uint)(_connectionSettings.Width ?? _container.Size.Width);
        var updatedHeight = isAutoSize ? (uint)_container.Size.Height : (uint)(_connectionSettings.Height ?? _container.Size.Height);

        var extendedSettings = (IMsRdpExtendedSettings)_rdpClient;
        var currentScaleFactor = (uint)extendedSettings.get_Property(DESKTOP_SCALE_FACTOR_PROPERTY);
        var updatedScaleFactor = _connectionSettings.ScaleFactor ?? currentScaleFactor;

        if (currentWidth == updatedWidth && currentHeight == updatedHeight && currentScaleFactor == updatedScaleFactor)
            return;

        try
        {
            if (_rdpClient is IMsRdpClient10 rdpClient10) // RDP 10/8.1+
                rdpClient10.UpdateSessionDisplaySettings(updatedWidth, updatedHeight, updatedWidth, updatedHeight, 0, updatedScaleFactor, DEVICE_SCALE_FACTOR);
            else // Fallback: RDP 8
                _rdpClient.Reconnect(updatedWidth, updatedHeight);
        }
        catch (COMException) { }
    }

    private void SetFullScreen(bool value, bool forceIfDisconnected)
    {
        if (_fullScreen == value)
            return;

        var isDisconnected = _rdpClient?.Connected != 1;
        if (isDisconnected && !forceIfDisconnected)
            return;

        _fullScreen = value;
        _rdpControl?.FullScreen = value;

        var fullScreenEventArgs = new FullScreenEventArgs(_fullScreen, _connectionSettings?.UseAllMonitors ?? false);
        FullScreenChanged?.Invoke(this, fullScreenEventArgs);
    }

    private string? GetErrorDescription(int reasonCode = 0)
        => _rdpClient?.GetErrorDescription((uint)reasonCode, (uint)_rdpClient.ExtendedDisconnectReason);

    private void OnConnected(object? sender, EventArgs e)
        => Connected?.Invoke(this, EventArgs.Empty);

    private void OnDisconnected(object? sender, IMsTscAxEvents_OnDisconnectedEvent e)
        => CleanupConnection(e.discReason, false);

    private static void OnConfirmClose(object? sender, IMsTscAxEvents_OnConfirmCloseEvent e)
        => e.pfAllowClose = true;

    private void OnRequestContainerMinimize(object? sender, EventArgs e)
        => Minimized?.Invoke(this, EventArgs.Empty);

    private void OnRequestLeaveFullScreen(object? sender, EventArgs e)
        => SetFullScreen(false, true);

    #region IDisposable Support
    public void Dispose()
        => CleanupConnection(DisconnectedEventArgs.LOCAL_NOT_ERROR, false);
    #endregion IDisposable Support
}