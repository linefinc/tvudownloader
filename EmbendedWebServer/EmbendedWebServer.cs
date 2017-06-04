using Nancy.Hosting.Self;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;

namespace TvUndergroundDownloaderLib.EmbendedWebServer
{
    public class EmbendedWebServer
    {
        private const string nancyUrl = "http://localhost:9696";
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private NancyHost nancyHost;

        /// <summary>
        /// configuration class
        /// </summary>
        public Config Config
        {
            set
            {
                GlobalVar.Config = value;
            }
            get
            {
                return GlobalVar.Config;
            }
        }

        public Worker Worker
        {
            set
            {
                GlobalVar.Worker = value;
            }
            get
            {
                return GlobalVar.Worker;
            }
        }


        public void Start()
        {
            //
            //  Setup Nlog
            //

            #region Setup NLog

            LoggingConfiguration nLogConfig = LogManager.Configuration;
            if (nLogConfig == null)
            {
                nLogConfig = new LoggingConfiguration();
            }

            GlobalVar.LogMemoryTarget = new MemoryTarget();
            GlobalVar.LogMemoryTarget.Layout = "${message}";

            LoggingRule loggingRule = new LoggingRule("*", LogLevel.Info, GlobalVar.LogMemoryTarget);
            nLogConfig.LoggingRules.Insert(0, loggingRule);

            LogManager.Configuration = nLogConfig;

            #endregion Setup NLog

            logger.Info("Starting server...");
            HostConfiguration nancyHostConfiguration = new HostConfiguration();

            bool reservationCreationFailure = false;

            try
            {
                nancyHostConfiguration.RewriteLocalhost = true;
                nancyHost = new NancyHost(nancyHostConfiguration, new Uri(nancyUrl));
                nancyHost.Start();
                logger.Info("Server is listening on {0}", nancyUrl);
            }
            catch (AutomaticUrlReservationCreationFailureException e)
            {
                logger.Error(e, "Reservation Creation Failure Exception");
                reservationCreationFailure = true;
            }


            if (reservationCreationFailure == true)
            {
                logger.Info("Try to restart without reservation");

                nancyHostConfiguration.RewriteLocalhost = false;
                nancyHost = new NancyHost(nancyHostConfiguration, new Uri(nancyUrl));
                nancyHost.Start();
                logger.Info("Server is listening on {0}", nancyUrl);
            }

        }

        public void Stop()
        {
            logger.Info("Stopping server...");
            nancyHost.Stop();
        }
    }
}