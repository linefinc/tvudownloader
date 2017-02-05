using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TvUndergroundDownloaderLib
{

    [Obsolete]
    public class sDonwloadFile
    {
        public sDonwloadFile(Ed2kfile ed2kLink, RssSubscription subscription)
        {
            this.File = ed2kLink;
            this.Subscription = subscription;
        }

        public Ed2kfile File { get; private set; }
        public RssSubscription Subscription { get; private set; } = null;
    };

    public class Worker
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private Thread thread;
        public Config Config = null;
        private bool cancellationPending = false;


        public Worker()
        {
            thread = new Thread(this.WorkerThreadFunc);
            thread.Name = "TvUndergroundDownloaderLib";
        }



        public void WorkerThreadFunc()
        {
            if (Config == null)
            {
                throw new NullReferenceException("Config");
            }



            //
            //  Load Cookies from configure
            //
            CookieContainer cookieContainer = new CookieContainer();
            Uri uriTvunderground = new Uri("http://tvunderground.org.ru/");
            cookieContainer.Add(uriTvunderground, new Cookie("h", Config.tvuCookieH));
            cookieContainer.Add(uriTvunderground, new Cookie("i", Config.tvuCookieI));
            cookieContainer.Add(uriTvunderground, new Cookie("t", Config.tvuCookieT));

            //
            //  start RSS Check
            //
            logger.Info("Start RSS Check");

            List<sDonwloadFile> downloadFileList = new List<sDonwloadFile>();

            // select only enabled RSS
            List<RssSubscription> rssFeedList = Config.RssFeedList.FindAll(delegate (RssSubscription rss) { return rss.Enabled == true; });

            foreach (RssSubscription feed in rssFeedList)
            {
                try
                {
                    logger.Info(@"Read RSS ""{0}""", feed.TitleCompact);
                    feed.Update(cookieContainer);
                    foreach (Ed2kfile file in feed.GetNewDownload())
                    {
                        sDonwloadFile sfile = new sDonwloadFile(file, feed);
                        downloadFileList.Add(sfile);
                        logger.Info(@"Found new file ""{0}""", file.FileName);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Some errors in RSS parsing (check login)");
                }

                if (cancellationPending)
                {
                    return;
                }

                //update RSS feed
                int progress = (Config.RssFeedList.IndexOf(feed) + 1) * 100;
                progress = progress / Config.RssFeedList.Count;
            }

            Config.Save();

            if (downloadFileList.Count == 0)
            {
                logger.Info("Nothing to download");
                return;
            }
            else
            {
                logger.Info("Total file found " + downloadFileList.Count);
            }

            IMuleWebManager service = null;

            switch (Config.ServiceType)
            {
                case Config.eServiceType.aMule:
                    logger.Info("Load aMule service");
                    service = new aMuleWebManager(Config.ServiceUrl, Config.Password);
                    break;

                case Config.eServiceType.eMule:
                default:
                    logger.Info("Load eMule service");
                    service = new eMuleWebManager(Config.ServiceUrl, Config.Password);
                    break;
            }

            //
            //  if emule is close and new file < min to start not do null
            //
            //
            // try to start emule
            // the if work only if rc == null ad
            if ((Config.StartEmuleIfClose != true) & (downloadFileList.Count <= Config.MinToStartEmule))
            {
                logger.Info("Emule off line");
                logger.Info("Min files to download not reached");
                return;
            }

            for (int cont = 1; (cont <= 5) & (service.Connect() != LoginStatus.Logged); cont++)
            {
                if (cancellationPending == true)
                {
                    return;
                }

                logger.Info(string.Format("Start eMule Now (try {0}/5)", cont));
                logger.Info("Wait 60 sec");
                try
                {
                    Process.Start(Config.eMuleExe);
                }
                catch
                {
                    logger.Info("Unable start application");
                }

                // avoid blocking during delay
                for (int n = 0; n < 10; n++)
                {
                    if (cancellationPending)
                    {
                        return;
                    }

                    Thread.Sleep(500);
                }
            }

            logger.Info("Check min download");
            if (service.IsConnected == false)
            {
                logger.Info("Unable to connect to eMule web server");
                return;
            }

            logger.Info("Retrieve list category");
            service.GetCategories(true);  // force upgrade category list

            logger.Debug("Clean download list (step 1) find channel from ed2k");

            List<Ed2kfile> knownFiles = new List<Ed2kfile>();
            Config.RssFeedList.GetDownloadFiles().ForEach((x) => knownFiles.Add(x.File));

            List<Ed2kfile> courrentDownloadsFormEmule = service.GetCurrentDownloads(knownFiles);/// file downloaded with this program and now in download in emule
            if (courrentDownloadsFormEmule == null)
            {
                logger.Error("eMule web server not respond");
                return;
            }

            logger.Info("Current Download Form Emule {0}", courrentDownloadsFormEmule.Count);

            //for debug
            courrentDownloadsFormEmule.ForEach(delegate (Ed2kfile file) { logger.Info(file.FileName.Replace("%20", " ")); });

            //
            //  note move this function in MainHistory class
            //
            logger.Info("Start search in history");

            List<DownloadFile> actualDownloadFileList = new List<DownloadFile>();
            foreach (var downloadedFile in Config.RssFeedList.GetDownloadFiles())
            {
                if (courrentDownloadsFormEmule.Contains(downloadedFile.File) == true)
                {
                    actualDownloadFileList.Add(downloadedFile);
                }
            }

            actualDownloadFileList.ForEach(delegate (DownloadFile file) { logger.Info("Found: \"{0}\"", file.File.FileName); });

            logger.Info("ActualDownloadFileList.Count = " + actualDownloadFileList.Count);
            logger.Info("Config.MaxSimultaneousFeedDownloads = " + Config.MaxSimultaneousFeedDownloadsDefault);

            // create a dictionary to count
            Dictionary<RssSubscription, int> maxSimultaneousDownloadsDictionary = new Dictionary<RssSubscription, int>();
            // set starting point for each feed
            foreach (RssSubscription rssFeed in Config.RssFeedList)
            {
                maxSimultaneousDownloadsDictionary.Add(rssFeed, (int)rssFeed.MaxSimultaneousDownload);
            }

            //
            //  remove all file already in download
            //
            foreach (DownloadFile file in actualDownloadFileList)
            {
                if (maxSimultaneousDownloadsDictionary.ContainsKey(file.Subscription) == true)
                {
                    int counter = Math.Max(0, maxSimultaneousDownloadsDictionary[file.Subscription] - 1);
                    maxSimultaneousDownloadsDictionary[file.Subscription] = counter;
                }
            }

            logger.Info("Download file");
            //
            //  Download file
            //
            foreach (sDonwloadFile downloadFile in downloadFileList)
            {
                //  this code allow to block anytime the loop
                if (cancellationPending)
                {
                    return;
                }

                int msdd = maxSimultaneousDownloadsDictionary[downloadFile.Subscription];
                if (msdd <= 0)
                {
                    string fileName = new Ed2kfile(downloadFile.File).FileName;
                    logger.Info(string.Format("File skipped: \"{0}\"", fileName));
                    continue;
                }

                maxSimultaneousDownloadsDictionary[downloadFile.Subscription] = Math.Max(0, msdd - 1);

                Ed2kfile ed2klink = new Ed2kfile(downloadFile.File);
                logger.Info("Add file to download");
                service.AddToDownload(ed2klink, downloadFile.Subscription.Category);

                if (downloadFile.Subscription.PauseDownload == true)
                {
                    logger.Info("Pause download");
                    service.StopDownload(ed2klink);
                }
                else
                {
                    logger.Info("Resume download");
                    service.StartDownload(ed2klink);
                }
                // mark the file download
                downloadFile.Subscription.SetFileDownloaded(downloadFile.File);

                logger.Info("Add file to emule \"{0}\"", downloadFile.File.GetFileName());
                SendMailDownload(downloadFile.File.GetFileName(), downloadFile.File.Ed2kLink);
                Config.TotalDownloads++;   //increase Total Downloads for statistic

            }
            Config.Save();

            logger.Info("Force Refresh Shared File List");
            service.ForceRefreshSharedFileList();

            logger.Info("Logout Emule");
            service.Close();

            logger.Info("Statistics");
            logger.Info(string.Format("Total file added {0}", Config.TotalDownloads));

            try
            {
                if (VersionChecker.CheckNewVersion(Config, true) == true)
                {
                    logger.Info("New Version is available at http://tvudownloader.sourceforge.net/");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        void SendMailDownload(string fileName, string ed2kLink)
        {
            if (Config.EmailNotification == true)
            {
                try
                {
                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                    message.To.Add(Config.MailReceiver);
                    message.Subject = "TV Underground Downloader Notification";
                    message.From = new System.Net.Mail.MailAddress(Config.MailSender);
                    message.Body = "New file add\r\n" + fileName + "\r\n";
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Config.ServerSMTP);

                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }

            }
        }

        public void Run()
        {
            thread.IsBackground = true;
            thread.Start();
        }

        public void Abort()
        {
            cancellationPending = true;
            if (!thread.Join(3000))
            { // give the thread 3 seconds to stop
                thread.Abort();
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
