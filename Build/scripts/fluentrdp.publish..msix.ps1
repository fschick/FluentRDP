# Ensure unsigned powershell script execution is allowed: Set-ExecutionPolicy -ExecutionPolicy RemoteSigned

param (
  [Parameter(Mandatory=$true)][String]$version,
  [Parameter(Mandatory=$false)][String]$runtime = "win-x64",
  [Parameter(Mandatory=$false)][String]$publishFolder
)

. $PSScriptRoot/_core.ps1

Push-Location $PSScriptRoot/../..

# Configure
$framework = "net10.0-windows"

# Publish MSIX package
Publish-Msix -project FluentRDP -version $version -framework $framework -runtime $runtime -publishFolder $publishFolder

Pop-Location
