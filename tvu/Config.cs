﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace TvUndergroundDownloader
{
    public class Config
    {
        public bool AutoClearLog;

        public bool CloseEmuleIfAllIsDone;

        public bool debug;

        public string DefaultCategory;

        public bool EmailNotification;

        public string eMuleExe;

        public bool Enebled;

        public int intervalBetweenUpgradeCheck;

        public int IntervalTime;

        public string LastUpgradeCheck;

        public string MailReceiver;

        public string MailSender;

        public uint MaxSimultaneousFeedDownloadsDefault;

        public int MinToStartEmule;

        public string Password;

        public bool PauseDownloadDefault;

        public RssSubscriptionList RssFeedList;

        public bool saveLog;

        public string ServerSMTP;

        public eServiceType ServiceType;

        public string ServiceUrl;

        public bool StartEmuleIfClose;

        public bool StartMinimized;

        public int TotalDownloads;

        public string tvuCookieH;

        public string tvuCookieI;

        public string tvuCookieT;

        public string tvudwid;//Unique id

        public bool UseHttpInsteadOfHttps;// to implement

        public bool Verbose;

        public Config()
        {
            //
            //  Enable web browser emulation
            //
            EnableWebBrowserEmulation();

            //
            // get local user application data path, remove version directory and add config.xml
            //
            RssFeedList = new RssSubscriptionList();
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

        public enum eServiceType { eMule = 0, aMule };

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

        public static string FileNameDB
        {
            get
            {
                return Path.Combine(ConfigFolder, "storage.sqlitedb");
            }
        }

        public static string FileNameHistory
        {
            get
            {
                return Path.Combine(ConfigFolder, "History.xml");
            }
        }

        public static string FileNameLog
        {
            get
            {
                return Path.Combine(ConfigFolder, "log.txt");
            }
        }

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

        public void Load()
        {
            RssFeedList.Clear();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Config.FileNameConfig);

            // Check configuration version to avoid bad behaviors
            string configVersionStr = ReadString(xDoc, "version", string.Empty);
            if (!string.IsNullOrEmpty(configVersionStr))
            {
                System.Version configVersion = new System.Version(configVersionStr);
                System.Version appVersion = new System.Version(Version);
                if (configVersion > appVersion)
                {
                    throw new Exception("Critical Error: the configuration file was created from a future version");
                }
            }

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

            StartMinimized = Convert.ToBoolean(ReadString(xDoc, "StartMinimized", "false"));

            CloseEmuleIfAllIsDone = Convert.ToBoolean(ReadString(xDoc, "CloseWhenAllDone", "false"));

            StartEmuleIfClose = Convert.ToBoolean(ReadString(xDoc, "AutoStartEmule", "false"));

            AutoClearLog = Convert.ToBoolean(ReadString(xDoc, "AutoClearLog", "false"));

            MaxSimultaneousFeedDownloadsDefault = ReadUInt(xDoc, "MaxSimultaneousFeedDownloads", 3, 0, 50);

            PauseDownloadDefault = Convert.ToBoolean(ReadString(xDoc, "PauseDownloadDefault", "false"));

            MinToStartEmule = ReadInt(xDoc, "MinToStartEmule", 0, 0, 50);

            eMuleExe = ReadString(xDoc, "eMuleExe", "");

            DefaultCategory = ReadString(xDoc, "DefaultCategory", "");

            debug = Convert.ToBoolean(ReadString(xDoc, "Debug", "false"));
#if DEBUG
            Verbose = true;
#else
            Verbose = Convert.ToBoolean(ReadString(xDoc, "Verbose", "false"));
#endif
            saveLog = Convert.ToBoolean(ReadString(xDoc, "SaveLog", "false"));

            EmailNotification = Convert.ToBoolean(ReadString(xDoc, "EmailNotification", "false"));

            ServerSMTP = ReadString(xDoc, "ServerSMTP", "");

            MailReceiver = ReadString(xDoc, "MailReceiver", "");

            MailSender = ReadString(xDoc, "MailSender", "");

            tvudwid = ReadString(xDoc, "tvudwid", RandomIDGenerator());

            intervalBetweenUpgradeCheck = ReadInt(xDoc, "intervalBetweenUpgradeCheck", 5, 1, 15);

            LastUpgradeCheck = ReadString(xDoc, "LastUpgradeCheck", DateTime.Now.ToString("yyyy-MM-dd"));

            TotalDownloads = ReadInt(xDoc, "TotalDownloads", 0);

            UseHttpInsteadOfHttps = (bool)Convert.ToBoolean(ReadString(xDoc, "useHttpInsteadOfHttps", "false"));

            //
            //  Load Channel
            //
            XmlNodeList Channels = xDoc.GetElementsByTagName("Channel");

            for (int i = 0; i < Channels.Count; i++)
            {
                RssSubscription newfeed = RssSubscription.LoadFormXml(Channels[i]);
                RssFeedList.Add(newfeed);
            }

            RssFeedList.Sort((x, y) => string.Compare(x.Title, y.Title));
        }

        public void Save()
        {
            XmlTextWriter writer = new XmlTextWriter(Config.FileNameConfig, new UTF8Encoding(false));
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

            writer.WriteElementString("ServiceUrl", ServiceUrl);
            writer.WriteElementString("Password", Password);
            writer.WriteElementString("tvuCookieH", tvuCookieH);
            writer.WriteElementString("tvuCookieI", tvuCookieI);
            writer.WriteElementString("tvuCookieT", tvuCookieT);
            writer.WriteElementString("IntervalTime", IntervalTime.ToString());
            writer.WriteElementString("StartMinimized", StartMinimized.ToString());

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
            writer.WriteString(MaxSimultaneousFeedDownloadsDefault.ToString());
            writer.WriteEndElement();

            writer.WriteElementString("PauseDownloadDefault", PauseDownloadDefault.ToString());

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
            writer.WriteString(UseHttpInsteadOfHttps.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("RSSChannel");

            List<RssSubscription> myRssFeedList = new List<RssSubscription>();
            myRssFeedList.AddRange(this.RssFeedList);
            myRssFeedList.Sort((x, y) => string.Compare(x.Title, y.Title));

            foreach (RssSubscription feed in myRssFeedList)
            {
                //<Channel>
                //<Title>[ed2k] tvunderground.org.ru: Lie To Me - Season 2 (HDTV) italian </Title>
                //<Url>http://tvunderground.org.ru/rss.php?se_id=32672</Url>
                //<Pause>False</Pause>
                //<Category>Anime</Category>
                //</Channel>
                feed.Write(writer);
            }
            writer.WriteEndElement();// end RSSChannel
            writer.Close();
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

        private uint ReadUInt(XmlDocument xDoc, string NodeName, uint defaultValue)
        {
            XmlNodeList t = xDoc.GetElementsByTagName(NodeName);
            if (t.Count == 0)
            {
                return defaultValue;
            }
            else
            {
                return Convert.ToUInt32(t[0].InnerText);
            }
        }

        private uint ReadUInt(XmlDocument xDoc, string NodeName, uint defaultValue, uint Min, uint Max)
        {
            uint val = ReadUInt(xDoc, NodeName, defaultValue);
            val = Math.Min(val, Max);
            val = Math.Max(val, Min);
            return val;
        }
    }
}