using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace tvu
{
    public class History
    {




        public string FileName { get; private set; }

        public List<fileHistory> fileHistoryList { get; private set; }
        private Hashtable HashtableGuid;

        public History()
        {
            fileHistoryList = new List<fileHistory>();
            HashtableGuid = new Hashtable();

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

                string strDate = DateTime.Now.ToString("s"); // to avoid compatibility with old history file
                string strFeedLink = "";
                string strEd2k = "";
                string strFeedSource = "";

                XmlNode node = ItemList[i];
                foreach (XmlNode t in node.ChildNodes)
                {

                    if (t.Name == "Ed2k")
                    {
                        strEd2k = t.InnerText;
                    }

                    if (t.Name == "FeedLink")
                    {
                        strFeedLink = t.InnerText;
                    }

                    if (t.Name == "FeedSource")
                    {
                        strFeedSource = t.InnerText;
                    }

                    if (t.Name == "Date")
                    {
                        strDate = t.InnerText;
                    }


                }
                fileHistory fh = new fileHistory(strEd2k, strFeedLink, strFeedSource, strDate);
                fileHistoryList.Add(fh);

                if (!HashtableGuid.ContainsKey(strFeedLink))
                    HashtableGuid.Add(strFeedLink, fh);
            }
        }

        public void Save()
        {
            XmlTextWriter textWritter = new XmlTextWriter(FileName, null);
            textWritter.Formatting = Formatting.Indented;
            textWritter.Indentation = 4;
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("History");

            foreach (fileHistory fh in fileHistoryList)
            {
                textWritter.WriteStartElement("Item");// open Item
                textWritter.WriteElementString("Ed2k", fh.GetLink());
                textWritter.WriteElementString("FeedLink", fh.FeedLink);
                textWritter.WriteElementString("FeedSource", fh.FeedSource);
                textWritter.WriteElementString("Date", fh.Date);
                textWritter.WriteEndElement(); // close Item

            }

            textWritter.Close();

        }

        ///
        /// <summary>Add a element to list </summary>
        /// <param name='ed2k'>ED2K Link</param>
        /// <param name='FeedLink'>Link in Feed</param>
        /// <param name='FeedSource'>Rss Feed Link</param>
        ///
        public void Add(string ed2k, string FeedLink, string FeedSource)
        {
            // delete old file whit same ed2k name
            int index;
            while ((index = ExistInHistoryByEd2k(ed2k)) != -1)
            {
                fileHistoryList.Remove(fileHistoryList[index]);
                HashtableGuid.Remove(FeedSource);
            }



            fileHistory fh = new fileHistory(ed2k, FeedLink, FeedSource);
            if (!HashtableGuid.ContainsKey(FeedLink))
                HashtableGuid.Add(FeedLink, fh);
            fileHistoryList.Add(fh);
        }

        public bool FileExistByFeedLink(string link)
        {
            return HashtableGuid.ContainsKey(link);
        }

        public int ExistInHistoryByEd2k(string Ed2kLink)
        {
            Ed2kfile A = new Ed2kfile(Ed2kLink);

            for (int index = 0; index < fileHistoryList.Count; index++)
            {
                Ed2kfile B = new Ed2kfile(fileHistoryList[index].GetLink());

                if (A == B)
                {
                    return index;
                }
            }
            return -1;
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

        public string LastDownloadDateByFeedSource(string FeedSource)
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

        public void DeleteFile(string FileNameED2k)
        {
            fileHistoryList.RemoveAll(delegate(fileHistory temp) { return temp.FileName == FileNameED2k; });
        }

        public void DeleteFileByFeedSource(string FeedSource)
        {

            List<fileHistory> fileToDelete = new List<fileHistory>();

            foreach (fileHistory fh in fileHistoryList)
            {
                if (fh.FeedSource == FeedSource)
                {
                    fileToDelete.Add(fh);
                }
            }

            foreach (fileHistory fh in fileToDelete)
            {
                fileHistoryList.Remove(fh);
            }
        }

        public List<fileHistory> GetRecentActivity(int size)
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

            if (myFileHistoryList.Count > size)
            {
                myFileHistoryList.RemoveRange(size, myFileHistoryList.Count - size);
            }
            return myFileHistoryList;
        }

        /// <summary>
        /// Return the number of active download from FeedSoruce
        /// </summary>
        /// <param name="list">List of ed2k in active download from GetActualDownloads()</param>
        /// <param name="FeedSource">Feed soruce</param>
        /// <returns></returns>
        public int GetFeedByDownload(List<string> list, string FeedSource)
        {
            int count = 0;


            Regex Pattern = new Regex(@"\|\d{1,40}\|\w{1,40}\|");

            foreach (fileHistory p in fileHistoryList)
            {
                if (p.FeedSource == FeedSource)
                {
                    Match match1 = Pattern.Match(p.GetLink());
                    if (match1.Success == true)
                    {
                        foreach (string t in list)
                        {
                            Match match2 = Pattern.Match(t);
                            if (match2.Success == true)
                            {
                                if (match1.ToString() == match2.ToString())
                                {
                                    count++;
                                }
                            }
                        }
                    }
                }
            }

            return count;

        }
    }
}
