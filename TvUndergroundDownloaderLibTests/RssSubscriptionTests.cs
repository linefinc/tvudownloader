using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TvUndergroundDownloaderLib;
using Config = TvUndergroundDownloaderLib.Tests.Config;

namespace TvUndergroundDownloaderLibTests
{
    [TestClass()]
    public class RssSubscriptionTests
    {
        [TestMethod()]
        public void UpdateTVUStatusTest()
        {
            CookieContainer cookieContainer = new CookieContainer();
            Uri uriTvunderground = new Uri("http://tvunderground.org.ru/");
            cookieContainer.Add(uriTvunderground, new Cookie("h", Config.t));
            cookieContainer.Add(uriTvunderground, new Cookie("i", Config.i));
            cookieContainer.Add(uriTvunderground, new Cookie("t", Config.t));

            string url = @"https://tvunderground.org.ru/rss.php?se_id=13158";
            RssSubscription newRssSubscription = new RssSubscription(url, cookieContainer);
            newRssSubscription.Update(cookieContainer);
            var list = newRssSubscription.DownloadedFiles;

            Assert.IsFalse(list.Count != 14);
            Assert.IsFalse(newRssSubscription.DownloadedFiles.Count != 0);

            var file1 = list[0];

            newRssSubscription.SetFileDownloaded(file1);
            Assert.IsFalse(newRssSubscription.DownloadedFiles.Count != 1);

            Assert.IsFalse(file1.DownloadDate.HasValue != true);
        }
    }
}