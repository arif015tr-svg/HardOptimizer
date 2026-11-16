using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HardOptimizer
{
    internal static class IntegratorHelper
    {
        internal static void CreateCustomCommand(string file, string keyword)
        {
            try
            {
                if (!keyword.EndsWith(".exe")) keyword += ".exe";
                var key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + keyword;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + keyword);
                Registry.SetValue(key, "", file);
                Registry.SetValue(key, "Path", Path.GetDirectoryName(file));
            }
            catch { }
        }

        internal static List<string> GetCustomCommands()
        {
            var items = new List<string>();
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths");
                if (key != null)
                    items.AddRange(key.GetSubKeyNames());
            }
            catch { }
            return items;
        }

        internal static void DeleteCustomCommand(string command)
        {
            try { Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + command, false); } catch { }
        }

        internal static List<string> GetDesktopItems()
        {
            var items = new List<string>();
            try
            {
                using var key = Registry.ClassesRoot.OpenSubKey(@"DesktopBackground\Shell");
                if (key != null)
                {
                    foreach (var item in key.GetSubKeyNames())
                    {
                        if (item.Contains("Gadgets") || item.Contains("Display") || item.Contains("Personalize")) continue;
                        items.Add(item);
                    }
                }
            }
            catch { }
            return items;
        }

        internal static void RemoveDesktopItem(string name)
        {
            try
            {
                using var key = Registry.ClassesRoot.OpenSubKey(@"DesktopBackground\Shell", true);
                key?.DeleteSubKeyTree(name, false);
            }
            catch { }
        }

        internal static string[] GetPathSystemVariables()
        {
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment");
                if (key != null)
                {
                    var val = key.GetValue("Path", new string[0]).ToString();
                    return val.Split(';');
                }
            }
            catch { }
            return Array.Empty<string>();
        }

        internal static void UpdatePathSystemVariables(string[] newValues)
        {
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment", true);
                key?.SetValue("Path", string.Join(";", newValues), RegistryValueKind.ExpandString);
                Utilities.NotifyEnvironmentChange();
            }
            catch { }
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool SendNotifyMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, string lParam);
        private const int HWND_BROADCAST = 0xffff;
        private const uint WM_SETTINGCHANGE = 0x001a;

        internal static void ApplyPathChanges()
        {
            try { SendNotifyMessage((IntPtr)HWND_BROADCAST, WM_SETTINGCHANGE, UIntPtr.Zero, "Environment"); } catch { }
        }
    }
}
