using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

namespace tvu
{
    public class Config
    {
        public const string Version = "0.04.1";
        public string ServiceUrl { get; set; }
        public string Password { get; set; }
        public int IntervalTime { get; set; }
        public bool StartMinimized { get; set; }
        public bool CloseEmuleIfAllIsDone { get; set; }
        public bool StartEmuleIfClose { get; set; }
        public bool AutoClearLog { get; set; }
        public List<RssSubscrission> RssFeedList { get; set; }
        public string eMuleExe { get; set; }
        public bool debug {get; set;}
        public string FileName { get; set; }
        public string DefaultCategory { get; set; }
        public enumStatus Status { get; set; }
        public bool Enebled { get; set; }
        public int MaxSimultaneousFeedDownloads { get; set; }
        public bool StartWithWindows
        {
            get
            {
                RegistryKey hkcu = Registry.CurrentUser;
                hkcu = hkcu.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", RegistryKeyPermissionCheck.ReadWriteSubTree);
                
                List<string> RegValueNames = new List<string>();
                foreach (string valueName in hkcu.GetValueNames())
                {
                    RegValueNames.Add(valueName);
                }

                if (RegValueNames.IndexOf(Application.ProductName) == -1)
                {
                    return false;
                }
                return true;

            }
            set
            {
                RegistryKey hkcu = Registry.CurrentUser;
                hkcu = hkcu.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", RegistryKeyPermissionCheck.ReadWriteSubTree);

                if (value == true)
                {
                    hkcu.SetValue(Application.ProductName, Environment.CommandLine, RegistryValueKind.String);
                    return;
                }
                
                List<string> RegValueNames = new List<string>();
                foreach (string valueName in hkcu.GetValueNames())
                {
                    RegValueNames.Add(valueName);
                }

                if (RegValueNames.IndexOf(Application.ProductName) == -1)
                {
                    return;
                }
                hkcu.DeleteValue(Application.ProductName);
            }
        }

        public Config()
        {
            //
            // get Local USer App data Path, remove version direcorty and add config.xml
            //
//#if DEBUG
//            FileName = "config.xml";
//#else
            FileName = Application.LocalUserAppDataPath;
            int rc = FileName.LastIndexOf("\\");
            FileName = FileName.Substring(0, rc) + "\\config.xml";
//#endif
            RssFeedList = new List<RssSubscrission>();
            if (!File.Exists(this.FileName))
            {
                // empty config file
                XmlTextWriter textWritter = new XmlTextWriter(this.FileName, null);
                textWritter.WriteStartDocument();
                textWritter.WriteStartElement("Config");
                textWritter.WriteEndElement();
                textWritter.Close();
            }
            Load();
        }

        public void Save()
        {
            XmlTextWriter textWritter = new XmlTextWriter(this.FileName, null);
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("Config");
            
            textWritter.WriteStartElement("ServiceUrl");
            textWritter.WriteString(ServiceUrl);
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("Password");
            textWritter.WriteString(Password);
            textWritter.WriteEndElement();
            
            textWritter.WriteStartElement("IntervalTime");
            textWritter.WriteString(IntervalTime.ToString());
            textWritter.WriteEndElement();
            
            textWritter.WriteStartElement("StartMinimized");
            textWritter.WriteString(StartMinimized.ToString());
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("AutoStartEmule");
            textWritter.WriteString(StartEmuleIfClose.ToString());
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("CloseWhenAllDone");
            textWritter.WriteString(CloseEmuleIfAllIsDone.ToString());
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("AutoClearLog");
            textWritter.WriteString(AutoClearLog.ToString());
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("eMuleExe");
            textWritter.WriteString(eMuleExe);
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("DefaultCategory");
            textWritter.WriteString(DefaultCategory);
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("MaxSimultaneousFeedDownloads");
            textWritter.WriteString(MaxSimultaneousFeedDownloads.ToString());
            textWritter.WriteEndElement();


            textWritter.WriteStartElement("RSSChannel");

            List<RssSubscrission> myRssFeedList = new List<RssSubscrission>();
            myRssFeedList.AddRange(this.RssFeedList);
            myRssFeedList.Sort((x, y) => string.Compare(x.Title, y.Title));

            foreach (RssSubscrission feed in myRssFeedList)
            {
                //<Channel>
                //<Title>[ed2k] tvunderground.org.ru: Lie To Me - Season 2 (HDTV) italian</Title>
                //<Url>http://tvunderground.org.ru/rss.php?se_id=32672</Url>
                //<Pause>False</Pause>
                //<Category>Anime</Category>
                //</Channel>
                textWritter.WriteStartElement("Channel");
                {
                    textWritter.WriteStartElement("Title");//Title
                    textWritter.WriteString(feed.Title);
                    textWritter.WriteEndElement();

                    textWritter.WriteStartElement("Url");//Url
                    textWritter.WriteString(feed.Url);
                    textWritter.WriteEndElement();

                    textWritter.WriteStartElement("Pause");//Category
                    textWritter.WriteString(feed.PauseDownload.ToString());
                    textWritter.WriteEndElement();

                    textWritter.WriteStartElement("Category");//Category
                    textWritter.WriteString(feed.Category);
                    textWritter.WriteEndElement();

                    textWritter.WriteStartElement("TotalDownloads");//Total Downloads
                    textWritter.WriteString(feed.TotalDownloads.ToString());
                    textWritter.WriteEndElement();

                    textWritter.WriteStartElement("LastUpgradeDate");//Last Upgrade Date
                    textWritter.WriteString(feed.LastUpgradeDate);
                    textWritter.WriteEndElement();
                    
                }
                textWritter.WriteEndElement();// end channel

            }
            textWritter.WriteEndElement();// end RSSChannel
            textWritter.Close();

        }

