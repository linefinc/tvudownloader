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
        Unknow
    }

    /// <summary>
    /// Rss Subsciission container 
    /// </summary>
    public class RssSubscrission
    {
        
        public string Title = "";
        public string Url = "";
        public string Category = "";
        public bool PauseDownload = false;
        public string LastUpgradeDate = "";
        public int TotalDownloads;
        public enumStatus status = enumStatus.Ok;
        public bool Enabled = true;
        public tvuStatus tvuStatus = tvuStatus.Unknow;
        
    }
}
