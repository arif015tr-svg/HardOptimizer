# Changelog

All notable changes to HardOptimizer are documented in this file.

## [18.0.0] - 2026-11-16

### First public release

**HardOptimizer v18.0** — Windows privacy, performance and gaming utility (.NET Framework 4.8 WinForms).

### Features

#### System tweaks
- Performance (AutoEndTasks, HungAppTimeout, WaitToKillAppTimeout)
- Disable Superfetch / SysMain
- Disable Error Reporting, SmartScreen, Hibernation, SMB1
- UTC time, menu delay removal
- Telemetry task disable, Cortana, Xbox Live, Game Bar
- Windows 11: Widgets, Copilot, Snap Assist

#### Cleaner
- Temp files, minidumps, recycle bin
- Browser cache: Chrome, Edge, Firefox, Brave
- Size preview before clean

#### Network
- Hosts file editor
- DNS: Cloudflare, Google, OpenDNS, Quad9
- DNS flush

#### Gaming / FPS
- Network throttling, TCP/Nagle related tweaks
- GPU priority, SysMain, GameDVR, visual effects

#### Other
- Hardware info (WMI: CPU, RAM, GPU, disk, motherboard, BIOS)
- Non-Microsoft service list / stop
- UWP app list, startup items
- Silent mode: `HardOptimizer.exe /silent`
- EN / TR strings, dark Fluent UI
- MIT License
- Startup system restore point (WMI `root\\default` / SystemRestore)

### Requirements
- Windows 7 / 8 / 10 / 11
- .NET Framework 4.8
- Administrator rights

### Build
```bash
msbuild Optimizer.csproj /p:Configuration=Release
```
Output: `bin\\release\\HardOptimizer.exe`

### Source
Full C# source is in this repository (`main` branch).

### Notes
- Creating a restore point requires System Restore enabled.
- Disabling Defender / SmartScreen reduces security — use at your own risk.
