using FluentRDP.Events;
using FluentRDP.Extensions;
using FluentRDP.Platform;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FluentRDP.UI.Services;

/// <summary>
/// Service to manage system menu items and handle system menu commands using NativeWindow.
/// Monitors WM_SYSCOMMAND messages and raises events for menu actions.
/// </summary>
internal sealed class FormSystemMenuService : NativeWindow, IDisposable
{
    private static readonly Dictionary<uint, int> _zoomLevels = new()
    {
        { 100,  Interop.SC_ZOOM_100 },
        { 125,  Interop.SC_ZOOM_125 },
        { 150,  Interop.SC_ZOOM_150 },
        { 175,  Interop.SC_ZOOM_175 },
        { 200,  Interop.SC_ZOOM_200 },
        { 250,  Interop.SC_ZOOM_250 },
        { 300,  Interop.SC_ZOOM_300 },
        { 400,  Interop.SC_ZOOM_400 },
        { 500,  Interop.SC_ZOOM_500 }
    };

    private Form? _parentForm;
    private bool _disposed;
    private IntPtr _zoomSubMenu;

    /// <summary>
    /// Event raised when Connect menu item is clicked
    /// </summary>
    public event EventHandler? ConnectRequested;

    /// <summary>
    /// Event raised when Disconnect menu item is clicked
    /// </summary>
    public event EventHandler? DisconnectRequested;

    /// <summary>
    /// Event raised when Full Screen menu item is clicked
    /// </summary>
    public event EventHandler? FullScreenRequested;

    /// <summary>
    /// Event raised when Settings menu item is clicked
    /// </summary>
    public event EventHandler? SettingsRequested;

    /// <summary>
    /// Event raised when a zoom level menu item is clicked
    /// </summary>
    public event EventHandler<ZoomLevelEventArgs>? ZoomLevelRequested;

    /// <summary>
    /// Attaches this service to a window handle and initializes the system menu
    /// </summary>
    /// <param name="form">The window to attach to</param>
    public void AttachToWindow(Form form)
    {
        if (form.Handle == IntPtr.Zero)
            throw new ArgumentException("Invalid window handle", nameof(form));

        _parentForm = form ?? throw new ArgumentNullException(nameof(form));
        AssignHandle(form.Handle);
        InitializeSystemMenu();
    }

    /// <summary>
    /// Enables or disables a system menu item
    /// </summary>
    /// <param name="menuItemId">The menu item ID to enable/disable</param>
    /// <param name="enabled">True to enable, false to disable</param>
    public void EnableMenuItem(int menuItemId, bool enabled)
    {
        var systemMenu = Interop.GetSystemMenu(Handle, false);
        if (systemMenu == IntPtr.Zero)
            return;

        var flags = enabled
            ? Interop.MF_BYCOMMAND | Interop.MF_ENABLED
            : Interop.MF_BYCOMMAND | Interop.MF_GRAYED;

        Interop.EnableMenuItem(systemMenu, menuItemId, flags);
    }

    /// <summary>
    /// Updates the checkmarks on zoom level menu items based on the current zoom level
    /// </summary>
    /// <param name="zoom">The current zoom level (100, 125, 150, etc.)</param>
    public void UpdateZoomCheckmarks(uint? zoom)
    {
        if (_zoomSubMenu == IntPtr.Zero)
            return;

        // Clear all checkmarks first
        foreach (var (_, menuId) in _zoomLevels)
            Interop.CheckMenuItem(_zoomSubMenu, menuId, Interop.MF_BYCOMMAND | Interop.MF_UNCHECKED);

        // Set checkmark for current zoom level
        zoom ??= _parentForm?.DeviceZoom;
        if (!zoom.HasValue)
            return;

        var menuIdToCheck = _zoomLevels[zoom.Value];
        Interop.CheckMenuItem(_zoomSubMenu, menuIdToCheck, Interop.MF_BYCOMMAND | Interop.MF_CHECKED);
    }

