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

                var osv = Environment.OSVersion;

                var geoIP = GeoIP.GetCountryFromWeb();

                // require update status
                var doc = new XmlDocument();
                string url =
                    string.Format(
                        "http://tvudownloader.sourceforge.net/version.php?ver={0}&tvuid={1}&TotalDownloads={2}&osv={3}&countryCode={4}",
                        Config.VersionFull.Replace(" ", "%20"),
                        config.tvudwid,
                        config.TotalDownloads,
                        osv.VersionString.Replace(" ", "%20"),
                        geoIP.CountryCode);
                doc.Load(url);

                string lastVersionStr = "";

                foreach (XmlNode t in doc.GetElementsByTagName("last"))
                    lastVersionStr = t.InnerText;

                var rg = new Regex(@"\d+\.\d+.\d+");

                var match = rg.Match(lastVersionStr);
                var lastVersion = new Version(match.Value);

                // convert
                match = rg.Match(Config.VersionFull);
                var currentVersion = new Version(match.Value);

                if (currentVersion < lastVersion)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}