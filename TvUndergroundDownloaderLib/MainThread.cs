﻿using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TvUndergroundDownloaderLib.Extensions;

namespace TvUndergroundDownloaderLib
{
    public delegate void WorkerEventHandler(object sender, EventArgs e);

    public class Worker
    {
        public Config Config = null;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private CancellationToken _cancellationToken;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _task;

        /// <summary>
        /// Constructor
        /// </summary>
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

        /// <summary>
        /// Abort the task
        /// </summary>
        public void Abort()
        {
            _cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Run
        /// </summary>
        public void Run()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _task = new Task(WorkerFunc, _cancellationToken);
            _task.ContinueWith(PostRun);
            _task.Start();
        }

        /// <summary>
        /// Main worker function
        /// </summary>
        public void WorkerFunc()
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
            if (string.IsNullOrEmpty(Config.TvuUserName))
            {
                throw new LoginException("Missing Tvu UserName");
            }

            if (string.IsNullOrEmpty(Config.TvuPassword))
            {
                throw new LoginException("Missing Tvu Password");
            }

            if (TryToLoignToTvu(Config.TvuUserName, Config.TvuPassword, cookieContainer) == false)
            {
                throw new LoginException("Unable to login to TVU");
            }

            //
            //  start RSS Check
            //
            _logger.Info("Start RSS Check");

            var downloadFileList = new List<DownloadFile>();

            // select only enabled RSS
            var rssFeedList = Config.RssFeedList.FindAll(delegate (RssSubscription rss) { return rss.Enabled; });

            //
            //  First update the chanels
            //
            foreach (var feed in rssFeedList)
            {
                try
                {
                    _logger.Info(@"Read RSS ""{0}""", feed.TitleCompact);
                    feed.Update(cookieContainer);
                    foreach (var file in feed.GetPendingFiles().Take(feed.MaxSimultaneousDownload))
                    {
                        _logger.Info(@"Found new file ""{0}""", file.FileName);
                    }
                }
                catch (LoginException ex)
                {
                    _logger.Error(ex, "Error: missing login cookie");
                }
                catch (WebException ex)
                {
                    _logger.Error(ex, "Network error: Check internet connection");
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

            //
            //  Add feed to list to download
            //
            foreach (var feed in rssFeedList)
            {
                int counter = feed.MaxSimultaneousDownload;

                foreach (var file in feed.GetPendingFiles())
                {
                    if (counter > 0)
                    {
                        downloadFileList.Add(file);
                        counter--;
                    }
                }
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

                if (string.IsNullOrEmpty(Config.eMuleExe))
                {
                    _logger.Warn("Mule.exe is not configured");
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

            //
            //  Free space check
            //

            #region Free space check

            try
            {
                BigInteger freeSpace = service.FreeSpace;
                _logger.Warn("Free space on temp Tempdrive: {0}", freeSpace.SmartFomater());

                freeSpace = freeSpace - Config.MinFreeSpace;

                while (downloadFileList.Count > 0 && downloadFileList.SumBigInteger(o => o.FileSize) > freeSpace)
                {
                    var biggestFile = downloadFileList.OrderByDescending(o => o.FileSize).FirstOrDefault();
                    if (biggestFile == null)
                        break;

                    downloadFileList.Remove(biggestFile);
                    _logger.Warn("Out of space: {0} ({1})", biggestFile.FileName, new BigInteger(biggestFile.FileSize).SmartFomater());
                }
            }
            catch (Exception e)
            {
                _logger.Info("This provider not support \"Get Free Space\"");
                _logger.Debug(e);
            }
            _logger.Warn("");

            #endregion Free space check

            //
            //  Download file
            //

            #region Download file

            _logger.Info("Download file");
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

            #endregion Download file

            //
            //  remove all completed
            //
            foreach (var rssSubscription in Config.RssFeedList.FindAll(MatchDeleteWhenCompleted))
            {
                _logger.Info("Auto remove: \"{0}\"", rssSubscription.TitleCompact);
            }

            Config.RssFeedList.RemoveAll(MatchDeleteWhenCompleted);

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
            catch (UnsupportedVersionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            OnWorkerCompleted();
        }

        /// <summary>
        /// filter to delete
        /// </summary>
        /// <param name="rssSubscription"></param>
        /// <returns></returns>
        private bool MatchDeleteWhenCompleted(RssSubscription rssSubscription)
        {
            if (rssSubscription.DeleteWhenCompleted == false)
            {
                return false;
            }

            if (rssSubscription.CurrentTVUStatus != TvuStatus.Complete)
            {
                return false;
            }

            DateTime lastDownload = DateTime.MinValue;
            foreach (DownloadFile file in rssSubscription.DownloadedFiles)
            {
                if (!file.DownloadDate.HasValue)
                {
                    return false;
                }
                if (lastDownload < file.DownloadDate.Value)
                {
                    lastDownload = file.DownloadDate.Value;
                }
            }

            if (lastDownload < DateTime.Now.AddDays(3))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// On Worker Completed
        /// </summary>
        private void OnWorkerCompleted()
        {
            if (WorkerCompleted != null)
                WorkerCompleted(this, new EventArgs());
        }

        /// <summary>
        /// Post Run Exception Handle
        /// </summary>
        /// <param name="task"></param>
        private void PostRun(Task task)
        {
            if (task.Exception != null)
            {
                var flattened = task.Exception.Flatten();

                flattened.Handle(ex =>
                {
                    _logger.Error(ex);
                    return true;
                });
            }
            OnWorkerCompleted();
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="ed2kLink"></param>
        /// todo: use class Ed2k instead of string
        private void SendMailDownload(string fileName, string ed2kLink)
        {
            if (Config.EmailNotification)
                try
                {
                    string subject = "TV Underground Downloader Notification";
                    string body = "New file add\r\n" + fileName + "\r\n";

                    SmtpSimpleClient simpleClient = new SmtpSimpleClient(this.Config);
                    simpleClient.SendEmail(subject, body);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
        }

        /// <summary>
        /// Try to login on TVU site
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="cookieContainer"></param>
        /// <returns></returns>
        public static bool TryToLoignToTvu(string name, string password, CookieContainer cookieContainer)
        {
            //
            //  Enable TLS 1.2 on legacy code
            //      https://stackoverflow.com/questions/37869135/is-that-possible-to-send-httpwebrequest-using-tls1-2-on-net-4-0-framework/41854685#41854685
            //      System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            // Create POST data and convert it to a byte array.
            string postData = string.Format("name={0}&password={1}&submit=Login", name, password);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            string responseFromServer = null;

            var request = (HttpWebRequest)WebRequest.Create("https://tvunderground.org.ru/index.php?show=login&act=login");
            request.CookieContainer = cookieContainer;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "POST";
            request.ContentLength = byteArray.Length;

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
            }

            // Close the response.
            response.Close();
#if DEBUG
            File.WriteAllText(string.Format("login-{0:HH}-{0:mm}-{0:ss}.html", DateTime.Now), responseFromServer);
#endif
            if (responseFromServer.IndexOf("index.php?show=login&amp;act=logout") > -1)
            {
                return true;
            }

            return false;
        }
    }
}