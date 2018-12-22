using System;
using System.IO;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Config = TvUndergroundDownloaderLib.Tests.Config;

namespace TvUndergroundDownloaderLibTests.FullSerieWatcher
{
    [TestClass()]
    public class FullSerieWatcherTest
    {
        [TestMethod()]
        public void ConstructorTest()
        {
            CookieContainer cookieContainer = new CookieContainer();
            Uri uriTvunderground = new Uri("http://tvunderground.org.ru/");
            cookieContainer.Add(uriTvunderground, new Cookie("h", Config.t));
            cookieContainer.Add(uriTvunderground, new Cookie("i", Config.i));
            cookieContainer.Add(uriTvunderground, new Cookie("t", Config.t));

            TvUndergroundDownloaderLib.FullSerieWatcher fullSerieWatcher = new TvUndergroundDownloaderLib.FullSerieWatcher("https://tvunderground.org.ru/index.php?show=season&sid=19185", "Ita", null, cookieContainer);

        
        }


        [TestMethod()]
        public void ParsePageTest()
        {
            string htmlPage = File.ReadAllText(".\\FullSerieWatcher\\page1.html");

            TvUndergroundDownloaderLib.FullSerieWatcher fullSerieWatcher = new TvUndergroundDownloaderLib.FullSerieWatcher("https://tvunderground.org.ru/index.php?show=season&sid=19185", "Ita", null, null);

            fullSerieWatcher.ParseSeriePage(htmlPage);


        }
    }
}