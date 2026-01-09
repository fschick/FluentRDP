using FluentRDP.Platform;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FluentRDP.Services;

/// <summary>
/// Service for managing RDP credentials stored in Windows Credential Manager
/// </summary>
internal static class RdpCredentialService
{
    private const string TERMSRV_PREFIX = "TERMSRV/";

    /// <summary>
    /// Checks if saved credentials exist for an RDP connection
    /// </summary>
    /// <param name="hostname">The hostname or IP address. Can include port (e.g., "server.com:3389")</param>
    /// <returns>The saved username if credentials exist, null otherwise</returns>
    public static string? CredentialsExist(string? hostname)
    {
        if (string.IsNullOrWhiteSpace(hostname))
            return null;

        var credentialTarget = $"{TERMSRV_PREFIX}{hostname}";

        var result = Interop.CredRead(credentialTarget, Interop.CredentialType.DomainPassword, 0, out var credentialPtr);
        if (!result || credentialPtr == IntPtr.Zero)
            return null;

        try
        {
            var credential = Marshal.PtrToStructure<Interop.CREDENTIAL>(credentialPtr);
            return credential.UserName != IntPtr.Zero
                ? Marshal.PtrToStringUni(credential.UserName)
                : null;
        }
        finally
        {
            Interop.CredFree(credentialPtr);
        }
    }

    /// <summary>
    /// Saves credentials for an RDP connection to Windows Credential Manager
    /// </summary>
    /// <param name="hostname">The hostname or IP address. Can include port (e.g., "server.com:3389")</param>
    /// <param name="userName">The username. Can include domain in format "domain\username" or "username@domain"</param>
    /// <param name="password">The password to save</param>
    public static void SaveCredentials(string hostname, string userName, string? password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hostname);
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);

        var credentialTarget = GetCredentialTarget(hostname);
        var passwordBytes = Encoding.Unicode.GetBytes(password ?? string.Empty);
        var targetNamePtr = Marshal.StringToCoTaskMemUni(credentialTarget);
        var userNamePtr = Marshal.StringToCoTaskMemUni(userName);
        var credentialBlobPtr = Marshal.AllocCoTaskMem(passwordBytes.Length);
        Marshal.Copy(passwordBytes, 0, credentialBlobPtr, passwordBytes.Length);

        var credential = new Interop.CREDENTIAL
        {
            Flags = 0,
            Type = (int)Interop.CredentialType.DomainPassword,
            TargetName = targetNamePtr,
            Comment = IntPtr.Zero,
            LastWritten = default,
            CredentialBlobSize = (uint)passwordBytes.Length,
            CredentialBlob = credentialBlobPtr,
            Persist = Interop.CRED_PERSIST_LOCAL_MACHINE,
            AttributeCount = 0,
            Attributes = IntPtr.Zero,
            TargetAlias = IntPtr.Zero,
            UserName = userNamePtr
        };

        var credentialPtr = IntPtr.Zero;
        try
        {
            credentialPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(Interop.CREDENTIAL)));
            Marshal.StructureToPtr(credential, credentialPtr, false);
            Interop.CredWrite(credentialPtr, 0);
        }
        finally
        {
            if (credentialPtr != IntPtr.Zero)
                Marshal.FreeCoTaskMem(credentialPtr);
            if (targetNamePtr != IntPtr.Zero)
                Marshal.FreeCoTaskMem(targetNamePtr);
            if (userNamePtr != IntPtr.Zero)
                Marshal.FreeCoTaskMem(userNamePtr);
            if (credentialBlobPtr != IntPtr.Zero)
                Marshal.FreeCoTaskMem(credentialBlobPtr);
        }
    }

    /// <summary>
    /// Removes saved credentials for an RDP connection
    /// </summary>
    /// <param name="hostname">The hostname or IP address. Can include port (e.g., "server.com:3389")</param>
    /// <returns>True if credentials were removed successfully, false otherwise</returns>
    public static void RemoveCredentials(string? hostname)
    {
        if (string.IsNullOrWhiteSpace(hostname))
            return;

        var credentialTarget = GetCredentialTarget(hostname);
        Interop.CredDelete(credentialTarget, Interop.CredentialType.DomainPassword, 0);
    }

    private static string GetCredentialTarget(string hostname)
        => $"{TERMSRV_PREFIX}{hostname}";
}
