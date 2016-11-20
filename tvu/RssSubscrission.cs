using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <!-- this construnctor is used to create class when we are loading from xml -->
        private RssSubscription()
        {

        }


        public RssSubscription(string Title, string Url)
        {
            this.Title = Title;
            this.Url = Url;


            // Static Regex "http(s)?://(www\.)?tvunderground.org.ru/rss.php\?se_id=(\d{1,10})"
            MatchCollection matchCollection = FileHistory.regexFeedSource.Matches(this.Url);
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
        static RssSubscription LoadFormXml(XmlNode doc)
        {
            if (doc == null)
            {
                throw new NullReferenceException("Doc is null");
            }

            RssSubscription newRssSubscrission = new RssSubscription();

            XmlNode node = doc.SelectSingleNode("Title");
            if (node == null)
            {
                throw new Exception("Error while loading xml: Missing RssSubscrission\\Title");
            }
            newRssSubscrission.Title = node.InnerText;

            node = doc.SelectSingleNode("Url");
            if (node == null)
            {
                throw new Exception("Error while loading xml: Missing RssSubscrission\\Title");
            }
            newRssSubscrission.Url = node.InnerText;

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

            return newRssSubscrission;
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
