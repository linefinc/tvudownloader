using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace TvUndergroundDownloaderLib
{
    public class eMuleWebManager : IMuleWebManager
    {
        private List<string> categoryCache;
        private readonly string host;
        private readonly string password;
        private string sesID;

        /// <summary>
        ///     constructor
        /// </summary>
        /// <param name="host">host uri</param>
        /// <param name="password">emule web interface password</param>
        public eMuleWebManager(string host, string password)
        {
            this.host = host;
            this.password = password;
            categoryCache = new List<string>();
            IsConnected = false;
        }

        public string DefaultCategory { get; private set; }
        public bool IsConnected { private set; get; }

        /// <summary>
        ///     Add a file to download
        /// </summary>
        /// <param name="ed2kLink">ed2k link</param>
        /// <param name="category">name of catogery</param>
        public void AddToDownload(Ed2kfile ed2kLink, string category)
        {
            int categoryId = categoryCache.IndexOf(category);
            if (categoryId == -1)
                categoryId = 0;

            WebSocketGET(string.Format("{0}/?ses={1}&w=transfer&ed2k={2}&cat={3}", host, sesID,
                ed2kLink.GetEscapedLink(), categoryId));
        }

        /// <summary>
        ///     Close connection with emule (Logout)
        /// </summary>
        /// <remarks>reset Session ID</remarks>
        public void Close()
        {
            string temp = string.Format("{0}/?ses={1}&w=logout", host, sesID);
            WebSocketGET(temp);
            IsConnected = false;
        }

        public void CloseEmuleApp()
        {
            WebSocketGET(string.Format("{0}/?ses={1}&w=close", host, sesID));
        }

        /// <summary>
        ///     Login
        /// </summary>
        /// <returns></returns>
        public LoginStatus Connect()
        {
            int j, i;
            //
            // generate login uri
            //ex: http://localhost:4000/?w=password&p=PASSWORD
            //
            IsConnected = false; // reset connection status

            string request = string.Format("{0}/?w=password&p={1}", host, password);
            string page;
            try
            {
                page = WebSocketGET(request);
            }
            catch
            {
                return LoginStatus.ServiceNotAvailable;
            }

            // check login
            // if we not found logout, there's a password error
            j = page.IndexOf("logout");
            if (j < 0)
                return LoginStatus.PasswordError;

            // find sesion id
            string temp = page;
            i = temp.IndexOf("&amp;w=logout");
            if (i == -1)
                return LoginStatus.PasswordError;

            j = temp.LastIndexOf("ses=", i) + "ses=".Length;
            if (j == -1)
                return LoginStatus.PasswordError;

            sesID = temp.Substring(j, i - j);

            if (sesID.Length == 0)
                return LoginStatus.PasswordError;

            IsConnected = true;
            return LoginStatus.Logged;
        }

        public void ForceRefreshSharedFileList()
        {
            //  "self.location.href='/?ses=-1051561950&w=shared&reload=true'
            WebSocketGET(string.Format("{0}/?ses={1}&w=shared&reload=true", host, sesID));
        }

        /// <summary>
        ///     get available category from emule
        /// </summary>
        /// <param name="forceUpdate">Force upgrade</param>
        /// <returns></returns>
        public List<string> GetCategories(bool forceUpdate)
        {
            if ((categoryCache.Count > 0) ^ (forceUpdate == false))
                return categoryCache;

            var myList = new List<string>();
            int i, j;
            string temp;

            temp = string.Format("{0}/?ses={1}&w=search", host, sesID);
            temp = WebSocketGET(temp);

            //
            //  Parse html to find category
            //
            i = temp.IndexOf("<select name=\"cat\" size=\"1\">");
            if (i < 0)
                return myList;

            temp = temp.Substring(i);
            i = temp.IndexOf("</select>");
            temp = temp.Substring(0, i);
            while (temp.Length > 10)
            {
                i = temp.IndexOf("value=\"") + "value=\"".Length;
                j = temp.IndexOf("\">", i);

                string sCatId = temp.Substring(i, j - i);

                temp = temp.Substring(j);

                i = temp.IndexOf(">") + ">".Length;
                j = temp.IndexOf("</option>");

                string sValue = temp.Substring(i, j - i);
                myList.Add(sValue);
                temp = temp.Substring(j);
            }
            categoryCache = myList;
            if (myList.Count > 0)
                DefaultCategory = myList[0];
            return myList;
        }

        /// <summary>
        ///     get list of actual file in download
        /// </summary>
        /// <returns></returns>
        public List<Ed2kfile> GetCurrentDownloads(List<Ed2kfile> knownFiles)
        {
            if (knownFiles == null)
                throw new NullReferenceException("knownFiles");
            var listDownloads = new List<Ed2kfile>();

            // get download page
            string page =
                WebSocketGET(string.Format("{0}/?ses={1}&w=transfer&showuploadqueue=false&cat=0", host, sesID));

            if (page == null)
                return null;

            foreach (var file in knownFiles)
                if (page.IndexOf(file.HashMD4) > -1)
                    listDownloads.Add(new Ed2kfile(file));

            return listDownloads;
        }

        /// <summary>
        ///     Force start download
        /// </summary>
        /// <param name="ed2kLink"></param>
        public void StartDownload(Ed2kfile ed2kLink)
        {
            WebSocketGET(string.Format("{0}/?ses={1}&w=transfer&op=resume&file={2}", host, sesID, ed2kLink.GetHash()));
        }

        /// <summary>
        ///     force stop download
        /// </summary>
        /// <param name="ed2kLink"></param>
        public void StopDownload(Ed2kfile ed2kLink)
        {
            WebSocketGET(string.Format("{0}/?ses={1}&w=transfer&op=stop&file={2}", host, sesID, ed2kLink.GetHash()));
        }

        /// <summary>
        ///     Web socket
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>html page</returns>
        private string WebSocketGET(string uri)
        {
            // create a request
            var request = (HttpWebRequest) WebRequest.Create(uri);
            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "GET";
            request.Headers["Accept-Encoding"] = "gzip";
            request.AutomaticDecompression = DecompressionMethods.GZip;

            // grab te response and print it out to the console along with the status code
            var response = (HttpWebResponse) request.GetResponse();

            using (var reader = new StreamReader(response.GetResponseStream(), new UTF8Encoding()))
            {
                string tempBuffer = reader.ReadToEnd();
#if DEBUG // save log file
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                string fileName = string.Format("emule-{0:yyyy-MM-dd-HH-mm-ss-ff}.html", DateTime.Now);
                fileName = Path.Combine(baseDirectory, fileName);
                using (TextWriter writer = File.CreateText(fileName))
                {
                    writer.WriteLine("<!-- {0} -->", uri);
                    writer.Write(tempBuffer);
                }
#endif
                response.Close();
                return tempBuffer;
            }
        }
    }
}