using FluentRDP.CommandLine;
using FluentRDP.Configuration;
using FluentRDP.Configuration.Enums;
using FluentRDP.Events;
using FluentRDP.Extensions;
using FluentRDP.Platform;
using FluentRDP.Services;
using FluentRDP.UI.Services;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FluentRDP.UI;

public partial class FormRdpClient : Form
{
    private readonly RdpClientService _rdpService;
    private ApplicationSettings _appSettings;
    private Rectangle _nonFullScreenBounds;
    private FormWindowState _nonFullScreenState;

    // Ambient services for window message handling
    private readonly FormSizeEnforcementService _formSizeEnforcementService;
    private readonly FormResizeDetectionService _formResizeDetectionService;
    private readonly FormSystemMenuService _formSystemMenuService;

    // Cancellation token source for delayed connection message display
    private CancellationTokenSource? _connectionDelayCts;

    public FormRdpClient(string[] args)
    {
        InitializeComponent();

        Icon = Properties.Resources.AppIcon;

        _appSettings = CommandLineOptions.Parse(args);

        _formSizeEnforcementService = new FormSizeEnforcementService();
        _formSizeEnforcementService.IsActive = _appSettings.Connection.AutoResize == true;

        _formResizeDetectionService = new FormResizeDetectionService();
        _formResizeDetectionService.ResizeDetected += ResizeDetected;

        _formSystemMenuService = new FormSystemMenuService();
        _formSystemMenuService.ConnectRequested += (_, _) => Connect();
        _formSystemMenuService.DisconnectRequested += (_, _) => Disconnect();
        _formSystemMenuService.FullScreenRequested += (_, _) => ToggleFullScreen();
        _formSystemMenuService.SettingsRequested += (_, _) => ShowSettings();
        _formSystemMenuService.ZoomLevelRequested += (_, e) => SetZoomLevel(e.ZoomLevel);

        _rdpService = new RdpClientService(panelRdp);
        _rdpService.Connected += RdpService_Connected;
        _rdpService.Disconnected += RdpService_Disconnected;
        _rdpService.FullScreenChanged += RdpService_FullScreenChanged;
        _rdpService.Minimized += RdpService_Minimized;
    }

    private void Connect()
    {
        var settingsAreInvalid = !_appSettings.Connection.IsValid();
        while (settingsAreInvalid && ShowSettings())
            settingsAreInvalid = !_appSettings.Connection.IsValid();

        panelStartup.Visible = settingsAreInvalid;

        if (settingsAreInvalid)
            return;

        ShowConnectingMessage($"Connecting to {_appSettings.Connection.Hostname}...");
        _rdpService.Connect(_appSettings.Connection);
    }

    private void AutoStartup()
    {
        var editMode = _appSettings.EditRdpFile == true;
        if (editMode && !ShowSettings())
            return;

        var autoConnectConfigured = _appSettings.NoAutoConnect != true;
        var autoConnectSuppressed = ModifierKeys.HasFlag(Keys.Shift);
        if (autoConnectConfigured && !autoConnectSuppressed)
            Connect();
    }

    private void Disconnect()
    {
        HideConnectingMessage();
        _rdpService.Disconnect();
    }

