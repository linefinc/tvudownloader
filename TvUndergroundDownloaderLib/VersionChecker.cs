using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;
using NLog;

namespace TvUndergroundDownloaderLib
{
    public static class VersionChecker
    {
        public static bool CheckNewVersion(Config config, bool force = false)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Check New Version");

            if (config.intervalBetweenUpgradeCheck == 0)
                return false;

            try
            {
                var provider = CultureInfo.InvariantCulture;

                var lastCheckDateTime = DateTime.ParseExact(config.LastUpgradeCheck, "yyyy-MM-dd", provider);
                var nextCheck = lastCheckDateTime.AddDays(config.intervalBetweenUpgradeCheck);

                lastCheckDateTime = DateTime.ParseExact(config.LastUpgradeCheck, "yyyy-MM-dd", provider);

                if ((DateTime.Now < nextCheck) & (force == false))
                    return false;

                config.LastUpgradeCheck = DateTime.Now.ToString("yyyy-MM-dd");

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