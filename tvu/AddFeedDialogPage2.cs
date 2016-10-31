using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;

namespace TvUndergroundDownloader
{
    public partial class AddFeedDialogPage2 : Form
    {
        public List<RssChannel> rssChannelList { private set; get; }
        public List<string> RssUrlList { private set; get; }
        public List<FileHistory> newFilesList { private set; get; }
        public List<string> ListCategory { private set; get; }
        private string ServiceUrl;
        private string Password;
        private bool FastAdd;

        private CookieContainer cookieContainer;

        public AddFeedDialogPage2(List<string> RssUrlList, string ServiceUrl, string Password, CookieContainer cookieContainer, bool FastAdd)
        {
            InitializeComponent();
            this.RssUrlList = RssUrlList;
            this.rssChannelList = new List<RssChannel>();
            this.newFilesList = new List<FileHistory>();
            this.ListCategory = new List<string>();

            this.cookieContainer = cookieContainer;
            this.ServiceUrl = ServiceUrl;
            this.Password = Password;
            this.FastAdd = FastAdd;

        }

        private void AddFeedDialogPage2_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            foreach (string url in RssUrlList)
            {

                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                try
                {
                    RssChannel rc = new RssChannel();
                    string WebPage = WebFetch.Fetch(url, true, cookieContainer);
                    rc = RssParserTVU.Parse(WebPage);
                    rc.Url = url;
                    rssChannelList.Add(rc);
                    backgroundWorker1.ReportProgress(0);
                }
                catch
                {
                }
            }

            if (this.FastAdd == false)
            {
                FeedLinkCache feedLinkCache = new FeedLinkCache();

                foreach (RssChannel rssChannel in rssChannelList)
                {
                    foreach (RssItem Item in rssChannel.ListItem)
                    {
                        if (backgroundWorker1.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }

                        try
                        {
                            // download page
                            string page = WebFetch.Fetch(Item.Guid, true, cookieContainer);
                            // find ed2k
                            string sEd2k = RssParserTVU.FindEd2kLink(page);
                            // add to history to avoid redonwload
                            FileHistory file = new FileHistory(sEd2k, Item.Guid, rssChannel.Url);
                            newFilesList.Add(file);
                            backgroundWorker1.ReportProgress(0);
                            // update feedLinkCache
                            FeedLinkCache.AddFeedLink(Item.Guid, sEd2k, DateTime.Now.ToString("s"));
                        }
                        catch
                        {
                        }
                    }

                }
            }

            //
            // update category list from mule
            //
            try
            {
                eMuleWebManager Service = new eMuleWebManager(ServiceUrl, Password);
                Service.Connect();

                if (Service.isConnected == false)
                {
                    return;
                }

                ListCategory.AddRange(Service.GetCategories(true));

                Service.Close();
            }
            catch
            {

            }


        }

 

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = ((progressBar1.Value + 10) % 100);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }

            this.Close();
        }
    }
}

