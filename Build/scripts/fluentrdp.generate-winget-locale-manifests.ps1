# Ensure unsigned powershell script execution is allowed: Set-ExecutionPolicy -ExecutionPolicy RemoteSigned

param (
  [Parameter(Mandatory=$true)][String]$version,
  [Parameter(Mandatory=$false)][String]$manifestPath,
  [Parameter(Mandatory=$false)][String]$publisher = "SchickSoftware",
  [Parameter(Mandatory=$false)][String]$packageName = "FluentRDP"
)

. $PSScriptRoot/_core.ps1

Push-Location $PSScriptRoot/../..

# Determine manifest directory
if (-not $manifestPath) {
    $manifestPath = "winget-manifest\$publisher\$packageName\$version"
}

if (-not (Test-Path $manifestPath)) {
    Write-Host -ForegroundColor Red "Error: Manifest directory not found: $manifestPath"
    Write-Host -ForegroundColor Yellow "Please run fluentrdp.generate-winget-manifest.ps1 first or specify the correct path."
    Pop-Location
    exit 1
}

# Get version info
$fileVersion = Get-FileVersion -version $version
$packageVersion = $fileVersion

# Read project metadata
$projectProps = [xml](Get-Content "Build\targets\version.props")
$description = $projectProps.Project.PropertyGroup.Description
$homepage = $projectProps.Project.PropertyGroup.PackageProjectUrl
$license = $projectProps.Project.PropertyGroup.PackageLicenseExpression
$authors = $projectProps.Project.PropertyGroup.Authors
$copyright = $projectProps.Project.PropertyGroup.Copyright

# Create locale manifest (en-US)
$localeManifestContent = @"
# yaml-language-server: `$schema=https://aka.ms/winget-manifest.schema.2.0.0
PackageIdentifier: $publisher.$packageName
PackageVersion: $packageVersion
PackageLocale: en-US
Publisher: $publisher
PublisherUrl: $homepage
PublisherSupportUrl: $homepage/issues
PrivacyUrl: $homepage
Author: $authors
PackageName: $packageName
PackageUrl: $homepage
License: $license
LicenseUrl: $homepage/blob/main/LICENSE
Copyright: $copyright
CopyrightUrl: $homepage
ShortDescription: $description
Description: $description

Tags:
  - rdp
  - remote-desktop
  - remote-access
  - mstsc
Moniker: fluentrdp
"@

$localeManifestFile = Join-Path $manifestPath "$publisher.$packageName.locale.en-US.yaml"
$localeManifestContent | Out-File -FilePath $localeManifestFile -Encoding UTF8 -NoNewline

Write-Host -ForegroundColor Green "Locale manifest created: $localeManifestFile"

# Create default locale manifest (version manifest)
$versionManifestContent = @"
# yaml-language-server: `$schema=https://aka.ms/winget-manifest.schema.2.0.0
PackageIdentifier: $publisher.$packageName
PackageVersion: $packageVersion
DefaultLocale: en-US
ManifestType: version
ManifestVersion: 1.7.0
"@

$versionManifestFile = Join-Path $manifestPath "$publisher.$packageName.yaml"
$versionManifestContent | Out-File -FilePath $versionManifestFile -Encoding UTF8 -NoNewline

Write-Host -ForegroundColor Green "Version manifest created: $versionManifestFile"
Write-Host -ForegroundColor Yellow "All required manifest files have been generated in: $manifestPath"

Pop-Location
