using Nancy.Hosting.Self;
using NLog;
using System;

namespace TvUndergroundDownloaderLib.EmbendedWebServer
{
    public class EmbendedWebServer
    {
        private NancyHost nancyHost;
        private Logger logger = LogManager.GetCurrentClassLogger();

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


        public void Start()
        {
            logger.Info("Starting server...");
            var NancyHostConfiguration = new HostConfiguration();
            NancyHostConfiguration.RewriteLocalhost = false;

            nancyHost = new NancyHost(NancyHostConfiguration, new Uri("http://localhost:9696"));

            nancyHost.Start();
            logger.Info("Server is listenign on {0}", "http://localhost:9696");
        }

        public void Stop()
        {
            logger.Info("Stopping server...");
            nancyHost.Stop();
        }
    }
}