using FluentRDP.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace FluentRDP.Services;

/// <summary>
/// Handles persistence of window settings to/from AppData
/// </summary>
internal static class WindowPersistenceService
{
    private static readonly string _settingsDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "FluentRDP"
    );

    private static readonly string _settingsFilePath = Path.Combine(
        _settingsDirectory,
        "window-settings.json"
    );

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Loads window settings from AppData JSON file for a specific hostname
    /// </summary>
    /// <param name="hostname">The hostname to load settings for. Uses empty string if null or empty.</param>
    /// <returns>WindowSettings if file exists and contains settings for the hostname, otherwise null</returns>
    public static WindowSettings? Load(string? hostname)
    {
        var key = string.IsNullOrEmpty(hostname) ? "" : hostname;

        if (!File.Exists(_settingsFilePath))
            return null;

        var json = File.ReadAllText(_settingsFilePath);
        var allSettings = JsonSerializer.Deserialize<Dictionary<string, WindowSettings>>(json, _jsonOptions);

        if (allSettings == null || !allSettings.TryGetValue(key, out var settings))
            return null;

        return settings;
    }

    /// <summary>
    /// Saves window settings to AppData JSON file for a specific hostname
    /// </summary>
    /// <param name="settings">The window settings to save</param>
    /// <param name="hostname">The hostname to save settings for. Uses empty string if null or empty.</param>
    public static void Save(this WindowSettings settings, string? hostname)
    {
        var key = string.IsNullOrEmpty(hostname) ? "" : hostname;

        if (!Directory.Exists(_settingsDirectory))
            Directory.CreateDirectory(_settingsDirectory);

        Dictionary<string, WindowSettings> allSettings;

        if (File.Exists(_settingsFilePath))
        {
            var json = File.ReadAllText(_settingsFilePath);
            allSettings = JsonSerializer.Deserialize<Dictionary<string, WindowSettings>>(json, _jsonOptions) 
                ?? new Dictionary<string, WindowSettings>();
        }
        else
        {
            allSettings = new Dictionary<string, WindowSettings>();
        }

        allSettings[key] = settings;

        var updatedJson = JsonSerializer.Serialize(allSettings, _jsonOptions);
        File.WriteAllText(_settingsFilePath, updatedJson);
    }

    public static void ApplyWindowSettings(this Form window, WindowSettings windowSettings)
    {
        var hasPosition = windowSettings.WindowX.HasValue && windowSettings.WindowY.HasValue;
        var hasSize = windowSettings.WindowWidth.HasValue || windowSettings.WindowHeight.HasValue;

        if (hasPosition || hasSize)
        {
            window.StartPosition = FormStartPosition.Manual;

            if (windowSettings.WindowX.HasValue)
                window.Left = windowSettings.WindowX.Value;

            if (windowSettings.WindowY.HasValue)
                window.Top = windowSettings.WindowY.Value;

            if (windowSettings.WindowWidth.HasValue)
                window.Width = windowSettings.WindowWidth.Value;

            if (windowSettings.WindowHeight.HasValue)
                window.Height = windowSettings.WindowHeight.Value;
        }

        if (windowSettings.Maximized == true)
            window.WindowState = FormWindowState.Maximized;
    }

    public static WindowSettings GetWindowSettings(this Form window)
    {
        var windowSettings = new WindowSettings
        {
            WindowX = window.WindowState == FormWindowState.Normal ? window.Left : window.RestoreBounds.Left,
            WindowY = window.WindowState == FormWindowState.Normal ? window.Top : window.RestoreBounds.Top,
            WindowWidth = window.WindowState == FormWindowState.Normal ? window.Width : window.RestoreBounds.Width,
            WindowHeight = window.WindowState == FormWindowState.Normal ? window.Height : window.RestoreBounds.Height,
            Maximized = window.WindowState == FormWindowState.Maximized,
        };

        return windowSettings;
    }
}
