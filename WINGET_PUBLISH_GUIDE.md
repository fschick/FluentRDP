# Winget Publishing Guide for FluentRDP

This guide will walk you through publishing FluentRDP to the Windows Package Manager (winget) repository.

## Prerequisites

1. **GitHub Account** - You need a GitHub account to submit PRs to the winget-pkgs repository
2. **Git** - Installed and configured on your system
3. **Winget CLI** - Usually pre-installed on Windows 10/11, or install from Microsoft Store
4. **PowerShell** - For running the build scripts

## Overview

Publishing to winget involves:
1. Building and publishing your MSI installer
2. Generating a winget manifest file
3. Creating a GitHub release with the MSI file
4. Submitting a PR to the winget-pkgs repository

## Step-by-Step Instructions

### Step 1: Build and Publish Your Application

First, build your MSI installer. You can either:

**Option A: Generate winget manifest during MSI publish (Recommended)**
```powershell
cd D:\Prog\CSharp\Schick\FS.FluentRDP
Build\scripts\fluentrdp.publish.msi.ps1 -version "1.0.0" -generateWingetManifest -releaseUrl "https://github.com/fschick/FluentRDP/releases/download/v1.0.0/FluentRDP-win-x64.msi"
```

**Option B: Generate winget manifest separately**
```powershell
# First, publish the MSI
Build\scripts\fluentrdp.publish.msi.ps1 -version "1.0.0"

# Then generate the manifest (adjust path to your MSI)
Build\scripts\fluentrdp.publish.winget.ps1 -version "1.0.0" -msiPath "path\to\FluentRDP-win-x64.msi" -releaseUrl "https://github.com/fschick/FluentRDP/releases/download/v1.0.0/FluentRDP-win-x64.msi"
```

The manifest will be generated in a `winget-manifest` folder with the structure:
```
winget-manifest/
└── SchickSoftware/
    └── FluentRDP/
        └── 1.0.0/
            └── SchickSoftware.FluentRDP.installer.yaml
```

### Step 2: Review the Generated Manifest

