using System;

namespace TvUndergroundDownloaderLib
{
    public class DownloadFile : Ed2kfile
    {
        public RssSubscription Subscription { get; private set; }
        public string Guid { get; private set; }
        public DateTime? DownloadDate { get; set; }
        public DateTime? PublicationDate { get; set; }

        public DownloadFile(Ed2kfile file, RssSubscription subscription) : base(file)
        {
            this.Subscription = subscription;
        }

        public DownloadFile(Ed2kfile file, RssSubscription subscription, string guid) : base(file)
        {
            this.Subscription = subscription;
            this.Guid = guid;
        }
    }
}