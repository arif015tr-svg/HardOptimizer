using System;
using System.IO;

namespace HardOptimizer
{
    internal static class SilentOps
    {
        internal static void ProcessAllActions()
        {
            Logger.Info("=== Optimizer Silent Mode Started ===");
            Logger.Info($"Version: {Program.VersionString}");

            try
            {
                if (OptionsHelper.CurrentOptions.EnablePerformanceTweaks)
                {
                    OptimizeHelper.EnablePerformanceTweaks();
                    Logger.Info("Applied: Performance Tweaks");
                }
                if (OptionsHelper.CurrentOptions.DisableSuperfetch)
                {
                    OptimizeHelper.DisableSuperfetch();
                    Logger.Info("Applied: Disable Superfetch");
                }
                if (OptionsHelper.CurrentOptions.DisableErrorReporting)
                {
                    OptimizeHelper.DisableErrorReporting();
                    Logger.Info("Applied: Disable Error Reporting");
                }
                if (OptionsHelper.CurrentOptions.DisableSmartScreen)
                {
                    OptimizeHelper.DisableSmartScreen();
                    Logger.Info("Applied: Disable SmartScreen");
                }
                if (OptionsHelper.CurrentOptions.DisableHibernation)
                {
                    OptimizeHelper.DisableHibernation();
                    Logger.Info("Applied: Disable Hibernation");
                }
                if (OptionsHelper.CurrentOptions.DisableSMB1)
                {
                    OptimizeHelper.DisableSMB1();
                    Logger.Info("Applied: Disable SMB1");
                }
                if (OptionsHelper.CurrentOptions.EnableUtcTime)
                {
                    OptimizeHelper.EnableUtcTime();
                    Logger.Info("Applied: Enable UTC Time");
                }
                if (OptionsHelper.CurrentOptions.DisableWindowsDefender)
                {
                    OptimizeHelper.DisableWindowsDefender();
                    Logger.Info("Applied: Disable Windows Defender");
                }
                if (OptionsHelper.CurrentOptions.DisableTelemetryTasks)
                {
                    OptimizeHelper.DisableTelemetryTasks();
                    Logger.Info("Applied: Disable Telemetry Tasks");
                }
                if (OptionsHelper.CurrentOptions.DisableCortana)
                {
                    OptimizeHelper.DisableCortana();
                    Logger.Info("Applied: Disable Cortana");
                }
                if (OptionsHelper.CurrentOptions.DisableXboxLive)
                {
                    OptimizeHelper.DisableXboxLive();
                    Logger.Info("Applied: Disable Xbox Live");
                }
                if (OptionsHelper.CurrentOptions.DisableGameBar)
                {
                    OptimizeHelper.DisableGameBar();
                    Logger.Info("Applied: Disable Game Bar");
                }
                if (OptionsHelper.CurrentOptions.DisableChromeTelemetry)
                {
                    OptimizeHelper.DisableChromeTelemetry();
                    Logger.Info("Applied: Disable Chrome Telemetry");
                }
                if (OptionsHelper.CurrentOptions.DisableEdgeTelemetry)
                {
                    OptimizeHelper.DisableEdgeTelemetry();
                    Logger.Info("Applied: Disable Edge Telemetry");
                }

                OptionsHelper.SaveSettings();
                Logger.Info("=== Optimizer Silent Mode Completed ===");
            }
            catch (Exception ex)
            {
                Logger.Log("SilentOps.ProcessAllActions", ex.Message, ex.StackTrace);
            }
        }
    }
}
