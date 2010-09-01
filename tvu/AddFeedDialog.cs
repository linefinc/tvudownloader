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
        public RssFeed Feed {get;private set;}
        private string ServiceUrl;
        private string Password;
        
        public AddFeedDialog(string ServiceUrl, string Password)
        {
            InitializeComponent();
            this.ServiceUrl = ServiceUrl;
            this.Password = Password;
        }

        public AddFeedDialog(string ServiceUrl, string Password, RssFeed feed)
        {
            InitializeComponent();

            this.ServiceUrl = ServiceUrl;
            this.Password = Password;

            textUrl.Text = feed.Url;
            this.comboBoxCategory.Items.Add(feed.Category);
            checkBoxPause.Checked = feed.PauseDownload;
            this.Text = feed.Title;
            butAdd.Text = "Save";

        }

        private void ButClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            if (textUrl.Text.IndexOf("http://tvunderground.org.ru/rss.php") < 0)
            {
                MessageBox.Show("Only tvunderground.org.ru service supported");
                return;
            }

            RssFeed newfeed = new RssFeed();

            XmlDocument doc = new XmlDocument();
            doc.Load(textUrl.Text);

            XmlNodeList elemList = doc.GetElementsByTagName("title");
            newfeed.Title = elemList[0].FirstChild.Value;
            newfeed.Url = textUrl.Text;

            newfeed.PauseDownload = checkBoxPause.Checked;

            newfeed.Category = comboBoxCategory.Text;

            this.Feed = newfeed;
            this.DialogResult = DialogResult.OK;
            this.Close();
            
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

            foreach (string t in category)
            {
                comboBoxCategory.Items.Add(t);
            }

            comboBoxCategory.SelectedIndex = 0;

            Service.LogOut();

            comboBoxCategory.Enabled = true;
            butUpdateCategory.Enabled = true;
        }

        






    }
}
