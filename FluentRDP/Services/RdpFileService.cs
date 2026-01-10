using FluentRDP.Configuration;
using FluentRDP.Extensions;
using FluentRDP.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace FluentRDP.Services;

/// <summary>
/// Parses and exports RDP files to/from ConnectionSettings
/// </summary>
internal static class RdpFileService
{
    private static readonly List<SettingToRdpFile> _connectionToRdpFileMappings = SettingsToRdpFileService.ConnectionMapping();

    /// <summary>
    /// Parses an RDP file and returns the corresponding ConnectionSettings
    /// </summary>
    /// <param name="filePath">Path to the RDP file</param>
    /// <returns>ConnectionSettings parsed from the file</returns>
    /// <exception cref="ArgumentNullException">Thrown when filePath is null or empty</exception>
    /// <exception cref="FileNotFoundException">Thrown when the file does not exist</exception>
    public static ConnectionSettings ReadFromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath), "File path cannot be null or empty");

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"RDP file not found: {filePath}", filePath);

        // RDP files are typically encoded in UTF-16 LE (Unicode)
        var rdpFileLines = ReadRdpFileLines(filePath);

        // Parse RDP lines and map to ConnectionSettings properties
        var rdpProperties = rdpFileLines
            .Select(ToRdpProperty)
            .ToList();

        var connectionSettings = _connectionToRdpFileMappings
            .OuterJoin(
                rdpProperties,
                mapping => mapping.RdpName,
                rdp => rdp?.Name,
                (mapping, rdp) =>
                {
                    var rdpValue = rdp?.Value ?? mapping.DefaultValue;
                    var settingValue = rdpValue.ToSettingValue(mapping.Property.PropertyType);
                    return new
                    {
                        mapping.Property,
                        Value = settingValue
                    };
                },
                StringComparer.OrdinalIgnoreCase
            )
            .Where(setting => setting.Value != null)
            .ToList();

        var settings = new ConnectionSettings();
        foreach (var setting in connectionSettings)
            setting.Property.SetValue(settings, setting.Value, null);

        settings.LastConnected = File.GetLastWriteTimeUtc(filePath);
        return settings;
    }

    /// <summary>
    /// Exports ConnectionSettings to an RDP file. If the file exists, it will be updated; otherwise, a new file will be created.
    /// </summary>
    /// <param name="settings">The connection settings to export</param>
    /// <param name="filePath">Path where the RDP file should be saved</param>
    /// <exception cref="ArgumentNullException">Thrown when settings or filePath is null</exception>
    /// <exception cref="IOException">Thrown when there's an error writing the file</exception>
    public static void SaveToFile(ConnectionSettings settings, string filePath)
    {
        ArgumentNullException.ThrowIfNull(settings);

        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath), "File path cannot be null or empty");

        // Ensure directory exists
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        // Read existing RDP properties if file exists
        var existingRdpProperties = new Dictionary<string, RdpProperty>(StringComparer.OrdinalIgnoreCase);
        if (File.Exists(filePath))
        {
            var rdpFileLines = ReadRdpFileLines(filePath);
            foreach (var property in rdpFileLines.Select(ToRdpProperty).WhereNotNull())
                existingRdpProperties[property.Name] = property;
        }

        // Map ConnectionSettings to RDP properties
        var rdpPropertiesToExport = _connectionToRdpFileMappings
            .Select(mapping =>
            {
                var settingValue = mapping.Property.GetValue(settings, null);
                var rdpValue = settingValue.ToRdpValue() ?? mapping.DefaultValue ?? "";
                var rdpType = GetRdpType(mapping.Property.PropertyType);
                return new RdpProperty(mapping.RdpName, rdpType, rdpValue);
            })
            .ToList();

        // Update existing properties
        foreach (var rdpProperty in rdpPropertiesToExport)
            if (string.IsNullOrWhiteSpace(rdpProperty.Value))
                existingRdpProperties.Remove(rdpProperty.Name);
            else
                existingRdpProperties[rdpProperty.Name] = rdpProperty;

        var rdpLines = existingRdpProperties.Values
            .Select(p => $"{p.Name}:{p.Type}:{p.Value}")
            .ToArray();

        // RDP files are typically encoded in UTF-16 LE (Unicode)
        File.WriteAllLines(filePath, rdpLines, Encoding.Unicode);
    }

    private static string GetRdpType(Type propertyType)
    {
        var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (underlyingType == typeof(string))
            return RdpPropertyType.STRING;
        if (underlyingType == typeof(bool) || underlyingType == typeof(int) ||
            underlyingType == typeof(uint) || underlyingType.IsEnum)
            return RdpPropertyType.INTEGER;

        return RdpPropertyType.STRING;
    }

    private static string[] ReadRdpFileLines(string filePath)
    {
        var rdpFileLines = File.ReadAllLines(filePath, Encoding.Unicode);
        if (IsValidRdp(rdpFileLines))
            return rdpFileLines;

        rdpFileLines = File.ReadAllLines(filePath, Encoding.UTF8);
        if (IsValidRdp(rdpFileLines))
            return rdpFileLines;

        throw new InvalidDataException("RDP file appears to be empty or invalid.");
    }

    private static bool IsValidRdp(string[] rdpFileLines)
    {
        var validRdpNames = _connectionToRdpFileMappings.Select(m => m.RdpName);
        return rdpFileLines.Any(line =>
        {
            var propertyName = line.Split(':')[0].Trim();
            return validRdpNames.Any(name => name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
        });
    }

    private static RdpProperty? ToRdpProperty(string line)
    {
        var matches = line.Split([':'], 3);
        if (matches.Length < 3)
            return null;

        var propertyName = matches[0].Trim();
        var propertyType = matches[1].Trim();
        var propertyValue = matches[2].Trim();

        return new RdpProperty(propertyName, propertyType, propertyValue);
    }

    /// <summary>
    /// Represents an RDP file property with its name and type
    /// </summary>
    [DebuggerDisplay("{Name}:{Type}:{Value},nq")]
    private class RdpProperty
    {
        public string Name { get; }
        public string Type { get; }
        public string Value { get; }

        public RdpProperty(string name, string type, string value)
        {
            Name = name;
            Type = type;
            Value = value;
        }
    }

    /// <summary>
    /// RDP property types
    /// </summary>
    private static class RdpPropertyType
    {
        public const string STRING = "s";
        public const string INTEGER = "i";
        public const string BINARY = "b";
    }
}