namespace TvUndergroundDownloaderLib.EmbendedWebServer
{
    public class ConfigModel
    {
        public bool CloseEmuleIfAllIsDone;

        public string DefaultCategory;

        public bool EmailNotification;

        public string eMuleExe;

        public string MailReceiver;

        public string MailSender;

        public uint MaxSimultaneousFeedDownloadsDefault;

        public int MinToStartEmule;

        public string Password;

        public bool PauseDownloadDefault;
        public string ServerSMTP;
        public bool ServiceTypeAmule;
        public bool ServiceTypeEmule;
        public string ServiceUrl;

        public bool StartEmuleIfClose;

        public bool StartMinimized;

       

        public string TvuUserName { get; internal set; }
        public string TvuPassword { get; internal set; }
    }
}