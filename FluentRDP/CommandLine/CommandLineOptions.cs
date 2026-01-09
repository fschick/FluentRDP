using FluentRDP.Configuration;
using FluentRDP.Configuration.Enums;
using FluentRDP.Extensions;
using FluentRDP.Platform;
using FluentRDP.Services;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FluentRDP.CommandLine;

/// <summary>
/// Parses and stores command line options for FluentRDP
/// </summary>
internal class CommandLineOptions
{
    /// <summary>
    /// Gets or sets whether to show help
    /// </summary>
    private bool ShowHelp { get; set; }

    /// <summary>
    /// Parses command line arguments using Mono.Options
    /// </summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>Parsed options</returns>
    public static ApplicationSettings Parse(string[] args)
    {
        var options = new CommandLineOptions();
        var settings = new ApplicationSettings();

        try
        {
            var optionSet = CreateOptionSet(options, settings);
            var extraArgs = optionSet.Parse(args);

            ProcessPositionalRdpFileArgument(settings, extraArgs);
            ProcessPositionalHostnameArgument(settings, extraArgs);

            if (extraArgs.Count > 0)
                throw new OptionException($"Unrecognized arguments: {string.Join(", ", extraArgs)}", string.Join(", ", extraArgs));

            if (options.ShowHelp)
                ShowHelpAndExit();

            if (!string.IsNullOrEmpty(settings.RdpFilePath))
            {
                var rdpFileSettings = RdpFileService.ReadFromFile(settings.RdpFilePath);
                settings.Connection.MergeFrom(rdpFileSettings, nameof(ConnectionSettings.Password));
            }

            settings.Connection.MergeFrom(ConnectionSettings.Default);

            return settings;
        }
        catch (OptionException ex)
        {
            var errorMessage = $"Command line error:\n\n{ex.Message}\n\nUse --help to see usage information.";
            DisplayMessage(errorMessage, "FluentRDP - Command Line Error", MessageBoxIcon.Error);
            Application.Exit();
            throw;
        }
    }

    /// <summary>
    /// Processes positional argument to check if it's an RDP file and sets RdpFilePath accordingly.
    /// If the first positional argument ends with .rdp and RdpFilePath is not already set,
    /// it will be used as the RDP file path.
    /// </summary>
    /// <param name="settings">The application settings to update</param>
    /// <param name="args">The list of extra arguments from command line parsing</param>
    private static void ProcessPositionalRdpFileArgument(ApplicationSettings settings, IList<string> args)
    {
        if (args.Count <= 0)
            return;

        var arg0IsRdpFile = args[0].EndsWith(".rdp", StringComparison.OrdinalIgnoreCase);
        if (!arg0IsRdpFile)
            return;

        settings.RdpFilePath = args[0];
        args.RemoveAt(0);
    }

    /// <summary>
    /// Processes positional argument as a hostname if it's not an RDP file.
    /// If the first positional argument doesn't end with .rdp and hostname is not already set,
    /// it will be used as the hostname.
    /// </summary>
    /// <param name="settings">The application settings to update</param>
    /// <param name="args">The list of extra arguments from command line parsing</param>
    private static void ProcessPositionalHostnameArgument(ApplicationSettings settings, IList<string> args)
    {
        if (args.Count <= 0)
            return;

        if (!args[0].IsValidHostname())
            return;

        settings.Connection.Hostname = args[0];
        args.RemoveAt(0);
    }

