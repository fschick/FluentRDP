using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FluentRDP.Platform;

[SuppressMessage("ReSharper", "IdentifierTypo")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression")]
internal static class Interop
{
    // Window Messages
    public const int WM_SETREDRAW = 0x000B;
    public const int WM_WINDOWPOSCHANGING = 0x0046;
    public const int WM_WINDOWPOSCHANGED = 0x0047;
    public const int WM_NCLBUTTONDOWN = 0x00A1;
    public const int WM_NCLBUTTONDBLCLK = 0x00A3;
    public const int WM_SIZING = 0x0214;
    public const int WM_ENTERSIZEMOVE = 0x0231;
    public const int WM_EXITSIZEMOVE = 0x0232;
    public const int WM_SYSCOMMAND = 0x0112;

    // Window Positioning Constants
    public const int SWP_NOSIZE = 0x0001;
    public const int SWP_NOMOVE = 0x0002;
    public const int SWP_NOZORDER = 0x0004;
    public const int SWP_NOACTIVATE = 0x0010;

    // Window Sizing Edge Constants
    public const int WMSZ_LEFT = 1;
    public const int WMSZ_RIGHT = 2;
    public const int WMSZ_TOP = 3;
    public const int WMSZ_TOPLEFT = 4;
    public const int WMSZ_TOPRIGHT = 5;
    public const int WMSZ_BOTTOM = 6;
    public const int WMSZ_BOTTOMLEFT = 7;
    public const int WMSZ_BOTTOMRIGHT = 8;

    // Non-Client Hit Test Constants
    public const int HTCAPTION = 2;
    public const int HTLEFT = 10;
    public const int HTRIGHT = 11;
    public const int HTTOP = 12;
    public const int HTTOPLEFT = 13;
    public const int HTTOPRIGHT = 14;
    public const int HTBOTTOM = 15;
    public const int HTBOTTOMLEFT = 16;
    public const int HTBOTTOMRIGHT = 17;

    // System Metrics Constants
    public const int SM_REMOTESESSION = 0x1000;

    // Menu Flags
    public const int MF_SEPARATOR = 0x800;
    public const int MF_STRING = 0x0;
    public const int MF_POPUP = 0x10;
    public const int MF_BYPOSITION = 0x400;
    public const int MF_BYCOMMAND = 0x0;
    public const int MF_ENABLED = 0x0;
    public const int MF_DISABLED = 0x2;
    public const int MF_GRAYED = 0x1;
    public const int MF_CHECKED = 0x8;
    public const int MF_UNCHECKED = 0x0;

    // Custom System Menu Command IDs (must be < 0xF000)
    public const int SC_CONNECT = 0x1000;
    public const int SC_DISCONNECT = 0x1001;
    public const int SC_FULLSCREEN = 0x1002;
    public const int SC_SETTINGS = 0x1003;
    public const int SC_ZOOM_100 = 0x1004;
    public const int SC_ZOOM_125 = 0x1005;
    public const int SC_ZOOM_150 = 0x1006;
    public const int SC_ZOOM_175 = 0x1007;
    public const int SC_ZOOM_200 = 0x1008;
    public const int SC_ZOOM_250 = 0x1009;
    public const int SC_ZOOM_300 = 0x100A;
    public const int SC_ZOOM_400 = 0x100B;
    public const int SC_ZOOM_500 = 0x100C;

    // Console Constants
    public const int ATTACH_PARENT_PROCESS = -1;

    // Credential Persistence Constants
    internal const int CRED_PERSIST_SESSION = 1;
    internal const int CRED_PERSIST_LOCAL_MACHINE = 2;
    internal const int CRED_PERSIST_ENTERPRISE = 3;

    private static readonly IEnumerable<Size> _commonResolutions = [
        new(1920, 1080),
        new(1680, 1050),
        new(1600, 900),
        new(1440, 900),
        new(1366, 768),
        new(1280, 1024),
        new(1280, 720),
        new(1024, 768),
        new(800, 600)
    ];

    /// <summary>
    /// Gets all available display resolutions from the system. If running in a remote session, it retrieves the resolutions of all screens and adds common resolutions.
    /// </summary>
    /// <returns>A collection of unique resolutions as (Width, Height) tuples, sorted by width then height</returns>
    public static List<Size> GetAvailableDisplayResolutions()
    {
        var resolutions = new HashSet<Size>();

        if (IsRemoteSession())
        {
            foreach (var screen in Screen.AllScreens)
                resolutions.Add(new Size(screen.Bounds.Width, screen.Bounds.Height));

            foreach (var resolution in _commonResolutions)
                resolutions.Add(resolution);
        }
        else
        {
            var modeNum = 0;
            var deviceMode = new Interop.DEVMODE();
            while (Interop.EnumDisplaySettings(null, modeNum, ref deviceMode))
            {
                if (deviceMode is { dmPelsWidth: > 0, dmPelsHeight: > 0 })
                    resolutions.Add(new Size(deviceMode.dmPelsWidth, deviceMode.dmPelsHeight));
                modeNum++;
            }
        }

        return resolutions
            .OrderByDescending(r => r.Width)
            .ThenByDescending(r => r.Height)
            .ToList();
    }

    /// <summary>
    /// Determines if the application is running in a remote desktop session
    /// </summary>
    public static bool IsRemoteSession()
        => GetSystemMetrics(SM_REMOTESESSION) != 0;

    // P/Invoke Methods - Kernel32.dll
    [DllImport("kernel32.dll")]
    public static extern bool AttachConsole(int dwProcessId);

    // P/Invoke Methods - User32.dll (System Menu)
    [DllImport("user32.dll")]
    public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

    [DllImport("user32.dll")]
    public static extern bool EnableMenuItem(IntPtr hMenu, int uIDEnableItem, int uEnable);

    [DllImport("user32.dll")]
    public static extern IntPtr CreatePopupMenu();

    [DllImport("user32.dll")]
    public static extern int CheckMenuItem(IntPtr hMenu, int uIDCheckItem, int uCheck);

    // P/Invoke Methods - User32.dll (Display Settings)
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool EnumDisplaySettings(string? deviceName, int modeNum, ref DEVMODE devMode);

    // P/Invoke Methods - User32.dll (System Metrics)
    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(int nIndex);

    // P/Invoke Methods - Advapi32.dll (Credential Manager)
    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool CredRead(string targetName, CredentialType type, int reservedFlag, out IntPtr credentialPtr);

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool CredWrite(IntPtr credential, int flags);

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool CredDelete(string targetName, CredentialType type, int reservedFlag);

    [DllImport("advapi32.dll", SetLastError = true)]
    internal static extern bool CredFree(IntPtr credential);

    // Enums
    internal enum CredentialType
    {
        Generic = 1,
        DomainPassword = 2,
        DomainCertificate = 3,
        DomainVisiblePassword = 4,
        GenericCertificate = 5,
        DomainExtended = 6,
        Maximum = 7,
        MaximumEx = Maximum + 1000
    }

    // Structures
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS
    {
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public int flags;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct DEVMODE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;
        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;
        public int dmPositionX;
        public int dmPositionY;
        public int dmDisplayOrientation;
        public int dmDisplayFixedOutput;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;
        public short dmLogPixels;
        public int dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;
        public int dmDisplayFlags;
        public int dmDisplayFrequency;
        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;
        public int dmReserved2;
        public int dmPanningWidth;
        public int dmPanningHeight;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct CREDENTIAL
    {
        public int Flags;
        public int Type;
        public IntPtr TargetName;
        public IntPtr Comment;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
        public uint CredentialBlobSize;
        public IntPtr CredentialBlob;
        public int Persist;
        public int AttributeCount;
        public IntPtr Attributes;
        public IntPtr TargetAlias;
        public IntPtr UserName;
    }
}