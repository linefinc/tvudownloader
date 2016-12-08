using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TvUndergroundDownloader
{
    public class DownloadedFile
    {
        public RssSubscription Subscription { get; private set; } = null;
        public Ed2kfile File { get; private set; }
        public DateTime? DownloadDate { get; private set; } = null;

        public DownloadedFile(Ed2kfile Ed2kLink, RssSubscription subscription)
        {
            this.File = Ed2kLink;
            this.Subscription = subscription;
            DownloadDate = null;
        }

        public DownloadedFile(Ed2kfile Ed2kLink, RssSubscription subscription, DateTime downloadDate)
        {
            this.File = Ed2kLink;
            this.Subscription = subscription;
            this.DownloadDate = downloadDate;
        }

    }
}
