using Nancy;
using System;
using System.Dynamic;
using System.Net;

//namespace TvUndergroundDownloader.EmbendedWebServer
namespace TvUndergroundDownloaderLib.EmbendedWebServer
{
    public class MainModule : NancyModule
    {
        public dynamic Model = new ExpandoObject();

        public MainModule()
        {
            Get["/"] = _ =>
            {
                Model.RssFeedList = GlobalVar.Config.RssFeedList;
                Model.LastActivity = GlobalVar.Config.RssFeedList.GetLastActivity();
                return View["files", Model];
            };

            Get["/files"] = _ =>
            {
                Model.RssFeedList = GlobalVar.Config.RssFeedList;
                Model.LastActivity = GlobalVar.Config.RssFeedList.GetLastActivity();
                return View["files", Model];
            };

            Get["/channels"] = _ =>
            {
                Model.RssFeedList = GlobalVar.Config.RssFeedList;
                Model.LastActivity = GlobalVar.Config.RssFeedList.GetLastActivity();
                return View["channels", Model];
            };

            Get["/RssSubscription/Delete/{id}"] = args =>
            {

                int sessionID = (int)args["id"];
                var channel = GlobalVar.Config.RssFeedList.Find((obj) => obj.seasonID == sessionID);
                GlobalVar.Config.RssFeedList.Remove(channel);
                GlobalVar.Config.Save();
                return Response.AsRedirect("/channels");
            };

            Get["/fff"] = x =>
            {
                return Response.AsJson(GlobalVar.Config.RssFeedList.GetLastActivity());
            };

            Post["/RssSubscription"] = args =>
            {
                var action = (string)this.Request.Form.action;
                var newFeedUrl = (string)this.Request.Form.newFeedUrl;

                if (action == "add_new_feed")
                {
                    CookieContainer cookieContainer = new CookieContainer();
                    Uri uriTvunderground = new Uri("http://tvunderground.org.ru/");
                    cookieContainer.Add(uriTvunderground, new Cookie("h", GlobalVar.Config.tvuCookieH));
                    cookieContainer.Add(uriTvunderground, new Cookie("i", GlobalVar.Config.tvuCookieI));
                    cookieContainer.Add(uriTvunderground, new Cookie("t", GlobalVar.Config.tvuCookieT));
                    var newSubscription = new RssSubscription(newFeedUrl, cookieContainer);

                    GlobalVar.Config.RssFeedList.Add(newSubscription);
                    GlobalVar.Config.Save();

                    if(GlobalVar.MainBackgroundWorker != null)
                    {
                        if(GlobalVar.MainBackgroundWorker.IsBusy== false)
                        {
                            GlobalVar.MainBackgroundWorker.RunWorkerAsync();
                        }
                    }
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