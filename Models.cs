using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace HardOptimizer
{
    public enum ThemeMode { Dark = 0, Light = 1 }
    public enum WindowsVersion { Unsupported = 0, Windows7 = 7, Windows8 = 8, Windows10 = 10, Windows11 = 11 }

    public enum LanguageCode
    {
        EN, RU, TR, DE, ES, PT, FR, IT, CN, CZ, TW, KO, PL, AR, KU, HU,
        RO, NL, UA, JA, FA, NE, BG, VN, UR, ID, HR
    }

    public enum StartupItemLocation { LMStartupFolder, CUStartupFolder, HKLM, HKLMWoW, HKCU }
    public enum StartupItemType { None, Run, RunOnce }
    public enum ActivityKind { Created, Modified, Deleted, Renamed }
    public enum RestartType { Normal, SafeMode }

    [Serializable]
    public sealed class Options
    {
        public Color Theme { get; set; } = Color.FromArgb(153, 102, 204);
        public ThemeMode ThemeMode { get; set; } = ThemeMode.Dark;
        public string AppsFolder { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public bool EnableTray { get; set; } = false;
        public bool AutoStart { get; set; } = false;
        public string InternalDNS { get; set; } = "1.1.1.1";
        public bool UpdateOnLaunch { get; set; } = true;
        public bool DisableIndicium { get; set; } = false;
        public bool DisableAppsTool { get; set; } = false;
        public bool DisableHostsEditor { get; set; } = false;
        public bool DisableUWPApps { get; set; } = false;
        public bool DisableStartupTool { get; set; } = false;
        public bool DisableCleaner { get; set; } = false;
        public bool DisableIntegrator { get; set; } = false;
        public bool DisablePinger { get; set; } = false;
        public LanguageCode LanguageCode { get; set; } = LanguageCode.EN;
        public bool EnablePerformanceTweaks { get; set; } = false;
        public bool DisableNetworkThrottling { get; set; } = false;
        public bool DisableWindowsDefender { get; set; } = false;
        public bool DisableSystemRestore { get; set; } = false;
        public bool DisablePrintService { get; set; } = false;
        public bool DisableMediaPlayerSharing { get; set; } = false;
        public bool DisableErrorReporting { get; set; } = false;
        public bool DisableHomeGroup { get; set; } = false;
        public bool DisableSuperfetch { get; set; } = false;
        public bool DisableTelemetryTasks { get; set; } = false;
        public bool DisableCompatibilityAssistant { get; set; } = false;
        public bool DisableFaxService { get; set; } = false;
        public bool DisableSmartScreen { get; set; } = false;
        public bool DisableCloudClipboard { get; set; } = false;
        public bool DisableStickyKeys { get; set; } = false;
        public bool DisableHibernation { get; set; } = false;
        public bool DisableSMB1 { get; set; } = false;
        public bool DisableSMB2 { get; set; } = false;
        public bool DisableNTFSTimeStamp { get; set; } = false;
        public bool DisableSearch { get; set; } = false;
        public bool EnableUtcTime { get; set; } = false;
        public bool ShowAllTrayIcons { get; set; } = false;
        public bool RemoveMenusDelay { get; set; } = false;
        public bool DisableOffice2016Telemetry { get; set; } = false;
        public bool DisableVisualStudioTelemetry { get; set; } = false;
        public bool DisableFirefoxTelemetry { get; set; } = false;
        public bool DisableChromeTelemetry { get; set; } = false;
        public bool DisableNVIDIATelemetry { get; set; } = false;
        public bool DisableEdgeDiscoverBar { get; set; } = false;
        public bool DisableEdgeTelemetry { get; set; } = false;
        public bool DisableOneDrive { get; set; } = false;
        public bool EnableLegacyVolumeSlider { get; set; } = false;
        public bool DisableQuickAccessHistory { get; set; } = false;
        public bool DisableStartMenuAds { get; set; } = false;
        public bool UninstallOneDrive { get; set; } = false;
        public bool DisableMyPeople { get; set; } = false;
        public bool DisableAutomaticUpdates { get; set; } = false;
        public bool ExcludeDrivers { get; set; } = false;
        public bool DisableTelemetryServices { get; set; } = false;
        public bool DisablePrivacyOptions { get; set; } = false;
        public bool DisableCortana { get; set; } = false;
        public bool DisableSensorServices { get; set; } = false;
        public bool DisableWindowsInk { get; set; } = false;
        public bool DisableSpellingTyping { get; set; } = false;
        public bool DisableXboxLive { get; set; } = false;
        public bool DisableGameBar { get; set; } = false;
        public bool DisableInsiderService { get; set; } = false;
        public bool DisableStoreUpdates { get; set; } = false;
        public bool EnableLongPaths { get; set; } = false;
        public bool RemoveCastToDevice { get; set; } = false;
        public bool EnableGamingMode { get; set; } = false;
        public bool RestoreClassicPhotoViewer { get; set; } = false;
        public bool DisableModernStandby { get; set; } = false;
        public bool HideTaskbarWeather { get; set; } = false;
        public bool HideTaskbarSearch { get; set; } = false;
        public bool DisableNewsInterests { get; set; } = false;
        public bool TaskbarToLeft { get; set; } = false;
        public bool DisableSnapAssist { get; set; } = false;
        public bool DisableWidgets { get; set; } = false;
        public bool DisableChat { get; set; } = false;
        public bool ClassicMenu { get; set; } = false;
        public bool DisableTPMCheck { get; set; } = false;
        public bool CompactMode { get; set; } = false;
        public bool DisableStickers { get; set; } = false;
        public bool DisableVBS { get; set; } = false;
        public bool DisableCoPilotAI { get; set; } = false;
        public bool DisableHPET { get; set; } = false;
        public bool EnableLoginVerbose { get; set; } = false;
        public bool EnableRegistryBackups { get; set; } = false;
        public string ObserverPath { get; set; } = "";
        public bool? ObserverCreated { get; set; } = null;
        public bool? ObserverModified { get; set; } = null;
        public bool? ObserverDeleted { get; set; } = null;
        public bool? ObserverRenamed { get; set; } = null;
        public bool? ObserverExclusionsEnabled { get; set; } = null;
        public List<string> ObserverExclusions { get; set; } = new List<string>();
    }

    public sealed class UwpApp
    {
        public string Name { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string InstallLocation { get; set; } = "";
        public bool IsFramework { get; set; } = false;
        public bool IsRemovable { get; set; } = true;
        public List<string> FullNames { get; } = new List<string>();
        public Image Logo { get; set; }
        public string Title => string.IsNullOrWhiteSpace(DisplayName) ? Name : DisplayName;
    }

    public sealed class AppInfo
    {
        public string Title { get; set; } = "";
        public string Link64 { get; set; } = "";
        public string Link { get; set; } = "";
        public string Tag { get; set; } = "";
        public string Image { get; set; } = "";
        public string Group { get; set; } = "";
    }

    internal class StartupItem
    {
        public string Name { get; set; } = "";
        public string FileLocation { get; set; } = "";
        public StartupItemLocation RegistryLocation { get; set; }
        public StartupItemType StartupType { get; set; }
        public bool CanToggle { get; set; }
        public bool Enabled { get; set; } = true;
        public virtual void Remove() { }
        public virtual void LocateFile() { }
        public virtual void LocateKey() { }
    }

    internal sealed class FolderStartupItem : StartupItem
    {
        public string Shortcut { get; set; } = "";
        public override void Remove()
        {
            try { if (File.Exists(Shortcut)) File.Delete(Shortcut); } catch { }
        }
        public override void LocateFile() { Utilities.FindFile(FileLocation); }
    }

    internal sealed class RegistryStartupItem : StartupItem
    {
        public Microsoft.Win32.RegistryKey Key { get; set; }
        public override void LocateKey() { Utilities.FindKeyInRegistry(Key?.ToString() ?? ""); }
        public override void Remove()
        {
            try { Key?.DeleteValue(Name, false); } catch { }
        }
        public override void LocateFile() { Utilities.FindFile(SanitizePath(FileLocation)); }
        private string SanitizePath(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            s = s.Replace("\"", "");
            while (s.Contains("/")) { var i = s.LastIndexOf('/'); s = s.Substring(0, i); }
            var exeIndex = s.IndexOf(".exe", StringComparison.OrdinalIgnoreCase);
            return exeIndex >= 0 ? s.Substring(0, exeIndex + 4).Trim() : s.Trim();
        }
    }

    internal sealed class ActivityEntry
    {
        public string Path { get; set; } = "";
        public string OldPath { get; set; }
        public ActivityKind Kind { get; set; }
        public int Count { get; set; }
        public DateTime LastSeen { get; set; }
    }

    internal sealed class HardwareInfo
    {
        public string CpuName { get; set; } = "";
        public string TotalRam { get; set; } = "";
        public string GpuName { get; set; } = "";
        public string DiskModel { get; set; } = "";
        public string MotherboardModel { get; set; } = "";
        public string BiosVersion { get; set; } = "";
    }
}
