using System.Windows.Forms;

namespace FluentRDP.Extensions;

/// <summary>
/// Extensions methods for type <see cref="Control"></see>
/// </summary>
public static class ControlExtensions
{
    extension(Control control)
    {
        public uint DeviceZoom => (uint)(control.DeviceDpi / 96f * 100);
    }
}