using System.Net;

namespace TvUndergroundDownloader
{
    class WebManagerTVU
    {
        public static tvuStatus CheckComplete(string url, CookieContainer cookieContainer)
        {
            tvuStatus status = tvuStatus.Unknown;
            string WebPage = WebFetch.Fetch(url, true, cookieContainer);

            if (WebPage != null)
            {
                if (WebPage.IndexOf("Still Running") > 0)
                {
                    status = tvuStatus.StillRunning;
                }

                if (WebPage.IndexOf("Complete") > 0)
                {
                    status = tvuStatus.Complete;
                }

                if (WebPage.IndexOf("Still Incomplete") > 0)
                {
                    status = tvuStatus.StillIncomplete;
                }

                if (WebPage.IndexOf("On Hiatus") > 0)
                {
                    status = tvuStatus.OnHiatus;
                }
            }

            return status;

        }
    }
}
