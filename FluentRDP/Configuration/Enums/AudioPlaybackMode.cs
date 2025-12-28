namespace FluentRDP.Configuration.Enums;

/// <summary>
/// Determines where audio is played during a remote session
/// </summary>
public enum AudioPlaybackMode
{
    /// <summary>
    /// Play sounds on the local device
    /// </summary>
    PlayOnLocal = 0,

    /// <summary>
    /// Play sounds in the remote session
    /// </summary>
    PlayOnRemote = 1,

    /// <summary>
    /// Don't play sounds
    /// </summary>
    DoNotPlay = 2
}
