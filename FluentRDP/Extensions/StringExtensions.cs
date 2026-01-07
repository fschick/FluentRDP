using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace FluentRDP.Extensions;

internal static partial class StringExtensions
{
    public static bool IsValidHostname(this string? ipaddressOrHostname)
    {
        if (string.IsNullOrWhiteSpace(ipaddressOrHostname))
            return false;

        // Normalize whitespace
        ipaddressOrHostname = ipaddressOrHostname.Trim();

        // Length limit for a FQDN in ASCII (after IDN conversion)
        // Max 253 characters for the whole name, 63 per label
        var exceed253CharLimit = ipaddressOrHostname.Length > 253;
        if (exceed253CharLimit)
            return false;

        var isValidIpaddress = IPAddress.TryParse(ipaddressOrHostname, out _);
        if (isValidIpaddress)
            return true;

        string asciiHost;
        try { asciiHost = ToAscii(ipaddressOrHostname); }
        catch (ArgumentException) { return false; }

        // Now validate the ASCII (LDH / punycode) form
        exceed253CharLimit = asciiHost.Length > 253;
        if (exceed253CharLimit)
            return false;

        // Remove optional trailing dot for label checks
        var hostNoDot = asciiHost.TrimEnd('.');

        // Regex for each label: 1–63 chars, alnum or hyphen, no leading/trailing hyphen
        var labelRegex = ValidHostnameRegex();

        var asciiLabels = hostNoDot.Split('.');
        foreach (var label in asciiLabels)
        {
            if (label.Length is 0 or > 63)
                return false;

            if (!labelRegex.IsMatch(label))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Converts a Unicode hostname to its ASCII representation using IDNA.
    /// </summary>
    /// <param name="ipaddressOrHostname">The hostname to convert.</param>
    /// <returns>The ASCII representation (with punycode) of the hostname.</returns>
    private static string ToAscii(string ipaddressOrHostname)
    {
        var idn = new IdnMapping();
        // Process each label separately to keep any trailing dot correct
        var labels = ipaddressOrHostname.TrimEnd('.').Split('.');
        for (var i = 0; i < labels.Length; i++)
            labels[i] = idn.GetAscii(labels[i]);

        var asciiHost = string.Join(".", labels);

        // Preserve a trailing dot if present (rooted FQDN)
        if (ipaddressOrHostname.EndsWith('.'))
            asciiHost += ".";

        return asciiHost;
    }

    /// <summary>
    /// Regex for each label: 1–63 chars, alphanumeric or hyphen, no leading/trailing hyphen
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"^[A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])?$", RegexOptions.Compiled)]
    private static partial Regex ValidHostnameRegex();
}