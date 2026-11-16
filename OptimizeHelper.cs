using Microsoft.Win32;
using System;

namespace HardOptimizer
{
    internal static class OptimizeHelper
    {
        #region General Tweaks

        internal static void EnablePerformanceTweaks()
        {
            try
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoComplete", "Append Completion", "yes", RegistryValueKind.String);
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "AutoEndTasks", "1");
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "1000");
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", "2000");
            }
            catch { }
        }

        internal static void DisableTelemetryTasks()
        {
            try { Utilities.RunCommand("/c schtasks /Change /TN \"\\Microsoft\\Windows\\Application Experience\\Microsoft Compatibility Appraiser\" /Disable"); } catch { }
            try { Utilities.RunCommand("/c schtasks /Change /TN \"\\Microsoft\\Windows\\Customer Experience Improvement Program\\Consolidator\" /Disable"); } catch { }
            try { Utilities.RunCommand("/c schtasks /Change /TN \"\\Microsoft\\Windows\\Customer Experience Improvement Program\\KernelCeipTask\" /Disable"); } catch { }
        }

        internal static void DisableWindowsDefender()
        {
            try { Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiSpyware", 1, RegistryValueKind.DWord); } catch { }
        }

        internal static void DisableSystemRestore()
        {
            try { Utilities.RunCommand("/c vssadmin delete shadows /all /quiet"); } catch { }
        }

        internal static void DisableSuperfetch()
        {
            try { Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnableSuperfetch", 0, RegistryValueKind.DWord); } catch { }
        }

        internal static void DisableErrorReporting()
        {
            try { Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "Disabled", 1, RegistryValueKind.DWord); } catch { }
        }

        internal static void DisableSmartScreen()
        {
            try { Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\System", "EnableSmartScreen", 0, RegistryValueKind.DWord); } catch { }
        }

        internal static void DisableHibernation()
        {
            try { Utilities.RunCommand("/c powercfg -h off"); } catch { }
        }

        internal static void DisableSMB1()
        {
            try { Utilities.RunCommand("/c dism /online /norestart /disable-feature /featurename:SMB1Protocol"); } catch { }
        }

        internal static void EnableUtcTime()
        {
            try { Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation", "RealTimeIsUniversal", 1, RegistryValueKind.DWord); } catch { }
        }

        internal static void RemoveMenusDelay()
        {
            try { Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuShowDelay", "0", RegistryValueKind.String); } catch { }
        }

        #endregion

        #region Windows 10 Tweaks

        internal static void DisableStartMenuAds()
        {
            try { Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0, RegistryValueKind.DWord); } catch { }
        }

        internal static void DisableCortana()
        {
            try { Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana", 0, RegistryValueKind.DWord); } catch { }
        }

        internal static void DisableXboxLive()
        {
            try { Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDevServices", "Enabled", 0, RegistryValueKind.DWord); } catch { }
        }

        internal static void DisableGameBar()
        {
            try { Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0, RegistryValueKind.DWord); } catch { }
        }

        internal static void DisableStoreUpdates()
        {
            try { Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload", 0, RegistryValueKind.DWord); } catch { }
        }

        internal static void UninstallOneDrive()
        {
            try { Utilities.RunCommand("/c %SystemRoot%\\System32\\OneDriveSetup.exe /uninstall"); } catch { }
            try { Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\OneDrive", "DisableFileSyncNGSC", 1, RegistryValueKind.DWord); } catch { }
        }

        internal static void RestoreClassicPhotoViewer()
        {
            Utilities.ImportRegistryScript("Resources\\Scripts\\RestoreClassicPhotoViewer.reg");
        }

        #endregion

        #region Windows 11 Tweaks

        internal static void DisableWidgets()
        {
            try { Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "DisableWidgets", 1, RegistryValueKind.DWord); } catch { }
        }

        internal static void DisableCoPilotAI()
        {
            try { Registry.SetValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\WindowsCopilot", "TurnOffWindowsCopilot", 1, RegistryValueKind.DWord); } catch { }
        }

        internal static void DisableSnapAssist()
        {
            try { Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "SnapAssist", 0, RegistryValueKind.DWord); } catch { }
        }

        #endregion

        #region Advanced Tweaks

        internal static void DisableHPET()
        {
            try { Utilities.RunCommand("/c bcdedit /set disabledticks yes"); } catch { }
        }

        internal static void EnableLoginVerbose()
        {
            try { Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "verbosityStatus", 1, RegistryValueKind.DWord); } catch { }
        }

        internal static void EnableRegistryBackups()
        {
            try { Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "EnablePeriodicBackup", 1, RegistryValueKind.DWord); } catch { }
        }

        #endregion

        #region Gaming / FPS Optimizations (Registry)

        internal static void ApplyGamingOptimizations()
        {
            try
            {
                SetTcpAckFrequency();
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "NetworkThrottlingIndex", 0xFFFFFFFF, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", "GPU Priority", 8, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", "Priority", 6, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 0x00000006, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\GameDVR", "AppCaptureEnabled", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4, RegistryValueKind.DWord);
                Logger.Info("Gaming/FPS registry optimizations applied successfully.");
            }
            catch (Exception ex) { Logger.Log("OptimizeHelper.ApplyGamingOptimizations", ex.Message, ex.StackTrace); }
        }

        internal static void SetTcpAckFrequency()
        {
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces");
                if (key != null)
                {
                    foreach (var subKeyName in key.GetSubKeyNames())
                    {
                        try { using var subKey = key.OpenSubKey(subKeyName, true); subKey?.SetValue("TcpAckFrequency", 1, RegistryValueKind.DWord); subKey?.SetValue("TCPNoDelay", 1, RegistryValueKind.DWord); } catch { }
                    }
                }
            }
            catch { }
        }

        internal static void DisableVisualEffects()
        {
            try { Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 2, RegistryValueKind.DWord); } catch { }
        }

        internal static void OptimizeNetworkForGaming()
        {
            try
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "Tcp1323Opts", 1, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "SackOpts", 1, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpWindowSize", 262144, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "DefaultTTL", 64, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "DisableTaskOffload", 1, RegistryValueKind.DWord);
            }
            catch { }
        }

        internal static void DisableChromeTelemetry()
        {
            try
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "MetricsReportingEnabled", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome", "ChromeCleanupEnabled", 0, RegistryValueKind.DWord);
            }
            catch { }
        }

        internal static void DisableEdgeTelemetry()
        {
            try
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "MetricsReportingEnabled", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge", "SendSiteInfoToImproveServices", 0, RegistryValueKind.DWord);
            }
            catch { }
        }

        #endregion
    }
}
