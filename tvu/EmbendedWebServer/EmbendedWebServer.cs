using Nancy.Hosting.Self;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;

namespace TvUndergroundDownloader.EmbendedWebServer
{
    public class EmbendedWebServer
    {
        private NancyHost nancyHost;
        private Logger logger = LogManager.GetCurrentClassLogger();

        public void Start()
        {
            // setup the logger for web pages
            LoggingConfiguration config = LogManager.Configuration;
            if (config == null)
            {
                config = new LoggingConfiguration();
            }

            MemoryTarget target = new MemoryTarget();
            target.Name = "MemoryTarget";
            target.Layout = "${message}";
            
            config.AddTarget(target.Name, target);

            LoggingRule m_loggingRule = new LoggingRule("*", LogLevel.Info, target);
            config.LoggingRules.Insert(0, m_loggingRule);
            LogManager.Configuration = config;

            //
            //  Start the server
            //
            logger.Info("Starting server...");
            HostConfiguration nancyHostConfiguration = new HostConfiguration();
            nancyHostConfiguration.RewriteLocalhost = false;
            nancyHostConfiguration.UrlReservations.CreateAutomatically = true;
            //Uri[] xxx = new Uri[] { new Uri("http://localhost:9696"), new Uri("http://192.168.1.48:9696")};
            Uri[] xxx = new Uri[] { new Uri("http://localhost:9696") };
            nancyHost = new NancyHost(nancyHostConfiguration, xxx);

            nancyHost.Start();
        }

        public void Stop()
        {
            logger.Info("Stopping server...");
            nancyHost.Stop();
        }
    }
}