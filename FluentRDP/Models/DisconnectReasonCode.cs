namespace FluentRDP.Models;

/// <summary>
/// Specifies the reason for the disconnection. Some of these error codes are defined in Wincred.h.
/// </summary>
public enum DisconnectReasonCode
{
    /// <summary>
    /// No information is available.
    /// </summary>
    NoInfo = 0x0,

    /// <summary>
    /// Local disconnection. This is not an error code.
    /// </summary>
    LocalNotError = 0x1,

    /// <summary>
    /// Remote disconnection by user. This is not an error code.
    /// </summary>
    RemoteByUser = 0x2,

    /// <summary>
    /// Remote disconnection by server. This is not an error code.
    /// </summary>
    ByServer = 0x3,

    /// <summary>
    /// DNS name lookup failure.
    /// </summary>
    DnsLookupFailed = 0x104,

    /// <summary>
    /// Connection timed out.
    /// </summary>
    ConnectionTimedOut = 0x108,

    /// <summary>
    /// Out of memory.
    /// </summary>
    OutOfMemory = 0x106,

    /// <summary>
    /// Host not found error.
    /// </summary>
    HostNotFound = 0x208,

    /// <summary>
    /// Out of memory.
    /// </summary>
    OutOfMemory2 = 0x206,

    /// <summary>
    /// Windows Sockets connect failed.
    /// </summary>
    SocketConnectFailed = 0x204,

    /// <summary>
    /// The IP address specified is not valid.
    /// </summary>
    InvalidIpAddr = 0x308,

    /// <summary>
    /// Out of memory.
    /// </summary>
    OutOfMemory3 = 0x306,

    /// <summary>
    /// Windows Sockets send call failed.
    /// </summary>
    WinsockSendFailed = 0x304,

    /// <summary>
    /// Security data is not valid.
    /// </summary>
    InvalidSecurityData = 0x406,

    /// <summary>
    /// Internal error.
    /// </summary>
    InternalError = 0x408,

    /// <summary>
    /// Windows Sockets recv call failed.
    /// </summary>
    SocketReceiveFailed = 0x404,

    /// <summary>
    /// The encryption method specified is not valid.
    /// </summary>
    InvalidEncryption = 0x506,

    /// <summary>
    /// DNS lookup failed.
    /// </summary>
    DnsLookupFailed2 = 0x508,

    /// <summary>
    /// Windows Sockets gethostbyname call failed.
    /// </summary>
    GetHostByNameFailed = 0x604,

    /// <summary>
    /// Server security data is not valid.
    /// </summary>
    InvalidServerSecurityInfo = 0x606,

    /// <summary>
    /// Internal timer error.
    /// </summary>
    TimerError = 0x608,

    /// <summary>
    /// Time-out occurred.
    /// </summary>
    TimeoutOccurred = 0x704,

    /// <summary>
    /// Failed to unpack server certificate.
    /// </summary>
    ServerCertificateUnpackErr = 0x706,

    /// <summary>
    /// Bad IP address specified.
    /// </summary>
    InvalidIp = 0x804,

    /// <summary>
    /// License negotiation failed.
    /// </summary>
    LicensingFailed = 0x808,

    /// <summary>
    /// Login failed.
    /// </summary>
    SslErrLogonFailure = 0x807,

    /// <summary>
    /// Socket closed.
    /// </summary>
    AtClientWinsockFdClose = 0x904,

    /// <summary>
    /// Internal security error.
    /// </summary>
    InternalSecurityError = 0x906,

    /// <summary>
    /// Licensing time-out.
    /// </summary>
    LicensingTimeout = 0x908,

    /// <summary>
    /// Encryption error.
    /// </summary>
    EncryptionError = 0xB06,

    /// <summary>
    /// The account is disabled.
    /// </summary>
    SslErrAccountDisabled = 0xB07,

    /// <summary>
    /// Decompression error.
    /// </summary>
    ClientDecompressionError = 0xC08,

    /// <summary>
    /// Decryption error.
    /// </summary>
    DecryptionError = 0xC06,

    /// <summary>
    /// The account is restricted.
    /// </summary>
    SslErrAccountRestriction = 0xC07,

    /// <summary>
    /// Internal security error.
    /// </summary>
    InternalSecurityError2 = 0xA06,

    /// <summary>
    /// The specified user has no account.
    /// </summary>
    SslErrNoSuchUser = 0xA07,

    /// <summary>
    /// The account is locked out.
    /// </summary>
    SslErrAccountLockedOut = 0xD07,

    /// <summary>
    /// The account is expired.
    /// </summary>
    SslErrAccountExpired = 0xE07,

    /// <summary>
    /// The password is expired.
    /// </summary>
    SslErrPasswordExpired = 0xF07,

    /// <summary>
    /// The user password must be changed before logging on for the first time.
    /// </summary>
    SslErrPasswordMustChange = 0x1207,

    /// <summary>
    /// The policy does not support delegation of credentials to the target server.
    /// </summary>
    SslErrDelegationPolicy = 0x1607,

    /// <summary>
    /// Delegation of credentials to the target server is not allowed unless mutual authentication has been achieved.
    /// </summary>
    SslErrPolicyNtlmOnly = 0x1707,

    /// <summary>
    /// No authority could be contacted for authentication. The domain name of the authenticating party could be wrong, the domain could be unreachable, or there might have been a trust relationship failure.
    /// </summary>
    SslErrNoAuthenticatingAuthority = 0x1807,

    /// <summary>
    /// The received certificate is expired.
    /// </summary>
    SslErrCertExpired = 0x1B07,

    /// <summary>
    /// An incorrect PIN was presented to the smart card.
    /// </summary>
    SslErrSmartcardWrongPin = 0x1C07,

    /// <summary>
    /// The server authentication policy does not allow connection requests using saved credentials. The user must enter new credentials.
    /// </summary>
    SslErrFreshCredRequiredByServer = 0x2107,

    /// <summary>
    /// The smart card is blocked.
    /// </summary>
    SslErrSmartcardCardBlocked = 0x2207
}
