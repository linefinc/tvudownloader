using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using NLog;

namespace TvUndergroundDownloaderLib
{
    public enum TvuStatus
    {
        Complete,
        StillRunning,
        Unknown,
        StillIncomplete,
        OnHiatus
    }

    /// <summary>
    ///     Rss submission container
    /// </summary>
    public class RssSubscription
    {
        public static Regex regexFeedSource =
            new Regex(@"http(s)?://(www\.)?((tvunderground)|(tvu)).org.ru/rss.php\?se_id=(?<seid>\d{1,10})");

        public DateTime LastSerieStatusUpgradeDate = DateTime.MinValue;
        public string LastUpgradeDate = string.Empty;
        public uint MaxSimultaneousDownload = 3;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static Regex _regexEdk2Link = new Regex(@"ed2k://\|file\|(.*)\|\d+\|\w+\|/");

        private static Regex _regexFeedLink =
                new Regex(
                    @"http(s)?://(www\.)?((tvunderground)|(tvu)).org.ru/index.php\?show=ed2k&season=(?<season>\d{1,10})&sid\[(?<sid>\d{1,10})\]=\d{1,10}")
            ;

        private readonly List<DownloadFile> _downloadFiles = new List<DownloadFile>();
        /// <summary>
        ///     Build a Rss Subscription
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param>
        public RssSubscription(string title, string url)
        {
            this.Title = title;
            this.Url = url;

            // Static Regex "http(s)?://(www\.)?tvunderground.org.ru/rss.php\?se_id=(\d{1,10})"
            var matchCollection = regexFeedSource.Matches(this.Url);
            if (matchCollection.Count == 0)
                throw new ApplicationException("Wrong URL");

            int integerBuffer;
            if (int.TryParse(matchCollection[0].Groups["seid"].ToString(), out integerBuffer) == false)
                throw new ApplicationException("Wrong URL");

            SeasonId = integerBuffer;
        }

        public RssSubscription(string newUrl, CookieContainer cookieContainer)
        {
            Url = newUrl;
            var matchCollection = regexFeedSource.Matches(newUrl);
            if (matchCollection.Count == 0)
                throw new ApplicationException("Wrong URL");

            int integerBuffer;
            if (int.TryParse(matchCollection[0].Groups["seid"].ToString(), out integerBuffer) == false)
                throw new ApplicationException("Wrong URL");
            SeasonId = integerBuffer;

            string webPageUrl = Url;
            string webPage = WebFetch.Fetch(Url, false, cookieContainer);

            var doc = new XmlDocument();
            doc.LoadXml(webPage);

            var node = doc.SelectSingleNode(@"/rss/channel/title");

            if (node == null)
                throw new ApplicationException("Wrong RSS file format");
            if (string.IsNullOrEmpty(node.InnerText))
                throw new ApplicationException("Wrong RSS file format");
            Title = node.InnerText;
        }

        public string Category { get; set; } = string.Empty;
        public TvuStatus CurrentTVUStatus { get; private set; } = TvuStatus.Unknown;

        public string DubLanguage
        {
            get
            {
                if (Title.IndexOf("english") > -1) return "gb";
                if (Title.IndexOf("french") > -1) return "fr";
                if (Title.IndexOf("german") > -1) return "de";
                if (Title.IndexOf("italian") > -1) return "it";
                if (Title.IndexOf("japanese") > -1) return "jp";
                if (Title.IndexOf("spanish") > -1) return "es";
                return string.Empty;
            }
        }

        public bool Enabled { get; set; } = true;
        public TimeSpan LastChannelUpdate => DateTime.Now - GetLastDownloadDate();
        public bool PauseDownload { get; set; }
        public int SeasonId { get; }
        public string Title { get; }
        public string TitleCompact => Title.Replace("[ed2k] tvunderground.org.ru:", "").Trim();
        public int TotalFilesDownloaded
        {
            get { return _downloadFiles.FindAll(o => o.DownloadDate.HasValue).Count; }
        }

        public string Url { get; }

