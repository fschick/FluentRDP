using FluentRDP.Extensions;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace FluentRDP.UI.Services;

internal static class BadgeColorService
{
    private const int DEFAULT_PALETTE_SIZE = 8;

    // App icon gradient: #94C11F -> #648A00
    private static readonly Color _iconGreenStart = ColorTranslator.FromHtml("#94C11F");
    private static readonly Color _iconGreenEnd = ColorTranslator.FromHtml("#648A00");
    private static readonly Color _iconGreenMid = BlendRgb(_iconGreenStart, _iconGreenEnd, 0.5f);

    public static readonly Color[] Palette = BuildDistinctBadgePalette(DEFAULT_PALETTE_SIZE);

    public static Icon GenerateBadgeIcon(this string? hostname, int x, int y, int width, int height, Color? color)
    {
        var badgeColor = color ?? GetBadgeColor(hostname);

        var originalIcon = Properties.Resources.AppIcon;
        var iconSize = originalIcon.Size;

        using var badgeIconBitmap = new Bitmap(iconSize.Width, iconSize.Height, PixelFormat.Format32bppArgb);
        using (var graphics = Graphics.FromImage(badgeIconBitmap))
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            using var originalBitmap = originalIcon.ToBitmap();
            graphics.DrawImage(originalBitmap, 0, 0, iconSize.Width, iconSize.Height);

            var defaultBadgeSize = Math.Max(18, iconSize.Width / 3);
            var badgeWidth = width > 0 ? width : defaultBadgeSize;
            var badgeHeight = height > 0 ? height : defaultBadgeSize;

            // Ensure the badge always fits inside the icon canvas with padding.
            badgeWidth = Math.Max(1, Math.Min(badgeWidth, iconSize.Width));
            badgeHeight = Math.Max(1, Math.Min(badgeHeight, iconSize.Height));

            var defaultBadgeX = iconSize.Width - badgeWidth - 2;
            const int defaultBadgeY = 2;

            var badgeX = x >= 0 ? x : defaultBadgeX;
            var badgeY = y >= 0 ? y : defaultBadgeY;

            // Keep the badge fully within the icon canvas.
            badgeX = Math.Max(0, Math.Min(badgeX, iconSize.Width - badgeWidth));
            badgeY = Math.Max(0, Math.Min(badgeY, iconSize.Height - badgeHeight));
            var cornerRadius = Math.Max(3, Math.Min(badgeWidth, badgeHeight) / 4);

            using var badgeBrush = new SolidBrush(badgeColor);
            var borderAlpha = Math.Min((int)badgeColor.A, 196);
            using var borderPen = new Pen(Color.FromArgb(borderAlpha, 255, 255, 255), 2f);
            using var badgePath = CreateRoundedRectanglePath(
                new Rectangle(badgeX, badgeY, badgeWidth, badgeHeight),
                cornerRadius
            );

            graphics.FillPath(badgeBrush, badgePath);
            graphics.DrawPath(borderPen, badgePath);
        }

