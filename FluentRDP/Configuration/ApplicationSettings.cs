using System.Collections.Generic;

namespace FluentRDP.Configuration;

/// <summary>
/// Represents application specific settings
/// </summary>
internal class ApplicationSettings
{
    /// <summary>
    /// RDP connection specific settings
    /// </summary>
    public ConnectionSettings Connection { get; set; } = new();

    /// <summary>
    /// Window positioning and state settings for the application window
    /// </summary>
    public WindowSettings Window { get; set; } = new();

    /// <summary>
    /// Gets or sets whether to disable automatic connection when hostname is provided
    /// </summary>
    public bool? NoAutoConnect { get; set; }

    /// <summary>
    /// Do not close the application window when RDP disconnects
    /// </summary>
    public bool? NoCloseOnDisconnect { get; set; }

    /// <summary>
    /// Gets or sets the path to an RDP file to load settings from
    /// </summary>
    public string? RdpFilePath { get; set; }

    /// <summary>
    /// Opens the specified .RDP connection file for editing
    /// </summary>
    public bool? EditRdpFile { get; set; }

    /// <summary>
    /// Indicates whether all validation checks pass
    /// </summary>
    public bool IsValid() => Validate().Count == 0;

    /// <summary>
    /// Validates all application settings including nested settings
    /// </summary>
    /// <returns>A list of validation error messages. Empty if validation succeeds.</returns>
    public IReadOnlyList<string> Validate()
    {
        var errors = new List<string>();

        var connectionErrors = Connection.Validate();
        errors.AddRange(connectionErrors);

        var windowErrors = Window.Validate();
        errors.AddRange(windowErrors);

        return errors;
    }
}