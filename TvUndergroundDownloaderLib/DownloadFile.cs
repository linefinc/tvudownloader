using System;

namespace TvUndergroundDownloaderLib
{
    public class DownloadFile : Ed2kfile
    {
        public DownloadFile(RssSubscription subscription, Ed2kfile file) : base(file)
        {
            Subscription = subscription;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="file"></param>
        /// <param name="guid"></param>
        public DownloadFile(RssSubscription subscription, Ed2kfile file, string guid) : base(file)
        {
            Subscription = subscription;
            Guid = guid;
        }

        public RssSubscription Subscription { get; }
        public string Guid { get; }
        public DateTime? DownloadDate { get; set; }
        public DateTime? PublicationDate { get; set; }
    }
}