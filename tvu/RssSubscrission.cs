using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Xml;


namespace TvUndergroundDownloader
{
    public enum tvuStatus
    {
        Complete,
        StillRunning,
        Unknown,
        StillIncomplete,
        OnHiatus
    }

    /// <summary>
    /// Rss submission container 
    /// </summary>
    public class RssSubscription
    {
        static private Regex regexFeedLink = new Regex(@"http(s)?://(www\.)?((tvunderground)|(tvu)).org.ru/index.php\?show=ed2k&season=(?<season>\d{1,10})&sid\[(?<sid>\d{1,10})\]=\d{1,10}");
        static public Regex regexFeedSource = new Regex(@"http(s)?://(www\.)?((tvunderground)|(tvu)).org.ru/rss.php\?se_id=(?<seid>\d{1,10})");
        static private Regex regexEDK2Link = new Regex(@"ed2k://\|file\|(.*)\|\d+\|\w+\|/");

        public string Title { private set; get; }
        public string Url { private set; get; }
        public string Category { get; set; } = string.Empty;
        public bool PauseDownload { get; set; } = false;
        public string LastUpgradeDate = string.Empty;
        public bool Enabled = true;
        public tvuStatus CurrentTVUStatus { get; private set; } = tvuStatus.Unknown;
        public int seasonID { get; private set; }
        public string TitleCompact { get { return this.Title.Replace("[ed2k] tvunderground.org.ru:", ""); } }

        public uint MaxSimultaneousDownload = 3;

        [Obsolete]
        public ListViewItem listViewItem = null;

        public DateTime LastSerieStatusUpgradeDate = DateTime.MinValue;

        private Dictionary<string, Ed2kfile> linkCache = new Dictionary<string, Ed2kfile>();
        private Dictionary<Ed2kfile, DateTime> downloaded = new Dictionary<Ed2kfile, DateTime>();

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Build a Rss Subscription
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Url"></param>
        public RssSubscription(string Title, string Url)
        {
            this.Title = Title;
            this.Url = Url;

            // Static Regex "http(s)?://(www\.)?tvunderground.org.ru/rss.php\?se_id=(\d{1,10})"
            MatchCollection matchCollection = regexFeedSource.Matches(this.Url);
            if (matchCollection.Count == 0)
            {
                throw new ApplicationException("Wrong URL");
            }

            int integerBuffer;
            if (int.TryParse(matchCollection[0].Groups["seid"].ToString(), out integerBuffer) == false)
            {

                throw new ApplicationException("Wrong URL");
            }

            this.seasonID = integerBuffer;
        }

        public RssSubscription(string newUrl, CookieContainer cookieContainer)
        {
            this.Url = newUrl;
            MatchCollection matchCollection = regexFeedSource.Matches(newUrl);
            if (matchCollection.Count == 0)
            {
                throw new ApplicationException("Wrong URL");
            }

            string webPageUrl = Url;
            string WebPage = WebFetch.Fetch(Url, false, cookieContainer);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(WebPage);

            XmlNode node = doc.SelectSingleNode(@"/rss/channel/title");

            if (node == null)
            {
                throw new ApplicationException("Wrong RSS file format");
            }
            if (string.IsNullOrEmpty(node.InnerText) == true)
            {
                throw new ApplicationException("Wrong RSS file format");
            }

            this.Title = node.InnerText;
        }

        public List<Ed2kfile> GetAllFile()
        {
            List<Ed2kfile> outArray = new List<Ed2kfile>();
            outArray.AddRange(linkCache.Values);
            outArray.Sort((A, B) => A.FileName.CompareTo(B.FileName));
            return outArray;
        }

        public void AddFile(DownloadFile file)
        {
            this.linkCache.Add(file.Guid, file.File);
            if(file.DownloadDate.HasValue)
            {
                this.downloaded.Add(file.File, file.DownloadDate.Value);
            }
        }

        public int GetDownloadFileCount()
        {
            return downloaded.Count;
        }

        public List<DownloadFile> GetDownloadFile()
        {
            List<DownloadFile> outArray = new List<DownloadFile>();

            foreach (string guid in linkCache.Keys)
            {
                Ed2kfile file = linkCache[guid];

                DownloadFile dw = new DownloadFile(file, this, guid);
                if (downloaded.ContainsKey(file) == true)
                {
                    dw.DownloadDate = downloaded[file];
                }

                outArray.Add(dw);
            }

            return outArray;
        }

