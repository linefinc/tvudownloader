using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using System.Xml.Schema;
using System.Threading;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Win32;
using System.Text.RegularExpressions;

using tvu;

namespace tvu
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

        public Config MainConfig;
        public History MainHistory;

        delegate void SetTextCallback(string text, bool verbose);

        private DateTime DownloadDataTime;
        private DateTime AutoCloseDataTime;




        public class sDonwloadFile
        {

            public string FeedSource{get; private set;}
            public string FeedLink{get; private set;}
            public string Ed2kLink{get; private set;}

            public bool PauseDownload{get; private set;}
            public string Category {get; private set;}
            
            public sDonwloadFile(string Ed2kLink, string FeedLink, string FeedSource, bool PauseDownload, string Category)
            {
                this.Ed2kLink = Ed2kLink;
                this.FeedLink = FeedLink;
                this.FeedSource = FeedSource;
                this.PauseDownload = PauseDownload;
                this.Category = Category;
            }
            
            
        };

        public FormMain()
        {
            // load config
            MainConfig = new Config();
            MainConfig.Load();

            // load History
            MainHistory = new History();
            MainHistory.Read();

            InitializeComponent();
            SetupNotify();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
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

            label8.Text = "";
            label10.Text = "";
            label12.Text = "";
            label14.Text = "";
            labelTotalFiles.Text = "";
            labelMaxSimultaneousDownloads.Text = "";

            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;

            UpdateRecentActivity();
            UpdateRssFeedGUI();

            Log.Instance.AddLogTarget(new LogTargetTextBox(this, LogTextBox));
            Log.Instance.AddLogTarget(new LogTargetFile(MainConfig.FileNameLog));
            Log.Instance.SetVerboseMode(MainConfig.Verbose);
            
        }


        private void SetupNotify()
        {

            IconUp = tvu.Properties.Resources.appicon1;

            IconDown = tvu.Properties.Resources.appicon2;

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
            Log.logInfo("Auto Start Emule");
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
                AutoCloseDataTime = DateTime.Now.AddMinutes(30); // do controll every 30 minuts
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

            while (listView1.Items.Count > 0)
            {
                listView1.Items.Remove(listView1.Items[0]);
            }

            FeedLinkCache feedLinkCache = new FeedLinkCache();
            feedLinkCache.Load();

            List<RssSubscrission> myRssFeedList = new List<RssSubscrission>();
            myRssFeedList.AddRange(MainConfig.RssFeedList);
            //
            //people.Sort((x, y) => string.Compare(x.LastName, y.LastName));
            //
            myRssFeedList.Sort((x, y) => string.Compare(x.Title, y.Title));



            foreach (RssSubscrission t in myRssFeedList)
            {
                string title = t.Title.Replace("[ed2k] tvunderground.org.ru:", "");
                ListViewItem item = new ListViewItem(title);

                // total downloads
                int counter =feedLinkCache.CountFeedLink(t.Url);
                
                
                if(counter >0)
                {
                    item.SubItems.Add(string.Format("{0}+{1}",t.TotalDownloads,counter));
                }
                else
                {
                    item.SubItems.Add(t.TotalDownloads.ToString());
                }
                // last upgrade
                uint days = 0;

                if (t.LastTvUStatusUpgradeDate == string.Empty)
                {
                    t.LastUpgradeDate = MainHistory.LastDownloadDateByFeedSource(t.Url);
                }

                if (t.LastUpgradeDate.Equals("") == false)
                {

                    DateTime LastDownloadTime = Convert.ToDateTime(t.LastUpgradeDate);

                    TimeSpan diff = DateTime.Now.Subtract(LastDownloadTime);
                    days = (uint)diff.TotalDays;
                    item.SubItems.Add(days.ToString() + " days");
                }
                else
                {
                    item.SubItems.Add("");
                }

                // tvunder status
                switch (t.tvuStatus)
                {
                    default:
                        item.SubItems.Add("Unknow");
                        break;
                    case tvuStatus.Complete:
                        item.SubItems.Add("Complete");
                        break;
                    case tvuStatus.StillRunning:
                        item.SubItems.Add("Still Running");
                        break;
                    case tvuStatus.StillIncomplete:
                        item.SubItems.Add("Still Incomplete");
                        break;
                    case tvuStatus.OnHiatus:
                        item.SubItems.Add("On Hiatus");
                        break;

                }

                listView1.Items.Add(item);
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (DateTime.Now > DownloadDataTime)
            {
                if (MainConfig.AutoClearLog == true)
                {
                    LogTextBox.Clear();
                }

                DownloadDataTime = DateTime.Now.AddMinutes(MainConfig.IntervalTime);

                Log.logInfo("Now : " + DateTime.Now.ToString());
                Log.logInfo("next tick : " + DownloadDataTime.ToString());
                StartDownloadThread();
                UpdateRssFeedGUI();

                if ((MainConfig.EmailNotification == true) && (sendLogToEmailToolStripMenuItem.Checked))
                {
                    string stmpServer = MainConfig.ServerSMTP;
                    string EmailReceiver = MainConfig.MailReceiver;
                    string EmailSender = MainConfig.MailSender;
                    string Subject = "TV Underground Downloader Notification";
                    SmtpClient.SendEmail(stmpServer, EmailReceiver, EmailSender, Subject, LogTextBox.Text);
                }
            }

            if (CheckNewVersion() == true)
            {
                MessageBox.Show("New Version is available at http://tvudownloader.sourceforge.net/");
            }


        }



        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
                return;

            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem temp = listView1.SelectedItems[0];
            int i = listView1.Items.IndexOf(temp);
            string feedTitle = listView1.Items[i].Text;

            RssSubscrission Feed = MainConfig.RssFeedList[0];

            bool found = false;
            for (i = 0; i < listView1.Items.Count; i++)
            {
                string Title1 = MainConfig.RssFeedList[i].Title.Replace("[ed2k] tvunderground.org.ru:", "");
                string Title2 = feedTitle.Replace("[ed2k] tvunderground.org.ru:", "");
                if (Title1 == Title2)
                {
                    Feed = MainConfig.RssFeedList[i];
                    found = true;
                    break;
                }
            }

            if (found == false)
            {
                return;
            }

            label8.Text = Feed.Category;
            label10.Text = Feed.PauseDownload.ToString();
            label12.Text = Feed.Url;

            label14.Text = MainHistory.LastDownloadDateByFeedSource(Feed.Url);
            labelTotalFiles.Text = MainHistory.LinkCountByFeedSource(Feed.Url).ToString();
            labelMaxSimultaneousDownloads.Text = Feed.maxSimultaneousDownload.ToString();

            // update list history
            listView2.Items.Clear();


            // extract file by feedLink
            List<fileHistory> ListHistory;
            ListHistory = MainHistory.fileHistoryList.FindAll(delegate(fileHistory fh) { return fh.FeedSource == Feed.Url; });

            ListHistory.Sort((x, y) => (y.Date.CompareTo(x.Date)));

            foreach (fileHistory fh in ListHistory)
            {
                ListViewItem item = new ListViewItem(fh.FileName);
                string date = fh.Date.Substring(0,10);
                item.SubItems.Add(date);
                listView2.Items.Add(item);
            }
        }

        private void StartDownloadThread()
        {
            if (backgroundWorker1.IsBusy == true)
            {
                Log.logInfo("Thread is busy");
                return;
            }

            if (autoClearToolStripMenuItem.Checked == true)
            {
                LogTextBox.Clear();
            }

            checkNowToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;
            addToolStripMenuItem.Enabled = false;
            deleteCompleteToolStripMenuItem.Enabled = false;
            toolStripMenuItemAdd.Enabled = false;
            toolStripMenuItemDelete.Enabled = false;
            toolStripMenuItemEdit.Enabled = false;

            menuItemCheckNow.Enabled = false;
            cancelCheckToolStripMenuItem.Enabled = true;
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

            Log.logInfo("Start RSS Check");

            List<sDonwloadFile> DownloadFileList = new List<sDonwloadFile>();

            FeedLinkCache feedLinkCache = new FeedLinkCache();
            feedLinkCache.Load();

            foreach (RssSubscrission feed in MainConfig.RssFeedList)
            {

                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                Log.logVerbose("Read RSS " + feed.Url);

                try
                {
                    string WebPage = WebFetch.Fetch(feed.Url, true);
                   
                    List<string> elemList = new List<string>();

                    if (WebPage != null)
                    {
                        //get GUID page

                        WebPage.Replace("http://www.tvunderground.org.ru/", "http://tvunderground.org.ru/");

                        Regex Pattern = new Regex(@"http://tvunderground.org.ru/index.php\?show=ed2k&season=\d{1,10}&sid\[\d{1,10}\]=\d{1,10}");

                        MatchCollection matchCollection = Pattern.Matches(WebPage);


                        foreach (Match value in matchCollection)
                        {
                            if (backgroundWorker1.CancellationPending)
                            {
                                e.Cancel = true;
                                return;
                            }

                            string FeedLink = value.ToString();
                            if (MainHistory.FileExistByFeedLink(FeedLink) == false)
                            {
                                elemList.Add(FeedLink);
                                feed.tvuStatus = tvuStatus.Unknow; // force refrash of tv Undergoud status when find a new file
                            }
                        }


                        try
                        {
                            DateTime LastTvUStatusUpgradeDate = Convert.ToDateTime(feed.LastTvUStatusUpgradeDate);
                            TimeSpan ts = DateTime.Now - LastTvUStatusUpgradeDate;
                            if (ts.TotalDays > 15)
                            {
                                feed.tvuStatus = tvuStatus.Unknow;
                            }
                        }
                        catch
                        {
                            feed.tvuStatus = tvuStatus.Unknow;
                        }
                        // 
                        //  Start check of complete element 
                        // 
                        if (feed.tvuStatus == tvuStatus.Unknow)
                        {
                            Pattern = new Regex(@"http://tvunderground.org.ru/index.php\?show=episodes&sid=\d{1,10}");
                            Match Match = Pattern.Match(WebPage);
                            string url = Match.Value;
                            feed.tvuStatus = WebManagerTVU.CheckComplete(url);
                            feed.LastTvUStatusUpgradeDate = DateTime.Now.ToString();
                        }


                    }
                    //
                    // end check compelte element
                    //

                    // reverse the list so the laft feed ( first temporal feed) became the first first feed in list
                    elemList.Reverse();

                    
                    

                    foreach (string FeedLink in elemList)
                    {
                        string sEd2k = string.Empty;
                        
                        if (backgroundWorker1.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }

                        sEd2k = feedLinkCache.FindFeedLink(FeedLink);
                        if (sEd2k == string.Empty)
                        {
                            string page = null;

                            // download the page in FeedLink
                            Log.logVerbose(string.Format("try download page {0}", FeedLink));

                            if ((page = WebFetch.Fetch(FeedLink, true)) == null)
                            {
                                continue;
                            }
                            // parse ed2k
                            sEd2k = RssParserTVU.FindEd2kLink(page);
                            feedLinkCache.AddFeedLink(FeedLink, sEd2k);

                        }

                        if (MainHistory.ExistInHistoryByEd2k(sEd2k) == -1)
                        {
                            Ed2kfile parser = new Ed2kfile(sEd2k);
                            Log.logInfo(string.Format("Found new file {0}", parser.GetFileName()));

                            sDonwloadFile DL = new sDonwloadFile(sEd2k, FeedLink, feed.Url, feed.PauseDownload, feed.Category);

                            // remove the comment:
                            //counter++;

                            DownloadFileList.Add(DL);
                        }
                        else
                        {
                            // link ed2k just download bat not correct registred
                            // to avoid rendondance of link
                            MainHistory.Add(sEd2k, FeedLink, feed.Url);
                        }


                    }
                   


                }
                catch
                {
                    Log.logInfo("Some Error in rss parsing");
                }

                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                //update rss feed
                feed.LastUpgradeDate = MainHistory.LastDownloadDateByFeedSource(feed.Url);
                feed.TotalDownloads = MainHistory.LinkCountByFeedSource(feed.Url);
                int progress = (MainConfig.RssFeedList.IndexOf(feed) + 1) * 100;
                progress = progress / MainConfig.RssFeedList.Count;
                backgroundWorker1.ReportProgress(progress);


            }

            feedLinkCache.Save();

            if (DownloadFileList.Count == 0)
            {
                Log.logVerbose("Nothing to download");
                return;
            }
            else
            {
                Log.logVerbose("Total File Found " + DownloadFileList.Count);
            }


            eMuleWebManager Service = new eMuleWebManager(MainConfig.ServiceUrl, MainConfig.Password);
            bool? rc = Service.LogIn();


            //
            //  if emule is close and new file < min to start not do null
            //
            //
            // try to start emule
            // the if work only if rc == null ad 
            if ((MainConfig.StartEmuleIfClose == true) & (DownloadFileList.Count > MainConfig.MinToStartEmule))
            {
                for (int i = 1; (i <= 5) & (rc == null); i++)
                {
                    if (backgroundWorker1.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }


                    Log.logInfo(string.Format("start eMule Now (try {0}/5)", i));
                    Log.logInfo("Wait 60 sec");
                    try
                    {
                        Process.Start(MainConfig.eMuleExe);
                    }
                    catch
                    {
                        Log.logInfo("Unable start application");
                    }

                    for (int n = 0; n < 10; n++)
                    {
                        if (backgroundWorker1.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                        
                        Thread.Sleep(500);
                    }
                    
                    rc = Service.LogIn();

                }
            }

            Log.logVerbose("Check min download");
            if (rc == null)
            {
                if (DownloadFileList.Count < MainConfig.MinToStartEmule)
                {
                    Log.logInfo("Min file download not reached");
                    return;
                }

                Log.logInfo("Unable to connect to eMule web server");
                return;


            }


            Log.logVerbose("Retrive list Category");
            Service.GetCategory(true);  // force upgrade category list 

            Log.logVerbose("Clean download list (step 1) Find channel from ed2k");


            List<string> CourrentDownloadsFormEmule = Service.GetActualDownloads();/// file downloaded with this program and now in download in emule

            Log.logVerbose("Courrent Download Form Emule " + CourrentDownloadsFormEmule.Count);
            List<sDonwloadFile> ActualDownloadFileList = new List<sDonwloadFile>();


            foreach (string ad in CourrentDownloadsFormEmule)
            {
                try
                {
                    Ed2kfile newFile = new Ed2kfile(ad);

                    foreach (fileHistory fh in MainHistory.fileHistoryList)
                    {
                        if (fh == newFile)
                        {
                            sDonwloadFile newADFile = newADFile = new sDonwloadFile(ad, fh.FeedLink, fh.FeedSource, false, "");
                            ActualDownloadFileList.Add(newADFile);

                            Log.logInfo(string.Format("Found file ({0}) ,{1} ,{2}", newFile.FileName, fh.FeedLink, fh.FeedSource));
                        }
                    }
                }
                catch (Exception exception)
                {
                    Log.logInfo("Error in ckeck file \"" + ad);
                    Log.logInfo("Error message error: \"" + exception.Message + "\"");
                    
                }
            }

            Log.logInfo("ActualDownloadFileList.Count = " + ActualDownloadFileList.Count);
            Log.logInfo("Clean download list (step 2) check limit for each channel");

            
            // clean RssFeedList
            List<RssSubscrission> RssFeedList = new List<RssSubscrission>();
            RssFeedList.AddRange(MainConfig.RssFeedList);

            Log.logInfo("MainConfig.MaxSimultaneousFeedDownloads = " + MainConfig.MaxSimultaneousFeedDownloads);

            foreach(RssSubscrission RssFeed in MainConfig.RssFeedList)
            {

                Log.logInfo("check feed = \"" + RssFeed.Title + "\"");
                string FeedURL = RssFeed.Url;
            
    
                // calcolo il numero di file che ho con quel feed;
                List<sDonwloadFile> CurrentlyDownloadingFileFromEmuleByFeed;
                CurrentlyDownloadingFileFromEmuleByFeed = ActualDownloadFileList.FindAll(delegate(sDonwloadFile t)
                                { return (t.FeedLink == FeedURL)^(t.FeedSource == FeedURL); });

                // estraggo i file da scaricare del feed
                List<sDonwloadFile> PendingFileFromRssFeed;
                PendingFileFromRssFeed = DownloadFileList.FindAll(delegate(sDonwloadFile file)
                                                        { return (file.FeedLink == FeedURL) ^ (file.FeedSource == FeedURL); });

                // calcolo il numero di file da scaricare:
                int dif = RssFeed.maxSimultaneousDownload - CurrentlyDownloadingFileFromEmuleByFeed.Count;

                if (PendingFileFromRssFeed.Count > 0)
                {
                    if (dif <= 0)
                    {
                        Log.logVerbose(string.Format("Limit reached ({0}), remove all pending element",RssFeed.maxSimultaneousDownload));

                        // nothing to download
                        foreach (sDonwloadFile file in PendingFileFromRssFeed)
                        {
                            DownloadFileList.Remove(file);
                        }
                       
                    }
                    else
                    {
                        int delta = PendingFileFromRssFeed.Count - dif;
                        Log.logVerbose(string.Format("Limit reached ({0}), remove {0} pending element",RssFeed.maxSimultaneousDownload, delta));

                        PendingFileFromRssFeed.Sort(delegate(sDonwloadFile A, sDonwloadFile B)
                                                        { return A.Ed2kLink.CompareTo(B.Ed2kLink); });

                        for (int index = dif; index < PendingFileFromRssFeed.Count; index++)
                        {
                            sDonwloadFile temp = PendingFileFromRssFeed[index];
                            DownloadFileList.Remove(temp);
                        }
                    }
                }
            }

            Log.logVerbose("DownloadFileList  = " + DownloadFileList.Count);

            Log.logVerbose("Download file");
            //
            //  Download file 
            // 
            foreach (sDonwloadFile DownloadFile in DownloadFileList)
            {

                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                        
                Ed2kfile ed2klink = new Ed2kfile(DownloadFile.Ed2kLink);
                Log.logVerbose("Add file to download" );
                Service.AddToDownload(ed2klink, DownloadFile.Category);

                if (DownloadFile.PauseDownload == true)
                {
                    Log.logVerbose("Pause download" );
                    Service.StopDownload(ed2klink);
                }
                else
                {
                    Log.logVerbose("Resume download");
                    Service.StartDownload(ed2klink);
                }
                MainHistory.Add(DownloadFile.Ed2kLink, DownloadFile.FeedLink, DownloadFile.FeedSource);
                Ed2kfile parser = new Ed2kfile(DownloadFile.Ed2kLink);
                Log.logInfo(string.Format("Add file to emule {0} \n", parser.GetFileName()));
                SendMailDownload(parser.GetFileName(), DownloadFile.Ed2kLink);

                { // progress bar
                    int progress = (DownloadFileList.IndexOf(DownloadFile) + 1) * 100;
                    backgroundWorker1.ReportProgress(progress / DownloadFileList.Count);
                }
            }
            MainHistory.Save();
            Service.LogOut();
        }

        /// <summary>
        /// This is on the main thread, so we can update a TextBox or anything.
        /// </summary>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Log.logInfo("Background Worker Cancelled");
            }
            
            UpdateRecentActivity();
            UpdateRssFeedGUI();
            
            cancelCheckToolStripMenuItem.Enabled = true;
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

            Log.logVerbose("FormClosing Event");
            Log.logVerbose(string.Format("{0} = {1}", "CloseReason", e.CloseReason));
            Log.logVerbose(string.Format("{0} = {1}", "Cancel", e.Cancel));
            Log.logVerbose(string.Format("{0} = {1}", "mAllowClose", mAllowClose));

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
            Log.logInfo("Config loaded " + MainConfig.FileName);
            timer2.Enabled = false;

            if (MainConfig.StartMinimized == true)
            {
                mVisible = false;
                this.Visible = false;
            }
            StartDownloadThread();

            if (CheckNewVersion() == true)
            {
                MessageBox.Show("New Version is available at http://tvudownloader.sourceforge.net/");
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
                string lastCheck = MainConfig.LastUpgradeCheck;
                string[] lastCheckSplit = lastCheck.Split('-');
                
                int year = Convert.ToInt32(lastCheckSplit[0]);
                int month = Convert.ToInt32(lastCheckSplit[1]);
                int day = Convert.ToInt32(lastCheckSplit[2]);

                DateTime lastCheckDateTime = new DateTime(year, month, day);
                DateTime nextCheck = lastCheckDateTime.AddDays(MainConfig.intervalBetweenUpgradeCheck);

                if (DateTime.Now < nextCheck)
                {
                    return false;
                }

                MainConfig.LastUpgradeCheck = DateTime.Now.ToString("yyyy-MM-dd");
                
                XmlDocument doc = new XmlDocument();
                doc.Load("http://tvudownloader.sourceforge.net/version.php?tvuid=" + MainConfig.tvudwid);

                string lastVersion = "";

                foreach (XmlNode t in doc.GetElementsByTagName("last"))
                {
                    lastVersion = t.InnerText;
                }

                List<int> lastVersionInt = new List<int>();
                List<int> currentVersionInt = new List<int>();

                string[] strSplit = lastVersion.Split('.');
                foreach (string t in strSplit)
                {
                    lastVersionInt.Add(Convert.ToInt16(t));
                }

                strSplit = Config.Version.Split('.');
                foreach (string t in strSplit)
                {
                    currentVersionInt.Add(Convert.ToInt16(t));
                }

                while (currentVersionInt.Count != lastVersionInt.Count)
                {

                    if (currentVersionInt.Count > lastVersionInt.Count)
                    {
                        lastVersionInt.Add(0);
                    }
                    if (currentVersionInt.Count < lastVersionInt.Count)
                    {
                        lastVersionInt.Add(0);
                    }
                }

                for (int index = 0; index < currentVersionInt.Count; index++)
                {
                    if (lastVersionInt[index] > currentVersionInt[index])
                    {
                        return true;
                    }

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
            listBox1.Items.Clear();
            List<fileHistory> myFileHistoryList = MainHistory.GetRecentActivity(32);

            foreach (fileHistory fh in myFileHistoryList)
            {
                string date = fh.Date.Replace('T', ' ');
                string t = string.Format("{0}:{1}", date, fh.GetFileName());
                listBox1.Items.Add(t);
            }

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
            LogTextBox.Clear();
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
                MainConfig.IntervalTime = OptDialog.IntervalTime;
                MainConfig.StartMinimized = OptDialog.StartMinimized;
                MainConfig.StartEmuleIfClose = OptDialog.StartEmuleIfClose;
                MainConfig.CloseEmuleIfAllIsDone = OptDialog.CloseEmuleIfAllIsDone;

                Config.StartWithWindows = OptDialog.StartWithWindows;
                
                MainConfig.Verbose = OptDialog.Verbose;
                Log.Instance.SetVerboseMode(MainConfig.Verbose);

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
                Log.logVerbose("[AutoClose Mule] MainConfig.CloseEmuleIfAllIsDone == false");
                return;
            }

            // check if Auto Close Data Time is not set
            if (AutoCloseDataTime == DateTime.MinValue)
            {
                Log.logVerbose("[AutoClose Mule] AutoCloseDataTime = DateTime.Now.AddMinutes(30);");
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

            // conncect to mule

            Log.logVerbose("[AutoClose Mule] Check Login");
            eMuleWebManager Service = new eMuleWebManager(MainConfig.ServiceUrl, MainConfig.Password);
            bool? rc = Service.LogIn();


            // if mule close ... end of game
            if (rc == null)
            {
                AutoCloseDataTime = DateTime.Now.AddMinutes(30); // do controll every 30 minuts
                Log.logVerbose("[AutoClose Mule] Login failed");
                return;
            }
            Log.logVerbose("[AutoClose Mule] Login ok");

            Log.logVerbose("[AutoClose Mule] Actual Downloads " + Service.GetActualDownloads().Count);
            // if donwload > 0 ... there' s some download ... end 
            if (Service.GetActualDownloads().Count > 0)
            {
                Log.logVerbose("[AutoClose Mule] GetActualDownloads return >0");
                AutoCloseDataTime = DateTime.Now.AddMinutes(30);
                Log.logVerbose("[AutoClose Mule] LogOut");
                Service.LogOut();
                return;
            }

            Log.logVerbose("[AutoClose Mule] Show dialog ");
            // pop up form to advise user
            FormAlerteMuleClose Dialog = new FormAlerteMuleClose();
            Dialog.ShowDialog();

            Log.logVerbose("[AutoClose Mule] Dialog return " + Dialog.AlertChoice.ToString());
            switch (Dialog.AlertChoice)
            {
                case AlertChoiceEnum.Close:// Close
                    Log.logVerbose("[AutoClose Mule: CLOSE] Close Service");
                    Dialog.Dispose();
                    Service.Close();
                    timerAutoClose.Enabled = true;  // enable timer 
                    break;
                // to fix here                    
                case AlertChoiceEnum.Skip: // SKIP
                    AutoCloseDataTime = DateTime.Now.AddMinutes(30); // do controll every 30 minuts
                    Log.logVerbose("[AutoClose Mule: SKIP] Skip");
                    Log.logVerbose("[AutoClose Mule: SKIP] Next Tock " + AutoCloseDataTime.ToString());
                    Dialog.Dispose();
                    Log.logVerbose("[AutoClose Mule] LogOut");
                    Service.LogOut();
                    timerAutoClose.Enabled = true;  // enable timer 
                    break;
                case AlertChoiceEnum.Disable:    // disable autoclose
                    Log.logVerbose("[AutoClose Mule: DISABLE] Disable");
                    Dialog.Dispose();
                    Log.logVerbose("[AutoClose Mule] LogOut");
                    Service.LogOut();
                    DisableAutoCloseEmule();
                    timerAutoClose.Enabled = true;  // enable timer 
                    break;
            }

            if ((MainConfig.EmailNotification == true) && (sendLogToEmailToolStripMenuItem.Checked))
            {
                Log.logVerbose("[AutoClose Mule] Send Mail");
                string stmpServer = MainConfig.ServerSMTP;
                string EmailReceiver = MainConfig.MailReceiver;
                string EmailSender = MainConfig.MailSender;
                string Subject = "TV Underground Downloader Notification";
                SmtpClient.SendEmail(stmpServer, EmailReceiver, EmailSender, Subject, LogTextBox.Text);
            }
        }

        public void EnableAutoCloseEmule()
        {
            this.menuItemAutoCloseEmule.Checked = true;   // Enable Trybar context menu
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
            Log.Instance.SetVerboseMode(MainConfig.Verbose);
        }

        private void sendLogToEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sendLogToEmailToolStripMenuItem.Checked == true)
            {
                sendLogToEmailToolStripMenuItem.Checked = false;
            }
            else
            {
                sendLogToEmailToolStripMenuItem.Checked = true;
            }
        }

        private void testAutoCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // remove 1h from AutoClose Data Time to froce function to work
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
                Log.logInfo("Unable start application");
            }
        }

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {

            DeleteRssChannel();
            
        }

        void DeleteRssChannel()
        {
            if (listView1.Items.Count == 0)
                return;

            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem temp = listView1.SelectedItems[0];
            int i = listView1.Items.IndexOf(temp);
            string feedTitle = listView1.Items[i].Text;

            // check user 
            string message = "Delete " + feedTitle;
            DialogResult rc;
            rc = MessageBox.Show(message, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            if (rc != DialogResult.OK)
            {
                return;
            }



            RssSubscrission Feed = MainConfig.RssFeedList[0];
            bool found = false;
            for (i = 0; i < listView1.Items.Count; i++)
            {
                //string Title1 = MainConfig.RssFeedList[i].Title.Replace("[ed2k] tvunderground.org.ru:", "");
                string Title1 = MainConfig.RssFeedList[i].TitleCompact;
                string Title2 = feedTitle.Replace("[ed2k] tvunderground.org.ru:", "");
                if (Title1 == Title2)
                {
                    Feed = MainConfig.RssFeedList[i];
                    found = true;
                    break;
                }
            }

            if (found == false)
            {
                return;
            }


            List<string> FileToDelete = new List<string>();

            foreach (fileHistory fh in MainHistory.fileHistoryList)
            {
                if (fh.FeedSource == Feed.Url)
                {
                    FileToDelete.Add(fh.Ed2kLink);
                }
            }


            MainHistory.fileHistoryList.RemoveAll(delegate(fileHistory fh) { return FileToDelete.IndexOf(fh.Ed2kLink) > -1; });
            FeedLinkCache newFeedLinkCache = new FeedLinkCache();
            newFeedLinkCache.Load();
            newFeedLinkCache.FeedLinkCacheTable.RemoveAll(delegate(FeedLinkCacheRow flcr) { return FileToDelete.IndexOf(flcr.Ed2kLink) > -1; });
            newFeedLinkCache.Save();

            MainConfig.RssFeedList.Remove(Feed);
            MainConfig.Save();
            MainHistory.Save();
            UpdateRssFeedGUI(); ///upgrade gui
        }

        void AddRssChannel()
        {

            List<string> CurrentRssUrlList = new List<string>();

            MainConfig.RssFeedList.ForEach(delegate(RssSubscrission t) { CurrentRssUrlList.Add(t.Url); });

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
            }

            List<string> RssUrlList = dialogPage1.RssUrlList;

            AddFeedDialogPage2 dialogPage2 = new AddFeedDialogPage2(RssUrlList,MainConfig.ServiceUrl,MainConfig.Password);
            dialogPage2.ShowDialog();

            if (dialogPage2.DialogResult != DialogResult.OK)
            {
                dialogPage1.Dispose();
                dialogPage2.Dispose();
                return;
            }


            if ((dialogPage2.RssChannelList.Count==0)^(dialogPage2.ListFileHistory.Count==0))
            {
                MessageBox.Show("Nothing to downloads");
                dialogPage1.Dispose();
                dialogPage2.Dispose();
                return;
            }

            List<RssChannel> RssChannelList = dialogPage2.RssChannelList;
            List<fileHistory> ListFileHistory = dialogPage2.ListFileHistory;

            // setup default 
            foreach (RssChannel rc in RssChannelList)
            {
                rc.Category = MainConfig.DefaultCategory;
                rc.Pause = false;
            }

            AddFeedDialogPage3 dialogPage3 = new AddFeedDialogPage3(RssChannelList, ListFileHistory);
            dialogPage3.ShowDialog();

            if (dialogPage3.DialogResult != DialogResult.OK)
            {
                dialogPage1.Dispose();
                dialogPage2.Dispose();
                dialogPage3.Dispose();
                return;
            }

            //
            //  Add rss channel
            //
            foreach (RssChannel rsschannel in dialogPage3.ListRssChannel)
            {
                RssSubscrission RssSubscrission = new RssSubscrission(rsschannel.Title, rsschannel.Url);
                RssSubscrission.Category = rsschannel.Category;
                RssSubscrission.PauseDownload = rsschannel.Pause;
                MainConfig.RssFeedList.Add(RssSubscrission);
            }
            //
            //  remove all data from history
            //
            foreach (fileHistory fh in dialogPage3.GlobalListFileHisotry)
            {
                MainHistory.fileHistoryList.RemoveAll(delegate(fileHistory t) { return t == fh; });
            }

            //
            //  Add filehistory
            //  
            List<fileHistory> temp = new List<fileHistory>(dialogPage3.UnselectedHistory);
            temp.ForEach(delegate(fileHistory fh) { Log.logDebug("UnselectedHistory " + fh.FileName); });

            // update data
            temp.ForEach(delegate(fileHistory fh) { fh.Date = DateTime.Now.AddYears(-1).ToString("s");});
            MainHistory.fileHistoryList.AddRange(temp);
            
            

            MainHistory.Save();
            MainConfig.Save();
            
            UpdateRssFeedGUI();
            dialogPage1.Dispose();
            dialogPage2.Dispose();
            dialogPage3.Dispose();
            StartDownloadThread();


        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView2.Items.Count == 0)
                return;

            if (listView2.SelectedItems.Count == 0)
                return;

            ListViewItem temp = listView2.SelectedItems[0];
            int i = listView2.Items.IndexOf(temp);
            string str = listView2.Items[i].Text;

            Log.logInfo(string.Format("DELETE {0}:{1}", i, str));
            
            MainHistory.DeleteFile(str);
            listView2.Items.Remove(temp);
            MainHistory.Save();
            UpdateRssFeedGUI();
            
        }

        private void deleteCompleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
                        
            List<RssSubscrission> channelToDelete = MainConfig.RssFeedList.FindAll(delegate(RssSubscrission t) { return t.tvuStatus == tvuStatus.Complete ;});

            List<string> ListUrl = new List<string>();

            foreach (RssSubscrission t in channelToDelete)
            {
                ListUrl.Add(t.Url);
            }

            MainConfig.RssFeedList.RemoveAll(delegate(RssSubscrission t) { return t.tvuStatus == tvuStatus.Complete; });
            MainHistory.fileHistoryList.RemoveAll(delegate(fileHistory t) { return ListUrl.IndexOf(t.FeedLink) > -1; });
            MainHistory.fileHistoryList.RemoveAll(delegate(fileHistory t) { return ListUrl.IndexOf(t.FeedSource) > -1; });

            MainConfig.Save();
            MainHistory.Save();
            UpdateRssFeedGUI(); ///upgrade gui
        }

        private void openLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Log.logInfo("Open log file " + MainConfig.FileNameLog);

                if (System.IO.File.Exists(MainConfig.FileNameLog) == true)
                {

                    Log.logInfo("File exist");
                    string command = string.Format("notepad.exe {0}", MainConfig.FileNameLog);
                    Process.Start(command);
                }
            }
            catch(Exception exception)
            {
                Log.logInfo("Exception \"" + exception.Message + "\"");
            }

        }

        private void cancelCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
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
            this.listView1.Sort();
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

            foreach (RssSubscrission sub in MainConfig.RssFeedList)
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
         
            if (listView1.Items.Count == 0)
                return;

            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem temp = listView1.SelectedItems[0];
            int i = listView1.Items.IndexOf(temp);
            string feedTitle = listView1.Items[i].Text;

            RssSubscrission SelectedFeed = MainConfig.RssFeedList[0];

            bool found = false;
            for (i = 0; i < listView1.Items.Count; i++)
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

            EditFeedForm dialog = new EditFeedForm(SelectedFeed.Category, SelectedFeed.PauseDownload, SelectedFeed.Enabled,SelectedFeed.maxSimultaneousDownload);
            dialog.ShowDialog();

            if (dialog.DialogResult != DialogResult.OK)
            {
                dialog.Dispose();
                return;
            }

            SelectedFeed.Enabled = dialog.feedEnable;
            SelectedFeed.Category = dialog.Category;
            SelectedFeed.PauseDownload = dialog.PauseDownload;
            SelectedFeed.maxSimultaneousDownload = dialog.maxSimultaneousDownload;

            MainConfig.Save();
            UpdateRssFeedGUI(); 

        }

        

    
     
    
    }

   
}
