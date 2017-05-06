using Microsoft.VisualStudio.TestTools.UnitTesting;
using TvUndergroundDownloaderLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace TvUndergroundDownloaderLib.Tests
{
    [TestClass()]
    public class RssSubscriptionTests
    {
        //[TestMethod()]
        //public void RssSubscriptionTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void RssSubscriptionTest1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void LoadFormXmlTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void AddFileTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetAllFileTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetDownloadedFilesTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetDownloadFileTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetDownloadFileCountTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetLastDownloadDateTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetNewDownloadTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetPendingFilesTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetFileDownloadedTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetFileNotDownloadedTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void UpdateTest()
        //{
        //    Assert.Fail();
        //}

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
            var list = newRssSubscription.GetDownloadFile();

            Assert.IsFalse(list.Count != 14);
        }

        //[TestMethod()]
        //public void WriteTest()
        //{
        //    Assert.Fail();
        //}
    }
}