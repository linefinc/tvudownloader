using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Xml;

namespace TvUndergroundDownloaderLib.Interfaces
{
    public interface ISubscription
    {
        string Category { get; set; }
        TvuStatus CurrentTVUStatus { get; }

        /// <summary>
        /// TV Underground status human readable
        /// </summary>
        string TVUStatus { get; }

        DateTime LastUpdate { get; set; }
        bool DeleteWhenCompleted { get; set; }
        string DubLanguage { get; }
        bool Enabled { get; set; }
        int LastChannelUpdate { get; }
        DateTime LastSerieStatusUpgradeDate { get; }
        int MaxSimultaneousDownload { get; set; }
        bool PauseDownload { get; set; }
        int SeasonId { get; }
        string Title { get; }
        string TitleCompact { get; }
        int TotalFilesDownloaded { get; }
        string Url { get; }

        /// <summary>
        /// Last download
        /// </summary>
        /// <remarks>Can be null</remarks>
        DateTime? LastDownload { get; }

        /// <summary>
        /// List of all files
        /// </summary>
        ReadOnlyCollection<DownloadFile> Files { get; }

        /// <summary>
        /// List of all downloaded files
        /// </summary>
        ReadOnlyCollection<DownloadFile> DownloadedFiles { get; }

        /// <summary>
        /// Last download date
        /// </summary>
        DateTime GetLastDownloadDate();

        List<Ed2kfile> GetNewDownload(int maxSimultaneousDownload);

        /// <summary>
        ///     Return all files that are waiting for download
        /// </summary>
        /// <returns></returns>
        List<DownloadFile> GetPendingFiles();

        void SetFileDownloaded(Ed2kfile file);
        void SetFileNotDownloaded(Ed2kfile file);

        /// <summary>
        ///     Update feed from Web server
        /// </summary>
        void Update(CookieContainer cookieContainer);

        void UpdateTVUStatus(CookieContainer cookieContainer);
        void Write(XmlTextWriter writer);
        Ed2kfile ProcessGuid(string url, CookieContainer cookieContainer);
    }
}