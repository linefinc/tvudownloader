using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TvUndergroundDownloaderLib;

namespace TvUndergroundDownloader
{
    public partial class AddFeedDialogPage1 : Form
    {
        private List<string> CurrentRssUrlList;
        public AddFeedDialogPage1(List<string> CurrentRssUrlList)
        {
            InitializeComponent();
            this.RssUrlList = new List<string>();
            this.CurrentRssUrlList = CurrentRssUrlList;
            this.FastAdd = false;
        }

        public bool FastAdd { private set; get; }
        public List<string> RssUrlList { private set; get; }

        private void AddFeedDialogPage1_Load(object sender, EventArgs e)
        {
            GoogleAnalyticsHelper.TrackScreen("AddFeedPage1");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            RssUrlList.Clear();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            List<string> Feed = new List<string>();

            openFileDialog1.Filter = "All files (*.*)|*.*|OPML (*.opml)|*.opml";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                return;
            }
            textBox1.Text = string.Empty;
            return;
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            this.FastAdd = true;
            buttonNext_Click(sender, e);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            string textToParse;

            if (textUrl.Enabled == true)
            {   // Manual URL
                textToParse = textUrl.Text;
            }
            else
            {   // OPML file
                try
                {
                    StreamReader frStream = new StreamReader(textBox1.Text);
                    //
                    textToParse = string.Empty;
                    string input;
                    while ((input = frStream.ReadLine()) != null)
                    {
                        textToParse += input;
                    }
                    frStream.Close();
                }
                catch
                {
                    MessageBox.Show("Error: Could not read file from disk.");
                    return;
                }
            }

            // Static Regex "http(s)?://(www\.)?tvunderground.org.ru/rss.php\?se_id=(\d{1,10})"
            MatchCollection mc = RssSubscription.regexFeedSource.Matches(textToParse);
            foreach (Match p in mc)
            {
                RssUrlList.Add(p.Value);
            }

            // remove duplicate, skipping source
            foreach (string rssUrl in RssUrlList)
            {
                if (CurrentRssUrlList.IndexOf(rssUrl) != -1)
                {
                    MessageBox.Show(string.Format("Warning: Rss URL duplicate ({0}), skipping source", rssUrl));
                }
            }

            RssUrlList.RemoveAll(delegate (string temp) { return CurrentRssUrlList.IndexOf(temp) > -1; });

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            buttonBrowse.Enabled = false;
            textBox1.Enabled = false;
            textUrl.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            buttonBrowse.Enabled = true;
            textBox1.Enabled = true;
            textUrl.Enabled = false;
        }

    }
}