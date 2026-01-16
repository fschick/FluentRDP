# Ensure unsigned powershell script execution is allowed: Set-ExecutionPolicy -ExecutionPolicy RemoteSigned
#
# Creates a self-signed code signing certificate for FluentRDP MSIX packages
# This certificate matches the Publisher field in FluentRDP.appxmanifest
#
# Usage:
#   .\fluentrdp.create-certificate.ps1 [-OutputPath <path>] [-Password <password>] [-InstallToTrustedRoot]
#

param (
    [Parameter(Mandatory=$false)]
    [String]$OutputPath = "$env:USERPROFILE\FluentRDP.pfx",
    
    [Parameter(Mandatory=$false)]
    [String]$Password,
    
    [Parameter(Mandatory=$false)]
    [Switch]$InstallToTrustedRoot
)

# Publisher from FluentRDP.appxmanifest - must match exactly
$subject = "CN=Florian Schick, O=Schick Software, L=Donaueschingen, S=Baden-Württemberg, C=Germany"

Write-Host -ForegroundColor Green "Creating self-signed code signing certificate for FluentRDP..."
Write-Host -ForegroundColor Cyan "Subject: $subject"

# Check if running as Administrator (needed for InstallToTrustedRoot)
if ($InstallToTrustedRoot) {
    $isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
    if (-not $isAdmin) {
        Write-Host -ForegroundColor Red "Error: Administrator privileges required to install certificate to Trusted Root."
        Write-Host -ForegroundColor Yellow "Please run PowerShell as Administrator, or remove -InstallToTrustedRoot flag."
        exit 1
    }
}

# Prompt for password if not provided
if (-not $Password) {
    $securePassword = Read-Host "Enter password for certificate (or press Enter for no password)" -AsSecureString
    if ($securePassword.Length -eq 0) {
        $Password = $null
    } else {
        $BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($securePassword)
        $Password = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
    }
}

# Create the certificate
Write-Host -ForegroundColor Green "Generating certificate..."
try {
    $cert = New-SelfSignedCertificate `
        -Type CodeSigningCert `
        -Subject $subject `
        -KeyUsage DigitalSignature `
        -FriendlyName "FluentRDP Code Signing Certificate" `
        -CertStoreLocation Cert:\CurrentUser\My `
        -KeyExportPolicy Exportable `
        -KeySpec Signature `
        -KeyLength 2048 `
        -KeyAlgorithm RSA `
        -HashAlgorithm SHA256 `
        -NotAfter (Get-Date).AddYears(3)
    
    Write-Host -ForegroundColor Green "Certificate created successfully!"
    Write-Host -ForegroundColor Cyan "Thumbprint: $($cert.Thumbprint)"
    Write-Host -ForegroundColor Cyan "Valid until: $($cert.NotAfter)"
} catch {
    Write-Host -ForegroundColor Red "Error creating certificate: $_"
    exit 1
}

# Export to PFX file
Write-Host -ForegroundColor Green "Exporting certificate to: $OutputPath"
try {
    $securePasswordObj = if ($Password) {
        ConvertTo-SecureString -String $Password -Force -AsPlainText
    } else {
        $null
    }
    
    Export-PfxCertificate -Cert $cert -FilePath $OutputPath -Password $securePasswordObj | Out-Null
    Write-Host -ForegroundColor Green "Certificate exported successfully!"
} catch {
    Write-Host -ForegroundColor Red "Error exporting certificate: $_"
    exit 1
}

# Install to Trusted Root if requested
if ($InstallToTrustedRoot) {
    Write-Host -ForegroundColor Green "Installing certificate to Trusted Root Certification Authorities..."
    try {
        $securePasswordObj = if ($Password) {
            ConvertTo-SecureString -String $Password -Force -AsPlainText
        } else {
            $null
        }
        
        Import-PfxCertificate -FilePath $OutputPath -CertStoreLocation Cert:\LocalMachine\Root -Password $securePasswordObj | Out-Null
        Write-Host -ForegroundColor Green "Certificate installed to Trusted Root successfully!"
        Write-Host -ForegroundColor Yellow "Note: This only affects this machine. Users installing your MSIX will still see warnings unless they also install the certificate."
    } catch {
        Write-Host -ForegroundColor Red "Error installing certificate to Trusted Root: $_"
        Write-Host -ForegroundColor Yellow "Continuing anyway - certificate file is still available at: $OutputPath"
    }
}

Write-Host ""
Write-Host -ForegroundColor Green "Certificate creation complete!"
Write-Host -ForegroundColor Cyan "Certificate file: $OutputPath"
Write-Host ""
Write-Host -ForegroundColor Yellow "To use this certificate when building MSIX:"
if ($Password) {
    Write-Host -ForegroundColor White "  .\Build\scripts\fluentrdp.msix.ps1 -version `"1.0.0`" -certificatePath `"$OutputPath`" -certificatePassword `"$Password`""
} else {
    Write-Host -ForegroundColor White "  .\Build\scripts\fluentrdp.msix.ps1 -version `"1.0.0`" -certificatePath `"$OutputPath`""
}
