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
- Temp files, minidumps, recycle bin
- Browser cache (Chrome, Edge, Firefox, Brave)

### Apps & Startup
- UWP / Store apps list
- Startup items manager

### Network Tools
- Hosts file editor
- DNS switcher (Cloudflare, Google, OpenDNS, Quad9)
- DNS cache flush

### Gaming / FPS
- Network throttling, GPU priority, SysMain, GameDVR, visual effects

### Other
- Hardware info, non-Microsoft services, silent mode (`/silent`)
- English + Turkish, dark Fluent theme

---

## Screenshots

Simple dark UI with left sidebar. Pages: Tweaks, Cleaner, Apps, Startup, Hosts, DNS, Hardware, Services, Gaming, Options.

---

## Requirements

- Windows 7 / 8 / 10 / 11
- .NET Framework 4.8
- Run as Administrator

---

## Build

```bash
msbuild Optimizer.csproj /p:Configuration=Release
```

Output: `bin\release\HardOptimizer.exe`

---

## Usage

```bash
HardOptimizer.exe          # GUI
HardOptimizer.exe /silent  # Apply saved settings and exit
```

---

## License

MIT License – see [LICENSE](LICENSE).

---

## Author

**arif015tr-svg** · https://github.com/arif015tr-svg/HardOptimizer
