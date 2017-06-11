using NLog;
using NLog.Config;
using NLog.Targets;
using System;

namespace TvUndergroundDownloaderConsole
{
    internal class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static private TvUndergroundDownloaderLib.Config mainConfig;
        private static TvUndergroundDownloaderLib.Worker worker;

        private static void Main(string[] args)
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

            #endregion NLog setup

            //
            //  Load config
            //
            logger.Info("TvUnderground Downloader starting");
            mainConfig = new TvUndergroundDownloaderLib.Config();
            mainConfig.Load();
            logger.Info("Loading config");

            //
            //  Setup Worker
            //
            #region SetupWorker

            worker = new TvUndergroundDownloaderLib.Worker();
            worker.Config = mainConfig;

            #endregion SetupWorker

            //
            //  Start Web Server
            //
            #region Stat Web Server

            var embendedWebServer = new TvUndergroundDownloaderLib.EmbendedWebServer.EmbendedWebServer();
            embendedWebServer.Config = mainConfig;
            embendedWebServer.Worker = worker;
            embendedWebServer.Start();

            #endregion Stat Web Server

            bool exitProgram = true;
            while (exitProgram)
            {
                //
                //  run worker
                //
                worker.Run();
                
                System.Threading.Thread.Sleep(TimeSpan.FromMinutes(10));
            }

            logger.Info("TvUnderground Downloader close");
        }
    }
}