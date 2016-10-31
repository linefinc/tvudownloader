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
                        writter.WriteStartElement("Link"); // max Simultaneous Downloads
                        writter.WriteString(file.GetLink());
                        writter.WriteEndElement();
                        writter.WriteStartElement("FeedLink"); // max Simultaneous Downloads
                        writter.WriteString(file.FeedLink);
                        writter.WriteEndElement();
                        writter.WriteStartElement("FeedSource"); // max Simultaneous Downloads
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
    }
}
