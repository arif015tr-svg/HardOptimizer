# HardOptimizer v18.0

**Advanced Windows Privacy, Performance & Security Configuration Utility**

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-blue.svg)](https://dotnet.microsoft.com/download/dotnet-framework)
[![Platform](https://img.shields.io/badge/Platform-Windows%207%2B-brightgreen.svg)](https://www.microsoft.com/windows)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Language](https://img.shields.io/badge/Language-C%23-purple.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)

> Modern, lightweight Windows optimizer focused on privacy, performance, gaming and system cleaning. Built with Fluent Design UI.

---

## Features

### System Tweaks
- Performance optimizations (AutoEndTasks, HungAppTimeout, etc.)
- Disable Superfetch / SysMain
- Disable Windows Error Reporting
- Disable SmartScreen
- Disable Hibernation
- Disable SMB1
- Enable UTC Time
- Remove Menu Delay
- Disable Telemetry Tasks
- Disable Cortana
- Disable Xbox Live / Game Bar
- Windows 11 tweaks (Widgets, Copilot, Snap Assist)

### Cleaner
- Temp files cleaning
- Minidumps cleaning
- Recycle Bin emptying
- Browser cache (Chrome, Edge, Firefox, Brave)

### Apps & Startup
- List and manage UWP / Microsoft Store apps
- Startup items manager (Registry + Startup folders)

### Network Tools
- Hosts file editor
- Quick DNS switcher (Cloudflare, Google, OpenDNS, Quad9)
- DNS cache flush
- Ping test

### Gaming / FPS Optimizations
- Nagle's Algorithm disable
- Network throttling disable
- GPU Priority increase
- SysMain disable
- Visual Effects optimization
- GameDVR disable
- Priority Separation tweaks

### Other Tools
- Hardware information (CPU, RAM, GPU, Disk, Motherboard, BIOS)
- Non-Microsoft Service Manager
- File Handle Unlocker
- System Integrator (custom commands, PATH variables)
- Silent mode (`/silent`)
- Multi-language (English + Turkish)
- Dark / Light Fluent theme

---

## Screenshots

> Screenshots will be added after first release. You can contribute by opening an issue with screenshots.

| Dashboard / Tweaks | Cleaner | Gaming |
|:------------------:|:-------:|:------:|
| *Coming soon* | *Coming soon* | *Coming soon* |

---

## Requirements

- Windows 7 / 8 / 10 / 11 (x64 recommended)
- .NET Framework 4.8
- Administrator privileges (required for system changes)

---

## Build

### Prerequisites
- Visual Studio 2019 / 2022 (or Build Tools)
- .NET Framework 4.8 Developer Pack

### Build Commands

```bash
# Using MSBuild
msbuild Optimizer.csproj /p:Configuration=Release

# Or open in Visual Studio and build
```

Output will be in `bin\release\HardOptimizer.exe`

---

## Usage

1. **Run as Administrator** (required)
2. Use the sidebar to navigate between tools
3. Toggle options and click Apply / Clean

### Command Line

```bash
HardOptimizer.exe          # Normal GUI mode
HardOptimizer.exe /silent  # Apply saved settings silently and exit
HardOptimizer.exe /unsafe  # Unsafe mode (if implemented)
```

---

## Project Structure

```
HardOptimizer/
├── Program.cs              # Entry point, admin check, restore point
├── MainForm.cs             # Main UI and page builders
├── Models.cs               # Options, UwpApp, StartupItem, HardwareInfo...
├── OptimizeHelper.cs       # Registry & system tweaks
├── CleanHelper.cs          # Temp, recycle, browser cache
├── ServiceHelper.cs        # Non-Microsoft services
├── StartupHelper.cs        # Startup items
├── HostsHelper.cs          # hosts file
├── PingerHelper.cs         # DNS & ping
├── UWPHelper.cs            # Store apps (PowerShell)
├── HardwareHelper.cs       # WMI hardware info
├── FileHandleHelper.cs     # Restart Manager API
├── IntegratorHelper.cs     # Custom commands & PATH
├── ThemeManager.cs         # Dark/Light palette
├── CustomControls.cs       # FluentButton, MoonToggle
├── OptionsHelper.cs        # Settings load/save + i18n
├── Utilities.cs            # Common helpers
├── Logger.cs               # File logging
├── SilentOps.cs            # Silent mode actions
├── Properties/             # AssemblyInfo, Resources
└── Resources/
    ├── i18n/               # EN.json, TR.json
    └── Scripts/            # DisableTelemetryTasks.bat
```

---

## Safety Notes

- Always create a **System Restore Point** before applying heavy tweaks (the app tries to create one on startup).
- Disabling Windows Defender / SmartScreen reduces security.
- Some changes require a reboot to take full effect.
- Use at your own risk.

---

## License

MIT License – free for personal and commercial use.

---

## Author

**arif015tr-svg**

Repository: https://github.com/arif015tr-svg/HardOptimizer

---

*HardOptimizer – Make Windows yours.*
