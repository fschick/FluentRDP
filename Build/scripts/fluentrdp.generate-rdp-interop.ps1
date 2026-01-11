# Generate RDP COM interop assemblies using aximp.exe
# This script generates AxMSTSCLib.dll and MSTSCLib.dll from the Windows RDP ActiveX control

param(
    [Parameter(Mandatory=$true)]
    [String]$OutputPath
)

$ErrorActionPreference = "Stop"

# Find aximp.exe - it's typically in the Windows SDK or .NET Framework SDK
$aximpPaths = @(
    "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\aximp.exe",
    "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools\aximp.exe",
    "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.1 Tools\aximp.exe",
    "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7 Tools\aximp.exe",
    "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools\aximp.exe",
    "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\aximp.exe",
    "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools\aximp.exe",
    "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools\aximp.exe",
    "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\aximp.exe",
    "${env:ProgramFiles}\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\aximp.exe",
    "${env:ProgramFiles}\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools\aximp.exe"
)

$aximp = $null
foreach ($path in $aximpPaths) {
    if (Test-Path $path) {
        $aximp = $path
        break
    }
}

if (-not $aximp) {
    # Try to find it in PATH
    $aximpCmd = Get-Command aximp.exe -ErrorAction SilentlyContinue
    if ($aximpCmd) {
        $aximp = $aximpCmd.Source
    }
}

# If still not found, try to find it via vswhere (Visual Studio Installer)
if (-not $aximp -or -not (Test-Path $aximp)) {
    $vswhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
    if (Test-Path $vswhere) {
        $vsPath = & $vswhere -latest -property installationPath
        if ($vsPath) {
            $aximpVsPaths = @(
                "$vsPath\MSBuild\Current\Bin\aximp.exe",
                "$vsPath\MSBuild\15.0\Bin\aximp.exe"
            )
            foreach ($path in $aximpVsPaths) {
                if (Test-Path $path) {
                    $aximp = $path
                    break
                }
            }
        }
    }
}

if (-not $aximp -or -not (Test-Path $aximp)) {
    throw "aximp.exe not found. Please install Windows SDK or .NET Framework SDK. The Windows SDK is typically included with Visual Studio Build Tools."
}

# Find mstscax.dll - the RDP ActiveX control
$mstscaxPaths = @(
    "${env:SystemRoot}\System32\mstscax.dll",
    "${env:SystemRoot}\SysWOW64\mstscax.dll"
)

$mstscax = $null
foreach ($path in $mstscaxPaths) {
    if (Test-Path $path) {
        $mstscax = $path
        break
    }
}

if (-not $mstscax) {
    throw "mstscax.dll not found. RDP ActiveX control is not available on this system."
}

# Ensure output directory exists and resolve to absolute path
if (-not (Test-Path $OutputPath)) {
    New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
}

$outputPathResolved = (Resolve-Path $OutputPath -ErrorAction SilentlyContinue).Path
if (-not $outputPathResolved) {
    $outputPathResolved = (New-Item -ItemType Directory -Path $OutputPath -Force).FullName
}

# Change to output directory before running aximp
$originalLocation = Get-Location
Push-Location $outputPathResolved

try {
    Write-Host "Generating RDP COM interop assemblies..."
    Write-Host "  Using aximp.exe: $aximp"
    Write-Host "  Using mstscax.dll: $mstscax"
    Write-Host "  Output path: $outputPathResolved"

    # Generate the interop assemblies
    # aximp.exe generates both AxMSTSCLib.dll and MSTSCLib.dll
    # We're already in the output directory, so no need for /out parameter
    # aximp will output to the current directory by default
    $arguments = @(
        "`"$mstscax`""
    )

    Write-Host "Running: $aximp $($arguments -join ' ')"
    & $aximp $arguments

    if ($LASTEXITCODE -ne 0) {
        throw "aximp.exe failed with exit code $LASTEXITCODE"
    }

    # Verify the files were created (relative to current directory)
    $axMstscLib = "AxMSTSCLib.dll"
    $mstscLib = "MSTSCLib.dll"

    if (-not (Test-Path $axMstscLib)) {
        throw "AxMSTSCLib.dll was not generated at $axMstscLib"
    }

    if (-not (Test-Path $mstscLib)) {
        throw "MSTSCLib.dll was not generated at $mstscLib"
    }

    Write-Host "Successfully generated RDP COM interop assemblies:"
    Write-Host "  - $(Join-Path $outputPathResolved $axMstscLib)"
    Write-Host "  - $(Join-Path $outputPathResolved $mstscLib)"
}
finally {
    Pop-Location
}
