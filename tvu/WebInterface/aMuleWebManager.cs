using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace TvUndergroundDownloader
{
    internal class aMuleWebManager : IMuleWebManager
    {
        private string Host;
        private string Password;
        private Cookie CookieSessionID;
        private List<string> CategoryCache;
        private string DefaultCategory;

        public bool isConnected { private set; get; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="Host">host uri</param>
        /// <param name="Password">emule web interface password</param>
        public aMuleWebManager(string Host, string Password)
        {
            this.Host = Host;
            this.Password = Password;
            this.CategoryCache = new List<string>();
            this.isConnected = false;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <returns></returns>
        public LoginStatus Connect()
        {
            CookieSessionID = null;
            //
            // generate login uri
            //ex: http://localhost:4000/?w=password&p=PASSWORD
            //
            this.isConnected = false;   // reset connection status

            string requestUri = string.Format("{0}/?pass={1}", Host, Password);
            try
            {
                RequestGET(requestUri);
                if (this.CookieSessionID != null)
                {
                    this.isConnected = true;
                    return LoginStatus.Logged;
                }

                return LoginStatus.PasswordError;
            }
            catch
            {
                return LoginStatus.ServiceNotAvailable;
            }
        }

        /// <summary>
        /// Close connection with emule (Logout)
        /// </summary>
        /// <remarks>reset Session ID</remarks>
        public void Close()
        {
            RequestGET(string.Format("{0}/logout.php", Host));
            this.isConnected = false;
            this.CookieSessionID = null;
        }

        /// <summary>
        /// Add a file to download
        /// </summary>
        /// <param name="link">ed2k link</param>
        /// <param name="Category">name of catogery</param>
        public void AddToDownload(Ed2kfile link, string Category)
        {
            NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
            outgoingQueryString.Add("ed2klink", link.Ed2kLink);
            if (string.IsNullOrEmpty(Category))
                outgoingQueryString.Add("selectcat", DefaultCategory);
            else
                outgoingQueryString.Add("selectcat", Category);
            outgoingQueryString.Add("Submit", "Download link");

            string requestUri = string.Format("{0}/footer.php", Host);
            RequestPOST(requestUri, outgoingQueryString);
        }

        /// <summary>
        /// Force start download
        /// </summary>
        /// <param name="Ed2kLink"></param>
        public void StartDownload(Ed2kfile link)
        {
            NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
            outgoingQueryString.Add("command", "resume");
            outgoingQueryString.Add("status", "all");
            outgoingQueryString.Add("category", DefaultCategory);
            outgoingQueryString.Add(link.HashMD4, "on");

            string requestUri = string.Format("{0}/amuleweb-main-dload.php", Host);

            RequestPOST(requestUri, outgoingQueryString);
        }

        /// <summary>
        /// force stop download
        /// </summary>
        /// <param name="Ed2kLink"></param>
        public void StopDownload(Ed2kfile link)
        {
            NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
            outgoingQueryString.Add("command", "pause");
            outgoingQueryString.Add("status", "all");
            outgoingQueryString.Add("category", DefaultCategory);
            outgoingQueryString.Add(link.HashMD4, "on");

            string requestUri = string.Format("{0}/amuleweb-main-dload.php", Host);
            RequestPOST(requestUri, outgoingQueryString);
        }

        /// <summary>
        /// get available category from emule
        /// </summary>
        /// <param name="forceUpdate">Force upgrade</param>
        /// <returns></returns>
        public List<string> GetCategories(bool forceUpdate)
        {
            // this function is not implemented on Amule

            List<string> categories = new List<string>();

            //<select name="category" id="category">
            //      < option > tutti </ option >
            //      < option selected = "" > Test1 </ option >
            //      < option > Test2 </ option >
            //</select>

            const string startString = "<select name=\"category\" id=\"category\">";
            const string endString = "</select>";
            string page = RequestGET(string.Format("{0}/amuleweb-main-dload.php", Host));

            int start = page.IndexOf(startString);

            if (start == -1)
            {
                return categories;
            }

            start += startString.Length;

            int stop = page.IndexOf(endString, start);
            if (stop == -1)
            {
                return categories;
            }

            page = page.Substring(start, stop - start);
            page = page.Replace("<option>", "");
            page = page.Replace("<option selected>", "");
            page = page.Replace("</option>", ";");
            page = page.TrimEnd(';');

            categories.AddRange(page.Split(';'));
            if (categories.Count > 0)
            {
                this.DefaultCategory = categories[0];
            }

            return categories;
        }

        /// <summary>
        /// get list of actual file in download
        /// </summary>
        /// <returns></returns>
        public List<Ed2kfile> GetCurrentDownloads(List<Ed2kfile> knownFiles)
        {
            if (knownFiles == null)
            {
                throw new NullReferenceException("knownFiles");
            }

            List<Ed2kfile> ListDownloads = new List<Ed2kfile>();
            // get download page
            string page = RequestGET(string.Format("{0}/amuleweb-main-dload.php", Host));

            if (page == null)
            {
                return ListDownloads;
            }

            foreach (Ed2kfile file in knownFiles)
            {
                if (page.IndexOf(file.HashMD4) > -1)
                {
                    ListDownloads.Add(new Ed2kfile(file));
                }
            }

            return ListDownloads;
        }

        public void CloseEmuleApp()
        {
            // function not availale
        }

        public void ForceRefreshSharedFileList()
        {
            // function not availale
        }

        /// <summary>
        /// Web socket
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>html page</returns>
        private string RequestGET(string uri)
        {
            // create a request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            // this is necessary becosue amule web send by default gzip page
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.CookieContainer = new CookieContainer();

            if (this.CookieSessionID != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(this.CookieSessionID);
            }

            // grab te response and print it out to the console along with the status code
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // this code update the coockie
            foreach (Cookie coockie in response.Cookies)
            {
                if (coockie.Name == "amuleweb_session_id")
                {
                    CookieSessionID = coockie;
                }
            }

            using (StreamReader reader = new StreamReader(response.GetResponseStream(), new UTF8Encoding()))
            {
                string tempBuffer = reader.ReadToEnd();
#if DEBUG   // save log file

                string fileName = string.Format("emule-{0:yyyy-MM-dd-HH-mm-ss-ff}.html", DateTime.Now);
                using (System.IO.TextWriter writer = System.IO.File.CreateText(fileName))
                {
                    writer.WriteLine("<!-- GET: {0} -->", uri);
                    writer.WriteLine("<!-- {0:yyyy-MM-dd-HH-mm-ss-ff} -->", DateTime.Now);
                    writer.Write(tempBuffer);
                }
#endif
                response.Close();
                return tempBuffer;
            }
        }

        private string RequestPOST(string uri, NameValueCollection outgoingQueryString)
        {
            // create a request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            // this is necessary becosue amule web send by default gzip page
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            request.CookieContainer = new CookieContainer();
            if (this.CookieSessionID != null)
            {
                request.CookieContainer.Add(new Uri(uri), new Cookie(this.CookieSessionID.Name, this.CookieSessionID.Value));
            }

            // Create POST data and convert it to a byte array.
            byte[] byteArray = Encoding.UTF8.GetBytes(outgoingQueryString.ToString());

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            // grab te response and print it out to the console along with the status code
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream(), new UTF8Encoding()))
            {
                string tempBuffer = reader.ReadToEnd();
#if DEBUG   // save log file

                string fileName = string.Format("emule-{0:yyyy-MM-dd-HH-mm-ss-ff}.html", DateTime.Now);
                using (System.IO.TextWriter writer = System.IO.File.CreateText(fileName))
                {
                    writer.WriteLine("<!-- POST: {0} -->", uri);
                    writer.WriteLine("<!-- DateTime: {0:yyyy-MM-dd HH:mm:ss.ff} -->", DateTime.Now);
                    writer.WriteLine("<!-- OutgoingQuery: {0} -->", outgoingQueryString.ToString());
                    writer.WriteLine("<!-- OutgoingCookie: \"{0}\":\"{1}\"-->", this.CookieSessionID.Name, this.CookieSessionID.Value);
                    writer.Write(tempBuffer);
                }
#endif
                response.Close();

                Thread.Sleep(3000);
                return tempBuffer;
            }
        }
    }
}