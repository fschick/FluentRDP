# Code Signing and WinGet Distribution Setup Guide

This guide provides step-by-step instructions for setting up code signing via SignPath.io and distributing FluentRDP via Windows Package Manager (WinGet).

## Overview

This project uses:
- **SignPath.io** for free code signing of open source projects
- **GitHub Actions** for automated builds and signing
- **WinGet** for easy distribution via Windows Package Manager

## Prerequisites

- GitHub repository with Actions enabled
- SignPath.io account (free for open source projects)
- Windows development environment (for local testing)

---

## Part 1: SignPath.io Setup

### Step 1: Apply for SignPath Open Source Subscription

1. Visit [SignPath Foundation Application Page](https://signpath.org/apply.html)
2. Fill out the application form with:
   - Project name: **FluentRDP**
   - Repository URL: `https://github.com/fschick/FluentRDP`
   - License: **GPL-3.0**
   - Description: Brief description of your project
3. Wait for approval (usually within a few days)

### Step 2: Create SignPath Project

1. Log in to [SignPath.io](https://signpath.io)
2. Create a new project:
   - **Project Name**: FluentRDP
   - **Project Slug**: `fluentrdp` (will be used in configuration)
   - **Repository URL**: `https://github.com/fschick/FluentRDP`
3. Note your **Organization ID** (found in account settings)

### Step 3: Configure Signing Policy

1. In your SignPath project, create a signing policy:
   - **Policy Slug**: `signing-policy` (or your preferred name)
   - **Allowed Sources**: GitHub Workflow
     - Repository: `fschick/FluentRDP`
     - Workflow: `build-and-sign.yml`
     - Branches: `main`, `refs/tags/v*`
   - **Artifact Configuration**: Create one for MSI files
     - **Slug**: `msi`
     - **File Patterns**: `*.msi`
     - **Signing Method**: Code Signing
     - **Timestamping**: Enabled
     - **Timestamp Server**: `http://timestamp.digicert.com`

2. The policy file is already created in `.signpath/policies/fluentrdp/signing-policy.yml` - you can reference this structure

### Step 4: Generate API Token

1. In SignPath, go to **Settings** → **API Tokens**
2. Create a new API token with permissions:
   - Submit signing requests
   - Read project information
3. **Save the token securely** - you'll need it for GitHub secrets

### Step 5: Configure GitHub Secrets

1. Go to your GitHub repository: `Settings` → `Secrets and variables` → `Actions`
2. Add the following secrets:
   - `SIGNPATH_ORGANIZATION_ID`: Your SignPath organization ID
   - `SIGNPATH_PROJECT_SLUG`: `fluentrdp`
   - `SIGNPATH_POLICY_SLUG`: `signing-policy` (or your policy slug)
   - `SIGNPATH_API_TOKEN`: Your SignPath API token

---

## Part 2: GitHub Actions Workflow

The workflow file `.github/workflows/build-and-sign.yml` is already configured. It will:

1. Build the project when a tag is pushed (format: `v1.0.0`)
2. Create an MSI installer
3. Submit the MSI to SignPath for signing
4. Wait for manual approval (you'll receive an email)
5. Download the signed MSI
6. Create a GitHub Release with the signed MSI
7. Generate WinGet manifest files

### Testing the Workflow

1. Create a test tag:
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

2. Monitor the workflow in GitHub Actions tab
3. When signing request is submitted, approve it in SignPath.io dashboard
4. The workflow will complete and create a release

---

## Part 3: WinGet Manifest Setup

### Step 1: Validate Generated Manifest

After a successful build, the workflow generates WinGet manifests in `WinGet/manifests/`. Validate them:

```powershell
# Install wingetcreate if not already installed
winget install Microsoft.WingetCreate

# Validate the manifest
winget validate WinGet\manifests\s\SchickSoftwareEntwicklung\FluentRDP\<version>\FluentRDP.yaml
```

### Step 2: Test Installation Locally

Before submitting to WinGet, test the manifest locally:

```powershell
# Test install from local manifest
winget install --manifest WinGet\manifests\s\SchickSoftwareEntwicklung\FluentRDP\<version>\FluentRDP.yaml

# Test uninstall
winget uninstall FluentRDP
```

### Step 3: Submit to WinGet Repository

1. **Fork the WinGet repository**:
   - Go to [microsoft/winget-pkgs](https://github.com/microsoft/winget-pkgs)
   - Click "Fork"

2. **Clone your fork**:
   ```bash
   git clone https://github.com/YOUR_USERNAME/winget-pkgs.git
   cd winget-pkgs
   ```

3. **Copy manifest files**:
   - Copy the entire version folder from `WinGet/manifests/` to:
     ```
     manifests/s/SchickSoftwareEntwicklung/FluentRDP/<version>/
     ```
   - The folder structure should be:
     ```
     manifests/
       s/
         SchickSoftwareEntwicklung/
           FluentRDP/
             <version>/
               FluentRDP.yaml
               FluentRDP.installer.yaml
               FluentRDP.locale.en-US.yaml
     ```

4. **Validate before committing**:
   ```powershell
   cd winget-pkgs
   winget validate manifests\s\SchickSoftwareEntwicklung\FluentRDP\<version>\FluentRDP.yaml
   ```

5. **Commit and push**:
   ```bash
   git checkout -b add-fluentrdp-<version>
   git add manifests/s/SchickSoftwareEntwicklung/FluentRDP/
   git commit -m "Add FluentRDP version <version>"
   git push origin add-fluentrdp-<version>
   ```

6. **Create Pull Request**:
   - Go to your fork on GitHub
   - Click "New Pull Request"
   - Fill out the PR template
   - Submit for review

### Step 4: Automated Manifest Updates (Optional)

For future releases, you can automate the PR creation using tools like:
- **GoReleaser** with WinGet support
- **GitHub Actions** to automatically create PRs to winget-pkgs

---

## Part 4: Maintenance

### For Each New Release

1. **Create and push a version tag**:
   ```bash
   git tag v1.1.0
   git push origin v1.1.0
   ```

2. **Monitor GitHub Actions**:
   - Build completes
   - Signing request is submitted
   - **Approve signing in SignPath.io** (you'll receive an email)
   - Release is created with signed MSI

3. **Submit updated WinGet manifest**:
   - The manifest is generated automatically in the workflow
   - Follow steps in "Part 3: WinGet Manifest Setup" to submit the new version

### Verifying Signatures

To verify that files are properly signed:

```powershell
# Check signature of MSI file
Get-AuthenticodeSignature -FilePath "FluentRDP-1.0.0-win-x64.msi"

# Should show Status: Valid
```

---

## Troubleshooting

### SignPath Issues

- **Signing request not appearing**: Check that GitHub secrets are correctly set
- **Approval not working**: Verify you're logged into SignPath with the correct account
- **Artifact not found**: Ensure the MSI file path in the workflow is correct

### WinGet Issues

- **Manifest validation fails**: Check YAML syntax and required fields
- **Installation fails**: Verify the MSI URL is accessible and the SHA256 hash matches
- **PR rejected**: Review WinGet repository requirements and fix any issues

### Build Issues

- **MSI not generated**: Check that WixSharp and all dependencies are installed
- **Version not set**: Ensure version tag follows format `v<version>` (e.g., `v1.0.0`)

---

## Additional Resources

- [SignPath.io Documentation](https://docs.signpath.io/)
- [SignPath Foundation](https://signpath.org/)
- [WinGet Manifest Schema](https://aka.ms/winget-manifest)
- [WinGet Package Repository](https://github.com/microsoft/winget-pkgs)
- [NETworkManager Example](https://github.com/BornToBeRoot/NETworkManager) - Reference implementation

---

## Code Signing Policy

FluentRDP uses free code signing services from SignPath.io and a free code signing certificate provided by the SignPath Foundation to sign all official binaries and installers, ensuring authenticity and integrity.

The binaries and installers are built on GitHub Actions directly from the GitHub repository. After each build, the artifacts are automatically sent to SignPath.io via the GitHub Actions workflow, where they are signed following manual approval by the maintainer. Once signed, the binaries are uploaded to the GitHub releases page.

---

## Support

If you encounter issues:
1. Check the troubleshooting section above
2. Review GitHub Actions logs
3. Check SignPath.io dashboard for signing status
4. Open an issue on GitHub with details
