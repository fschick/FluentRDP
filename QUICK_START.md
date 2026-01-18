# Quick Start: Code Signing & WinGet Setup

This is a quick reference for the code signing and WinGet distribution setup. For detailed instructions, see [SIGNING_AND_WINGET_SETUP.md](SIGNING_AND_WINGET_SETUP.md).

## What Was Added

### Files Created

1. **`.github/workflows/build-and-sign.yml`** - GitHub Actions workflow for automated building and signing
2. **`Build/scripts/fluentrdp.generate-winget-manifest.ps1`** - Script to generate WinGet manifests
3. **`.signpath/policies/fluentrdp/signing-policy.yml`** - SignPath signing policy configuration
4. **`SIGNING_AND_WINGET_SETUP.md`** - Complete step-by-step setup guide

### Files Modified

1. **`Build/scripts/_core.ps1`** - Added `Sign-File` function for code signing support

## Quick Setup Checklist

### 1. SignPath.io Setup (5-10 minutes)

- [ ] Apply for free OSS subscription at https://signpath.org/apply.html
- [ ] Create project in SignPath.io dashboard
- [ ] Generate API token
- [ ] Add GitHub secrets:
  - `SIGNPATH_ORGANIZATION_ID`
  - `SIGNPATH_PROJECT_SLUG` (e.g., `fluentrdp`)
  - `SIGNPATH_POLICY_SLUG` (e.g., `signing-policy`)
  - `SIGNPATH_API_TOKEN`

### 2. Test Build (5 minutes)

```bash
# Create a test tag
git tag v1.0.0
git push origin v1.0.0
```

- [ ] Monitor GitHub Actions workflow
- [ ] Approve signing request in SignPath.io (you'll get an email)
- [ ] Verify release is created with signed MSI

### 3. WinGet Submission (10-15 minutes)

- [ ] Fork https://github.com/microsoft/winget-pkgs
- [ ] Copy generated manifest from `WinGet/manifests/` to your fork
- [ ] Validate: `winget validate <manifest-path>`
- [ ] Create PR to microsoft/winget-pkgs

## Workflow Overview

```
Git Tag Push
    ↓
GitHub Actions Build
    ↓
MSI Created
    ↓
Submit to SignPath.io
    ↓
[Manual Approval Required]
    ↓
Signed MSI Downloaded
    ↓
GitHub Release Created
    ↓
WinGet Manifest Generated
```

## Key Commands

### Build Locally
```powershell
.\Build\scripts\fluentrdp.build.ps1 -version 1.0.0
.\Build\scripts\fluentrdp.publish.msi.ps1 -version 1.0.0 -runtime win-x64
```

### Generate WinGet Manifest
```powershell
.\Build\scripts\fluentrdp.generate-winget-manifest.ps1 -version 1.0.0 -sha256 <hash> -msiPath <path-to-msi>
```

### Validate WinGet Manifest
```powershell
winget validate WinGet\manifests\s\SchickSoftwareEntwicklung\FluentRDP\1.0.0\FluentRDP.yaml
```

## Troubleshooting

**Workflow fails at signing step?**
- Check GitHub secrets are set correctly
- Verify SignPath project and policy slugs match
- Check SignPath.io dashboard for error messages

**WinGet manifest validation fails?**
- Ensure all required fields are present
- Check YAML syntax (no tabs, proper indentation)
- Verify SHA256 hash matches the signed MSI

**MSI not signed?**
- Check SignPath approval was completed
- Verify artifact configuration in SignPath matches file pattern `*.msi`

## Next Steps

1. Complete SignPath.io application and setup
2. Test the workflow with a release tag
3. Submit first WinGet manifest
4. Document your code signing policy (add to README)

For detailed instructions, see [SIGNING_AND_WINGET_SETUP.md](SIGNING_AND_WINGET_SETUP.md).
