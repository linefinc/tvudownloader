using NLog.Targets;
using System.ComponentModel;

namespace TvUndergroundDownloaderLib.EmbendedWebServer
{
    public static class GlobalVar
    {
        public static Config Config;
        public static BackgroundWorker MainBackgroundWorker;
        public static MemoryTarget LogMemoryTarget;
    }
}
