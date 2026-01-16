# Ensure unsigned powershell script execution is allowed: Set-ExecutionPolicy -ExecutionPolicy RemoteSigned

param (
  [Parameter(Mandatory=$true)][String]$version,
  [Parameter(Mandatory=$true)][String]$msiPath,
  [Parameter(Mandatory=$false)][String]$releaseUrl,
  [Parameter(Mandatory=$false)][String]$outputPath,
  [Parameter(Mandatory=$false)][String]$publisher = "SchickSoftware",
  [Parameter(Mandatory=$false)][String]$packageName = "FluentRDP"
)

. $PSScriptRoot/_core.ps1

Push-Location $PSScriptRoot/../..

# Generate installer manifest
& $PSScriptRoot/fluentrdp.generate-winget-manifest.ps1 `
    -version $version `
    -msiPath $msiPath `
    -releaseUrl $releaseUrl `
    -outputPath $outputPath `
    -publisher $publisher `
    -packageName $packageName

if ($LASTEXITCODE -ne 0) {
    Pop-Location
    exit $LASTEXITCODE
}

# Determine manifest path for locale manifests
$fileVersion = Get-FileVersion -version $version
if ($outputPath) {
    $manifestPath = Join-Path $outputPath "$publisher\$packageName\$fileVersion"
} else {
    $manifestPath = Join-Path (Split-Path $msiPath) "winget-manifest\$publisher\$packageName\$fileVersion"
}

# Generate locale and version manifests
& $PSScriptRoot/fluentrdp.generate-winget-locale-manifests.ps1 `
    -version $version `
    -manifestPath $manifestPath `
    -publisher $publisher `
    -packageName $packageName

if ($LASTEXITCODE -ne 0) {
    Pop-Location
    exit $LASTEXITCODE
}

Write-Host -ForegroundColor Green "All winget manifest files generated successfully!"
Write-Host -ForegroundColor Yellow "Manifest location: $manifestPath"

Pop-Location
