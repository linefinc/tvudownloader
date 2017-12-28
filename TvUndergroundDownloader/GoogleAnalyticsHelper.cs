using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TvUndergroundDownloader
{
    static class GoogleAnalyticsHelper
    {
        internal static string cid = string.Empty;
        internal static string appVersion = string.Empty;

        internal static void TrackScreen(string screenName)
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;

            StringBuilder hitPayload = new StringBuilder();
            hitPayload.Append("v=1");
            hitPayload.Append("&tid=UA-37156697-2");
            hitPayload.AppendFormat("&cid={0}", cid);
            hitPayload.Append("&t=screenview");
            hitPayload.AppendFormat("&cd={0}", screenName);
            hitPayload.AppendFormat("&av={0}", appVersion);
            hitPayload.AppendFormat("&ul={0}", cultureInfo.Name);
            hitPayload.Append("&an=TvUndergroundDownloader");

            Task task = new Task(() =>
           {
               Track(hitPayload.ToString());
           });
            task.Start();
        }


        internal static void TrackEvent(string evenAction)
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;

            StringBuilder hitPayload = new StringBuilder();
            hitPayload.Append("v=1");
            hitPayload.Append("&tid=UA-37156697-2");
            hitPayload.AppendFormat("&cid={0}", cid);
            hitPayload.Append("&t=event");
            hitPayload.AppendFormat("&ea={0}", evenAction);
            hitPayload.Append("&ec=Main");
            hitPayload.AppendFormat("&av={0}", appVersion);
            hitPayload.AppendFormat("&ul={0}", cultureInfo.Name);
            hitPayload.Append("&an=TvUndergroundDownloader");
            Track(hitPayload.ToString());

            Task task = new Task(() =>
            {
                Track(hitPayload.ToString());
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
                WebRequest request = WebRequest.Create("https://www.google-analytics.com/collect");

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