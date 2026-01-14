using FluentRDP.Platform;
using System.Drawing;

namespace FluentRDP.Extensions;

internal static class BitmapExtensions
{
    internal static Icon ToIcon(this Bitmap bitmap)
    {
        var bitmapIconHandle = bitmap.GetHicon();
        var tempIcon = Icon.FromHandle(bitmapIconHandle);
        var resultIcon = new Icon(tempIcon, tempIcon.Size);
        tempIcon.Dispose();
        Interop.DestroyIcon(bitmapIconHandle);
        return resultIcon;
    }
}