    private void ShowConnectingMessage(string message)
    {
        _connectionDelayCts?.Cancel();
        _connectionDelayCts?.Dispose();

        panelConnect.Visible = true;

        _connectionDelayCts = new CancellationTokenSource();
        var token = _connectionDelayCts.Token;
        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(1000, token);
                await InvokeAsync(() =>
                {
                    lbConnecting.Text = message;
                    lbConnecting.Visible = true;
                    btnCancel.Visible = true;
                }, token);
            }
            catch (OperationCanceledException) { }
        }, token);
    }

    private void HideConnectingMessage()
    {
        panelConnect.Visible = false;
        lbConnecting.Visible = false;
        btnCancel.Visible = false;
        _connectionDelayCts?.Cancel();
        _connectionDelayCts?.Dispose();
        _connectionDelayCts = null;
    }

    private void ToggleFullScreen()
        => _rdpService.FullScreen = !_rdpService.FullScreen;

    private bool ShowSettings()
    {
        var settingsDialog = new FormSettings(_appSettings);
        var dialogConfirmed = settingsDialog.ShowDialog(this) == DialogResult.OK;
        if (!dialogConfirmed)
            return false;

        _appSettings = settingsDialog.UpdatedSettings;
        _formSystemMenuService.UpdateZoomCheckmarks(_appSettings.Connection.ScaleFactor);
        _rdpService.FullScreen = _appSettings.Connection.ScreenMode == ScreenMode.FullScreen;
        _rdpService.Reconnect(_appSettings.Connection);
        _formSizeEnforcementService.IsActive = _appSettings.Connection.AutoResize == true;
        UpdateWindowTitle();

        return true;
    }

    private void ShowDisconnectStatus(DisconnectedEventArgs e)
    {
        lblStatusMessage.Text = $"Disconnected: Error {e.ReasonCode} ({(int)e.ReasonCode:0}) / {e.ExtendedReasonCode} ({(int)e.ExtendedReasonCode:0})\n{e.ErrorDescription}";
        lblStatusMessage.Visible = !e.IsIntentionalDisconnect;
    }

    private void LoadAndApplyWindowSettings()
    {
        var hostname = _appSettings.Connection.Hostname;
        var persistedSettings = WindowPersistenceService.Load(hostname);
        _appSettings.Window.MergeFrom(persistedSettings);
        this.ApplyWindowSettings(_appSettings.Window);
    }

    private void UpdateWindowTitle()
    {
        var title = "FluentRDP";
        if (!string.IsNullOrEmpty(_appSettings.RdpFilePath))
            title = $"({Path.GetFileNameWithoutExtension(_appSettings.RdpFilePath)}) - {title}";
        if (!string.IsNullOrEmpty(_appSettings.Connection.Hostname))
            title = $"{_appSettings.Connection.Hostname} - {title}";
        Text = title;
    }

    private void SaveWindowSettings()
    {
        // Don't save settings if in full screen mode
        if (FormBorderStyle == FormBorderStyle.None)
            return;

        var windowSettings = this.GetWindowSettings();
        var hostname = _appSettings.Connection.Hostname;
        windowSettings.Save(hostname);
    }

    private void SetZoomLevel(uint zoomPercent)
    {
        _appSettings.Connection.ScaleFactor = zoomPercent;
        _rdpService.Reconnect(_appSettings.Connection);
        _formSystemMenuService.UpdateZoomCheckmarks(_appSettings.Connection.ScaleFactor);
    }

    private void SetFullScreenMode(bool isFullScreen, bool isMultiMonitor)
    {
        if (isFullScreen)
        {
            _formResizeDetectionService.Enabled = false;
            _nonFullScreenState = WindowState;
            if (WindowState == FormWindowState.Maximized)
                WindowState = FormWindowState.Normal;

            _nonFullScreenBounds = Bounds;

            FormBorderStyle = FormBorderStyle.None;
            Bounds = GetTotalMonitorBounds(isMultiMonitor);
            _formResizeDetectionService.Enabled = true;
            _formResizeDetectionService.Resize();
        }
        else
        {
            _formResizeDetectionService.Enabled = false;
            FormBorderStyle = FormBorderStyle.Sizable;
            Bounds = _nonFullScreenBounds;
            WindowState = _nonFullScreenState;
            _formResizeDetectionService.Enabled = true;
            _formResizeDetectionService.Resize();
        }
    }

    private Rectangle GetTotalMonitorBounds(bool useAllMonitors)
    {
        if (!useAllMonitors)
        {
            var screen = Screen.FromControl(this);
            return screen.Bounds;
        }

        var screens = Screen.AllScreens;
        if (screens.Length == 0)
            return Screen.PrimaryScreen?.Bounds ?? new Rectangle();

        var minX = int.MaxValue;
        var minY = int.MaxValue;
        var maxX = int.MinValue;
        var maxY = int.MinValue;

        foreach (var screen in screens)
        {
            if (screen.Bounds.X < minX)
                minX = screen.Bounds.X;
            if (screen.Bounds.Y < minY)
                minY = screen.Bounds.Y;
            if (screen.Bounds.Right > maxX)
                maxX = screen.Bounds.Right;
            if (screen.Bounds.Bottom > maxY)
                maxY = screen.Bounds.Bottom;
        }

        return new Rectangle(minX, minY, maxX - minX, maxY - minY);
    }

    /// <summary>
    /// Called after the form handle has been created
    /// </summary>
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);

        _formResizeDetectionService.AttachToWindow(Handle);
        _formSizeEnforcementService.AttachToWindow(this);
        _formSystemMenuService.AttachToWindow(this);
        _formSystemMenuService.UpdateZoomCheckmarks(_appSettings.Connection.ScaleFactor);
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        var escapeKeyPressed = keyData == Keys.Escape;
        var connectionActive = _rdpService.IsConnected;
        if (!escapeKeyPressed || connectionActive)
            return base.ProcessCmdKey(ref msg, keyData);

        Close();
        return true;
    }

    protected override void WndProc(ref Message m)
    {
        var noBorderClick = m.Msg != Interop.WM_NCLBUTTONDBLCLK;
        if (noBorderClick)
        {
            base.WndProc(ref m);
            return;
        }

        var shiftDown = ModifierKeys.HasFlag(Keys.Shift);
        var isFullScreen = _appSettings.Connection.ScreenMode == ScreenMode.FullScreen;
        var useAllMonitors = _appSettings.Connection.UseAllMonitors == true;
        var noAutoResize = _appSettings.Connection.AutoResize != true;
        switch (m.WParam.ToInt32())
        {
            case Interop.HTCAPTION when shiftDown || isFullScreen || useAllMonitors:
                ToggleFullScreen();
                return;
            case Interop.HTLEFT when noAutoResize:
            case Interop.HTRIGHT when noAutoResize:
            case Interop.HTTOP when noAutoResize:
            case Interop.HTTOPLEFT when noAutoResize:
            case Interop.HTTOPRIGHT when noAutoResize:
            case Interop.HTBOTTOM when noAutoResize:
            case Interop.HTBOTTOMLEFT when noAutoResize:
            case Interop.HTBOTTOMRIGHT when noAutoResize:
                if (_appSettings.Connection.Height.HasValue)
                    BeginInvoke(() => Height = _appSettings.Connection.Height.Value + 39);
                if (_appSettings.Connection.Width.HasValue)
                    BeginInvoke(() => Width = _appSettings.Connection.Width.Value + 16);
                return;
            default:
                base.WndProc(ref m);
                break;
        }
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connectionDelayCts?.Cancel();
            _connectionDelayCts?.Dispose();
            _rdpService.Dispose();
            _formResizeDetectionService.Dispose();
            _formSizeEnforcementService.Dispose();
            _formSystemMenuService.Dispose();
            components?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void RdpService_Connected(object? sender, EventArgs e)
    {
        HideConnectingMessage();
        _formSystemMenuService.EnableMenuItem(Interop.SC_FULLSCREEN, true);
        RecentConnectionsService.AddOrUpdate(_appSettings.Connection);
    }

    private void RdpService_Disconnected(object? sender, DisconnectedEventArgs e)
    {
        if (e.IsReconnect)
            return;

        panelStartup.Visible = true;
        _formSystemMenuService.EnableMenuItem(Interop.SC_FULLSCREEN, false);

        HideConnectingMessage();
        ShowDisconnectStatus(e);

        var closeOnDisconnect = _appSettings.NoCloseOnDisconnect != true;
        var closeNotSuppressed = !ModifierKeys.HasFlag(Keys.Shift);
        if (closeOnDisconnect && closeNotSuppressed && e.IsIntentionalDisconnect)
        {
            Close();
            Application.Exit();
        }
    }

    private void RdpService_FullScreenChanged(object? sender, FullScreenEventArgs e)
        => SetFullScreenMode(e.IsFullScreen, e.IsMultiMonitor);

    private void RdpService_Minimized(object? sender, EventArgs e)
        => WindowState = FormWindowState.Minimized;

    private void ResizeDetected(object? sender, EventArgs e)
    {
        if (WindowState != FormWindowState.Minimized)
            _rdpService.Reconnect();
    }

    private void FormRdpClient_Shown(object sender, EventArgs e)
    {
        LoadAndApplyWindowSettings();
        UpdateWindowTitle();
        AutoStartup();
    }

    private void FormRdpClient_FormClosing(object? sender, FormClosingEventArgs e)
        => SaveWindowSettings();

    private void btnConnect_Click(object? sender, EventArgs e)
        => Connect();

    private void btnSettings_Click(object? sender, EventArgs e)
    {
        if (ShowSettings())
            Connect();
    }

    private void btnCancel_Click(object sender, EventArgs e)
        => Disconnect();
}
