using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace tvu
{
    public class FeedLinkCacheRow
    {
        public string FeedLink;
        public string Ed2kLink;
        public string Date;
    }
    
    public class FeedLinkCache
    {
        

        public List<FeedLinkCacheRow> FeedLinkCacheTable;
        public string FileName { get; private set; }

        public FeedLinkCache()
        {
            FeedLinkCacheTable = new List<FeedLinkCacheRow>();
            FileName = Application.LocalUserAppDataPath;
            int rc = FileName.LastIndexOf("\\");
            FileName = FileName.Substring(0, rc) + "\\FeedLinkCache.xml";
        }

        public void AddFeedLink(string FeedLink, string Ed2kLink)
        {

            AddFeedLink(FeedLink, Ed2kLink, DateTime.Now.ToString("s"));

        }

        public void AddFeedLink(string FeedLink,string Ed2kLink, string Date)
        {
            // to avoid duplicate
            foreach (FeedLinkCacheRow t in FeedLinkCacheTable)
            {
                if (t.FeedLink == FeedLink)
                {
                    return;
                }
            }
            
            // add new item
            FeedLinkCacheRow flcr = new FeedLinkCacheRow();
            flcr.Ed2kLink = Ed2kLink;
            flcr.FeedLink = FeedLink;
            
            flcr.Date = DateTime.Now.ToString("s");

            FeedLinkCacheTable.Add(flcr);
        }




        public string FindFeedLink(string FeedLink)
        {
            foreach (FeedLinkCacheRow flcr in FeedLinkCacheTable)
            {
                if (flcr.FeedLink == FeedLink)
                {
                    return flcr.Ed2kLink;
                }

            }
            return string.Empty;
        }

        public void Load()
        {
            // Clear list
            FeedLinkCacheTable.Clear();

            if (!File.Exists(FileName))
            {
                return;
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(FileName);

            XmlNodeList ItemList = xDoc.GetElementsByTagName("Item");

            for (int i = 0; i < ItemList.Count; i++)
            {

                string strDate = DateTime.Now.ToString("s"); // to avoid compatibility with old history file
                
                string strFeedLink = "";
                string strEd2k = "";
                

                XmlNode node = ItemList[i];
                foreach (XmlNode t in node.ChildNodes)
                {

                    if (t.Name == "Ed2kLink")
                    {
                        strEd2k = t.InnerText;
                    }

                    if (t.Name == "FeedLink")
                    {
                        strFeedLink = t.InnerText;
                    }

                    if (t.Name == "Date")
                    {
                        strDate = t.InnerText;
                    }

                    DateTime dateTimeLimit = DateTime.Now.AddDays(15.0);
                    DateTime itemDateTime = DateTime.Parse(strDate);
                    TimeSpan ts = dateTimeLimit - itemDateTime;
                    if (ts.Days > 0)
                    {
                        AddFeedLink(strFeedLink, strEd2k, strDate);
                       
                    }

                }
        
            }

        }


        public void Save()
        {

            XmlTextWriter textWritter = new XmlTextWriter(FileName, null);
            textWritter.Formatting = Formatting.Indented;
            textWritter.Indentation = 4;
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("FeedLinkCache");

            foreach (FeedLinkCacheRow flcr in FeedLinkCacheTable)
            {
                textWritter.WriteStartElement("Item");// open Item
                textWritter.WriteElementString("Ed2kLink", flcr.Ed2kLink);
                textWritter.WriteElementString("FeedLink", flcr.FeedLink);
                textWritter.WriteElementString("Date", flcr.Date);
                textWritter.WriteEndElement(); // close Item

            }

            textWritter.Close();
        }
    }
}
