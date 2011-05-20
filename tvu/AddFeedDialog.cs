using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace tvu
{
    public partial class AddFeedDialog : Form
    {
        public RssSubscrission NewFeed {get;private set;}
        public List<fileHistory> NewHistory { get; private set; }
        private string ServiceUrl;
        private string Password;
        private Rss RssChannel;



        public AddFeedDialog(string ServiceUrl, string Password, string DefaultCategory)
        {
            InitializeComponent();
            this.ServiceUrl = ServiceUrl;
            this.Password = Password;
            this.comboBoxCategory.Text = DefaultCategory;
            this.NewHistory = new List<fileHistory>();
            this.NewFeed = new RssSubscrission();

        }

        //public AddFeedDialog(string ServiceUrl, string Password, RssSubscrission feed)
        //{
        //    InitializeComponent();

        //    this.ServiceUrl = ServiceUrl;
        //    this.Password = Password;

        //    textUrl.Text = feed.Url;
        //    this.comboBoxCategory.Items.Add(feed.Category);
        //    checkBoxPause.Checked = feed.PauseDownload;
        //    this.Text = feed.Title;
        //    butAdd.Text = "Save";

        //}

        private void ButClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy == true)
            {
                return;
            }
            buttonGetFeed.Enabled = false;
            butAdd.Enabled = false;
            ButClose.Enabled = false;
            backgroundWorker1.RunWorkerAsync();

           
        
            
        }

        private void butUpdateCategory_Click(object sender, EventArgs e)
        {
            comboBoxCategory.Enabled = false;
            butUpdateCategory.Enabled = false;
            comboBoxCategory.Items.Clear();

            eMuleWebManager Service = new eMuleWebManager(ServiceUrl, Password);
            bool? rc = Service.LogIn();

            if ((rc == null) & (rc == false))
            {
                MessageBox.Show("Unable to connect Emule service");
                return;
            }

            List<string> category = new List<string>();
            category.AddRange(Service.GetCategory(true));

            if (category.Count == 0)
            {
                comboBoxCategory.Items.Clear();
                Service.LogOut();
                comboBoxCategory.Enabled = true;
                butUpdateCategory.Enabled = true;                
                return;
            }

            foreach (string t in category)
            {
                comboBoxCategory.Items.Add(t);
            }

            comboBoxCategory.SelectedIndex = 0;

            Service.LogOut();

            comboBoxCategory.Enabled = true;
            butUpdateCategory.Enabled = true;
        }


        public bool checkLink(string url)
        {
            if (url.IndexOf("http://tvunderground.org.ru/rss.php") >= 0)
            {
                return true;
            }

            if (url.IndexOf("http://www.tvunderground.org.ru/rss.php") >= 0)
            {
                return true;
            }

            return false;
        }

        private void buttonGetFeed_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkLink(textUrl.Text) == false)
                {
                    MessageBox.Show("Only tvunderground.org.ru service supported");
                    return;
                }

                string WebPage = WebFetch.Fetch(textUrl.Text,true);

                RssChannel = RssParserTVU.Parse(WebPage);
                RssChannel.Url = textUrl.Text;

                checkedListBox1.Items.Clear();
                foreach (RssItem Item in RssChannel.ListItem)
                {
                    checkedListBox1.Items.Add(Item.Title);
                }

                butAdd.Enabled = true;
                
            }
            catch
            {
                MessageBox.Show("Error to read or parse RSS feed");
            }

            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 0;

            foreach (RssItem Item in RssChannel.ListItem)
            {

                if (checkedListBox1.CheckedItems.Contains(Item.Title) == false)
                {
                    try
                    {
                        // download page
                        string page = WebFetch.Fetch(Item.Guid,true);
                        // find ed2k
                        string sEd2k = RssParserTVU.FindEd2kLink(page);
                        // add to history to avoid redonwload
                        fileHistory file = new fileHistory();

                        file.FeedSource = RssChannel.Url;
                        file.Ed2kLink = sEd2k;
                        file.FeedLink = RssChannel.Link;
                        NewHistory.Add(file);

                    }
                    catch
                    {

                    }
                }

                backgroundWorker1.ReportProgress((int)(++i * 100.0f / RssChannel.ListItem.Count));


            }
       
       
    }

        /// <summary>
        /// This is on the main thread, so we can update a TextBox or anything.
        /// </summary>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonGetFeed.Enabled = true;
            butAdd.Enabled = true;
            ButClose.Enabled = true;

            this.NewFeed = new RssSubscrission();

            this.NewFeed.Title = RssChannel.Title;
            this.NewFeed.Url = RssChannel.Url;
            this.NewFeed.PauseDownload = checkBoxPause.Checked;
            this.NewFeed.Category = comboBoxCategory.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// show progres on Progress bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void textUrl_TextChanged(object sender, EventArgs e)
        {
            butAdd.Enabled = false;
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < checkedListBox1.Items.Count; index++)
                checkedListBox1.SetItemChecked(index, true);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < checkedListBox1.Items.Count; index++)
                checkedListBox1.SetItemChecked(index, false);
        }

   
    }
}
