# HardOptimizer v18.0

**Advanced Windows Privacy, Performance & Security Configuration Utility**

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-blue.svg)](https://dotnet.microsoft.com/download/dotnet-framework)
[![Platform](https://img.shields.io/badge/Platform-Windows%207%2B-brightgreen.svg)](https://www.microsoft.com/windows)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Language](https://img.shields.io/badge/Language-C%23-purple.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Release](https://img.shields.io/badge/release-v18.0.0-brightgreen.svg)](CHANGELOG.md)

> Modern, lightweight Windows optimizer focused on privacy, performance, gaming and system cleaning. Built with Fluent Design UI.

---

## Features

### System Tweaks
- Performance optimizations, Superfetch, Error Reporting, SmartScreen, Hibernation, SMB1
- UTC time, menu delay, telemetry tasks, Cortana, Xbox Live, Game Bar
- Windows 11: Widgets, Copilot, Snap Assist

### Cleaner
- Temp, minidumps, recycle bin
- Browser cache (Chrome, Edge, Firefox, Brave)

### Network
- Hosts editor, DNS (Cloudflare / Google / OpenDNS / Quad9), DNS flush

### Gaming / FPS
- Network throttling, GPU priority, SysMain, GameDVR, visual effects

### Other
- Hardware info (WMI), non-Microsoft services, UWP list, startup items
- Silent mode (`/silent`), EN/TR, dark Fluent theme

---

## Releases

### [v18.0.0](CHANGELOG.md) — 2026-11-16 (First public release)

| Item | Detail |
|------|--------|
| **Type** | Source release (build from repo) |
| **Tag** | `v18.0.0` |
| **License** | MIT |
| **Code** | Full C# sources on `main` |

**Included in this release**
- Complete WinForms app source (MainForm, helpers, theme, i18n)
- Fixed WMI system restore point (`root\\default` / `SystemRestore`)
- Service start-type WMI with `ReturnValue` check
- Cleaner with size preview, gaming apply-all, tweaks page

**How to get the code**
```bash
git clone https://github.com/arif015tr-svg/HardOptimizer.git
cd HardOptimizer
msbuild Optimizer.csproj /p:Configuration=Release
```

**GitHub Release page:** create tag `v18.0.0` and use [CHANGELOG.md](CHANGELOG.md) as the release body.  
Binary `.exe` is not attached in this source-only release — build locally after clone.

Full changelog: **[CHANGELOG.md](CHANGELOG.md)**

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

Output: `bin\\release\\HardOptimizer.exe`

---

## Usage

```bash
HardOptimizer.exe          # GUI
HardOptimizer.exe /silent  # Apply saved settings and exit
```

---

## Safety

- App tries to create a System Restore point on startup (needs Restore enabled).
- Disabling security features reduces protection — use at your own risk.

---

## License

MIT License — see [LICENSE](LICENSE).

---

## Author

**arif015tr-svg** · https://github.com/arif015tr-svg/HardOptimizer
