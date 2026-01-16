# MSIX Certificate Guide for FluentRDP

This guide explains how to obtain and use a certificate for signing MSIX packages.

## Certificate Requirements

Your MSIX manifest specifies the Publisher as:
```
CN=Florian Schick, O=Schick Software, L=Donaueschingen, S=Baden-Württemberg, C=Germany
```

**Important**: The certificate's Subject must match this Publisher field exactly, or the MSIX package will fail to install.

## Option 1: Self-Signed Certificate (Testing/Development)

A self-signed certificate is free and quick to create, but Windows will show a warning when installing the MSIX package. This is suitable for testing and development.

### Create a Self-Signed Certificate

Run this PowerShell command as Administrator:

```powershell
$cert = New-SelfSignedCertificate `
    -Type CodeSigningCert `
    -Subject "CN=Florian Schick, O=Schick Software, L=Donaueschingen, S=Baden-Württemberg, C=Germany" `
    -KeyUsage DigitalSignature `
    -FriendlyName "FluentRDP Code Signing Certificate" `
    -CertStoreLocation Cert:\CurrentUser\My `
    -KeyExportPolicy Exportable `
    -KeySpec Signature `
    -KeyLength 2048 `
    -KeyAlgorithm RSA `
    -HashAlgorithm SHA256 `
    -NotAfter (Get-Date).AddYears(3)

# Export the certificate to a PFX file
$password = Read-Host "Enter password for certificate" -AsSecureString
$certPath = "$env:USERPROFILE\FluentRDP.pfx"
Export-PfxCertificate -Cert $cert -FilePath $certPath -Password $password

