using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HardOptimizer
{
    internal static class StartupHelper
    {
        private static readonly string LocalMachineRun = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private static readonly string LocalMachineRunOnce = @"Software\Microsoft\Windows\CurrentVersion\RunOnce";
        private static readonly string LocalMachineRunWoW = @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Run";
        private static readonly string LocalMachineRunOnceWow = @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\RunOnce";
        private static readonly string CurrentUserRun = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private static readonly string CurrentUserRunOnce = @"Software\Microsoft\Windows\CurrentVersion\RunOnce";
        private static readonly string LocalMachineStartupFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);
        private static readonly string CurrentUserStartupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

        internal static List<StartupItem> GetStartupItems()
        {
            var items = new List<StartupItem>();
            GetRegistryItems(items, Registry.LocalMachine, LocalMachineRun, StartupItemLocation.HKLM, StartupItemType.Run);
            GetRegistryItems(items, Registry.LocalMachine, LocalMachineRunOnce, StartupItemLocation.HKLM, StartupItemType.RunOnce);
            GetRegistryItems(items, Registry.CurrentUser, CurrentUserRun, StartupItemLocation.HKCU, StartupItemType.Run);
            GetRegistryItems(items, Registry.CurrentUser, CurrentUserRunOnce, StartupItemLocation.HKCU, StartupItemType.RunOnce);

            if (Environment.Is64BitOperatingSystem)
            {
                GetRegistryItems(items, Registry.LocalMachine, LocalMachineRunWoW, StartupItemLocation.HKLMWoW, StartupItemType.Run);
                GetRegistryItems(items, Registry.LocalMachine, LocalMachineRunOnceWow, StartupItemLocation.HKLMWoW, StartupItemType.RunOnce);
            }

            GetFolderItems(items, CurrentUserStartupFolder, StartupItemLocation.CUStartupFolder);
            GetFolderItems(items, LocalMachineStartupFolder, StartupItemLocation.LMStartupFolder);

            return items;
        }

        private static void GetRegistryItems(List<StartupItem> list, RegistryKey hive, string keyPath, StartupItemLocation location, StartupItemType type)
        {
            try
            {
                using var key = hive.OpenSubKey(keyPath, true);
                if (key == null) return;
                foreach (var name in key.GetValueNames())
                {
                    try
                    {
                        list.Add(new RegistryStartupItem
                        {
                            Name = name,
                            FileLocation = key.GetValue(name)?.ToString() ?? "",
                            Key = key,
                            RegistryLocation = location,
                            StartupType = type
                        });
                    }
                    catch { }
                }
            }
            catch { }
        }

        private static void GetFolderItems(List<StartupItem> list, string folder, StartupItemLocation location)
        {
            try
            {
                if (!Directory.Exists(folder)) return;
                foreach (var file in Directory.GetFiles(folder, "*.lnk"))
                {
                    try
                    {
                        list.Add(new FolderStartupItem
                        {
                            Name = Path.GetFileNameWithoutExtension(file),
                            FileLocation = Utilities.GetShortcutTargetFile(file),
                            Shortcut = file,
                            RegistryLocation = location
                        });
                    }
                    catch { }
                }
            }
            catch { }
        }

        internal static void RemoveItem(StartupItem item) => item.Remove();
        internal static void ToggleItem(StartupItem item, bool enable)
        {
            try
            {
                if (item is RegistryStartupItem rsi)
                {
                    if (enable) rsi.Key?.SetValue(rsi.Name, rsi.FileLocation);
                    else rsi.Key?.DeleteValue(rsi.Name, false);
                }
            }
            catch { }
        }
    }
}
