using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace HardOptimizer
{
    static class Program
    {
        internal const float Major = 18;
        internal const float Minor = 0;
        internal const bool EXPERIMENTAL_BUILD = false;

        internal static string VersionString => $"{Major}.{Minor}";
        internal static bool SILENT_MODE = false;
        internal static bool UNSAFE_MODE = false;
        internal static MainForm MainFormInstance;

        private static readonly string MUTEX_GUID = @"{HARDOPTIMIZER-18-0EFC7B8A-D1FC-467F-B4B1-0117C643FE19}";
        private static Mutex _mutex;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Process command-line switches
            foreach (var arg in args)
            {
                if (string.Equals(arg, "/silent", StringComparison.OrdinalIgnoreCase))
                    SILENT_MODE = true;
                else if (string.Equals(arg, "/unsafe", StringComparison.OrdinalIgnoreCase))
                    UNSAFE_MODE = true;
            }

            // Single-instance enforcement
            bool createdNew;
            _mutex = new Mutex(true, MUTEX_GUID, out createdNew);
            if (!createdNew)
            {
                MessageBox.Show("HardOptimizer is already running.", "HardOptimizer",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Require administrator privileges
                if (!IsAdmin())
                {
                    var exe = Process.GetCurrentProcess().MainModule?.FileName;
                    if (string.IsNullOrEmpty(exe)) return;
                    var psi = new ProcessStartInfo(exe)
                    {
                        UseShellExecute = true,
                        Verb = "runas",
                        Arguments = string.Join(" ", args)
                    };
                    try { Process.Start(psi); } catch { /* User cancelled UAC */ }
                    return;
                }

                // Verify Windows version (Windows 7+)
                var os = Environment.OSVersion.Version;
                if (os.Major < 6 || (os.Major == 6 && os.Minor < 1))
                {
                    MessageBox.Show("HardOptimizer requires Windows 7 or higher.", "HardOptimizer",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Create system restore point at startup so user can undo changes
                CreateRestorePoint("HardOptimizer Startup");

                // Load settings and start main form
                OptionsHelper.LoadSettings();
                MainFormInstance = new MainForm(SILENT_MODE);
                Application.Run(MainFormInstance);
            }
            catch (Exception ex)
            {
                Logger.Log("Program.Main", ex.Message, ex.StackTrace);
                MessageBox.Show($"Fatal error: {ex.Message}", "HardOptimizer",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _mutex?.ReleaseMutex();
            }
        }

        private static bool IsAdmin()
        {
            try
            {
                var id = System.Security.Principal.WindowsIdentity.GetCurrent();
                var principal = new System.Security.Principal.WindowsPrincipal(id);
                return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            }
            catch { return false; }
        }

        internal static void CreateRestorePoint(string description)
        {
            try
            {
                // Use WMI to create a system restore point
                using var searcher = new System.Management.ManagementObjectSearcher(
                    "SELECT * FROM SystemRestore");
                var restorePoints = searcher.Get();
                foreach (var mo in restorePoints)
                {
                    try
                    {
                        var outParams = mo.InvokeMethod("CreateRestorePoint",
                            new object[] { description, 0, 100 }, null); // 0=APPLICATION_INSTALL, 100=undetermined
                        break;
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Program.CreateRestorePoint", ex.Message, ex.StackTrace);
                // Restore point creation failure is non-fatal
            }
        }

        internal static void RestartInSafeMode()
        {
            Utilities.RunCommand("bcdedit /set {current} safeboot Minimal");
            Thread.Sleep(500);
            Utilities.Reboot();
        }

        internal static void RestartInNormalMode()
        {
            Utilities.RunCommand("bcdedit /deletevalue {current} safeboot");
            Thread.Sleep(500);
            Utilities.Reboot();
        }
    }
}
