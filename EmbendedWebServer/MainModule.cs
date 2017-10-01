using Nancy;
using NLog;
using System;
using System.Dynamic;
using System.Net;

//namespace TvUndergroundDownloader.EmbendedWebServer
namespace TvUndergroundDownloaderLib.EmbendedWebServer
{
    public class MainModule : NancyModule
    {
        public dynamic Model = new ExpandoObject();

        public static Logger logger = LogManager.GetCurrentClassLogger();

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

            Get["/channels/{id}"] = args =>
            {
                int sessionID = (int)args["id"];
                RssSubscription channel = GlobalVar.Config.RssFeedList.Find((obj) => obj.SeasonId == sessionID);
                
                Model.RssFeedList = GlobalVar.Config.RssFeedList;
                Model.LastActivity = GlobalVar.Config.RssFeedList.GetLastActivity();
                Model.DownloadFile = channel.GetDownloadFile();
                return View["channels", Model];
            };

            Get["/update"] = _ =>
            {
                //GlobalVar.Worker.Run();
                if (GlobalVar.Worker.IsBusy)
                {
                    logger.Info("Thread is busy");
                }
                else
                {
                    GlobalVar.Worker.Run();
                }

                return Response.AsRedirect("/log");
            };

            Get["/setup"] = agrs =>
            {
                var configModel = new ConfigModel();
                configModel.ServiceTypeEmule = GlobalVar.Config.ServiceType == Config.eServiceType.eMule;
                configModel.ServiceTypeAmule = GlobalVar.Config.ServiceType == Config.eServiceType.aMule;
                configModel.ServiceUrl = GlobalVar.Config.ServiceUrl;
                configModel.Password = GlobalVar.Config.Password;
                configModel.TVUCookieH = GlobalVar.Config.TVUCookieH;
                configModel.TVUCookieI = GlobalVar.Config.TVUCookieI;
                configModel.TVUCookieT = GlobalVar.Config.TVUCookieT;
                configModel.CloseEmuleIfAllIsDone = GlobalVar.Config.CloseEmuleIfAllIsDone;
                configModel.MaxSimultaneousFeedDownloadsDefault = GlobalVar.Config.MaxSimultaneousFeedDownloadsDefault;
                configModel.PauseDownloadDefault = GlobalVar.Config.PauseDownloadDefault;
                configModel.MinToStartEmule = GlobalVar.Config.MinToStartEmule;
                configModel.eMuleExe = GlobalVar.Config.eMuleExe;
                configModel.EmailNotification = GlobalVar.Config.EmailNotification;
                configModel.ServerSMTP = GlobalVar.Config.ServerSMTP;
                configModel.MailSender = GlobalVar.Config.MailSender;
                configModel.MailReceiver = GlobalVar.Config.MailReceiver;
                return View["setup", configModel];
            };

            Post["/setup"] = args =>
            {
                try
                {
                    string serviceType = (string)Request.Form.ServiceType;
                    string serviceUrl = (string)Request.Form.ServiceUrl;
                    string password = (string)Request.Form.Password;
                    string tvuCookieH = (string)Request.Form.TVUCookieH;
                    string tvuCookieI = (string)Request.Form.TVUCookieI;
                    string tvuCookieT = (string)Request.Form.TVUCookieT;
                    bool closeEmuleIfAllIsDone = (bool)Request.Form.CloseEmuleIfAllIsDone;
                    uint maxSimultaneousFeedDownloadsDefault = (uint)Request.Form.MaxSimultaneousFeedDownloadsDefault;
                    bool pauseDownloadDefault = (bool)Request.Form.PauseDownloadDefault;
                    int minToStartEmule = (int)Request.Form.MinToStartEmule;
                    string eMuleExe = (string)Request.Form.eMuleExe;
                    bool emailNotification = (bool)Request.Form.EmailNotification;
                    string serverSMTP = (string)Request.Form.ServerSMTP;
                    string mailSender = (string)Request.Form.MailSender;
                    string mailReceiver = (string)Request.Form.MailReceiver;

                    Config config = GlobalVar.Config;
                    config.ServiceType = (Config.eServiceType)Enum.Parse(typeof(Config.eServiceType), serviceType);
                    config.ServiceUrl = serviceUrl;
                    config.Password = password;
                    config.TVUCookieH = tvuCookieH;
                    config.TVUCookieI = tvuCookieI;
                    config.TVUCookieT = tvuCookieT;
                    config.CloseEmuleIfAllIsDone = closeEmuleIfAllIsDone;
                    config.MaxSimultaneousFeedDownloadsDefault = maxSimultaneousFeedDownloadsDefault;
                    config.PauseDownloadDefault = pauseDownloadDefault;
                    config.MinToStartEmule = minToStartEmule;
                    config.eMuleExe = eMuleExe;
                    config.EmailNotification = emailNotification;
                    config.ServerSMTP = serverSMTP;
                    config.MailSender = mailSender;
                    config.MailReceiver = mailReceiver;

                    config.Save();
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
                return Response.AsRedirect("/setup");
            };

            Get["/RssSubscription/Delete/{id}"] = args =>
            {
                int sessionID = (int)args["id"];
                var channel = GlobalVar.Config.RssFeedList.Find((obj) => obj.SeasonId == sessionID);
                GlobalVar.Config.RssFeedList.Remove(channel);
                GlobalVar.Config.Save();
                return Response.AsRedirect("/channels");
            };

            Get["/log"] = arg =>
            {
                return View["log", GlobalVar.LogMemoryTarget.Logs];
            };

            Post["/RssSubscription"] = args =>
            {
                var action = (string)this.Request.Form.action;
                var newFeedUrl = (string)this.Request.Form.newFeedUrl;

                if (action == "add_new_feed")
                {
                    CookieContainer cookieContainer = new CookieContainer();
                    Uri uriTvunderground = new Uri("http://tvunderground.org.ru/");
                    cookieContainer.Add(uriTvunderground, new Cookie("h", GlobalVar.Config.TVUCookieH));
                    cookieContainer.Add(uriTvunderground, new Cookie("i", GlobalVar.Config.TVUCookieI));
                    cookieContainer.Add(uriTvunderground, new Cookie("t", GlobalVar.Config.TVUCookieT));
                    var newSubscription = new RssSubscription(newFeedUrl, cookieContainer);

                    if (!string.IsNullOrEmpty(GlobalVar.Config.DefaultCategory))
                    {
                        newSubscription.Category = GlobalVar.Config.DefaultCategory;
                    }
                    
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

                return Response.AsRedirect("/");
            };

            Get["/favicon.ico"] = x => { return View["favicon.ico"]; };
        }
    }
}