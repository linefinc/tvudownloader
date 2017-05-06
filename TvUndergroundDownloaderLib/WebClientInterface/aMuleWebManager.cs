using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace TvUndergroundDownloaderLib
{
    public class aMuleWebManager : IMuleWebManager
    {
        private List<string> categoryCache;
        private Cookie cookieSessionID;
        private string defaultCategory;
        private readonly string host;
        private readonly string password;

        /// <summary>
        ///     constructor
        /// </summary>
        /// <param name="host">host uri</param>
        /// <param name="password">emule web interface password</param>
        public aMuleWebManager(string host, string password)
        {
            this.host = host;
            this.password = password;
            categoryCache = new List<string>();
            IsConnected = false;
        }

        public bool IsConnected { private set; get; }

        /// <summary>
        ///     Login
        /// </summary>
        /// <returns></returns>
        public LoginStatus Connect()
        {
            cookieSessionID = null;
            //
            // generate login uri
            //ex: http://localhost:4000/?w=password&p=PASSWORD
            //
            IsConnected = false; // reset connection status

            string requestUri = string.Format("{0}/?pass={1}", host, password);
            try
            {
                RequestGET(requestUri);
                if (cookieSessionID != null)
                {
                    IsConnected = true;
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
        ///     Close connection with emule (Logout)
        /// </summary>
        /// <remarks>reset Session ID</remarks>
        public void Close()
        {
            RequestGET(string.Format("{0}/logout.php", host));
            IsConnected = false;
            cookieSessionID = null;
        }

        /// <summary>
        ///     Add a file to download
        /// </summary>
        /// <param name="link">ed2k link</param>
        /// <param name="category">name of catogery</param>
        public void AddToDownload(Ed2kfile link, string category)
        {
            var outgoingQueryString = HttpUtility.ParseQueryString(string.Empty);
            outgoingQueryString.Add("ed2klink", link.Ed2kLink);
            if (string.IsNullOrEmpty(category))
                outgoingQueryString.Add("selectcat", defaultCategory);
            else
                outgoingQueryString.Add("selectcat", category);
            outgoingQueryString.Add("Submit", "Download link");

            string requestUri = string.Format("{0}/footer.php", host);
            RequestPOST(requestUri, outgoingQueryString);
        }

        /// <summary>
        ///     Force start download
        /// </summary>
        /// <param name="Ed2kLink"></param>
        public void StartDownload(Ed2kfile link)
        {
            var outgoingQueryString = HttpUtility.ParseQueryString(string.Empty);
            outgoingQueryString.Add("command", "resume");
            outgoingQueryString.Add("status", "all");
            outgoingQueryString.Add("category", defaultCategory);
            outgoingQueryString.Add(link.HashMD4, "on");

            string requestUri = string.Format("{0}/amuleweb-main-dload.php", host);

            RequestPOST(requestUri, outgoingQueryString);
        }

        /// <summary>
        ///     force stop download
        /// </summary>
        /// <param name="Ed2kLink"></param>
        public void StopDownload(Ed2kfile link)
        {
            var outgoingQueryString = HttpUtility.ParseQueryString(string.Empty);
            outgoingQueryString.Add("command", "pause");
            outgoingQueryString.Add("status", "all");
            outgoingQueryString.Add("category", defaultCategory);
            outgoingQueryString.Add(link.HashMD4, "on");

            string requestUri = string.Format("{0}/amuleweb-main-dload.php", host);
            RequestPOST(requestUri, outgoingQueryString);
        }

        /// <summary>
        ///     get available category from emule
        /// </summary>
        /// <param name="forceUpdate">Force upgrade</param>
        /// <returns></returns>
        public List<string> GetCategories(bool forceUpdate)
        {
            // this function is not implemented on Amule

            var categories = new List<string>();

            //<select name="category" id="category">
            //      < option > tutti </ option >
            //      < option selected = "" > Test1 </ option >
            //      < option > Test2 </ option >
            //</select>

            const string StartString = "<select name=\"category\" id=\"category\">";
            const string EndString = "</select>";
            string page = RequestGET(string.Format("{0}/amuleweb-main-dload.php", host));

            int start = page.IndexOf(StartString);

            if (start == -1)
                return categories;

            start += StartString.Length;

            int stop = page.IndexOf(EndString, start);
            if (stop == -1)
                return categories;

            page = page.Substring(start, stop - start);
            page = page.Replace("<option>", "");
            page = page.Replace("<option selected>", "");
            page = page.Replace("</option>", ";");
            page = page.TrimEnd(';');

            categories.AddRange(page.Split(';'));
            if (categories.Count > 0)
                defaultCategory = categories[0];

            return categories;
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
            string page = RequestGET(string.Format("{0}/amuleweb-main-dload.php", host));

            if (page == null)
                return listDownloads;

            foreach (var file in knownFiles)
                if (page.IndexOf(file.HashMD4) > -1)
                    listDownloads.Add(new Ed2kfile(file));

            return listDownloads;
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
        ///     Web socket
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>html page</returns>
        private string RequestGET(string uri)
        {
            // create a request
            var request = (HttpWebRequest) WebRequest.Create(uri);
            // this is necessary becosue amule web send by default gzip page
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.CookieContainer = new CookieContainer();

            if (cookieSessionID != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookieSessionID);
            }

            // grab te response and print it out to the console along with the status code
            var response = (HttpWebResponse) request.GetResponse();

            // this code update the coockie
            foreach (Cookie coockie in response.Cookies)
                if (coockie.Name == "amuleweb_session_id")
                    cookieSessionID = coockie;

            using (var reader = new StreamReader(response.GetResponseStream(), new UTF8Encoding()))
            {
                string tempBuffer = reader.ReadToEnd();
#if DEBUG // save log file

                string fileName = string.Format("emule-{0:yyyy-MM-dd-HH-mm-ss-ff}.html", DateTime.Now);
                using (TextWriter writer = File.CreateText(fileName))
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
            var request = (HttpWebRequest) WebRequest.Create(uri);
            // this is necessary becosue amule web send by default gzip page
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            request.CookieContainer = new CookieContainer();
            if (cookieSessionID != null)
                request.CookieContainer.Add(new Uri(uri), new Cookie(cookieSessionID.Name, cookieSessionID.Value));

            // Create POST data and convert it to a byte array.
            var byteArray = Encoding.UTF8.GetBytes(outgoingQueryString.ToString());

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            var dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            // grab te response and print it out to the console along with the status code
            var response = (HttpWebResponse) request.GetResponse();

            using (var reader = new StreamReader(response.GetResponseStream(), new UTF8Encoding()))
            {
                string tempBuffer = reader.ReadToEnd();
#if DEBUG // save log file

                string fileName = string.Format("emule-{0:yyyy-MM-dd-HH-mm-ss-ff}.html", DateTime.Now);
                using (TextWriter writer = File.CreateText(fileName))
                {
                    writer.WriteLine("<!-- POST: {0} -->", uri);
                    writer.WriteLine("<!-- DateTime: {0:yyyy-MM-dd HH:mm:ss.ff} -->", DateTime.Now);
                    writer.WriteLine("<!-- OutgoingQuery: {0} -->", outgoingQueryString);
                    writer.WriteLine("<!-- OutgoingCookie: \"{0}\":\"{1}\"-->", cookieSessionID.Name,
                        cookieSessionID.Value);
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