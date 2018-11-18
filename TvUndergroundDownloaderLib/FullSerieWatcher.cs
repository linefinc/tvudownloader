using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using TvUndergroundDownloaderLib.Interfaces;

namespace TvUndergroundDownloaderLib
{
    public class FullSerieWatcher : ISubscription
    {
        public string Category { get; set; }
        public TvuStatus CurrentTVUStatus { get; }
        public string TVUStatus { get; }
        public DateTime LastUpdate { get; set; }
        public bool DeleteWhenCompleted { get; set; }
        public string DubLanguage { get; }
        public bool Enabled { get; set; }
        public int LastChannelUpdate { get; }
        public DateTime LastSerieStatusUpgradeDate { get; }
        public int MaxSimultaneousDownload { get; set; }
        public bool PauseDownload { get; set; }
        public int SeasonId { get; }
        public string Title { get; }
        public string TitleCompact { get; }
        public int TotalFilesDownloaded { get; }
        public string Url { get; }
        public DateTime? LastDownload { get; }
        public ReadOnlyCollection<DownloadFile> Files { get; }
        public ReadOnlyCollection<DownloadFile> DownloadedFiles { get; }


        public FullSerieWatcher(string inUrl, string dubLanguage,string subLanguage, CookieContainer cookieContainer)
        {
            Match match;
            var regexPageSource = new Regex("http(s)?://(www\\.)?((tvunderground)|(tvu)).org.ru/index.php\\?show=season&sid=(?<seid>\\d{1,10})");

            if (regexPageSource.IsMatch(inUrl))
            {
                match = regexPageSource.Match(inUrl);
                Url = string.Format(@"https://tvunderground.org.ru/rss.php?se_id={0}", match.Groups["seid"]);
            }
            else
            {
                Url = inUrl;
            }

            //match = _regexFeedSource.Match(Url);
            //if (!match.Success)
            //{
            //    throw new ApplicationException("Wrong URL");
            //}

            //int integerBuffer;
            //if (int.TryParse(match.Groups["seid"].ToString(), out integerBuffer) == false)
            //{
            //    throw new ApplicationException("Wrong URL");
            //}

            //SeasonId = integerBuffer;

            //string webPage = WebFetch.Fetch(Url, false, cookieContainer);

            //var doc = new XmlDocument();
            //doc.LoadXml(webPage);

            //var node = doc.SelectSingleNode(@"/rss/channel/title");

            //if (node == null)
            //{
            //    throw new WrongPageFormatException("Wrong RSS file format");
            //}

            //if (string.IsNullOrEmpty(node.InnerText))
            //{
            //    throw new WrongPageFormatException("Wrong RSS file format");
            //}
            //Title = node.InnerText;
        }

        public DateTime GetLastDownloadDate()
        {
            throw new NotImplementedException();
        }

        public List<Ed2kfile> GetNewDownload(int maxSimultaneousDownload)
        {
            throw new NotImplementedException();
        }

        public List<DownloadFile> GetPendingFiles()
        {
            throw new NotImplementedException();
        }

        public void SetFileDownloaded(Ed2kfile file)
        {
            throw new NotImplementedException();
        }

        public void SetFileNotDownloaded(Ed2kfile file)
        {
            throw new NotImplementedException();
        }

        public void Update(CookieContainer cookieContainer)
        {
            throw new NotImplementedException();
        }

        public void UpdateTVUStatus(CookieContainer cookieContainer)
        {
            throw new NotImplementedException();
        }

        public void Write(XmlTextWriter writer)
        {
            throw new NotImplementedException();
        }

        public Ed2kfile ProcessGuid(string url, CookieContainer cookieContainer)
        {
            throw new NotImplementedException();
        }
    }
}
