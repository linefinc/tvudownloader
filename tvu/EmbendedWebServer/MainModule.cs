using Nancy;
using Nancy.Responses.Negotiation;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
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
                Model.RssFeedList = GlobalVar.Config.RssFeedList;
                Model.LastActivity = GlobalVar.Config.RssFeedList.GetLastActivity();
                return View["index", Model];
            };

            Get["/fff"] = x =>
            {
                return Response.AsJson(GlobalVar.Config.RssFeedList.GetLastActivity());
            };

            Post["/RssSubscription"] = args =>
            {

                var action = (string)this.Request.Form.action;
                var newFeedUrl = (string)this.Request.Form.newFeedUrl;

                if(action == "add_new_feed")
                {
                    CookieContainer cookieContainer = new CookieContainer();
                    Uri uriTvunderground = new Uri("http://tvunderground.org.ru/");
                    cookieContainer.Add(uriTvunderground, new Cookie("h", GlobalVar.Config.tvuCookieH));
                    cookieContainer.Add(uriTvunderground, new Cookie("i", GlobalVar.Config.tvuCookieI));
                    cookieContainer.Add(uriTvunderground, new Cookie("t", GlobalVar.Config.tvuCookieT));
                    var newSubscription = new RssSubscription(newFeedUrl, cookieContainer);

                    GlobalVar.Config.RssFeedList.Add(newSubscription);
                }

                //Files.Add(new File() { Name = newFeedUrl });

                //Model.Files = Files;
                //Model.Products = Products;
                //Model.Deleted = true;

                return Response.AsRedirect("/");

            };

            Get["/favicon.ico"] = x => { return View["favicon.ico"]; };
        }
    }
}
