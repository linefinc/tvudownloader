using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace TvUndergroundDownloader
{
    static class GoogleAnalyticsHelper
    {
        static public string cid = string.Empty;
        static public string appVersion = string.Empty;


        static public void TrackScreen(string screenName)
        {
            StringBuilder hitPayload = new StringBuilder();
            hitPayload.Append("v=1");
            hitPayload.Append("&tid=UA-37156697-2");
            hitPayload.AppendFormat("&cid={0}", cid);
            hitPayload.Append("&t=screenview");
            hitPayload.AppendFormat("&cd={0}", screenName);
            hitPayload.AppendFormat("&av={0}", appVersion);
            hitPayload.Append("&an=TvUndergroundDownloader");
            
            Thread workerThread = new Thread(GoogleAnalyticsHelper.Track);
            workerThread.Start(hitPayload.ToString());
        }


        static public void TrackEvent(string evenAction)
        {
            StringBuilder hitPayload = new StringBuilder();
            hitPayload.Append("v=1");
            hitPayload.Append("&tid=UA-37156697-2");
            hitPayload.AppendFormat("&cid={0}", cid);
            hitPayload.Append("&t=event");
            hitPayload.AppendFormat("&ea={0}", evenAction);
            hitPayload.Append("&ec=Main");
            hitPayload.AppendFormat("&av={0}", appVersion);
            hitPayload.Append("&an=TvUndergroundDownloader");
            Track(hitPayload.ToString());

            Thread workerThread = new Thread(GoogleAnalyticsHelper.Track);
            workerThread.Start(hitPayload.ToString());

        }


        static public void Track(object hitPayload)
        {
            if(hitPayload.GetType() != typeof(string))
            {
                throw new Exception("hitPayload must bu string");
            }

            try
            {
                //
                //  GoAnal
                //
                WebRequest request = WebRequest.Create("https://www.google-analytics.com/collect");

                request.Proxy = null;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                //   (request as HttpWebRequest).UserAgent = "my user agent";

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
