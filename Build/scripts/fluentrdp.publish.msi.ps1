# Ensure unsigned powershell script execution is allowed: Set-ExecutionPolicy -ExecutionPolicy RemoteSigned

param (
  [Parameter(Mandatory=$true)][String]$version,
  [Parameter(Mandatory=$false)][String]$runtime,
  [Parameter(Mandatory=$false)][String]$publishFolder,
  [Parameter(Mandatory=$false)][Switch]$generateWingetManifest,
  [Parameter(Mandatory=$false)][String]$releaseUrl,
  [Parameter(Mandatory=$false)][String]$wingetOutputPath
)

. $PSScriptRoot/_core.ps1

Push-Location $PSScriptRoot/../..

# Configure
$framework = "net10.0-windows"

# Publish MSI package
$msiPath = Publish-Msi -project FluentRDP -version $version -framework $framework -runtime $runtime -publishFolder $publishFolder

# Generate winget manifest if requested
if ($generateWingetManifest) {
    Write-Host -ForegroundColor Green "Generating winget manifest..."
    & $PSScriptRoot/fluentrdp.publish.winget.ps1 `
        -version $version `
        -msiPath $msiPath `
        -releaseUrl $releaseUrl `
        -outputPath $wingetOutputPath
}

Pop-Location
