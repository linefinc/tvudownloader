﻿using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TvUndergroundDownloader
{
    internal static class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            //
            //  Enable app level error traking
            //
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(AppDomain_CurrentDomain_UnhandledException);

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

            FileTarget fileTarget = new FileTarget();
            fileTarget.Name = "logfile";
            fileTarget.FileName = Config.FileNameLog;
            config.AddTarget(fileTarget.Name, fileTarget);
            LoggingRule m_loggingRule = new LoggingRule("*", LogLevel.Info, fileTarget);
            config.LoggingRules.Insert(0, m_loggingRule);
            LogManager.Configuration = config;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        private static void AppDomain_CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;
            logger.Error(ex);
        }
    }
}