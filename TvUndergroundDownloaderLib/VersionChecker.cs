using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace TvUndergroundDownloaderLib
{
    class VersionChecker
    {
        public static bool CheckNewVersion(Config config, bool force = false)
        {
            if (config.intervalBetweenUpgradeCheck == 0)
            {
                return false;
            }

            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;

                DateTime lastCheckDateTime = DateTime.ParseExact(config.LastUpgradeCheck, "yyyy-MM-dd", provider);
                DateTime nextCheck = lastCheckDateTime.AddDays(config.intervalBetweenUpgradeCheck);

                lastCheckDateTime = DateTime.ParseExact(config.LastUpgradeCheck, "yyyy-MM-dd", provider);

                if ((DateTime.Now < nextCheck) & (force == false))
                {
                    return false;
                }

                config.LastUpgradeCheck = DateTime.Now.ToString("yyyy-MM-dd");

                OperatingSystem osv = Environment.OSVersion;

                GeoIP geoIP = GeoIP.GetCountryFromWeb();

                // require update status
                XmlDocument doc = new XmlDocument();
                string url = string.Format("http://tvudownloader.sourceforge.net/version.php?ver={0}&tvuid={1}&TotalDownloads={2}&osv={3}&countryCode={4}",
                    Config.VersionFull.Replace(" ", "%20"),
                    config.tvudwid,
                    config.TotalDownloads,
                    osv.VersionString.Replace(" ", "%20"),
                   geoIP.CountryCode);
                doc.Load(url);

                string lastVersionStr = "";

                foreach (XmlNode t in doc.GetElementsByTagName("last"))
                {
                    lastVersionStr = t.InnerText;
                }

                Regex rg = new Regex(@"\d+\.\d+.\d+");

                Match match = rg.Match(lastVersionStr);
                Version lastVersion = new Version(match.Value);

                // convert
                match = rg.Match(Config.VersionFull);
                Version currentVersion = new Version(match.Value);

                if (currentVersion < lastVersion)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

    }
}
