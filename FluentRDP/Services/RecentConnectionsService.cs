using FluentRDP.Configuration;
using FluentRDP.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FluentRDP.Services;

/// <summary>
/// Handles persistence of recent connection settings to/from AppData
/// </summary>
internal static class RecentConnectionsService
{
    private static readonly string _recentConnectionsDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "FluentRDP",
        "Recent"
    );

    /// <summary>
    /// Loads all connections from recent connections folder.
    /// </summary>
    /// <returns>A dictionary with hostname as key and ConnectionSettings as value. Returns empty dictionary if folder doesn't exist or contains no valid RDP files.</returns>
    public static List<ConnectionSettings> LoadAll()
    {
        if (!Directory.Exists(_recentConnectionsDirectory))
            return [];

        var rdpFiles = Directory.GetFiles(_recentConnectionsDirectory, "*.rdp", SearchOption.TopDirectoryOnly);

        var connections = rdpFiles
            .Select(RdpFileService.ReadFromFile)
            .Where(setting => setting.Hostname.IsValidHostname())
            .DistinctBy(setting => setting.Hostname, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return connections;
    }

    /// <summary>
    /// Adds or updates a connection to recent connections.
    /// If a connection with the same hostname exists, it will be updated.
    /// </summary>
    /// <param name="settings">The connection settings to save</param>
    /// <exception cref="ArgumentNullException">Thrown when settings is null</exception>
    /// <exception cref="ArgumentException">Thrown when hostname is null or empty</exception>
    /// <exception cref="IOException">Thrown when there's an error writing the file</exception>
    public static void AddOrUpdate(ConnectionSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        if (string.IsNullOrWhiteSpace(settings.Hostname))
            throw new ArgumentException("Hostname cannot be null or empty.", nameof(settings));

        settings.LastConnected = DateTime.Now;
        var filePath = GetRdpFilePath(settings.Hostname);
        RdpFileService.SaveToFile(settings, filePath);
    }

    /// <summary>
    /// Removes a connection from recent connections.
    /// </summary>
    /// <param name="hostname">The hostname of the connection to remove</param>
    /// <exception cref="ArgumentException">Thrown when hostname is null or empty</exception>
    /// <returns>True if the file was deleted, false if it didn't exist</returns>
    public static bool Remove(string? hostname)
    {
        if (string.IsNullOrWhiteSpace(hostname))
            throw new ArgumentException("Hostname cannot be null or empty.", nameof(hostname));

        var filePath = GetRdpFilePath(hostname);

        if (!File.Exists(filePath))
            return false;

        File.Delete(filePath);
        return true;
    }

    /// <summary>
    /// Gets the file path for a connection based on its hostname.
    /// </summary>
    /// <param name="hostname">The hostname to get the file path for</param>
    /// <returns>The full path to the RDP file</returns>
    private static string GetRdpFilePath(string hostname)
    {
        var sanitizedHostname = SanitizeHostnameForFilename(hostname);
        var fileName = $"{sanitizedHostname}.rdp";
        return Path.Combine(_recentConnectionsDirectory, fileName);
    }

    /// <summary>
    /// Sanitizes a hostname to be safe for use as a filename.
    /// Replaces invalid filename characters with underscores.
    /// </summary>
    /// <param name="hostname">The hostname to sanitize</param>
    /// <returns>A sanitized hostname safe for use as a filename</returns>
    private static string SanitizeHostnameForFilename(string hostname)
    {
        var sanitizedPath = Path.GetInvalidFileNameChars()
            .Concat([':', '[', ']']) // Port separator, IPv6 brackets
            .Aggregate(
                hostname,
                (current, invalidChar) => current.Replace(invalidChar, '_')
            );

        return sanitizedPath;
    }
}