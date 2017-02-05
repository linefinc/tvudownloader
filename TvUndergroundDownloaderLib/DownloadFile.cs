using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvUndergroundDownloaderLib
{
    public class DownloadFile
    {
        public RssSubscription Subscription { get; private set; }
        public Ed2kfile File { get; private set; }
        public string Guid { get; private set; }
        public DateTime? DownloadDate { get; set; }
        public DateTime? PublicationDate { get; set; }

        public DownloadFile(Ed2kfile file, RssSubscription subscription)
        {
            this.File = file;
            this.Subscription = subscription;
        }

        public DownloadFile(Ed2kfile file, RssSubscription subscription, string guid)
        {
            this.File = file;
            this.Subscription = subscription;
            this.Guid = guid;
        }
    }

}
