using System.Collections.Generic;
using System.Numerics;

namespace TvUndergroundDownloaderLib
{
    public enum LoginStatus
    {
        Logged,
        PasswordError,
        ServiceNotAvailable
    }

    public interface IMuleWebManager
    {
        bool IsConnected { get; }

        void AddToDownload(Ed2kfile link, string category);

        void Close();

        void CloseEmuleApp();

        LoginStatus Connect();

        void ForceRefreshSharedFileList();

        List<string> GetCategories(bool forceUpdate);

        List<Ed2kfile> GetCurrentDownloads(List<Ed2kfile> knownFiles);

        BigInteger FreeSpace { get; }

        void StartDownload(Ed2kfile link);

        void StopDownload(Ed2kfile link);
    }
}