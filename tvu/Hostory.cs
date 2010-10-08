using System;
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
                fh.Date = DateTime.Now.ToString("s"); // to avoid compatibility with old history file

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
            fh.Date = DateTime.Now.ToString("s");
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

        public bool ExistInHistoryByEd2k(string Ed2kLink)
        {
            Ed2kParser A = new Ed2kParser(Ed2kLink);

            foreach (fileHistory fh in fileHistoryList)
            {
                Ed2kParser B = new Ed2kParser(fh.Ed2kLink);

                if (A == B)
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
                if (fh.FeedSource == FeedSource)
                {
                    count++;
                }
            }
            return count;

        }

        public string LastDownloadByFeedSource(string FeedSource)
        {
            string date = "";
            foreach (fileHistory fh in fileHistoryList)
            {
                if (fh.FeedSource == FeedSource)
                {
                    if (date.Length == 0)
                    {
                        date = fh.Date;
                    }
                    
                    if (date.CompareTo(fh.Date) == -1)
                    { 
                        date = fh.Date; 
                    }
                }
            }
            return date;

        }


        public List<fileHistory> GetRecentActivity()
        {
            List<fileHistory> myFileHistoryList = new List<fileHistory>();

            myFileHistoryList.AddRange(fileHistoryList);



            int index = 0;
            while (index < myFileHistoryList.Count - 1)
            {

                string str1 = myFileHistoryList[index].Date;
                string str2 = myFileHistoryList[index + 1].Date;

                if (str1.CompareTo(str2) == -1)
                {
                    fileHistory t = myFileHistoryList[index];
                    myFileHistoryList[index] = myFileHistoryList[index + 1];
                    myFileHistoryList[index + 1] = t;
                    index = 0;
                }
                else
                {
                    index++;
                }

            }

            if (myFileHistoryList.Count > 30)
            {
                myFileHistoryList.RemoveRange(30, myFileHistoryList.Count - 30);
            }
            return myFileHistoryList;
        }
    }
}
