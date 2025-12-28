using FluentRDP.Platform;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FluentRDP.UI.Services;

/// <summary>
/// Service to enforce even width and height dimensions for RDP windows using NativeWindow.
/// Monitors and adjusts WM_SIZING and WM_WINDOWPOSCHANGING messages to ensure even dimensions.
/// </summary>
internal sealed class FormSizeEnforcementService : NativeWindow, IDisposable
{
    private bool _disposed;
    private Form? _parentForm;

    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Attaches this service to a window
    /// </summary>
    /// <param name="form">The window to monitor</param>
    [MemberNotNull(nameof(_parentForm))]
    public void AttachToWindow(Form form)
    {
        _parentForm = form ?? throw new ArgumentNullException(nameof(form));
        AssignHandle(form.Handle);
    }

    /// <summary>
    /// Intercepts window messages to enforce even dimensions
    /// </summary>
    protected override void WndProc(ref Message m)
    {
        if (!IsActive)
        {
            base.WndProc(ref m);
            return;
        }

        switch (m.Msg)
        {
            case Interop.WM_SIZING: // Handle interactive resizing (dragging borders)
                var rc = Marshal.PtrToStructure<Interop.RECT>(m.LParam);

                var width = rc.Right - rc.Left;
                var height = rc.Bottom - rc.Top;
                var wParam = m.WParam.ToInt32();

                // Round to even numbers
                if (width % 2 != 0)
                {
                    if (wParam == Interop.WMSZ_LEFT || wParam == Interop.WMSZ_TOPLEFT || wParam == Interop.WMSZ_BOTTOMLEFT) // Left edge
                        rc.Left--;
                    else // Right edge
                        rc.Right++;
                }

                if (height % 2 != 0)
                {
                    if (wParam == Interop.WMSZ_TOP || wParam == Interop.WMSZ_TOPRIGHT || wParam == Interop.WMSZ_TOPLEFT) // Top edge
                        rc.Top--;
                    else // Bottom edge
                        rc.Bottom++;
                }

                Marshal.StructureToPtr(rc, m.LParam, true);
                break;
            case Interop.WM_WINDOWPOSCHANGING: // Handle programmatic resizing and window state changes
                var windowPos = Marshal.PtrToStructure<Interop.WINDOWPOS>(m.LParam);
                if ((windowPos.flags & Interop.SWP_NOSIZE) != 0)
                    break;

                if (windowPos.cx % 2 != 0)
                    windowPos.cx++;
                if (windowPos.cy % 2 != 0)
                    windowPos.cy++;

                Marshal.StructureToPtr(windowPos, m.LParam, true);
                break;
            case Interop.WM_WINDOWPOSCHANGED: // Catch any odd dimensions that slipped through
                base.WndProc(ref m);

                if (_parentForm == null)
                    return;

                // Skip adjustment if maximized or minimized - Windows manages the size
                if (_parentForm.WindowState != FormWindowState.Normal)
                    return;

                // Skip if dimensions are already even
                if (_parentForm.Width % 2 == 0 && _parentForm.Height % 2 == 0)
                    return;

                var newWidth = _parentForm.Width % 2 != 0 ? _parentForm.Width + 1 : _parentForm.Width;
                var newHeight = _parentForm.Height % 2 != 0 ? _parentForm.Height + 1 : _parentForm.Height;

                // Use BeginInvoke to defer the resize until after message processing completes
                _parentForm.BeginInvoke(new Action(() =>
                {
                    if (_parentForm.WindowState != FormWindowState.Normal || (_parentForm.Width % 2 == 0 && _parentForm.Height % 2 == 0))
                        return;

                    _parentForm.Width = newWidth;
                    _parentForm.Height = newHeight;
                }));

                return;
        }

        base.WndProc(ref m);
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        if (Handle != IntPtr.Zero)
            ReleaseHandle();

        _parentForm = null;
        _disposed = true;
    }
}