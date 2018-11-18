using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using TvUndergroundDownloaderLib.Interfaces;

namespace TvUndergroundDownloaderLib
{
    public enum TvuStatus
    {
        Complete,
        StillRunning,
        Unknown,
        StillIncomplete,
        OnHiatus,
        Error
    }

    /// <summary>
    ///     Rss submission container
    /// </summary>
    public class RssSubscription : ISubscription
    {
        private static readonly Regex _regexPageSource = new Regex(@"http(s)?://(www\.)?((tvunderground)|(tvu)).org.ru/index.php\?show=episodes&sid=(?<seid>\d{1,10})");

        private static readonly Regex _regexFeedSource = new Regex(@"http(s)?://(www\.)?((tvunderground)|(tvu)).org.ru/rss.php\?se_id=(?<seid>\d{1,10})");

        private static Regex _regexEdk2Link = new Regex(@"ed2k://\|file\|(.*)\|\d+\|\w+\|/");

        private static Regex _regexFeedLink =
                new Regex(
                    @"http(s)?://(www\.)?((tvunderground)|(tvu)).org.ru/index.php\?show=ed2k&season=(?<season>\d{1,10})&sid\[(?<sid>\d{1,10})\]=\d{1,10}");

        private readonly List<DownloadFile> _downloadFiles = new List<DownloadFile>();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Build a Rss Subscription
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param>
        public RssSubscription(string title, string url)
        {
            this.Title = title;
            this.Url = url;

            var matchCollection = _regexFeedSource.Matches(this.Url);
            if (matchCollection.Count == 0)
                throw new ApplicationException("Wrong URL");

            int integerBuffer;
            if (int.TryParse(matchCollection[0].Groups["seid"].ToString(), out integerBuffer) == false)
                throw new ApplicationException("Wrong URL");

            SeasonId = integerBuffer;
        }

        /// <summary>
        /// Initialize a new Feed By fatch online URL
        /// </summary>
        /// <param name="inUrl"></param>
        /// <param name="cookieContainer"></param>
        public RssSubscription(string inUrl, CookieContainer cookieContainer)
        {
            Match match;

            if (_regexPageSource.IsMatch(inUrl))
            {
                match = _regexPageSource.Match(inUrl);
                Url = string.Format(@"https://tvunderground.org.ru/rss.php?se_id={0}", match.Groups["seid"]);
            }
            else
            {
                Url = inUrl;
            }

            match = _regexFeedSource.Match(Url);
            if (!match.Success)
            {
                throw new ApplicationException("Wrong URL");
            }

            int integerBuffer;
            if (int.TryParse(match.Groups["seid"].ToString(), out integerBuffer) == false)
            {
                throw new ApplicationException("Wrong URL");
            }

            SeasonId = integerBuffer;

            string webPage = WebFetch.Fetch(Url, false, cookieContainer);

            var doc = new XmlDocument();
            doc.LoadXml(webPage);

            var node = doc.SelectSingleNode(@"/rss/channel/title");

            if (node == null)
            {
                throw new WrongPageFormatException("Wrong RSS file format");
            }

            if (string.IsNullOrEmpty(node.InnerText))
            {
                throw new WrongPageFormatException("Wrong RSS file format");
            }
            Title = node.InnerText;
        }

        public string Category { get; set; } = string.Empty;
        public TvuStatus CurrentTVUStatus { get; private set; } = TvuStatus.Unknown;

        /// <summary>
        /// TV Underground status human readable
        /// </summary>
        public string TVUStatus
        {
            get
            {
                switch (CurrentTVUStatus)
                {
                    case TvuStatus.Complete:
                        return "Complete";

                    case TvuStatus.StillRunning:
                        return "Still Running";

                    case TvuStatus.Unknown:
                        return "Unknown";

                    case TvuStatus.StillIncomplete:
                        return "Still Incomplete";

                    case TvuStatus.OnHiatus:
                        return "On Hiatus";

                    default:
                        return "Error";
                }
            }
        }

        public DateTime LastUpdate { get; set; } = DateTime.MinValue;
        public bool DeleteWhenCompleted { get; set; } = false;

        public string DubLanguage
        {
            get
            {
                if (Title.IndexOf("english", 0, StringComparison.InvariantCulture) > -1) return "gb";
                if (Title.IndexOf("french", 0, StringComparison.InvariantCulture) > -1) return "fr";
                if (Title.IndexOf("german", 0, StringComparison.InvariantCulture) > -1) return "de";
                if (Title.IndexOf("italian", 0, StringComparison.InvariantCulture) > -1) return "it";
                if (Title.IndexOf("japanese", 0, StringComparison.InvariantCulture) > -1) return "jp";
                if (Title.IndexOf("spanish", 0, StringComparison.InvariantCulture) > -1) return "es";
                return string.Empty;
            }
        }

        public bool Enabled { get; set; } = true;

        public int LastChannelUpdate
        {
            get
            {
                var diff = DateTime.Now - GetLastDownloadDate();
                return Convert.ToInt32(diff.TotalDays);
            }
        }


        public DateTime LastSerieStatusUpgradeDate { get; private set; } = DateTime.MinValue;
        public int MaxSimultaneousDownload { get; set; } = 3;
        public bool PauseDownload { get; set; }

        public int SeasonId { get; }

        public string Title { get; private set; }

        public string TitleCompact => Title.Replace("[ed2k] tvunderground.org.ru:", "").Trim();

        public int TotalFilesDownloaded
        {
            get { return _downloadFiles.FindAll(o => o.DownloadDate.HasValue).Count; }
        }

