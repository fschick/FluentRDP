# Ensure unsigned powershell script execution is allowed: Set-ExecutionPolicy -ExecutionPolicy RemoteSigned

param (
  [Parameter(Mandatory=$true)][String]$version,
  [Parameter(Mandatory=$true)][String]$msiPath,
  [Parameter(Mandatory=$false)][String]$outputPath,
  [Parameter(Mandatory=$false)][String]$releaseUrl,
  [Parameter(Mandatory=$false)][String]$publisher = "SchickSoftware",
  [Parameter(Mandatory=$false)][String]$packageName = "FluentRDP"
)

. $PSScriptRoot/_core.ps1

# Function to extract ProductCode from MSI
function Get-MsiProductCode {
    param([String]$msiPath)
    
    try {
        # Use Windows Installer COM object to read ProductCode
        $installer = New-Object -ComObject WindowsInstaller.Installer
        $database = $installer.GetType().InvokeMember("OpenDatabase", "InvokeMethod", $null, $installer, @($msiPath, 0))
        $view = $database.GetType().InvokeMember("OpenView", "InvokeMethod", $null, $database, "SELECT Value FROM Property WHERE Property='ProductCode'")
        $view.GetType().InvokeMember("Execute", "InvokeMethod", $null, $view, $null)
        $record = $view.GetType().InvokeMember("Fetch", "InvokeMethod", $null, $view, $null)
        $productCode = $record.GetType().InvokeMember("StringData", "GetProperty", $null, $record, 1)
        $view.GetType().InvokeMember("Close", "InvokeMethod", $null, $view, $null)
        [System.Runtime.Interopservices.Marshal]::ReleaseComObject($view) | Out-Null
        [System.Runtime.Interopservices.Marshal]::ReleaseComObject($database) | Out-Null
        [System.Runtime.Interopservices.Marshal]::ReleaseComObject($installer) | Out-Null
        return $productCode
    }
    catch {
        Write-Host -ForegroundColor Yellow "Warning: Could not extract ProductCode from MSI. You may need to set it manually in the manifest."
        Write-Host -ForegroundColor Yellow "Error: $_"
        # Return empty string - user will need to fill this in
        return ""
    }
}

Push-Location $PSScriptRoot/../..

# Validate MSI file exists
if (-not (Test-Path $msiPath)) {
    Write-Host -ForegroundColor Red "Error: MSI file not found: $msiPath"
    exit 1
}

# Get MSI file info
$msiFile = Get-Item $msiPath
$msiFileName = $msiFile.Name

# Calculate SHA256 hash
Write-Host -ForegroundColor Green "Calculating SHA256 hash for $msiFileName..."
$sha256 = (Get-FileHash -Path $msiPath -Algorithm SHA256).Hash

# Extract ProductCode from MSI (required for winget)
Write-Host -ForegroundColor Green "Extracting ProductCode from MSI..."
$productCode = Get-MsiProductCode -msiPath $msiPath

# Get version info
$fileVersion = Get-FileVersion -version $version
$packageVersion = $fileVersion

# Determine architecture from MSI (assuming x64 based on installer project)
$architecture = "x64"

# If release URL not provided, construct GitHub release URL
if (-not $releaseUrl) {
    $repositoryUrl = "https://github.com/fschick/FluentRDP"
    $releaseUrl = "$repositoryUrl/releases/download/v$packageVersion/$msiFileName"
}

# Determine output path
if (-not $outputPath) {
    $outputPath = Join-Path (Split-Path $msiPath) "winget-manifest"
}

# Create manifest directory structure: Publisher/PackageName/Version/
$manifestDir = Join-Path $outputPath "$publisher\$packageName\$packageVersion"
New-Item -ItemType Directory -Path $manifestDir -Force | Out-Null

# Read project metadata
$projectProps = [xml](Get-Content "Build\targets\version.props")
$description = $projectProps.Project.PropertyGroup.Description
$homepage = $projectProps.Project.PropertyGroup.PackageProjectUrl
$license = $projectProps.Project.PropertyGroup.PackageLicenseExpression
$authors = $projectProps.Project.PropertyGroup.Authors

# Create manifest YAML content
$manifestContent = @"
# yaml-language-server: `$schema=https://aka.ms/winget-manifest.schema.2.0.0
PackageIdentifier: $publisher.$packageName
PackageVersion: $packageVersion
PackageName: $packageName
Publisher: $publisher
Description: $description
License: $license
Homepage: $homepage
Author: $authors
PackageLocale: en-US
Moniker: fluentrdp
Tags:
  - rdp
  - remote-desktop
  - remote-access
  - mstsc
Commands:
  - FluentRDP
InstallerType: msi
Installers:
  - Architecture: $architecture
    InstallerUrl: $releaseUrl
    InstallerSha256: $sha256
    ProductCode: $productCode
"@

# Write manifest file
$manifestFile = Join-Path $manifestDir "$publisher.$packageName.installer.yaml"
$manifestContent | Out-File -FilePath $manifestFile -Encoding UTF8 -NoNewline

Write-Host -ForegroundColor Green "Winget manifest created: $manifestFile"
Write-Host -ForegroundColor Yellow "Manifest directory: $manifestDir"
Write-Host -ForegroundColor Yellow "Next steps:"
Write-Host -ForegroundColor Yellow "  1. Review the manifest file"
Write-Host -ForegroundColor Yellow "  2. Test locally: winget install --manifest $manifestFile"
Write-Host -ForegroundColor Yellow "  3. Submit PR to: https://github.com/microsoft/winget-pkgs"

Pop-Location
