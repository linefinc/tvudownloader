using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using TvUndergroundDownloaderLib;

namespace TvUndergroundDownloader
{
    internal static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Process aProcess = Process.GetCurrentProcess();
            string aProcName = aProcess.ProcessName;

            if (Process.GetProcessesByName(aProcName).Length > 1)
            {
                MessageBox.Show("Application just started");
                Application.ExitThread();
                return;
            }

            //
            //  Setup Nlog
            //
            LoggingConfiguration config = LogManager.Configuration;
            if (config == null)
            {
                config = new LoggingConfiguration();
            }

            //
            //  Load Config
            //
            ConfigWindows configWindows = new ConfigWindows();
            if (configWindows.SaveLog == false)
            {
                FileTarget fileTarget = new FileTarget();
                fileTarget.Name = "logfile";
                fileTarget.FileName = configWindows.FileNameLog;
                config.AddTarget(fileTarget.Name, fileTarget);
                LoggingRule loggingRule = new LoggingRule("*", LogLevel.Info, fileTarget);
                config.LoggingRules.Insert(0, loggingRule);
                LogManager.Configuration = config;
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}