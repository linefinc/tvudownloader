using NLog;
using NLog.Config;
using NLog.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace TvUndergroundDownloader
{
    public partial class FormMain : Form
    {
        public Config MainConfig;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private DateTime autoCloseDataTime;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private DateTime downloadDataTime;
        private Icon iconDown;
        private Icon iconUp;
        private bool allowClose = false;
        private System.Windows.Forms.MenuItem menuItemAutoCloseEmule;
        private System.Windows.Forms.MenuItem menuItemAutoStartEmule;
        private System.Windows.Forms.MenuItem menuItemCheckNow;
        private System.Windows.Forms.MenuItem menuItemEnable;
        private System.Windows.Forms.MenuItem menuItemExit;
        private bool mVisible = true;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private EmbendedWebServer.EmbendedWebServer embendedWebServer;

        public FormMain()
        {
            // load configuration
            MainConfig = new Config();
            MainConfig.Load();

            InitializeComponent();
            SetupNotify();

            embendedWebServer = new EmbendedWebServer.EmbendedWebServer();
            embendedWebServer.Start();

            EmbendedWebServer.GlobalVar.Config = MainConfig;
            EmbendedWebServer.GlobalVar.MainBackgroundWorker = backgroundWorker1;
        }

        public static string GetUserAppDataPath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path += '\\' + Application.ProductName + '\\';
            return path;
        }

        /// <summary>
        /// Show or Hide application form
        /// </summary>
        public void ApplicationShowHide()
        {
            if (this.Visible == true)
            {
                mVisible = false;
                this.Visible = false;
            }
            else
            {
                mVisible = true;
                this.Visible = true;
            }
        }

        //
        //  to insert inside a timer3
        //  here for debug
        //
        public void autoclose()
        {
            if (MainConfig.CloseEmuleIfAllIsDone == false)
            {
                logger.Info("[AutoClose Mule] MainConfig.CloseEmuleIfAllIsDone == false");
                return;
            }

            // check if Auto Close Data Time is not set
            if (autoCloseDataTime == DateTime.MinValue)
            {
                logger.Info("[AutoClose Mule] AutoCloseDataTime = DateTime.Now.AddMinutes(30);");
                autoCloseDataTime = DateTime.Now.AddMinutes(30);
            }

            //
            // Auto close
            //
            if (DateTime.Now < autoCloseDataTime)
            {
                return;
            }

            // suspend event while connect with mule
            timerAutoClose.Enabled = false;

            // connect to mule
            try
            {
                logger.Info("[AutoClose Mule] Check Login");
                eMuleWebManager service = new eMuleWebManager(MainConfig.ServiceUrl, MainConfig.Password);
                LoginStatus returnCode = service.Connect();

                // if mule close ... end of game
                if (returnCode != LoginStatus.Logged)
                {
                    autoCloseDataTime = DateTime.Now.AddMinutes(30); // do control every 30 minutes
                    logger.Info("[AutoClose Mule] Login failed");
                    return;
                }
                logger.Info("[AutoClose Mule] Login ok");

                List<Ed2kfile> knownFiles = new List<Ed2kfile>();
                MainConfig.RssFeedList.ForEach((file) => knownFiles.AddRange(file.GetDownloadedFiles()));

                logger.Info("[AutoClose Mule] Actual Downloads " + service.GetCurrentDownloads(knownFiles).Count);
                // if donwload > 0 ... there' s some download ... end
                if (service.GetCurrentDownloads(knownFiles).Count > 0)
                {
                    logger.Info("[AutoClose Mule] GetActualDownloads return >0");
                    autoCloseDataTime = DateTime.Now.AddMinutes(30);
                    logger.Info("[AutoClose Mule] LogOut");
                    service.Close();
                    return;
                }

                logger.Info("[AutoClose Mule] Show dialog ");
                // pop up form to advise user
                FormAlerteMuleClose dialog = new FormAlerteMuleClose();
                dialog.ShowDialog();

                logger.Info("[AutoClose Mule] Dialog return " + dialog.AlertChoice.ToString());
                switch (dialog.AlertChoice)
                {
                    case AlertChoiceEnum.Close:// Close
                        logger.Info("[AutoClose Mule: CLOSE] Close Service");
                        dialog.Dispose();
                        service.CloseEmuleApp();
                        service.Close();
                        timerAutoClose.Enabled = true;  // enable timer
                        break;
                    // to fix here
                    case AlertChoiceEnum.Skip: // SKIP
                        autoCloseDataTime = DateTime.Now.AddMinutes(30); // do controll every 30 minuts
                        logger.Info("[AutoClose Mule: SKIP] Skip");
                        logger.Info("[AutoClose Mule: SKIP] Next Tock " + autoCloseDataTime.ToString());
                        dialog.Dispose();
                        logger.Info("[AutoClose Mule] LogOut");
                        service.Close();
                        timerAutoClose.Enabled = true;  // enable timer
                        break;

                    case AlertChoiceEnum.Disable:    // disable autoclose
                        logger.Info("[AutoClose Mule: DISABLE] Disable");
                        dialog.Dispose();
                        logger.Info("[AutoClose Mule] LogOut");
                        service.Close();
                        DisableAutoCloseEmule();
                        timerAutoClose.Enabled = true;  // enable timer
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Autoclose error");
            }
        }

        public void DisableAutoCloseEmule()
        {
            this.menuItemAutoCloseEmule.Checked = false; // disable context menu
            this.autoCloseEMuleToolStripMenuItem.Checked = false; // File -> Menu -> Configure
            this.MainConfig.CloseEmuleIfAllIsDone = false; // disable function
            timerAutoClose.Enabled = false;
        }

        public void DisableAutoStarteMule()
        {
        }

        public void EnableAutoCloseEmule()
        {
            this.menuItemAutoCloseEmule.Checked = true;   // Enable trybar context menu
            this.autoCloseEMuleToolStripMenuItem.Checked = true; // File -> Menu -> Configure
            this.MainConfig.CloseEmuleIfAllIsDone = true; // Enable function
            autoCloseDataTime = DateTime.Now.AddMinutes(30);
            timerAutoClose.Enabled = true;
        }

        public void EnableAutoStarteMule()
        {
        }

        public void SendMailDownload(string fileName, string ed2kLink)
        {
            if (MainConfig.EmailNotification == true)
            {
                string stmpServer = MainConfig.ServerSMTP;
                string emailReceiver = MainConfig.MailReceiver;
                string emailSender = MainConfig.MailSender;
                string subject = "TV Underground Downloader Notification";
                string message = "New file add\r\n" + fileName + "\r\n";
                SmtpClient.SendEmail(stmpServer, emailReceiver, emailSender, subject, message);
            }
        }

        protected override void SetVisibleCore(bool value)
        {
            // MSDN http://social.msdn.microsoft.com/forums/en-US/csharpgeneral/thread/eab563c3-37d0-4ebd-a086-b9ea7bb03fed
            if (!mVisible)
                value = false;         // Prevent form getting visible
            base.SetVisibleCore(value);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox1 dialog = new AboutBox1();
            dialog.ShowDialog();
        }

        /// <summary>
        /// wizard for add new feed
        /// </summary>
        private void AddRssChannel()
        {
            if ((MainConfig.tvuCookieH == string.Empty) | (MainConfig.tvuCookieI == string.Empty) | (MainConfig.tvuCookieT == string.Empty))
            {
                MessageBox.Show("Please login before add new RSS feed (File > Login)");
                return;
            }

            if (MainConfig.Password == string.Empty)
            {
                MessageBox.Show("Please configure eMule web interface (File > Option > Global Option > Network)");
            }

            List<string> currentRssUrlList = new List<string>();
            //
            //  Load Cookies from configuration
            //
            CookieContainer cookieContainer = new CookieContainer();
            Uri uriTvunderground = new Uri("http://tvunderground.org.ru/");
            cookieContainer.Add(uriTvunderground, new Cookie("h", MainConfig.tvuCookieH));
            cookieContainer.Add(uriTvunderground, new Cookie("i", MainConfig.tvuCookieI));
            cookieContainer.Add(uriTvunderground, new Cookie("t", MainConfig.tvuCookieT));

            //
            //  Get list of current feed URL
            //
            MainConfig.RssFeedList.ForEach(delegate (RssSubscription t) { currentRssUrlList.Add(t.Url); });

            //
            //  Open dialog 1 to path the URL
            //
            AddFeedDialogPage1 dialogPage1 = new AddFeedDialogPage1(currentRssUrlList);
            dialogPage1.ShowDialog();

            if (dialogPage1.DialogResult != DialogResult.OK)
            {
                dialogPage1.Dispose();
                return;
            }

            if (dialogPage1.RssUrlList.Count == 0)
            {
                MessageBox.Show("Nothing to downloads");
                return;
            }

            List<string> rssUrlList = dialogPage1.RssUrlList;
            bool fastAdd = dialogPage1.FastAdd;
            dialogPage1.Dispose();
            //
            //  Open dialog 2 to get all ed2k from feed
            //
            AddFeedDialogPage2 dialogPage2 = new AddFeedDialogPage2(rssUrlList, MainConfig.ServiceUrl, MainConfig.Password, cookieContainer, fastAdd);
            dialogPage2.ShowDialog();

            if (dialogPage2.DialogResult != DialogResult.OK)
            {
                dialogPage2.Dispose();
                return;
            }

            //
            //  check file count
            //
            if ((dialogPage2.rssSubscriptionsList.Count == 0) & (fastAdd == false))
            {
                MessageBox.Show("Nothing to downloads");
                dialogPage2.Dispose();
                return;
            }

            List<RssSubscription> rssSubscriptionsList = dialogPage2.rssSubscriptionsList;
            List<string> eMuleCategoryList = dialogPage2.ListCategory;
            dialogPage2.Dispose();  // free dialog
                                    // setup default

            foreach (RssSubscription rssSubscription in rssSubscriptionsList)
            {
                rssSubscription.Category = MainConfig.DefaultCategory;
                rssSubscription.PauseDownload = MainConfig.PauseDownloadDefault;
                rssSubscription.Enabled = true;
                rssSubscription.MaxSimultaneousDownload = MainConfig.MaxSimultaneousFeedDownloadsDefault;
            }

            if (fastAdd == false)
            {
                ////
                //  show Page 3 : choose single files to download.
                //
                AddFeedDialogPage3 dialogPage3 = new AddFeedDialogPage3(rssSubscriptionsList, eMuleCategoryList);
                dialogPage3.ShowDialog();
                if (dialogPage3.DialogResult != DialogResult.OK)
                {
                    dialogPage3.Dispose();
                    return;
                }
            }

            MainConfig.RssFeedList.AddRange(dialogPage2.rssSubscriptionsList);
            MainConfig.Save();

            UpdateRssFeedGUI();
            StartDownloadThread();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRssChannel();
        }

        /// <summary>
        /// Close the application
        /// </summary>
        private void ApplicationExit()
        {
            mVisible = true;
            this.Visible = true;
            allowClose = true;
            this.Close();
        }

        private void autoClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (autoClearToolStripMenuItem.Checked == false)
            {
                autoClearToolStripMenuItem.Checked = true;
                MainConfig.AutoClearLog = true;
            }
            else
            {
                autoClearToolStripMenuItem.Checked = false;
                MainConfig.AutoClearLog = false;
            }
        }

        private void autoCloseEMuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainConfig.CloseEmuleIfAllIsDone == false)
            {
                EnableAutoCloseEmule();
            }
            else
            {
                DisableAutoCloseEmule();
            }
        }

        private void autoStartEMuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainConfig.StartEmuleIfClose == false)
            {
                MainConfig.StartEmuleIfClose = true;
                autoStartEMuleToolStripMenuItem.Checked = true;
            }
            else
            {
                MainConfig.StartEmuleIfClose = false;
                autoStartEMuleToolStripMenuItem.Checked = false;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //
            //  Load Cookies from configure
            //
            CookieContainer cookieContainer = new CookieContainer();
            Uri uriTvunderground = new Uri("http://tvunderground.org.ru/");
            cookieContainer.Add(uriTvunderground, new Cookie("h", MainConfig.tvuCookieH));
            cookieContainer.Add(uriTvunderground, new Cookie("i", MainConfig.tvuCookieI));
            cookieContainer.Add(uriTvunderground, new Cookie("t", MainConfig.tvuCookieT));

            //
            //  start RSS Check
            //
            logger.Info("Start RSS Check");

            List<sDonwloadFile> downloadFileList = new List<sDonwloadFile>();

            // select only enabled RSS
            List<RssSubscription> rssFeedList = MainConfig.RssFeedList.FindAll(delegate (RssSubscription rss) { return rss.Enabled == true; });

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

                    //
                    //  this code allow to block anytime the loop
                    //
                    if (backgroundWorker1.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Some errors in RSS parsing (check login)");
                }

                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                //update RSS feed
                int progress = (MainConfig.RssFeedList.IndexOf(feed) + 1) * 100;
                progress = progress / MainConfig.RssFeedList.Count;
                backgroundWorker1.ReportProgress(progress);
            }

            MainConfig.Save();

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

            switch (MainConfig.ServiceType)
            {
                case Config.eServiceType.aMule:
                    logger.Info("Load aMule service");
                    service = new aMuleWebManager(MainConfig.ServiceUrl, MainConfig.Password);
                    break;

                case Config.eServiceType.eMule:
                default:
                    logger.Info("Load eMule service");
                    service = new eMuleWebManager(MainConfig.ServiceUrl, MainConfig.Password);
                    break;
            }

            //
            //  if emule is close and new file < min to start not do null
            //
            //
            // try to start emule
            // the if work only if rc == null ad
            if ((MainConfig.StartEmuleIfClose != true) & (downloadFileList.Count <= MainConfig.MinToStartEmule))
            {
                logger.Info("Emule off line");
                logger.Info("Min files to download not reached");
                return;
            }

            for (int cont = 1; (cont <= 5) & (service.Connect() != LoginStatus.Logged); cont++)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                logger.Info(string.Format("Start eMule Now (try {0}/5)", cont));
                logger.Info("Wait 60 sec");
                try
                {
                    Process.Start(MainConfig.eMuleExe);
                }
                catch
                {
                    logger.Info("Unable start application");
                }

                // avoid blocking during delay
                for (int n = 0; n < 10; n++)
                {
                    if (backgroundWorker1.CancellationPending)
                    {
                        e.Cancel = true;
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
            MainConfig.RssFeedList.GetDownloadFiles().ForEach((x) => knownFiles.Add(x.File));

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
            foreach (var downloadedFile in MainConfig.RssFeedList.GetDownloadFiles())
            {
                if (courrentDownloadsFormEmule.Contains(downloadedFile.File) == true)
                {
                    actualDownloadFileList.Add(downloadedFile);
                }
            }

            actualDownloadFileList.ForEach(delegate (DownloadFile file) { logger.Info("Found: \"{0}\"", file.File.FileName); });

            logger.Info("ActualDownloadFileList.Count = " + actualDownloadFileList.Count);
            logger.Info("MainConfig.MaxSimultaneousFeedDownloads = " + MainConfig.MaxSimultaneousFeedDownloadsDefault);

            // create a dictionary to count
            Dictionary<RssSubscription, int> maxSimultaneousDownloadsDictionary = new Dictionary<RssSubscription, int>();
            // set starting point for each feed
            foreach (RssSubscription rssFeed in MainConfig.RssFeedList)
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
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
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
                MainConfig.TotalDownloads++;   //increase Total Downloads for statistic

                // progress bar
                int progress = (downloadFileList.IndexOf(downloadFile) + 1) * 100;
                backgroundWorker1.ReportProgress(progress / downloadFileList.Count);
            }
            MainConfig.Save();

            logger.Info("Force Refresh Shared File List");
            service.ForceRefreshSharedFileList();

            logger.Info("Logout Emule");
            service.Close();

            logger.Info("Statistics");
            logger.Info(string.Format("Total file added {0}", MainConfig.TotalDownloads));

            try
            {
                if (CheckNewVersion(true) == true)
                {
                    logger.Info("New Version is available at http://tvudownloader.sourceforge.net/");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// This is on the main thread, so we can update a TextBox or anything.
        /// </summary>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                logger.Info("Background Worker Cancelled");
            }

            UpdateRecentActivity();
            UpdatePendingFiles();
            UpdateRssFeedGUI();

            menuItemCheckNow.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
            addToolStripMenuItem.Enabled = true;
            checkNowToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
            addToolStripMenuItem.Enabled = true;
            toolStripMenuItemAdd.Enabled = true;
            toolStripMenuItemDelete.Enabled = true;
            toolStripMenuItemEdit.Enabled = true;
            toolStripButtonCheckNow.Enabled = true;
            toolStripButtonAddFeed.Enabled = true;
            toolStripButtonStop.Enabled = false;
            cancelCheckToolStripMenuItem.Enabled = false;
        }

        private void cancelCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void checkBoxAutoClear_CheckedChanged(object sender, EventArgs e)
        {
        }

        /// <summary>check if a new version is avable on web</summary>
        /// <returns>true if new version is available or false in other case</returns>
        private bool CheckNewVersion(bool force = false)
        {
            if (MainConfig.intervalBetweenUpgradeCheck == 0)
            {
                return false;
            }

            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;

                DateTime lastCheckDateTime = DateTime.ParseExact(MainConfig.LastUpgradeCheck, "yyyy-MM-dd", provider);
                DateTime nextCheck = lastCheckDateTime.AddDays(MainConfig.intervalBetweenUpgradeCheck);

                lastCheckDateTime = DateTime.ParseExact(MainConfig.LastUpgradeCheck, "yyyy-MM-dd", provider);

                if ((DateTime.Now < nextCheck) & (force == false))
                {
                    return false;
                }

                MainConfig.LastUpgradeCheck = DateTime.Now.ToString("yyyy-MM-dd");

                OperatingSystem osv = Environment.OSVersion;

                GeoIP geoIP = GeoIP.GetCountryFromWeb();

                // require update status
                XmlDocument doc = new XmlDocument();
                string url = string.Format("http://tvudownloader.sourceforge.net/version.php?ver={0}&tvuid={1}&TotalDownloads={2}&osv={3}&countryCode={4}",
                    Config.VersionFull.Replace(" ", "%20"),
                    MainConfig.tvudwid,
                    MainConfig.TotalDownloads,
                    osv.VersionString.Replace(" ", "%20"),
                   geoIP.CountryCode);
                doc.Load(url);

                string lastVersionStr = "";

                foreach (XmlNode t in doc.GetElementsByTagName("last"))
                {
                    lastVersionStr = t.InnerText;
                }

                Regex rg = new Regex(@"\d+\.\d+.\d+");

                Match match = rg.Match(lastVersionStr);
                Version lastVersion = new Version(match.Value);

                // convert
                match = rg.Match(Config.VersionFull);
                Version currentVersion = new Version(match.Value);

                if (currentVersion < lastVersion)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check Now menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartDownloadThread();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Clear();
        }

        private void dataGridViewMain_SelectionChanged(object sender, EventArgs e)
        {
            if ((dataGridViewMain.Rows.Count == 0) | (dataGridViewMain.SelectedRows.Count == 0))
            {
                labelFeedCategory.Text = string.Empty;
                labelFeedPauseDownload.Text = string.Empty;
                labelFeedUrl.Text = string.Empty;

                labelLastDownloadDate.Text = string.Empty;
                labelTotalFiles.Text = string.Empty;
                labelMaxSimultaneousDownloads.Text = string.Empty;
                listViewFeedFilesList.Items.Clear();
                return;
            }

            string titleCompact = dataGridViewMain.SelectedRows[0].Cells[DataGridViewTextBoxColumnTitle.Name].Value.ToString();
            RssSubscription feed = MainConfig.RssFeedList.Find(x => (x.TitleCompact == titleCompact));
            if (feed == null)
            {
                return;
            }

            labelFeedCategory.Text = feed.Category;
            labelFeedPauseDownload.Text = feed.PauseDownload.ToString();
            labelFeedUrl.Text = feed.Url;

            DateTime lastDownloadDate = feed.GetLastDownloadDate();
            if (lastDownloadDate == DateTime.MinValue)
            {
                labelLastDownloadDate.Text = "-";
            }
            else
            {
                labelLastDownloadDate.Text = lastDownloadDate.Date.ToString("yyyy-MM-dd");
            }
            labelTotalFiles.Text = feed.GetDownloadFileCount().ToString();
            labelMaxSimultaneousDownloads.Text = feed.MaxSimultaneousDownload.ToString();

            UpdateSubscriptionFilesList();
        }

        private void DeleteRssChannel()
        {
            if (dataGridViewMain.SelectedRows.Count == 0)
            {
                return;
            }

            // check user
            DialogResult rc;
            rc = MessageBox.Show("Confirm delete", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            if (rc != DialogResult.OK)
            {
                return;
            }

            foreach (DataGridViewRow selectedItem in dataGridViewMain.SelectedRows)
            {
                DataGridViewColumn col = DataGridViewTextBoxColumnTitle;
                string titleCompact = selectedItem.Cells[col.Name].Value.ToString();
                RssSubscription feed = MainConfig.RssFeedList.Find(x => (x.TitleCompact == titleCompact));

                if (feed == null)
                {
                    continue;
                }
                MainConfig.RssFeedList.Remove(feed);
            }
            MainConfig.Save();

            listViewFeedFilesList.Items.Clear();
            UpdateRssFeedGUI(); ///upgrade GUI
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteRssChannel();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count == 0)
                return;

            if (listViewFeedFilesList.Items.Count == 0)
                return;

            if (listViewFeedFilesList.SelectedItems.Count == 0)
                return;

            // get the feed
            DataGridViewColumn col = DataGridViewTextBoxColumnTitle;
            string titleCompact = dataGridViewMain.SelectedRows[0].Cells[col.Name].Value.ToString();
            RssSubscription feed = MainConfig.RssFeedList.Find(x => (x.TitleCompact == titleCompact));

            if (feed == null)
            {
                return;
            }

            foreach (ListViewItem selectedItem in listViewFeedFilesList.SelectedItems)
            {
                string strSelectItemText = selectedItem.Text;   // this contain name file
                logger.Info("Marked as not Downloaded \"{0}\"", strSelectItemText);

                Ed2kfile file = feed.GetDownloadFile().Find(t => t.File.FileName == strSelectItemText).File;

                feed.SetFileNotDownloaded(file);
            }
            // finally update GUI
            UpdateSubscriptionFilesList();
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (ListViewItem selectedItem in listViewFeed.SelectedItems)
            //{
            //    string feedTitle = selectedItem.Text;

            //    RssSubscription Feed = null;

            //    Feed = MainConfig.RssFeedList.Find(delegate (RssSubscription t)
            //    {
            //        return t.TitleCompact.IndexOf(feedTitle) > -1;
            //    });

            //    if (Feed != null)
            //    {
            //        Feed.Enabled = false;

            //    }
            //}
            //MainConfig.Save();

            listViewFeedFilesList.Items.Clear();
            UpdateRssFeedGUI(); ///upgrade GUI
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count == 0)
            {
                return;
            }

            string titleCompact = dataGridViewMain.SelectedRows[0].Cells[0].Value.ToString();
            RssSubscription feed = MainConfig.RssFeedList.Find(x => (x.TitleCompact == titleCompact));

            if (feed == null)
            {
                return;
            }

            EditFeedForm dialog = new EditFeedForm(MainConfig, feed.Category, feed.PauseDownload, feed.Enabled, feed.MaxSimultaneousDownload);
            dialog.ShowDialog();

            if (dialog.DialogResult != DialogResult.OK)
            {
                dialog.Dispose();
                return;
            }

            feed.Enabled = dialog.feedEnable;
            feed.Category = dialog.Category;
            feed.PauseDownload = dialog.PauseDownload;
            feed.MaxSimultaneousDownload = dialog.maxSimultaneousDownload;

            MainConfig.Save();

            listViewFeedFilesList.Items.Clear();
            UpdateRssFeedGUI(); ///upgrade GUI
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (ListViewItem selectedItem in listViewFeed.SelectedItems)
            //{
            //    string feedTitle = selectedItem.Text;

            //    RssSubscription Feed = null;

            //    Feed = MainConfig.RssFeedList.Find(delegate (RssSubscription t)
            //    {
            //        return t.TitleCompact.IndexOf(feedTitle) > -1;
            //    });

            //    if (Feed != null)
            //    {
            //        Feed.Enabled = true;

            //    }
            //}
            MainConfig.Save();

            listViewFeedFilesList.Items.Clear();
            UpdateRssFeedGUI(); ///upgrade GUI
        }

        private void excludeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in listBoxPending.SelectedItems)
            {
                logger.Info("selected item ", item.ToString());
            }
        }

        private void exitNotifyIconMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationExit();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationExit();
        }

        private void exportDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "XML (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    ExportImportHelper.Export(MainConfig, myStream);
                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            logger.Info("FormClosing Event");
            logger.Debug("{0} = {1}", "CloseReason", e.CloseReason);
            logger.Debug("{0} = {1}", "Cancel", e.Cancel);
            logger.Debug("{0} = {1}", "mAllowClose", allowClose);

            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                allowClose = true;
            }

            // MSDN http://social.msdn.microsoft.com/forums/en-US/csharpgeneral/thread/eab563c3-37d0-4ebd-a086-b9ea7bb03fed
            if (!allowClose)
            {                   // Hide when user clicks X
                mVisible = false;
                this.Visible = false;
                e.Cancel = true;
            }
            else
            {
                notifyIcon1.Visible = false;      // Avoid ghost
                backgroundWorker1.CancelAsync();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //
            //  Setup Nlog
            //
            LoggingConfiguration config = LogManager.Configuration;
            if (config == null)
            {
                config = new LoggingConfiguration();
            }
            RichTextBoxTarget textBoxTarget = new RichTextBoxTarget();
            textBoxTarget.AutoScroll = true;
            textBoxTarget.ControlName = richTextBoxLog.Name;
            textBoxTarget.FormName = this.Name;
            textBoxTarget.MaxLines = 1024;
            textBoxTarget.Name = "TextBox";
            textBoxTarget.Layout = "${message} ${exception:format=tostring}";
            textBoxTarget.RowColoringRules.Add(new RichTextBoxRowColoringRule("level==LogLevel.Warn", "Empty", "Empty", FontStyle.Bold));
            textBoxTarget.RowColoringRules.Add(new RichTextBoxRowColoringRule("level==LogLevel.Error", "Red", "Empty", FontStyle.Bold));
            textBoxTarget.RowColoringRules.Add(new RichTextBoxRowColoringRule("level==LogLevel.Trace", "Gray", "Empty", FontStyle.Regular));
            config.AddTarget(textBoxTarget.Name, textBoxTarget);

            LoggingRule m_loggingRule = new LoggingRule("*", LogLevel.Info, textBoxTarget);
            //add before final rules from config
            config.LoggingRules.Insert(0, m_loggingRule);

            LogManager.Configuration = config;

            if (MainConfig.AutoClearLog == true)
            {
                autoClearToolStripMenuItem.Checked = true;
            }

            if (MainConfig.Verbose == true)
            {
                verboseToolStripMenuItem.Checked = true;
            }

            // download date time
            downloadDataTime = DateTime.Now.AddMinutes(MainConfig.IntervalTime);

            //auto close mule
            if (MainConfig.CloseEmuleIfAllIsDone == true)
            {
                EnableAutoCloseEmule();
            }
            else
            {
                DisableAutoCloseEmule();
            }

            labelFeedCategory.Text = "";
            labelFeedPauseDownload.Text = "";
            labelFeedUrl.Text = "";
            labelLastDownloadDate.Text = "";
            labelTotalFiles.Text = "";
            labelMaxSimultaneousDownloads.Text = "";

            UpdateRecentActivity();
            UpdateRssFeedGUI();
            UpdatePendingFiles();
#if !DEBUG
            autoStartEMuleToolStripMenuItem.Visible = false;
            autoCloseEMuleToolStripMenuItem.Visible = false;
#endif
            // attach textBox to the logger
        }

        private void forumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://sourceforge.net/tracker/?group_id=357576&atid=1492909");
        }

        private void globalOptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsDialog optDialog = new OptionsDialog(MainConfig);
            optDialog.ShowDialog();

            if (optDialog.DialogResult == DialogResult.OK)
            {
                MainConfig.ServiceType = optDialog.ServiceType;
                MainConfig.IntervalTime = optDialog.IntervalTime;
                MainConfig.StartMinimized = optDialog.StartMinimized;
                MainConfig.StartEmuleIfClose = optDialog.StartEmuleIfClose;
                MainConfig.CloseEmuleIfAllIsDone = optDialog.CloseEmuleIfAllIsDone;

                Config.StartWithWindows = optDialog.StartWithWindows;

                MainConfig.Verbose = optDialog.Verbose;
                MainConfig.ServiceUrl = optDialog.ServiceUrl;
                MainConfig.Password = optDialog.Password;
                MainConfig.DefaultCategory = optDialog.DefaultCategory;
                MainConfig.eMuleExe = optDialog.eMuleExe;
                MainConfig.IntervalTime = optDialog.IntervalTime;
                MainConfig.MinToStartEmule = optDialog.MinToStartEmule;
                MainConfig.EmailNotification = optDialog.EmailNotification;
                MainConfig.ServerSMTP = optDialog.ServerSMTP;
                MainConfig.MailSender = optDialog.MailSender;
                MainConfig.MailReceiver = optDialog.MailReceiver;
                MainConfig.AutoClearLog = optDialog.AutoClearLog;
                MainConfig.intervalBetweenUpgradeCheck = optDialog.intervalBetweenUpgradeCheck;
                MainConfig.MaxSimultaneousFeedDownloadsDefault = optDialog.MaxSimultaneousFeedDownloads;
                MainConfig.saveLog = optDialog.saveLog;

                // tvu save cookie
                MainConfig.tvuCookieH = optDialog.tvuCookieH;
                MainConfig.tvuCookieI = optDialog.tvuCookieI;
                MainConfig.tvuCookieT = optDialog.tvuCookieT;

                if (MainConfig.CloseEmuleIfAllIsDone == true)
                {
                    EnableAutoCloseEmule();
                }
                else
                {
                    DisableAutoCloseEmule();
                }

                if (MainConfig.StartEmuleIfClose == true)
                {
                    autoStartEMuleToolStripMenuItem.Checked = true;
                }
                else
                {
                    autoStartEMuleToolStripMenuItem.Checked = false;
                }

                MainConfig.Save();
            }

            optDialog.Dispose();
            return;
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://tvudownloader.sourceforge.net/");
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationShowHide();
        }

        private void importDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "XML (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ExportImportHelper.Import(MainConfig, openFileDialog1.FileName);
                UpdateRecentActivity();
                UpdateRssFeedGUI();
                UpdatePendingFiles();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://sourceforge.net/projects/tvudownloader/");
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLogin form = new FormLogin();
            form.ShowDialog();

            if (form.DialogResult != DialogResult.OK)
            {
                return;
            }

            MainConfig.tvuCookieT = form.cookieT;
            MainConfig.tvuCookieI = form.cookieI;
            MainConfig.tvuCookieH = form.cookieH;
            MainConfig.Save();
        }

        private void markAsDownloadedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count == 0)
                return;

            if (listViewFeedFilesList.Items.Count == 0)
                return;

            if (listViewFeedFilesList.SelectedItems.Count == 0)
                return;

            // get the feed
            DataGridViewColumn col = DataGridViewTextBoxColumnTitle;
            string titleCompact = dataGridViewMain.SelectedRows[0].Cells[col.Name].Value.ToString();
            RssSubscription feed = MainConfig.RssFeedList.Find(x => (x.TitleCompact == titleCompact));

            if (feed == null)
            {
                return;
            }

            foreach (ListViewItem selectedItem in listViewFeedFilesList.SelectedItems)
            {
                string strSelectItemText = selectedItem.Text;   // this contain name file
                logger.Info("Marked as Downloaded file:\"{0}\"", strSelectItemText);

                Ed2kfile file = feed.GetDownloadFile().Find(t => t.File.FileName == strSelectItemText).File;

                feed.SetFileDownloaded(file);
            }
            // finally update GUI
            UpdateSubscriptionFilesList();
        }

        private void menu_AutoCloseEmule(object sender, EventArgs e)
        {
            if (MainConfig.CloseEmuleIfAllIsDone == true)
            {
                this.menuItemAutoCloseEmule.Checked = false;
                this.MainConfig.CloseEmuleIfAllIsDone = false;
            }
            else
            {
                this.menuItemAutoCloseEmule.Checked = true;
                this.MainConfig.CloseEmuleIfAllIsDone = true;
                autoCloseDataTime = DateTime.Now.AddMinutes(30); // do control every 30 minutes
            }
        }

        private void menu_AutoStartEmule(object sender, EventArgs e)
        {
            logger.Info("Auto Start Emule");
            if (MainConfig.StartEmuleIfClose == true)
            {
                this.menuItemAutoStartEmule.Checked = false;
                this.MainConfig.StartEmuleIfClose = false;
            }
            else
            {
                this.menuItemAutoStartEmule.Checked = true;
                this.MainConfig.StartEmuleIfClose = true;
            }
        }

        private void Menu_CheckNow(object sender, EventArgs e)
        {
            StartDownloadThread();
        }

        private void Menu_Enable(object sender, EventArgs e)
        {
            if (MainConfig.Enebled == false)
            {
                this.menuItemEnable.Checked = false;
                MainConfig.Enebled = false;
                timerRssCheck.Enabled = false;
            }
            else
            {
                this.menuItemEnable.Checked = true;
                MainConfig.Enebled = true;
                timerRssCheck.Enabled = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon.

            // Set the WindowState to normal if the form is minimized.
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            // Activate the form.
            this.Activate();
        }

        private void notifyIcon1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Clicks == 2) & (e.Button == MouseButtons.Left))
            {
                if (this.Visible == true)
                {
                    mVisible = false;
                    this.Visible = false;
                }
                else
                {
                    mVisible = true;
                    this.Visible = true;
                }
            }
        }

        private void openLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(Config.FileNameLog) == true)
                {
                    Process.Start("notepad.exe", Config.FileNameLog);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Open Log File ");
            }
        }

        private void oPMLExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            // todo: replace string with stringbuilder
            string strOPML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine;
            strOPML += "<opml version=\"1.1\">" + Environment.NewLine;
            strOPML += "<head>" + Environment.NewLine;
            strOPML += "<title>Export</title>" + Environment.NewLine;
            strOPML += "<ownerName></ownerName>" + Environment.NewLine;
            string dateCreated = DateTime.Now.ToString("ddd, dd MMM yyyy hh:mm:ss") + " GMT";
            strOPML += "<dateCreated>" + dateCreated + "</dateCreated>" + Environment.NewLine;
            strOPML += "<dateModified>" + dateCreated + "</dateModified>" + Environment.NewLine;
            strOPML += "</head>";
            strOPML += "<body>" + Environment.NewLine;

            strOPML += "<outline text=\"tvunderground.org.ru\" title=\"tvunderground.org.ru\">" + Environment.NewLine;

            foreach (RssSubscription sub in MainConfig.RssFeedList)
            {
                strOPML += "<outline text=\"" + sub.Title + "\" title=\"" + sub.Title + "\" xmlUrl=\"" + sub.Url + "\" type=\"rss\"/>" + Environment.NewLine;
            }

            strOPML += "</outline>" + Environment.NewLine;
            strOPML += "</body>" + Environment.NewLine;
            strOPML += "</opml>" + Environment.NewLine;

            List<string> feed = new List<string>();

            dialog.Filter = "OPML (*.opml)|*.opml";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = false;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream myStream;

                    if ((myStream = dialog.OpenFile()) != null)
                    {
                        foreach (char c in strOPML)
                        {
                            myStream.WriteByte((byte)c);
                        }

                        myStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not write file. Original error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Show Option dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optionToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void reportABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://sourceforge.net/tracker/?group_id=357576&atid=1492909");
        }

        private void SetupNotify()
        {
            iconUp = TvUndergroundDownloader.Properties.Resources.appicon1;

            iconDown = TvUndergroundDownloader.Properties.Resources.appicon2;

            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItemCheckNow = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItemAutoStartEmule = new System.Windows.Forms.MenuItem();
            this.menuItemAutoCloseEmule = new System.Windows.Forms.MenuItem();
            this.menuItemEnable = new System.Windows.Forms.MenuItem();

            // Initialize menuItem1
            this.menuItemAutoStartEmule.Index = 0;
            this.menuItemAutoStartEmule.Text = "Auto Start eMule";

            if (MainConfig.StartEmuleIfClose == true)
            {
                this.menuItemAutoStartEmule.Checked = true;
            }
            else
            {
                this.menuItemAutoStartEmule.Checked = false;
            }

            this.menuItemAutoStartEmule.Click += new System.EventHandler(this.menu_AutoStartEmule);
            this.contextMenu1.MenuItems.Add(menuItemAutoStartEmule);

            this.menuItemAutoCloseEmule.Index = 1;
            this.menuItemAutoCloseEmule.Text = "Auto Close eMule";

            if (MainConfig.CloseEmuleIfAllIsDone == true)
            {
                EnableAutoCloseEmule();
            }
            else
            {
                DisableAutoCloseEmule();
            }

            if (MainConfig.StartEmuleIfClose == true)
            {
                autoStartEMuleToolStripMenuItem.Checked = true;
            }
            else
            {
                autoStartEMuleToolStripMenuItem.Checked = false;
            }

            this.menuItemAutoCloseEmule.Click += new System.EventHandler(this.menu_AutoCloseEmule);
            this.contextMenu1.MenuItems.Add(menuItemAutoCloseEmule);

            this.menuItemExit.Index = 5;
            this.menuItemExit.Text = "E&xit";
            this.menuItemExit.Click += new System.EventHandler(this.exitNotifyIconMenuItem_Click);
            this.contextMenu1.MenuItems.Add(menuItemExit);

            this.menuItemEnable.Index = 3;
            this.menuItemEnable.Text = "Enable";
            this.menuItemEnable.Checked = true;
            this.menuItemEnable.Click += new System.EventHandler(this.Menu_Enable);
            this.contextMenu1.MenuItems.Add(menuItemEnable);

            this.menuItemCheckNow.Index = 2;
            this.menuItemCheckNow.Text = "Check Now";
            this.menuItemCheckNow.Click += new System.EventHandler(this.Menu_CheckNow);
            this.contextMenu1.MenuItems.Add(menuItemCheckNow);

            // Create the NotifyIcon.
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            notifyIcon1.MouseDown += notifyIcon1_MouseDown; // add event

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            notifyIcon1.Icon = iconUp;

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon1.ContextMenu = this.contextMenu1;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon1.Text = "TV Underground Downloader";
            notifyIcon1.Visible = true;

            // Handle the DoubleClick event to activate the form.
            notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
        }

        private void showHideNotifyIconMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationShowHide();
        }

        private void StartDownloadThread()
        {
            if (backgroundWorker1.IsBusy == true)
            {
                logger.Info("Thread is busy");
                return;
            }

            if (autoClearToolStripMenuItem.Checked == true)
            {
                richTextBoxLog.Clear();
            }

            checkNowToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;
            addToolStripMenuItem.Enabled = false;
            toolStripMenuItemAdd.Enabled = false;
            toolStripMenuItemDelete.Enabled = false;
            toolStripMenuItemEdit.Enabled = false;

            toolStripButtonCheckNow.Enabled = false;
            toolStripButtonAddFeed.Enabled = false;

            menuItemCheckNow.Enabled = false;

            cancelCheckToolStripMenuItem.Enabled = true;
            toolStripButtonStop.Enabled = true;

            listBoxPending.Items.Clear();
            listBoxPending.Refresh();
            backgroundWorker1.RunWorkerAsync();
        }

        private void testAutoCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // remove 1h from AutoClose Data Time to force function to work
            TimeSpan delta = new TimeSpan(1, 0, 0);
            autoCloseDataTime = DateTime.Now.Subtract(delta);
            autoclose();
        }

        private void testAutoStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(MainConfig.eMuleExe);
            }
            catch
            {
                logger.Info("Unable start application");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now > downloadDataTime)
            {
                if (MainConfig.AutoClearLog == true)
                {
                    richTextBoxLog.Clear();
                }

                downloadDataTime = DateTime.Now.AddMinutes(MainConfig.IntervalTime);

                logger.Info("Now : {0}", DateTime.Now.ToString());
                logger.Info("next tick : " + downloadDataTime.ToString());
                StartDownloadThread();
                UpdateRssFeedGUI();
            }

            if (CheckNewVersion() == true)
            {
                MessageBox.Show("New Version is available at http://tvudownloader.sourceforge.net/");
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            logger.Info("Configuration loaded " + Config.FileNameConfig);
            timer2.Enabled = false;

            if (MainConfig.StartMinimized == true)
            {
                mVisible = false;
                this.Visible = false;
            }
            StartDownloadThread();

            if (CheckNewVersion() == true)
            {
                MessageBox.Show("New version is available at http://tvudownloader.sourceforge.net/");
            }
        }

        private void timerAutoClose_Tick(object sender, EventArgs e)
        {
            autoclose();
        }

        private void toolStripButtonAddFeed_Click(object sender, EventArgs e)
        {
            AddRssChannel();
        }

        private void toolStripButtonCheckNow_Click(object sender, EventArgs e)
        {
            StartDownloadThread();
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            AddRssChannel();
        }

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            DeleteRssChannel();
        }

        private void UpdatePendingFiles()
        {
            listBoxPending.Items.Clear();

            foreach (var subscrission in MainConfig.RssFeedList)
            {
                foreach (Ed2kfile file in subscrission.GetPendingFiles())
                {
                    listBoxPending.Items.Add(file.FileName);
                }
            }
        }

        private void UpdateRecentActivity()
        {
            DataTable table = new DataTable();
            table.Columns.Add("FileName");
            table.Columns.Add("LastUpdate");

            foreach (DownloadFile file in MainConfig.RssFeedList.GetLastActivity())
            {
                if (file.DownloadDate.HasValue == false)
                {
                    continue;
                }

                var newRow = table.NewRow();

                newRow["FileName"] = file.File.FileName;
                newRow["LastUpdate"] = file.DownloadDate;
                table.Rows.Add(newRow);
            }

            dataGridViewRecentActivity.DataSource = table;
        }

        private void UpdateRssFeedGUI()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Title", typeof(String));
            dataTable.Columns.Add("TotalDownloads", typeof(Int32));
            dataTable.Columns.Add("LastUpgrade", typeof(Int32));
            dataTable.Columns.Add("Status", typeof(String));
            dataTable.Columns.Add("Enabled", typeof(Boolean));
            dataTable.Columns.Add("DubLanguage", typeof(Image));

            foreach (RssSubscription subscrission in MainConfig.RssFeedList)
            {
                var newRow = dataTable.NewRow();
                newRow["Title"] = subscrission.TitleCompact;
                newRow["TotalDownloads"] = subscrission.TotalFilesDownloaded;

                newRow["LastUpgrade"] = subscrission.LastChannelUpdate.Days;

                switch (subscrission.CurrentTVUStatus)
                {
                    case TvuStatus.Complete:
                        newRow["Status"] = "Complete";
                        break;

                    case TvuStatus.StillIncomplete:
                        newRow["Status"] = "Incomplete";
                        break;

                    case TvuStatus.StillRunning:
                        newRow["Status"] = "Running";
                        break;

                    case TvuStatus.OnHiatus:
                        newRow["Status"] = "On Hiatus";
                        break;

                    case TvuStatus.Unknown:
                    default:
                        newRow["Status"] = "Unknown";
                        break;
                }

                newRow["Enabled"] = subscrission.Enabled == true ? "True" : "False";

                switch (subscrission.DubLanguage)
                {
                    case "gb":
                        newRow["DubLanguage"] = new Bitmap(Properties.Resources.gb);
                        break;

                    case "it":
                        newRow["DubLanguage"] = new Bitmap(Properties.Resources.it);
                        break;

                    case "fr":
                        newRow["DubLanguage"] = new Bitmap(Properties.Resources.fr);
                        break;

                    case "jp":
                        newRow["DubLanguage"] = new Bitmap(Properties.Resources.jp);
                        break;

                    case "es":
                        newRow["DubLanguage"] = new Bitmap(Properties.Resources.es);
                        break;

                    default:
                        break;
                }

                dataTable.Rows.Add(newRow);
            }
            dataGridViewMain.DataSource = dataTable;
        }

        private void UpdateSubscriptionFilesList()
        {
            if (dataGridViewMain.SelectedRows.Count == 0)
            {
                return;
            }

            DataGridViewRow selectedItem = dataGridViewMain.SelectedRows[0];
            if (selectedItem == null)
            {
                return;
            }

            DataGridViewColumn col = DataGridViewTextBoxColumnTitle;
            string titleCompact = selectedItem.Cells[col.Name].Value.ToString();
            RssSubscription feed = MainConfig.RssFeedList.Find(x => (x.TitleCompact == titleCompact));

            if (feed == null)
            {
                return;
            }

            // update list history
            listViewFeedFilesList.Items.Clear();

            // extract file by feedLink
            List<DownloadFile> ldf = feed.GetDownloadFile();
            var listFile = feed.GetDownloadFile();
            listFile.Sort((a, b) => b.File.FileName.CompareTo(a.File.FileName));
            foreach (DownloadFile file in listFile)
            {
                ListViewItem item = new ListViewItem(file.File.GetFileName());

                if (file.PublicationDate.HasValue == true)
                {
                    item.SubItems.Add(file.PublicationDate.Value.ToString("d", CultureInfo.InvariantCulture));
                }
                else
                {
                    item.SubItems.Add(string.Empty);
                }

                if (file.DownloadDate.HasValue == true)
                {
                    if (file.DownloadDate.Value == DateTime.MinValue)
                    {
                        item.SubItems.Add("Skipped");
                    }
                    else
                    {
                        string date = file.DownloadDate.Value.ToString("o", CultureInfo.InvariantCulture);
                        item.SubItems.Add(date);
                    }
                }
                else
                {
                    item.SubItems.Add(string.Empty);
                }

                listViewFeedFilesList.Items.Add(item);
            }
        }

        private void verboseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainConfig.Verbose == true)
            {
                verboseToolStripMenuItem.Checked = false;
                MainConfig.Verbose = false;
            }
            else
            {
                verboseToolStripMenuItem.Checked = true;
                MainConfig.Verbose = true;
            }

            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                if (MainConfig.Verbose == true)
                    rule.EnableLoggingForLevel(LogLevel.Trace);
                else
                    rule.EnableLoggingForLevel(LogLevel.Info);
            }

            //Call to update existing Loggers created with GetLogger() or GetCurrentClassLogger()
            LogManager.ReconfigExistingLoggers();
        }

        private void versionCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckNewVersion(true) == true)
            {
                MessageBox.Show("New Version is available at http://tvudownloader.sourceforge.net/");
            }
            else
            {
                MessageBox.Show("Software is already update");
            }
        }

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
    }
}