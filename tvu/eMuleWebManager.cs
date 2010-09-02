using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;


namespace tvu
{
    class eMuleWebManager
    {
        private string Host;
        private string Password;
        private string SesID;
        private List<string> CategoryCache;
        
        public eMuleWebManager(string Host, string Password)
        {
            this.Host = Host;
            this.Password = Password;
            this.CategoryCache = new List<string>();
        }

        public List<string> GetActualDownloads()
        {
            List<string> ListDownloads = new List<string>();

            string temp = string.Format("{0}/?ses={1}&w=transfer", Host, SesID);
            temp = DownloadPage(temp);

            int p = temp.IndexOf("ed2k://");
            int m = temp.IndexOf("'", p + 5);
            while(p != -1)
            {

                string link = temp.Substring(p, m - p);
                ListDownloads.Add(link);
                p = temp.IndexOf("ed2k://", m + 1);
                m = temp.IndexOf("'", p + 5);
            }
            
            return ListDownloads;
        }

        

        public List<string> GetCategory(bool forceUpdate)
        {
            if ((this.CategoryCache.Count > 0) ^ (forceUpdate == false))
            {
                return this.CategoryCache;
            }
            
            List<string> myList = new List<string>();
            int i, j;
            string temp;

            temp = string.Format("{0}/?ses={1}&w=search", Host, SesID);
            temp = DownloadPage(temp);
            //
            //  Parse html to find category
            //
            i = temp.IndexOf("<select name=\"cat\" size=\"1\">");
            temp = temp.Substring(i);
            i = temp.IndexOf("</select>");
            temp = temp.Substring(0, i);
            while (temp.Length > 10)
            {
                i = temp.IndexOf("value=\"") + ("value=\"").Length;
                j = temp.IndexOf("\">", i);

                string sCatId = temp.Substring(i, j - i);

                temp = temp.Substring(j);

                i = temp.IndexOf(">") + ">".Length;
                j = temp.IndexOf("</option>");

                string sValue = temp.Substring(i, j - i);
                myList.Add(sValue);
                temp = temp.Substring(j);
            }
            this.CategoryCache = myList;
            return myList;

        }

        public bool? LogIn()
        {
            int j, i;

            //ex: http://localhost:4000/?w=password&p=PASSWORD
            string request = string.Format("{0}/?w=password&p={1}", Host, Password);
            string page = DownloadPage(request);

            if (page == null)
            {
                return null;
            }

            j = page.IndexOf("logout");
            if (j < 0)
            {
                return false;
            }

            {
                string temp = page;
                i = temp.IndexOf("&amp;w=logout");
                //temp = temp.Substring(0, i);
                j = temp.LastIndexOf("ses=", i) + ("ses=").Length;
                SesID = temp.Substring(j, i - j);
            }

            if (SesID.Length == 0)
            {
                return false;
            }
            

            return true;
        }

        public void LogOut()
        {
            string temp = string.Format("{0}/?ses={1}&w=logout", Host, SesID);
            DownloadPage(temp);
        }

        public void Close()
        {
            string temp = string.Format("{0}/?ses={1}&w=close", Host, SesID);
            DownloadPage(temp);
        }

        public void AddToDownload(Ed2kParser Ed2kLink, string Category)
        {
            int CategoryId = CategoryCache.IndexOf(Category);
            if (CategoryId == -1)
            {
                CategoryId = 0;
            }
            
            string temp;
            temp = string.Format("{0}/?ses={1}&w=transfer&ed2k={2}&cat={3}", Host, SesID, Ed2kLink.GetLink(), CategoryId);
            DownloadPage(temp);
        }

        public void StartDownload(Ed2kParser Ed2kLink)
        {
            string temp = string.Format("{0}/?ses={1}&w=transfer&op=resume&file={2}", Host, SesID, Ed2kLink.GetHash());
            DownloadPage(temp);
        }

        public void StopDownload(Ed2kParser Ed2kLink)
        {
            string temp = string.Format("{0}/?ses={1}&w=transfer&op=stop&file={2}", Host, SesID, Ed2kLink.GetHash());
            DownloadPage(temp);

        }

        private void UpdateCategory(string text)
        {
            int i, j;
            i = text.IndexOf("<select name=\"cat\" size=\"1\">");
            text = text.Substring(i);
            i = text.IndexOf("</select>");
            text = text.Substring(0, i);

            List<string> lsOut = new List<string>();

            while (text.Length > 10)
            {
                i = text.IndexOf("value=\"") + ("value=\"").Length;
                j = text.IndexOf("\">", i);

                string sCatId = text.Substring(i, j - i);

                text = text.Substring(j);

                i = text.IndexOf(">") + ">".Length;
                j = text.IndexOf("</option>");

                string sValue = text.Substring(i, j - i);
                lsOut.Add(sValue);
                text = text.Substring(j);
            }
            //Category = lsOut;

        }




        public static string DownloadPage(string sUrl)
        {
            try
            {
                // used to build entire input
                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];

                // prepare the web page we will be asking for
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);

                // execute the request
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // we will read data via the response stream
                Stream resStream = response.GetResponseStream();

                string tempString = null;
                int count = 0;

                do
                {
                    // fill the buffer with data
                    count = resStream.Read(buf, 0, buf.Length);

                    // make sure we read some data
                    if (count != 0)
                    {
                        // translate from bytes to ASCII text
                        tempString = Encoding.ASCII.GetString(buf, 0, count);

                        // continue building the string
                        sb.Append(tempString);
                    }
                }
                while (count > 0); // any more data to read?

                return sb.ToString();
            }
            catch
            {
                return null;
            }


        }
    }
}
