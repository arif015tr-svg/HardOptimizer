using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace HardOptimizer
{
    internal static class UWPHelper
    {
        internal static List<UwpApp> GetUWPApps(bool showAll)
        {
            var apps = new List<UwpApp>();
            try
            {
                using var ps = PowerShell.Create();
                ps.AddScript("Get-AppxPackage | Select-Object Name, PackageFamilyName, InstallLocation, IsFramework");
                foreach (var result in ps.Invoke())
                {
                    var name = result.Properties["Name"]?.Value?.ToString() ?? "";
                    var fullName = result.Properties["PackageFamilyName"]?.Value?.ToString() ?? "";
                    var location = result.Properties["InstallLocation"]?.Value?.ToString() ?? "";
                    var isFramework = bool.TryParse(result.Properties["IsFramework"]?.Value?.ToString(), out var fw) && fw;

                    if (!showAll && isFramework) continue;

                    apps.Add(new UwpApp
                    {
                        Name = name,
                        DisplayName = name,
                        InstallLocation = location,
                        IsFramework = isFramework,
                        IsRemovable = true,
                        FullNames = { fullName }
                    });
                }
            }
            catch { }
            return apps;
        }

        internal static bool UninstallUWPApp(UwpApp app)
        {
            try
            {
                foreach (var fullName in app.FullNames)
                {
                    using var ps = PowerShell.Create();
                    ps.AddScript($"Remove-AppxPackage -Package '{fullName}'");
                    ps.Invoke();
                }
                return true;
            }
            catch { return false; }
        }

        internal static bool RestoreAllUWPApps()
        {
            try
            {
                using var ps = PowerShell.Create();
                ps.AddScript("Get-AppxPackage -AllUsers | Foreach {Add-AppxPackage -DisableDevelopmentMode -Register \"$($_.InstallLocation)\\AppXManifest.xml\"}");
                ps.Invoke();
                return true;
            }
            catch { return false; }
        }
    }
}
