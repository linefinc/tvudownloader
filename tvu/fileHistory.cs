using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace TvUndergroundDownloader
{
    public class fileHistory : Ed2kfile
    {
        static public Regex regexFeedLink = new Regex(@"https?://(www\.)?tvunderground.org.ru/index.php\?show=ed2k&season=(\d{1,10})&sid\[(\d{1,10})\]=\d{1,10}");
        static public Regex regexFeedSource = new Regex(@"http(s)?://(www\.)?tvunderground.org.ru/rss.php\?se_id=(\d{1,10})");
        
        public fileHistory(string link, string FeedLink, string FeedSource, string Date)
            : base(link)
        {
            if (regexFeedLink.IsMatch(FeedLink) == false)
            {
                throw (new System.ApplicationException("Wrong FeedLink"));
            }


            if (regexFeedLink.IsMatch(FeedLink) == false)
            {
                throw (new System.ApplicationException("Wrong FeedSource"));
            }
            this.FeedLink = FeedLink;
            this.FeedSource = FeedSource;
            this.Date = Date;

        }

        public fileHistory(string link, string FeedLink, string FeedSource)
            : this(link, FeedLink, FeedSource, DateTime.Now.ToString("s"))
        {

        }
        public fileHistory(Ed2kfile file, string FeedLink, string FeedSource, string Date)
            : base(file)
        {
            if (regexFeedLink.IsMatch(FeedLink) == false)
            {
                throw (new System.ApplicationException("Wrong FeedLink"));
            }


            if (regexFeedLink.IsMatch(FeedLink) == false)
            {
                throw (new System.ApplicationException("Wrong FeedSource"));
            }
            this.FeedLink = FeedLink;
            this.FeedSource = FeedSource;
            this.Date = Date;
        }


        /// <summary>
        /// link of page than contain ed2k link
        /// </summary>
        public string FeedLink { get; private set; }
        /// <summary>
        /// url of feed
        /// </summary>

        public string FeedSource { get; private set; }
        public string Date { get; set; }
    }


}