Write-Host "Certificate created at: $certPath"
Write-Host "Certificate thumbprint: $($cert.Thumbprint)"
```

### Install the Certificate to Trusted Root (Optional)

To avoid the "Unknown Publisher" warning during installation, you can install the certificate to the Trusted Root Certification Authorities store:

```powershell
# Import to Trusted Root (requires Administrator)
$certPath = "$env:USERPROFILE\FluentRDP.pfx"
$password = Read-Host "Enter certificate password" -AsSecureString
$cert = Import-PfxCertificate -FilePath $certPath -CertStoreLocation Cert:\LocalMachine\Root -Password $password
```

**Note**: This only affects the current machine. Users installing your MSIX will still see warnings unless they also install your certificate.

### Using the Self-Signed Certificate

When building your MSIX package:

```powershell
.\Build\scripts\fluentrdp.msix.ps1 -version "1.0.0" -certificatePath "$env:USERPROFILE\FluentRDP.pfx" -certificatePassword "YourPassword"
```

## Option 2: Sigstore/Fulcio (Free, but Limited for MSIX)

[Sigstore](https://docs.sigstore.dev/) is an open-source project that provides free code signing certificates through its certificate authority called **Fulcio**. These certificates are:

- ✅ **Free** - No cost to obtain
- ✅ **Identity-verified** - Uses OIDC (OpenID Connect) for identity verification
- ✅ **Transparent** - Signing events are logged in public transparency logs
- ✅ **Short-lived** - Certificates are valid for about 10 minutes (ephemeral keys)

### Limitations for MSIX Signing

**⚠️ Important**: While Sigstore certificates are free, they have significant limitations for MSIX packages:

1. **Not Trusted by Windows**: Fulcio's root certificate is **not** in Windows' trusted root certificate store. This means:
   - Windows will **not automatically trust** MSIX packages signed with Fulcio certificates
   - Users will see security warnings or installation failures
   - You would need to manually install Fulcio's root certificate on every target machine

2. **Short Validity**: Certificates are only valid for ~10 minutes, which may complicate signing workflows

3. **Designed for Different Use Cases**: Sigstore is primarily designed for:
   - Container image signing
   - Binary artifact signing in CI/CD pipelines
   - Open source software supply chain security
   - Not specifically for Windows code signing/MSIX

### When Sigstore Might Work

Sigstore/Fulcio certificates could work for MSIX if:
- You're distributing within a controlled environment where you can install the Fulcio root certificate on all target machines
- You're using it for internal/development purposes
- You're combining it with other signing methods for provenance/attestation

### How to Use Sigstore (If You Want to Try)

1. Install Cosign (Sigstore's signing tool): https://docs.sigstore.dev/cosign/installation/
2. Authenticate with an OIDC provider (GitHub, Google, etc.)
3. Sign your MSIX package (though this may require additional tooling to convert/use with Windows signtool)

**Bottom Line**: For production MSIX distribution, Sigstore certificates won't work without manual trust installation on every user's machine. You'll still need a commercial certificate or Microsoft's services for public distribution.

## Option 3: Commercial Code Signing Certificate (Production)

For production distribution, you need a code signing certificate from a trusted Certificate Authority (CA). This ensures your MSIX package is trusted by Windows without requiring users to install your certificate.

### Recommended Certificate Authorities

1. **DigiCert** - https://www.digicert.com/code-signing/
2. **Sectigo (formerly Comodo)** - https://sectigo.com/ssl-certificates-tls/code-signing
3. **GlobalSign** - https://www.globalsign.com/en/code-signing-certificate
4. **SSL.com** - https://www.ssl.com/code-signing-certificates/

### Certificate Requirements

- **Type**: Code Signing Certificate (not SSL/TLS certificate)
- **Subject**: Must match your Publisher field exactly:
  ```
  CN=Florian Schick, O=Schick Software, L=Donaueschingen, S=Baden-Württemberg, C=Germany
  ```
- **Key Size**: 2048-bit RSA minimum (4096-bit recommended)
- **Hash Algorithm**: SHA-256 or higher

### Purchase Process

1. **Choose a CA** and purchase a code signing certificate
2. **Complete identity verification** (Extended Validation certificates require business verification)
3. **Generate a Certificate Signing Request (CSR)** or receive the certificate from the CA
4. **Export to PFX format** if needed (most CAs provide this option)

### Using the Commercial Certificate

Once you have the certificate in PFX format:

```powershell
.\Build\scripts\fluentrdp.msix.ps1 -version "1.0.0" -certificatePath "C:\Path\To\Your\Certificate.pfx" -certificatePassword "YourPassword"
```

## Option 4: Microsoft Store (If Publishing to Store)

If you plan to publish to the Microsoft Store, you don't need your own certificate. Microsoft will sign your package automatically when you submit it through the Microsoft Partner Center.

However, you still need a valid certificate for:
- Testing the MSIX package before submission
- Side-loading the package outside the Store

## Verifying Certificate Match

Before building, verify your certificate matches the Publisher field:

```powershell
$cert = Get-PfxData -FilePath "YourCertificate.pfx" -Password (Read-Host "Password" -AsSecureString)
$subject = $cert.EndEntityCertificates[0].Subject
Write-Host "Certificate Subject: $subject"
Write-Host "Expected Publisher: CN=Florian Schick, O=Schick Software, L=Donaueschingen, S=Baden-Württemberg, C=Germany"
```

The subjects must match exactly (case-sensitive).

## Troubleshooting

### Error: "The certificate chain was issued by an authority that is not trusted"

- **Self-signed certificate**: Install it to Trusted Root (see Option 1 above)
- **Sigstore/Fulcio certificate**: Fulcio's root is not in Windows' trusted store by default. You'd need to install it manually on all target machines (not practical for public distribution)
- **Commercial certificate**: Ensure the CA's root certificate is in Windows' trusted store (usually automatic)

### Error: "The publisher name in the app manifest doesn't match the certificate subject"

- Verify the Publisher field in `FluentRDP.appxmanifest` matches the certificate Subject exactly
- Check for typos, extra spaces, or case differences

### Error: "The signature is invalid"

- Ensure you're using a code signing certificate (not an SSL certificate)
- Verify the certificate hasn't expired
- Check that the certificate password is correct

## Security Best Practices

1. **Protect your certificate**: Store the PFX file securely and never commit it to version control
2. **Use strong passwords**: Use a strong password for your PFX file
3. **Rotate certificates**: Renew certificates before they expire
4. **Use environment variables**: Consider storing certificate paths and passwords in environment variables instead of hardcoding

## Example: Using Environment Variables

You can set environment variables to avoid passing credentials on the command line:

```powershell
$env:MSIX_CERT_PATH = "$env:USERPROFILE\FluentRDP.pfx"
$env:MSIX_CERT_PASSWORD = "YourPassword"

.\Build\scripts\fluentrdp.msix.ps1 -version "1.0.0" -certificatePath $env:MSIX_CERT_PATH -certificatePassword $env:MSIX_CERT_PASSWORD
```

Or modify the build script to read from environment variables automatically.

## References

- **Sigstore Documentation**: https://docs.sigstore.dev/
- **Fulcio Certificate Authority**: https://docs.sigstore.dev/certificate_authority/certificate-issuing-overview/
- **Microsoft MSIX Signing Guide**: https://learn.microsoft.com/en-us/windows/msix/package/signing-package-overview
- **Windows Code Signing Best Practices**: https://learn.microsoft.com/en-us/windows/msix/package/sign-app-package-using-signtool
