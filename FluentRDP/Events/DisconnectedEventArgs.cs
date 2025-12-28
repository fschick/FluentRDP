using System;

namespace FluentRDP.Events;

/// <summary>
/// Provides data for the OnDisconnected event.
/// </summary>
public class DisconnectedEventArgs : EventArgs
{
    public const int NO_INFO = 0x0;
    public const int LOCAL_NOT_ERROR = 0x1;
    public const int REMOTE_BY_USER = 0x2;
    public const int BY_SERVER = 0x3;

    /// <summary>
    /// Gets the raw disconnect reason code.
    /// </summary>
    public int ErrorCode { get; }

    /// <summary>
    /// Human-readable description of the disconnect reason
    /// </summary>
    public string? ErrorDescription { get; }

    /// <summary>
    /// <c>true</c> if initiated by a reconnect; otherwise, <c>false</c>.
    /// </summary>
    public bool IsReconnect { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisconnectedEventArgs"/> class.
    /// </summary>
    /// <param name="reasonCode">The disconnect reason code.</param>
    /// <param name="errorDescription">Human-readable description of the disconnect reason</param>
    /// <param name="isReconnect"><c>true</c> if initiated by a reconnect; otherwise, <c>false</c>.</param>
    public DisconnectedEventArgs(int reasonCode, string? errorDescription, bool isReconnect)
    {
        ErrorCode = reasonCode;
        ErrorDescription = errorDescription;
        IsReconnect = isReconnect;
    }

    /// <summary>
    /// Gets whether the disconnection was intentional (not an error).
    /// </summary>
    public bool IsIntentionalDisconnect =>
        ErrorCode == LOCAL_NOT_ERROR ||
        ErrorCode == REMOTE_BY_USER ||
        ErrorCode == BY_SERVER;
}