Open the generated manifest file and verify:
- Package version matches your release
- Installer URL points to your GitHub release
- SHA256 hash is correct
- ProductCode is extracted correctly (if empty, you'll need to fill it in manually)

**Important**: If the ProductCode extraction failed, you can find it by:
1. Installing the MSI on a test machine
2. Running: `Get-WmiObject Win32_Product | Where-Object {$_.Name -eq "Fluent RDP"} | Select-Object IdentifyingNumber`
3. Or using Orca/MSI tools to inspect the MSI file

### Step 3: Test the Manifest Locally

Before submitting to winget, test the manifest locally:

```powershell
# Test install from local manifest
winget install --manifest "winget-manifest\SchickSoftware\FluentRDP\1.0.0\SchickSoftware.FluentRDP.installer.yaml"

# Or test with the directory
winget install --manifest "winget-manifest\SchickSoftware\FluentRDP\1.0.0"
```

Verify that:
- Installation works correctly
- Application launches properly
- Uninstallation works: `winget uninstall FluentRDP`

### Step 4: Create a GitHub Release

1. **Tag your release** (if not already done):
   ```powershell
   git tag -a v1.0.0 -m "Release version 1.0.0"
   git push origin v1.0.0
   ```

2. **Create a GitHub Release**:
   - Go to https://github.com/fschick/FluentRDP/releases
   - Click "Draft a new release"
   - Select the tag (e.g., `v1.0.0`)
   - Add release notes
   - **Upload your MSI file** to the release
   - Publish the release

3. **Verify the download URL**:
   The URL should be: `https://github.com/fschick/FluentRDP/releases/download/v1.0.0/FluentRDP-win-x64.msi`
   (Adjust version and filename as needed)

### Step 5: Fork and Clone the winget-pkgs Repository

1. **Fork the repository**:
   - Go to https://github.com/microsoft/winget-pkgs
   - Click "Fork" to create your fork

2. **Clone your fork**:
   ```powershell
   git clone https://github.com/YOUR_USERNAME/winget-pkgs.git
   cd winget-pkgs
   ```

3. **Add upstream remote**:
   ```powershell
   git remote add upstream https://github.com/microsoft/winget-pkgs.git
   ```

### Step 6: Prepare Your Manifest for Submission

1. **Copy your manifest** to the winget-pkgs structure:
   ```powershell
   # From your FluentRDP project directory
   $wingetPkgsPath = "path\to\winget-pkgs"
   Copy-Item -Recurse "winget-manifest\SchickSoftware" "$wingetPkgsPath\manifests\s\SchickSoftware\FluentRDP"
   ```

   Note: The folder structure in winget-pkgs uses the first letter of the publisher name.

2. **Create additional manifest files** (if this is your first submission):
   
   You need three files in the version folder:
   - `SchickSoftware.FluentRDP.installer.yaml` (already created)
   - `SchickSoftware.FluentRDP.locale.en-US.yaml` (version manifest)
   - `SchickSoftware.FluentRDP.yaml` (default locale)

   **Create `SchickSoftware.FluentRDP.locale.en-US.yaml`**:
   ```yaml
   # yaml-language-server: $schema=https://aka.ms/winget-manifest.schema.2.0.0
   PackageIdentifier: SchickSoftware.FluentRDP
   PackageVersion: 1.0.0
   PackageLocale: en-US
   Publisher: SchickSoftware
   PublisherUrl: https://github.com/fschick/FluentRDP
   PublisherSupportUrl: https://github.com/fschick/FluentRDP/issues
   PrivacyUrl: https://github.com/fschick/FluentRDP
   Author: Florian Schick
   PackageName: FluentRDP
   PackageUrl: https://github.com/fschick/FluentRDP
   License: GPL-3.0
   LicenseUrl: https://github.com/fschick/FluentRDP/blob/main/LICENSE
   Copyright: © Florian Schick, 2026 all rights reserved
   CopyrightUrl: https://github.com/fschick/FluentRDP
   ShortDescription: Modern RDP client with live window resizing. Drop-in replacement for mstsc.exe
   Description: Modern RDP client with live window resizing and zoom (by DPI). Drop-in replacement for mstsc.exe.
   
   Tags:
     - rdp
     - remote-desktop
     - remote-access
     - mstsc
   Moniker: fluentrdp
   ```

   **Create `SchickSoftware.FluentRDP.yaml`** (default locale, same as above):
   ```yaml
   # yaml-language-server: $schema=https://aka.ms/winget-manifest.schema.2.0.0
   PackageIdentifier: SchickSoftware.FluentRDP
   PackageVersion: 1.0.0
   DefaultLocale: en-US
   ManifestType: version
   ManifestVersion: 1.7.0
   ```

### Step 7: Validate Your Manifest

Use winget's validation tool:

```powershell
cd winget-pkgs
winget validate "manifests\s\SchickSoftware\FluentRDP\1.0.0"
```

Fix any validation errors before proceeding.

### Step 8: Submit Your PR

1. **Create a branch**:
   ```powershell
   git checkout -b schicksoftware-fluentrdp-1.0.0
   ```

2. **Add and commit your changes**:
   ```powershell
   git add manifests/s/SchickSoftware/FluentRDP/
   git commit -m "Add FluentRDP version 1.0.0"
   ```

3. **Push to your fork**:
   ```powershell
   git push origin schicksoftware-fluentrdp-1.0.0
   ```

4. **Create a Pull Request**:
   - Go to https://github.com/microsoft/winget-pkgs
   - Click "New Pull Request"
   - Select your fork and branch
   - Fill in the PR description
   - Submit the PR

### Step 9: Monitor Your PR

- The winget team will review your PR
- Automated validation will run
- Address any feedback or issues
- Once merged, your package will be available in winget!

### Step 10: Test the Published Package

After your PR is merged (usually within a few days), test the published package:

```powershell
# Update winget source
winget source update

# Search for your package
winget search FluentRDP

# Install your package
winget install FluentRDP
```

## Updating Your Package

For future releases:

1. Follow Steps 1-3 to generate a new manifest
2. Create a new GitHub release
3. Add the new version folder to your existing package in winget-pkgs:
   ```
   manifests/s/SchickSoftware/FluentRDP/
   ├── 1.0.0/
   │   └── ...
   └── 1.1.0/  ← New version
       └── ...
   ```
4. Update the version manifest file (`SchickSoftware.FluentRDP.yaml`) with the new version
5. Submit a new PR

## Troubleshooting

### ProductCode Extraction Failed
If the ProductCode is empty in your manifest:
1. Install the MSI on a test machine
2. Run: `Get-WmiObject Win32_Product | Where-Object {$_.Name -like "*Fluent*"} | Select-Object IdentifyingNumber`
3. Manually add the ProductCode to the manifest

### Validation Errors
- Check that all URLs are accessible
- Verify SHA256 hash matches
- Ensure version format is correct (semantic versioning)
- Check that all required fields are present

### PR Rejected
Common reasons:
- Manifest validation errors
- Download URL not accessible
- SHA256 hash mismatch
- Missing required fields

## Additional Resources

- [Winget Manifest Documentation](https://github.com/microsoft/winget-cli/blob/master/doc/ManifestSpecv1.7.md)
- [Winget-pkgs Repository](https://github.com/microsoft/winget-pkgs)
- [Winget Validation Tool](https://github.com/microsoft/winget-validate)

## Automation Script

You can create a helper script to automate steps 1-3:

```powershell
# publish-and-prepare-winget.ps1
param(
    [Parameter(Mandatory=$true)][String]$version,
    [Parameter(Mandatory=$false)][String]$runtime
)

# Publish MSI with winget manifest
Build\scripts\fluentrdp.publish.msi.ps1 `
    -version $version `
    -runtime $runtime `
    -generateWingetManifest `
    -releaseUrl "https://github.com/fschick/FluentRDP/releases/download/v$version/FluentRDP-win-x64.msi"

Write-Host "Next steps:"
Write-Host "1. Review the manifest in winget-manifest folder"
Write-Host "2. Test locally: winget install --manifest winget-manifest\SchickSoftware\FluentRDP\$version"
Write-Host "3. Create GitHub release and upload MSI"
Write-Host "4. Submit PR to winget-pkgs"
```

## Notes

- The manifest generator uses "SchickSoftware" as the publisher by default. If you want to change this, modify the `-publisher` parameter.
- The package name defaults to "FluentRDP". This should match your application name.
- Make sure your GitHub release URL matches exactly what's in the manifest.
- The ProductCode must remain consistent across versions for proper upgrade detection.
