using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Reflection;

namespace tvu
{
    public class Config
    {
        public static string Version
        {
            get
            {
                Assembly temp = typeof(Config).Assembly;
                return temp.GetName().Version.ToString();
            }
        }
        public static string VersionFull
        {
            get
            {
                return ((AssemblyInformationalVersionAttribute)Assembly.GetAssembly(typeof(Config))
                                .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)[0]).InformationalVersion;
            }
        }
        public string ServiceUrl;
        public string Password;
        public string tvuCookieH;
        public string tvuCookieI;
        public string tvuCookieT;
        public int IntervalTime;
        public int TotalDownloads;
        public bool StartMinimized;
        public bool CloseEmuleIfAllIsDone;
        public bool StartEmuleIfClose;
        public bool AutoClearLog;
        public List<RssSubscrission> RssFeedList;
        public string eMuleExe;
        public bool debug;
        public string DefaultCategory;
        public bool Enebled;
        public int MaxSimultaneousFeedDownloads;
        public int MinToStartEmule;
        public string tvudwid; //Unique id
        public bool Verbose;
        public bool EmailNotification;
        public string ServerSMTP;
        public string MailReceiver;
        public string MailSender;
        public int intervalBetweenUpgradeCheck;
        public string LastUpgradeCheck;
        public static string FileNameConfig
        {
            get
            {
#if DEBUG
                return "config.xml";
#else
                string temp = Directory.GetParent(Application.LocalUserAppDataPath).FullName;
                return temp + "\\config.xml";
#endif
            }
        }
        public static string FileNameLog
        {
            get
            {
#if DEBUG
                return "log.txt";
#else
                string temp = Application.LocalUserAppDataPath;
                int rc = temp.LastIndexOf("\\");
                return temp.Substring(0, rc) + "\\log.txt";
#endif
            }
        }

        public static string FileNameHistory
        {
            get
            {
#if DEBUG
                return "History.xml";
#else
                string temp = Application.LocalUserAppDataPath;
                int rc = temp.LastIndexOf("\\");
                return temp.Substring(0, rc) + "\\History.xml";
#endif
            }
        }

        public static string FileNameDB
        {
            get
            {
#if DEBUG
                return "storage.db";
#else
                string FileName = Application.LocalUserAppDataPath;
                int rc = FileName.LastIndexOf("\\");
                return FileName.Substring(0, rc) + "\\storage.db";
#endif
            }
        }

