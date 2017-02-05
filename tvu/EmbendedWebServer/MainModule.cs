using Nancy;
using Nancy.Responses.Negotiation;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

//namespace TvUndergroundDownloader.EmbendedWebServer
namespace TvUndergroundDownloader.EmbendedWebServer
{
    public class MainModule : NancyModule
    {
        public dynamic Model = new ExpandoObject();

        public MainModule()
        {
            Get["/"] = x =>
            {
                return View["index.sshtml"];
            };

            Get["/test"] = x =>
            {
                //if(GlobalVar.Config == null)
                //{
                //    return 404;
                //}

                Model.RssFeedList = GlobalVar.Config.RssFeedList;
                Model.LastActivity = GlobalVar.Config.RssFeedList.GetLastActivity();

                return View["index.sshtml", Model];
                //                Model.RssFeedList = GlobalVar.Config.RssFeedList;

                //return GlobalVar.Config.RssFeedList;
            };

            Get["/fff"] = x =>
            {
                return Response.AsJson(GlobalVar.Config.RssFeedList.GetLastActivity());
            };

            Post["/"] = args =>
        {
                // var newFeedUrl = (string)this.Request.Form.newFeedUrl;

                //Files.Add(new File() { Name = newFeedUrl });

                //Model.Files = Files;
                //Model.Products = Products;
                //Model.Deleted = true;

                return View["index"];

        };

            Get["/favicon.ico"] = x => { return View["favicon.ico"]; };
        }
    }
}
