using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Diagnostics;
using System.Windows.Forms;

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

            InitializzeNlog();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        private static void InitializzeNlog()
        {
            //
            //  Setup Nlog
            //
            LoggingConfiguration config = LogManager.Configuration;
            if (config == null)
            {
                config = new LoggingConfiguration();
            }

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(AppDomain_CurrentDomain_UnhandledException);
            //
            //  Load Config
            //
            FileTarget fileTarget = new FileTarget();
            fileTarget.Name = "logfile";
            fileTarget.FileName = new ConfigWindows().FileNameLog;
            config.AddTarget(fileTarget.Name, fileTarget);
            LoggingRule loggingRule = new LoggingRule("*", LogLevel.Info, fileTarget);
            config.LoggingRules.Insert(0, loggingRule);
            LogManager.Configuration = config;
        }

        private static void AppDomain_CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            Exception ex = (Exception)args.ExceptionObject;
            logger.Error(ex);
        }
       
    }
}