    private static OptionSet CreateOptionSet(CommandLineOptions options, ApplicationSettings settings)
    {
        var optionSet = new OptionSet
        {
            // Connection options
            { "h|host|hostname=", "Remote computer hostname or IP address. Supports: hostname, IPv4, IPv6. With port: hostname:port, IPv4:port, [IPv6]:port. Examples: server.com, 10.0.0.1:3390, [2001:db8::1]:3389",
                v => settings.Connection.Hostname = v },
            { "d|domain=", "Domain for authentication",
                v => settings.Connection.Domain = v },
            { "u|user|username=", "Username for authentication",
                v => settings.Connection.Username = v },
            { "password=", "Password for authentication (not recommended)",
                v => settings.Connection.Password = v },
            { "enable-credssp|credssp", "Enable CredSSP (Credential Security Support Provider) authentication",
                v => settings.Connection.EnableCredSsp = v != null ? true : null },
            { "auth-level|authentication-level=", "Server authentication level: no-warning, no-connect, warn, none (default: none)",
                v => settings.Connection.AuthenticationLevel = ParseAuthenticationLevel(v) },
            { "keep-alive-interval=", "Keep-alive interval in milliseconds (default: not set)",
                (int? v) => settings.Connection.KeepAliveInterval = v },
            { "max-reconnect-attempts=", "Maximum number of automatic reconnection attempts (default: not set)",
                (int? v) => settings.Connection.MaxReconnectAttempts = v },
            { "no-auto-connect|edit", "Disable automatic connection when hostname is provided",
                v => settings.NoAutoConnect = v != null ? true : null },

            // Display options
            { "width=", "Desktop width in pixels",
                (int? v) => settings.Connection.Width = v },
            { "height=", "Desktop height in pixels",
                (int? v) => settings.Connection.Height = v },
            { "color-depth=", "Color depth (8, 15, 16, 24, or 32, default: 32)",
                (int? v) => settings.Connection.ColorDepth = v },
            { "scale|scale-factor=", "DPI scale factor percentage (100, 125, 150, 200, etc.). Use 0 for auto-detection (default: 0)",
                (uint? v) => settings.Connection.ScaleFactor = v },
            { "smart-sizing", "Enable SmartSizing (bitmap scaling)",
                v => settings.Connection.SmartSizing = v != null ? true: null },
            { "no-auto-resize", "Disable automatic resize on window resize",
                v => settings.Connection.AutoResize = v != null ? false: null },
            { "f|fullscreen", "Start in full-screen mode",
                v => settings.Connection.ScreenMode = v != null ? ScreenMode.FullScreen : null },
            { "all-monitors|multi-monitor|multimon", "Use all monitors for the remote desktop session (span across all monitors)",
                v => settings.Connection.UseAllMonitors = v != null ? true: null },

            // Experience options
            { "show-connection-bar|display-connection-bar", "Display connection bar in fullscreen mode",
                v => settings.Connection.DisplayConnectionBar = v != null ? true: null },
            { "hide-connection-bar|no-connection-bar", "Hide connection bar in fullscreen mode",
                v => settings.Connection.DisplayConnectionBar = v != null ? false: null },
            { "pin-connection-bar", "Pin the connection bar in fullscreen mode",
                v => settings.Connection.PinConnectionBar = v != null ? true: null },

            // Redirection options
            { "redirect-clipboard", "Disable clipboard redirection",
                v => settings.Connection.RedirectClipboard = v == null ? false: null },
            { "audio-mode=", "Audio output mode: local, remote, or none (default: local)",
                v => settings.Connection.AudioPlaybackMode = ParseAudioMode(v) },
            { "redirect-audio-capture|redirect-microphone", "Enable audio input/microphone redirection",
                v => settings.Connection.RedirectAudioCapture = v != null ? true: null },
            { "redirect-cameras=", "Redirect cameras (* for all, device path for specific)",
                v => settings.Connection.RedirectCameras = v },
            { "redirect-comports|redirect-com-ports", "Enable COM port redirection",
                v => settings.Connection.RedirectComPorts = v != null ? true: null },
            { "redirect-pnp-devices=|redirect-pnp=", "Redirect PnP devices (* for all, DynamicDevices for dynamic, device instance IDs/friendly names for specific, MTP/PTP devices)",
                v => settings.Connection.RedirectPnpDevices = v },
            { "redirect-drives=", "Drives to redirect (* for all, DynamicDrives, or drive letters like C;E)",
                v => settings.Connection.RedirectDrives = v != null ? "*":null },
            { "redirect-location", "Enable location redirection",
                v => settings.Connection.RedirectLocation = v != null ? true: null },
            { "redirect-printers", "Enable printer redirection",
                v => settings.Connection.RedirectPrinters = v != null ? true: null },
            { "redirect-smartcards", "Enable smart card redirection",
                v => settings.Connection.RedirectSmartCards = v != null ? true: null },
            { "redirect-webauthn", "Enable WebAuthn redirection (Windows Hello, security keys)",
                v => settings.Connection.RedirectWebAuthn = v != null ? true: null },
            { "keyboard-mode=", "Keyboard hook mode: local, remote, or fullscreen",
                v => settings.Connection.KeyboardMode = ParseKeyboardMode(v) },

            // Window state and position options
            { "window-position|windows-pos|position|pos=", "Window position in pixels (format: x,y)",
                v => ParseWindowPosition(v, settings) },
            { "window-size|size=", "Window size in pixels (format: <width>,<height> or <width>x<height>)",
                v => ParseWindowSize(v, settings) },
            { "windows-maximized|maximized|windows-max|max", "Start with maximized window",
                v => settings.Window.Maximized = v != null ? true: null },
            { "no-close-on-disconnect|no-auto-close", "Do not close the application window when RDP disconnects",
                v => settings.Window.NoCloseOnDisconnect = v != null ? true: null },

            // Other options
            { "rdp|rdp-file=", "Path to an RDP file to load settings from",
                v => settings.RdpFilePath = v },

            { "help|?", "Show this help message",
                v => options.ShowHelp = v != null }
        };

        return optionSet;
    }

