# Fluent RDP

Modern RDP client with live window resizing and zoom (by DPI). Drop-in replacement for `mstsc.exe`.

## Features

- **Dynamic Window Resizing**: Automatically adjusts the remote desktop resolution as you resize the window
- **Zoomable (by DPI)**: Adjustable zoom levels via DPI scaling for better readability
- **RDP File Support**: Load and save connection settings from standard `.rdp` files
- **Command-Line Interface**: Full-featured CLI for automation and scripting
- **Modern UI**: Clean Windows interface with high DPI support

## Installation

### Requirements

- Windows 11, might work under other Windows versions but untested
- .NET 10.0 Runtime (only required for standard versions, not for Full versions)
- Microsoft RDP ActiveX Control (MSTSCLib) - included with Windows

### Pre-built Releases

Download the latest release from the [Releases](https://github.com/fschick/FluentRDP/releases) page. Choose the version that best fits your needs:

#### Installer Versions (MSI)

- **`FluentRDP-X.X.X-FullSetup.msi`** - Full installer with .NET runtime included
  
- **`FluentRDP-X.X.X-Setup.msi`** - Standard installer (requires .NET 10.0)

#### Portable Versions (ZIP)

- **`FluentRDP-X.X.X-Full.zip`** - Portable version with .NET runtime included
  
- **`FluentRDP-X.X.X.zip`** - Standard portable version (requires .NET 10.0)

**Which version should I choose?**

- If you're unsure or don't have .NET installed: Choose the **Full** version (either MSI or ZIP)
- If you already have .NET 10.0 installed: Choose the standard version to save download size
- For easy installation: Use the **MSI** installer
- For portable use: Use the **ZIP** archive

## Usage

For command-line options and usage information, run:

```bash
FluentRDP.exe --help
```

## Development

### Requirements

- .NET 10.0 SDK
- Windows (for Windows Forms development)
- PowerShell (for build scripts)

### Build from Source

1. Clone the repository:

   ```bash
   git clone https://github.com/fschick/FluentRDP.git
   cd FluentRDP
   ```

2. Ensure you have .NET 10.0 SDK installed

3. Build the project:

   ```powershell
   Build/scripts/fluentrdp.build.ps1
   ```

4. Publish (optional):

   ```powershell
   Build/scripts/fluentrdp.publish.ps1 -version 1.0.0
   ```

### Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Acknowledgments

FluentRDP uses the Microsoft RDP ActiveX Control (MSTSCLib) for RDP connectivity.