        public void Load()
        {
            RssFeedList.Clear();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(this.FileName);

            ServiceUrl = ReadString(xDoc, "ServiceUrl", "http://localhost:4000");

            Password = ReadString(xDoc, "Password", "password");

            IntervalTime = (int)Convert.ToInt32(ReadString(xDoc, "IntervalTime", "30"));

            StartMinimized = (bool)Convert.ToBoolean(ReadString(xDoc, "StartMinimized", "false"));

            CloseEmuleIfAllIsDone = (bool)Convert.ToBoolean(ReadString(xDoc, "CloseWhenAllDone", "false"));

            StartEmuleIfClose = (bool)Convert.ToBoolean(ReadString(xDoc, "AutoStartEmule", "false"));

            AutoClearLog = (bool)Convert.ToBoolean(ReadString(xDoc, "AutoClearLog", "false"));

            MaxSimultaneousFeedDownloads = (int)Convert.ToInt32(ReadString(xDoc, "MaxSimultaneousFeedDownloads", "3"));

            eMuleExe = ReadString(xDoc, "eMuleExe", "");

            DefaultCategory = ReadString(xDoc, "DefaultCategory", "");

            debug = (bool)Convert.ToBoolean(ReadString(xDoc, "Debug", "false"));



            //
            //  Load Channel
            //
            XmlNodeList Channels = xDoc.GetElementsByTagName("Channel");

            for (int i = 0; i < Channels.Count; i++)
            {
                XmlNodeList child = Channels[i].ChildNodes;
                RssSubscrission newfeed = new RssSubscrission();
                foreach (XmlNode t in child)
                {
                    if (t.Name == "Title")
                    {
                        newfeed.Title = t.FirstChild.Value;
                    }

                    if (t.Name == "Url")
                    {
                        newfeed.Url = t.FirstChild.Value;
                    }

                    if (t.Name == "Pause")
                    {
                        newfeed.PauseDownload = Convert.ToBoolean(t.FirstChild.Value);
                    }

                    if ((t.Name == "Category") & (t.FirstChild != null))
                    {
                        newfeed.Category = t.FirstChild.Value;
                    }

                    if ((t.Name == "TotalDownloads") & (t.FirstChild != null))
                    {
                        newfeed.TotalDownloads = (int) Convert.ToInt32( t.FirstChild.Value);
                    }

                    if ((t.Name == "LastUpgradeDate") & (t.FirstChild != null))
                    {
                        newfeed.LastUpgradeDate = t.FirstChild.Value;
                    }

                }

                RssFeedList.Add(newfeed);
            }


        }

        private static string ReadString(XmlDocument xDoc, string NodeName, string defaultValue)
        {
            XmlNodeList t = xDoc.GetElementsByTagName(NodeName);
            if (t.Count == 0)
            {
                return defaultValue;
            }
            else
            {
                return t[0].InnerText;
            }
        }


    }
}
