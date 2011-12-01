using System;
using System.Collections.Generic;
using System.Text;

namespace tvu
{
    public enum enumStatus
    {
        Ok,
        Error,
        Idle
    }

    public enum tvuStatus
    {
        Complete,
        StillRunning,
        Unknow,
        StillIncomplete
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
        }

        public string Title { set; get; }
        public string Url { set; get; }
        public string Category = "";
        public bool PauseDownload = false;
        public string LastUpgradeDate = "";
        public int TotalDownloads;
        public enumStatus status = enumStatus.Ok;
        public bool Enabled = true;
        public tvuStatus tvuStatus = tvuStatus.Unknow;
        public string LastTvUStatusUpgradeDate = "";

        public string TitleCompact { get { return this.Title.Replace("[ed2k] tvunderground.org.ru:", ""); } }

        
    }
}
