using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace tvu
{
    public class Config
    {
        public string ServiceUrl { get; set; }
        public string Password { get; set; }
        public int IntervalTime { get; set; }
        public bool StartMinimized { get; set; }
        public bool CloseWhenAllDone { get; set; }
        public bool AutoStartEmule { get; set; }
        public List<RssFeed> RssFeedList { get; set; }
        public string eMuleExe { get; set; }
        public bool debug {get; set;}
        public string FileName { get; set; }

        public Config(string FileName)
        {
            this.FileName = FileName;

            RssFeedList = new List<RssFeed>();
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
            textWritter.WriteString(AutoStartEmule.ToString());
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("CloseWhenAllDone");
            textWritter.WriteString(CloseWhenAllDone.ToString());
            textWritter.WriteEndElement();

            textWritter.WriteStartElement("eMuleExe");
            textWritter.WriteString(eMuleExe);
            textWritter.WriteEndElement();
            
            textWritter.WriteStartElement("RSSChannel");
        


            foreach (RssFeed feed in RssFeedList)
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

                    textWritter.WriteStartElement("Url");//Title
                    textWritter.WriteString(feed.Url);
                    textWritter.WriteEndElement();

                    textWritter.WriteStartElement("Pause");//Title
                    textWritter.WriteString(feed.PauseDownload.ToString());
                    textWritter.WriteEndElement();

                    textWritter.WriteStartElement("Category");//Title
                    textWritter.WriteString(feed.Category);
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

            CloseWhenAllDone = (bool)Convert.ToBoolean(ReadString(xDoc, "CloseWhenAllDone", "false"));

            AutoStartEmule = (bool)Convert.ToBoolean(ReadString(xDoc, "AutoStartEmule", "false"));

            eMuleExe = ReadString(xDoc, "eMuleExe", "");

            debug = (bool)Convert.ToBoolean(ReadString(xDoc, "Debug", "false"));
            
            //
            //  Load Channel
            //
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

                    if (t.Name == "Url")
                    {
                        newfeed.Url = t.FirstChild.Value;
                    }

                    if (t.Name == "Pause")
                    {
                        newfeed.PauseDownload = Convert.ToBoolean(t.FirstChild.Value);
                    }

                    if( (t.Name == "Category")& (t.FirstChild != null))
                    {
                        newfeed.Category = t.FirstChild.Value;
                    }
                }

                RssFeedList.Add(newfeed);
            }
                
        
        }

        private string ReadString(XmlDocument xDoc, string NodeName, string defaultValue)
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
