using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HardOptimizer
{
    internal static class HostsHelper
    {
        private static readonly string HostsFile = @"C:\Windows\System32\drivers\etc\hosts";

        internal static string[] ReadHosts()
        {
            try { return File.Exists(HostsFile) ? File.ReadAllLines(HostsFile) : Array.Empty<string>(); }
            catch { return Array.Empty<string>(); }
        }

        internal static void SaveHosts(string[] lines)
        {
            try
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (!lines[i].StartsWith("#") && !string.IsNullOrEmpty(lines[i]))
                        lines[i] = SanitizeEntry(lines[i]);
                }
                File.WriteAllLines(HostsFile, lines);
            }
            catch (Exception ex)
            {
                Logger.Log("HostsHelper.SaveHosts", ex.Message, ex.StackTrace);
                MessageBox.Show("Hosts dosyası yazılamadı!", "Optimizer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void RestoreDefaultHosts()
        {
            try
            {
                if (File.Exists(HostsFile)) File.Delete(HostsFile);
                File.WriteAllText(HostsFile, "127.0.0.1   localhost" + Environment.NewLine);
            }
            catch { }
        }

        internal static void AddEntry(string entry)
        {
            try { File.AppendAllText(HostsFile, Environment.NewLine + SanitizeEntry(entry)); }
            catch { }
        }

        internal static void RemoveEntry(string entry)
        {
            try
            {
                if (!File.Exists(HostsFile)) return;
                var lines = File.ReadAllLines(HostsFile);
                File.WriteAllLines(HostsFile, Array.FindAll(lines, l => l != entry));
            }
            catch { }
        }

        internal static List<string> GetHostsEntries()
        {
            var entries = new List<string>();
            foreach (var line in ReadHosts())
            {
                if (!line.StartsWith("#") && !string.IsNullOrWhiteSpace(line))
                    entries.Add(line.Replace(" ", " : "));
            }
            return entries;
        }

        internal static bool GetReadOnly()
        {
            try { return new FileInfo(HostsFile).IsReadOnly; } catch { return false; }
        }

        internal static void SetReadOnly(bool enable)
        {
            try { new FileInfo(HostsFile).IsReadOnly = enable; } catch { }
        }

        private static string SanitizeEntry(string entry) => Regex.Replace(entry, @"\s{2,}", " ");
    }
}
