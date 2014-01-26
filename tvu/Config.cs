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
        public const string Version = "0.05.5";
        public string ServiceUrl;
        public string Password;
        public string tvuUsername;
        public string tvuPassword;
        public string tvuCookieH;
        public string tvuCookieI;
        public string tvuCookieT;
        public int IntervalTime;
        public int TotalDownloads;
        public bool StartMinimized;
        public bool CloseEmuleIfAllIsDone;
        public bool StartEmuleIfClose;
        public bool AutoClearLog;
        public List<RssSubscrission> RssFeedList ;
        public string eMuleExe;
        public bool debug;
        public string FileName
        {
            get
            {
                string temp = Application.LocalUserAppDataPath;
                int rc = temp.LastIndexOf("\\");
                return temp.Substring(0, rc) + "\\config.xml";
            }
        }
        public string DefaultCategory ;
        public bool Enebled ;
        public int MaxSimultaneousFeedDownloads ;
        public int MinToStartEmule ;
        public string tvudwid ; //Unique id
        public bool Verbose ;
        public bool EmailNotification ;
        public string ServerSMTP ;
        public string MailReceiver ;
        public string MailSender ;
        public int intervalBetweenUpgradeCheck ;
        public string LastUpgradeCheck ;
        public string FileNameLog
        {
            get
            {
                string temp = Application.LocalUserAppDataPath;
                int rc = temp.LastIndexOf("\\");
                return temp.Substring(0, rc) + "\\log.txt";
            }
        }
        public bool saveLog  ;
        public static bool StartWithWindows
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
            
            textWritter.WriteStartElement("tvuUsername");
            textWritter.WriteString(tvuUsername);
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("tvuPassword");
            textWritter.WriteString(tvuPassword);
            textWritter.WriteEndElement();

            
            textWritter.WriteStartElement("tvuCookieH");
            textWritter.WriteString(tvuPassword);
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("tvuCookieI");
            textWritter.WriteString(tvuPassword);
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("tvuCookieT");
            textWritter.WriteString(tvuPassword);
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

            textWritter.WriteStartElement("MinToStartEmule");
            textWritter.WriteString(MinToStartEmule.ToString());
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("MaxSimultaneousFeedDownloads");
            textWritter.WriteString(MaxSimultaneousFeedDownloads.ToString());
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("Verbose");
            textWritter.WriteString(Verbose.ToString());
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("EmailNotification");
            textWritter.WriteString(EmailNotification.ToString());
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("ServerSMTP");
            textWritter.WriteString(ServerSMTP);
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("MailReceiver");
            textWritter.WriteString(MailReceiver.ToString());
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("MailSender");
            textWritter.WriteString(MailSender.ToString());
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("SaveLog");
            textWritter.WriteString(saveLog.ToString());
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("tvudwid");
            textWritter.WriteString(tvudwid);
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("intervalBetweenUpgradeCheck");
            textWritter.WriteString(intervalBetweenUpgradeCheck.ToString());
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("LastUpgradeCheck");
            textWritter.WriteString(LastUpgradeCheck);
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("TotalDownloads");
            textWritter.WriteString(TotalDownloads.ToString());
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

                    //TODO: enable not to fix
                    textWritter.WriteStartElement("Enabled");
                    textWritter.WriteString(feed.Enabled.ToString());
                    textWritter.WriteEndElement();

                    textWritter.WriteStartElement("maxSimultaneousDownload"); // max Simultaneous Downloads
                    textWritter.WriteString(feed.maxSimultaneousDownload.ToString());
                    textWritter.WriteEndElement();

                    textWritter.WriteStartElement("tvuStatus");
                    switch (feed.tvuStatus)
                    {
                        case tvuStatus.Complete:
                            textWritter.WriteString("Complete");
                            break;
                        case tvuStatus.StillIncomplete:
                            textWritter.WriteString("StillIncomplete");
                            break;
                        case tvuStatus.StillRunning:
                            textWritter.WriteString("StillRunning");
                            break;
                        case tvuStatus.OnHiatus:
                            textWritter.WriteString("OnHiatus");
                            break;
                        case tvuStatus.Unknow:
                        default:
                            textWritter.WriteString("Unknow");
                            break;
                    }
                    textWritter.WriteEndElement();
                }

                textWritter.WriteStartElement("LastTvUStatusUpgradeDate");//Last Tv Undergraund Status Upgrade Date
                textWritter.WriteString(feed.LastTvUStatusUpgradeDate);
                textWritter.WriteEndElement();
                
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

            tvuUsername = ReadString(xDoc, "tvuUsername", string.Empty);

            tvuPassword = ReadString(xDoc, "tvuPassword", string.Empty);

            tvuCookieH = ReadString(xDoc, "tvuCookieH", string.Empty);

            tvuCookieI = ReadString(xDoc, "tvuCookieI", string.Empty);

            tvuCookieT = ReadString(xDoc, "tvuCookieT", string.Empty);

            IntervalTime = (int)Convert.ToInt32(ReadString(xDoc, "IntervalTime", "30"));

            StartMinimized = (bool)Convert.ToBoolean(ReadString(xDoc, "StartMinimized", "false"));

            CloseEmuleIfAllIsDone = (bool)Convert.ToBoolean(ReadString(xDoc, "CloseWhenAllDone", "false"));

            StartEmuleIfClose = (bool)Convert.ToBoolean(ReadString(xDoc, "AutoStartEmule", "false"));

            AutoClearLog = (bool)Convert.ToBoolean(ReadString(xDoc, "AutoClearLog", "false"));

            MaxSimultaneousFeedDownloads = (int)Convert.ToInt32(ReadString(xDoc, "MaxSimultaneousFeedDownloads", "3"));

            MinToStartEmule = (int)Convert.ToInt32(ReadString(xDoc, "MinToStartEmule", "0"));

            eMuleExe = ReadString(xDoc, "eMuleExe", "");

            DefaultCategory = ReadString(xDoc, "DefaultCategory", "");

            debug = (bool)Convert.ToBoolean(ReadString(xDoc, "Debug", "false"));

            Verbose = (bool)Convert.ToBoolean(ReadString(xDoc, "Verbose", "false"));

            saveLog = (bool)Convert.ToBoolean(ReadString(xDoc, "SaveLog", "false"));

            EmailNotification = (bool)Convert.ToBoolean(ReadString(xDoc, "EmailNotification", "false"));

            ServerSMTP = ReadString(xDoc, "ServerSMTP", "");

            MailReceiver = ReadString(xDoc, "MailReceiver", "");

            MailSender = ReadString(xDoc, "MailSender", "");

            tvudwid = ReadString(xDoc, "tvudwid", RandomIDGenerator());

            intervalBetweenUpgradeCheck = (int)Convert.ToInt32(ReadString(xDoc, "intervalBetweenUpgradeCheck", "15"));

            LastUpgradeCheck = ReadString(xDoc, "LastUpgradeCheck", DateTime.Now.ToString("yyyy-MM-dd"));

            TotalDownloads = (int)Convert.ToInt32(ReadString(xDoc, "TotalDownloads", "0"));
            //
            //  Load Channel
            //
            XmlNodeList Channels = xDoc.GetElementsByTagName("Channel");

            for (int i = 0; i < Channels.Count; i++)
            {
                XmlNodeList child = Channels[i].ChildNodes;
                RssSubscrission newfeed = new RssSubscrission("", ""); //empty
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

                    if ((t.Name == "LastUpgradeDate") & (t.FirstChild != null))
                    {
                        newfeed.LastUpgradeDate = t.FirstChild.Value;
                    }

                    if ((t.Name == "Enabled") & (t.FirstChild != null))
                    {
                        newfeed.Enabled = (bool)Convert.ToBoolean(t.FirstChild.Value);
                    }

                    if ((t.Name == "tvuStatus") & (t.FirstChild != null))
                    {
                        switch (t.FirstChild.Value)
                        {
                            case "Complete":
                                newfeed.tvuStatus = tvuStatus.Complete;
                                break;
                            case "StillIncomplete":
                                newfeed.tvuStatus = tvuStatus.StillIncomplete;
                                break;
                            case "StillRunning":
                                newfeed.tvuStatus = tvuStatus.StillRunning;
                                break;
                            case "OnHiatus":
                                newfeed.tvuStatus = tvuStatus.OnHiatus;
                                break;
                            case "Unknow":
                            default:
                                newfeed.tvuStatus = tvuStatus.Unknow;
                                break;
                        }

                    }

                    if ((t.Name == "LastTvUStatusUpgradeDate") & (t.FirstChild != null))
                    {
                        newfeed.LastTvUStatusUpgradeDate = t.FirstChild.Value;
                    }

                    if ((t.Name == "maxSimultaneousDownload") & (t.FirstChild != null))
                    {
                        newfeed.maxSimultaneousDownload = (int)Convert.ToInt32(t.FirstChild.Value);
                    }

                    // to avoid error in upgrade from previous version
                    if (newfeed.maxSimultaneousDownload < 1)
                    {
                        newfeed.maxSimultaneousDownload = MaxSimultaneousFeedDownloads;
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


        private static string RandomIDGenerator()
        {
            string temp = "";

            Random rand = new Random();
            for (int i = 0; i < 24; i++)
            {
                temp += string.Format("{0:X}",rand.Next(0, 15));
            }
            return temp;
        }

    }
}
