using NLog;
using System;
using System.Text.RegularExpressions;
using System.Xml;

namespace TvUndergroundDownloaderLib
{
    public static class VersionChecker
    {
        public static bool CheckNewVersion(Config config, bool force = false)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Check New Version");

            if (config.IntervalBetweenUpgradeCheck == 0)
                return false;

            DateTime nextCheck = DateTime.MinValue;

            try
            {
                if (config.LastUpgradeCheck.HasValue)
                {
                    nextCheck = config.LastUpgradeCheck.Value.AddDays(config.IntervalBetweenUpgradeCheck);
                }

                if ((DateTime.Now < nextCheck) & (force == false))
                {
                    return false;
                }

                config.LastUpgradeCheck = DateTime.Now;

                // require update status
                var doc = new XmlDocument();
                doc.Load("http://tvudownloader.sourceforge.net/version.php");

                string lastVersionStr = "";

                foreach (XmlNode t in doc.GetElementsByTagName("last"))
                    lastVersionStr = t.InnerText;

                var rg = new Regex(@"\d+\.\d+.\d+");

                var match = rg.Match(lastVersionStr);
                var lastVersion = new Version(match.Value);

                // convert
                match = rg.Match(Config.VersionFull);
                var currentVersion = new Version(match.Value);

                string lastSupportedVersionStr = "";

                foreach (XmlNode t in doc.GetElementsByTagName("lastSupported"))
                    lastSupportedVersionStr = t.InnerText;

                match = rg.Match(lastSupportedVersionStr);
                var lastSupportedVersion = new Version(match.Value);

                if (currentVersion < lastSupportedVersion)
                {
                    throw new UnsupportedVersionException();
                }

                if (currentVersion < lastVersion)
                    return true;

                return false;
            }
            catch (UnsupportedVersionException ex)
            {
                logger.Error(ex);
                throw;
            }
            catch
            {
                return false;
            }

            return false;
        }
    }
}