        /// <summary>
        ///     Load data from xml
        /// </summary>
        /// <param name="doc">XML node with RSS subscription data</param>
        /// <returns></returns>
        public static RssSubscription LoadFormXml(XmlNode doc)
        {
            if (doc == null)
            {
                throw new NullReferenceException("Doc is null");
            }

            var node = doc.SelectSingleNode("Title");
            if (node == null)
            {
                throw new Exception("Error while loading xml: Missing RssSubscrission\\Title");
            }
            string title = node.InnerText;

            node = doc.SelectSingleNode("Url");
            if (node == null)
            {
                throw new Exception("Error while loading xml: Missing RssSubscrission\\Url");
            }

            string url = node.InnerText;
            var newRssSubscrission = new RssSubscription(title, url);

            node = doc.SelectSingleNode("Pause");
            if (node != null)
            {
                newRssSubscrission.PauseDownload = Convert.ToBoolean(node.InnerText);
            }

            node = doc.SelectSingleNode("Category");
            if (node != null)
            {
                newRssSubscrission.Category = node.InnerText;
            }

            node = doc.SelectSingleNode("LastUpgradeDate");
            if (node != null)
            {
                newRssSubscrission.LastUpgradeDate = node.InnerText;
            }

            node = doc.SelectSingleNode("Enabled");
            if (node != null)
            {
                newRssSubscrission.Enabled = Convert.ToBoolean(node.InnerText);
            }

            if (node != null)
            {
                node = doc.SelectSingleNode("tvuStatus");
            }
            if (node != null)
            {
                switch (node.InnerText)
                {
                    case "Complete":
                        newRssSubscrission.CurrentTVUStatus = TvuStatus.Complete;
                        break;

                    case "StillIncomplete":
                        newRssSubscrission.CurrentTVUStatus = TvuStatus.StillIncomplete;
                        break;

                    case "StillRunning":
                        newRssSubscrission.CurrentTVUStatus = TvuStatus.StillRunning;
                        break;

                    case "OnHiatus":
                        newRssSubscrission.CurrentTVUStatus = TvuStatus.OnHiatus;
                        break;

                    case "Unknown":
                    default:
                        newRssSubscrission.CurrentTVUStatus = TvuStatus.Unknown;
                        break;
                }
            }
            node = doc.SelectSingleNode("maxSimultaneousDownload");
            if (node != null)
            {
                newRssSubscrission.MaxSimultaneousDownload = Convert.ToUInt16(node.InnerText);
            }
            var filesNode = doc.SelectNodes("Files/File");
            foreach (XmlNode fileNode in filesNode)
            {
                var ed2kLinkNode = fileNode.SelectSingleNode("LinkED2K");
                if (ed2kLinkNode == null)
                    continue;

                var newFile = new Ed2kfile(ed2kLinkNode.InnerText);


                var guidLinkNode = fileNode.SelectSingleNode("Guid");
                if (guidLinkNode == null)
                    continue;

                var dwFile = new DownloadFile(newRssSubscrission, newFile, guidLinkNode.InnerText);
                newRssSubscrission._downloadFiles.Add(dwFile);

                var downloadedNode = fileNode.SelectSingleNode("Downloaded");
                if (downloadedNode != null)
                {
                    var dtDownloaded = DateTime.Parse(downloadedNode.InnerText);
                    dwFile.DownloadDate = dtDownloaded;
                }

                var publicationDateNode = fileNode.SelectSingleNode("PublicationDate");
                if (publicationDateNode != null)
                {
                    var publicationDateNodeDt = DateTime.Parse(publicationDateNode.InnerText);
                    dwFile.PublicationDate = publicationDateNodeDt;
                }
            }

            return newRssSubscrission;
        }

        [Obsolete]
        public void AddFile(DownloadFile file)
        {
            if (!_downloadFiles.Contains(file))
                _downloadFiles.Add(file);
        }

        public List<Ed2kfile> GetAllFile()
        {
            return new List<Ed2kfile>(GetDownloadFile());
        }

        public List<Ed2kfile> GetDownloadedFiles()
        {
            var outArray = new List<Ed2kfile>();
            outArray.AddRange(_downloadFiles.FindAll(o => o.DownloadDate.HasValue));
            outArray.Sort((a, b) => string.Compare(a.FileName, b.FileName, StringComparison.InvariantCulture));
            return outArray;
        }

        /// <summary>
        ///     Return all files available in the subscrission download or not
        /// </summary>
        /// <returns></returns>
        /// TODO: Convert to property
        public List<DownloadFile> GetDownloadFile()
        {
            var outArray = new List<DownloadFile>();
            outArray.AddRange(_downloadFiles);
            return outArray;
        }

