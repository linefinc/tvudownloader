using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TvUndergroundDownloader.EmbendedWebServer
{
    class MainModule : NancyModule
    {
        public MainModule()
        {
            Get["/"] = _ =>
            
                 "Hello World";
            
        }
    }
}
