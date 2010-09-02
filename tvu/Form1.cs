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

using tvu;

namespace tvu
{
    public partial class Form1 : Form
    {
        private string Password;
        private string ServiceUrl;
        public int IntervalTime;
        private bool StartMinimized;

        private bool mVisible = true; 
        private bool mAllowClose;

        private Icon IconUp;
        private Icon IconDown;

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItemCheckNow;
        private System.Windows.Forms.MenuItem menuItemHide;
        private System.Windows.Forms.MenuItem menuItemExit;

        delegate void SetTextCallback(string text);

        private DateTime DateTime2;

        public class sDonwloadFile
        {
            public string FeedSource;
            public string FeedLink;
            public string Ed2kLink;
           
            public bool PauseDownload;
            public string Category;
        };


        public List<RssFeed> RssFeedList;
        
        public Form1()
        {


            RssFeedList = new List<RssFeed>();
            
            InitializeComponent();
            InitConfig();
            LoadConfig();
            SetupNotify();

            DateTime2 = DateTime.Now;

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
            notifyIcon1.Text = "Form1 (NotifyIcon example)";
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
            CheckNow();
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

        private void LoadConfig()
        {

            while (listView1.Items.Count > 0)
            {
                listView1.Items.Remove(listView1.Items[0]);
            }

            RssFeedList.Clear();
        
            XmlDocument xDoc = new XmlDocument(); 
            xDoc.Load("config.xml");

            XmlNodeList ServiceUrlNode = xDoc.GetElementsByTagName("ServiceUrl");
            ServiceUrl = ServiceUrlNode[0].InnerText;
            ServiceUrlTextBox.Text = ServiceUrl;

            XmlNodeList PasswordNode = xDoc.GetElementsByTagName("Password");
            Password = PasswordNode[0].InnerText;
            PasswordTextBox.Text = Password;

            XmlNodeList IntervalTimeNode = xDoc.GetElementsByTagName("IntervalTime");
            IntervalTime = (int)Convert.ToInt32(IntervalTimeNode[0].InnerText);
            numericUpDown1.Value = IntervalTime;

            XmlNodeList StartMinimizedNode = xDoc.GetElementsByTagName("StartMinimized");
            StartMinimized = (bool)Convert.ToBoolean(StartMinimizedNode[0].InnerText);
            checkBoxStartMinimized.Checked = StartMinimized;
            
            numericUpDown1.Value = IntervalTime;           

            XmlNodeList Channels = xDoc.GetElementsByTagName("Channel");



            for (int i = 0; i < Channels.Count; i++)
            {


                XmlNodeList child = Channels[i].ChildNodes;
                RssFeed newfeed = new RssFeed();
                foreach (XmlNode t in child)
                {


                    if (t.Name == "Title")
                    {
                        newfeed.Title = t.FirstChild.Value;
                    }

                    if(t.Name=="Url")
                    {
                            newfeed.Url = t.FirstChild.Value;
                    }

                    if (t.Name == "Pause")
                    {
                        newfeed.PauseDownload = Convert.ToBoolean(t.FirstChild.Value);
                    }

                    if (t.Name == "Category")
                    {
                        newfeed.Category = t.FirstChild.Value;
                    }

                    
                }

                RssFeedList.Add(newfeed);
                
                

            }

            foreach (RssFeed t in RssFeedList)
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
            ServiceUrl = ServiceUrlTextBox.Text;
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


        public static void AddToXmlHostory(string ed2k, string FeedLink, string FeedSource)
        {
            if (!File.Exists("History.xml"))
            {

                XmlTextWriter textWritter = new XmlTextWriter("History.xml", null);
                textWritter.WriteStartDocument();
                textWritter.WriteStartElement("History");
                textWritter.WriteEndElement();

                textWritter.Close();
            }



            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load("History.xml");

            //Item
            {
                XmlElement Element1 = xmlDoc.CreateElement("Item");
                XmlElement Element2 = xmlDoc.CreateElement("Ed2k");
                XmlElement Element3 = xmlDoc.CreateElement("FeedLink");
                XmlElement Element4 = xmlDoc.CreateElement("FeedSource");

                Element2.AppendChild(xmlDoc.CreateTextNode(ed2k));
                Element3.AppendChild(xmlDoc.CreateTextNode(FeedLink));
                Element4.AppendChild(xmlDoc.CreateTextNode(FeedSource));

                Element1.AppendChild(Element2);
                Element1.AppendChild(Element3);
                Element1.AppendChild(Element4);

                xmlDoc.DocumentElement.AppendChild(Element1);
            }
            

            xmlDoc.Save("History.xml");
        }


        public static bool ExistInHistoryByEd2k(string ed2k)
        {
            if (!File.Exists("History.xml"))
            {
                return false;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load("history.xml");

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

        public static bool ExistInHistoryByFeedLink(string link)
        {
            if (!File.Exists("History.xml"))
            {
                return false;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load("history.xml");

            XmlNodeList elemList = doc.GetElementsByTagName("FeedLink");


            for (int i = 0; i < elemList.Count; i++)
            {
                string temp = elemList[i].InnerXml;
                temp = temp.Replace("&amp;","&");
                if (temp == link)
                    return true;
                
                
            }
            return false;
        }


        public void InitConfig()
        {
            if (!File.Exists("Config.xml"))
            {

                XmlTextWriter textWritter = new XmlTextWriter("Config.xml", null);
                textWritter.WriteStartDocument();
                textWritter.WriteStartElement("Config");
                textWritter.WriteStartElement("ServiceUrl");
                
                textWritter.WriteString("http://localhost:4000");
                                
                textWritter.WriteEndElement();
                textWritter.WriteStartElement("Password");
                textWritter.WriteString("password");
                textWritter.WriteEndElement();
                textWritter.WriteStartElement("IntervalTime");
                textWritter.WriteString("30");
                textWritter.WriteEndElement();
                textWritter.WriteStartElement("StartMinimized");
                textWritter.WriteString("false");
                textWritter.WriteEndElement();
                textWritter.WriteStartElement("RSSChannel");
                textWritter.WriteEndElement();

                textWritter.Close();
            }

        }

        public void AddRssToXMLConfig(string Title, string url, bool DonwloadInPause, string Category)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load("Config.xml");

            //Item
            XmlElement ChannelElementItem = xmlDoc.CreateElement("Channel");
            XmlElement TitleElementItem = xmlDoc.CreateElement("Title");
            XmlElement UrlElementItem = xmlDoc.CreateElement("Url");
            XmlElement PauseElementItem = xmlDoc.CreateElement("Pause");
            XmlElement CategoryElementItem = xmlDoc.CreateElement("Category");

            XmlText TitleXmlText = xmlDoc.CreateTextNode(Title);
            XmlText UrlXmlText = xmlDoc.CreateTextNode(url);
            XmlText PauseXmlText = xmlDoc.CreateTextNode(DonwloadInPause.ToString());
            XmlText CategoryXmlText = xmlDoc.CreateTextNode(Category);

            TitleElementItem.AppendChild(TitleXmlText);
            UrlElementItem.AppendChild(UrlXmlText);
            PauseElementItem.AppendChild(PauseXmlText);
            CategoryElementItem.AppendChild(CategoryXmlText);

            ChannelElementItem.AppendChild(TitleElementItem);
            ChannelElementItem.AppendChild(UrlElementItem);
            ChannelElementItem.AppendChild(PauseElementItem);
            ChannelElementItem.AppendChild(CategoryElementItem);

            XmlNodeList temp = xmlDoc.GetElementsByTagName("RSSChannel");

            temp[0].AppendChild(ChannelElementItem);

            xmlDoc.Save("Config.xml");

        }

        public void DeleteFromXMLConfig(string url)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load("Config.xml");

            XmlNodeList temp = xmlDoc.GetElementsByTagName("Url");
            foreach (XmlNode node in temp)
            {
                if (node.FirstChild.Value == url)
                {
                    XmlNode ParentChannel = node.ParentNode; // url -> channel
                    XmlNode RSSChannel = ParentChannel.ParentNode; // url -> RSSChannel
                    RSSChannel.RemoveChild(ParentChannel);
                    xmlDoc.Save("Config.xml");
                    return;

                }

            }
            return;
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Password = PasswordTextBox.Text;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveConfiguration(this.ServiceUrl, this.Password, (int) numericUpDown1.Value );
        }


        private void SaveConfiguration(string ServiceUrl, string ServicePassword, int IntervalTime)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load("Config.xml");

            XmlNodeList temp = xmlDoc.GetElementsByTagName("Password");
            temp[0].FirstChild.Value = ServicePassword;

            temp = xmlDoc.GetElementsByTagName("ServiceUrl");
            temp[0].FirstChild.Value = ServiceUrl;

            temp = xmlDoc.GetElementsByTagName("IntervalTime");
            temp[0].FirstChild.Value = IntervalTime.ToString();

            temp = xmlDoc.GetElementsByTagName("StartMinimized");
            if (checkBoxStartMinimized.Checked == true)
            {
                temp[0].FirstChild.Value = "true";
            }
            else
            {
                temp[0].FirstChild.Value = "false";
            }

            xmlDoc.Save("Config.xml");

            


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime DateTime1 = DateTime.Now;
            TimeSpan diff = DateTime1.Subtract(DateTime2);
            if (diff.Minutes > numericUpDown1.Value)
            {
                DateTime2 = DateTime.Now;
                DownloadNow();
            }
        }

        public static string ApplicationConfigPath()
        {
            string appPath;
            appPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            return appPath;

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
            if (this.textBox1.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(AppendText);
                this.Invoke
                    (d, new object[] { text });
            }
            else
            {
                // It's on the same thread, no need for Invoke
                this.textBox1.Text = text;
            }
        }

        private void DownloadNow()
        {
            
            AppendLogMessage("Start check");

            List<sDonwloadFile> myList = new List<sDonwloadFile>();

                
            foreach (RssFeed feed in RssFeedList)
            {
                
                AppendLogMessage("Read RSS " + feed.Url + Environment.NewLine );
    
                XmlDocument doc = new XmlDocument();
                doc.Load(feed.Url);

                XmlNodeList elemList = doc.GetElementsByTagName("guid");
                
                for (int i = 0; i < elemList.Count; i++)
                {
                    string FeedLink = elemList[i].InnerXml;

                    FeedLink = FeedLink.Replace("&amp;", "&");
                    //AppendLogMessage(string.Format("Process feed {0} \n", FeedLink));

                    if (ExistInHistoryByFeedLink(FeedLink) == false)
                    {
                        string page = eMuleWebManager.DownloadPage(elemList[i].InnerXml);
                        string sEd2k = findEd2kLink(page);

                        Ed2kParser parser = new Ed2kParser(sEd2k);
                        AppendLogMessage(string.Format("Found new file {0} \n", parser.GetFileName()) + Environment.NewLine);

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

            if (myList.Count == 0)
            {
                // nothing to download
                return;
            }

            // for future separation in thread
            string sUrl = ServiceUrlTextBox.Text;
            string sPass = PasswordTextBox.Text;
            

            eMuleWebManager Service = new eMuleWebManager(sUrl, sPass);
            bool? rc = Service.LogIn();


            if (rc == null) 
            {
                if (checkBox2.Checked == true)
                {
                    Process.Start(textBox1.Text);
                    LogTextBox.AppendText("Start eMule app" + Environment.NewLine);
                    return;
                }
                else
                {
                    LogTextBox.AppendText("Unable to connect to eMule web server" + Environment.NewLine);
                    return;
                }
                
            }


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
                    AddToXmlHostory(DownloadFile.Ed2kLink, DownloadFile.FeedLink, DownloadFile.FeedSource);
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

            if (!File.Exists("History.xml"))
            {
                return ;
            }

            listView2.Items.Clear();

            ListViewItem temp = listView1.SelectedItems[0];
            int i = listView1.Items.IndexOf(temp);
            string feedUrl = RssFeedList[i].Url;
            
            XmlDocument doc = new XmlDocument();
            doc.Load("history.xml");

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

            if (!File.Exists("History.xml"))
            {
                return ;
            }

            ListViewItem temp = listView1.SelectedItems[0];
            int i = listView1.Items.IndexOf(temp);
            RssFeed feed = RssFeedList[i];


            AddFeedDialog dialog = new AddFeedDialog(ServiceUrl, Password, feed);
            dialog.ShowDialog();

            if (dialog.DialogResult == DialogResult.OK)
            {
                feed = dialog.Feed;
                DeleteFromXMLConfig(feed.Url);
                AddRssToXMLConfig(feed.Title, feed.Url, feed.PauseDownload, feed.Category);
                LoadConfig();
                dialog.Dispose();
                return;
            }

            dialog.Dispose();
            return;
        }


        private void ServiceUrlTextBox_TextChanged(object sender, EventArgs e)
        {
            ServiceUrl = ServiceUrlTextBox.Text;
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            Password = PasswordTextBox.Text;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            LogTextBox.Clear();
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            CheckNow();
        }

        private void CheckNow()
        {
            CheckButton.Enabled = false;
            menuItemCheckNow.Enabled = false;
            backgroundWorker1.RunWorkerAsync(LogTextBox);
  
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
            DeleteFromXMLConfig(RssFeedList[i].Url);
            LoadConfig();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddFeedDialog dialog = new AddFeedDialog(ServiceUrl,Password);
            dialog.ShowDialog();

            if (dialog.DialogResult == DialogResult.OK)
            {
                RssFeed feed = dialog.Feed;
                AddRssToXMLConfig(feed.Title, feed.Url, feed.PauseDownload, feed.Category);
                LoadConfig();
                return;
            }

            dialog.Dispose();
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

        private void Form1_Load(object sender, EventArgs e)
        {
            DownloadNow();
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
            timer2.Enabled = false;

            
            if (StartMinimized == true)
            {
                mVisible = false;
                this.Visible = false;
            }
            CheckNow();

        }

    }

   
}
