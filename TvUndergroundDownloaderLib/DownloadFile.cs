using System;

namespace TvUndergroundDownloaderLib
{
    public class DownloadFile : Ed2kfile
    {
        public RssSubscription Subscription { get; private set; }
        public string Guid { get; private set; }
        public DateTime? DownloadDate { get; set; }
        public DateTime? PublicationDate { get; set; }

        public DownloadFile(RssSubscription subscription, Ed2kfile file) : base(file)
        {
            this.Subscription = subscription;
        }
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="file"></param>
        /// <param name="guid"></param>
        public DownloadFile(RssSubscription subscription, Ed2kfile file, string guid) : base(file)
        {
            this.Subscription = subscription;
            this.Guid = guid;
        }
    }
}