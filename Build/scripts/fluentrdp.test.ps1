# Ensure unsigned powershell script execution is allowed: Set-ExecutionPolicy -ExecutionPolicy RemoteSigned

param (
  [Parameter()][String]$version
)

. $PSScriptRoot/_core.ps1

Push-Location $PSScriptRoot/../..

# Configure
if (!$version) {
	$version = git describe --tags
}

# Run tests
Build-Project -project FluentRDP -version $version
Test-Project -project FluentRDP.slnx -version $version

Pop-Location