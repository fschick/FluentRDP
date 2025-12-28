using FluentRDP.Platform;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FluentRDP.UI.Services;

/// <summary>
/// Service to detect window resize operations using NativeWindow.
/// Monitors WM_ENTERSIZEMOVE, WM_EXITSIZEMOVE, and WM_WINDOWPOSCHANGED messages.
/// </summary>
internal sealed class FormResizeDetectionService : NativeWindow, IDisposable
{
    private bool _resizeInProgress;
    private bool _disposed;

    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Event raised when window size changes
    /// </summary>
    public event EventHandler? ResizeDetected;

    /// <summary>
    /// Attaches this service to a window handle
    /// </summary>
    /// <param name="handle">The window handle to monitor</param>
    public void AttachToWindow(IntPtr handle)
    {
        if (handle == IntPtr.Zero)
            throw new ArgumentException("Invalid window handle", nameof(handle));

        AssignHandle(handle);
    }

    /// <summary>
    /// Manually triggers a resize event
    /// </summary>
    public void Resize()
    {
        if (Enabled)
            ResizeDetected?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Intercepts window messages to detect resize operations
    /// </summary>
    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        switch (m.Msg)
        {
            case Interop.WM_ENTERSIZEMOVE:
                _resizeInProgress = true;
                break;

            case Interop.WM_EXITSIZEMOVE:
                _resizeInProgress = false;
                if (Enabled)
                    ResizeDetected?.Invoke(this, EventArgs.Empty);
                break;

            case Interop.WM_WINDOWPOSCHANGED:
                var windowPos = Marshal.PtrToStructure<Interop.WINDOWPOS>(m.LParam);
                var sizeChanged = (windowPos.flags & Interop.SWP_NOSIZE) == 0;
                if (sizeChanged && !_resizeInProgress && Enabled)
                    ResizeDetected?.Invoke(this, EventArgs.Empty);
                break;
        }
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