        public List<Ed2kfile> GetNewDownload()
        {
            List<Ed2kfile> outArray = new List<Ed2kfile>();

            foreach (Ed2kfile file in linkCache.Values)
            {
                if (downloaded.ContainsKey(file) == false)
                {
                    outArray.Add(file);
                }
            }

            outArray.Sort((A, B) => A.FileName.CompareTo(B.FileName));
            if (outArray.Count > MaxSimultaneousDownload)
            {
                outArray.RemoveRange((int)MaxSimultaneousDownload, outArray.Count - (int)MaxSimultaneousDownload);
            }

            return outArray;
        }

        public List<Ed2kfile> GetDownloadedFiles()
        {
            List<Ed2kfile> outArray = new List<Ed2kfile>();
            outArray.AddRange(downloaded.Keys);
            outArray.Sort((A, B) => A.FileName.CompareTo(B.FileName));
            return outArray;
        }



        public void SetFileDownloaded(Ed2kfile file)
        {
            if (downloaded.ContainsKey(file) == true)
            {
                return;
            }

            downloaded.Add(file, DateTime.Now);
        }

        public void SetFileNotDownloaded(Ed2kfile file)
        {
            if (downloaded.ContainsKey(file) == false)
            {
                return;
            }

            downloaded.Remove(file);
        }

        /// <summary>
        /// Load data from xml
        /// </summary>
        /// <param name="doc">XML node with RSS subscription data</param>
        /// <returns></returns>
        static public RssSubscription LoadFormXml(XmlNode doc)
        {
            if (doc == null)
            {
                throw new NullReferenceException("Doc is null");
            }

            XmlNode node = doc.SelectSingleNode("Title");
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

            RssSubscription newRssSubscrission = new RssSubscription(title, url);

            node = doc.SelectSingleNode("Pause");
            newRssSubscrission.PauseDownload = Convert.ToBoolean(node.InnerText);

            node = doc.SelectSingleNode("Category");
            newRssSubscrission.Category = node.InnerText;

            node = doc.SelectSingleNode("LastUpgradeDate");
            newRssSubscrission.LastUpgradeDate = node.InnerText;

            node = doc.SelectSingleNode("Enabled");
            newRssSubscrission.Enabled = Convert.ToBoolean(node.InnerText);


            node = doc.SelectSingleNode("tvuStatus");
            switch (node.InnerText)
            {
                case "Complete":
                    newRssSubscrission.CurrentTVUStatus = tvuStatus.Complete;
                    break;
                case "StillIncomplete":
                    newRssSubscrission.CurrentTVUStatus = tvuStatus.StillIncomplete;
                    break;
                case "StillRunning":
                    newRssSubscrission.CurrentTVUStatus = tvuStatus.StillRunning;
                    break;
                case "OnHiatus":
                    newRssSubscrission.CurrentTVUStatus = tvuStatus.OnHiatus;
                    break;
                case "Unknown":
                default:
                    newRssSubscrission.CurrentTVUStatus = tvuStatus.Unknown;
                    break;
            }

            node = doc.SelectSingleNode("maxSimultaneousDownload");
            newRssSubscrission.MaxSimultaneousDownload = Convert.ToUInt16(node.InnerText);

            XmlNodeList filesNode = doc.SelectNodes("Files/File");
            foreach (XmlNode fileNode in filesNode)
            {
                XmlNode ed2kLinkNode = fileNode.SelectSingleNode("LinkED2K");
                if (ed2kLinkNode == null)
                {
                    continue;
                }

                Ed2kfile newFile = new Ed2kfile(ed2kLinkNode.InnerText);

                XmlNode guidLinkNode = fileNode.SelectSingleNode("Guid");
                newRssSubscrission.linkCache.Add(guidLinkNode.InnerText, newFile);

                XmlNode downloadedNode = fileNode.SelectSingleNode("Downloaded");
                if (downloadedNode != null)
                {
                    DateTime dtDownloaded = DateTime.Parse(downloadedNode.InnerText);
                    newRssSubscrission.downloaded.Add(newFile, dtDownloaded);
                }
            }

            return newRssSubscrission;
        }

        public DateTime GetLastDownloadDate()
        {
            DateTime dt = DateTime.MinValue;
            foreach(DateTime value in this.downloaded.Values)
            {
                if (dt < value)
                {
                    dt = value;
                }
            }
            return dt; 
        }

