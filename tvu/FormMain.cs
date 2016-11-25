﻿using NLog;
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
        private bool mVisible = true;
        private bool mAllowClose = false;

        private Icon IconUp;
        private Icon IconDown;

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItemCheckNow;
        private System.Windows.Forms.MenuItem menuItemExit;
        private System.Windows.Forms.MenuItem menuItemAutoStartEmule;
        private System.Windows.Forms.MenuItem menuItemAutoCloseEmule;
        private System.Windows.Forms.MenuItem menuItemEnable;

        private ListViewColumnSorter lvwColumnSorter;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Config MainConfig;
        public History MainHistory;

        private DateTime DownloadDataTime;
        private DateTime AutoCloseDataTime;

        public class sDonwloadFile
        {
            public RssSubscription subscription { get; private set; } = null;

            public string FeedSource { get; private set; }
            public string FeedLink { get; private set; }
            public string Ed2kLink { get; private set; }

            public bool PauseDownload { get; private set; }
            public string Category { get; private set; }

            public sDonwloadFile(string Ed2kLink, string FeedLink, string FeedSource, bool PauseDownload, string Category)
            {
                this.Ed2kLink = Ed2kLink;
                this.FeedLink = FeedLink;
                this.FeedSource = FeedSource;
                this.PauseDownload = PauseDownload;
                this.Category = Category;
            }

            public sDonwloadFile(string Ed2kLink, RssSubscription subscription)
            {
                this.Ed2kLink = Ed2kLink;
                this.FeedLink = subscription.Url;
                this.FeedSource = string.Empty;
                this.PauseDownload = subscription.PauseDownload;
                this.Category = Category;
                this.subscription = subscription;
            }

        };

        public FormMain()
        {
            // load configuration
            MainConfig = new Config();
            MainConfig.Load();

            // load History
            MainHistory = new History();

            InitializeComponent();
            SetupNotify();

        }

        private void Form1_Load(object sender, EventArgs e)
        {


            //
            //  Setup Nlog
            //
            LoggingConfiguration config = LogManager.Configuration;
            if(config == null)
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
            DownloadDataTime = DateTime.Now.AddMinutes(MainConfig.IntervalTime);

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

            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            lvwColumnSorter = new ListViewColumnSorter();
            this.listViewFeed.ListViewItemSorter = lvwColumnSorter;

            UpdateRecentActivity();
            UpdateRssFeedGUI();

            // attach textBox to the logger
        }


        private void SetupNotify()
        {

            IconUp = TvUndergroundDownloader.Properties.Resources.appicon1;

            IconDown = TvUndergroundDownloader.Properties.Resources.appicon2;

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
            this.menuItemEnable.Click += new System.EventHandler(this.menu_Enable);
            this.contextMenu1.MenuItems.Add(menuItemEnable);

            this.menuItemCheckNow.Index = 2;
            this.menuItemCheckNow.Text = "Check Now";
            this.menuItemCheckNow.Click += new System.EventHandler(this.menu_CheckNow);
            this.contextMenu1.MenuItems.Add(menuItemCheckNow);

            // Create the NotifyIcon.
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            notifyIcon1.MouseDown += notifyIcon1_MouseDown; // add event

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            notifyIcon1.Icon = IconUp;

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


        private void menu_CheckNow(object Sender, EventArgs e)
        {
            StartDownloadThread();
        }

        private void menu_AutoStartEmule(object Sender, EventArgs e)
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

        private void menu_AutoCloseEmule(object Sender, EventArgs e)
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
                AutoCloseDataTime = DateTime.Now.AddMinutes(30); // do control every 30 minutes
            }
        }


        private void menu_Enable(object Sender, EventArgs e)
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

        private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon.

            // Set the WindowState to normal if the form is minimized.
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            // Activate the form.
            this.Activate();
        }

        private void UpdateRssFeedGUI()
        {
            foreach (RssSubscription subscrission in MainConfig.RssFeedList)
            {
                if (subscrission.listViewItem == null)
                {
                    string title = subscrission.Title.Replace("[ed2k] tvunderground.org.ru:", "");
                    subscrission.listViewItem = new ListViewItem(title);
                    subscrission.listViewItem.SubItems.Add("Total downloads");
                    subscrission.listViewItem.SubItems.Add("Last upgrade");
                    subscrission.listViewItem.SubItems.Add("Status");
                    subscrission.listViewItem.SubItems.Add("Enabled");
                    listViewFeed.Items.Add(subscrission.listViewItem);
                }

                ListViewItem item = subscrission.listViewItem;


                // total downloads      column 1
                item.SubItems[1].Text = MainHistory.GetDownloadedFileCountByFeedSoruce(subscrission.Url).ToString();

                // last upgrade         column 2
                uint days = 0;

                string LastDownloadDate = MainHistory.LastDownloadDateByFeedSource(subscrission.Url);
                if (LastDownloadDate != string.Empty)
                {
                    DateTime LastDownloadTime = Convert.ToDateTime(LastDownloadDate);

                    if (LastDownloadTime > DateTime.MinValue)
                    {
                        TimeSpan diff = DateTime.Now.Subtract(LastDownloadTime);
                        days = (uint)diff.TotalDays;

                        item.SubItems[2].Text = days.ToString() + " days";
                    }
                    else
                    {
                        item.SubItems[2].Text = string.Empty;
                    }
                }
                else
                {
                    item.SubItems[2].Text = string.Empty;
                }

                // serie status      column 3
                switch (subscrission.tvuStatus)
                {
                    default:
                        item.SubItems[3].Text = "Unknown";
                        break;
                    case tvuStatus.Complete:
                        item.SubItems[3].Text = "Complete";
                        break;
                    case tvuStatus.StillRunning:
                        item.SubItems[3].Text = "Still Running";
                        break;
                    case tvuStatus.StillIncomplete:
                        item.SubItems[3].Text = "Still Incomplete";
                        break;
                    case tvuStatus.OnHiatus:
                        item.SubItems[3].Text = "On Hiatus";
                        break;
                }

                // TV status
                if (subscrission.Enabled == true)
                {
                    item.SubItems[4].Text = "Enabled";
                }
                else
                {
                    item.SubItems[4].Text = "Disabled";
                }

            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (DateTime.Now > DownloadDataTime)
            {
                if (MainConfig.AutoClearLog == true)
                {
                    richTextBoxLog.Clear();
                }

                DownloadDataTime = DateTime.Now.AddMinutes(MainConfig.IntervalTime);

                logger.Info("Now : {0}", DateTime.Now.ToString());
                logger.Info("next tick : " + DownloadDataTime.ToString());
                StartDownloadThread();
                UpdateRssFeedGUI();

            }

            if (CheckNewVersion() == true)
            {
                MessageBox.Show("New Version is available at http://tvudownloader.sourceforge.net/");
            }



        }

        private void versionCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainConfig.LastUpgradeCheck = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");

            if (CheckNewVersion() == true)
            {
                MessageBox.Show("New Version is available at http://tvudownloader.sourceforge.net/");
            }
            else
            {
                MessageBox.Show("Software is already update");
            }
        }

        private void listViewFeed_SelectedIndexChanged(object sender, EventArgs e)
        {

            if ((listViewFeed.Items.Count == 0) ^ (listViewFeed.SelectedItems.Count == 0) ^ (MainConfig.RssFeedList.Count == 0))
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


            var Feed = MainConfig.RssFeedList.Find(x => (x.listViewItem == listViewFeed.SelectedItems[0]));

            if (Feed == null)
            {
                return;
            }

            labelFeedCategory.Text = Feed.Category;
            labelFeedPauseDownload.Text = Feed.PauseDownload.ToString();
            labelFeedUrl.Text = Feed.Url;

            string temp = MainHistory.LastDownloadDateByFeedSource(Feed.Url);
            if (temp.IndexOf("0001-01-01") > -1)
            {
                labelLastDownloadDate.Text = "-";
            }
            else
            {
                labelLastDownloadDate.Text = temp.Replace('T', ' ');
            }
            labelTotalFiles.Text = MainHistory.GetDownloadedFileCountByFeedSoruce(Feed.Url).ToString();
            labelMaxSimultaneousDownloads.Text = Feed.MaxSimultaneousDownload.ToString();

            // update list history
            listViewFeedFilesList.Items.Clear();


            // extract file by feedLink

            foreach (DataRow row in MainHistory.GetDownloadedFileByFeedSoruce(Feed.Url).Rows)
            {
                ListViewItem item = new ListViewItem(row[0].ToString());
                string date = row[1].ToString().Substring(0, 10);
                if (date == "0001-01-01")
                {
                    item.SubItems.Add("Skipped");
                }
                else
                {
                    item.SubItems.Add(date);
                }
                listViewFeedFilesList.Items.Add(item);
            }
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
            deleteCompleteToolStripMenuItem.Enabled = false;
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

        public static string GetUserAppDataPath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path += '\\' + Application.ProductName + '\\';
            return path;
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

            List<sDonwloadFile> DownloadFileList = new List<sDonwloadFile>();

            FeedLinkCache feedLinkCache = new FeedLinkCache();

            // select only enabled RSS
            List<RssSubscription> RssFeedList = MainConfig.RssFeedList.FindAll(delegate (RssSubscription rss) { return rss.Enabled == true; });

            foreach (RssSubscription feed in RssFeedList)
            {
                try
                {
                    logger.Info("Read RSS {0}", feed.TitleCompact);
                    feed.Update(cookieContainer);
                    foreach (Ed2kfile file in feed.GetNewDownload())
                    {
                        sDonwloadFile sfile = new sDonwloadFile(file.Ed2kLink, feed);
                        DownloadFileList.Add(sfile);
                    }

                    //
                    //  this code allow to block anytime the loop
                    //
                    if (backgroundWorker1.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }


                    //try
                    //{
                    //    string webPageUrl = feed.Url;
                    //    if (MainConfig.useHttpInsteadOfHttps == true)
                    //    {
                    //        webPageUrl = webPageUrl.Replace("https", "http");
                    //    }

                    //    string WebPage = WebFetch.Fetch(feed.Url, true, cookieContainer);

                    //    List<string> elemList = new List<string>();

                    //    if (WebPage != null)
                    //    {
                    //        //get GUID page

                    //        //static Regex "https?://(www\.)?tvunderground.org.ru/index.php\?show=ed2k&season=\d{1,10}&sid\[\d{1,10}\]=\d{1,10}"
                    //        MatchCollection matchCollection = FileHistory.regexFeedLink.Matches(WebPage);


                    //        foreach (Match value in matchCollection)
                    //        {
                    //            if (backgroundWorker1.CancellationPending)
                    //            {
                    //                e.Cancel = true;
                    //                return;
                    //            }

                    //            string FeedLink = value.ToString();
                    //            if (MainHistory.FileExistByFeedLink(FeedLink) == false)
                    //            {
                    //                elemList.Add(FeedLink);
                    //                feed.tvuStatus = tvuStatus.Unknown; // force refrash of tv Undergoud status when find a new file
                    //            }
                    //        }

                    //        //
                    //        //  force status check if more than 15 days ago
                    //        TimeSpan ts = DateTime.Now - feed.LastSerieStatusUpgradeDate;
                    //        if ((ts.TotalDays > 15) | (feed.tvuStatus == tvuStatus.Unknown))
                    //        {
                    //            logger.Info("Checking serie status");
                    //            Regex Pattern = new Regex(@"http(s)?://(www\.)?((tvunderground)|(tvu)).org.ru/index.php\?show=episodes&sid=\d{1,10}");
                    //            Match Match = Pattern.Match(WebPage);
                    //            string url = Match.Value;

                    //            feed.tvuStatus = WebManagerTVU.CheckComplete(url, cookieContainer);
                    //            feed.LastSerieStatusUpgradeDate = DateTime.Now;

                    //            switch (feed.tvuStatus)
                    //            {
                    //                case tvuStatus.Complete:
                    //                    logger.Info("Serie status: Complete");
                    //                    break;
                    //                default:
                    //                    logger.Info("Serie status: Unknown");
                    //                    break;
                    //                case tvuStatus.StillRunning:
                    //                    logger.Info("Serie status: Still Running");
                    //                    break;
                    //                case tvuStatus.StillIncomplete:
                    //                    logger.Info("Serie status: Still Incomplete");
                    //                    break;
                    //                case tvuStatus.OnHiatus:
                    //                    logger.Info("Serie status: On Hiatus");
                    //                    break;
                    //            }
                    //        }
                    //    }

                    //    // reverse the list so the last feed ( first temporal feed) became the first feed in list
                    //    elemList.Reverse();

                    //    // download 
                    //    foreach (string FeedLink in elemList)
                    //    {
                    //        string sEd2k = string.Empty;

                    //        if (backgroundWorker1.CancellationPending)
                    //        {
                    //            e.Cancel = true;
                    //            return;
                    //        }

                    //        // check is the feed is already present in feed link cache
                    //        sEd2k = feedLinkCache.FindFeedLink(FeedLink);
                    //        if (sEd2k == string.Empty)
                    //        {
                    //            string page = null;

                    //            // download the page in FeedLink
                    //            logger.Info(string.Format("Download page {0}", FeedLink));

                    //            if ((page = WebFetch.Fetch(FeedLink, true, cookieContainer)) == null)
                    //            {
                    //                logger.Info(string.Format("Unable to download"));
                    //                continue;
                    //            }
                    //            // parse ed2k
                    //            sEd2k = RssParserTVU.FindEd2kLink(page);
                    //            FeedLinkCache.AddFeedLink(FeedLink, sEd2k, DateTime.Now.ToString("s"));
                    //        }

                    //        Ed2kfile parser = new Ed2kfile(sEd2k);
                    //        logger.Info(string.Format("Found new file {0}", parser.GetFileName()));

                    //        if (elemList.IndexOf(FeedLink) < feed.MaxSimultaneousDownload)
                    //        {
                    //            // add file to Download list 
                    //            sDonwloadFile DL = new sDonwloadFile(sEd2k, FeedLink, feed.Url, feed.PauseDownload, feed.Category);
                    //            DownloadFileList.Add(DL);
                    //        }
                    //        else
                    //        {
                    //            logger.Info(string.Format("Found new file end skipped {0}", parser.GetFileName()));
                    //            //
                    //            //  Use invoke to avoid thread issue
                    //            //
                    //            if (this.listBoxPending.InvokeRequired == true)
                    //            {

                    //                Invoke(new MethodInvoker(
                    //                    delegate { this.AddItemToListBoxPending(parser.GetFileName()); }
                    //                ));

                    //            }
                    //            else
                    //            {
                    //                this.AddItemToListBoxPending(parser.GetFileName());
                    //            }
                    //        }
                    //    }



                }
                catch (Exception ex)
                {

                    logger.Info("Some errors in RSS parsing (check login)");
                }

                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                //update RSS feed
                feed.LastUpgradeDate = MainHistory.LastDownloadDateByFeedSource(feed.Url);
                int progress = (MainConfig.RssFeedList.IndexOf(feed) + 1) * 100;
                progress = progress / MainConfig.RssFeedList.Count;
                backgroundWorker1.ReportProgress(progress);


            }

            FeedLinkCache.CleanUp();
            MainConfig.Save();

            if (DownloadFileList.Count == 0)
            {
                logger.Info("Nothing to download");
                return;
            }
            else
            {
                logger.Info("Total file found " + DownloadFileList.Count);
            }

            IMuleWebManager Service = null;


            switch (MainConfig.ServiceType)
            {
                case Config.eServiceType.aMule:
                    logger.Info("Load aMule service");
                    Service = new aMuleWebManager(MainConfig.ServiceUrl, MainConfig.Password);
                    break;
                case Config.eServiceType.eMule:
                default:
                    logger.Info("Load eMule service");
                    Service = new eMuleWebManager(MainConfig.ServiceUrl, MainConfig.Password);
                    break;
            }

            //
            //  if emule is close and new file < min to start not do null
            //
            //
            // try to start emule
            // the if work only if rc == null ad 
            if ((MainConfig.StartEmuleIfClose != true) & (DownloadFileList.Count <= MainConfig.MinToStartEmule))
            {
                logger.Info("Emule off line");
                logger.Info("Min files to download not reached");
                return;
            }

            for (int cont = 1; (cont <= 5) & (Service.Connect() != LoginStatus.Logged); cont++)
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
            if (Service.isConnected == false)
            {
                logger.Info("Unable to connect to eMule web server");
                return;
            }

            logger.Info("Retrieve list category");
            Service.GetCategories(true);  // force upgrade category list 

            logger.Info("Clean download list (step 1) find channel from ed2k");

            List<Ed2kfile> CourrentDownloadsFormEmule = Service.GetCurrentDownloads(MainHistory.GetKnownFiles());/// file downloaded with this program and now in download in emule
            if (CourrentDownloadsFormEmule == null)
            {
                logger.Info("eMule web server not respond");
                return;
            }

            logger.Info("Current Download Form Emule " + CourrentDownloadsFormEmule.Count);

            //for debug
            CourrentDownloadsFormEmule.ForEach(delegate (Ed2kfile file) { logger.Info(file.FileName.Replace("%20", " ")); });

            //
            //  note move this function in MainHistory class
            //
            logger.Info("Start search in history");
            List<FileHistory> ActualDownloadFileList = MainHistory.getFileHistoryFromDB(CourrentDownloadsFormEmule);
            ActualDownloadFileList.ForEach(delegate (FileHistory file) { logger.Info("Found :\"" + file.FileName.Replace("%20", " ") + "\""); });

            logger.Info("ActualDownloadFileList.Count = " + ActualDownloadFileList.Count);
            logger.Info("MainConfig.MaxSimultaneousFeedDownloads = " + MainConfig.MaxSimultaneousFeedDownloads);

            // create a dictionary to count 
            Dictionary<string, uint> MaxSimultaneousDownloadsDictionary = new Dictionary<string, uint>();
            // set starting point for each feed
            foreach (RssSubscription RssFeed in MainConfig.RssFeedList)
            {
                MaxSimultaneousDownloadsDictionary.Add(RssFeed.Url, RssFeed.MaxSimultaneousDownload);
            }
            // 
            //  remove all file already in download
            //
            foreach (FileHistory fh in ActualDownloadFileList)
            {
                if (MaxSimultaneousDownloadsDictionary.ContainsKey(fh.FeedSource) == true)
                {
                    uint x = MaxSimultaneousDownloadsDictionary[fh.FeedSource];
                    x = Math.Max(0, x - 1);
                    MaxSimultaneousDownloadsDictionary[fh.FeedSource] = x;
                }
            }



            logger.Info("Download file");
            //
            //  Download file 
            // 
            foreach (sDonwloadFile DownloadFile in DownloadFileList)
            {
                //  this code allow to block anytime the loop
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                uint MSDD = MaxSimultaneousDownloadsDictionary[DownloadFile.FeedSource];
                if (MSDD == 0)
                {
                    string fileName = new Ed2kfile(DownloadFile.Ed2kLink).FileName;
                    logger.Info(string.Format("File skipped {0}", fileName));
                    //
                    //  Use invoke to avoid thread issue
                    //
                    if (this.listBoxPending.InvokeRequired == true)
                    {

                        Invoke(new MethodInvoker(
                            delegate { this.AddItemToListBoxPending(fileName); }
                        ));

                    }
                    else
                    {
                        this.AddItemToListBoxPending(fileName);
                    }

                    continue;
                }

                MaxSimultaneousDownloadsDictionary[DownloadFile.FeedSource] = Math.Max(0, MSDD - 1);

                Ed2kfile ed2klink = new Ed2kfile(DownloadFile.Ed2kLink);
                logger.Info("Add file to download");
                Service.AddToDownload(ed2klink, DownloadFile.Category);

                if (DownloadFile.PauseDownload == true)
                {
                    logger.Info("Pause download");
                    Service.StopDownload(ed2klink);
                }
                else
                {
                    logger.Info("Resume download");
                    Service.StartDownload(ed2klink);
                }
                History.Add(DownloadFile.Ed2kLink, DownloadFile.FeedLink, DownloadFile.FeedSource, DateTime.Now.ToString("s"));
                Ed2kfile parser = new Ed2kfile(DownloadFile.Ed2kLink);
                logger.Info("Add file to emule {0}", parser.GetFileName());
                SendMailDownload(parser.GetFileName(), DownloadFile.Ed2kLink);
                MainConfig.TotalDownloads++;   //increase Total Downloads for statistic

                { // progress bar
                    int progress = (DownloadFileList.IndexOf(DownloadFile) + 1) * 100;
                    backgroundWorker1.ReportProgress(progress / DownloadFileList.Count);
                }
            }


            logger.Info("Force Refresh Shared File List");
            Service.ForceRefreshSharedFileList();

            logger.Info("Logout Emule");
            Service.Close();

            logger.Info("Statistics");
            logger.Info(string.Format("Total file added {0}", MainConfig.TotalDownloads));
            MainHistory.Save();


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
            UpdateRssFeedGUI();

            menuItemCheckNow.Enabled = true;

            deleteToolStripMenuItem.Enabled = true;
            addToolStripMenuItem.Enabled = true;

            checkNowToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
            addToolStripMenuItem.Enabled = true;
            deleteCompleteToolStripMenuItem.Enabled = true;
            toolStripMenuItemAdd.Enabled = true;
            toolStripMenuItemDelete.Enabled = true;
            toolStripMenuItemEdit.Enabled = true;

            toolStripButtonCheckNow.Enabled = true;
            toolStripButtonAddFeed.Enabled = true;

            toolStripButtonStop.Enabled = false;
            cancelCheckToolStripMenuItem.Enabled = false;
        }



        protected override void SetVisibleCore(bool value)
        {
            // MSDN http://social.msdn.microsoft.com/forums/en-US/csharpgeneral/thread/eab563c3-37d0-4ebd-a086-b9ea7bb03fed
            if (!mVisible)
                value = false;         // Prevent form getting visible
            base.SetVisibleCore(value);
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            logger.Info("FormClosing Event");
            logger.Debug("{0} = {1}", "CloseReason", e.CloseReason);
            logger.Debug("{0} = {1}", "Cancel", e.Cancel);
            logger.Debug("{0} = {1}", "mAllowClose", mAllowClose);

            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                mAllowClose = true;
            }

            // MSDN http://social.msdn.microsoft.com/forums/en-US/csharpgeneral/thread/eab563c3-37d0-4ebd-a086-b9ea7bb03fed
            if (!mAllowClose)
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

        void notifyIcon1_MouseDown(object sender, MouseEventArgs e)
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

        /// <summary>check if a new version is avable on web</summary>
        /// <returns>true if new version is available or false in other case</returns>
        private bool CheckNewVersion()
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

                if (DateTime.Now < nextCheck)
                {
                    return false;
                }

                MainConfig.LastUpgradeCheck = DateTime.Now.ToString("yyyy-MM-dd");

                OperatingSystem osv = Environment.OSVersion;

                // require update status
                XmlDocument doc = new XmlDocument();
                doc.Load(string.Format("http://tvudownloader.sourceforge.net/version.php?ver={0}&tvuid={1}&TotalDownloads={2}&osv={3}",
                    Config.VersionFull.Replace(" ", "%20"),
                    MainConfig.tvudwid,
                    MainConfig.TotalDownloads,
                    osv.VersionString.Replace(" ", "%20")));

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




        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://sourceforge.net/projects/tvudownloader/");
        }


        public void UpdateRecentActivity()
        {
            DataTable list = MainHistory.GetRecentActivity();

            foreach (DataRow row in list.Rows)
            {
                string temp = (string)row["LastUpdate"];

                if (temp.IndexOf("0001-01-01") > -1)
                {
                    row.Delete();
                }
                else
                {
                    row["LastUpdate"] = temp.Replace('T', ' ');
                }

            }

            dataGridViewRecentActivity.DataSource = list;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationExit();
        }

        private void exitNotifyIconMenuItem_Click(object Sender, EventArgs e)
        {

            ApplicationExit();
        }

        /// <summary>
        /// Close the application
        /// </summary>
        private void ApplicationExit()
        {
            mVisible = true;
            this.Visible = true;
            mAllowClose = true;
            this.Close();
        }
        /// <summary>
        /// Show Option dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optionToolStripMenuItem_Click(object sender, EventArgs e)
        {

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


        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationShowHide();
        }

        private void showHideNotifyIconMenuItem_Click(object Sender, EventArgs e)
        {
            ApplicationShowHide();
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

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRssChannel();

        }

        private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            AddRssChannel();
        }



        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteRssChannel();

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Clear();
        }

        private void checkBoxAutoClear_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void globalOptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsDialog OptDialog = new OptionsDialog(MainConfig);
            OptDialog.ShowDialog();


            if (OptDialog.DialogResult == DialogResult.OK)
            {
                MainConfig.ServiceType = OptDialog.ServiceType;
                MainConfig.IntervalTime = OptDialog.IntervalTime;
                MainConfig.StartMinimized = OptDialog.StartMinimized;
                MainConfig.StartEmuleIfClose = OptDialog.StartEmuleIfClose;
                MainConfig.CloseEmuleIfAllIsDone = OptDialog.CloseEmuleIfAllIsDone;

                Config.StartWithWindows = OptDialog.StartWithWindows;

                MainConfig.Verbose = OptDialog.Verbose;
                MainConfig.ServiceUrl = OptDialog.ServiceUrl;
                MainConfig.Password = OptDialog.Password;
                MainConfig.DefaultCategory = OptDialog.DefaultCategory;
                MainConfig.eMuleExe = OptDialog.eMuleExe;
                MainConfig.IntervalTime = OptDialog.IntervalTime;
                MainConfig.MinToStartEmule = OptDialog.MinToStartEmule;
                MainConfig.EmailNotification = OptDialog.EmailNotification;
                MainConfig.ServerSMTP = OptDialog.ServerSMTP;
                MainConfig.MailSender = OptDialog.MailSender;
                MainConfig.MailReceiver = OptDialog.MailReceiver;
                MainConfig.AutoClearLog = OptDialog.AutoClearLog;
                MainConfig.intervalBetweenUpgradeCheck = OptDialog.intervalBetweenUpgradeCheck;
                MainConfig.MaxSimultaneousFeedDownloads = OptDialog.MaxSimultaneousFeedDownloads;
                MainConfig.saveLog = OptDialog.saveLog;





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


            OptDialog.Dispose();
            return;
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

        private void timerAutoClose_Tick(object sender, EventArgs e)
        {
            autoclose();
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
            if (AutoCloseDataTime == DateTime.MinValue)
            {
                logger.Info("[AutoClose Mule] AutoCloseDataTime = DateTime.Now.AddMinutes(30);");
                AutoCloseDataTime = DateTime.Now.AddMinutes(30);
            }

            //
            // Auto close
            //
            if (DateTime.Now < AutoCloseDataTime)
            {
                return;
            }

            // suspend event while connect with mule
            timerAutoClose.Enabled = false;

            // connect to mule

            logger.Info("[AutoClose Mule] Check Login");
            eMuleWebManager Service = new eMuleWebManager(MainConfig.ServiceUrl, MainConfig.Password);
            LoginStatus returnCode = Service.Connect();


            // if mule close ... end of game
            if (returnCode != LoginStatus.Logged)
            {
                AutoCloseDataTime = DateTime.Now.AddMinutes(30); // do control every 30 minutes
                logger.Info("[AutoClose Mule] Login failed");
                return;
            }
            logger.Info("[AutoClose Mule] Login ok");

            logger.Info("[AutoClose Mule] Actual Downloads " + Service.GetCurrentDownloads(null).Count);
            // if donwload > 0 ... there' s some download ... end 
            if (Service.GetCurrentDownloads(null).Count > 0)
            {
                logger.Info("[AutoClose Mule] GetActualDownloads return >0");
                AutoCloseDataTime = DateTime.Now.AddMinutes(30);
                logger.Info("[AutoClose Mule] LogOut");
                Service.Close();
                return;
            }

            logger.Info("[AutoClose Mule] Show dialog ");
            // pop up form to advise user
            FormAlerteMuleClose Dialog = new FormAlerteMuleClose();
            Dialog.ShowDialog();

            logger.Info("[AutoClose Mule] Dialog return " + Dialog.AlertChoice.ToString());
            switch (Dialog.AlertChoice)
            {
                case AlertChoiceEnum.Close:// Close
                    logger.Info("[AutoClose Mule: CLOSE] Close Service");
                    Dialog.Dispose();
                    Service.CloseEmuleApp();
                    Service.Close();
                    timerAutoClose.Enabled = true;  // enable timer 
                    break;
                // to fix here                    
                case AlertChoiceEnum.Skip: // SKIP
                    AutoCloseDataTime = DateTime.Now.AddMinutes(30); // do controll every 30 minuts
                    logger.Info("[AutoClose Mule: SKIP] Skip");
                    logger.Info("[AutoClose Mule: SKIP] Next Tock " + AutoCloseDataTime.ToString());
                    Dialog.Dispose();
                    logger.Info("[AutoClose Mule] LogOut");
                    Service.Close();
                    timerAutoClose.Enabled = true;  // enable timer 
                    break;
                case AlertChoiceEnum.Disable:    // disable autoclose
                    logger.Info("[AutoClose Mule: DISABLE] Disable");
                    Dialog.Dispose();
                    logger.Info("[AutoClose Mule] LogOut");
                    Service.Close();
                    DisableAutoCloseEmule();
                    timerAutoClose.Enabled = true;  // enable timer 
                    break;
            }
        }

        public void EnableAutoCloseEmule()
        {
            this.menuItemAutoCloseEmule.Checked = true;   // Enable trybar context menu
            this.autoCloseEMuleToolStripMenuItem.Checked = true; // File -> Menu -> Configure
            this.MainConfig.CloseEmuleIfAllIsDone = true; // Enable function
            AutoCloseDataTime = DateTime.Now.AddMinutes(30);
            timerAutoClose.Enabled = true;
        }

        public void DisableAutoCloseEmule()
        {
            this.menuItemAutoCloseEmule.Checked = false; // disable context menu
            this.autoCloseEMuleToolStripMenuItem.Checked = false; // File -> Menu -> Configure
            this.MainConfig.CloseEmuleIfAllIsDone = false; // disable function
            timerAutoClose.Enabled = false;

        }

        public void EnableAutoStarteMule()
        {

        }


        public void DisableAutoStarteMule()
        {

        }

        public void SendMailDownload(string FileName, string Ed2kLink)
        {
            if (MainConfig.EmailNotification == true)
            {
                string stmpServer = MainConfig.ServerSMTP;
                string EmailReceiver = MainConfig.MailReceiver;
                string EmailSender = MainConfig.MailSender;
                string Subject = "TV Underground Downloader Notification";
                string message = "New file add\r\n" + FileName + "\r\n";
                SmtpClient.SendEmail(stmpServer, EmailReceiver, EmailSender, Subject, message);
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

        private void testAutoCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // remove 1h from AutoClose Data Time to force function to work
            TimeSpan delta = new TimeSpan(1, 0, 0);
            AutoCloseDataTime = DateTime.Now.Subtract(delta);
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

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            DeleteRssChannel();
        }

        void DeleteRssChannel()
        {
            if (listViewFeed.Items.Count == 0)
                return;

            if (listViewFeed.SelectedItems.Count == 0)
                return;

            // check user 
            DialogResult rc;
            rc = MessageBox.Show("Confirm delete", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            if (rc != DialogResult.OK)
            {
                return;
            }


            foreach (ListViewItem selectedItem in listViewFeed.SelectedItems)
            {
                string feedTitle = selectedItem.Text;

                RssSubscription Feed = null;

                Feed = MainConfig.RssFeedList.Find(delegate (RssSubscription subscrission)
                {
                    return subscrission.listViewItem == selectedItem;
                });


                if (Feed != null)
                {
                    listViewFeed.Items.Remove(Feed.listViewItem);
                    MainHistory.DeleteFileByFeedSource(Feed.Url);
                    MainConfig.RssFeedList.Remove(Feed);
                    DataBaseHelper.RssSubscriptionList.Delete(Feed);
                }
            }
            MainConfig.Save();

            listViewFeedFilesList.Items.Clear();
            UpdateRssFeedGUI(); ///upgrade GUI
        }
        /// <summary>
        /// wizard for add new feed
        /// </summary>
        void AddRssChannel()
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

            List<string> CurrentRssUrlList = new List<string>();
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
            MainConfig.RssFeedList.ForEach(delegate (RssSubscription t) { CurrentRssUrlList.Add(t.Url); });

            //
            //  Open dialog 1 to path the URL
            //
            AddFeedDialogPage1 dialogPage1 = new AddFeedDialogPage1(CurrentRssUrlList);
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

            List<string> RssUrlList = dialogPage1.RssUrlList;
            bool fastAdd = dialogPage1.FastAdd;
            dialogPage1.Dispose();
            //
            //  Open dialog 2 to get all ed2k from feed
            //
            AddFeedDialogPage2 dialogPage2 = new AddFeedDialogPage2(RssUrlList, MainConfig.ServiceUrl, MainConfig.Password, cookieContainer, fastAdd);
            dialogPage2.ShowDialog();

            if (dialogPage2.DialogResult != DialogResult.OK)
            {
                dialogPage2.Dispose();
                return;
            }

            // 
            //  check file count
            //
            if ((dialogPage2.rssChannelList.Count == 0) ^ ((dialogPage2.newFilesList.Count == 0) & (fastAdd == false)))
            {
                MessageBox.Show("Nothing to downloads");
                dialogPage2.Dispose();
                return;
            }

            List<RssChannel> rssChannelList = dialogPage2.rssChannelList;
            List<FileHistory> newFilesList = dialogPage2.newFilesList;
            List<string> eMuleCategoryList = dialogPage2.ListCategory;

            dialogPage2.Dispose();  // free dialog
            // setup default 
            foreach (RssChannel rc in rssChannelList)
            {
                rc.Category = MainConfig.DefaultCategory;
                rc.Pause = false;
                rc.maxSimultaneousDownload = MainConfig.MaxSimultaneousFeedDownloads;
            }

            //
            //  show Page 3 : choose single files to download.
            //
            AddFeedDialogPage3 dialogPage3 = new AddFeedDialogPage3(rssChannelList, newFilesList, eMuleCategoryList);

            if (fastAdd == false)
            {
                dialogPage3.ShowDialog();
                if (dialogPage3.DialogResult != DialogResult.OK)
                {
                    dialogPage3.Dispose();
                    return;
                }
            }
            //
            //  Add rss channel
            //
            foreach (RssChannel rsschannel in rssChannelList)
            {
                RssSubscription RssSubscrission = new RssSubscription(rsschannel.Title, rsschannel.Url);
                RssSubscrission.Category = rsschannel.Category;
                RssSubscrission.PauseDownload = rsschannel.Pause;
                RssSubscrission.Enabled = true;
                RssSubscrission.MaxSimultaneousDownload = rsschannel.maxSimultaneousDownload;
                MainConfig.RssFeedList.Add(RssSubscrission);
            }
            //
            //  remove all data from history
            //  note: this is important when you re add a old series
            //
            foreach (FileHistory fh in newFilesList)
            {
                MainHistory.DeleteFile(fh);
            }

            //
            //  Add file history
            //  
            dialogPage3.UnselectedFile.ForEach(delegate (FileHistory fh) { logger.Debug("UnselectedHistory {0}", fh.FileName); });

            // Add the unselected file to the history to avoid redownload
            foreach (FileHistory fh in dialogPage3.UnselectedFile)
            {
                History.Add(fh.Ed2kLink, fh.FeedLink, fh.FeedSource, DateTime.MinValue.ToString("s"));
            }
            dialogPage3.Dispose();

            FeedLinkCache.CleanUp();

            MainConfig.Save();
            UpdateRssFeedGUI();
            StartDownloadThread();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listViewFeedFilesList.Items.Count == 0)
                return;

            if (listViewFeedFilesList.SelectedItems.Count == 0)
                return;

            ListViewItem selectedItem = listViewFeedFilesList.SelectedItems[0];
            string strSelectItemText = selectedItem.Text;   // this contain name file

            logger.Info(string.Format("Delete {0}", strSelectItemText));

            // remove from list view
            listViewFeedFilesList.Items.Remove(selectedItem);

            // remove file from main history
            MainHistory.DeleteFile(strSelectItemText);

            // finally update GUI
            UpdateRssFeedGUI();

        }

        private void deleteCompleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            List<RssSubscription> channelToDelete = MainConfig.RssFeedList.FindAll(delegate (RssSubscription t) { return t.tvuStatus == tvuStatus.Complete; });
            foreach (RssSubscription subscrission in channelToDelete)
            {
                MainHistory.DeleteFileByFeedSource(subscrission.Url);
            }

            // check user 
            DialogResult rc;
            rc = MessageBox.Show("Delete all complete feed", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            if (rc != DialogResult.OK)
            {
                return;
            }

            List<RssSubscription> completeFeed = new List<RssSubscription>();
            completeFeed = MainConfig.RssFeedList.FindAll(delegate (RssSubscription t) { return t.tvuStatus == tvuStatus.Complete; });

            foreach (RssSubscription feed in completeFeed)
            {
                // remove from main configuration
                MainConfig.RssFeedList.Remove(feed);

                // remove from list view
                if (feed.listViewItem != null)
                    listViewFeed.Items.Remove(feed.listViewItem);
            }
            // save changes
            MainConfig.Save();

            ///upgrade GUI
            UpdateRssFeedGUI();
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

        private void cancelCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void listViewFeed_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //http://support.microsoft.com/kb/319401

            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listViewFeed.Sort();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox1 dialog = new AboutBox1();
            dialog.ShowDialog();
        }

        private void reportABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://sourceforge.net/tracker/?group_id=357576&atid=1492909");
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://tvudownloader.sourceforge.net/");
        }

        private void forumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://sourceforge.net/tracker/?group_id=357576&atid=1492909");
        }

        private void oPMLExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog Dialog = new SaveFileDialog();
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

            List<string> Feed = new List<string>();

            Dialog.Filter = "OPML (*.opml)|*.opml";
            Dialog.FilterIndex = 2;
            Dialog.RestoreDirectory = false;

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream myStream;

                    if ((myStream = Dialog.OpenFile()) != null)
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

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (listViewFeed.Items.Count == 0)
                return;

            if (listViewFeed.SelectedItems.Count == 0)
                return;

            ListViewItem temp = listViewFeed.SelectedItems[0];
            int i = listViewFeed.Items.IndexOf(temp);
            string feedTitle = listViewFeed.Items[i].Text;

            RssSubscription SelectedFeed = MainConfig.RssFeedList[0];

            bool found = false;
            for (i = 0; i < listViewFeed.Items.Count; i++)
            {
                string Title1 = MainConfig.RssFeedList[i].Title.Replace("[ed2k] tvunderground.org.ru:", "");
                string Title2 = feedTitle.Replace("[ed2k] tvunderground.org.ru:", "");
                if (Title1 == Title2)
                {
                    SelectedFeed = MainConfig.RssFeedList[i];
                    found = true;
                    break;
                }
            }

            if (found == false)
            {
                return;
            }

            EditFeedForm dialog = new EditFeedForm(MainConfig, SelectedFeed.Category, SelectedFeed.PauseDownload, SelectedFeed.Enabled, SelectedFeed.MaxSimultaneousDownload);
            dialog.ShowDialog();

            if (dialog.DialogResult != DialogResult.OK)
            {
                dialog.Dispose();
                return;
            }

            SelectedFeed.Enabled = dialog.feedEnable;
            SelectedFeed.Category = dialog.Category;
            SelectedFeed.PauseDownload = dialog.PauseDownload;
            SelectedFeed.MaxSimultaneousDownload = dialog.maxSimultaneousDownload;

            MainConfig.Save();
            UpdateRssFeedGUI();

        }



        void AddItemToListBoxPending(string text)
        {
            listBoxPending.Items.Add(text);
            listBoxPending.Refresh();
            return;

        }


        void RemoveItemToListBoxPending(string text)
        {

            int index = listBoxPending.Items.IndexOf(text);
            if (index > -1)
            {
                listBoxPending.Items.RemoveAt(index);
            }
            return;
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

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in listViewFeed.SelectedItems)
            {
                string feedTitle = selectedItem.Text;

                RssSubscription Feed = null;

                Feed = MainConfig.RssFeedList.Find(delegate (RssSubscription t)
                {
                    return t.TitleCompact.IndexOf(feedTitle) > -1;
                });


                if (Feed != null)
                {
                    Feed.Enabled = true;

                }
            }
            MainConfig.Save();

            listViewFeedFilesList.Items.Clear();
            UpdateRssFeedGUI(); ///upgrade GUI
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in listViewFeed.SelectedItems)
            {
                string feedTitle = selectedItem.Text;

                RssSubscription Feed = null;

                Feed = MainConfig.RssFeedList.Find(delegate (RssSubscription t)
                {
                    return t.TitleCompact.IndexOf(feedTitle) > -1;
                });


                if (Feed != null)
                {
                    Feed.Enabled = false;

                }
            }
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

        private void importOldHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // migrate History.xml only for backup because it's replace by sqlite
            //
            // old path
            //      C:\Users\User\AppData\Local\tvu\tvu\History.xml
            // new path, not yet used 
            //      C:\Users\User\AppData\Local\TvUndergroundDownloader\History.xml
            //

            // try to load history from old storage position
            string oldHistoryFile = Config.FileNameHistory;
            oldHistoryFile = oldHistoryFile.Replace(@"TvUndergroundDownloader\History.xml", @"tvu\tvu\History.xml");

            if (File.Exists(oldHistoryFile))
            {
                bool rc = History.MigrateFromXMLToDB(oldHistoryFile);
                if (rc == false)
                {
                    MessageBox.Show("An error occurred during migration to new data system");
                }
            }
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
                    ExportImportHelper.Export(MainConfig, MainHistory, myStream);
                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }


        }

        private void importDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "XML (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ExportImportHelper.Import(MainConfig, MainHistory, openFileDialog1.FileName);
                UpdateRecentActivity();
                UpdateRssFeedGUI();
            }

        }
    }


}
