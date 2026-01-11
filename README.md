# FluentRDP

Modern RDP client with live window resizing capability. Drop-in replacement for `mstsc.exe`.

## Features

- **Dynamic Window Resizing**: Automatically adjusts the remote desktop resolution as you resize the window
- **RDP File Support**: Load and save connection settings from standard `.rdp` files
- **Command-Line Interface**: Full-featured CLI for automation and scripting
- **Modern UI**: Clean Windows interface with high DPI support

## Requirements

- Windows 11, might work under other Windows versions but untested
- .NET 10.0 Runtime (for framework-dependent builds)
- Microsoft RDP ActiveX Control (MSTSCLib) - included with Windows

## Installation

### Pre-built Releases

Download the latest release from the [Releases](https://github.com/fschick/FluentRDP/releases) page:

- **Self-Contained**: Includes .NET runtime (larger download, no installation required)
- **Framework-Dependent**: Requires .NET 10.0 runtime installed (smaller download)

Extract the ZIP file and run `FluentRDP.exe`.Usage

### Command-Line

```bash
FluentRDP.exe [options] [hostname|rdp-file]
```

#### Basic Examples

```bash
# Connect to a hostname
FluentRDP.exe myserver.example.com

# Connect with username
FluentRDP.exe -h myserver.example.com -u Administrator

# Load from RDP file
FluentRDP.exe connection.rdp

# Connect with custom resolution
FluentRDP.exe -h myserver.example.com --width 1920 --height 1080
```

#### Common Options

**Connection:**
- `-h, --host, --hostname <host>` - Hostname or IP (supports IPv4/IPv6, port: `host:port`, `[IPv6]:port`)
- `-u, --user, --username <user>` - Username (supports: `User`, `DOMAIN\user`, `user@domain`)
- `-d, --domain <domain>` - Domain for authentication
- `--password <password>` - Password (not recommended for security)
- `--connection-timeout <sec>` - Connection timeout in seconds

**Display:**
- `--width <pixels>` - Desktop width
- `--height <pixels>` - Desktop height
- `--color-depth <depth>` - Color depth: 8, 15, 16, 24, 32 (default: 32)
- `--scale, --scale-factor <pct>` - DPI scale: 100, 125, 150, 200, etc. (0 = auto, default: 0)

**Security:**
- `--auth-level <level>` - Server auth level: `no-warning`, `no-connect`, `warn`, `none` (default: none)
- `--enable-credssp, --credssp` - Enable CredSSP authentication
- `--keep-alive-interval <ms>` - Keep-alive interval in milliseconds
- `--max-reconnect-attempts <num>` - Max automatic reconnection attempts

**RDP File:**
- `--rdp, --rdp-file <path>` - Load settings from RDP file (command line options override file settings)

For a complete list of options, run:
```bash
FluentRDP.exe --help
```

### Development Requirements

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

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Acknowledgments

FluentRDP uses the Microsoft RDP ActiveX Control (MSTSCLib) for RDP connectivity.
