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
        static public Regex regexFeedLink = new Regex(@"http(s)?://(www\.)?((tvunderground)|(tvu)).org.ru/index.php\?show=ed2k&season=(?<season>\d{1,10})&sid\[(?<sid>\d{1,10})\]=\d{1,10}");
        static public Regex regexFeedSource = new Regex(@"http(s)?://(www\.)?((tvunderground)|(tvu)).org.ru/rss.php\?se_id=(?<seid>\d{1,10})");

        /// <summary>
        /// link of page than contain ed2k link
        /// </summary>
        public string FeedLink { get; private set; }
        /// <summary>
        /// url of feed
        /// </summary>
        public string FeedSource { get; private set; }
        public string Date { get; set; }


        public fileHistory(string link, string FeedLink, string FeedSource, string Date)
            : base(link)
        {
            if (regexFeedLink.IsMatch(FeedLink) == false)
            {
                throw (new System.ApplicationException("Wrong FeedLink"));
            }


            if (regexFeedSource.IsMatch(FeedSource) == false)
            {
                throw (new System.ApplicationException("Wrong FeedSource"));
            }
            this.FeedLink = FeedLink;
            this.FeedSource = FeedSource;
            this.Date = Date;

        }

        public fileHistory(string link, string FeedLink, string FeedSource, DateTime Date)
            : base(link)
        {
            if (regexFeedLink.IsMatch(FeedLink) == false)
            {
                throw (new System.ApplicationException("Wrong FeedLink"));
            }


            if (regexFeedSource.IsMatch(FeedSource) == false)
            {
                throw (new System.ApplicationException("Wrong FeedSource"));
            }
            this.FeedLink = FeedLink;
            this.FeedSource = FeedSource;
            this.Date = Date.ToString();

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


            if (regexFeedSource.IsMatch(FeedSource) == false)
            {
                throw (new System.ApplicationException("Wrong FeedSource"));
            }
            this.FeedLink = FeedLink;
            this.FeedSource = FeedSource;
            this.Date = Date;
        }


      
    }


}
