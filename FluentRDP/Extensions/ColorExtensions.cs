using System.ComponentModel;
using System.Drawing;

namespace FluentRDP.Extensions;

/// <summary>
/// Extensions methods for type <see cref="Color"></see>
/// </summary>
internal static class ColorExtensions
{
    /// <summary>Parse color named color or hex string.</summary>
    /// <param name="colorStr">Color string.</param>
    public static Color? ToColor(this string colorStr)
    {
        var converter = TypeDescriptor.GetConverter(typeof(Color));
        var result = (Color?)(converter.ConvertFromString(colorStr) ?? null);
        return result;
    }

    /// <summary>Converts a color to named color or hex string.</summary>
    /// <param name="color">The color to convert.</param>
    public static string ToColorString(this Color color)
    {
        if (color.IsNamedColor)
            return color.Name;

        return color.A == 255
            ? $"#{color.R:X2}{color.G:X2}{color.B:X2}"
            : $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    }
}