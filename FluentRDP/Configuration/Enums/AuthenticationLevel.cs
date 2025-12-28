namespace FluentRDP.Configuration.Enums;

/// <summary>
/// Specifies the server authentication level for certificate validation
/// </summary>
public enum AuthenticationLevel
{
    /// <summary>
    /// Connect without warning if authentication fails (least secure)
    /// </summary>
    ConnectWithoutWarning = 0,

    /// <summary>
    /// Don't connect if authentication fails (most secure)
    /// </summary>
    DoNotConnect = 1,

    /// <summary>
    /// Show warning and let user decide if authentication fails
    /// </summary>
    WarnUser = 2,

    /// <summary>
    /// No authentication requirement (default)
    /// </summary>
    NoRequirement = 3
}