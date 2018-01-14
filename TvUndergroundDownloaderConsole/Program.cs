using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Diagnostics;

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
            try
            {
                mainConfig.Load("./Config.xml");
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
            
            logger.Info("Loading config");

            FileTarget fileTarget = new FileTarget();
            fileTarget.Name = "logfile";
            fileTarget.FileName = mainConfig.FileNameLog;
            nLogConfig.AddTarget(fileTarget.Name, fileTarget);
            LoggingRule m_loggingRule = new LoggingRule("*", LogLevel.Info, fileTarget);
            nLogConfig.LoggingRules.Insert(0, m_loggingRule);
            LogManager.Configuration = nLogConfig;
            
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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            worker.Run();

            while (exitProgram)
            {
                //
                //  run worker
                //
                if (stopwatch.Elapsed.Minutes > mainConfig.IntervalTime)
                {
                    worker.Run();
                    stopwatch.Reset();
                    stopwatch.Start();
                }
                System.Threading.Thread.Sleep(TimeSpan.FromMinutes(10));
            }

            logger.Info("TvUnderground Downloader close");
        }
    }
}