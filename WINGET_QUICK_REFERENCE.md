# Winget Publishing - Quick Reference

## Quick Start

### Generate All Manifest Files During MSI Publish

```powershell
Build\scripts\fluentrdp.publish.msi.ps1 -version "1.0.0" -generateWingetManifest -releaseUrl "https://github.com/fschick/FluentRDP/releases/download/v1.0.0/FluentRDP-win-x64.msi"
```

This will:
1. Build and publish the MSI
2. Generate installer manifest
3. Generate locale manifest
4. Generate version manifest

### Generate Manifest Files Separately

```powershell
# Step 1: Publish MSI
Build\scripts\fluentrdp.publish.msi.ps1 -version "1.0.0"

# Step 2: Generate all winget manifests
Build\scripts\fluentrdp.publish.winget.ps1 `
    -version "1.0.0" `
    -msiPath "path\to\FluentRDP-win-x64.msi" `
    -releaseUrl "https://github.com/fschick/FluentRDP/releases/download/v1.0.0/FluentRDP-win-x64.msi"
```

### Generate Only Installer Manifest

```powershell
Build\scripts\fluentrdp.generate-winget-manifest.ps1 `
    -version "1.0.0" `
    -msiPath "path\to\FluentRDP-win-x64.msi" `
    -releaseUrl "https://github.com/fschick/FluentRDP/releases/download/v1.0.0/FluentRDP-win-x64.msi"
```

### Generate Only Locale Manifests

```powershell
Build\scripts\fluentrdp.generate-winget-locale-manifests.ps1 -version "1.0.0"
```

## Manifest File Structure

After generation, you'll have:
```
winget-manifest/
└── SchickSoftware/
    └── FluentRDP/
        └── 1.0.0/
            ├── SchickSoftware.FluentRDP.installer.yaml  (installer manifest)
            ├── SchickSoftware.FluentRDP.locale.en-US.yaml  (locale manifest)
            └── SchickSoftware.FluentRDP.yaml  (version manifest)
```

## Testing Locally

```powershell
# Test install
winget install --manifest "winget-manifest\SchickSoftware\FluentRDP\1.0.0"

# Test uninstall
winget uninstall FluentRDP
```

## Next Steps

1. Review generated manifests
2. Create GitHub release and upload MSI
3. Copy manifests to winget-pkgs repository
4. Submit PR to https://github.com/microsoft/winget-pkgs

See `WINGET_PUBLISH_GUIDE.md` for detailed instructions.
