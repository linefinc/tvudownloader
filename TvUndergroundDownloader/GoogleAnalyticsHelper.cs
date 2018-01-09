using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TvUndergroundDownloader
{
    internal static class GoogleAnalyticsHelper
    {
        internal static string Cid = string.Empty;
        internal static string AppVersion = string.Empty;

        internal static string Base()
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;

            StringBuilder hitPayload = new StringBuilder();
            hitPayload.Append("v=1");
            hitPayload.Append("&tid=UA-37156697-2");
            hitPayload.AppendFormat("&cid={0}", Cid);
            hitPayload.Append("&t=screenview");
            // App Version
            hitPayload.AppendFormat("&av={0}", AppVersion);
            // User Language
            hitPayload.AppendFormat("&ul={0}", cultureInfo.Name);
            // Screen Resolution
            hitPayload.AppendFormat("&sr={0}x{1}",
                Screen.PrimaryScreen.Bounds.Width,
                Screen.PrimaryScreen.Bounds.Height);
            hitPayload.Append("&an=TvUndergroundDownloader");
            return hitPayload.ToString();
        }

        internal static void TrackScreen(string screenName)
        {
            // Screen Name
            string hitPayload = string.Format("{0}&cd={1}", Base(), screenName);

            Task task = new Task(() =>
           {
               Track(hitPayload);
           });
            task.Start();
        }

        internal static void TrackEvent(string evenAction)
        {
            string hitPayload = string.Format("{0}&ea={1}", Base(), evenAction);

            Task task = new Task(() =>
            {
                Track(hitPayload);
            });
            task.Start();
        }

        internal static void Track(object hitPayload)
        {
            if (hitPayload.GetType() != typeof(string))
            {
                throw new Exception("hitPayload must bu string");
            }

            try
            {
                //
                //  GoAnal
                //
                //  Parameter reference: https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters
                //  Protocol reference: https://developers.google.com/analytics/devguides/collection/protocol/v1/reference
                //
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google-analytics.com/collect");
                request.UserAgent = "";
                request.Proxy = null;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                byte[] reqData = Encoding.UTF8.GetBytes(hitPayload.ToString());
                request.ContentLength = reqData.Length;

                using (Stream reqStream = request.GetRequestStream())
                    reqStream.Write(reqData, 0, reqData.Length);

                using (WebResponse res = request.GetResponse())
                using (Stream resSteam = res.GetResponseStream())
                using (StreamReader sr = new StreamReader(resSteam))
                    sr.ReadToEnd();
            }
            catch
            {
            }
        }
    }
}