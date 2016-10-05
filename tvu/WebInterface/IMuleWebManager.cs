﻿using System.Collections.Generic;

namespace TvUndergroundDownloader
{
    public enum LoginStatus { Logged, PasswordError, ServiceNotAvailable };

    interface IMuleWebManager
    {
        bool isConnected { get; }

        LoginStatus Connect();
        void Close();
        void AddToDownload(Ed2kfile link, string Category);
        void StartDownload(Ed2kfile link);
        void StopDownload(Ed2kfile link);
        void CloseEmuleApp();
        void ForceRefreshSharedFileList();
        List<string> GetCategories(bool forceUpdate);
        List<Ed2kfile> GetActualDownloads(List<Ed2kfile> knownFiles);
    }
}