    private void InitializeSystemMenu()
    {
        var systemMenu = Interop.GetSystemMenu(Handle, false);

        if (systemMenu == IntPtr.Zero)
            return;

        // Add separator before custom items
        Interop.AppendMenu(systemMenu, Interop.MF_SEPARATOR, 0, string.Empty);

        // Add connection menu items
        Interop.AppendMenu(systemMenu, Interop.MF_STRING, Interop.SC_CONNECT, "&Connect");
        Interop.AppendMenu(systemMenu, Interop.MF_STRING, Interop.SC_DISCONNECT, "&Disconnect");
        Interop.AppendMenu(systemMenu, Interop.MF_STRING, Interop.SC_FULLSCREEN, "&Full Screen");

        // Add separator before zoom
        Interop.AppendMenu(systemMenu, Interop.MF_SEPARATOR, 0, string.Empty);

        // Create Zoom submenu
        _zoomSubMenu = Interop.CreatePopupMenu();
        foreach (var (factor, menuId) in _zoomLevels)
            Interop.AppendMenu(_zoomSubMenu, Interop.MF_STRING, menuId, $"{factor}%");

        // Add Zoom submenu to system menu
        Interop.AppendMenu(systemMenu, Interop.MF_POPUP, (int)_zoomSubMenu, "&Zoom");

        // Add separator before settings
        Interop.AppendMenu(systemMenu, Interop.MF_SEPARATOR, 0, string.Empty);

        // Add "Settings" menu item
        Interop.AppendMenu(systemMenu, Interop.MF_STRING, Interop.SC_SETTINGS, "&Settings");

        // Initially disable full screen menu item
        EnableMenuItem(Interop.SC_FULLSCREEN, false);
    }

    /// <summary>
    /// Intercepts window messages to handle system menu commands
    /// </summary>
    protected override void WndProc(ref Message m)
    {
        if (m.Msg == Interop.WM_SYSCOMMAND)
        {
            var command = m.WParam.ToInt32();
            switch (command)
            {
                case Interop.SC_CONNECT:
                    ConnectRequested?.Invoke(this, EventArgs.Empty);
                    return;

                case Interop.SC_DISCONNECT:
                    DisconnectRequested?.Invoke(this, EventArgs.Empty);
                    return;

                case Interop.SC_FULLSCREEN:
                    FullScreenRequested?.Invoke(this, EventArgs.Empty);
                    return;

                case Interop.SC_SETTINGS:
                    SettingsRequested?.Invoke(this, EventArgs.Empty);
                    return;

                case Interop.SC_ZOOM_100:
                    ZoomLevelRequested?.Invoke(this, new ZoomLevelEventArgs(100));
                    return;

                case Interop.SC_ZOOM_125:
                    ZoomLevelRequested?.Invoke(this, new ZoomLevelEventArgs(125));
                    return;

                case Interop.SC_ZOOM_150:
                    ZoomLevelRequested?.Invoke(this, new ZoomLevelEventArgs(150));
                    return;

                case Interop.SC_ZOOM_175:
                    ZoomLevelRequested?.Invoke(this, new ZoomLevelEventArgs(175));
                    return;

                case Interop.SC_ZOOM_200:
                    ZoomLevelRequested?.Invoke(this, new ZoomLevelEventArgs(200));
                    return;

                case Interop.SC_ZOOM_250:
                    ZoomLevelRequested?.Invoke(this, new ZoomLevelEventArgs(250));
                    return;

                case Interop.SC_ZOOM_300:
                    ZoomLevelRequested?.Invoke(this, new ZoomLevelEventArgs(300));
                    return;

                case Interop.SC_ZOOM_400:
                    ZoomLevelRequested?.Invoke(this, new ZoomLevelEventArgs(400));
                    return;

                case Interop.SC_ZOOM_500:
                    ZoomLevelRequested?.Invoke(this, new ZoomLevelEventArgs(500));
                    return;
            }
        }

        base.WndProc(ref m);
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        if (Handle != IntPtr.Zero)
            ReleaseHandle();

        _disposed = true;
    }
}

