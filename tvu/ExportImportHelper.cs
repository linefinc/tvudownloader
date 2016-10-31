using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;

namespace TvUndergroundDownloader
{
    class ExportImportHelper
    {
        public static void Export(Config config, History history, Stream steam)
        {
            XmlTextWriter writter = new XmlTextWriter(steam, Encoding.UTF8);
            writter.Formatting = Formatting.Indented;

            writter.WriteStartDocument();

            writter.WriteStartElement("Channels");
            writter.WriteAttributeString("version", Config.Version);

            List<RssSubscrission> myRssFeedList = new List<RssSubscrission>();
            myRssFeedList.AddRange(config.RssFeedList);
            myRssFeedList.Sort((x, y) => string.Compare(x.Title, y.Title));

            foreach (RssSubscrission feed in myRssFeedList)
            {
                //<Channel>
                //<Title>[ed2k] tvunderground.org.ru: Lie To Me - Season 2 (HDTV) italian </Title>
                //<Url>http://tvunderground.org.ru/rss.php?se_id=32672</Url>
                //<Pause>False</Pause>
                //<Category>Anime</Category>
                //</Channel>
                writter.WriteStartElement("Channel");
                {
                    writter.WriteStartElement("Title");//Title
                    writter.WriteString(feed.Title);
                    writter.WriteEndElement();

                    writter.WriteStartElement("Url");//Url
                    writter.WriteString(feed.Url);
                    writter.WriteEndElement();

                    writter.WriteStartElement("Category");//Category
                    writter.WriteString(feed.Category);
                    writter.WriteEndElement();

                    writter.WriteStartElement("maxSimultaneousDownload"); // max Simultaneous Downloads
                    writter.WriteString(feed.maxSimultaneousDownload.ToString());
                    writter.WriteEndElement();

                    writter.WriteStartElement("DownloadedFiles");

                    foreach (var file in history.ExportDownloadedFileByFeedSoruce(feed.Url))
                    {
                        writter.WriteStartElement("File");
                        writter.WriteStartElement("Link");
                        writter.WriteString(file.GetLink());
                        writter.WriteEndElement();
                        writter.WriteStartElement("FeedLink");
                        writter.WriteString(file.FeedLink);
                        writter.WriteEndElement();
                        writter.WriteStartElement("FeedSource");
                        writter.WriteString(file.FeedSource);
                        writter.WriteEndElement();
                        writter.WriteStartElement("Date");
                        writter.WriteString(file.Date);
                        writter.WriteEndElement();
                        writter.WriteEndElement();
                    }
                    writter.WriteEndElement();
                }
                writter.WriteEndElement();// end channel
            }
            writter.WriteEndElement();// end RSSChannel
            writter.Close();
        }

        public static void Import(Config config, History history, string fileName)
        {
            List<FileHistory> localFileHistory = new List<FileHistory>();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(fileName);

            XmlNodeList Channels = xDoc.GetElementsByTagName("Channel");
            foreach (XmlNode Channel in Channels)
            {
                string newFeedTitle = string.Empty;
                string newFeedUrl = string.Empty;
                RssSubscrission newFeed = null;

                foreach (XmlNode t in Channel)
                {
                    switch (t.Name.ToString())
                    {
                        case "Title":
                            newFeedTitle = t.FirstChild.Value;
                            break;
                        case "Url":
                            newFeedUrl = t.FirstChild.Value;
                            break;
                        default:
                            break;
                    }

                }

                if (string.IsNullOrEmpty(newFeedTitle) == true)
                    continue;

                if (string.IsNullOrEmpty(newFeedUrl) == true)
                    continue;

                bool exist = config.RssFeedList.Exists(delegate (RssSubscrission rs) { return rs.Url == newFeedUrl; });
                if (exist == true)
                    continue;

                newFeed = new RssSubscrission(newFeedTitle, newFeedUrl);
                newFeed.Enabled = true;
                newFeed.Category = config.DefaultCategory;
                newFeed.maxSimultaneousDownload = config.MaxSimultaneousFeedDownloads;
                newFeed.PauseDownload = false;
                newFeed.LastSerieStatusUpgradeDate = DateTime.Now;
                newFeed.LastUpgradeDate = DateTime.Now.ToString("s");
                newFeed.tvuStatus = tvuStatus.Unknown;
                config.RssFeedList.Add(newFeed);
                // load feed file to avoid duplicate
                localFileHistory.AddRange(history.ExportDownloadedFileByFeedSoruce(newFeedUrl));
                DataBaseHelper.RssSubscrissionList.AddOrUpgrade(newFeed);
            }

            XmlNodeList downloadFiles = xDoc.GetElementsByTagName("File");
            foreach (XmlNode fileXmlNode in downloadFiles)
            {
                string newLink = string.Empty;
                string newFeedLink = string.Empty;
                string newFeedSource = string.Empty;
                string newDate = string.Empty;

                foreach (XmlNode temp in fileXmlNode)
                {
                    switch (temp.Name.ToString())
                    {
                        case "Link":
                            newLink = temp.FirstChild.Value;
                            break;
                        case "FeedLink":
                            newFeedLink = temp.FirstChild.Value;
                            break;
                        case "FeedSource":
                            newFeedSource = temp.FirstChild.Value;
                            break;
                        case "Date":
                            newDate = temp.FirstChild.Value;
                            break;
                        default:
                            break;
                    }

                }

                if (string.IsNullOrEmpty(newLink) == true)
                    continue;

                if (string.IsNullOrEmpty(newFeedLink) == true)
                    continue;

                if (string.IsNullOrEmpty(newFeedSource) == true)
                    continue;

                if (string.IsNullOrEmpty(newDate) == true)
                    continue;

                var file = new FileHistory(newLink, newFeedLink, newFeedSource, newDate);

                bool exist = localFileHistory.Exists(delegate (FileHistory f) { return f.HashMD4 == file.HashMD4; }); 
                if (exist == true)
                    continue;

                History.Add(newLink, newFeedLink, newFeedSource, newDate);

                history.Save();

            }

        }
    }
}
