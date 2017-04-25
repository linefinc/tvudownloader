using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TvUndergroundDownloaderConsole
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static private TvUndergroundDownloaderLib.Config MainConfig;
        private static TvUndergroundDownloaderLib.Worker worker;

        static void Main(string[] args)
        {
            //
            //  Setup Nlog
            //
            #region NLog setup
            LoggingConfiguration nLogConfig = LogManager.Configuration;
            if (nLogConfig == null)
            {
                nLogConfig = new LoggingConfiguration();
            }

            FileTarget fileTarget = new FileTarget();
            fileTarget.Name = "logfile";
            fileTarget.FileName = TvUndergroundDownloaderLib.Config.FileNameLog;
            nLogConfig.AddTarget(fileTarget.Name, fileTarget);
            LoggingRule m_loggingRule = new LoggingRule("*", LogLevel.Info, fileTarget);
            nLogConfig.LoggingRules.Insert(0, m_loggingRule);

            ConsoleTarget consoleTarget = new ConsoleTarget();
            consoleTarget.Name = "consoleTarget";
            nLogConfig.AddTarget(consoleTarget.Name, consoleTarget);
            nLogConfig.LoggingRules.Insert(0, new LoggingRule("*", LogLevel.Info, consoleTarget));

            LogManager.Configuration = nLogConfig;
            #endregion

            //
            //  Load config
            //
            logger.Info("TvUnderground Downloader starting");
            MainConfig = new TvUndergroundDownloaderLib.Config();
            MainConfig.Load();
            logger.Info("Loading config");

            #region Stat Web Server
            var embendedWebServer = new TvUndergroundDownloaderLib.EmbendedWebServer.EmbendedWebServer();
            embendedWebServer.Config = MainConfig;
            embendedWebServer.Start();
            #endregion



            bool exitProgram = true;
            while (exitProgram)
            {
                //
                //  run worker
                //
                worker = new TvUndergroundDownloaderLib.Worker();
                worker.Config = MainConfig;
                worker.Run();
                System.Threading.Thread.Sleep(TimeSpan.FromMinutes(10));
            }

            logger.Info("TvUnderground Downloader close");
        }
    }
}
