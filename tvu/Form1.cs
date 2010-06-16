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

using tvu;

namespace tvu
{
   
    public partial class Form1 : Form
    {
        private string Password;
        private string ServiceUrl;
        public int IntervalTime;

        private Icon IconUp;
        private Icon IconDown;

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;


        public List<RssFeed> RssFeedList;

        public Form1()
        {


            RssFeedList = new List<RssFeed>();
            
            InitializeComponent();
            InitConfig();
            LoadConfig();

            SetupNotify();






        }


        private void SetupNotify()
        {

            IconUp = tvu.Properties.Resources.appicon1; 
            
            IconDown = tvu.Properties.Resources.appicon2; 
            
            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();

            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.menuItem1, this.menuItem2 });

            // Initialize menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

            this.menuItem2.Index = 1;
            this.menuItem2.Text = "Hide";
            this.menuItem2.Click += new System.EventHandler(this.menuItem1_Prova_Click);


            // Create the NotifyIcon.
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);

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
        private void menuItem1_Prova_Click(object Sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                this.Hide();
                this.contextMenu1.MenuItems[1].Text = "Show";
                notifyIcon1.Icon = IconDown;

            }
            else
            {
                this.Show();
                this.contextMenu1.MenuItems[1].Text = "Hide";
                notifyIcon1.Icon = IconUp;
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

        private void menuItem1_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
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
            textBox2.Text = ServiceUrl;

            XmlNodeList PasswordNode = xDoc.GetElementsByTagName("Password");
            Password = PasswordNode[0].InnerText;
            textBox3.Text = Password;

            XmlNodeList IntervalTimeNode = xDoc.GetElementsByTagName("IntervalTime");
            IntervalTime = (int)Convert.ToInt32(IntervalTimeNode[0].InnerText);
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
                        newfeed.Category = Convert.ToInt32(t.FirstChild.Value);
                    }

                    
                }

                RssFeedList.Add(newfeed);
                
                

            }

            foreach (RssFeed t in RssFeedList)
            {
                ListViewItem item1 = new ListViewItem(t.Title);
                item1.SubItems.Add(t.PauseDownload.ToString());
                item1.SubItems.Add(t.Category.ToString());
                item1.SubItems.Add(t.Url);

                listView1.Items.Add(item1);




            }
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> lEd2kList = new List<string>();

            foreach (RssFeed feed in RssFeedList)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(feed.Url);

                XmlNodeList elemList = doc.GetElementsByTagName("guid");

                

                textBox1.Clear();
                for (int i = 0; i < elemList.Count; i++)
                {
                    Console.WriteLine(elemList[i].InnerXml);
                    textBox1.Text += elemList[i].InnerXml;
                    string page;
                    page = DownloadPage(elemList[i].InnerXml);
                    string sEd2k = findEd2kLink(page);
                    textBox1.Text += sEd2k;

                    textBox1.AppendText(sEd2k);
                    lEd2kList.Add(sEd2k);


                }

            }

            //DownloadEd2k(sEd2k);
            //Thread t = new Thread(Form1.DoWork);
            //t.Start(textBox1.Text, textBox2.Text, lEd2kList);
            DoWork(textBox2.Text, textBox3.Text, lEd2kList);
        }

        public static string DownloadPage(string sUrl)
        {
            try
            {
                // used to build entire input
                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];

                // prepare the web page we will be asking for
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);

                // execute the request
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // we will read data via the response stream
                Stream resStream = response.GetResponseStream();

                string tempString = null;
                int count = 0;

                do
                {
                    // fill the buffer with data
                    count = resStream.Read(buf, 0, buf.Length);

                    // make sure we read some data
                    if (count != 0)
                    {
                        // translate from bytes to ASCII text
                        tempString = Encoding.ASCII.GetString(buf, 0, count);

                        // continue building the string
                        sb.Append(tempString);
                    }
                }
                while (count > 0); // any more data to read?

                return sb.ToString();
            }
            catch
            { 
                return null; 
            }
        }

        static void DoWork(object Url, object pass, object DataIn)
        {
            string sPass = (string)pass;
            string sUrl = (string)Url;
            List<string> sList = (List<string>)DataIn;

            string temp = LogInEmuleServer(sUrl, sPass);
            if (temp == null)
                return;
            
            int j = temp.IndexOf("logout");
            if (j < 0)
                return;
            
            string sSesId = GetSessionID(temp);

            foreach (string ed2k in sList)
            {

                int cat = 2;
                if (ExistInHistory(ed2k) == false) // if file is not dwnl
                {
                    AddEd2kToDownload(sUrl, sSesId, ed2k, cat);
                    StartDownloadFromEd2k(sUrl, sSesId, ed2k);
                    StopDownloadEd2k(sUrl, sSesId, ed2k);
                    AddToXmlHostory(ed2k);
                }
                else
                {
                    CreateDumpFile("File alredy exist " + ed2k);
                }
            }
            
            

            LogOut(sUrl, sSesId);
            //textBox1.Text += string.Format("Session id {0}", sSesId);





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

        private static string LogInEmuleServer(string sUrl, string sPassword)
        {
            string temp;
            //http://localhost:4000/?w=password&p=PASSWORD
            temp = string.Format("{0}/?w=password&p={1}", sUrl, sPassword);
            return DownloadPage(temp);
            
            
            ////string parameters = " name1=value1&name2=value2";
            ////string parameters = " p=tellurio&w=password";
            //string parameters = string.Format("p={0}&w=password", sPassword);
            //WebRequest webRequest = WebRequest.Create(sUrl);
            //webRequest.ContentType = "application/x-www-form-urlencoded";
            //webRequest.Method = "POST";
            //byte[] bytes = Encoding.ASCII.GetBytes(parameters);
            //Stream os = null;
            //try
            //{ // send the Post
            //    webRequest.ContentLength = bytes.Length;   //Count bytes to send
            //    os = webRequest.GetRequestStream();
            //    os.Write(bytes, 0, bytes.Length);         //Send it
            //}
            //catch (WebException ex)
            //{
            //    MessageBox.Show(ex.Message, "HttpPost: Request error",
            //       MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    if (os != null)
            //    {
            //        os.Close();
            //    }
            //}

            //try
            //{ // get the response
            //    WebResponse webResponse = webRequest.GetResponse();
            //    if (webResponse == null)
            //    { return null; }
            //    StreamReader sr = new StreamReader(webResponse.GetResponseStream());
            //    return sr.ReadToEnd().Trim();
            //}
            //catch (WebException ex)
            //{
            //    MessageBox.Show(ex.Message, "HttpPost: Response error",
            //       MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //return null;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string temp = LogInEmuleServer(ServiceUrl, Password);
            if (temp == null)
            {
                MessageBox.Show("Unable conncet with target URL");
                return;
            }


            
            int j = temp.IndexOf("name=w value=\"password\"");
            if (j > 0)
            {
                MessageBox.Show("Error in password");
                return;
            }

            string sSesId = GetSessionID(temp);
            LogOut(ServiceUrl, sSesId);

            MessageBox.Show("OK service is configure");
            return;
       


        }

        private void DownloadEd2k(string ed2k)
        {
            string temp = LogInEmuleServer(textBox2.Text, textBox3.Text);
            textBox1.Text = temp;
            int j = temp.IndexOf("logout");
            if (j > 0)
                textBox1.Text = "ok";
            else
                textBox1.Text = "error";

            CreateDumpFile("Get Session ID phase");

            string sSesId = GetSessionID(temp);
            textBox1.Text += string.Format("Session id {0}", sSesId);

            CreateDumpFile("Ses ID " + sSesId);
            //GetCategory(temp);
            
            int cat = 2;

            

            AddEd2kToDownload(textBox2.Text, sSesId, ed2k, cat);
            StartDownloadFromEd2k(textBox2.Text, sSesId, ed2k);
            StopDownloadEd2k(textBox2.Text, sSesId, ed2k);
            LogOut(textBox2.Text, sSesId);

        }


        private static string GetSessionID(string text)
        {

            int i = text.IndexOf("&amp;w=logout");
            text = text.Substring(0,i);
            i = text.LastIndexOf("ses=");
            text = text.Substring(i + ("ses=").Length);
            return text;
        }


        private List<string> GetCategory(string text)
        {
            int i, j;
            i = text.IndexOf("<select name=\"cat\" size=\"1\">");
            text = text.Substring(i);
            i = text.IndexOf("</select>");
            text = text.Substring(0, i);

            List<string> lsOut = new List<string>();

            while (text.Length > 10)
            {
                i = text.IndexOf("value=\"") + ("value=\"").Length;
                j = text.IndexOf("\">", i);

                string sCatId = text.Substring(i, j - i);
                
                text = text.Substring(j);

                i = text.IndexOf(">") + ">".Length;
                j = text.IndexOf("</option>");

                string sValue = text.Substring(i, j - i);
                lsOut.Add(sValue);
                text = text.Substring(j);

                

            }
            return lsOut;

        }

        private static string AddEd2kToDownload(string sUrl,string sSesID, string Ed2k, int Category)
        {
            string temp;
            //http://linefinc.homeip.net:4000/?ses=-323522093&w=transfer&ed2k=Ed2kprova&cat=1
            temp = string.Format("{0}/?ses={1}&w=transfer&ed2k={2}&cat={3}", sUrl, sSesID, Ed2k, Category);
            CreateDumpFile("AddEd2kToDownload  " + temp);
            return DownloadPage(temp);
            
        }


        private static string StartDownloadFromEd2k(string sUrl, string sSesID, string Ed2k)
        {

            int i;
            
            // if the link is not a ed2k file link return
            if (Ed2k.IndexOf("ed2k://|file|") < 0)
            {
                return null;
            }

            i = Ed2k.IndexOf("|", "ed2k://|file|".Length + 1);
            i = Ed2k.IndexOf("|", i + 1);


            string sHash = Ed2k.Substring(i + 1, 32);// 32 is the size of md4
            

            //http://linefinc.homeip.net:4000/?ses=-323522093&w=transfer&op=resume&file=546EE8DE741752FBA9899DFF30C4FE75
            string temp = string.Format("{0}/?ses={1}&w=transfer&op=resume&file={2}", sUrl, sSesID, sHash);

            CreateDumpFile("StartDownloadFromEd2k  " + temp);
            return DownloadPage(temp);
        }

        private static string StopDownloadEd2k(string sUrl, string sSesID, string Ed2k)
        {

            int i;

            // if the link is not a ed2k file link return
            if (Ed2k.IndexOf("ed2k://|file|") < 0)
            {
                return null;
            }

            i = Ed2k.IndexOf("|", "ed2k://|file|".Length + 1);
            i = Ed2k.IndexOf("|", i + 1);


            string sHash = Ed2k.Substring(i + 1, 32);// 32 is the size of md4


            //http://linefinc.homeip.net:4000/?ses=-323522093&w=transfer&op=resume&file=546EE8DE741752FBA9899DFF30C4FE75
            string temp = string.Format("{0}/?ses={1}&w=transfer&op=stop&file={2}", sUrl, sSesID, sHash);

            CreateDumpFile("StopDownloadEd2k  " + temp);
            return DownloadPage(temp);
        }



        private static string LogOut(string sUrl, string sSesID)
        {
            string temp = string.Format("{0}/?ses={1}&w=logout", sUrl, sSesID);
            return DownloadPage(temp);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ServiceUrl = textBox2.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox4.Text.IndexOf("http://tvunderground.org.ru/rss.php") < 0)
            {
                MessageBox.Show("Only tvunderground.org.ru service supported");
                return;
            }

            
            RssFeed newfeed = new RssFeed();
            
            XmlDocument doc = new XmlDocument();
            doc.Load(textBox4.Text);

            XmlNodeList elemList = doc.GetElementsByTagName("title");
            newfeed.Title = elemList[0].FirstChild.Value;
            newfeed.Url = textBox4.Text;


            newfeed.PauseDownload = checkBox1.Checked;
            newfeed.Category = comboBox1.SelectedIndex;

            if (newfeed.Category < 0) newfeed.Category = 0;

            AddRssToXMLConfig(newfeed.Title, newfeed.Url, newfeed.PauseDownload, newfeed.Category);

            LoadConfig();

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


        public static void AddToXmlHostory(string ed2k)
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
            XmlElement appendedElementItem = xmlDoc.CreateElement("Item");
            XmlText xmlTextLink = xmlDoc.CreateTextNode(ed2k);
            appendedElementItem.AppendChild(xmlTextLink);
            xmlDoc.DocumentElement.AppendChild(appendedElementItem);
           
            xmlDoc.Save("History.xml");
        }

        public static int CompareEd2k(string linkA, string linkB)
        {
            // if the link is not a ed2k file link return
            int i,j;

            string HashA, HashB, SizeA, SizeB;
            i = linkA.IndexOf("|", "ed2k://|file|".Length + 1);
            j = linkA.IndexOf("|", i + 1);
            SizeA = linkA.Substring(i + 1, j - i -1);
            HashA = linkA.Substring(j + 1, 32);// 32 is the size of md4

            i = linkB.IndexOf("|", "ed2k://|file|".Length + 1);
            j = linkB.IndexOf("|", i + 1);
            SizeB = linkB.Substring(i + 1, j - i - 1);
            HashB = linkB.Substring(j + 1, 32);// 32 is the size of md4

            if (SizeA != SizeB)
                return -1;

            if (HashA != HashB)
                return -1;

            return 0;
        }

        public static bool ExistInHistory(string ed2k)
        {
            if (!File.Exists("History.xml"))
            {
                return false;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load("history.xml");

            XmlNodeList elemList = doc.GetElementsByTagName("Item");

        
            for (int i = 0; i < elemList.Count; i++)
            {

                int rc = CompareEd2k(elemList[i].InnerXml, ed2k);
                if (rc == 0)
                    return true;

            }
            return false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
                return;

            if (listView1.SelectedItems.Count == 0)
                return;

          

            ListViewItem  temp = listView1.SelectedItems[0];
            int i = listView1.Items.IndexOf(temp);
            DeleteFromXMLConfig(RssFeedList[i].Url);
            LoadConfig();
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
                textWritter.WriteStartElement("RSSChannel");
                textWritter.WriteEndElement();

                textWritter.Close();
            }

        }

        public void AddRssToXMLConfig(string Title, string url, bool DonwloadInPause, int Category)
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
            XmlText CategoryXmlText = xmlDoc.CreateTextNode(Category.ToString());

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

         private void button5_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;

            string temp = LogInEmuleServer(ServiceUrl, Password);
            if (temp == null)
            {
                MessageBox.Show("Unable to connect Emule service");
                return;
            }

            int j = temp.IndexOf("logout");
            if (j < 0)
            {
                MessageBox.Show("Unable to log in Emule service (wrong password)");
                return;
            }

            string sSesId = GetSessionID(temp);

            List<string> category = GetCategory(temp);
            foreach (string t in category)
            {
                comboBox1.Items.Add(t);
            }

            comboBox1.SelectedIndex = 0;




            LogOut(ServiceUrl, sSesId);

            button5.Enabled = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Password = textBox3.Text;
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

            xmlDoc.Save("Config.xml");

            


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            List<string> lEd2kList = new List<string>();

            foreach (RssFeed feed in RssFeedList)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(feed.Url);

                XmlNodeList elemList = doc.GetElementsByTagName("guid");



                textBox1.Clear();
                for (int i = 0; i < elemList.Count; i++)
                {
                    Console.WriteLine(elemList[i].InnerXml);
                    textBox1.Text += elemList[i].InnerXml;
                    string page;
                    page = DownloadPage(elemList[i].InnerXml);
                    string sEd2k = findEd2kLink(page);
                    textBox1.Text += sEd2k;

                    textBox1.AppendText(sEd2k);
                    lEd2kList.Add(sEd2k);


                }

            }

            //DownloadEd2k(sEd2k);
            //Thread t = new Thread(Form1.DoWork);
            //t.Start(textBox1.Text, textBox2.Text, lEd2kList);
            DoWork(textBox2.Text, textBox3.Text, lEd2kList);
        }

        private void button7_Click(object sender, EventArgs e)
        {

            //string appPath = Environment.GetFolderPath(Path.GetDirectoryName(Application.ExecutablePath);
            string appPath;
            appPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);


            

        }

        public static string ApplicationConfigPath()
        {
            string appPath;
            appPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //appPath += Environment.

            return appPath;

        }




   


    }

   
}
