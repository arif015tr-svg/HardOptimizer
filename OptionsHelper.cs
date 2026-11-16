using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace HardOptimizer
{
    internal static class OptionsHelper
    {
        internal static readonly string SettingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Optimizer.cfg");
        internal static Options CurrentOptions = new Options();
        internal static Dictionary<string, string> TranslationList = new Dictionary<string, string>();

        internal static void LoadSettings()
        {
            try
            {
                if (!File.Exists(SettingsFile))
                {
                    CurrentOptions = new Options();
                    SaveSettings();
                    return;
                }
                foreach (var line in File.ReadAllLines(SettingsFile))
                {
                    if (string.IsNullOrWhiteSpace(line) || !line.Contains("=")) continue;
                    var parts = line.Split(new[] { '=' }, 2);
                    var key = parts[0].Trim();
                    var val = parts[1].Trim();
                    switch (key)
                    {
                        case "ThemeMode": CurrentOptions.ThemeMode = (ThemeMode)int.Parse(val); break;
                        case "LanguageCode": CurrentOptions.LanguageCode = (LanguageCode)int.Parse(val); break;
                        case "EnablePerformanceTweaks": CurrentOptions.EnablePerformanceTweaks = bool.Parse(val); break;
                        case "DisableNetworkThrottling": CurrentOptions.DisableNetworkThrottling = bool.Parse(val); break;
                        case "DisableWindowsDefender": CurrentOptions.DisableWindowsDefender = bool.Parse(val); break;
                        case "DisableSuperfetch": CurrentOptions.DisableSuperfetch = bool.Parse(val); break;
                        case "DisableErrorReporting": CurrentOptions.DisableErrorReporting = bool.Parse(val); break;
                        case "DisableSmartScreen": CurrentOptions.DisableSmartScreen = bool.Parse(val); break;
                        case "DisableHibernation": CurrentOptions.DisableHibernation = bool.Parse(val); break;
                        case "DisableSMB1": CurrentOptions.DisableSMB1 = bool.Parse(val); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("OptionsHelper.LoadSettings", ex.Message, ex.StackTrace);
                CurrentOptions = new Options();
            }
        }

        internal static void SaveSettings()
        {
            try
            {
                using var writer = new StreamWriter(SettingsFile);
                writer.WriteLine($"ThemeMode={(int)CurrentOptions.ThemeMode}");
                writer.WriteLine($"LanguageCode={(int)CurrentOptions.LanguageCode}");
                writer.WriteLine($"EnablePerformanceTweaks={CurrentOptions.EnablePerformanceTweaks}");
                writer.WriteLine($"DisableNetworkThrottling={CurrentOptions.DisableNetworkThrottling}");
                writer.WriteLine($"DisableWindowsDefender={CurrentOptions.DisableWindowsDefender}");
                writer.WriteLine($"DisableSuperfetch={CurrentOptions.DisableSuperfetch}");
                writer.WriteLine($"DisableErrorReporting={CurrentOptions.DisableErrorReporting}");
                writer.WriteLine($"DisableSmartScreen={CurrentOptions.DisableSmartScreen}");
                writer.WriteLine($"DisableHibernation={CurrentOptions.DisableHibernation}");
                writer.WriteLine($"DisableSMB1={CurrentOptions.DisableSMB1}");
                writer.WriteLine($"EnableUtcTime={CurrentOptions.EnableUtcTime}");
                writer.WriteLine($"RemoveMenusDelay={CurrentOptions.RemoveMenusDelay}");
                writer.WriteLine($"DisableTelemetryTasks={CurrentOptions.DisableTelemetryTasks}");
                writer.WriteLine($"DisableCortana={CurrentOptions.DisableCortana}");
                writer.WriteLine($"DisableXboxLive={CurrentOptions.DisableXboxLive}");
                writer.WriteLine($"DisableGameBar={CurrentOptions.DisableGameBar}");
                writer.WriteLine($"DisableStoreUpdates={CurrentOptions.DisableStoreUpdates}");
                writer.WriteLine($"DisableChromeTelemetry={CurrentOptions.DisableChromeTelemetry}");
                writer.WriteLine($"DisableEdgeTelemetry={CurrentOptions.DisableEdgeTelemetry}");
                writer.WriteLine($"UpdateOnLaunch={CurrentOptions.UpdateOnLaunch}");
                writer.WriteLine($"AutoStart={CurrentOptions.AutoStart}");
            }
            catch (Exception ex)
            {
                Logger.Log("OptionsHelper.SaveSettings", ex.Message, ex.StackTrace);
            }
        }

        internal static void LoadTranslation(LanguageCode code)
        {
            TranslationList.Clear();
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "i18n",
                    code == LanguageCode.TR ? "TR.json" : "EN.json");
                if (File.Exists(path))
                {
                    foreach (var line in File.ReadAllLines(path))
                    {
                        if (string.IsNullOrWhiteSpace(line) || !line.Contains(":")) continue;
                        var parts = line.Split(new[] { ':' }, 2);
                        var key = parts[0].Trim().Trim('"');
                        var val = parts[1].Trim().Trim('"', ',');
                        TranslationList[key] = val;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("OptionsHelper.LoadTranslation", ex.Message, ex.StackTrace);
            }
        }

        internal static string T(string key)
        {
            return TranslationList.TryGetValue(key, out var value) ? value : key;
        }
    }
}
