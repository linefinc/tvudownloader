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

        public Config MainConfig;
        public History MainHistory;

        delegate void SetTextCallback(string text);

        private DateTime DownloadDataTime;


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
            
            LoadConfigToGUI();
            SetupNotify();

            DownloadDataTime = DateTime.Now.AddMinutes(MainConfig.IntervalTime);
            
            // load History
            MainHistory = new History();
            MainHistory.Read();


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

            // Initialize menuItem1
            this.menuItemCheckNow.Index = 0;
            this.menuItemCheckNow.Text = "Check Now";
            this.menuItemCheckNow.Click += new System.EventHandler(this.menu_CheckNow);
            this.contextMenu1.MenuItems.Add(menuItemCheckNow);

            this.menuItemHide.Index = 1;
            this.menuItemHide.Text = "Hide";
            this.menuItemHide.Click += new System.EventHandler(this.menu_Hide);
            this.contextMenu1.MenuItems.Add(menuItemHide);

            this.menuItemExit.Index = 0;
            this.menuItemExit.Text = "E&xit";
            this.menuItemExit.Click += new System.EventHandler(this.menu_Exit);
            this.contextMenu1.MenuItems.Add(menuItemExit);

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

        private void LoadConfigToGUI()
        {

            while (listView1.Items.Count > 0)
            {
                listView1.Items.Remove(listView1.Items[0]);
            }

            MainConfig.Load();     

            ServiceUrlTextBox.Text = MainConfig.ServiceUrl;
            PasswordTextBox.Text = MainConfig.Password;
            textBoxEmuleExec.Text = MainConfig.eMuleExe;
            numericUpDown1.Value = MainConfig.IntervalTime;
            checkBoxStartMinimized.Checked = MainConfig.StartMinimized;
            checkBoxCloseWhenAllDone.Checked = MainConfig.CloseWhenAllDone;
            checkBoxStartWithWindows.Checked = MainConfig.StartWithWindows;

            foreach (RssFeed t in MainConfig.RssFeedList)
            {
                ListViewItem item1 = new ListViewItem(t.Title);
                item1.SubItems.Add(t.Category.ToString());
                item1.SubItems.Add(t.PauseDownload.ToString());
                item1.SubItems.Add(t.Url);
                listView1.Items.Add(item1);
            }
        
        }

        private string findEd2kLink(string text)
        {
            //
            int i = text.IndexOf("ed2k://|file|");
            text = text.Substring(i);
            i = text.IndexOf("|/");
            text = text.Substring(0, i + "|/".Length);
            return text;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            eMuleWebManager service = new eMuleWebManager(ServiceUrlTextBox.Text, PasswordTextBox.Text);
            bool? rc = service.LogIn();

            if (rc == null)
            {
                MessageBox.Show("Unable conncet with target URL");
                return;
            }
            
            if(rc == false)
            {
                MessageBox.Show("Password error ");
                return;
            }

            MessageBox.Show("OK service is correctly configured");
            
            return;
            


        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            MainConfig.ServiceUrl = ServiceUrlTextBox.Text;
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


       

        public static bool ExistInHistoryByEd2k(string ed2k)
        {
            string HistoryFileName = Application.LocalUserAppDataPath;
            int rc = HistoryFileName.LastIndexOf("\\");
            HistoryFileName = HistoryFileName.Substring(0, rc) + "\\History.xml";
            
            if (!File.Exists(HistoryFileName))
            {
                return false;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(HistoryFileName);

            XmlNodeList elemList = doc.GetElementsByTagName("Ed2k");

        
            for (int i = 0; i < elemList.Count; i++)
            {

                Ed2kParser A = new Ed2kParser(elemList[i].InnerXml);
                Ed2kParser B = new Ed2kParser(ed2k);

                if (A == B)
                    return true;
                

            }
            return false;
        }

        


        
    
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            MainConfig.Password = PasswordTextBox.Text;
        }

        private void ButtonSaveConfig_Click(object sender, EventArgs e)
        {
            MainConfig.AutoStartEmule = checkBoxAutoStartEmule.Checked;
            MainConfig.StartMinimized = checkBoxStartMinimized.Checked;
            MainConfig.CloseWhenAllDone = checkBoxCloseWhenAllDone.Checked;
            MainConfig.Save();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            
            

            if ( DateTime.Now > DownloadDataTime)
            {
                DownloadDataTime = DateTime.Now.AddMinutes(MainConfig.IntervalTime);

                AppendLogMessage("Now : " + DateTime.Now.ToString());
                AppendLogMessage("next tick : " + DownloadDataTime.ToString());
                StartDownloadThread();
            }

        }

        private void AppendText(string text)
        {
            this.LogTextBox.Text += text;
        }

        private void AppendLogMessage(string text)
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
                this.Invoke(d, new object[] { text });
            }
            else
            {
                // It's on the same thread, no need for Invoke
                this.LogTextBox.Text += text;
            }
        }

        private void DownloadNow()
        {
            
            AppendLogMessage("Start check");

            List<sDonwloadFile> myList = new List<sDonwloadFile>();

                
            foreach (RssFeed feed in MainConfig.RssFeedList)
            {
                
                
                AppendLogMessage("Read RSS " + feed.Url);

                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(feed.Url);

                    XmlNodeList elemList = doc.GetElementsByTagName("guid");

                    for (int i = 0; i < elemList.Count; i++)
                    {
                        string FeedLink = elemList[i].InnerXml;

                        FeedLink = FeedLink.Replace("&amp;", "&");

                        if (MainHistory.FileExistByFeedLink(FeedLink) == false)
                        {
                            string page = eMuleWebManager.DownloadPage(elemList[i].InnerXml);
                            string sEd2k = findEd2kLink(page);

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
                }
            }

            if ((myList.Count == 0)&(MainConfig.CloseWhenAllDone == false))
            {
                // nothing to download
                return;
            }

            // for future separation in thread
            string sUrl = ServiceUrlTextBox.Text;
            string sPass = PasswordTextBox.Text;
            

            eMuleWebManager Service = new eMuleWebManager(sUrl, sPass);
            bool? rc = Service.LogIn();

            // try to start emule
            // the if work only if rc == null
            if (MainConfig.AutoStartEmule == true)
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

            if(rc==null)
            {
                  AppendLogMessage("Unable to connect to eMule web server" );
                  return;
            }


            //
            // Auto close
            //
            if ((myList.Count == 0) & (checkBoxCloseWhenAllDone.Checked == false))
            {
                List<string> ActualDownloads = Service.GetActualDownloads();
                if (ActualDownloads.Count == 0)
                {
                    Service.Close();
                }
                return;
            }
            //
            //  Download file 
            // 

            Service.GetCategory(true);  // force upgrade category list 

            foreach (sDonwloadFile DownloadFile in myList)
            {
                if (ExistInHistoryByEd2k(DownloadFile.Ed2kLink) == false) // if file is not dwnl
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
             }
            Service.LogOut();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
                return;

            if (listView1.SelectedItems.Count == 0)
                return;

            string HistoryFileName = Application.LocalUserAppDataPath;
            int rc = HistoryFileName.LastIndexOf("\\");
            HistoryFileName = HistoryFileName.Substring(0, rc) + "\\History.xml";

            if (!File.Exists(HistoryFileName))
            {
                return ;
            }

            listView2.Items.Clear();

            ListViewItem temp = listView1.SelectedItems[0];
            int i = listView1.Items.IndexOf(temp);
            string feedUrl = MainConfig.RssFeedList[i].Url;
            
            XmlDocument doc = new XmlDocument();
            doc.Load(HistoryFileName);

            XmlNodeList FeedLinkList = doc.GetElementsByTagName("FeedLink");
            XmlNodeList Ed2kList = doc.GetElementsByTagName("Ed2k");
            XmlNodeList FeedSourceList = doc.GetElementsByTagName("FeedSource");

            for (i = 0; i < FeedSourceList.Count; i++)
            {
                if (FeedSourceList[i].FirstChild.InnerText == feedUrl)
                {
                    string str = Ed2kList[i].FirstChild.InnerText;
                    Ed2kParser parser = new Ed2kParser (str);

                    ListViewItem item1 = new ListViewItem(parser.GetFileName());
                    listView2.Items.Add(item1);
                }
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
                return;

            if (listView1.SelectedItems.Count == 0)
                return;

            string HistoryFileName = Application.LocalUserAppDataPath;
            int rc = HistoryFileName.LastIndexOf("\\");
            HistoryFileName = HistoryFileName.Substring(0, rc) + "\\History.xml";

            if (!File.Exists(HistoryFileName))
            {
                return ;
            }

            ListViewItem temp = listView1.SelectedItems[0];
            int i = listView1.Items.IndexOf(temp);
            RssFeed feed = MainConfig.RssFeedList[i];


            AddFeedDialog dialog = new AddFeedDialog(MainConfig.ServiceUrl, MainConfig.Password, feed);
            dialog.ShowDialog();

            if (dialog.DialogResult == DialogResult.OK)
            {
                feed = dialog.Feed;

                for (int j = 0; j < MainConfig.RssFeedList.Count; j++)
                {
                    if (MainConfig.RssFeedList[j].Url == feed.Url)
                    {
                        MainConfig.RssFeedList.Remove(MainConfig.RssFeedList[j]);
                        break;
                    }
                }

                MainConfig.RssFeedList.Add(feed);
                LoadConfigToGUI();
                dialog.Dispose();
                return;
            }

            dialog.Dispose();
            return;
        }


        private void ServiceUrlTextBox_TextChanged(object sender, EventArgs e)
        {
            MainConfig.ServiceUrl = ServiceUrlTextBox.Text;
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            MainConfig.Password = PasswordTextBox.Text;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            LogTextBox.Clear();
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            StartDownloadThread();
        }

        private void StartDownloadThread()
        {
            if (backgroundWorker1.IsBusy == true)
            {
                AppendLogMessage("Thread is busy");
                return;
            }
            CheckButton.Enabled = false;
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
            if (listView1.Items.Count == 0)
                return;

            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem temp = listView1.SelectedItems[0];
            int i = listView1.Items.IndexOf(temp);

            MainConfig.RssFeedList.Remove(MainConfig.RssFeedList[i]);
            MainConfig.Save();
            LoadConfigToGUI(); ///upgrade gui

            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddFeedDialog dialog = new AddFeedDialog(MainConfig.ServiceUrl, MainConfig.Password);
            dialog.ShowDialog();

            if (dialog.DialogResult == DialogResult.OK)
            {
                RssFeed feed = dialog.Feed;
                MainConfig.RssFeedList.Add(feed);
                MainConfig.Save();
            }

            dialog.Dispose();
            LoadConfigToGUI();
            return;
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
            menuItemCheckNow.Enabled = true;
            CheckButton.Enabled = true;
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
                this.contextMenu1.MenuItems[1].Text = "Hide";
            }
            else
            {
                this.contextMenu1.MenuItems[1].Text = "Show";
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

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            MainConfig.eMuleExe = textBoxEmuleExec.Text;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            MainConfig.IntervalTime = Convert.ToInt32(numericUpDown1.Value.ToString());
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxStartWithWindows.Checked == true)
            {
                MainConfig.StartWithWindows = true;
                return;
            }
            MainConfig.StartWithWindows = false;
            

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://linefinc.blogspot.com"); 
        }








  

    }

   
}
