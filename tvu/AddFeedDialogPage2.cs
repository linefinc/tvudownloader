using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace tvu
{
    public partial class AddFeedDialogPage2 : Form
    {
        public List<RssChannel> RssChannelList {private set; get;}
        public List<string> RssUrlList { private set; get; }
        public List<fileHistory> ListFileHistory { private set; get; }
        public List<string> ListCategory { private set; get; }
        private string ServiceUrl;
        private string Password;

        public AddFeedDialogPage2(List<string> RssUrlList, string ServiceUrl, string Password)
        {
            InitializeComponent();
            this.RssUrlList = RssUrlList;
            this.RssChannelList = new List<RssChannel>();
            this.ListFileHistory = new List<fileHistory>();
            this.ListCategory = new List<string>();
            this.ServiceUrl = ServiceUrl;
            this.Password = Password;

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
                    string WebPage = WebFetch.Fetch(url, true);
                    rc = RssParserTVU.Parse(WebPage);
                    rc.Url = url;
                    RssChannelList.Add(rc);
                    backgroundWorker1.ReportProgress(0);
                }
                catch 
                { 
                }
            }

            foreach (RssChannel rssChannel in RssChannelList)
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
                        string page = WebFetch.Fetch(Item.Guid, true);
                        // find ed2k
                        string sEd2k = RssParserTVU.FindEd2kLink(page);
                        // add to history to avoid redonwload
                        fileHistory file = new fileHistory(sEd2k, rssChannel.Link, rssChannel.Url);
                        ListFileHistory.Add(file);
                        backgroundWorker1.ReportProgress(0);
                    }
                    catch
                    {
                    }
                }

            }

            try
            {
                /// update category list from mule
                eMuleWebManager Service = new eMuleWebManager(ServiceUrl, Password);
                bool? returnCode = Service.LogIn();

                if ((returnCode == null) & (returnCode == false))
                {
                    return;
                }

                ListCategory.AddRange(Service.GetCategory(true));

                Service.LogOut();
            }
            catch
            {

            }
    
            
        }

        private void AddFeedDialogPage2_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
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

