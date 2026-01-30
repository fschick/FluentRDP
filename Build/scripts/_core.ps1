# Ensure unsigned powershell script execution ist allowed: Set-ExecutionPolicy -ExecutionPolicy RemoteSigned

#
#region Build
#

function Npm-Restore([String] $folder) {
	Push-Location $folder
	
	# Restore npm packages
	& npm ci --prefer-offline --no-audit
	if(!$?) {
		Pop-Location
		exit $LASTEXITCODE
	}
	
	Pop-Location
}

function Build-Project([String] $project, [String] $version = "0.0.0") {
    # Configure
	Create-Private-NuGet-Config
    $fileVersion = Get-FileVersion -version $version

    # Create arguments
    $arguments = @("-p:Version=$version", "-p:FileVersion=$fileVersion")

    # Build with configuration "Debug"
    Write-Host -ForegroundColor Green "dotnet build $project --configuration Debug $arguments"
    & dotnet build $project --configuration Debug $arguments
    if(!$?) {
		Pop-Location
        exit $LASTEXITCODE
    }

    # Build with configuration "Release"
    $arguments += "-warnaserror"
    Write-Host -ForegroundColor Green "dotnet build $project --configuration Release $arguments"
    & dotnet build $project --configuration Release $arguments
    if(!$?) {
		Pop-Location
        exit $LASTEXITCODE
    }
}

function Build-Ui([String] $project, [String] $outputFolder) {
    Push-Location $project
	
	& npm run lint
	if(!$?) {
		Pop-Location
		exit $LASTEXITCODE
	}
	
	$arguments = @()
	
	if ($outputFolder) {
        $arguments +=@("--output-path=""$outputFolder""")
    }
	
	Write-Host -ForegroundColor Green "npm run build $arguments"
	& npm run build "--" $arguments
	if(!$?) {
		Pop-Location
		exit $LASTEXITCODE
	}
	
	Pop-Location
}

#endregion

#
#region Tests
#

function Test-Project([String] $project, [String] $version, [String] $filter) {
    # Configure
	Create-Private-NuGet-Config
    $fileVersion = Get-FileVersion -version $version
	
	# Clean previous test results
	Get-ChildItem "TestResult" -Recurse | foreach { $_.Delete($TRUE) }

    # Create arguments
    $arguments = @("--logger:trx", "--logger:html", "-p:Version=$version", "-p:FileVersion=$fileVersion")

    if ($filter) {
        $arguments +=@("--filter", $filter)
    }

    # Run tests
    Write-Host -ForegroundColor Green "dotnet test $project $arguments"
    & dotnet test $project $arguments
    if(!$?) {
        exit $LASTEXITCODE
    }
}

function Test-Ui([String] $project) {
    Push-Location $project
	
	Write-Host -ForegroundColor Green "npm run test"
	& npm run test
	if(!$?) {
		Pop-Location
		exit $LASTEXITCODE
	}
	
	Pop-Location
}

#endregion

#
#region Publish
#

function Publish-Project([String] $project, [String] $version, [String] $framework, [String] $runtime, [String] $publishFolder, [String] $publishProfile, [String] $password, [String[]] $compilerSwitches) {
    # Configure
	Create-Private-NuGet-Config
    $fileVersion = Get-FileVersion -version $version
    $configuration = "Release"

    # Create arguments
    $arguments = @("--configuration", $configuration, "--framework", $framework, "-p:Version=$version", "-p:FileVersion=$fileVersion")

    $isSelfContained = !!$runtime
    if ($isSelfContained) {
        $arguments +=@("--self-contained", "--runtime", $runtime)
    }

    if ($publishFolder) {
        $arguments +=@("--output", $publishFolder)
    }
	else {
		$projectIsFile = Test-Path $project -PathType Leaf
		$projectFolder = $(if($projectIsFile) {Split-Path -Path $project} else {$project})
        $runtimeFolder = $(if($isSelfContained) {"/$runtime"} else {""})
        $publishFolder =  "$projectFolder/bin/$configuration/$framework$runtimeFolder/publish"
	}

    if ($publishProfile) {
        $arguments +=@("-p:PublishProfile=""$publishProfile""")
    }

    if ($password) {
        $arguments +=@("-p:Password=""$password""")
    }

    if ($compilerSwitches) {
        Write-Host -ForegroundColor Green "Adding compiler switches: $compilerSwitches"
        $arguments += $compilerSwitches
    }

    # Publish project
    Write-Host -ForegroundColor Green "dotnet publish $project $arguments"
    & dotnet publish $project $arguments
    if(!$?) {
        exit $LASTEXITCODE
    }
}

function Publish-Ui([String] $project, [String] $publishFolder) {	
	Build-Ui -project $project -outputFolder $publishFolder
}

function Publish-Nuget([String] $project, [String] $version, [String] $publishFolder) {
    # Configure
	Create-Private-NuGet-Config
    $fileVersion = Get-FileVersion -version $version
    $configuration = "Release"

    # Create arguments
    $arguments = @("--configuration", $configuration, "-p:Version=$version", "-p:FileVersion=$fileVersion")

    if ($publishFolder) {
        $arguments +=@("--output", $publishFolder)
    }

    Write-Host -ForegroundColor Green "dotnet pack $project $arguments"
    & dotnet pack $project $arguments
}