    /// <summary>
    /// Parses a window position string in "x,y" format
    /// </summary>
    private static void ParseWindowPosition(string? value, ApplicationSettings settings)
    {
        if (value == null)
            return;

        var parts = value.Split(',');
        if (parts.Length != 2)
            throw new OptionException($"Invalid window position format: {value}. Expected format: x,y (e.g., 100,200)", "window-position");

        if (!int.TryParse(parts[0].Trim(), out var x))
            throw new OptionException($"Invalid window position X value: {parts[0]}. Must be an integer.", "window-position");

        if (!int.TryParse(parts[1].Trim(), out var y))
            throw new OptionException($"Invalid window position Y value: {parts[1]}. Must be an integer.", "window-position");

        settings.Window.WindowX = x;
        settings.Window.WindowY = y;
    }

    /// <summary>
    /// Parses a window size string in "x&lt;width&gt;,x&lt;height&gt;" or "x&lt;width&gt;x&lt;height&gt;" format
    /// </summary>
    private static void ParseWindowSize(string? value, ApplicationSettings settings)
    {
        if (value == null)
            return;

        // Try both comma and 'x' as separators
        var parts = value.Split(',', 'x');

        if (parts.Length != 2)
            throw new OptionException($"Invalid window size format: {value}. Expected format: <width>,<height> or <width>x<height> (e.g., 1024,768 or 1024x768)", "window-size");

        if (!int.TryParse(parts[0].Trim(), out var width))
            throw new OptionException($"Invalid window width value: {parts[0]}. Must be an integer.", "window-size");

        if (!int.TryParse(parts[1].Trim(), out var height))
            throw new OptionException($"Invalid window height value: {parts[1]}. Must be an integer.", "window-size");

        settings.Window.WindowWidth = width;
        settings.Window.WindowHeight = height;
    }

    /// <summary>
    /// Parses an audio mode string to AudioPlaybackMode enum
    /// </summary>
    private static AudioPlaybackMode? ParseAudioMode(string? value)
    {
        if (value == null)
            return null;

        switch (value.ToLowerInvariant())
        {
            case "local":
            case "play-local":
            case "0":
                return AudioPlaybackMode.PlayOnLocal;
            case "remote":
            case "play-remote":
            case "1":
                return AudioPlaybackMode.PlayOnRemote;
            case "none":
            case "dont-play":
            case "do-not-play":
            case "2":
                return AudioPlaybackMode.DoNotPlay;
            default:
                throw new OptionException($"Invalid audio mode: {value}. Valid values are: local, remote, none (or 0, 1, 2)", "audio-mode");
        }
    }

