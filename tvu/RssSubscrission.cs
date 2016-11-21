using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
        static private Regex regexFeedSource = new Regex(@"http(s)?://(www\.)?((tvunderground)|(tvu)).org.ru/rss.php\?se_id=(?<seid>\d{1,10})");
        static private Regex regexEDK2Link = new Regex(@"ed2k://\|file\|(.*)\|\d+\|\w+\|/");

        public string Title { private set; get; }
        public string Url { private set; get; }
        public string Category = string.Empty;
        public bool PauseDownload = false;
        public string LastUpgradeDate = string.Empty;
        public bool Enabled = true;
        public tvuStatus tvuStatus = tvuStatus.Unknown;
        public int seasonID { get; private set; }
        public string TitleCompact { get { return this.Title.Replace("[ed2k] tvunderground.org.ru:", ""); } }

        public uint maxSimultaneousDownload = 3;

        [Obsolete]
        public ListViewItem listViewItem = null;

        public DateTime LastSerieStatusUpgradeDate = DateTime.MinValue;

        private Dictionary<string, Ed2kfile> cache = new Dictionary<string, Ed2kfile>();
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
                System.ApplicationException ex = new System.ApplicationException("Wrong URL");
                throw ex;
            }

            int integerBuffer;
            if (int.TryParse(matchCollection[0].Groups["seid"].ToString(), out integerBuffer) == false)
            {
                System.ApplicationException ex = new System.ApplicationException("Wrong URL");
                throw ex;
            }

            this.seasonID = integerBuffer;
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
                    newRssSubscrission.tvuStatus = tvuStatus.Complete;
                    break;
                case "StillIncomplete":
                    newRssSubscrission.tvuStatus = tvuStatus.StillIncomplete;
                    break;
                case "StillRunning":
                    newRssSubscrission.tvuStatus = tvuStatus.StillRunning;
                    break;
                case "OnHiatus":
                    newRssSubscrission.tvuStatus = tvuStatus.OnHiatus;
                    break;
                case "Unknown":
                default:
                    newRssSubscrission.tvuStatus = tvuStatus.Unknown;
                    break;
            }

            node = doc.SelectSingleNode("maxSimultaneousDownload");
            newRssSubscrission.maxSimultaneousDownload = Convert.ToUInt16(node.InnerText);

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
                newRssSubscrission.cache.Add(guidLinkNode.InnerText, newFile);

                XmlNode downloadedNode = fileNode.SelectSingleNode("Downloaded");
                DateTime dtDownloaded = DateTime.Parse(downloadedNode.InnerText);
                newRssSubscrission.downloaded.Add(newFile, dtDownloaded);
            }

            return newRssSubscrission;
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
            writer.WriteElementString("maxSimultaneousDownload", maxSimultaneousDownload.ToString());

            switch (tvuStatus)
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
            foreach (var fileKeys in cache.Keys)
            {
                writer.WriteStartElement("File");
                if (cache[fileKeys] == null)
                    writer.WriteElementString("LinkED2K", "null");
                else
                    writer.WriteElementString("LinkED2K", cache[fileKeys].Ed2kLink);

                writer.WriteElementString("Guid", fileKeys);
                writer.WriteElementString("Downloaded", DateTime.Now.ToString());
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

                if (cache.ContainsKey(guid) == false)
                {
                    try
                    {
                        var newFile = ProcessGUID(guid, cookieContainer);
                        cache.Add(guid, newFile);
                    }
                    catch (Exception ex)
                    {
                        cache.Add(guid, null);
                    }
                }
            }
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

    }

    public class DataBaseHelper
    {
        public class RssSubscriptionList
        {
            public static void InitDB()
            {
                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
                {
                    connection.Open();

                    string sql = @"CREATE TABLE IF NOT EXISTS RssSubscrission (
                                uuid INTEGER PRIMARY KEY AUTOINCREMENT,
                                Title TEXT,
                                Url TEXT UNIQUE,
                                seasonID INTEGER UNIQUE,
                                Pause INTEGER,
                                Category TEXT,
                                Enabled INTEGER,
                                tvuStatus INTEGER,
                                maxSimultaneousDownload INTEGER);";
                    SQLiteCommand command = new SQLiteCommand(sql, connection);
                    command.ExecuteNonQuery();
                }

                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
                {
                    connection.Open();
                    string sql = @"CREATE VIEW IF NOT EXISTS vwRssSubscrission AS
                                    SELECT *, 
                                    (SELECT COUNT(*) from History WHERE History.seasonID = RssSubscrission.seasonID  ) AS Downloaded ,
                                    (SELECT COUNT(*) from FeedLinkCache WHERE FeedLinkCache.seasonID = RssSubscrission.seasonID  ) AS Pending
                                    FROM RssSubscrission;";
                    SQLiteCommand command = new SQLiteCommand(sql, connection);
                    command.ExecuteNonQuery();
                }


            }
            public static void CleanUp()
            {

            }

            public static void CleanUp(List<RssSubscription> RssFeedList)
            {

                StringBuilder filterSB = new StringBuilder();

                foreach (var rssSubscrission in RssFeedList)
                {
                    filterSB.AppendFormat("{0},", rssSubscrission.seasonID);
                }

                char[] charsToTrim = { ',', ' ' };


                string temp = filterSB.ToString().TrimEnd(charsToTrim);

                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
                {
                    connection.Open();
                    const string sqlTemplate = @"DELETE FROM RssSubscrission WHERE RssSubscrission.seasonID NOT IN ({0});";
                    SQLiteCommand command = new SQLiteCommand(string.Format(sqlTemplate, temp), connection);
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }


            public static void AddOrUpgrade(RssSubscription rssSubscrission)
            {
                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
                {
                    connection.Open();
                    const string sqlTemplate = @"INSERT OR REPLACE INTO RssSubscrission (title, url, seasonID, pause, category, enabled, tvuStatus, maxSimultaneousDownload )
                                                VALUES
                                                (@title, @url, @seasonID, @pause, @category, @enabled, @tvuStatus, @maxSimultaneousDownload)";

                    SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SQLiteParameter("@title", rssSubscrission.Title));
                    command.Parameters.Add(new SQLiteParameter("@url", rssSubscrission.Url));
                    command.Parameters.Add(new SQLiteParameter("@seasonID", rssSubscrission.seasonID));
                    command.Parameters.Add(new SQLiteParameter("@pause", rssSubscrission.PauseDownload));
                    command.Parameters.Add(new SQLiteParameter("@category", rssSubscrission.Category));
                    command.Parameters.Add(new SQLiteParameter("@enabled", rssSubscrission.Enabled));
                    command.Parameters.Add(new SQLiteParameter("@tvuStatus", rssSubscrission.tvuStatus));
                    command.Parameters.Add(new SQLiteParameter("@maxSimultaneousDownload", rssSubscrission.maxSimultaneousDownload));
                    command.ExecuteNonQuery();
                }
            }

            public static void Delete(RssSubscription rssSubscrission)
            {
                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Config.FileNameDB)))
                {
                    connection.Open();
                    const string sqlTemplate = @"DELETE FROM RssSubscrission WHERE url = @url AND seasonID = @seasonID";

                    SQLiteCommand command = new SQLiteCommand(sqlTemplate, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SQLiteParameter("@url", rssSubscrission.Url));
                    command.Parameters.Add(new SQLiteParameter("@seasonID", rssSubscrission.seasonID));
                    command.ExecuteNonQuery();
                }

            }

        }
    }
}
