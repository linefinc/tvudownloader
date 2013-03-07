using System;
using System.Collections.Generic;
using System.Text;

namespace tvu
{


    public enum tvuStatus
    {
        Complete,
        StillRunning,
        Unknow,
        StillIncomplete,
        OnHiatus
    }

    /// <summary>
    /// Rss Subsciission container 
    /// </summary>
    public class RssSubscrission
    {
        public RssSubscrission (string Title, string Url)
        {
            this.Title = Title;
            this.Url = Url;
            DownloadedFile = new List<fileHistory>();
        }

        public string Title { set; get; }
        public string Url { set; get; }
        public string Category = "";
        public bool PauseDownload = false;
        public string LastUpgradeDate = "";
        public int TotalDownloads;
        public bool Enabled = true;
        public tvuStatus tvuStatus = tvuStatus.Unknow;
        public string LastTvUStatusUpgradeDate = "";

        public string TitleCompact { get { return this.Title.Replace("[ed2k] tvunderground.org.ru:", ""); } }

        public int maxSimultaneousDownload;

        public List<fileHistory> DownloadedFile;

        
    }
}
