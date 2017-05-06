using System.Collections.Generic;

namespace TvUndergroundDownloaderLib
{
    public class RssSubscriptionList : List<RssSubscription>
    {
        public List<DownloadFile> GetLastActivity(int Size = 20)
        {
            var fileList = new List<DownloadFile>();

            foreach (var subscription in this)
                fileList.AddRange(subscription.GetDownloadFile());

            fileList.RemoveAll(temp => temp.DownloadDate.HasValue == false);
            fileList.Sort((A, B) => B.DownloadDate.Value.CompareTo(A.DownloadDate.Value));
            if (fileList.Count > Size)
                fileList.RemoveRange(Size, fileList.Count - Size);
            return fileList;
        }

        public List<DownloadFile> GetDownloadFiles()
        {
            var fileList = new List<DownloadFile>();

            foreach (var subscription in this)
                fileList.AddRange(subscription.GetDownloadFile());
            return fileList;
        }
    }
}