function Push-Nuget([String] $publishFolder, [String] $serverUrl, [String] $apiKey) {
    # Create arguments
    $arguments = @()

    if ($serverUrl) {
        $arguments += @("--source", $serverUrl);
    }

    if ($apiKey) {
        $arguments += @("--api-key", $apiKey);
    }

    Write-Host -ForegroundColor Green "dotnet nuget push $publishFolder/*.nupkg $arguments"
    & dotnet nuget push $publishFolder/*.nupkg $arguments
}

function Publish-Msi([String] $project, [String] $version, [String] $framework, [String] $runtime, [String] $publishFolder, [String] $installerProject = $null) {
    # Publish project
    Publish-Project -project $project -version $version -framework $framework -runtime $runtime -publishFolder $publishFolder
    
    if (-not $installerProject) {
        $p = Resolve-Path $project 
        $projectPath = Resolve-Path $project | Select-Object -ExpandProperty Path | Split-Path -Leaf
        $installerProject = "${projectPath}.Installer"
    }
    
    # Publish installer project
    Write-Host -ForegroundColor Green "dotnet build $installerProject --configuration Release -p:WixSharpBuild=false"
    & dotnet build $installerProject --configuration Release -p:WixSharpBuild=false
    if(!$?) {
        exit $LASTEXITCODE
    }

    # Excecute installer to build MSI
    $installerExe = Join-Path $installerProject "bin\Release\net48\${installerProject}.exe"
    $fileVersion = Get-FileVersion -version $version
    $msiOutputPath = "${publishFolder}.msi"
    
    Write-Host -ForegroundColor Green "$installerExe -version "$fileVersion" -publish-folder "$publishFolder" -output "$msiOutputPath""
    & $installerExe -version "$fileVersion" -publish-folder "$publishFolder" -output "$msiOutputPath"
    if(!$?) {
        exit $LASTEXITCODE
    }
	
	Remove-Item -Path "$publishFolder" -Recurse -Force
    
    return $msiOutputPath
}

function Publish-Msix([String] $project, [String] $version, [String] $framework, [String] $runtime, [String] $publishFolder) {
    Publish-Project -project $projectPath -version $version -framework $framework -runtime $runtime -publishFolder $publishFolder -compilerSwitches @("-p:WindowsPackageType=MSIX")
                                                       
    $fileVersion = Get-FileVersion -version $version
    
    $manifestFile = Join-Path $publishFolder "AppxManifest.xml"
    $msixVersion = Get-MsixVersion -fileVersion $fileVersion
    $manifestContent = Get-Content $manifestFile -Encoding UTF8
    $manifestContent = $manifestContent -replace '(?i)\bVersion\s*=\s*"\d+(?:\.\d+){2,3}"', "Version=""$msixVersion"""
    [IO.File]::WriteAllLines($manifestFile, $manifestContent)

    $makeAppxPath = Get-MakeAppxPath
    $msixOutputPath = "$publishFolder.msix"
    $makeAppxArgs = @("pack", "/d", $publishFolder, "/p", $msixOutputPath, "/o")
    & $makeAppxPath $makeAppxArgs
    if(!$?) {
        exit $LASTEXITCODE
    }

    # Clean up temp folder
    Remove-Item -Path $publishFolder -Recurse -Force

    Write-Host -ForegroundColor Green "MSIX package created: $msixOutputPath"
    return $msixOutputPath
}

#endregion

function Create-Private-NuGet-Config {
	# Copy private configuration holding private repository secrets
	if ($NUGET_CONFIG) {
		Copy-Item $NUGET_CONFIG /nuget.config -Force
	}
}

function Get-FileVersion([String] $version) {
    return $version -replace '(\d+(?:\.\d+)*)(.*)', '$1'
}

function Get-MsixVersion([String] $fileVersion) {
    
    # MSIX version format: major.minor.build.revision (4 parts)
    if ($fileVersion.Split('.').Count -eq 4) {
        return $fileVersion
    }
    
    $parts = $fileVersion.Split('.')
    while ($parts.Count -lt 4) {
        $parts += "0"
    }

    $msixVersion = $parts -join "."
    return $msixVersion
}

function Get-MakeAppxPath {
    # Find MakeAppx.exe (usually in Windows SDK)
    $makeAppxPath = $null
    $sdkPaths = @(
        "${env:ProgramFiles(x86)}\Windows Kits\10\App Certification Kit\makeappx.exe",
        "${env:ProgramFiles(x86)}\Windows Kits\10\bin\*\x64\makeappx.exe",
        "${env:ProgramFiles}\Windows Kits\10\bin\*\x64\makeappx.exe"
    )
    
    foreach ($path in $sdkPaths) {
        $found = Get-ChildItem -Path $path -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($found) {
            $makeAppxPath = $found.FullName
            break
        }
    }

    if (!$makeAppxPath) {
        Write-Host -ForegroundColor Red "Error: MakeAppx.exe not found. Please install Windows SDK."
        Write-Host -ForegroundColor Yellow "Download from: https://developer.microsoft.com/en-us/windows/downloads/windows-sdk/"
        exit 1
    }

    return $makeAppxPath
}