    /// <summary>
    /// Parses a keyboard mode string to KeyboardMode enum
    /// </summary>
    private static KeyboardMode? ParseKeyboardMode(string? value)
    {
        if (value == null)
            return null;

        switch (value.ToLowerInvariant())
        {
            case "local":
            case "0":
                return KeyboardMode.OnLocalComputer;
            case "remote":
            case "1":
                return KeyboardMode.OnRemoteComputer;
            case "fullscreen":
            case "fullscreen-only":
            case "full-screen-only":
            case "full-screen":
            case "2":
                return KeyboardMode.InFullScreenOnly;
            default:
                throw new OptionException($"Invalid keyboard mode: {value}. Valid values are: local, remote, fullscreen-only (or 0, 1, 2)", "keyboard-mode");
        }
    }

    /// <summary>
    /// Parses an authentication level string to AuthenticationLevel enum
    /// </summary>
    private static AuthenticationLevel? ParseAuthenticationLevel(string? value)
    {
        if (value == null)
            return null;

        switch (value.ToLowerInvariant())
        {
            case "no-warning":
            case "connect-without-warning":
            case "0":
                return AuthenticationLevel.ConnectWithoutWarning;
            case "no-connect":
            case "dont-connect":
            case "do-not-connect":
            case "1":
                return AuthenticationLevel.DoNotConnect;
            case "warn":
            case "warn-user":
            case "warning":
            case "2":
                return AuthenticationLevel.WarnUser;
            case "none":
            case "no-requirement":
            case "3":
                return AuthenticationLevel.NoRequirement;
            default:
                throw new OptionException($"Invalid authentication level: {value}. Valid values are: no-warning, no-connect, warn, none (or 0, 1, 2, 3)", "authentication-level");
        }
    }

    private static void ShowHelpAndExit()
    {
        var helpText = GetHelpText();
        DisplayMessage(helpText, "FluentRDP - Help", MessageBoxIcon.Information);
        Application.Exit();
    }

