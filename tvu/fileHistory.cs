using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace tvu
{
    public class fileHistory : Ed2kfile
    {

        public fileHistory(string link, string FeedLink, string FeedSource)
            : base(link)
        {
            this.FeedLink = FeedLink;
            this.FeedSource = FeedSource;
            this.Date = DateTime.Now.ToString("s");
        }


        public fileHistory(string link, string FeedLink, string FeedSource, string Date)
            : base(link)
        {

            this.FeedLink = FeedLink;
            this.FeedSource = FeedSource;
            this.Date = Date;

        }

        public fileHistory(Ed2kfile file, string FeedLink, string FeedSource, string Date)
            :base(file)
        {
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
