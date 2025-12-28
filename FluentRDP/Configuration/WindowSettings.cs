using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FluentRDP.Configuration;

/// <summary>
/// Window positioning and state settings for the application window
/// </summary>
internal class WindowSettings
{
    /// <summary>
    /// Gets or sets the window X position
    /// </summary>
    public int? WindowX { get; set; }

    /// <summary>
    /// Gets or sets the window Y position
    /// </summary>
    public int? WindowY { get; set; }

    /// <summary>
    /// Gets or sets the window width
    /// </summary>
    public int? WindowWidth { get; set; }

    /// <summary>
    /// Gets or sets the window height
    /// </summary>
    public int? WindowHeight { get; set; }

    /// <summary>
    /// Start the window maximized
    /// </summary>
    public bool? Maximized { get; set; }

    /// <summary>
    /// Do not close the application window when RDP disconnects
    /// </summary>
    [JsonIgnore]
    public bool? NoCloseOnDisconnect { get; set; }

    /// <summary>
    /// Indicates whether all validation checks pass
    /// </summary>
    public bool IsValid() => Validate().Count == 0;

    /// <summary>
    /// Validates the window settings
    /// </summary>
    /// <returns>A list of validation error messages. Empty if validation succeeds.</returns>
    public IReadOnlyList<string> Validate()
    {
        var errors = new List<string>();

        if (WindowWidth is <= 0)
            errors.Add("WindowWidth must be greater than 0.");

        if (WindowHeight is <= 0)
            errors.Add("WindowHeight must be greater than 0.");

        return errors;
    }
}