using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using TvUndergroundDownloaderLib;

namespace TvUndergroundDownloader
{
    public partial class AddFeedDialogPage2 : Form
    {
        public List<RssSubscription> rssSubscriptionsList { private set; get; }
        public List<string> RssUrlList { private set; get; }

        public List<string> ListCategory { private set; get; }
        private string ServiceUrl;
        private string Password;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private CookieContainer cookieContainer;

        public AddFeedDialogPage2(List<string> RssUrlList, string ServiceUrl, string Password, CookieContainer cookieContainer, bool FastAdd)
        {
            InitializeComponent();
            this.RssUrlList = RssUrlList;
            this.rssSubscriptionsList = new List<RssSubscription>();
            this.ListCategory = new List<string>();

            this.cookieContainer = cookieContainer;
            this.ServiceUrl = ServiceUrl;
            this.Password = Password;
        }

        private void AddFeedDialogPage2_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            GoogleAnalyticsHelper.TrackEvent("AddFeedPage2");
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
                    RssSubscription rs = new RssSubscription(url, cookieContainer);
                    rs.Update(cookieContainer);
                    rssSubscriptionsList.Add(rs);
                    backgroundWorker1.ReportProgress(0);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error while parse link {0}", url);
                }

                backgroundWorker1.ReportProgress(100 * RssUrlList.IndexOf(url) / RssUrlList.Count);
            }

            //
            // update category list from mule
            //
            try
            {
                eMuleWebManager Service = new eMuleWebManager(ServiceUrl, Password);
                Service.Connect();

                if (Service.IsConnected == false)
                {
                    return;
                }

                ListCategory.AddRange(Service.GetCategories(true));

                Service.Close();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while try to connect to server \"{0}\"", ServiceUrl);
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