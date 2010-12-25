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

    
    public class RssFeed
    {
        public string Title = "";
        public string Url = "";
        public string Category = "";
        public bool PauseDownload = false;
        public string LastUpgradeDate = "";
        public int TotalDownloads;
        public enumStatus status = enumStatus.Ok;
        
    }
}
