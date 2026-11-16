using System;
using System.IO;
using System.Text;

namespace HardOptimizer
{
    internal static class Logger
    {
        private static readonly string LogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Optimizer.log");

        internal static void Log(string functionName, string errorMessage, string stackTrace)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] [{functionName}]");
                sb.AppendLine(errorMessage);
                sb.AppendLine(stackTrace);
                sb.AppendLine(new string('-', 60));
                File.AppendAllText(LogFile, sb.ToString());
            }
            catch { /* Logging failures are silent */ }
        }

        internal static void Info(string message)
        {
            try
            {
                File.AppendAllText(LogFile, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [INFO] {message}{Environment.NewLine}");
            }
            catch { }
        }
    }
}
