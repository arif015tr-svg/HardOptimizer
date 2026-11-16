using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HardOptimizer
{
    internal static class Utilities
    {
        internal static WindowsVersion CurrentWindowsVersion = WindowsVersion.Unsupported;

        internal static string GetOS()
        {
            try
            {
                var productName = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", "");
                if (!string.IsNullOrEmpty(productName))
                {
                    if (productName.Contains("Windows 11")) CurrentWindowsVersion = WindowsVersion.Windows11;
                    else if (productName.Contains("Windows 10")) CurrentWindowsVersion = WindowsVersion.Windows10;
                    else if (productName.Contains("Windows 8.1")) CurrentWindowsVersion = WindowsVersion.Windows8;
                    else if (productName.Contains("Windows 8")) CurrentWindowsVersion = WindowsVersion.Windows8;
                    else if (productName.Contains("Windows 7")) CurrentWindowsVersion = WindowsVersion.Windows7;
                    return productName;
                }
            }
            catch { }
            return "Unknown Windows";
        }

        internal static string GetWindowsDetails()
        {
            var bitness = Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit";
            try
            {
                var build = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "DisplayVersion", "");
                return $"{GetOS()} - {build} ({bitness})";
            }
            catch { return $"{GetOS()} - ({bitness})"; }
        }

        internal static string GetNETFramework()
        {
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full");
                if (key != null)
                {
                    var release = key.GetValue("Release") as int?;
                    if (release.HasValue)
                    {
                        return release.Value switch
                        {
                            >= 528040 => "4.8.1",
                            >= 528012 => "4.8",
                            >= 461308 => "4.7.2",
                            >= 461814 => "4.7.1",
                            >= 460798 => "4.7",
                            >= 394802 => "4.6.2",
                            >= 394801 => "4.6",
                            >= 379893 => "4.5.2",
                            _ => "4.x"
                        };
                    }
                }
            }
            catch { }
            return "Unknown";
        }

        internal static void RunCommand(string command)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                using var process = Process.Start(psi);
                process?.WaitForExit(10000);
            }
            catch (Exception ex)
            {
                Logger.Log("Utilities.RunCommand", ex.Message, ex.StackTrace);
            }
        }

        internal static void RunBatchFile(string path)
        {
            try
            {
                if (!File.Exists(path)) return;
                RunCommand($"/c \"{path}\"");
            }
            catch (Exception ex)
            {
                Logger.Log("Utilities.RunBatchFile", ex.Message, ex.StackTrace);
            }
        }

        internal static void Reboot()
        {
            try { RunCommand("shutdown /r /t 0"); } catch { }
        }

        internal static bool IsAdmin()
        {
            try
            {
                var id = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(id);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch { return false; }
        }

        internal static void ImportRegistryScript(string regFilePath)
        {
            try
            {
                if (!File.Exists(regFilePath)) return;
                RunCommand($"/c reg import \"{regFilePath}\"");
            }
            catch (Exception ex)
            {
                Logger.Log("Utilities.ImportRegistryScript", ex.Message, ex.StackTrace);
            }
        }

        internal static void FindFile(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return;
                var psi = new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = $"/select,\"{path}\"",
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Logger.Log("Utilities.FindFile", ex.Message, ex.StackTrace);
            }
        }

        internal static void FindKeyInRegistry(string keyPath)
        {
            try
            {
                if (string.IsNullOrEmpty(keyPath)) return;
                RunCommand($"/c regedit /e {keyPath}");
            }
            catch (Exception ex)
            {
                Logger.Log("Utilities.FindKeyInRegistry", ex.Message, ex.StackTrace);
            }
        }

        internal static string GetShortcutTargetFile(string shortcutPath)
        {
            try
            {
                var shell = Type.GetTypeFromProgID("WScript.Shell");
                if (shell == null) return "";
                var instance = Activator.CreateInstance(shell);
                var shortcut = shell.InvokeMember("CreateShortcut", BindingFlags.InvokeMethod, null, instance, new object[] { shortcutPath });
                var targetPath = shell.GetType().InvokeMember("TargetPath", BindingFlags.GetProperty, null, shortcut, null);
                return targetPath?.ToString() ?? "";
            }
            catch { return ""; }
        }

        internal static void PreventProcessFromRunning(string processName)
        {
            try { RunCommand($"/c taskkill /F /IM {processName} /T 2>nul"); } catch { }
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool SendNotifyMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, string lParam);
        private const int HWND_BROADCAST = 0xffff;
        private const uint WM_SETTINGCHANGE = 0x001a;

        internal static void NotifyEnvironmentChange()
        {
            try { SendNotifyMessage((IntPtr)HWND_BROADCAST, WM_SETTINGCHANGE, UIntPtr.Zero, "Environment"); } catch { }
        }

        internal static string ByteSizeToString(long bytes)
        {
            if (bytes == 0) return "0 B";
            var sizes = new[] { "B", "KB", "MB", "GB", "TB" };
            var order = Math.Min(sizes.Length - 1, (int)Math.Floor(Math.Log(bytes, 1024)));
            return $"{bytes / Math.Pow(1024, order):0.##} {sizes[order]}";
        }
    }
}
