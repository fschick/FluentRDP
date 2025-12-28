namespace FluentRDP.Events;

/// <summary>
/// Provides data for zoom level change events.
/// </summary>
public class ZoomLevelEventArgs : System.EventArgs
{
    /// <summary>
    /// Gets the zoom level percentage (100, 125, 150, etc.)
    /// </summary>
    public uint ZoomLevel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ZoomLevelEventArgs"/> class.
    /// </summary>
    /// <param name="zoomLevel">The zoom level percentage</param>
    public ZoomLevelEventArgs(uint zoomLevel)
    {
        ZoomLevel = zoomLevel;
    }
}

