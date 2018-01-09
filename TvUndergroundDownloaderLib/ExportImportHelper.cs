using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace TvUndergroundDownloaderLib
{
    public class ExportImportHelper
    {
        public static void Export(Config config, Stream steam)
        {
            var writter = new XmlTextWriter(steam, Encoding.UTF8);
            writter.Formatting = Formatting.Indented;

            writter.WriteStartDocument();

            writter.WriteStartElement("Channels");
            writter.WriteAttributeString("version", Config.Version);

            var myRssFeedList = new List<RssSubscription>();
            myRssFeedList.AddRange(config.RssFeedList);
            myRssFeedList.Sort((x, y) => string.Compare(x.Title, y.Title));

            foreach (var feed in myRssFeedList)
            {
                //<Channel>
                //<Title>[ed2k] tvunderground.org.ru: Lie To Me - Season 2 (HDTV) italian </Title>
                //<Url>http://tvunderground.org.ru/rss.php?se_id=32672</Url>
                //<Pause>False</Pause>
                //<Category>Anime</Category>
                //</Channel>
                writter.WriteStartElement("Channel");
                {
                    writter.WriteStartElement("Title"); //Title
                    writter.WriteString(feed.Title);
                    writter.WriteEndElement();

                    writter.WriteStartElement("Url"); //Url
                    writter.WriteString(feed.Url);
                    writter.WriteEndElement();

                    writter.WriteStartElement("Category"); //Category
                    writter.WriteString(feed.Category);
                    writter.WriteEndElement();

                    writter.WriteStartElement("maxSimultaneousDownload"); // max Simultaneous Downloads
                    writter.WriteString(feed.MaxSimultaneousDownload.ToString());
                    writter.WriteEndElement();

                    writter.WriteStartElement("DownloadedFiles");

                    foreach (DownloadFile file in feed.Files)
                    {
                        writter.WriteStartElement("File");

                        writter.WriteStartElement("Link");
                        writter.WriteString(file.Ed2kLink);
                        writter.WriteEndElement();
                        writter.WriteStartElement("FeedLink");
                        writter.WriteString(file.Guid);
                        writter.WriteEndElement();
                        writter.WriteStartElement("FeedSource");
                        writter.WriteString(file.Subscription.Url);
                        writter.WriteEndElement();

                        if (file.DownloadDate.HasValue)
                        {
                            writter.WriteStartElement("Date");
                            writter.WriteString(file.DownloadDate.ToString());
                            writter.WriteEndElement();
                        }
                        writter.WriteEndElement();
                    }
                    writter.WriteEndElement();
                }
                writter.WriteEndElement(); // end channel
            }
            writter.WriteEndElement(); // end RSSChannel
            writter.Close();
        }

        public static void Import(Config config, string fileName)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(fileName);

            var Channels = xDoc.GetElementsByTagName("Channel");
            foreach (XmlNode Channel in Channels)
            {
                string newFeedTitle = string.Empty;
                string newFeedUrl = string.Empty;
                RssSubscription newFeed = null;

                foreach (XmlNode t in Channel)
                    switch (t.Name)
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

                if (string.IsNullOrEmpty(newFeedTitle))
                    continue;

                if (string.IsNullOrEmpty(newFeedUrl))
                    continue;

                bool exist = config.RssFeedList.Exists(delegate(RssSubscription rs) { return rs.Url == newFeedUrl; });
                if (exist)
                    continue;

                newFeed = new RssSubscription(newFeedTitle, newFeedUrl);
                newFeed.Enabled = true;
                newFeed.Category = config.DefaultCategory;
                newFeed.MaxSimultaneousDownload =(int) config.MaxSimultaneousFeedDownloadsDefault;
                newFeed.PauseDownload = false;
                newFeed.LastUpdate = DateTime.MinValue;
                config.RssFeedList.Add(newFeed);
            }

            var downloadFiles = xDoc.GetElementsByTagName("File");
            foreach (XmlNode fileXmlNode in downloadFiles)
            {
                string newLink = string.Empty;
                string newFeedLink = string.Empty;
                string newFeedSource = string.Empty;
                string newDate = string.Empty;

                foreach (XmlNode temp in fileXmlNode)
                    switch (temp.Name)
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

                if (string.IsNullOrEmpty(newLink))
                    continue;

                if (string.IsNullOrEmpty(newFeedLink))
                    continue;

                if (string.IsNullOrEmpty(newFeedSource))
                    continue;

                if (string.IsNullOrEmpty(newDate))
                    continue;

                var file = new Ed2kfile(newLink);
                var subscription = config.RssFeedList.Find(temp => temp.Url == newFeedLink);
                var dw = new DownloadFile(subscription, file);
                if (string.IsNullOrEmpty(newDate) == false)
                {
                    DateTime dt;
                    if (DateTime.TryParse(newDate, out dt))
                        dw.DownloadDate = dt;
                }

                //subscription.
                //var file = new FileHistory(newLink, newFeedLink, newFeedSource, newDate);

                //bool exist = localFileHistory.Exists(delegate (FileHistory f) { return f.HashMD4 == file.HashMD4; });
                //if (exist == true)
                //    continue;

                //History.Add(newLink, newFeedLink, newFeedSource, newDate);

                //history.Save();
            }
        }
    }
}