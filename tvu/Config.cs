using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace TvUndergroundDownloader
{


    public class Config
    {
        public enum eServiceType { eMule = 0, aMule };

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
        public eServiceType ServiceType;
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
        public bool useHttpInsteadOfHttps;

        public static string ConfigFolder
        {
            get
            {
                // base path = C:\Users\User\AppData\Local\TvUndergroundDownloader\TvUndergroundDownloader\version
                // return  C:\Users\User\AppData\Local\TvUndergroundDownloader
                string basePath = Application.LocalUserAppDataPath;
                basePath = Directory.GetParent(basePath).FullName;
                basePath = Directory.GetParent(basePath).FullName;
                return basePath;
            }
        }
        public static string FileNameConfig
        {
            get
            {
                return Path.Combine(ConfigFolder, "config.xml");
            }
        }
        public static string FileNameLog
        {
            get
            {
                return Path.Combine(ConfigFolder, "log.txt");
            }
        }

        public static string FileNameHistory
        {
            get
            {
                return Path.Combine(ConfigFolder, "History.xml");
            }
        }

        public static string FileNameDB
        {
            get
            {
                return Path.Combine(ConfigFolder, "storage.sqlitedb");
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
            //  Enable web browser emulation
            //
            EnableWebBrowserEmulation();

            //
            // get local user application data path, remove version directory and add config.xml
            //
            RssFeedList = new List<RssSubscrission>();
            if (!File.Exists(Config.FileNameConfig))
            {
                // empty configure file
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
            XmlTextWriter writer = new XmlTextWriter(Config.FileNameConfig, null);
            writer.Formatting = Formatting.Indented;

            writer.WriteStartDocument();
            writer.WriteStartElement("Config");

            writer.WriteStartElement("version");
            writer.WriteString(Version);
            writer.WriteEndElement();

            writer.WriteStartElement("ServiceType");
            if (ServiceType == eServiceType.eMule)
            {
                writer.WriteString("eMule");
            }
            else
            {
                writer.WriteString("aMule");
            }
            writer.WriteEndElement();

            writer.WriteStartElement("ServiceUrl");
            writer.WriteString(ServiceUrl);
            writer.WriteEndElement();

            writer.WriteStartElement("Password");
            writer.WriteString(Password);
            writer.WriteEndElement();

            writer.WriteStartElement("tvuCookieH");
            writer.WriteString(this.tvuCookieH);
            writer.WriteEndElement();

            writer.WriteStartElement("tvuCookieI");
            writer.WriteString(this.tvuCookieI);
            writer.WriteEndElement();

            writer.WriteStartElement("tvuCookieT");
            writer.WriteString(this.tvuCookieT);
            writer.WriteEndElement();

            writer.WriteStartElement("IntervalTime");
            writer.WriteString(IntervalTime.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("StartMinimized");
            writer.WriteString(StartMinimized.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("AutoStartEmule");
            writer.WriteString(StartEmuleIfClose.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("CloseWhenAllDone");
            writer.WriteString(CloseEmuleIfAllIsDone.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("AutoClearLog");
            writer.WriteString(AutoClearLog.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("eMuleExe");
            writer.WriteString(eMuleExe);
            writer.WriteEndElement();

            writer.WriteStartElement("DefaultCategory");
            writer.WriteString(DefaultCategory);
            writer.WriteEndElement();

            writer.WriteStartElement("MinToStartEmule");
            writer.WriteString(MinToStartEmule.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("MaxSimultaneousFeedDownloads");
            writer.WriteString(MaxSimultaneousFeedDownloads.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("Verbose");
            writer.WriteString(Verbose.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("EmailNotification");
            writer.WriteString(EmailNotification.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("ServerSMTP");
            writer.WriteString(ServerSMTP);
            writer.WriteEndElement();

            writer.WriteStartElement("MailReceiver");
            writer.WriteString(MailReceiver.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("MailSender");
            writer.WriteString(MailSender.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("SaveLog");
            writer.WriteString(saveLog.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("tvudwid");
            writer.WriteString(tvudwid);
            writer.WriteEndElement();

            writer.WriteStartElement("intervalBetweenUpgradeCheck");
            writer.WriteString(intervalBetweenUpgradeCheck.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("LastUpgradeCheck");
            writer.WriteString(LastUpgradeCheck);
            writer.WriteEndElement();

            writer.WriteStartElement("TotalDownloads");
            writer.WriteString(TotalDownloads.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("useHttpInsteadOfHttps");
            writer.WriteString(useHttpInsteadOfHttps.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("RSSChannel");

            List<RssSubscrission> myRssFeedList = new List<RssSubscrission>();
            myRssFeedList.AddRange(this.RssFeedList);
            myRssFeedList.Sort((x, y) => string.Compare(x.Title, y.Title));

            foreach (RssSubscrission feed in myRssFeedList)
            {
                //<Channel>
                //<Title>[ed2k] tvunderground.org.ru: Lie To Me - Season 2 (HDTV) italian </Title>
                //<Url>http://tvunderground.org.ru/rss.php?se_id=32672</Url>
                //<Pause>False</Pause>
                //<Category>Anime</Category>
                //</Channel>
                writer.WriteStartElement("Channel");
                {
                    writer.WriteStartElement("Title");//Title
                    writer.WriteString(feed.Title);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Url");//Url
                    writer.WriteString(feed.Url);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Pause");//Category
                    writer.WriteString(feed.PauseDownload.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("Category");//Category
                    writer.WriteString(feed.Category);
                    writer.WriteEndElement();

                    writer.WriteStartElement("LastUpgradeDate");//Last Upgrade Date
                    writer.WriteString(feed.LastUpgradeDate);
                    writer.WriteEndElement();

                    //TODO: enable not to fix
                    writer.WriteStartElement("Enabled");
                    writer.WriteString(feed.Enabled.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("maxSimultaneousDownload"); // max Simultaneous Downloads
                    writer.WriteString(feed.maxSimultaneousDownload.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("tvuStatus");
                    switch (feed.tvuStatus)
                    {
                        case tvuStatus.Complete:
                            writer.WriteString("Complete");
                            break;
                        case tvuStatus.StillIncomplete:
                            writer.WriteString("StillIncomplete");
                            break;
                        case tvuStatus.StillRunning:
                            writer.WriteString("StillRunning");
                            break;
                        case tvuStatus.OnHiatus:
                            writer.WriteString("OnHiatus");
                            break;
                        case tvuStatus.Unknown:
                        default:
                            writer.WriteString("Unknown");
                            break;
                    }
                    writer.WriteEndElement();
                    {
                        writer.WriteStartElement("Files");
                        History history = new History();
                        foreach (var file in history.ExportDownloadedFileByFeedSoruce(feed.Url))
                        {
                            writer.WriteStartElement("File");
                            writer.WriteElementString("LinkED2K", file.Ed2kLink);
                            writer.WriteElementString("Guid", file.FeedLink);
                            writer.WriteElementString("Downloaded", file.Date.ToString());
                            writer.WriteEndElement();// end file
                        }
                        writer.WriteEndElement();// end file
                    }
                }

                writer.WriteEndElement();// end channel

            }
            writer.WriteEndElement();// end RSSChannel
            writer.Close();

        }

        public void Load()
        {
            RssFeedList.Clear();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Config.FileNameConfig);

            switch (ReadString(xDoc, "ServiceType", "eMule"))
            {
                case "aMule":
                    ServiceType = eServiceType.aMule;
                    break;
                case "eMule":
                default:
                    ServiceType = eServiceType.eMule;
                    break;
            }

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
#if DEBUG
            Verbose = true;
#else
            Verbose = (bool)Convert.ToBoolean(ReadString(xDoc, "Verbose", "false"));
#endif
            saveLog = (bool)Convert.ToBoolean(ReadString(xDoc, "SaveLog", "false"));

            EmailNotification = (bool)Convert.ToBoolean(ReadString(xDoc, "EmailNotification", "false"));

            ServerSMTP = ReadString(xDoc, "ServerSMTP", "");

            MailReceiver = ReadString(xDoc, "MailReceiver", "");

            MailSender = ReadString(xDoc, "MailSender", "");

            tvudwid = ReadString(xDoc, "tvudwid", RandomIDGenerator());

            intervalBetweenUpgradeCheck = ReadInt(xDoc, "intervalBetweenUpgradeCheck", 5, 1, 15);

            LastUpgradeCheck = ReadString(xDoc, "LastUpgradeCheck", DateTime.Now.ToString("yyyy-MM-dd"));

            TotalDownloads = ReadInt(xDoc, "TotalDownloads", 0);

            useHttpInsteadOfHttps = (bool)Convert.ToBoolean(ReadString(xDoc, "useHttpInsteadOfHttps", "false"));

            //
            //  Initialize db
            //
            DataBaseHelper.RssSubscrissionList.InitDB();
            //
            //  Load Channel
            //
            XmlNodeList Channels = xDoc.GetElementsByTagName("Channel");

            for (int i = 0; i < Channels.Count; i++)
            {
                XmlNodeList child = Channels[i].ChildNodes;


                string newFeedTitle = string.Empty;
                string newFeedUrl = string.Empty; ;

                //
                //  first search constructor data
                //

                foreach (XmlNode t in child)
                {
                    if (t.Name == "Title")
                    {
                        newFeedTitle = t.FirstChild.Value;
                    }

                    if (t.Name == "Url")
                    {
                        newFeedUrl = t.FirstChild.Value;
                    }

                }

                RssSubscrission newfeed = new RssSubscrission(newFeedTitle, newFeedUrl);


                //
                //  load secondary field
                //
                foreach (XmlNode t in child)
                {
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
                            case "Unknown":
                            default:
                                newfeed.tvuStatus = tvuStatus.Unknown;
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
                DataBaseHelper.RssSubscrissionList.AddOrUpgrade(newfeed);
            }

            RssFeedList.Sort((x, y) => string.Compare(x.Title, y.Title));
            //
            //  remove difference between XML and DB
            //
            DataBaseHelper.RssSubscrissionList.CleanUp(RssFeedList);


        }



        public static string ReadString(XmlDocument xDoc, string NodeName, string defaultValue)
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

        private static void EnableWebBrowserEmulation()
        {
            RegistryKey hkcu = Registry.CurrentUser;
            hkcu = hkcu.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (hkcu != null)
            {
                hkcu.SetValue(Application.ProductName, 0x00002ee1, RegistryValueKind.DWord);
            }

            hkcu = Registry.CurrentUser;
            hkcu = hkcu.OpenSubKey(@"Software\WOW6432Node\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (hkcu != null)
            {
                hkcu.SetValue(Application.ProductName, 0x00002ee1, RegistryValueKind.DWord);
            }
        }
    }
}
