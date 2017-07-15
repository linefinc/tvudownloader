using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace TvUndergroundDownloaderLib
{
    public class Config
    {
        public Config()
        {
            //
            // get local user application data path, remove version directory and add config.xml
            //
            RssFeedList = new RssSubscriptionList();
            if (!File.Exists(FileNameConfig))
            {
                // empty configure file
                var textWritter = new XmlTextWriter(FileNameConfig, null);
                textWritter.WriteStartDocument();
                textWritter.WriteStartElement("Config");
                textWritter.WriteEndElement();
                textWritter.Close();
            }
            Load();
        }

        [Flags]
        public enum eServiceType
        {
            eMule = 0,
            aMule
        }

        /// <summary>
        ///     Get current assembly version
        /// </summary>
        public static string Version
        {
            get
            {
                var temp = typeof(Config).Assembly;
                return temp.GetName().Version.ToString();
            }
        }

        public bool AutoClearLog { get; set; }
        public bool CloseEmuleIfAllIsDone { get; set; }
        public static string ConfigFolder => "./";
        public string DefaultCategory { get; set; }
        public bool EmailNotification { get; set; }
        public string eMuleExe { get; set; }
        public bool Enebled { get; set; }
        public static string FileNameConfig => Path.Combine(ConfigFolder, "config.xml");
        public static string FileNameDB => Path.Combine(ConfigFolder, "storage.sqlitedb");
        public static string FileNameHistory => Path.Combine(ConfigFolder, "History.xml");
        public static string FileNameLog => Path.Combine(ConfigFolder, "log.txt");
        public int intervalBetweenUpgradeCheck { get; set; }
        public int IntervalTime { get; set; }
        public string LastUpgradeCheck { get; set; }
        public string MailReceiver { get; set; }
        public string MailSender { get; set; }
        public uint MaxSimultaneousFeedDownloadsDefault { get; set; }
        public int MinToStartEmule { get; set; }
        public string Password { get; set; }
        public bool PauseDownloadDefault { get; set; }
        public RssSubscriptionList RssFeedList { get; set; }
        public bool saveLog { get; set; }
        public string ServerSMTP { get; set; }
        public eServiceType ServiceType { get; set; }
        public string ServiceUrl { get; set; }
        public bool StartEmuleIfClose { get; set; }
        public bool StartMinimized { get; set; }
        public int TotalDownloads { get; set; }
        public string TVUCookieH { get; set; }
        public string TVUCookieI { get; set; }
        public string TVUCookieT { get; set; }
        public string tvudwid { get; set; }
        public bool UseHttpInsteadOfHttps { get; set; } // to implement
        public bool Verbose { get; set; }
        //Unique id
        /// <summary>
        ///     Get full assembly version
        /// </summary>
        public static string VersionFull => ((AssemblyInformationalVersionAttribute)Assembly
            .GetAssembly(typeof(Config))
            .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)[0]).InformationalVersion;

        public bool WebServerEnable { get; set; }
        public int WebServerPort { get; set; }
        public void Load()
        {
            RssFeedList.Clear();

            var xDoc = new XmlDocument();
            xDoc.Load(FileNameConfig);

            // Check configuration version to avoid bad behaviors
            string configVersionStr = ReadString(xDoc, "version", string.Empty);
            if (!string.IsNullOrEmpty(configVersionStr))
            {
                var configVersion = new Version(configVersionStr);
                var appVersion = new Version(Version);
                if (configVersion > appVersion)
                    throw new Exception("Critical Error: the configuration file was created from a future version");
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

            TVUCookieH = ReadString(xDoc, "tvuCookieH", string.Empty);

            TVUCookieI = ReadString(xDoc, "tvuCookieI", string.Empty);

            TVUCookieT = ReadString(xDoc, "tvuCookieT", string.Empty);

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

            UseHttpInsteadOfHttps = Convert.ToBoolean(ReadString(xDoc, "useHttpInsteadOfHttps", "false"));

            WebServerEnable = Convert.ToBoolean(ReadString(xDoc, "WebServerEnable", "false"));

            WebServerPort = ReadInt(xDoc, "WebServerPort", 9696);

            //
            //  Load Channel
            //
            var Channels = xDoc.GetElementsByTagName("Channel");

            for (int i = 0; i < Channels.Count; i++)
            {
                var newfeed = RssSubscription.LoadFormXml(Channels[i]);
                RssFeedList.Add(newfeed);
            }

            RssFeedList.Sort((x, y) => string.Compare(x.Title, y.Title));
        }

        public void Save()
        {
            var writer = new XmlTextWriter(FileNameConfig, new UTF8Encoding(false));
            writer.Formatting = Formatting.Indented;

            writer.WriteStartDocument();
            writer.WriteStartElement("Config");

            writer.WriteElementString("version", Version);

            writer.WriteStartElement("ServiceType");
            if (ServiceType == eServiceType.eMule)
                writer.WriteString("eMule");
            else
                writer.WriteString("aMule");
            writer.WriteEndElement();

            writer.WriteElementString("ServiceUrl", ServiceUrl);
            writer.WriteElementString("Password", Password);
            writer.WriteElementString("tvuCookieH", TVUCookieH);
            writer.WriteElementString("tvuCookieI", TVUCookieI);
            writer.WriteElementString("tvuCookieT", TVUCookieT);
            writer.WriteElementString("IntervalTime", IntervalTime.ToString());
            writer.WriteElementString("StartMinimized", StartMinimized.ToString());

            writer.WriteElementString("AutoStartEmule", StartEmuleIfClose.ToString());

            writer.WriteElementString("CloseWhenAllDone", CloseEmuleIfAllIsDone.ToString());

            writer.WriteElementString("AutoClearLog", AutoClearLog.ToString());

            writer.WriteElementString("eMuleExe", eMuleExe);

            writer.WriteElementString("DefaultCategory", DefaultCategory);

            writer.WriteElementString("MinToStartEmule", MinToStartEmule.ToString());

            writer.WriteElementString("MaxSimultaneousFeedDownloads", MaxSimultaneousFeedDownloadsDefault.ToString());

            writer.WriteElementString("PauseDownloadDefault", PauseDownloadDefault.ToString());

            writer.WriteElementString("Verbose", Verbose.ToString());

            writer.WriteElementString("EmailNotification", EmailNotification.ToString());

            writer.WriteElementString("ServerSMTP", ServerSMTP);

            writer.WriteElementString("MailReceiver", MailReceiver);

            writer.WriteElementString("MailSender", MailSender);

            writer.WriteElementString("SaveLog", saveLog.ToString());

            writer.WriteElementString("tvudwid", tvudwid);

            writer.WriteElementString("intervalBetweenUpgradeCheck", intervalBetweenUpgradeCheck.ToString());

            writer.WriteElementString("LastUpgradeCheck", LastUpgradeCheck);

            writer.WriteElementString("TotalDownloads", TotalDownloads.ToString());

            writer.WriteElementString("useHttpInsteadOfHttps", UseHttpInsteadOfHttps.ToString());

            writer.WriteElementString("WebServerEnable", WebServerEnable.ToString());
            writer.WriteElementString("WebServerPort", WebServerPort.ToString());

            writer.WriteStartElement("RSSChannel");

            var myRssFeedList = new List<RssSubscription>();
            myRssFeedList.AddRange(RssFeedList);
            myRssFeedList.Sort((x, y) => string.Compare(x.Title, y.Title));

            foreach (var feed in myRssFeedList)
                //<Channel>
                //<Title>[ed2k] tvunderground.org.ru: Lie To Me - Season 2 (HDTV) italian </Title>
                //<Url>http://tvunderground.org.ru/rss.php?se_id=32672</Url>
                //<Pause>False</Pause>
                //<Category>Anime</Category>
                //</Channel>
                feed.Write(writer);
            writer.WriteEndElement(); // end RSSChannel
            writer.Close();
        }

        private static string RandomIDGenerator()
        {
            string temp = "";

            var rand = new Random();
            for (int i = 0; i < 24; i++)
                temp += string.Format("{0:X}", rand.Next(0, 15));
            return temp;
        }

        private static string ReadString(XmlDocument xDoc, string NodeName, string defaultValue)
        {
            var t = xDoc.GetElementsByTagName(NodeName);
            if (t.Count == 0)
                return defaultValue;
            return t[0].InnerText;
        }

        private int ReadInt(XmlDocument xDoc, string NodeName, int defaultValue)
        {
            var t = xDoc.GetElementsByTagName(NodeName);
            if (t.Count == 0)
                return defaultValue;
            return Convert.ToInt32(t[0].InnerText);
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
            var t = xDoc.GetElementsByTagName(NodeName);
            if (t.Count == 0)
                return defaultValue;
            return Convert.ToUInt32(t[0].InnerText);
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