﻿using System;
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


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="Host">host uri</param>
        /// <param name="Password">emule web interface password</param>
        public eMuleWebManager(string Host, string Password)
        {
            this.Host = Host;
            this.Password = Password;
            this.CategoryCache = new List<string>();
        }


        /// <summary>
        /// Login
        /// </summary>
        /// <returns></returns>
        public bool? LogIn()
        {
            int j, i;
            //
            // generate login uri
            //ex: http://localhost:4000/?w=password&p=PASSWORD
            //
            string request = string.Format("{0}/?w=password&p={1}", Host, Password);
            string page;
            try
            {
                 page = webSocketGET(request);
            }
            catch
            {
                return null;
            }


            // check login 
            // if we not found logout, there's a password error
            j = page.IndexOf("logout");
            if (j < 0)
            {
                return false;
            }

            // find sesion id
            string temp = page;
            i = temp.IndexOf("&amp;w=logout");
            if (i == -1)
            {
                return false;
            }

            j = temp.LastIndexOf("ses=", i) + ("ses=").Length;
            if (j == -1)
            {
                return false;
            }

            SesID = temp.Substring(j, i - j);

            if (SesID.Length == 0)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Close connection with emule
        /// </summary>
        /// <remarks>reset Session ID</remarks>
        public void LogOut()
        {
            string temp = string.Format("{0}/?ses={1}&w=logout", Host, SesID);
            webSocketGET(temp);
        }
        

        /// <summary>
        /// Close mule
        /// </summary>
        /// <remarks>close mule ( require special permis)</remarks>
        public void Close()
        {
            string temp = string.Format("{0}/?ses={1}&w=close", Host, SesID);
            webSocketGET(temp);
        }


        /// <summary>
        /// Add a file to download
        /// </summary>
        /// <param name="Ed2kLink">ed2k link</param>
        /// <param name="Category">name of catogery</param>
        public void AddToDownload(Ed2kfile Ed2kLink, string Category)
        {
            int CategoryId = CategoryCache.IndexOf(Category);
            if (CategoryId == -1)
            {
                CategoryId = 0;
            }

            string temp;

            temp = string.Format("{0}/?ses={1}&w=transfer&ed2k={2}&cat={3}", Host, SesID, Ed2kLink.GetEscapedLink(), CategoryId);

            webSocketGET(temp);
        }


        /// <summary>
        /// Force start download
        /// </summary>
        /// <param name="Ed2kLink"></param>
        public void StartDownload(Ed2kfile Ed2kLink)
        {
            string temp = string.Format("{0}/?ses={1}&w=transfer&op=resume&file={2}", Host, SesID, Ed2kLink.GetHash());
            webSocketGET(temp);
        }


        /// <summary>
        /// force stop download
        /// </summary>
        /// <param name="Ed2kLink"></param>
        public void StopDownload(Ed2kfile Ed2kLink)
        {
            string temp = string.Format("{0}/?ses={1}&w=transfer&op=stop&file={2}", Host, SesID, Ed2kLink.GetHash());
            webSocketGET(temp);

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


        /// <summary>
        /// get available category from emule
        /// </summary>
        /// <param name="forceUpdate"></param>
        /// <returns></returns>
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
            temp = webSocketGET(temp);

            //
            //  Parse html to find category
            //
            i = temp.IndexOf("<select name=\"cat\" size=\"1\">");
            if (i < 0)
            {
                return myList;
            }

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


        /// <summary>
        /// get list of actual file in download
        /// </summary>
        /// <returns></returns>
        public List<string> GetActualDownloads()
        {
            List<string> ListDownloads = new List<string>();

            string temp = string.Format("{0}/?ses={1}&w=transfer&cat=0", Host, SesID);
            temp = webSocketGET(temp);

            if (temp == null)
            {
                return ListDownloads;
            }

            int p = temp.IndexOf("ed2k://|file|");
            int m = temp.IndexOf("|/", p + 5);
            while (p != -1)
            {

                string link = temp.Substring(p, m - p);
                ListDownloads.Add(link);
                p = temp.IndexOf("ed2k://|file|", m + 1);
                m = temp.IndexOf("/'", p + 5);
            }

            return ListDownloads;
        }


        /// <summary>
        /// Web socket
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>html page</returns>
        static public string webSocketGET(string uri)
        {

            // create a request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "GET";
            request.Headers["Accept-Encoding"] = "gzip";
            request.AutomaticDecompression = DecompressionMethods.GZip;

            // grab te response and print it out to the console along with the status code
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), new UTF8Encoding());

#if DEBUG   // save log file
            string tempBuffer = reader.ReadToEnd();
            string fileName = string.Format("emule-{0:yyyy-MM-dd-HH-mm-ss-ff}.html", DateTime.Now);
            using (System.IO.TextWriter writer = System.IO.File.CreateText(fileName))
            {
                writer.WriteLine("<!-- {0} -->", uri);
                writer.Write(tempBuffer);
                writer.Close();
            }
            reader.Close();
            reader.Dispose();
            response.Close();
            return tempBuffer;
#else
            string tempBuffer = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            response.Close();
            return tempBuffer;

#endif

        }
    }
}