    /// <summary>
    /// Shows help information
    /// </summary>
    private static string GetHelpText()
    {
        return """
            FluentRDP - A clean and maintainable RDP client with dynamic resize capability

            Usage: FluentRDP.exe [options] [hostname|rdp-file]

            Note: Auto-connect is enabled by default when a hostname is provided.
                    Use --no-auto-connect to disable this behavior.

            RDP file options:
                --rdp, --rdp-file <path>        Load settings from an RDP file
                                                Command line options will override RDP file settings

            Connection options:
                -h, --host, --hostname <host>   Remote computer hostname or IP address
                                                Supports: hostname, IPv4, IPv6
                                                With port: hostname:port, IPv4:port, [IPv6]:port
                                                Examples: server.com, 10.0.0.1:3390, [2001:db8::1]:3389
                -u, --user, --username <user>   Username for authentication
                --password <password>           Password for authentication (not recommended)
                -d, --domain <domain>           Domain for authentication

            Security options:
                --auth-level,                   Server authentication level for certificate validation:
                --authentication-level <lvl>    no-warning - Connect without warning if auth fails (least secure)
                                                no-connect - Don't connect if auth fails (most secure)
                --enable-credssp, --credssp     Enable CredSSP (Credential Security Support Provider) authentication
                                                for Kerberos/NTLM SSO support
                --keep-alive-interval <ms>      Keep-alive interval in milliseconds (sends keep-alive packets to maintain connection)
                --max-reconnect-attempts <num>  Maximum number of automatic reconnection attempts when connection is lost

            Display options:
                --width <width>                 Desktop width in pixels
                --height <height>               Desktop height in pixels
                --color-depth <depth>           Color depth (8, 15, 16, 24, or 32, default: 32)
                --scale, --scale-factor <pct>   DPI scale factor percentage (100, 125, 150, 200, etc.)
                                                Use 0 for auto-detection (default: 0)
                --smart-sizing                  Enable SmartSizing (bitmap scaling)
                --no-auto-resize                Disable automatic resize on window resize
                -f, --fullscreen                Start in full-screen mode
                --use-multimon, --multimon      Use all monitors for remote desktop (span across all monitors)
                                                Requires RDP 8.0 or later

            Experience options:
                --show-connection-bar           Display connection bar in fullscreen mode
                --hide-connection-bar           Hide connection bar in fullscreen mode
                --pin-connection-bar            Pin the connection bar in fullscreen mode

            Window options:
                --window-position <x,y>         Window position in pixels (format: x,y)
                --window-size <size>            Window size in pixels (format: <width>,<height> or <width>x<height>)
                --maximized                     Start with maximized window
                --no-close-on-disconnect         Do not close the application window when RDP disconnects

            Redirection options:
                --no-clipboard                  Disable clipboard redirection
                --audio-mode <mode>             Audio output mode: local, remote, or none (default: local)
                --redirect-audio-capture,       Enable audio input/microphone redirection
                --redirect-microphone
                --redirect-cameras <value>      Redirect cameras (* for all, device path for specific)
                --redirect-comports,            Enable COM port redirection
                --redirect-com-ports
                --redirect-pnp-devices <value>, Redirect PnP devices (* for all, DynamicDevices for dynamic, device instance IDs or friendly names for specific, MTP/PTP devices)
                --redirect-pnp <value>
                --redirect-drives <value>       Specific drives to redirect (* for all, DynamicDrives, or paths like C:\;E:\)
                --redirect-location             Enable location redirection
                --redirect-printers             Enable printer redirection
                --redirect-smartcards           Enable smart card redirection
                --redirect-webauthn             Enable WebAuthn redirection (Windows Hello, security keys)
                --keyboard-mode <mode>          Keyboard hook mode: local, remote, or fullscreen

            Other options:
                --no-auto-connect               Disable automatic connection when hostname is provided
                --help, -?, /?                  Show this help message

            Examples:
                FluentRDP.exe 192.168.1.100
                FluentRDP.exe connection.rdp
                FluentRDP.exe --rdp-file connection.rdp -u administrator
                FluentRDP.exe connection.rdp -u administrator
                FluentRDP.exe --host server.example.com -u administrator
                FluentRDP.exe -h 10.0.0.50 -u user -w 1920 --height 1080 --scale 125
                FluentRDP.exe server01 -u domain\\user --fullscreen --redirect-drives=*
                FluentRDP.exe 192.168.1.100 --no-auto-connect
                FluentRDP.exe -h server01 --maximized --window-size=1280,720
                FluentRDP.exe -h server01 --window-position=100,100 --window-size=1024x768
                FluentRDP.exe -h server01 --use-multimon --fullscreen
                FluentRDP.exe -h server01 --redirect-audio-capture --redirect-cameras=* 
                FluentRDP.exe server01 --audio-mode=remote --redirect-comports --redirect-location
                FluentRDP.exe server01 --redirect-pnp-devices=* --redirect-drives=*
                FluentRDP.exe server01 --redirect-pnp-devices=DynamicDevices --redirect-drives=DynamicDrives

            Note: When loading an RDP file, command line options will override the file settings.

            """;
    }

    /// <summary>
    /// Displays a message either to the console or via MessageBox depending on how the application was launched
    /// </summary>
    /// <param name="message">The message to display</param>
    /// <param name="title">The title for the MessageBox (used only when not in console mode)</param>
    /// <param name="icon">The icon for the MessageBox (used only when not in console mode)</param>
    private static void DisplayMessage(string message, string title, MessageBoxIcon icon)
    {
        // If launched from console, attach to it and write output
        if (IsLaunchedFromConsole())
        {
            Console.WriteLine();
            Console.WriteLine(message);

            // Send a newline to release the console prompt
            SendKeys.SendWait("{ENTER}");
        }
        else
        {
            // Otherwise show message box (e.g., when double-clicked)
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }
    }

    /// <summary>
    /// Checks if the application was launched from a console window
    /// </summary>
    private static bool IsLaunchedFromConsole()
    {
        // Try to attach to the parent process's console
        return Interop.AttachConsole(Interop.ATTACH_PARENT_PROCESS);
    }
}