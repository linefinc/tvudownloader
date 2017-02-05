using Nancy.Hosting.Self;
using NLog;
using System;

namespace TvUndergroundDownloader.EmbendedWebServer
{
    public class EmbendedWebServer
    {
        private NancyHost nancyHost;
        private Logger logger = LogManager.GetCurrentClassLogger();

        public void Start()
        {
            logger.Info("Starting server...");
            var NancyHostConfiguration = new HostConfiguration();
            NancyHostConfiguration.RewriteLocalhost = false;

            nancyHost = new NancyHost(NancyHostConfiguration, new Uri("http://localhost:9696"));

            nancyHost.Start();
        }

        public void Stop()
        {
            logger.Info("Stopping server...");
            nancyHost.Stop();
        }
    }
}