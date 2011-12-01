﻿using System;
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
    public partial class Form1 : Form
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

        public Form1()
        {

            this.FormClosing += Form1_FormClosing;

            MainConfig = new Config();
            MainConfig.Load();

            InitializeComponent();

            UpdateRssFeedGUI();
            SetupNotify();

            label8.Text = "";
            label10.Text = "";
            label12.Text = "";
            label14.Text = "";
            label16.Text = "";


            DownloadDataTime = DateTime.Now.AddMinutes(MainConfig.IntervalTime);
            AutoCloseDataTime = DateTime.Now.AddMinutes(1);

            // load History
            MainHistory = new History();
            MainHistory.Read();

            UpdateRecentActivity();
            UpdateRssFeedGUI();

            if (MainConfig.AutoClearLog == true)
            {
                autoClearToolStripMenuItem.Checked = true;
            }

            if (MainConfig.Verbose == true)
            {
                verboseToolStripMenuItem.Checked = true;
            }

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
            AppendLogMessage("Auto Start Emule", false);
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
            //AppendLogMessage("Auto Close Emule");
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
                timer1.Enabled = false;
            }
            else
            {
                this.menuItemEnable.Checked = true;
                MainConfig.Enebled = true;
                timer1.Enabled = true;
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

                switch (t.status)
                {
                    case enumStatus.Ok:
                        item.SubItems.Add("Ok");
                        break;

                    case enumStatus.Error:
                        item.SubItems.Add("Error");
                        break;

                }
                // total downloads
                item.SubItems.Add(t.TotalDownloads.ToString());

                // last upgrade
                uint days = 0;
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

                AppendLogMessage("Now : " + DateTime.Now.ToString(), false);
                AppendLogMessage("next tick : " + DownloadDataTime.ToString(), false);
                StartDownloadThread();
                UpdateRssFeedGUI();

                if ((MainConfig.EmailNotification == true) && (sendLogToEmailToolStripMenuItem.Checked))
                {
                    string stmpServer = MainConfig.ServerSMTP;
                    string EmailReceiver = MainConfig.MailReceiver;
                    string EmailSender = MainConfig.MailSender;
                    string Subject = "TV Underground Downloader Notification";
                    string dump = SmtpClient.SendEmail(stmpServer, EmailReceiver, EmailSender, Subject, LogTextBox.Text);
                }
            }

            if (CheckNewVersion() == true)
            {
                MessageBox.Show("New Version is available at http://tvudownloader.sourceforge.net/");
            }


        }

        private void AppendText(string text, bool verbose)
        {
            if (verbose == false)
            {
                this.LogTextBox.Text += text;
                this.LogTextBox.SelectionStart = this.LogTextBox.Text.Length;
                this.LogTextBox.ScrollToCaret();
                this.LogTextBox.Refresh();
                return;
            }

            if ((verbose == true) && (MainConfig.Verbose == true))
            {
                this.LogTextBox.Text += text;
                this.LogTextBox.SelectionStart = this.LogTextBox.Text.Length;
                this.LogTextBox.ScrollToCaret();
                this.LogTextBox.Refresh();
                return;
            }

        }

        private void AppendLogMessage(string text, bool verbose)
        {
            text = "[" + DateTime.Now.ToString() + "]" + text;

            if (text.IndexOf(Environment.NewLine) == -1)
            {
                text += Environment.NewLine;
            }

            // from msdn guide http://msdn.microsoft.com/en-us/library/ms171728%28VS.90%29.aspx
            if (this.LogTextBox.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(AppendText);
                this.Invoke(d, new object[] { text, verbose });
            }
            else
            {
                // It's on the same thread, no need for Invoke
                AppendText(text, verbose);
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
            label16.Text = MainHistory.LinkCountByFeedSource(Feed.Url).ToString();


            // update list history
            listView2.Items.Clear();



            List<fileHistory> ListHistory = new List<fileHistory>();


            foreach (fileHistory fh in MainHistory.fileHistoryList)
            {
                
                if (fh.FeedSource == Feed.Url)
                {
                    ListHistory.Add(fh);
                }
            }

            ListHistory.Sort((x, y) => (y.Date.CompareTo(x.Date)));

            foreach (fileHistory fh in ListHistory)
            {
                ListViewItem item = new ListViewItem(fh.FileName);
                item.SubItems.Add(fh.Date);
                listView2.Items.Add(item);
            }
        }

        private void StartDownloadThread()
        {
            if (backgroundWorker1.IsBusy == true)
            {
                AppendLogMessage("Thread is busy", false);
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
            menuItemCheckNow.Enabled = false;
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


            


            AppendLogMessage("Start RSS Check", false);

            List<sDonwloadFile> myList = new List<sDonwloadFile>();


            foreach (RssSubscrission feed in MainConfig.RssFeedList)
            {

                feed.status = enumStatus.Ok;
                AppendLogMessage("Read RSS " + feed.Url, true);

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

                     
                        foreach(Match value in matchCollection)
                        {
                            string FeedLink = value.ToString();
                            if (MainHistory.FileExistByFeedLink(FeedLink) == false)
                            {
                                elemList.Add(FeedLink);
                                feed.tvuStatus = tvuStatus.Unknow; // force refrash of tv Undergoud status when find a new file
                            }

                            // limit the number of possible download for check
                            //if (counter > MainConfig.MaxSimultaneousFeedDownloads)
                            //{
                            //    AppendLogMessage("Max Simultaneous Feed Downloads Limit", false);
                            //    break;
                            //}
                        
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
                        }
                    }
                    //
                    // end check compelte element
                    //

                    // reverse the list so the laft feed ( first temporal feed) became the first first feed in list
                    elemList.Reverse();

                    int counter = 0;
                    foreach (string FeedLink in elemList)
                    {

                        

                        string page = null;

                        if (counter < 4)
                        {
                            // download the page in FeedLink
                            AppendLogMessage(string.Format("try download page {0}", FeedLink), true);
                            page = WebFetch.Fetch(FeedLink, true);
                        }


                        if (counter >= 4)
                        {
                            AppendLogMessage("Limit donwload per channel",false);
                        }


                        if (page != null) 
                        {
                            // find ed2k
                            string sEd2k = RssParserTVU.FindEd2kLink(page);

                            if (MainHistory.ExistInHistoryByEd2k(sEd2k) == -1)
                            {
                                Ed2kParser parser = new Ed2kParser(sEd2k);
                                AppendLogMessage(string.Format("Found new file {0}", parser.GetFileName()), false);

                                sDonwloadFile DL = new sDonwloadFile(sEd2k, FeedLink, feed.Url,feed.PauseDownload,feed.Category);
                                
                                // remove the comment:
                                //counter++;

                                myList.Add(DL);
                            }
                            else
                            {
                                // link ed2k just download bat not correct registred
                                // to avoid rendondance of link
                                MainHistory.Add(sEd2k, FeedLink, feed.Url);
                            }

                        }

                    }



                }
                catch
                {
                    AppendLogMessage("Some Error in rss parsing", false);
                    feed.status = enumStatus.Error;
                }

                //update rss feed
                feed.LastUpgradeDate = MainHistory.LastDownloadDateByFeedSource(feed.Url);
                feed.TotalDownloads = MainHistory.LinkCountByFeedSource(feed.Url);
                int progress = (MainConfig.RssFeedList.IndexOf(feed) + 1) * 100;
                progress = progress / MainConfig.RssFeedList.Count;
                backgroundWorker1.ReportProgress(progress);


            }



            if (myList.Count == 0)
            {
                AppendLogMessage("Nothing to download", true);
                return;
            }
            else
            {
                AppendLogMessage("Total File Found " + myList.Count, true);
            }


            eMuleWebManager Service = new eMuleWebManager(MainConfig.ServiceUrl, MainConfig.Password);
            bool? rc = Service.LogIn();


            //
            //  if emule is close and new file < min to start not do null
            //
            //
            // try to start emule
            // the if work only if rc == null ad 
            if ((MainConfig.StartEmuleIfClose == true) & (myList.Count > MainConfig.MinToStartEmule))
            {
                for (int i = 1; (i <= 5) & (rc == null); i++)
                {
                    AppendLogMessage(string.Format("start eMule Now (try {0}/5)", i), false);
                    AppendLogMessage("Wait 60 sec", false);
                    try
                    {
                        Process.Start(MainConfig.eMuleExe);
                    }
                    catch
                    {
                        AppendLogMessage("Unable start application", false);
                    }
                    Thread.Sleep(5000);
                    rc = Service.LogIn();

                }
            }

            AppendLogMessage("Check min download", true);
            if (rc == null)
            {
                if (myList.Count < MainConfig.MinToStartEmule)
                {
                    AppendLogMessage("Min file download not reached", false);
                    return;
                }

                AppendLogMessage("Unable to connect to eMule web server", false);
                return;


            }


            AppendLogMessage("Retrive list Category", true);
            Service.GetCategory(true);  // force upgrade category list 

            List<string> ActualDownloads = Service.GetActualDownloads();
            List<string> myFeedSoruceLink = new List<string>();

            foreach (string ad in ActualDownloads)
            {

                //int TrimStart = ad.IndexOf("h=") + 2;
                //int TrimEnd = ad.IndexOf("|", TrimStart + 1);
                //string a = ad.Substring(TrimStart, TrimEnd - TrimStart);

                string[] mySplit = ad.Split('|');
                string myFileName = mySplit[2];
                ulong myFileSize = Convert.ToUInt64(mySplit[3]);

                


                foreach (fileHistory fh in MainHistory.fileHistoryList)
                {


                    if ((fh.FileName == myFileName) && (fh.FileSize == myFileSize))
                    {
                        myFeedSoruceLink.Add(fh.FeedSource);
                    }
                    
                    
                }

            }

            


            foreach(RssSubscrission RssFeed in MainConfig.RssFeedList)
            {
                string FeedURL = RssFeed.Url;
            
    
                // calcolo il numero di file che ho con quel feed;
                List<string> myFeedList = myFeedSoruceLink.FindAll(delegate(string t) { return t == FeedURL; });

                // calcolo il numero di file da scaricare:
                // NumeroMassimoDiFilePerCanale - NumeroDiFileGiàInDownlaod
                int dif = MainConfig.MaxSimultaneousFeedDownloads - myFeedList.Count;

                
                // estraggo i file da scaricare del feed
                List<sDonwloadFile> tempDonwloadFileList = myList.FindAll(delegate(sDonwloadFile file) { return file.FeedSource == FeedURL; });

                List<sDonwloadFile> temp = new List<sDonwloadFile>();

                // copio la lista dei file da scaricare
                for (; dif > 0; dif--)
                {
                    temp.Add(tempDonwloadFileList[dif]);
                }

                // rimuovo tutti dalla mian list
                myList.RemoveAll(delegate(sDonwloadFile file) { return file.FeedSource == FeedURL; });

                // aggiungo i file da scaricare
                myList.AddRange(temp);


                // to remove, not usefull
                //myFeedSoruceLink.RemoveAll(delegate(string t) { return t == FeedURL; });
            }
      
            //
            //  Download file 
            // 
            foreach (sDonwloadFile DownloadFile in myList)
            {
                Ed2kParser ed2klink = new Ed2kParser(DownloadFile.Ed2kLink);
                AppendLogMessage("Add file to download", true);
                Service.AddToDownload(ed2klink, DownloadFile.Category);

                if (DownloadFile.PauseDownload == true)
                {
                    AppendLogMessage("Start download", true);
                    Service.StopDownload(ed2klink);
                }
                else
                {
                    AppendLogMessage("Stop download (pause)", true);
                    Service.StartDownload(ed2klink);
                }
                MainHistory.Add(DownloadFile.Ed2kLink, DownloadFile.FeedLink, DownloadFile.FeedSource);
                Ed2kParser parser = new Ed2kParser(DownloadFile.Ed2kLink);
                AppendLogMessage(string.Format("Add file to emule {0} \n", parser.GetFileName()) + Environment.NewLine, false);
                SendMailDownload(parser.GetFileName(), DownloadFile.Ed2kLink);

                { // progress bar
                    int progress = (myList.IndexOf(DownloadFile) + 1) * 100;
                    backgroundWorker1.ReportProgress(progress / myList.Count);
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
            UpdateRecentActivity();
            UpdateRssFeedGUI();
            menuItemCheckNow.Enabled = true;
            checkNowToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
            addToolStripMenuItem.Enabled = true;
            deleteCompleteToolStripMenuItem.Enabled = true;
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
            AppendLogMessage("Config loaded " + MainConfig.FileName, false);
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

                bool upgrade = false;

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
            AboutBox1 dialog = new AboutBox1();
            dialog.ShowDialog();

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
                MainConfig.IntervalTime = OptDialog.LocalConfig.IntervalTime;
                MainConfig.StartMinimized = OptDialog.LocalConfig.StartMinimized;
                MainConfig.StartWithWindows = OptDialog.LocalConfig.StartWithWindows;
                MainConfig.StartEmuleIfClose = OptDialog.LocalConfig.StartEmuleIfClose;
                MainConfig.CloseEmuleIfAllIsDone = OptDialog.LocalConfig.CloseEmuleIfAllIsDone;
                MainConfig.Verbose = OptDialog.LocalConfig.Verbose;
                MainConfig.ServiceUrl = OptDialog.LocalConfig.ServiceUrl;
                MainConfig.Password = OptDialog.LocalConfig.Password;
                MainConfig.DefaultCategory = OptDialog.LocalConfig.DefaultCategory;
                MainConfig.eMuleExe = OptDialog.LocalConfig.eMuleExe;
                MainConfig.IntervalTime = OptDialog.LocalConfig.IntervalTime;
                MainConfig.MinToStartEmule = OptDialog.LocalConfig.MinToStartEmule;
                MainConfig.EmailNotification = OptDialog.LocalConfig.EmailNotification;
                MainConfig.ServerSMTP = OptDialog.LocalConfig.ServerSMTP;
                MainConfig.MailSender = OptDialog.LocalConfig.MailSender;
                MainConfig.MailReceiver = OptDialog.LocalConfig.MailReceiver;
                MainConfig.AutoClearLog = OptDialog.LocalConfig.AutoClearLog;
                MainConfig.intervalBetweenUpgradeCheck = OptDialog.LocalConfig.intervalBetweenUpgradeCheck;

                MainConfig.Save();

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

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOPMLImporter newForm = new FormOPMLImporter();
            newForm.ShowDialog();
        }

        private void timer3_Tick(object sender, EventArgs e)
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
                return;
            }

            // check if Auto Close Data Time is not set
            if (AutoCloseDataTime == DateTime.MinValue)
            {
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
            timer3.Enabled = false;

            // conncect to mule

            AppendLogMessage("[AutoClose Mule] Check Login", true);
            eMuleWebManager Service = new eMuleWebManager(MainConfig.ServiceUrl, MainConfig.Password);
            bool? rc = Service.LogIn();


            // if mule close ... end of game
            if (rc == null)
            {
                AutoCloseDataTime = DateTime.Now.AddMinutes(30); // do controll every 30 minuts
                AppendLogMessage("[AutoClose Mule] Login failed", true);
                return;
            }
            AppendLogMessage("[AutoClose Mule] Login ok", true);

            AppendLogMessage("[AutoClose Mule] Actual Downloads " + Service.GetActualDownloads().Count, true);
            // if donwload > 0 ... there' s some download ... end 
            if (Service.GetActualDownloads().Count > 0)
            {
                AppendLogMessage("[AutoClose Mule] GetActualDownloads return >0", true);
                AutoCloseDataTime = DateTime.Now.AddMinutes(30);
                AppendLogMessage("[AutoClose Mule] LogOut", true);
                Service.LogOut();
                return;
            }

            AppendLogMessage("[AutoClose Mule] Show dialog ", true);
            // pop up form to advise user
            FormAlerteMuleClose Dialog = new FormAlerteMuleClose();
            Dialog.ShowDialog();

            AppendLogMessage("[AutoClose Mule] Dialog return " + Dialog.AlertChoice.ToString(), true);
            switch (Dialog.AlertChoice)
            {
                case AlertChoiceEnum.Close:// Close
                    AppendLogMessage("[AutoClose Mule: CLOSE] Close Service", true);
                    Dialog.Dispose();
                    Service.Close();
                    timer3.Enabled = true;  // enable timer 
                    break;
                // to fix here                    
                case AlertChoiceEnum.Skip: // SKIP
                    AutoCloseDataTime = DateTime.Now.AddMinutes(30); // do controll every 30 minuts
                    AppendLogMessage("[AutoClose Mule: SKIP] Skip", true);
                    AppendLogMessage("[AutoClose Mule: SKIP] Next Tock " + AutoCloseDataTime.ToString(), true);
                    Dialog.Dispose();
                    AppendLogMessage("[AutoClose Mule] LogOut", true);
                    Service.LogOut();
                    timer3.Enabled = true;  // enable timer 
                    break;
                case AlertChoiceEnum.Disable:    // disable autoclose
                    AppendLogMessage("[AutoClose Mule: DISABLE] Disable", true);
                    Dialog.Dispose();
                    AppendLogMessage("[AutoClose Mule] LogOut", true);
                    Service.LogOut();
                    DisableAutoCloseEmule();
                    timer3.Enabled = true;  // enable timer 
                    break;
            }

            if ((MainConfig.EmailNotification == true) && (sendLogToEmailToolStripMenuItem.Checked))
            {
                AppendLogMessage("[AutoClose Mule] Send Mail", true);
                string stmpServer = MainConfig.ServerSMTP;
                string EmailReceiver = MainConfig.MailReceiver;
                string EmailSender = MainConfig.MailSender;
                string Subject = "TV Underground Downloader Notification";
                string dump = SmtpClient.SendEmail(stmpServer, EmailReceiver, EmailSender, Subject, LogTextBox.Text);
            }
        }

        public void EnableAutoCloseEmule()
        {
            this.menuItemAutoCloseEmule.Checked = true;   // Enable Trybar context menu
            this.autoCloseEMuleToolStripMenuItem.Checked = true; // File -> Menu -> Configure
            this.MainConfig.CloseEmuleIfAllIsDone = true; // Enable function
            AutoCloseDataTime = DateTime.Now.AddMinutes(30);
        }

        public void DisableAutoCloseEmule()
        {
            this.menuItemAutoCloseEmule.Checked = false; // disable context menu
            this.autoCloseEMuleToolStripMenuItem.Checked = false; // File -> Menu -> Configure
            this.MainConfig.CloseEmuleIfAllIsDone = false; // disable function
            

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

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
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
            
            strOPML +="<outline text=\"tvunderground.org.ru\" title=\"tvunderground.org.ru\">"+ Environment.NewLine;

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

        private void testAutoStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(MainConfig.eMuleExe);
            }
            catch
            {
                AppendLogMessage("Unable start application", false);
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


            List<fileHistory> FileToDelete = new List<fileHistory>();

            foreach (fileHistory fh in MainHistory.fileHistoryList)
            {
                if (fh.FeedSource == Feed.Url)
                {
                    FileToDelete.Add(fh);
                }
            }

            foreach (fileHistory fh in FileToDelete)
            {
                MainHistory.fileHistoryList.Remove(fh);
            }

            MainConfig.RssFeedList.Remove(Feed);
            MainConfig.Save();
            MainHistory.Save();
            UpdateRssFeedGUI(); ///upgrade gui
        }
       
        void AddRssChannel()
        {
            AddFeedDialog dialog = new AddFeedDialog(MainConfig.ServiceUrl, MainConfig.Password, MainConfig.DefaultCategory);
            dialog.ShowDialog();

            if (dialog.DialogResult != DialogResult.OK)
            {
                dialog.Dispose();
                return;
            }

            // find rss duplicate
            foreach (RssSubscrission temp in MainConfig.RssFeedList)
            {
                if (temp.Url == dialog.NewFeed.Url)
                {
                    dialog.Dispose();
                    return;
                }

            }


            MainConfig.RssFeedList.Add(dialog.NewFeed);
            MainConfig.Save();
            foreach (fileHistory file in dialog.NewHistory)
            {
                MainHistory.Add(file.GetLink(), file.FeedLink, file.FeedSource);
            }
            MainHistory.Save();
            UpdateRssFeedGUI();
            dialog.Dispose();
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

            string p = string.Format("DELETE {0}:{1}", i, str);
            AppendLogMessage(p, false);

            MainHistory.DeleteFile(str);
        }

        private void deleteCompleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<RssSubscrission> channelToDelete = new List<RssSubscrission>();
            
            foreach (RssSubscrission channel in MainConfig.RssFeedList)
            {
                if (channel.tvuStatus == tvuStatus.Complete)
                {
                    channelToDelete.Add(channel);
                }
            }

            while (channelToDelete.Count > 0)
            {
                string channelUrl = channelToDelete[0].Url;
                MainHistory.DeleteFileByFeedSource(channelUrl);
                MainConfig.RssFeedList.Remove(channelToDelete[0]);
                
            }
            MainConfig.Save();
            MainHistory.Save();
            UpdateRssFeedGUI(); ///upgrade gui
        }
     
    
    }

   
}