using FluentRDP.Attributes;
using FluentRDP.Configuration.Enums;
using FluentRDP.Extensions;
using FluentRDP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;

namespace FluentRDP.Configuration;

/// <summary>
/// Represents RDP connection specific settings
/// </summary>
internal class ConnectionSettings
{
    #region Connection options
    /// <summary>
    /// Gets or sets the hostname or IP address to connect to.
    /// Can include optional port using formats: hostname:port, IPv4:port, or [IPv6]:port
    /// Examples: server.com, server.com:3390, 192.168.1.1:3389, [2001:db8::1]:3389
    /// </summary>
    [RdpFile("full address")]
    public string? Hostname { get; set; }

    /// <summary>
    /// Gets or sets the domain for authentication
    /// </summary>
    [RdpFile("domain")]
    public string? Domain { get; set; }

    /// <summary>
    /// Gets or sets the username for authentication
    /// </summary>
    [RdpFile("username")]
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the password for authentication.
    /// If not provided, RDP attempts Windows Credential Manager, then Kerberos/NTLM authentication.
    /// If both username and password are omitted, uses current Windows credentials (SSO).
    /// </summary>
    [NotMapped]
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets whether to enable CredSSP (Kerberos/NTLM SSO) authentication
    /// </summary>
    [RdpFile("enablecredsspsupport", DefaultValue = "1")]
    public bool? EnableCredSsp { get; set; }

    /// <summary>
    /// Gets or sets the server authentication level for certificate validation
    /// </summary>
    [RdpFile("authentication level", DefaultValue = "3")]
    public AuthenticationLevel? AuthenticationLevel { get; set; }

    /// <summary>
    /// Gets or sets the keep-alive interval in milliseconds. Sends keep-alive packets to maintain the connection.
    /// </summary>
    [NotMapped]
    public int? KeepAliveInterval { get; set; }

