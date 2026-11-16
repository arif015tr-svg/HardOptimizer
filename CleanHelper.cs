using System;
using System.IO;
using System.Linq;

namespace HardOptimizer
{
    internal static class CleanHelper
    {
        private static readonly string TempFolder = Path.GetTempPath();
        private static readonly string ProfileAppDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static readonly string ProfileAppDataRoaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        internal static long PreviewTemp()
        {
            long size = 0;
            try
            {
                if (Directory.Exists(TempFolder))
                    size += DirectorySize(new DirectoryInfo(TempFolder));
            }
            catch { }
            return size;
        }

        internal static void CleanTemp()
        {
            try
            {
                if (!Directory.Exists(TempFolder)) return;
                foreach (var file in Directory.GetFiles(TempFolder, "*.*", SearchOption.AllDirectories))
                {
                    try { File.Delete(file); } catch { }
                }
                foreach (var dir in Directory.GetDirectories(TempFolder, "*.*", SearchOption.AllDirectories).OrderByDescending(d => d.Length))
                {
                    try { Directory.Delete(dir, true); } catch { }
                }
            }
            catch { }
        }

        internal static long PreviewMinidumps()
        {
            long size = 0;
            try
            {
                var minidump = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Minidump");
                if (Directory.Exists(minidump))
                    size += DirectorySize(new DirectoryInfo(minidump));
            }
            catch { }
            return size;
        }

        internal static void CleanMinidumps()
        {
            try
            {
                var minidump = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Minidump");
                if (!Directory.Exists(minidump)) return;
                foreach (var file in Directory.GetFiles(minidump, "*.dmp"))
                { try { File.Delete(file); } catch { } }
            }
            catch { }
        }

        internal static long PreviewRecycleBin()
        {
            long size = 0;
            try
            {
                foreach (var drive in DriveInfo.GetDrives())
                {
                    try
                    {
                        var recycle = Path.Combine(drive.Name, "$Recycle.Bin");
                        if (Directory.Exists(recycle))
                            size += DirectorySize(new DirectoryInfo(recycle));
                    }
                    catch { }
                }
            }
            catch { }
            return size;
        }

        internal static void CleanRecycleBin()
        {
            try
            {
                System.Diagnostics.Process.Start("cmd.exe", "/c echo Y | powershell -Command \"Clear-RecycleBin -Force -ErrorAction SilentlyContinue\"");
            }
            catch { }
        }

        internal static long PreviewBrowserCache(string browser)
        {
            long size = 0;
            try
            {
                string cachePath = browser switch
                {
                    "Chrome" => Path.Combine(ProfileAppDataLocal, @"Google\Chrome\User Data\Default\Cache"),
                    "Edge" => Path.Combine(ProfileAppDataLocal, @"Microsoft\Edge\User Data\Default\Cache"),
                    "Firefox" => Path.Combine(ProfileAppDataLocal, @"Mozilla\Firefox\Profiles"),
                    "Brave" => Path.Combine(ProfileAppDataLocal, @"BraveSoftware\Brave-Browser\User Data\Default\Cache"),
                    _ => ""
                };
                if (!string.IsNullOrEmpty(cachePath) && Directory.Exists(cachePath))
                    size += DirectorySize(new DirectoryInfo(cachePath));
            }
            catch { }
            return size;
        }

        internal static void CleanBrowserCache(string browser)
        {
            try
            {
                string cachePath = browser switch
                {
                    "Chrome" => Path.Combine(ProfileAppDataLocal, @"Google\Chrome\User Data\Default\Cache"),
                    "Edge" => Path.Combine(ProfileAppDataLocal, @"Microsoft\Edge\User Data\Default\Cache"),
                    "Firefox" => Path.Combine(ProfileAppDataLocal, @"Mozilla\Firefox\Profiles"),
                    "Brave" => Path.Combine(ProfileAppDataLocal, @"BraveSoftware\Brave-Browser\User Data\Default\Cache"),
                    _ => ""
                };
                if (!string.IsNullOrEmpty(cachePath) && Directory.Exists(cachePath))
                {
                    foreach (var file in Directory.GetFiles(cachePath, "*.*", SearchOption.AllDirectories))
                    { try { File.Delete(file); } catch { } }
                }
            }
            catch { }
        }

        private static long DirectorySize(DirectoryInfo d)
        {
            long size = 0;
            try
            {
                foreach (var f in d.GetFiles("*.*", SearchOption.AllDirectories))
                { try { size += f.Length; } catch { } }
                foreach (var sub in d.GetDirectories())
                { try { size += DirectorySize(sub); } catch { } }
            }
            catch { }
            return size;
        }
    }
}