namespace FluentRDP.Configuration.Enums;

/// <summary>
/// Specifies where Windows key combinations (like Alt+Tab, Win+D) should be applied
/// </summary>
public enum KeyboardMode
{
    /// <summary>
    /// Apply Windows key combinations on the local computer
    /// </summary>
    OnLocalComputer = 0,

    /// <summary>
    /// Apply Windows key combinations on the remote computer
    /// </summary>
    OnRemoteComputer = 1,

    /// <summary>
    /// Apply Windows key combinations on the remote computer only when in full screen mode
    /// </summary>
    InFullScreenOnly = 2
}