        public string Url { get; }

        /// <summary>
        /// Last download
        /// </summary>
        /// <remarks>Can be null</remarks>
        public DateTime? LastDownload
        {
            get
            {
                if (_downloadFiles == null)
                {
                    return null;
                }

                if (_downloadFiles.Count == 0)
                {
                    return null;
                }

                return _downloadFiles.Where(o => o.DownloadDate.HasValue).Max(o => o.DownloadDate.Value);
            }
        }

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

            node = doc.SelectSingleNode(nameof(LastUpdate));
            if (node != null)
            {
                DateTime dt;
                if (DateTime.TryParse(node.InnerText, out dt))
                {
                    newRssSubscrission.LastUpdate = dt;
                }
            }

            node = doc.SelectSingleNode("Enabled");
            if (node != null)
            {
                newRssSubscrission.Enabled = Convert.ToBoolean(node.InnerText);
            }

            node = doc.SelectSingleNode(nameof(DeleteWhenCompleted));
            if (node != null)
            {
                newRssSubscrission.DeleteWhenCompleted = Convert.ToBoolean(node.InnerText);
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

        public static IEnumerable<string> ParsePossibleUrl(string text)
        {
            List<string> list = new List<string>();

            foreach (Match match in _regexFeedSource.Matches(text))
            {
                if (!list.Contains(match.Value))
                    list.Add(match.Value);
            }

            foreach (Match match in _regexPageSource.Matches(text))
            {
                if (!list.Contains(match.Value))
                    list.Add(match.Value);
            }

            return list;
        }

        /// <summary>
        /// List of all files
        /// </summary>
        public ReadOnlyCollection<DownloadFile> Files => _downloadFiles.AsReadOnly();

        /// <summary>
        /// List of all downloaded files
        /// </summary>
        public ReadOnlyCollection<DownloadFile> DownloadedFiles
        {
            get
            {
                return _downloadFiles.FindAll(o => o.DownloadDate.HasValue)
                    .OrderBy(o => o.FileName).ToList().AsReadOnly();
            }
        }

        /// <summary>
        /// Last download date
        /// </summary>
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

        [Obsolete]
        public List<Ed2kfile> GetNewDownload(int maxSimultaneousDownload)
        {
            var outArray = new List<Ed2kfile>();

            outArray.AddRange(_downloadFiles.FindAll(o => o.DownloadDate.HasValue == false));
            outArray.Sort((a, b) => String.Compare(a.FileName, b.FileName, StringComparison.Ordinal));
            while (outArray.Count > MaxSimultaneousDownload)
            {
                outArray.Remove(outArray.Last());
            }
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
            outArray.Sort((a, b) => String.Compare(a.FileName, b.FileName, StringComparison.Ordinal));
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

            try
            {
                string webPage = WebFetch.Fetch(Url, false, cookieContainer);

                var doc = new XmlDocument();
                doc.LoadXml(webPage);

                var nodeList = doc.SelectNodes(@"/rss/channel/item");
                if (nodeList == null)
                {
                    throw new WrongPageFormatException();
                }

                foreach (XmlNode itemNode in nodeList)
                {
                    var guidNode = itemNode.SelectSingleNode("guid");

                    if (guidNode == null)
                    {
                        throw new WrongPageFormatException();
                    }

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

                LastUpdate = DateTime.Now;
                var ts = DateTime.Now - LastSerieStatusUpgradeDate;
                if (ts.TotalDays > 15 || (CurrentTVUStatus == TvuStatus.Error))
                {
                    UpdateTVUStatus(cookieContainer);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
                CurrentTVUStatus = TvuStatus.Error;
                throw;
            }
        }

        public void UpdateTVUStatus(CookieContainer cookieContainer)
        {
            _logger.Info("Checking serie status: \"{0}\"", this.TitleCompact);
            string url = string.Format(@"http://tvunderground.org.ru/index.php?show=episodes&sid={0}", SeasonId);

            string webPage = WebFetch.Fetch(url, true, cookieContainer);

            if (webPage != null)
            {
                LastSerieStatusUpgradeDate = DateTime.Now;
                if (webPage.IndexOf("Still Running", 0, StringComparison.InvariantCulture) > 0)
                {
                    CurrentTVUStatus = TvuStatus.StillRunning;
                    _logger.Info("Serie status: Still Running");
                    return;
                }

                if (webPage.IndexOf("Complete", 0, StringComparison.InvariantCulture) > 0)
                {
                    CurrentTVUStatus = TvuStatus.Complete;
                    _logger.Info("Serie status: Complete");
                    return;
                }

                if (webPage.IndexOf("Still Incomplete", 0, StringComparison.InvariantCulture) > 0)
                {
                    CurrentTVUStatus = TvuStatus.StillIncomplete;
                    _logger.Info("Serie status: Still Incomplete");
                    return;
                }

                if (webPage.IndexOf("On Hiatus", 0, StringComparison.InvariantCulture) > 0)
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
            if (DeleteWhenCompleted)
            {
                int count = _downloadFiles.Count(o => o.DownloadDate.HasValue == false);

                if ((CurrentTVUStatus == TvuStatus.Complete) && (count == 0))
                {
                    return;
                }
            }

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
            writer.WriteElementString(nameof(LastUpdate), LastUpdate.ToString("s")); //Last Upgrade Date
            writer.WriteElementString("Enabled", Enabled.ToString());
            writer.WriteElementString(nameof(DeleteWhenCompleted), DeleteWhenCompleted.ToString());
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

        public Ed2kfile ProcessGuid(string url, CookieContainer cookieContainer)
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