    /// <summary>
    /// Gets or sets the connection timeout in seconds. Determines how long to wait for the remote host to accept the connection.
    /// </summary>
    [RdpFile("connection timeout")]
    public int? ConnectionTimeout { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of automatic reconnection attempts when the connection is lost.
    /// </summary>
    [RdpFile("autoreconnect max retries", DefaultValue = "20")]
    public int? MaxReconnectAttempts { get; set; }
    #endregion Connection options

    #region Display options
    /// <summary>
    /// Gets or sets the desktop width
    /// </summary>
    [RdpFile("desktopwidth")]
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the desktop height
    /// </summary>
    [RdpFile("desktopheight")]
    public int? Height { get; set; }

    /// <summary>
    /// Gets or sets the color depth
    /// </summary>
    [RdpFile("session bpp", DefaultValue = "32")]
    public int? ColorDepth { get; set; }

    /// <summary>
    /// Gets or sets the desktop scale factor (DPI percentage) in the range from 100 to 500
    /// </summary>
    [RdpFile("desktopscalefactor")]
    public uint? ScaleFactor { get; set; }

    /// <summary>
    /// Gets or sets whether to enable SmartSizing
    /// </summary>
    [RdpFile("smart sizing", DefaultValue = "0")]
    public bool? SmartSizing { get; set; }

    /// <summary>
    /// Gets or sets whether to enable automatic resize
    /// </summary>
    [RdpFile("dynamic resolution", DefaultValue = "1")]
    public bool? AutoResize { get; set; }

    /// <summary>
    /// Determines whether a remote session window appears full screen
    /// </summary>
    [RdpFile("screen mode id", DefaultValue = "1")]
    public ScreenMode? ScreenMode { get; set; }

    /// <summary>
    /// Gets or sets whether to use multiple monitors (span across all monitors)
    /// </summary>
    [RdpFile("use multimon", DefaultValue = "0")]
    public bool? UseAllMonitors { get; set; }

    /// <summary>
    /// Gets or sets whether to enable compression
    /// </summary>
    [RdpFile("compression", DefaultValue = "1")]
    public bool? EnableCompression { get; set; }

    /// <summary>
    /// Gets or sets whether to enable bitmap persistence
    /// </summary>
    [RdpFile("bitmapcachepersistenable", DefaultValue = "1")]
    public bool? EnableBitmapPersistence { get; set; }
    #endregion Display options

    #region Experience options
    /// <summary>
    /// Gets or sets whether to display the connection bar in fullscreen mode
    /// </summary>
    [RdpFile("displayconnectionbar", DefaultValue = "1")]
    public bool? DisplayConnectionBar { get; set; }

    /// <summary>
    /// Gets or sets whether to pin the connection bar
    /// </summary>
    [RdpFile("pinconnectionbar", DefaultValue = "1")]
    public bool? PinConnectionBar { get; set; }
    #endregion Experience options

    #region Redirection options
    /// <summary>
    /// Gets or sets the audio playback mode
    /// </summary>
    [RdpFile("audiomode", DefaultValue = "0")]
    public AudioPlaybackMode? AudioPlaybackMode { get; set; }

    /// <summary>
    /// Gets or sets whether to enable audio input/microphone redirection
    /// </summary>
    [RdpFile("audiocapturemode", DefaultValue = "0")]
    public bool? RedirectAudioCapture { get; set; }

    /// <summary>
    /// Gets or sets the cameras to redirect. Use "*" for all cameras, device path for specific, or null for none
    /// </summary>
    [RdpFile("camerastoredirect")]
    public string? RedirectCameras { get; set; }

    /// <summary>
    /// Gets or sets whether to enable clipboard redirection
    /// </summary>
    [RdpFile("redirectclipboard", DefaultValue = "1")]
    public bool? RedirectClipboard { get; set; }

    /// <summary>
    /// Gets or sets whether to enable COM port redirection
    /// </summary>
    [RdpFile("redirectcomports", DefaultValue = "0")]
    public bool? RedirectComPorts { get; set; }

    /// <summary>
    /// Gets or sets the PnP devices to redirect. Use "*" for all, "DynamicDevices" for dynamic, specific device instance IDs or friendly names, or null for none.
    /// </summary>
    [RdpFile("devicestoredirect")]
    public string? RedirectPnpDevices { get; set; }

    /// <summary>
    /// Gets or sets whether to enable printer redirection
    /// </summary>
    [RdpFile("redirectprinters", DefaultValue = "0")]
    public bool? RedirectPrinters { get; set; }

    /// <summary>
    /// Gets or sets the drives to redirect. Use "*" for all, "DynamicDrives" for dynamic, specific drive letters like "C;E", or null for none.
    /// </summary>
    [RdpFile("drivestoredirect")]
    public string? RedirectDrives { get; set; }

    /// <summary>
    /// Gets or sets whether to enable location redirection
    /// </summary>
    [RdpFile("redirectlocation", DefaultValue = "0")]
    public bool? RedirectLocation { get; set; }

    /// <summary>
    /// Gets or sets whether to enable smart card redirection
    /// </summary>
    [RdpFile("redirectsmartcards", DefaultValue = "1")]
    public bool? RedirectSmartCards { get; set; }

    /// <summary>
    /// Gets or sets whether to enable WebAuthn redirection for Windows Hello and security keys
    /// </summary>
    [RdpFile("redirectwebauthn", DefaultValue = "1")]
    public bool? RedirectWebAuthn { get; set; }

    /// <summary>
    /// Gets or sets the keyboard mode for redirecting Windows key combinations
    /// </summary>
    [RdpFile("keyboardhook", DefaultValue = "2")]
    public KeyboardMode? KeyboardMode { get; set; }
    #endregion Redirection options

    #region Fluent RDP specific settings (non-standard)
    [NotMapped]
    public DateTime LastConnected { get; set; }

    /// <summary>
    /// Gets or sets a color override for the window icon badge.
    /// Allowed values are either a named color (parsed by <see cref="Color.FromName(string)"/>)
    /// or a hex value in the form RRGGBB or AARRGGBB (optional leading '#').
    /// Examples: "Red", "#FF0000", "00FF00", "#80FF0000"
    /// </summary>
    [RdpFile("fluent rdp badge color")]
    public Color? BadgeColor { get; set; }
    #endregion Fluent RDP specific settings (non-standard)

    public ConnectionSettings Clone()
        => (ConnectionSettings)MemberwiseClone();

    /// <summary>
    /// Default connection settings.
    /// </summary>
    [NotMapped]
    public static readonly ConnectionSettings Default = CreateDefaultSettings();

    private static ConnectionSettings CreateDefaultSettings()
    {
        var defaultSettingValues = SettingsToRdpFileService.ConnectionMapping()
            .Select(map => new
            {
                map.Property,
                DefaultValue = map.DefaultValue.ToSettingValue(map.Property.PropertyType)
            })
            .Where(x => x.DefaultValue != null);

        var defaultSettings = new ConnectionSettings();
        foreach (var setting in defaultSettingValues)
            setting.Property.SetValue(defaultSettings, setting.DefaultValue, null);
        return defaultSettings;
    }

    #region Validation
    /// <summary>
    /// Indicates whether all validation checks pass
    /// </summary>
    public bool IsValid() => Validate().Count == 0;

    /// <summary>
    /// Validates the connection settings
    /// </summary>
    /// <returns>A list of validation error messages. Empty if validation succeeds.</returns>
    public IReadOnlyList<string> Validate()
    {
        var errors = new List<string>();

        if (Width is <= 0)
            errors.Add("Width must be greater than 0.");

        if (Height is <= 0)
            errors.Add("Height must be greater than 0.");

        if (ColorDepth.HasValue && ColorDepth.Value is not (8 or 15 or 16 or 24 or 32))
            errors.Add("ColorDepth must be 8, 15, 16, 24, or 32.");

        if (ScaleFactor is < 100 or > 500)
            errors.Add("ScaleFactor must be in the range from 100 to 500.");

        if (!Hostname.IsValidHostname())
            errors.Add("Valid hostname is required.");

        return errors;
    }
    #endregion Validation
}