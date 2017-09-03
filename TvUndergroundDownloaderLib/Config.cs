using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using NLog;

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

        public bool AutoClearLog { get; set; } = false;
        public bool CloseEmuleIfAllIsDone { get; set; } = false;

        public string DefaultCategory { get; set; } = string.Empty;

        public string eMuleExe { get; set; } = String.Empty;
        public bool Enebled { get; set; } = true;
        public virtual string FileNameConfig => "config.xml";
        public virtual string FileNameLog => "log.txt";
        public int IntervalBetweenUpgradeCheck { get; set; } = 5;
        public int IntervalTime { get; set; } = 30;     // todo: replace with timespan
        public string LastUpgradeCheck { get; set; }    // todo: make nullable

        public uint MaxSimultaneousFeedDownloadsDefault { get; set; } = 3;
        public int MinToStartEmule { get; set; } = 0;
        public string Password { get; set; } = "password";
        public bool PauseDownloadDefault { get; set; } = false;
        public RssSubscriptionList RssFeedList { get; set; }

        #region Email 

        public bool EmailNotification { get; set; } = false;
        public string SmtpServerAddress { get; set; } = string.Empty;
        public int SmtpServerPort { get; set; } = 25;
        public bool SmtpServerEnableSsl { get; set; } = false;
        public bool SmtpServerEnableAuthentication { get; set; } = false;
        public string SmtpServerUserName { get; set; } = string.Empty;
        public string SmtpServerPassword { get; set; } = string.Empty;
        public string MailReceiver { get; set; } = string.Empty;
        public string MailSender { get; set; } = string.Empty;

        #endregion

        public eServiceType ServiceType { get; set; } = eServiceType.eMule;
        public string ServiceUrl { get; set; } = "http://localhost:4000";
        public bool StartEmuleIfClose { get; set; } = false;
        public bool StartMinimized { get; set; } = false;
        public int TotalDownloads { get; set; } = 0;

        #region Tvu Cookie

        public string TVUCookieH { get; set; } = string.Empty;
        public string TVUCookieI { get; set; } = string.Empty;
        public string TVUCookieT { get; set; } = string.Empty;

        #endregion


        public string tvudwid { get; set; } = null;
        public bool UseHttpInsteadOfHttps { get; set; } = false;    // todo: implement
        public bool Verbose { get; set; } = false;
        //Unique id
        /// <summary>
        ///     Get full assembly version
        /// </summary>
        public static string VersionFull => ((AssemblyInformationalVersionAttribute)Assembly
            .GetAssembly(typeof(Config))
            .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)[0]).InformationalVersion;

        public bool WebServerEnable { get; set; } = false;
        public int WebServerPort { get; set; } = 9696;
        public void Load(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            RssFeedList.Clear();

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("FileNameConfig {0}", fileName);

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


            if (NodeExist(xDoc, "ServiceUrl"))
                ServiceUrl = ReadString(xDoc, "ServiceUrl", "http://localhost:4000");

            if (NodeExist(xDoc, "Password"))
                Password = ReadString(xDoc, "Password", "password");

            if (NodeExist(xDoc, "tvuCookieH"))
            {
                TVUCookieH = ReadString(xDoc, "tvuCookieH", string.Empty);
                logger.Info("Raw config tvuCookieH {0}", ReadString(xDoc, "tvuCookieH", string.Empty));
            }

            if (NodeExist(xDoc, "tvuCookieI"))
            {
                TVUCookieI = ReadString(xDoc, "tvuCookieI", string.Empty);
                logger.Info("Raw config tvuCookieI {0}", ReadString(xDoc, "tvuCookieI", string.Empty));
            }

            if (NodeExist(xDoc, "tvuCookieT"))
            {
                TVUCookieT = ReadString(xDoc, "tvuCookieT", string.Empty);
                logger.Info("Raw config tvuCookieI {0}", ReadString(xDoc, "tvuCookieT", string.Empty));
            }

            if (NodeExist(xDoc, "IntervalTime"))
                IntervalTime = ReadInt(xDoc, "IntervalTime", 30, 1, 24 * 60 * 60);

            if (NodeExist(xDoc, "StartMinimized"))
                StartMinimized = ReadBoolean(xDoc, "StartMinimized", false);
            //
            if (NodeExist(xDoc, "CloseWhenAllDone"))
                CloseEmuleIfAllIsDone = ReadBoolean(xDoc, "CloseWhenAllDone", false);

            if (NodeExist(xDoc, "AutoStartEmule"))
                StartEmuleIfClose = ReadBoolean(xDoc, "AutoStartEmule", false);

            if (NodeExist(xDoc, "AutoClearLog"))
                AutoClearLog = ReadBoolean(xDoc, "AutoClearLog", false);

            if (NodeExist(xDoc, "MaxSimultaneousFeedDownloads"))
                MaxSimultaneousFeedDownloadsDefault = ReadUInt(xDoc, "MaxSimultaneousFeedDownloads", 3, 0, 50);

            if (NodeExist(xDoc, "PauseDownloadDefault"))
                PauseDownloadDefault = ReadBoolean(xDoc, "PauseDownloadDefault", false);

            if (NodeExist(xDoc, "MinToStartEmule"))
                MinToStartEmule = ReadInt(xDoc, "MinToStartEmule", 0, 0, 50);

            if (NodeExist(xDoc, "eMuleExe"))
                eMuleExe = ReadString(xDoc, "eMuleExe", "");

            if (NodeExist(xDoc, "DefaultCategory"))
                DefaultCategory = ReadString(xDoc, "DefaultCategory", "");
#if DEBUG
            Verbose = true;
#else
            if (NodeExist(xDoc, "Verbose"))
                Verbose = ReadBoolean(xDoc, "Verbose", false);
#endif

            #region Email notification

            if (NodeExist(xDoc, "EmailNotification"))
                EmailNotification = ReadBoolean(xDoc, "EmailNotification", false);

            if (NodeExist(xDoc, "SmtpServerAddress"))
                SmtpServerAddress = ReadString(xDoc, "SmtpServerAddress", "");

            if (NodeExist(xDoc, "SmtpServerPort"))
                SmtpServerPort = ReadInt(xDoc, "SmtpServerPort", 25);

            if (NodeExist(xDoc, "SmtpServerEnableSsl"))
                SmtpServerEnableSsl= ReadBoolean(xDoc, "SmtpServerEnableSsl", false);

            if (NodeExist(xDoc, "SmtpServerEnableAuthentication"))
                SmtpServerEnableAuthentication = ReadBoolean(xDoc, "SmtpServerEnableAuthentication", false);

            if (NodeExist(xDoc, nameof(SmtpServerUserName)))
                SmtpServerUserName = ReadString(xDoc, nameof(SmtpServerUserName), "");

            if (NodeExist(xDoc, "SmtpServerPassword"))
                SmtpServerPassword = ReadString(xDoc, "SmtpServerPassword", "");


            if (NodeExist(xDoc, "MailReceiver"))
                MailReceiver = ReadString(xDoc, "MailReceiver", "");

            if (NodeExist(xDoc, "MailSender"))
                MailSender = ReadString(xDoc, "MailSender", "");

            #endregion


            if (NodeExist(xDoc, "tvudwid"))
                tvudwid = ReadString(xDoc, "tvudwid", RandomIdGenerator());
            else
                tvudwid = RandomIdGenerator();

            if (NodeExist(xDoc, "intervalBetweenUpgradeCheck"))
                IntervalBetweenUpgradeCheck = ReadInt(xDoc, "intervalBetweenUpgradeCheck", 5, 1, 15);

            if (NodeExist(xDoc, "LastUpgradeCheck"))
                LastUpgradeCheck = ReadString(xDoc, "LastUpgradeCheck", DateTime.Now.ToString("yyyy-MM-dd"));

            if (NodeExist(xDoc, "TotalDownloads"))
                TotalDownloads = ReadInt(xDoc, "TotalDownloads", 0);

            if (NodeExist(xDoc, "useHttpInsteadOfHttps"))
                UseHttpInsteadOfHttps = ReadBoolean(xDoc, "useHttpInsteadOfHttps", false);

            if (NodeExist(xDoc, "WebServerEnable"))
                WebServerEnable = ReadBoolean(xDoc, "WebServerEnable", false);

            if (NodeExist(xDoc, "WebServerPort"))
                WebServerPort = ReadInt(xDoc, "WebServerPort", 9696);

            //
            //  Load Channel
            //
            XmlNodeList channelNodeList = xDoc.GetElementsByTagName("Channel");

            for (int i = 0; i < channelNodeList.Count; i++)
            {
                var newfeed = RssSubscription.LoadFormXml(channelNodeList[i]);
                RssFeedList.Add(newfeed);
            }

            RssFeedList.Sort((x, y) => string.Compare(x.Title, y.Title, StringComparison.InvariantCulture));
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

            writer.WriteElementString("ServerSMTP", SmtpServerAddress);

            writer.WriteElementString("MailReceiver", MailReceiver);

            writer.WriteElementString("MailSender", MailSender);

            writer.WriteElementString("tvudwid", tvudwid);

            writer.WriteElementString("intervalBetweenUpgradeCheck", IntervalBetweenUpgradeCheck.ToString());

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

        private static string RandomIdGenerator()
        {
            string temp = "";

            var rand = new Random();
            for (int i = 0; i < 24; i++)
                temp += string.Format("{0:X}", rand.Next(0, 15));
            return temp;
        }

        private static string ReadString(XmlDocument xDoc, string nodeName, string defaultValue)
        {
            var t = xDoc.GetElementsByTagName(nodeName);
            if (t.Count == 0)
                return defaultValue;
            return t[0].InnerText;
        }

        private static int ReadInt(XmlDocument xDoc, string nodeName, int defaultValue)
        {
            var t = xDoc.GetElementsByTagName(nodeName);
            if (t.Count == 0)
                return defaultValue;
            return Convert.ToInt32(t[0].InnerText);
        }

        private static int ReadInt(XmlDocument xDoc, string nodeName, int defaultValue, int Min, int Max)
        {
            int val = ReadInt(xDoc, nodeName, defaultValue);
            val = Math.Min(val, Max);
            val = Math.Max(val, Min);
            return val;
        }


        private static uint ReadUInt(XmlDocument xDoc, string nodeName, uint defaultValue)
        {
            var t = xDoc.GetElementsByTagName(nodeName);
            if (t.Count == 0)
                return defaultValue;
            return Convert.ToUInt32(t[0].InnerText);
        }

        private static uint ReadUInt(XmlDocument xDoc, string nodeName, uint defaultValue, uint min, uint max)
        {
            uint val = ReadUInt(xDoc, nodeName, defaultValue);
            val = Math.Min(val, max);
            val = Math.Max(val, min);
            return val;
        }

        private static bool ReadBoolean(XmlDocument xDoc, string nodeName, bool defaultValue)
        {
            var t = xDoc.GetElementsByTagName(nodeName);
            if (t.Count == 0)
                return defaultValue;
            return t[0].InnerText.ToLower().Contains("true");
        }

        private static bool NodeExist(XmlDocument xDoc, string nodeName)
        {
            var t = xDoc.GetElementsByTagName(nodeName);
            if (t.Count == 0)
                return false;
            return true;

        }

    }
}