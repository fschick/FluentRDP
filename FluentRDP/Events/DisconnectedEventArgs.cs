using FluentRDP.Models;
using System;

namespace FluentRDP.Events;

/// <summary>
/// Provides data for the OnDisconnected event.
/// </summary>
public class DisconnectedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the raw disconnect reason code.
    /// </summary>
    public DisconnectReasonCode ReasonCode { get; }

    /// <summary>
    /// Gets the raw extended disconnect reason code.
    /// </summary>
    public DisconnectReasonCodeExt ExtendedReasonCode { get; }

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
    /// <param name="errorCode">The disconnect reason code.</param>
    /// <param name="extendedErrorCode">The extended disconnect reason code.</param>
    /// <param name="errorDescription">Human-readable description of the disconnect reason</param>
    /// <param name="isReconnect"><c>true</c> if initiated by a reconnect; otherwise, <c>false</c>.</param>
    public DisconnectedEventArgs(DisconnectReasonCode errorCode, DisconnectReasonCodeExt extendedErrorCode, string? errorDescription, bool isReconnect)
    {
        ReasonCode = errorCode;
        ExtendedReasonCode = extendedErrorCode;
        ErrorDescription = errorDescription;
        IsReconnect = isReconnect;
    }

    /// <summary>
    /// Gets whether the disconnection was intentional (not an error).
    /// </summary>
    public bool IsIntentionalDisconnect =>
        ReasonCode
            is DisconnectReasonCode.LocalNotError
            or DisconnectReasonCode.RemoteByUser
            or DisconnectReasonCode.ByServer
        &&
        ExtendedReasonCode
            is DisconnectReasonCodeExt.NoInfo
            or DisconnectReasonCodeExt.LogoffByUser
            or DisconnectReasonCodeExt.RpcInitiatedDisconnectByUser
            or DisconnectReasonCodeExt.ApiInitiatedDisconnect;
}