        public int GetDownloadFileCount()
        {
            return _downloadFiles.Count;
        }

        public DateTime GetLastDownloadDate()
        {
            var dt = DateTime.MinValue;
            foreach (var file in _downloadFiles)
                if (file.DownloadDate.HasValue)
                {
                    var value = file.DownloadDate.Value;
                    if (dt < value)
                        dt = value;
                }
            return dt;
        }

        public List<Ed2kfile> GetNewDownload(int maxSimultaneousDownload)
        {
            var outArray = new List<Ed2kfile>();

            outArray.AddRange(_downloadFiles.FindAll(o => o.DownloadDate.HasValue == false));
            outArray.Sort((A, B) => A.FileName.CompareTo(B.FileName));
            if (outArray.Count > MaxSimultaneousDownload)
                outArray.RemoveRange(maxSimultaneousDownload, outArray.Count - maxSimultaneousDownload);

            return outArray;
        }

        /// <summary>
        ///     Return all files that are waiting for download
        /// </summary>
        /// <returns></returns>
        public List<DownloadFile> GetPendingFiles()
        {
            var outArray = new List<DownloadFile>();

            outArray.AddRange(_downloadFiles.FindAll(o => o.DownloadDate.HasValue == false));

            return outArray;
        }

        public void SetFileDownloaded(Ed2kfile file)
        {
            var localFile = _downloadFiles.Find(o => o.Equals(file as DownloadFile));
            if (localFile == null)
                throw new Exception("File missing in feed");
            localFile.DownloadDate = DateTime.Now;
        }

        public void SetFileNotDownloaded(Ed2kfile file)
        {
            var localFile = _downloadFiles.Find(o => o.Equals(file as DownloadFile));
            if (localFile == null)
                throw new Exception("File missing in feed");
            localFile.DownloadDate = null;
        }


        /// <summary>
        ///     Update feed from Web server
        /// </summary>
        public void Update(CookieContainer cookieContainer)
        {
            if (cookieContainer.Count == 0)
            {
                throw new LoginException();
            }

            var uri = new Uri(Url);
            var cookies = cookieContainer.GetCookies(uri);

            if (string.IsNullOrWhiteSpace(cookies["h"].Value))
            {
                throw new LoginException();
            }

            if (string.IsNullOrWhiteSpace(cookies["i"].Value))
            {
                throw new LoginException();
            }

            if (string.IsNullOrWhiteSpace(cookies["t"].Value))
            {
                throw new LoginException();
            }

            string webPage = WebFetch.Fetch(Url, false, cookieContainer);

            var doc = new XmlDocument();
            doc.LoadXml(webPage);

            var nodeList = doc.SelectNodes(@"/rss/channel/item");
            foreach (XmlNode itemNode in nodeList)
            {
                var guidNode = itemNode.SelectSingleNode("guid");
                string guid = HttpUtility.UrlDecode(guidNode.InnerText);


                // here check if the file is already downloaded
                var newFile = _downloadFiles.Find(o => o.Guid == guid);

                if (newFile == null)
                {
                    try
                    {
                        //
                        //  try to load data from remote server
                        //
                        var newEd2kFile = ProcessGuid(guid, cookieContainer);

                        _logger.Info("Found new file {0}", newEd2kFile.FileName);
                        newFile = new DownloadFile(this, newEd2kFile, guid);

                        _downloadFiles.Add(newFile);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Error while try to parse or fatch GUID");
                        continue;
                    }

                    //
                    //  try to update pudDate
                    //
                    if (!newFile.PublicationDate.HasValue)
                    {
                        var pubDateNode = itemNode.SelectSingleNode("pubDate");
                        if (pubDateNode != null)
                        {
                            string pubDateStr = HttpUtility.UrlDecode(pubDateNode.InnerText);
                            if (!string.IsNullOrEmpty(pubDateStr))
                            {
                                DateTime pubDateDateTime;
                                if (DateTime.TryParse(pubDateStr, out pubDateDateTime))
                                {
                                    newFile.PublicationDate = pubDateDateTime;
                                }
                            }
                        }
                    }
                }
            }

            if (CurrentTVUStatus == TvuStatus.Complete)
                return;

            var ts = DateTime.Now - LastSerieStatusUpgradeDate;

            if (ts.TotalDays < 15)
                return;
            UpdateTVUStatus(cookieContainer);
        }

