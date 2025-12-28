namespace FluentRDP.Events;

public class FullScreenEventArgs : System.EventArgs
{
    public FullScreenEventArgs(bool isFullScreen, bool isMultiMonitor)
    {
        IsFullScreen = isFullScreen;
        IsMultiMonitor = isMultiMonitor;
    }

    public bool IsFullScreen { get; set; }
    public bool IsMultiMonitor { get; set; }
}