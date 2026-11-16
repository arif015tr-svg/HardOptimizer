using System;
using System.Collections.Generic;
using System.ServiceProcess;

namespace HardOptimizer
{
    public sealed class ServiceInfo
    {
        public string Name { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Status { get; set; } = "";
        public string StartType { get; set; } = "";
        public bool IsMicrosoft { get; set; } = false;
    }

    internal static class ServiceHelper
    {
        private static readonly string[] MicrosoftServicePrefixes = {
            "Microsoft", "Windows", "Defender", "WinDefend", "WinRM", "WSearch",
            "SysMain", "WudfSvc", "Wecsvc", "W3SVC", "WindowsUpdate", "wuauserv",
            "UsoSvc", "DoSvc", "WerSvc", "ShellHWDetection", "Themes",
            "AudioSrv", "PlugPlay", "EventLog", "Spooler", "Schedule",
            "MpsSvc", "Dhcp", "Dnscache", "LanmanServer", "LanmanWorkstation",
            "RPCSS", "RpcEptMapper", "Winmgmt", "BITS", "SSTR",
            "TrustedInstaller", "Appinfo", "AppMgmt", "BDESVC", "cryptsvc",
            "FontCache", "gpsvc", "iphlpsvc", "Netman", "NlaSvc",
            "PolicyAgent", "RemoteAccess", "SstpSvc", "TapiSrv",
            "TermService", "Umrdp", "W32Time", "WinHttpAutoProxySvc"
        };

        internal static List<ServiceInfo> GetNonMicrosoftServices()
        {
            var services = new List<ServiceInfo>();
            try
            {
                var scServices = ServiceController.GetServices();
                foreach (var svc in scServices)
                {
                    try
                    {
                        bool isMicrosoft = IsMicrosoftService(svc.ServiceName, svc.DisplayName);
                        services.Add(new ServiceInfo
                        {
                            Name = svc.ServiceName,
                            DisplayName = svc.DisplayName,
                            Status = svc.Status.ToString(),
                            StartType = GetStartType(svc),
                            IsMicrosoft = isMicrosoft
                        });
                    }
                    catch { }
                }
            }
            catch { }
            return services;
        }

        private static bool IsMicrosoftService(string serviceName, string displayName)
        {
            var combined = (serviceName + " " + displayName).ToLowerInvariant();
            foreach (var prefix in MicrosoftServicePrefixes)
            {
                if (combined.Contains(prefix.ToLowerInvariant()))
                    return true;
            }
            return false;
        }

        private static string GetStartType(ServiceController svc)
        {
            try
            {
                var safeName = (svc.ServiceName ?? "").Replace("'", "\\'");
                using var wmi = new System.Management.ManagementObject($"Win32_Service.Name='{safeName}'");
                var startMode = wmi["StartMode"]?.ToString() ?? "Unknown";
                return startMode;
            }
            catch { return "Unknown"; }
        }

        internal static bool StopService(string serviceName)
        {
            try
            {
                using var svc = new ServiceController(serviceName);
                if (svc.Status == ServiceControllerStatus.Running || svc.Status == ServiceControllerStatus.StartPending)
                {
                    svc.Stop();
                    svc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                    return true;
                }
            }
            catch { }
            return false;
        }

        internal static bool SetServiceStartType(string serviceName, string startType)
        {
            try
            {
                var safeName = (serviceName ?? "").Replace("'", "\\'");
                using var wmi = new System.Management.ManagementObject($"Win32_Service.Name='{safeName}'");
                var inParams = wmi.GetMethodParameters("ChangeStartMode");
                inParams["StartMode"] = startType; // "Automatic", "Manual", "Disabled"
                var outParams = wmi.InvokeMethod("ChangeStartMode", inParams, null) as System.Management.ManagementBaseObject;
                if (outParams == null) return false;
                var returnValue = Convert.ToUInt32(outParams["ReturnValue"]);
                return returnValue == 0;
            }
            catch { return false; }
        }
    }
}
