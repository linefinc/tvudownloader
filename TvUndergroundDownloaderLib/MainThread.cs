using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Channels;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace TvUndergroundDownloaderLib
{
    public delegate void WorkerEventHandler(object sender, EventArgs e);

    public class Worker
    {
        public Config Config = null;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private Task _task;

        public Worker()
        {

        }

        /// <summary>
        ///     Event call when the thread is completed or aborted
        /// </summary>
        public event WorkerEventHandler WorkerCompleted;


        // public ThreadState ThreadState => thread.ThreadState;

        public bool IsBusy
        {
            get
            {
                if (_task == null)
                    return false;

                return (_task.IsCompleted == false ||
                        _task.Status == TaskStatus.Running);
                //return (task.IsCompleted == false ||
                //        task.Status == TaskStatus.Running ||
                //        task.Status == TaskStatus.WaitingToRun ||
                //        task.Status == TaskStatus.WaitingForActivation);
            }
        }


        public void Abort()
        {
            _cancellationTokenSource.Cancel();
        }

        public void Run()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _task = new Task(WorkerThreadFunc, _cancellationTokenSource.Token);
            _task.Start();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void WorkerThreadFunc()
        {
            if (Config == null)
            {
                throw new NullReferenceException(nameof(Config));
            }

            //
            //  Load Cookies from configure
            //
            var cookieContainer = new CookieContainer();
            var uriTvunderground = new Uri("http://tvunderground.org.ru/");
            if (string.IsNullOrEmpty(Config.TVUCookieH))
            {
                throw new LoginException("H");
            }
            cookieContainer.Add(uriTvunderground, new Cookie("h", Config.TVUCookieH));

            if (string.IsNullOrEmpty(Config.TVUCookieI))
            {
                throw new LoginException("I");
            }
            cookieContainer.Add(uriTvunderground, new Cookie("i", Config.TVUCookieI));

            if (string.IsNullOrEmpty(Config.TVUCookieT))
            {
                throw new LoginException("T");
            }
            cookieContainer.Add(uriTvunderground, new Cookie("t", Config.TVUCookieT));
            //
            //  start RSS Check
            //
            _logger.Info("Start RSS Check");

            var downloadFileList = new List<DownloadFile>();

            // select only enabled RSS
            var rssFeedList = Config.RssFeedList.FindAll(delegate (RssSubscription rss) { return rss.Enabled; });

            foreach (var feed in rssFeedList)
            {
                try
                {
                    _logger.Info(@"Read RSS ""{0}""", feed.TitleCompact);
                    feed.Update(cookieContainer);
                    foreach (var file in feed.GetNewDownload((int)Config.MaxSimultaneousFeedDownloadsDefault))
                    {
                        var sfile = new DownloadFile(feed, file);
                        downloadFileList.Add(sfile);
                        _logger.Info(@"Found new file ""{0}""", file.FileName);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Some errors in RSS parsing (check login)");
                }

                if (_cancellationToken.IsCancellationRequested)
                {
                    OnWorkerCompleted();
                    return;
                }

                //update RSS feed
                int progress = (Config.RssFeedList.IndexOf(feed) + 1) * 100;
                progress = progress / Config.RssFeedList.Count;
            }

            Config.Save();

            if (downloadFileList.Count == 0)
            {
                _logger.Info("Nothing to download");
                OnWorkerCompleted();
                return;
            }
            _logger.Info("Total file found " + downloadFileList.Count);

            IMuleWebManager service = null;

            switch (Config.ServiceType)
            {
                case Config.eServiceType.aMule:
                    _logger.Info("Load aMule service");
                    service = new aMuleWebManager(Config.ServiceUrl, Config.Password);
                    break;

                case Config.eServiceType.eMule:
                default:
                    _logger.Info("Load eMule service");
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
                _logger.Info("Emule off line");
                _logger.Info("Min files to download not reached");
                OnWorkerCompleted();
                return;
            }

            for (int cont = 1; (cont <= 5) & (service.Connect() != LoginStatus.Logged); cont++)
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    OnWorkerCompleted();
                    return;
                }

                _logger.Info("Start eMule Now (try {0}/5)", cont);
                _logger.Info("Wait 60 sec");
                try
                {
                    Process.Start(Config.eMuleExe);
                }
                catch
                {
                    _logger.Info("Unable start application");
                }

                // avoid blocking during delay
                for (int n = 0; n < 10; n++)
                {
                    if (_cancellationToken.IsCancellationRequested)
                    {
                        OnWorkerCompleted();
                        return;
                    }
                    Thread.Sleep(500);
                }
            }

            _logger.Info("Check min download");
            if (service.IsConnected == false)
            {
                _logger.Info("Unable to connect to eMule web server");
                OnWorkerCompleted();
                return;
            }

            _logger.Info("Retrieve list category");
            service.GetCategories(true); // force upgrade category list

            _logger.Debug("Clean download list (step 1) find channel from ed2k");

            var knownFiles = new List<Ed2kfile>();
            Config.RssFeedList.GetDownloadFiles().ForEach(x => knownFiles.Add(x));

            var courrentDownloadsFormEmule =
                service.GetCurrentDownloads(
                    knownFiles); // file downloaded with this program and now in download in emule
            if (courrentDownloadsFormEmule == null)
            {
                _logger.Error("eMule web server not respond");
                OnWorkerCompleted();
                return;
            }

            _logger.Info("Current Download Form Emule {0}", courrentDownloadsFormEmule.Count);

            //for debug
            courrentDownloadsFormEmule.ForEach(delegate (Ed2kfile file)
            {
                _logger.Info(file.FileName.Replace("%20", " "));
            });

            //
            //  note move this function in MainHistory class
            //
            _logger.Info("Start search in history");

            var actualDownloadFileList = new List<DownloadFile>();
            foreach (var downloadedFile in Config.RssFeedList.GetDownloadFiles())
                if (courrentDownloadsFormEmule.Contains(downloadedFile))
                    actualDownloadFileList.Add(downloadedFile);

            actualDownloadFileList.ForEach(
                delegate (DownloadFile file) { _logger.Info("Found: \"{0}\"", file.FileName); });

            _logger.Info("ActualDownloadFileList.Count = " + actualDownloadFileList.Count);
            _logger.Info("Config.MaxSimultaneousFeedDownloads = " + Config.MaxSimultaneousFeedDownloadsDefault);

            // create a dictionary to count
            var maxSimultaneousDownloadsDictionary = new Dictionary<RssSubscription, int>();
            // set starting point for each feed
            foreach (var rssFeed in Config.RssFeedList)
                maxSimultaneousDownloadsDictionary.Add(rssFeed, (int)rssFeed.MaxSimultaneousDownload);

            //
            //  remove all file already in download
            //
            foreach (var file in actualDownloadFileList)
                if (maxSimultaneousDownloadsDictionary.ContainsKey(file.Subscription))
                {
                    int counter = Math.Max(0, maxSimultaneousDownloadsDictionary[file.Subscription] - 1);
                    maxSimultaneousDownloadsDictionary[file.Subscription] = counter;
                }

            _logger.Info("Download file");
            //
            //  Download file
            //
            foreach (var downloadFile in downloadFileList)
            {
                //  this code allow to block anytime the loop
                if (_cancellationToken.IsCancellationRequested)
                {
                    OnWorkerCompleted();
                    return;
                }

                int msdd = maxSimultaneousDownloadsDictionary[downloadFile.Subscription];
                if (msdd <= 0)
                {
                    string fileName = new Ed2kfile(downloadFile).FileName;
                    _logger.Info("File skipped: \"{0}\"", fileName);
                    continue;
                }

                maxSimultaneousDownloadsDictionary[downloadFile.Subscription] = Math.Max(0, msdd - 1);

                var ed2klink = new Ed2kfile(downloadFile);
                _logger.Info("Add file to download");
                service.AddToDownload(ed2klink, downloadFile.Subscription.Category);

                if (downloadFile.Subscription.PauseDownload)
                {
                    _logger.Info("Pause download");
                    service.StopDownload(ed2klink);
                }
                else
                {
                    _logger.Info("Resume download");
                    service.StartDownload(ed2klink);
                }
                // mark the file download
                downloadFile.Subscription.SetFileDownloaded(downloadFile);

                _logger.Info("Add file to emule \"{0}\"", downloadFile.GetFileName());
                SendMailDownload(downloadFile.GetFileName(), downloadFile.Ed2kLink);
                Config.TotalDownloads++; //increase Total Downloads for statistic
            }
            Config.Save();

            _logger.Info("Force Refresh Shared File List");
            service.ForceRefreshSharedFileList();

            _logger.Info("Logout Emule");
            service.Close();

            _logger.Info("Statistics");
            _logger.Info("Total file added {0}", Config.TotalDownloads);

            try
            {
                if (VersionChecker.CheckNewVersion(Config, true))
                    _logger.Info("New Version is available at http://tvudownloader.sourceforge.net/");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            OnWorkerCompleted();
        }

        private void OnWorkerCompleted()
        {
            if (WorkerCompleted != null)
                WorkerCompleted(this, new EventArgs());
        }
        private void SendMailDownload(string fileName, string ed2kLink)
        {
            if (Config.EmailNotification)
                try
                {
                    var message = new MailMessage();
                    message.To.Add(Config.MailReceiver);
                    message.Subject = "TV Underground Downloader Notification";
                    message.From = new MailAddress(Config.MailSender);
                    message.Body = "New file add\r\n" + fileName + "\r\n";
                    var smtp = new System.Net.Mail.SmtpClient(Config.ServerSMTP);

                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
        }
    }
}