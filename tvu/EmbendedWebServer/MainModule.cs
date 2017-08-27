using Nancy;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Dynamic;
using System.IO;
using System.Net;

//namespace TvUndergroundDownloader.EmbendedWebServer
namespace TvUndergroundDownloader.EmbendedWebServer
{
    public class MainModule : NancyModule
    {
        public dynamic Model = new ExpandoObject();
        private Logger logger = LogManager.GetCurrentClassLogger();

        public MainModule()
        {
            Get["/"] = args =>
            {
                Model.RssFeedList = GlobalVar.Config.RssFeedList;
                Model.LastActivity = GlobalVar.Config.RssFeedList.GetLastActivity();
                return View["files", Model];
            };

            Get["/files"] = args =>
            {
                Model.RssFeedList = GlobalVar.Config.RssFeedList;
                Model.LastActivity = GlobalVar.Config.RssFeedList.GetLastActivity();
                return View["files", Model];
            };

            Get["/update"] = args =>
            {
                return View["log"];
            };

            Get["/channels"] = args =>
            {
                Model.RssFeedList = GlobalVar.Config.RssFeedList;

                if (GlobalVar.Config.RssFeedList.Count > 0)
                {
                    var channel = GlobalVar.Config.RssFeedList[0];
                    Model.DownloadFile = channel.GetDownloadFile();
                }
                return View["channels", Model];
            };

            Get["/channels/{id}"] = args =>
            {
                int sessionID = (int)args["id"];
                var channel = GlobalVar.Config.RssFeedList.Find((obj) => obj.seasonID == sessionID);

                Model.RssFeedList = GlobalVar.Config.RssFeedList;
                Model.DownloadFile = channel.GetDownloadFile();
                foreach(DownloadFile file in Model.DownloadFile)
                {
                    
                }
                return View["channels", Model];
            };

          
            Get["/log"] = args =>
            {
                LoggingConfiguration config = LogManager.Configuration;
                MemoryTarget memoryTarget = config.FindTargetByName("MemoryTarget") as MemoryTarget;
                return View["log", memoryTarget.Logs];
            };

            Get["/RssSubscription/Delete/{id}"] = args =>
            {
                int sessionID = (int)args["id"];
                var channel = GlobalVar.Config.RssFeedList.Find((obj) => obj.seasonID == sessionID);
                GlobalVar.Config.RssFeedList.Remove(channel);
                GlobalVar.Config.Save();
                return Response.AsRedirect("/channels");
            };

            Get["/RssSubscription/MarkAsDownloaded/{id}"] = args =>
            {
               /* int sessionID = (int)args["id"];
                var channel = GlobalVar.Config.RssFeedList.Find((obj) => obj.seasonID == sessionID);
                GlobalVar.Config.RssFeedList.Remove(channel);
                GlobalVar.Config.Save();*/
                return Response.AsRedirect("/channels");
            };

            Get["/RssSubscription/MarkAsNotDownloaded/{id}"] = args =>
            {
                /* int sessionID = (int)args["id"];
                 var channel = GlobalVar.Config.RssFeedList.Find((obj) => obj.seasonID == sessionID);
                 GlobalVar.Config.RssFeedList.Remove(channel);
                 GlobalVar.Config.Save();*/
                return Response.AsRedirect("/channels");
            };

            Get["/fff"] = args =>
            {
                return Response.AsJson(GlobalVar.Config.RssFeedList.GetLastActivity());
            };

            Get["/setup"] = args =>
            {
                return View["setup", GlobalVar.Config];
            };

            Post["/setup"] = args =>
            {
                var action = (string)this.Request.Form.action;
                if (action == "update_setup")
                {
                    logger.Info("WebInterface change setup");
                    string password = (string)this.Request.Form.password;

                }
                return Response.AsRedirect("/setup");
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

                    if (GlobalVar.MainBackgroundWorker != null)
                    {
                        if (GlobalVar.MainBackgroundWorker.IsBusy == false)
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