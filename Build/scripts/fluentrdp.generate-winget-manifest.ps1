# Ensure unsigned powershell script execution is allowed: Set-ExecutionPolicy -ExecutionPolicy RemoteSigned

param (
  [Parameter(Mandatory=$true)][String]$version,
  [Parameter(Mandatory=$true)][String]$sha256,
  [Parameter(Mandatory=$true)][String]$msiPath
)

. $PSScriptRoot/_core.ps1

Push-Location $PSScriptRoot/../..

# Read project metadata
$versionProps = [xml](Get-Content "Build\targets\version.props")
$product = $versionProps.Project.PropertyGroup.Product
$publisher = $versionProps.Project.PropertyGroup.Company
$description = $versionProps.Project.PropertyGroup.Description
$homepage = $versionProps.Project.PropertyGroup.PackageProjectUrl
$license = $versionProps.Project.PropertyGroup.PackageLicenseExpression

# Determine publisher identifier (first letter lowercase)
# Remove spaces and special characters, keep only alphanumeric
$publisherId = $publisher -replace '\s+', '' -replace '[^a-zA-Z0-9]', ''
if ($publisherId.Length -eq 0) {
    $publisherId = "Publisher"
}
$publisherIdFirstLetter = $publisherId.Substring(0,1).ToLower()
$publisherIdRest = $publisherId.Substring(1)
$packageIdentifier = "${publisherIdFirstLetter}${publisherIdRest}.${product}"

# Get file name from MSI path
$msiFileName = Split-Path -Leaf $msiPath

# Create manifest directory structure
$manifestBaseDir = "WinGet\manifests\$publisherIdFirstLetter\$publisherId\$product"
$versionDir = Join-Path $manifestBaseDir $version

if (-not (Test-Path $versionDir)) {
    New-Item -ItemType Directory -Path $versionDir -Force | Out-Null
}

# Determine installer URL (GitHub Releases)
$repoUrl = $homepage
$installerUrl = "$repoUrl/releases/download/v$version/$msiFileName"

# Create installer manifest
$installerManifest = @"
# yaml-language-server: `$schema=https://aka.ms/winget-manifest.installer.1.4.0.schema.json

InstallerType: msi
Architecture: x64
InstallerUrl: $installerUrl
InstallerSha256: $sha256
ProductCode: `{A8B5C3D4-E5F6-4A7B-8C9D-0E1F2A3B4C5D`}
"@

$installerManifestPath = Join-Path $versionDir "FluentRDP.installer.yaml"
$installerManifest | Out-File -FilePath $installerManifestPath -Encoding UTF8

# Create locale manifest (default)
$localeManifest = @"
# yaml-language-server: `$schema=https://aka.ms/winget-manifest.defaultLocale.1.4.0.schema.json

PackageLocale: en-US
Publisher: $publisher
PublisherUrl: $homepage
PackageName: $product
PackageUrl: $homepage
License: $license
LicenseUrl: $homepage/blob/main/LICENSE
Copyright: $($versionProps.Project.PropertyGroup.Copyright)
CopyrightUrl: $homepage
ShortDescription: $description
Description: $description
"@

$localeManifestPath = Join-Path $versionDir "FluentRDP.locale.en-US.yaml"
$localeManifest | Out-File -FilePath $localeManifestPath -Encoding UTF8

# Create version manifest
$versionManifest = @"
# yaml-language-server: `$schema=https://aka.ms/winget-manifest.version.1.4.0.schema.json

PackageIdentifier: $packageIdentifier
PackageVersion: $version
DefaultLocale: en-US
ManifestType: version
ManifestVersion: 1.4
"@

$versionManifestPath = Join-Path $versionDir "FluentRDP.yaml"
$versionManifest | Out-File -FilePath $versionManifestPath -Encoding UTF8

Write-Host -ForegroundColor Green "Winget manifest generated:"
Write-Host -ForegroundColor Green "  Version: $version"
Write-Host -ForegroundColor Green "  Package Identifier: $packageIdentifier"
Write-Host -ForegroundColor Green "  Manifest Directory: $versionDir"

Pop-Location