        public void Write(XmlTextWriter writer)
        {
            //<Channel>
            //  <Title>[ed2k] tvunderground.org.ru: Lie To Me - Season 2 (HDTV) italian </Title>
            //  <Url>http://tvunderground.org.ru/rss.php?se_id=32672</Url>
            //  <Pause>False</Pause>
            //  <Category>Anime</Category>
            //</Channel>

            writer.WriteStartElement("Channel");//Title
            writer.WriteAttributeString("type", "TVU");
            writer.WriteElementString("Title", Title);//Title
            writer.WriteElementString("Url", Url);//Url
            writer.WriteElementString("Pause", PauseDownload.ToString());//Category
            writer.WriteElementString("Category", Category);//Category
            writer.WriteElementString("LastUpgradeDate", LastUpgradeDate);//Last Upgrade Date
            writer.WriteElementString("Enabled", Enabled.ToString());
            writer.WriteElementString("maxSimultaneousDownload", MaxSimultaneousDownload.ToString());

            switch (CurrentTVUStatus)
            {
                case tvuStatus.Complete:
                    writer.WriteElementString("tvuStatus", "Complete");
                    break;
                case tvuStatus.StillIncomplete:
                    writer.WriteElementString("tvuStatus", "StillIncomplete");
                    break;
                case tvuStatus.StillRunning:
                    writer.WriteElementString("tvuStatus", "StillRunning");
                    break;
                case tvuStatus.OnHiatus:
                    writer.WriteElementString("tvuStatus", "OnHiatus");
                    break;
                case tvuStatus.Unknown:
                default:
                    writer.WriteElementString("tvuStatus", "Unknown");
                    break;
            }

            writer.WriteStartElement("Files");
            foreach (var fileKeys in linkCache.Keys)
            {
                writer.WriteStartElement("File");

                Ed2kfile file = linkCache[fileKeys];
                writer.WriteElementString("LinkED2K", file.Ed2kLink);

                writer.WriteElementString("Guid", fileKeys);
                if (downloaded.ContainsKey(linkCache[fileKeys]))
                {
                    writer.WriteElementString("Downloaded", downloaded[file].ToString());
                }
                writer.WriteEndElement();// end file
            }
            writer.WriteEndElement();// end file

            writer.WriteEndElement();// end channel
        }

        /// <summary>
        /// Update feed
        /// </summary>
        public void Update(CookieContainer cookieContainer)
        {
            string webPageUrl = Url;
            string WebPage = WebFetch.Fetch(Url, false, cookieContainer);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(WebPage);

            XmlNodeList nodeList = doc.SelectNodes(@"/rss/channel/item");
            foreach (XmlNode itemNode in nodeList)
            {
                XmlNode guidNode = itemNode.SelectSingleNode("guid");
                string guid = HttpUtility.UrlDecode(guidNode.InnerText);

                if (linkCache.ContainsKey(guid) == false)
                {
                    try
                    {
                        var newFile = ProcessGUID(guid, cookieContainer);
                        linkCache.Add(guid, newFile);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Error while try to parse or fatch GUID");
                    }
                }
            }

            TimeSpan ts = DateTime.Now - LastSerieStatusUpgradeDate;
            if (CurrentTVUStatus != tvuStatus.Complete)
            {
                return;
            }

            UpdateTVUStatus(cookieContainer);

        }

        private Ed2kfile ProcessGUID(string url, CookieContainer cookieContainer)
        {
            string WebPage = WebFetch.Fetch(url, false, cookieContainer);
            //
            int i = WebPage.IndexOf("ed2k://|file|");
            WebPage = WebPage.Substring(i);
            i = WebPage.IndexOf("|/");
            WebPage = WebPage.Substring(0, i + "|/".Length);
            return new Ed2kfile(WebPage);
        }

        public void UpdateTVUStatus(CookieContainer cookieContainer)
        {

            logger.Info("Checking serie status");
            string url = string.Format("http://tvunderground.org.ru/index.php?show=episodes&sid={0}", this.seasonID);

            string WebPage = WebFetch.Fetch(url, true, cookieContainer);
            LastSerieStatusUpgradeDate = DateTime.Now;
            if (WebPage != null)
            {
                if (WebPage.IndexOf("Still Running") > 0)
                {

                    CurrentTVUStatus = tvuStatus.StillRunning;
                    logger.Info("Serie status: Still Running");
                    return;
                }

                if (WebPage.IndexOf("Complete") > 0)
                {
                    CurrentTVUStatus = tvuStatus.Complete;
                    logger.Info("Serie status: Complete");
                    return;
                }

                if (WebPage.IndexOf("Still Incomplete") > 0)
                {
                    CurrentTVUStatus = tvuStatus.StillIncomplete;
                    logger.Info("Serie status: Still Incomplete");
                    return;
                }

                if (WebPage.IndexOf("On Hiatus") > 0)
                {
                    logger.Info("Serie status: On Hiatus");
                    CurrentTVUStatus = tvuStatus.OnHiatus;
                    return;
                }
            }

            this.CurrentTVUStatus = tvuStatus.Unknown;
            logger.Info("Serie status: Unknown");

        }
    }
    
}
