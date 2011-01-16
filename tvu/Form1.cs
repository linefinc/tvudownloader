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
        private System.Windows.Forms.MenuItem menuItemHide;
        private System.Windows.Forms.MenuItem menuItemExit;
        private System.Windows.Forms.MenuItem menuItemAutoStartEmule;
        private System.Windows.Forms.MenuItem menuItemAutoCloseEmule;
        private System.Windows.Forms.MenuItem menuItemEnable;


        public Config MainConfig;
        public History MainHistory;

        delegate void SetTextCallback(string text);

        private DateTime DownloadDataTime;
        private DateTime AutoCloseDataTime;


        public class sDonwloadFile
        {
            public string FeedSource;
            public string FeedLink;
            public string Ed2kLink;
           
            public bool PauseDownload;
            public string Category;
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
            AutoCloseDataTime = DateTime.Now.AddMinutes(30);

            // load History
            MainHistory = new History();
            MainHistory.Read();

            UpdateRecentActivity();
            UpdateRssFeedGUI();


        }

        


        private void SetupNotify()
        {

            IconUp = tvu.Properties.Resources.appicon1; 
            
            IconDown = tvu.Properties.Resources.appicon2; 
            
            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItemCheckNow = new System.Windows.Forms.MenuItem();
            this.menuItemHide = new System.Windows.Forms.MenuItem();
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
                this.menuItemAutoCloseEmule.Checked = true;
            }
            else
            {
                this.menuItemAutoCloseEmule.Checked = false;
            }
            this.menuItemAutoCloseEmule.Click += new System.EventHandler(this.menu_AutoCloseEmule);
            this.contextMenu1.MenuItems.Add(menuItemAutoCloseEmule);

            this.menuItemExit.Index = 5;
            this.menuItemExit.Text = "E&xit";
            this.menuItemExit.Click += new System.EventHandler(this.menu_Exit);
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

            this.menuItemHide.Index = 4;
            this.menuItemHide.Text = "Hide";
            this.menuItemHide.Click += new System.EventHandler(this.menu_Hide);
            this.contextMenu1.MenuItems.Add(menuItemHide);

            

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
        private void menu_Hide(object Sender, EventArgs e)
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

        private void menu_CheckNow(object Sender, EventArgs e)
        {
            StartDownloadThread();
        }

        private void menu_AutoStartEmule(object Sender, EventArgs e)
        {
            AppendLogMessage("Auto Start Emule");
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

        private void menu_Exit(object Sender, EventArgs e)
        {
            mVisible = true;
            this.Visible = true;
            mAllowClose = true;
            this.Close();
            
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
                ListViewItem item = new ListViewItem(t.Title);

                switch (t.status)
                {
                    case enumStatus.Ok:
                        item.SubItems.Add("Ok");
                        break;

                    case enumStatus.Idle:
                        item.SubItems.Add("Idle");
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

                listView1.Items.Add(item);
            }
        
        }

       
      
        public static void CreateDumpFile( string textToAdd)
        {
            string ora = DateTime.Now.ToString();
            string fileName = "debug.log";
            StreamWriter swFromFile = File.AppendText(fileName);
            swFromFile.WriteLine(ora + ": " + textToAdd);
            swFromFile.Flush();
            swFromFile.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (DateTime.Now > DownloadDataTime)
            {
                if (MainConfig.AutoClearLog == true)
                {
                    TextBoxLog.Clear();
                }
                
                DownloadDataTime = DateTime.Now.AddMinutes(MainConfig.IntervalTime);

                AppendLogMessage("Now : " + DateTime.Now.ToString());
                AppendLogMessage("next tick : " + DownloadDataTime.ToString());
                StartDownloadThread();
                UpdateRssFeedGUI();
            }

            //
            // Auto close
            //
            if ((MainConfig.CloseEmuleIfAllIsDone == true) & (AutoCloseDataTime > DateTime.Now))
            {
                // set next time 
                AutoCloseDataTime = DateTime.Now.AddMinutes(30); // do controll every 30 minuts
               
                eMuleWebManager Service = new eMuleWebManager(MainConfig.ServiceUrl,MainConfig.Password);
                bool? rc = Service.LogIn();

                if (rc == null)
                {
                    return;
                }

                if (Service.GetActualDownloads().Count == 0)
                {
                    // pop up form to advise
                    FormAlerteMuleClose Dialog = new FormAlerteMuleClose();
                    Dialog.ShowDialog();

                    if (Dialog.DialogResult == DialogResult.Cancel)
                    {
                        //skeep event
                        Dialog.Dispose();
                        Service.LogOut();
                        return;
                    }

                    switch (Dialog.AlertChoice)
                    {
                        case AlertChoiceEnum.Close:// Close
                            Service.Close();
                            break;
                        case AlertChoiceEnum.Skip: // SKIP
                            AutoCloseDataTime = DateTime.Now.AddMinutes(30); // do controll every 30 minuts
                            break;
                        case AlertChoiceEnum.Disable:    // disable autoclose
                            this.menuItemAutoCloseEmule.Checked = false; // disable context menu
                            this.MainConfig.CloseEmuleIfAllIsDone = false; // disable function
                            break;
                    }
    
                    Dialog.Dispose();
                }

                Service.LogOut();
            }
        }

        private void AppendText(string text)
        {
            this.TextBoxLog.Text += text;
            this.TextBoxLog.SelectionStart = this.TextBoxLog.Text.Length;
            this.TextBoxLog.ScrollToCaret();
            this.TextBoxLog.Refresh();
        }

        private void AppendLogMessage(string text)
        {
            text = "[" + DateTime.Now.ToString() + "]" + text;

            if (text.IndexOf(Environment.NewLine) == -1)
            {
                text += Environment.NewLine;
            }
            
            // from msdn guide http://msdn.microsoft.com/en-us/library/ms171728%28VS.90%29.aspx
            if (this.TextBoxLog.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(AppendText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                // It's on the same thread, no need for Invoke
                AppendText(text);
            }
        }

        private void DownloadNow()
        {

            int counter = 0;


            AppendLogMessage("Start check");

            List<sDonwloadFile> myList = new List<sDonwloadFile>();

            if (checkBoxAutoClear.Checked == true)
            {
                TextBoxLog.Clear();
            }

            foreach (RssSubscrission feed in MainConfig.RssFeedList)
            {

                feed.status = enumStatus.Ok;
                AppendLogMessage("Read RSS " + feed.Url);

                try
                {
                    string WebPage = WebFetch.Fetch(feed.Url);

                    List<string> elemList = new List<string>();

                    while (WebPage.IndexOf("</guid>") > 0)
                    {
                        string strleft = WebPage.Substring(0, WebPage.IndexOf("</guid>"));
                        string guid = strleft.Substring(strleft.IndexOf("<guid>") + ("<guid>").Length);

                        WebPage = WebPage.Substring(WebPage.IndexOf("</guid>") + ("</guid>").Length);
                        guid = guid.Replace("&amp;", "&");
                        elemList.Add(guid);
                    }

                    foreach (string FeedLink in elemList)
                    {
                        if (MainHistory.FileExistByFeedLink(FeedLink) == false)
                        {
                            // download the page in FeedLink
                            string page = eMuleWebManager.DownloadPage(FeedLink);
                            // find ed2k
                            string sEd2k = RssParserTVU.FindEd2kLink(page);

                            Ed2kParser parser = new Ed2kParser(sEd2k);
                            AppendLogMessage(string.Format("Found new file {0}", parser.GetFileName()));

                            sDonwloadFile DL = new sDonwloadFile();
                            DL.FeedSource = feed.Url;
                            DL.FeedLink = FeedLink;
                            DL.Ed2kLink = sEd2k;
                            DL.PauseDownload = feed.PauseDownload;
                            DL.Category = feed.Category;

                            myList.Add(DL);
                        }
                    }



                }
                catch
                {
                    AppendLogMessage("Unable to load rss");

                    feed.status = enumStatus.Error;
                    //int index = MainConfig.RssFeedList.IndexOf(feed);
                    //MainConfig.RssFeedList[index].status = enumStatus.Error;
                }

                if (feed.status == enumStatus.Ok)
                {
                    string strDate = MainHistory.LastDownloadDateByFeedSource(feed.Url);
                    if (strDate == "")
                    {
                        strDate = DateTime.Now.ToString();
                    }
                    DateTime t = DateTime.Parse(strDate);
                    TimeSpan span = DateTime.Now.Subtract(t);
                    if (span.TotalDays > 15)
                    {
                        feed.status = enumStatus.Idle;
                    }
                }

                //update rss feed
                feed.LastUpgradeDate = MainHistory.LastDownloadDateByFeedSource(feed.Url);
                feed.TotalDownloads = MainHistory.LinkCountByFeedSource(feed.Url);
                int progress = (int)(++counter * 100.0f / MainConfig.RssFeedList.Count);
                backgroundWorker1.ReportProgress(progress);
                

            }



            if (myList.Count == 0)
            {
                // nothing to download
                return;
            }

            eMuleWebManager Service = new eMuleWebManager(MainConfig.ServiceUrl, MainConfig.Password);
            bool? rc = Service.LogIn();

            // try to start emule
            // the if work only if rc == null
            if (MainConfig.StartEmuleIfClose == true)
            {
                for (int i = 1; (i <= 5) & (rc == null); i++)
                {
                    AppendLogMessage(string.Format("start eMule Now (try {0}/5)", i));
                    AppendLogMessage("Wait 60 sec");
                    try
                    {

                        Process.Start(MainConfig.eMuleExe);
                    }
                    catch
                    {
                        AppendLogMessage("Unable start application");
                    }
                    Thread.Sleep(5000);
                    rc = Service.LogIn();

                }
            }

            if (rc == null)
            {
                AppendLogMessage("Unable to connect to eMule web server");
                return;
            }



            //
            //  Download file 
            // 

            Service.GetCategory(true);  // force upgrade category list 
            
            //reset counter 
            counter = 0;
            foreach (sDonwloadFile DownloadFile in myList)
            {
                if (MainHistory.ExistInHistoryByEd2k(DownloadFile.Ed2kLink) == false) // if file is not dwnl
                {
                    Ed2kParser ed2klink = new Ed2kParser(DownloadFile.Ed2kLink);
                    Service.AddToDownload(ed2klink, DownloadFile.Category);

                    if (DownloadFile.PauseDownload == true)
                    {
                        Service.StopDownload(ed2klink);
                    }
                    else
                    {
                        Service.StartDownload(ed2klink);
                    }
                    MainHistory.Add(DownloadFile.Ed2kLink, DownloadFile.FeedLink, DownloadFile.FeedSource);
                    Ed2kParser parser = new Ed2kParser(DownloadFile.Ed2kLink);
                    AppendLogMessage(string.Format("Add file to emule {0} \n", parser.GetFileName()) + Environment.NewLine);
                }
                backgroundWorker1.ReportProgress((int)(++counter * 100.0f / myList.Count));
            }
            MainHistory.Save();
            Service.LogOut();
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

            RssSubscrission Feed = MainConfig.RssFeedList[0]; ;

            for (i = 0; i < listView1.Items.Count; i++)
            {
                if (MainConfig.RssFeedList[i].Title == feedTitle)
                {
                    Feed = MainConfig.RssFeedList[i];
                }
            }


            label8.Text = Feed.Category;
            label10.Text = Feed.PauseDownload.ToString();
            label12.Text = Feed.Url;

            label14.Text = MainHistory.LastDownloadDateByFeedSource(Feed.Url);
            label16.Text = MainHistory.LinkCountByFeedSource(Feed.Url).ToString();
   

            // update list history
            listView2.Items.Clear();

            List<string> ListHistory = new List<string>();

            foreach (fileHistory fh in MainHistory.fileHistoryList)
            {
                if (fh.FeedSource == Feed.Url)
                {
                    Ed2kParser ed2k = new Ed2kParser(fh.Ed2kLink);
                    ListHistory.Add(ed2k.GetFileName());
                
                }
            }

            ListHistory.Sort((x, y) => (y.CompareTo(x)));

            foreach (string t in ListHistory)
            {
                ListViewItem item1 = new ListViewItem(t);
                listView2.Items.Add(item1);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {

        //    if (listView1.Items.Count == 0)
        //        return;

        //    if (listView1.SelectedItems.Count == 0)
        //        return;

        //    ListViewItem temp = listView1.SelectedItems[0];
        //    int i = listView1.Items.IndexOf(temp);
        //    string feedTitle = listView1.Items[i].Text;

        //    RssSubscrission Feed = MainConfig.RssFeedList[0]; ;

        //    for (i = 0; i < listView1.Items.Count; i++)
        //    {
        //        if (MainConfig.RssFeedList[i].Title == feedTitle)
        //        {
        //            Feed = MainConfig.RssFeedList[i];
        //        }
        //    }
            
        //    AddFeedDialog dialog = new AddFeedDialog(MainConfig.ServiceUrl, MainConfig.Password, Feed);
        //    dialog.ShowDialog();

        //    if (dialog.DialogResult == DialogResult.OK)
        //    {
        //        Feed = dialog.NewFeed;
                
        //        for (int j = 0; j < MainConfig.RssFeedList.Count; j++)
        //        {
        //            if (MainConfig.RssFeedList[j].Url == Feed.Url)
        //            {
        //                MainConfig.RssFeedList.Remove(MainConfig.RssFeedList[j]);
        //                break;
        //            }
        //        }

        //        MainConfig.RssFeedList.Add(Feed);
        //        UpdateRssFeedGUI();
        //        dialog.Dispose();
        //        return;
        //    }

        //    dialog.Dispose();
        //    return;
        }


        private void ClearButton_Click(object sender, EventArgs e)
        {
            TextBoxLog.Clear();
        }



        private void StartDownloadThread()
        {
            if (backgroundWorker1.IsBusy == true)
            {
                AppendLogMessage("Thread is busy");
                return;
            }
            CheckButton.Enabled = false;
            button4.Enabled = false;
            button1.Enabled = false;
            menuItemCheckNow.Enabled = false;
            backgroundWorker1.RunWorkerAsync();

        }

        public static string GetUserAppDataPath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path += '\\' + Application.ProductName + '\\';
            return path;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            //if (listView1.Items.Count == 0)
            //    return;

            //if (listView1.SelectedItems.Count == 0)
            //    return;

            //ListViewItem temp = listView1.SelectedItems[0];
            //int i = listView1.Items.IndexOf(temp);

            //MainConfig.RssFeedList.Remove(MainConfig.RssFeedList[i]);
            //MainConfig.Save();
            //UpdateRssFeedGUI(); ///upgrade gui
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
                if (MainConfig.RssFeedList[i].Title == feedTitle)
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
            
            MainConfig.RssFeedList.Remove(Feed);
            MainConfig.Save();
            UpdateRssFeedGUI(); ///upgrade gui


            
            

            

        }

        private void button1_Click(object sender, EventArgs e)
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
                if (dialog.NewFeed.Url == dialog.NewFeed.Url)
                {
                    dialog.Dispose();
                    return;
                }
            }

            MainConfig.RssFeedList.Add(dialog.NewFeed);
            MainConfig.Save();
            MainHistory.fileHistoryList.AddRange(dialog.NewHistory.fileHistoryList);
            MainHistory.Save();
            UpdateRssFeedGUI();
            dialog.Dispose();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DownloadNow();
        }

        /// <summary>
        /// This is on the main thread, so we can update a TextBox or anything.
        /// </summary>
        private void backgroundWorker1_RunWorkerCompleted(object sender,RunWorkerCompletedEventArgs e)
        {
            UpdateRecentActivity();
            UpdateRssFeedGUI();
            menuItemCheckNow.Enabled = true;
            CheckButton.Enabled = true;
            button4.Enabled = true;
            button1.Enabled = true;
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
            if (this.Visible == true)
            {
                this.menuItemHide.Text = "Hide";
            }
            else
            {
                this.menuItemHide.Text = "Show";
            }

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
            AppendLogMessage("Config loaded " + MainConfig.FileName);
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
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("http://tvudownloader.sourceforge.net/version.xml");

                string lastVersion = "";
                
                foreach( XmlNode t in doc.GetElementsByTagName("last"))
                {
                    lastVersion = t.InnerText;
                }

                if (String.Compare(lastVersion, Config.Version) == 1)
                {
                    return true;
                }


            }
            catch 
            { 
                return false; 
            }
            return false;
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
                Ed2kParser parser = new Ed2kParser(fh.Ed2kLink);
                fh.Date = fh.Date.Replace('T', ' ');
                string t = string.Format("{0}:{1}", fh.Date, parser.GetFileName());
                listBox1.Items.Add(t);
            }

        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            StartDownloadThread();
        }

        private void buttonOption_Click(object sender, EventArgs e)
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
                MainConfig.ServiceUrl = OptDialog.LocalConfig.ServiceUrl;
                MainConfig.Password = OptDialog.LocalConfig.Password;
                MainConfig.DefaultCategory = OptDialog.LocalConfig.DefaultCategory;
                MainConfig.eMuleExe = OptDialog.LocalConfig.eMuleExe;
                MainConfig.IntervalTime = OptDialog.LocalConfig.IntervalTime;
                MainConfig.Save();
            }
            // reset counter to avoid error
            AutoCloseDataTime = DateTime.Now.AddMinutes(30); // do controll every 30 minuts
            
            OptDialog.Dispose();
            return;
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.Items.Count == 0)
                return;

            if (listView2.SelectedItems.Count == 0)
                return;

            ListViewItem temp = listView2.SelectedItems[0];
            int i = listView2.Items.IndexOf(temp);
            string str = listView2.Items[i].Text;

            string p = string.Format("DELETE {0}:{1}", i, str);
            AppendLogMessage(p);

            MainHistory.DeleteFile(str);
        }


        private void label5_Paint(object sender, PaintEventArgs e)
        {
            label5.Text = "Version " + Config.Version;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }









  

    }

   
}
