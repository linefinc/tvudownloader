using System;
using System.Collections.Generic;
using System.Text;

namespace tvu
{
    public class RssChannel
    {
        public RssChannel()
        {
            ListItem = new List<RssItem>();
        }

        /// <summary>
        /// Url (Download the xml rss from here)
        /// </summary>
        public string Url; 

        public List<RssItem> ListItem;
        public string Title;
        public string Description;
        /// <summary>
        ///URL (Link of page were is located the rss)
        /// </summary>
        public string Link;
        public string Category = "";
        public bool Pause;
        public int maxSimultaneousDownload;
    }
    
    public class RssItem
    {
        
        public string PubDate;
        public string Title;
        public string Description;
        public string Link;
        public string Guid;
    }

    class RssParserTVU
    {


        public static RssChannel Parse(string page)
        {

            //<item>
            //  <pubDate>Tue, 16 Nov 2010 12:03:21 GMT</pubDate> 
            //  <title>[ed2k] G String Divas 2000 - 1x11 - Jamais Avec Un Client.avi</title> 
            //  <description>Type: PDTV<br />Size: 249 MB<br />Ripper: BaLLanTeAm<br />Submitter: SkyBreak<br />Download: <a href="http://tvunderground.org.ru/index.php?show=ed2k&amp;season=34875&sid[514856]=1">Download</a></description> 
            //  <link>http://tvunderground.org.ru/index.php?show=episodes&sid=34875</link> 
            //  <guid>http://tvunderground.org.ru/index.php?show=ed2k&season=34875&sid%5B514856%5D=1</guid> 
            //</item>

            RssChannel RssChannel = new RssChannel();
            RssChannel.Title = GetStringByDelimiter(page, "<title>", "</title>", 0);
            RssChannel.Description = GetStringByDelimiter(page, "<title>", "</title>", 0);

            RssChannel.Link = GetStringByDelimiter(page, "<link>", "</link>", 0);
            

            int index = -1;

            while ((index = page.IndexOf("<item>", index + 1)) > 0)
            {
                string temp = GetStringByDelimiter(page, "<item>", "</item>", index);

                RssItem p = new RssItem();
                p.Title = GetStringByDelimiter(page, "<title>", "</title>", index);
                p.Description = GetStringByDelimiter(page, "<description>", "</description>", index);
                p.Link = GetStringByDelimiter(page, "<link>", "</link>", index);
                p.Guid = GetStringByDelimiter(page, "<guid>", "</guid>", index);
                p.PubDate = GetStringByDelimiter(page, "<pubDate>", "</pubDate>", index);

                RssChannel.ListItem.Add(p);

            }

            return RssChannel;


        }

        public static string GetStringByDelimiter(string Source, string startDelimiter, string stopDelimiter, int startIndex)
        {
            int pos1 = Source.IndexOf(startDelimiter, startIndex);
            int pos2 = Source.IndexOf(stopDelimiter, startIndex);
            return Source.Substring(pos1 + startDelimiter.Length, pos2 - pos1 - startDelimiter.Length);
        }

        public static string FindEd2kLink(string text)
        {
            //
            int i = text.IndexOf("ed2k://|file|");
            text = text.Substring(i);
            i = text.IndexOf("|/");
            text = text.Substring(0, i + "|/".Length);
            return text;
        }
        
    }
}
