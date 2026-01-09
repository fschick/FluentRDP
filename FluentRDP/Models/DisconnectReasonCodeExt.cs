namespace FluentRDP.Models;

/// <summary>
/// Defines extended information about the control's reason for disconnection.
/// </summary>
/// <remarks>
/// Reference: <see href="https://learn.microsoft.com/en-us/windows/win32/termserv/extendeddisconnectreasoncode"/>
/// </remarks>
public enum DisconnectReasonCodeExt
{
    /// <summary>
    /// No additional information is available.
    /// </summary>
    NoInfo = 0,

    /// <summary>
    /// An application initiated the disconnection.
    /// </summary>
    ApiInitiatedDisconnect = 1,

    /// <summary>
    /// An application logged off the client.
    /// </summary>
    ApiInitiatedLogoff = 2,

    /// <summary>
    /// The server has disconnected the client because the client has been idle for a period of time longer than the designated time-out period.
    /// </summary>
    ServerIdleTimeout = 3,

    /// <summary>
    /// The server has disconnected the client because the client has exceeded the period designated for connection.
    /// </summary>
    ServerLogonTimeout = 4,

    /// <summary>
    /// The client's connection was replaced by another connection.
    /// </summary>
    ReplacedByOtherConnection = 5,

    /// <summary>
    /// No memory is available.
    /// </summary>
    OutOfMemory = 6,

    /// <summary>
    /// The server denied the connection.
    /// </summary>
    ServerDeniedConnection = 7,

    /// <summary>
    /// The server denied the connection for security reasons.
    /// </summary>
    ServerDeniedConnectionFips = 8,

    /// <summary>
    /// The server denied the connection for security reasons.
    /// </summary>
    ServerInsufficientPrivileges = 9,

    /// <summary>
    /// Fresh credentials are required.
    /// </summary>
    ServerFreshCredentialsRequired = 10,

    /// <summary>
    /// User activity has initiated the disconnect.
    /// </summary>
    RpcInitiatedDisconnectByUser = 11,

    /// <summary>
    /// The user logged off, disconnecting the session.
    /// </summary>
    LogoffByUser = 12,

    /// <summary>
    /// Internal licensing error.
    /// </summary>
    LicenseInternal = 256,

    /// <summary>
    /// No license server was available.
    /// </summary>
    LicenseNoLicenseServer = 257,

    /// <summary>
    /// No valid software license was available.
    /// </summary>
    LicenseNoLicense = 258,

    /// <summary>
    /// The remote computer received a licensing message that was not valid.
    /// </summary>
    LicenseErrClientMsg = 259,

    /// <summary>
    /// The hardware ID does not match the one designated on the software license.
    /// </summary>
    LicenseHardwareIdDoesNotMatchLicense = 260,

    /// <summary>
    /// Client license error.
    /// </summary>
    LicenseErrClientLicense = 261,

    /// <summary>
    /// Network problems occurred during the licensing protocol.
    /// </summary>
    LicenseCantFinishProtocol = 262,

    /// <summary>
    /// The client ended the licensing protocol prematurely.
    /// </summary>
    LicenseClientEndedProtocol = 263,

    /// <summary>
    /// A licensing message was encrypted incorrectly.
    /// </summary>
    LicenseErrClientEncryption = 264,

    /// <summary>
    /// The local computer's client access license could not be upgraded or renewed.
    /// </summary>
    LicenseCantUpgradeLicense = 265,

    /// <summary>
    /// The remote computer is not licensed to accept remote connections.
    /// </summary>
    LicenseNoRemoteConnections = 266,

    /// <summary>
    /// An access denied error was received while creating a registry key for the license store.
    /// </summary>
    LicenseCreatingLicStoreAccDenied = 267,

    /// <summary>
    /// Invalid credentials were encountered.
    /// </summary>
    RdpEncInvalidCredentials = 768,

    /// <summary>
    /// Beginning the range of internal protocol errors. Check the server event log for additional details.
    /// </summary>
    ProtocolRangeStart = 4096,

    /// <summary>
    /// Ending the range of internal protocol errors.
    /// </summary>
    ProtocolRangeEnd = 32767
}