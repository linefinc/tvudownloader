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
        public string FeedLinkList;
        public string FeedSource;
        public string Date;
    }

    public class Hostory
    {

        public string FileName { get; private set; }

        public List<fileHistory> fileHistoryList;

        public Hostory()
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
                XmlNode node = ItemList[i];
                

            }
        }
    }
}
