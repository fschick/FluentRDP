# FluentRDP

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
- .NET 10.0 Runtime (for framework-dependent builds)
- Microsoft RDP ActiveX Control (MSTSCLib) - included with Windows

### Pre-built Releases

Download the latest release from the [Releases](https://github.com/fschick/FluentRDP/releases) page:

- **Self-Contained**: Includes .NET runtime (larger download, no installation required)
- **Framework-Dependent**: Requires .NET 10.0 runtime installed (smaller download)

Extract the ZIP file and run `FluentRDP.exe`.

### Usage

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
