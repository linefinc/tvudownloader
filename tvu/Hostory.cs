﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace tvu
{
    public class fileHistory
    {
        public string Ed2kLink;
        public string FeedLink;
        public string FeedSource;
        public string Date;
    }

    public class History
    {

        public string FileName { get; private set; }

        public List<fileHistory> fileHistoryList;

        public History()
        {
            fileHistoryList = new List<fileHistory>();
            
            FileName = Application.LocalUserAppDataPath;
            int rc = FileName.LastIndexOf("\\");
            FileName = FileName.Substring(0, rc) + "\\History.xml";
        }

        public void Read()
        {
            if (!File.Exists(FileName))
            {
                return;
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(FileName);

            XmlNodeList ItemList = xDoc.GetElementsByTagName("Item");

            for (int i = 0; i < ItemList.Count; i++)
            {
                fileHistory fh = new fileHistory();
                fh.Date = DateTime.Now.ToString(); // to avoid compatibility with old history file

                XmlNode node = ItemList[i];
                foreach (XmlNode t in node.ChildNodes)
                {
                    if (t.Name == "Ed2k")
                    {
                        fh.Ed2kLink= t.InnerText;
                    }

                    if (t.Name == "FeedLink")
                    {
                        fh.FeedLink = t.InnerText;
                    }

                    if (t.Name == "FeedSource")
                    {
                        fh.FeedSource = t.InnerText;
                    }

                    if (t.Name == "Date")
                    {
                        fh.Date = t.InnerText;
                    }


                }
                fileHistoryList.Add(fh);
            }
        }

        public void Save()
        {
            XmlTextWriter textWritter = new XmlTextWriter(FileName, null);
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("History");

            foreach (fileHistory fh in fileHistoryList)
            {
                textWritter.WriteStartElement("Item");// open Item
                textWritter.WriteElementString("Ed2k", fh.Ed2kLink);
                textWritter.WriteElementString("FeedLink", fh.FeedLink);
                textWritter.WriteElementString("FeedSource", fh.FeedSource);
                textWritter.WriteElementString("Date", fh.Date);
                textWritter.WriteEndElement(); // close Item
            }

            textWritter.Close();
           
        }




        public void Add(string ed2k, string FeedLink, string FeedSource)
        {
            fileHistory fh = new fileHistory();
            fh.Ed2kLink = ed2k;
            fh.FeedLink = FeedLink;
            fh.FeedSource = FeedSource;
            fileHistoryList.Add(fh);

            this.Save();
        }

        public bool FileExistByFeedLink(string link)
        {
            foreach (fileHistory fh in fileHistoryList)
            {
                if (fh.FeedLink == link)
                {
                    return true;
                }
            }
            return false;
        }

        public int LinkCountByFeedSource(string FeedSource)
        {
            int count = 0;
            foreach (fileHistory fh in fileHistoryList)
            {
                if (fh.FeedLink == FeedSource)
                {
                    count++;
                }
            }
            return count;

        }
    }
}
