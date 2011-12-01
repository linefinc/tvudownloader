﻿using System;
using System.Collections.Generic;
using System.Text;

namespace tvu
{
    class WebManagerTVU
    {
        public static tvuStatus CheckComplete(string url)
        {
            tvuStatus status = tvuStatus.Unknow;
            string WebPage = WebFetch.Fetch(url, true);

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
            }

            return status;

        }
    }
}