        public void UpdateTVUStatus(CookieContainer cookieContainer)
        {
            _logger.Info("Checking serie status");
            string url = string.Format(@"http://tvunderground.org.ru/index.php?show=episodes&sid={0}", SeasonId);

            string WebPage = WebFetch.Fetch(url, true, cookieContainer);
            LastSerieStatusUpgradeDate = DateTime.Now;
            if (WebPage != null)
            {
                if (WebPage.IndexOf("Still Running") > 0)
                {
                    CurrentTVUStatus = TvuStatus.StillRunning;
                    _logger.Info("Serie status: Still Running");
                    return;
                }

                if (WebPage.IndexOf("Complete") > 0)
                {
                    CurrentTVUStatus = TvuStatus.Complete;
                    _logger.Info("Serie status: Complete");
                    return;
                }

                if (WebPage.IndexOf("Still Incomplete") > 0)
                {
                    CurrentTVUStatus = TvuStatus.StillIncomplete;
                    _logger.Info("Serie status: Still Incomplete");
                    return;
                }

                if (WebPage.IndexOf("On Hiatus") > 0)
                {
                    _logger.Info("Serie status: On Hiatus");
                    CurrentTVUStatus = TvuStatus.OnHiatus;
                    return;
                }
            }

            CurrentTVUStatus = TvuStatus.Unknown;
            _logger.Info("Serie status: Unknown");
        }

        public void Write(XmlTextWriter writer)
        {
            //<Channel>
            //  <Title>[ed2k] tvunderground.org.ru: Lie To Me - Season 2 (HDTV) italian </Title>
            //  <Url>http://tvunderground.org.ru/rss.php?se_id=32672</Url>
            //  <Pause>False</Pause>
            //  <Category>Anime</Category>
            //</Channel>

            writer.WriteStartElement("Channel"); //Title
            writer.WriteAttributeString("type", "TVU");
            writer.WriteElementString("Title", Title); //Title
            writer.WriteElementString("Url", Url); //Url
            writer.WriteElementString("Pause", PauseDownload.ToString()); //Category
            writer.WriteElementString("Category", Category); //Category
            writer.WriteElementString("LastUpgradeDate", LastUpgradeDate); //Last Upgrade Date
            writer.WriteElementString("Enabled", Enabled.ToString());
            writer.WriteElementString("maxSimultaneousDownload", MaxSimultaneousDownload.ToString());

            switch (CurrentTVUStatus)
            {
                case TvuStatus.Complete:
                    writer.WriteElementString("tvuStatus", "Complete");
                    break;

                case TvuStatus.StillIncomplete:
                    writer.WriteElementString("tvuStatus", "StillIncomplete");
                    break;

                case TvuStatus.StillRunning:
                    writer.WriteElementString("tvuStatus", "StillRunning");
                    break;

                case TvuStatus.OnHiatus:
                    writer.WriteElementString("tvuStatus", "OnHiatus");
                    break;

                case TvuStatus.Unknown:
                default:
                    writer.WriteElementString("tvuStatus", "Unknown");
                    break;
            }

            writer.WriteStartElement("Files");
            foreach (var file in _downloadFiles)
            {
                writer.WriteStartElement("File");
                writer.WriteElementString("LinkED2K", file.Ed2kLink);
                writer.WriteElementString("Guid", file.Guid);
                if (file.DownloadDate.HasValue)
                    writer.WriteElementString("Downloaded",
                        file.DownloadDate.Value.ToString("s", CultureInfo.InvariantCulture));

                if (file.PublicationDate.HasValue)
                    writer.WriteElementString("PublicationDate",
                        file.PublicationDate.Value.ToString("s", CultureInfo.InvariantCulture));
                writer.WriteEndElement(); // end file
            }
            writer.WriteEndElement(); // end file

            writer.WriteEndElement(); // end channel
        }

        private Ed2kfile ProcessGuid(string url, CookieContainer cookieContainer)
        {
            _logger.Info("Get page {0}", url);
            string webPage = WebFetch.Fetch(url, false, cookieContainer);
            //
            int i = webPage.IndexOf("ed2k://|file|", StringComparison.InvariantCulture);
            webPage = webPage.Substring(i);
            i = webPage.IndexOf("|/", StringComparison.InvariantCulture);
            webPage = webPage.Substring(0, i + "|/".Length);
            return new Ed2kfile(webPage);
        }
    }
}