        public bool saveLog;
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
            if (!File.Exists(Config.FileNameConfig))
            {
                // empty config file
                XmlTextWriter textWritter = new XmlTextWriter(Config.FileNameConfig, null);
                textWritter.WriteStartDocument();
                textWritter.WriteStartElement("Config");
                textWritter.WriteEndElement();
                textWritter.Close();
            }
            Load();
        }

        public void Save()
        {
            XmlTextWriter writter = new XmlTextWriter(Config.FileNameConfig, null);
            writter.Formatting = Formatting.Indented;

            writter.WriteStartDocument();
            writter.WriteStartElement("Config");

            writter.WriteStartElement("version");
            writter.WriteString(Version);
            writter.WriteEndElement();

            writter.WriteStartElement("ServiceUrl");
            writter.WriteString(ServiceUrl);
            writter.WriteEndElement();

            writter.WriteStartElement("Password");
            writter.WriteString(Password);
            writter.WriteEndElement();

            writter.WriteStartElement("tvuCookieH");
            writter.WriteString(this.tvuCookieH);
            writter.WriteEndElement();

            writter.WriteStartElement("tvuCookieI");
            writter.WriteString(this.tvuCookieI);
            writter.WriteEndElement();

            writter.WriteStartElement("tvuCookieT");
            writter.WriteString(this.tvuCookieT);
            writter.WriteEndElement();

            writter.WriteStartElement("IntervalTime");
            writter.WriteString(IntervalTime.ToString());
            writter.WriteEndElement();

            writter.WriteStartElement("StartMinimized");
            writter.WriteString(StartMinimized.ToString());
            writter.WriteEndElement();

            writter.WriteStartElement("AutoStartEmule");
            writter.WriteString(StartEmuleIfClose.ToString());
            writter.WriteEndElement();

            writter.WriteStartElement("CloseWhenAllDone");
            writter.WriteString(CloseEmuleIfAllIsDone.ToString());
            writter.WriteEndElement();

            writter.WriteStartElement("AutoClearLog");
            writter.WriteString(AutoClearLog.ToString());
            writter.WriteEndElement();

            writter.WriteStartElement("eMuleExe");
            writter.WriteString(eMuleExe);
            writter.WriteEndElement();

            writter.WriteStartElement("DefaultCategory");
            writter.WriteString(DefaultCategory);
            writter.WriteEndElement();

            writter.WriteStartElement("MinToStartEmule");
            writter.WriteString(MinToStartEmule.ToString());
            writter.WriteEndElement();

            writter.WriteStartElement("MaxSimultaneousFeedDownloads");
            writter.WriteString(MaxSimultaneousFeedDownloads.ToString());
            writter.WriteEndElement();

            writter.WriteStartElement("Verbose");
            writter.WriteString(Verbose.ToString());
            writter.WriteEndElement();

            writter.WriteStartElement("EmailNotification");
            writter.WriteString(EmailNotification.ToString());
            writter.WriteEndElement();

            writter.WriteStartElement("ServerSMTP");
            writter.WriteString(ServerSMTP);
            writter.WriteEndElement();

            writter.WriteStartElement("MailReceiver");
            writter.WriteString(MailReceiver.ToString());
            writter.WriteEndElement();

            writter.WriteStartElement("MailSender");
            writter.WriteString(MailSender.ToString());
            writter.WriteEndElement();

            writter.WriteStartElement("SaveLog");
            writter.WriteString(saveLog.ToString());
            writter.WriteEndElement();

            writter.WriteStartElement("tvudwid");
            writter.WriteString(tvudwid);
            writter.WriteEndElement();

            writter.WriteStartElement("intervalBetweenUpgradeCheck");
            writter.WriteString(intervalBetweenUpgradeCheck.ToString());
            writter.WriteEndElement();

            writter.WriteStartElement("LastUpgradeCheck");
            writter.WriteString(LastUpgradeCheck);
            writter.WriteEndElement();

            writter.WriteStartElement("TotalDownloads");
            writter.WriteString(TotalDownloads.ToString());
            writter.WriteEndElement();

            writter.WriteStartElement("RSSChannel");

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
                writter.WriteStartElement("Channel");
                {
                    writter.WriteStartElement("Title");//Title
                    writter.WriteString(feed.Title);
                    writter.WriteEndElement();

                    writter.WriteStartElement("Url");//Url
                    writter.WriteString(feed.Url);
                    writter.WriteEndElement();

                    writter.WriteStartElement("Pause");//Category
                    writter.WriteString(feed.PauseDownload.ToString());
                    writter.WriteEndElement();

                    writter.WriteStartElement("Category");//Category
                    writter.WriteString(feed.Category);
                    writter.WriteEndElement();

                    writter.WriteStartElement("LastUpgradeDate");//Last Upgrade Date
                    writter.WriteString(feed.LastUpgradeDate);
                    writter.WriteEndElement();

                    //TODO: enable not to fix
                    writter.WriteStartElement("Enabled");
                    writter.WriteString(feed.Enabled.ToString());
                    writter.WriteEndElement();

                    writter.WriteStartElement("maxSimultaneousDownload"); // max Simultaneous Downloads
                    writter.WriteString(feed.maxSimultaneousDownload.ToString());
                    writter.WriteEndElement();

                    writter.WriteStartElement("tvuStatus");
                    switch (feed.tvuStatus)
                    {
                        case tvuStatus.Complete:
                            writter.WriteString("Complete");
                            break;
                        case tvuStatus.StillIncomplete:
                            writter.WriteString("StillIncomplete");
                            break;
                        case tvuStatus.StillRunning:
                            writter.WriteString("StillRunning");
                            break;
                        case tvuStatus.OnHiatus:
                            writter.WriteString("OnHiatus");
                            break;
                        case tvuStatus.Unknow:
                        default:
                            writter.WriteString("Unknow");
                            break;
                    }
                    writter.WriteEndElement();
                }

                writter.WriteEndElement();// end channel

            }
            writter.WriteEndElement();// end RSSChannel
            writter.Close();

        }

        public void Load()
        {
            RssFeedList.Clear();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Config.FileNameConfig);

            ServiceUrl = ReadString(xDoc, "ServiceUrl", "http://localhost:4000");

            Password = ReadString(xDoc, "Password", "password");

            tvuCookieH = ReadString(xDoc, "tvuCookieH", string.Empty);

            tvuCookieI = ReadString(xDoc, "tvuCookieI", string.Empty);

            tvuCookieT = ReadString(xDoc, "tvuCookieT", string.Empty);

            IntervalTime = ReadInt(xDoc, "IntervalTime", 30, 1, 24 * 60 * 60);

            StartMinimized = (bool)Convert.ToBoolean(ReadString(xDoc, "StartMinimized", "false"));

            CloseEmuleIfAllIsDone = (bool)Convert.ToBoolean(ReadString(xDoc, "CloseWhenAllDone", "false"));

            StartEmuleIfClose = (bool)Convert.ToBoolean(ReadString(xDoc, "AutoStartEmule", "false"));

            AutoClearLog = (bool)Convert.ToBoolean(ReadString(xDoc, "AutoClearLog", "false"));

            MaxSimultaneousFeedDownloads = ReadInt(xDoc, "MaxSimultaneousFeedDownloads", 3, 0, 50);

            MinToStartEmule = ReadInt(xDoc, "MinToStartEmule", 0, 0, 50);

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

            intervalBetweenUpgradeCheck = ReadInt(xDoc, "intervalBetweenUpgradeCheck", 5, 1, 15);

            LastUpgradeCheck = ReadString(xDoc, "LastUpgradeCheck", DateTime.Now.ToString("yyyy-MM-dd"));

            TotalDownloads = ReadInt(xDoc, "TotalDownloads", 0);
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


        private int ReadInt(XmlDocument xDoc, string NodeName, int defaultValue)
        {
            XmlNodeList t = xDoc.GetElementsByTagName(NodeName);
            if (t.Count == 0)
            {
                return defaultValue;
            }
            else
            {
                return (int)Convert.ToInt32(t[0].InnerText);
            }
        }

        private int ReadInt(XmlDocument xDoc, string NodeName, int defaultValue, int Min, int Max)
        {
            int val = ReadInt(xDoc, NodeName, defaultValue);
            val = Math.Min(val, Max);
            val = Math.Max(val, Min);
            return val;
        }



        private static string RandomIDGenerator()
        {
            string temp = "";

            Random rand = new Random();
            for (int i = 0; i < 24; i++)
            {
                temp += string.Format("{0:X}", rand.Next(0, 15));
            }
            return temp;
        }

    }
}
