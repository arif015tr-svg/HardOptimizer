using System;
using System.Management;

namespace HardOptimizer
{
    internal static class HardwareHelper
    {
        internal static HardwareInfo GetHardwareInfo()
        {
            var info = new HardwareInfo();

            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                foreach (var mo in searcher.Get())
                {
                    info.CpuName = mo.Properties["Name"]?.Value?.ToString() ?? "";
                    break;
                }
            }
            catch { }

            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
                ulong total = 0;
                foreach (var mo in searcher.Get())
                {
                    total += Convert.ToUInt64(mo.Properties["Capacity"]?.Value ?? 0);
                }
                info.TotalRam = Utilities.ByteSizeToString((long)total);
            }
            catch { }

            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
                foreach (var mo in searcher.Get())
                {
                    info.GpuName = mo.Properties["Name"]?.Value?.ToString() ?? "";
                    break;
                }
            }
            catch { }

            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (var mo in searcher.Get())
                {
                    info.DiskModel = mo.Properties["Model"]?.Value?.ToString() ?? "";
                    break;
                }
            }
            catch { }

            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                foreach (var mo in searcher.Get())
                {
                    info.MotherboardModel = mo.Properties["Product"]?.Value?.ToString() ?? "";
                    break;
                }
            }
            catch { }

            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
                foreach (var mo in searcher.Get())
                {
                    info.BiosVersion = mo.Properties["Version"]?.Value?.ToString() ?? "";
                    break;
                }
            }
            catch { }

            return info;
        }
    }
}
