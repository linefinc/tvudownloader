﻿using System;
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
            XmlTextWriter writter = new XmlTextWriter(this.FileName, null);
            writter.Formatting = Formatting.Indented;

            writter.WriteStartDocument();
            writter.WriteStartElement("Config");
            
            writter.WriteStartElement("ServiceUrl");
            writter.WriteString(ServiceUrl);
            writter.WriteEndElement();

            writter.WriteStartElement("Password");
            writter.WriteString(Password);
            writter.WriteEndElement();
            
            writter.WriteStartElement("tvuUsername");
            writter.WriteString(tvuUsername);
            writter.WriteEndElement();

            writter.WriteStartElement("tvuPassword");
            writter.WriteString(tvuPassword);
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

                    writter.WriteStartElement("TotalDownloads");//Total Downloads
                    writter.WriteString(feed.TotalDownloads.ToString());
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

                writter.WriteStartElement("LastTvUStatusUpgradeDate");//Last Tv Undergraund Status Upgrade Date
                writter.WriteString(feed.LastTvUStatusUpgradeDate);
                writter.WriteEndElement();
                
                writter.WriteEndElement();// end channel

            }
            writter.WriteEndElement();// end RSSChannel
            writter.Close();

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

            intervalBetweenUpgradeCheck = (int)Convert.ToInt32(ReadString(xDoc, "intervalBetweenUpgradeCheck", "5"));

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
