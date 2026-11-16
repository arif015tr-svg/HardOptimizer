using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace HardOptimizer
{
    internal static class PingerHelper
    {
        private static readonly Ping _pinger = new Ping();

        internal static string[] GoogleDNSv4 = { "8.8.8.8", "8.8.4.4" };
        internal static string[] CloudflareDNSv4 = { "1.1.1.1", "1.0.0.1" };
        internal static string[] OpenDNSv4 = { "208.67.222.222", "208.67.220.220" };
        internal static string[] Quad9DNSv4 = { "9.9.9.9", "149.112.112.112" };

        internal static NetworkInterface[] GetActiveNetworkAdapters()
        {
            try
            {
                return NetworkInterface.GetAllNetworkInterfaces()
                    .Where(a => a.OperationalStatus == OperationalStatus.Up &&
                        (a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                         a.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                    .ToArray();
            }
            catch { return Array.Empty<NetworkInterface>(); }
        }

        internal static void SetDNS(string nic, string[] dnsV4, string[] dnsV6)
        {
            try
            {
                if (dnsV4 != null && dnsV4.Length > 0)
                    Utilities.RunCommand($"/c netsh interface ipv4 set dnsservers \"{nic}\" static {dnsV4[0]} primary");
                if (dnsV4 != null && dnsV4.Length > 1)
                    Utilities.RunCommand($"/c netsh interface ipv4 add dnsservers \"{nic}\" {dnsV4[1]} index=2");
            }
            catch { }
        }

        internal static void ResetDefaultDNS(string nic)
        {
            try
            {
                Utilities.RunCommand($"/c netsh interface ipv4 set dnsservers \"{nic}\" dhcp");
            }
            catch { }
        }

        internal static void ResetDefaultDNSForAllNICs()
        {
            foreach (var nic in GetActiveNetworkAdapters().Select(x => x.Name))
                ResetDefaultDNS(nic);
        }

        internal static PingReply PingHost(string nameOrAddress)
        {
            try
            {
                var ip = Dns.GetHostAddresses(nameOrAddress)
                    .FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
                if (ip == null) return null;
                return _pinger.Send(ip, 3000);
            }
            catch { return null; }
        }

        internal static bool IsInternetAvailable()
        {
            try
            {
                var reply = _pinger.Send("1.1.1.1", 1000);
                return reply != null && reply.Status == IPStatus.Success;
            }
            catch { return false; }
        }

        internal static void FlushDNSCache()
        {
            Utilities.RunCommand("/c ipconfig /flushdns");
        }
    }
}