        return badgeIconBitmap.ToIcon();
    }

    /// <summary>
    /// Returns a stable badge color for a hostname.
    /// </summary>
    private static Color GetBadgeColor(string? hostname)
    {
        if (string.IsNullOrWhiteSpace(hostname))
            return Palette[0];

        var hash = hostname.Trim().ToLowerInvariant().Fnv1A32();
        var idx = (int)((uint)hash % (uint)Palette.Length);
        return Palette[idx];
    }

    private static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();

        if (radius <= 0)
        {
            path.AddRectangle(rect);
            path.CloseFigure();
            return path;
        }

        var diameter = radius * 2;
        if (diameter > rect.Width) diameter = rect.Width;
        if (diameter > rect.Height) diameter = rect.Height;

        var arc = new Rectangle(rect.Location, new Size(diameter, diameter));

        // top-left
        path.AddArc(arc, 180, 90);

        // top-right
        arc.X = rect.Right - diameter;
        path.AddArc(arc, 270, 90);

        // bottom-right
        arc.Y = rect.Bottom - diameter;
        path.AddArc(arc, 0, 90);

        // bottom-left
        arc.X = rect.Left;
        path.AddArc(arc, 90, 90);

        path.CloseFigure();
        return path;
    }

    private static Color[] BuildDistinctBadgePalette(int count)
    {
        if (count < 1)
            throw new ArgumentOutOfRangeException(nameof(count), "count must be >= 1");

        // Compute icon midpoint hue to avoid generating green-ish badge colors.
        var iconHsl = RgbToHsl(_iconGreenMid);
        var excludedCenterHue = iconHsl.H; // degrees 0..360

        // Exclude a band around the icon green hue (covers yellow-green through green).
        // Wider band -> fewer green-ish candidates.
        const float excludedHalfWidth = 40f; // degrees

        // Generate candidates by golden-angle stepping for good dispersion.
        // Start opposite the icon hue so the first pick is maximally distinct.
        const float goldenAngle = 137.50776405f;
        var hue = (excludedCenterHue + 180f) % 360f;

        var palette = new Color[count];
        var picked = 0;
        var attempts = 0;

        while (picked < count && attempts < count * 200)
        {
            attempts++;

            if (IsHueInExcludedBand(hue, excludedCenterHue, excludedHalfWidth))
            {
                hue = (hue + goldenAngle) % 360f;
                continue;
            }

            // Badge styling: vivid, readable at icon sizes.
            const float s = 0.78f;
            const float l = 0.52f;

            var candidate = HslToColor(hue, s, l);

            // Subtle brand tie: a small tint toward icon green midpoint.
            // Keep it small so colors remain clearly non-green.
            candidate = BlendRgb(candidate, _iconGreenMid, 0.08f);

            palette[picked++] = candidate;
            hue = (hue + goldenAngle) % 360f;
        }

        // Defensive fallback: fill remaining with evenly spaced hues skipping the excluded band.
        if (picked < count)
        {
            var step = 360f / Math.Max(1, count * 3);
            hue = (excludedCenterHue + 180f) % 360f;
            while (picked < count)
            {
                if (!IsHueInExcludedBand(hue, excludedCenterHue, excludedHalfWidth))
                {
                    palette[picked++] = BlendRgb(HslToColor(hue, 0.78f, 0.52f), _iconGreenMid, 0.08f);
                }

                hue = (hue + step) % 360f;
            }
        }

        return palette;
    }

    private static bool IsHueInExcludedBand(float hue, float center, float halfWidth)
    {
        var d = HueDistance(hue, center);
        return d <= halfWidth;
    }

    private static float HueDistance(float a, float b)
    {
        var d = Math.Abs(a - b) % 360f;
        return d > 180f ? 360f - d : d;
    }

    private static Color BlendRgb(Color a, Color b, float t)
    {
        t = Math.Max(0f, Math.Min(1f, t));
        var r = (int)Math.Round(a.R + (b.R - a.R) * t);
        var g = (int)Math.Round(a.G + (b.G - a.G) * t);
        var bl = (int)Math.Round(a.B + (b.B - a.B) * t);
        return Color.FromArgb(255, ClampByte(r), ClampByte(g), ClampByte(bl));
    }

    private static int ClampByte(int v) => v < 0 ? 0 : (v > 255 ? 255 : v);

    private readonly record struct Hsl(float H, float S, float L);

    // H in [0,360), S/L in [0,1]
    private static Hsl RgbToHsl(Color c)
    {
        var r = c.R / 255f;
        var g = c.G / 255f;
        var b = c.B / 255f;

        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));
        var delta = max - min;

        var l = (max + min) / 2f;

        float h;
        float s;

        if (delta == 0f)
        {
            h = 0f;
            s = 0f;
        }
        else
        {
            s = delta / (1f - Math.Abs(2f * l - 1f));

            if (max == r)
                h = 60f * (((g - b) / delta) % 6f);
            else if (max == g)
                h = 60f * (((b - r) / delta) + 2f);
            else
                h = 60f * (((r - g) / delta) + 4f);

            if (h < 0f)
                h += 360f;
        }

        return new Hsl(h, s, l);
    }

    private static Color HslToColor(float h, float s, float l)
    {
        h = ((h % 360f) + 360f) % 360f;
        s = Math.Max(0f, Math.Min(1f, s));
        l = Math.Max(0f, Math.Min(1f, l));

        var c = (1f - Math.Abs(2f * l - 1f)) * s;
        var x = c * (1f - Math.Abs(((h / 60f) % 2f) - 1f));
        var m = l - c / 2f;

        float rp, gp, bp;
        if (h < 60f) { rp = c; gp = x; bp = 0f; }
        else if (h < 120f) { rp = x; gp = c; bp = 0f; }
        else if (h < 180f) { rp = 0f; gp = c; bp = x; }
        else if (h < 240f) { rp = 0f; gp = x; bp = c; }
        else if (h < 300f) { rp = x; gp = 0f; bp = c; }
        else { rp = c; gp = 0f; bp = x; }

        var r = (int)Math.Round((rp + m) * 255f);
        var g = (int)Math.Round((gp + m) * 255f);
        var b = (int)Math.Round((bp + m) * 255f);
        return Color.FromArgb(255, ClampByte(r), ClampByte(g), ClampByte(b));
    }
}

