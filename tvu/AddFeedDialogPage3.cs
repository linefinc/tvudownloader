using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using TvUndergroundDownloaderLib;

namespace TvUndergroundDownloader
{
    public partial class AddFeedDialogPage3 : Form
    {
        private int _subscriptionListIndex;
        public List<RssSubscription> RssSubscriptionList { get; private set; }

        public AddFeedDialogPage3(List<RssSubscription> RssSubscriptionList, List<string> ListCategory)
        {
            InitializeComponent();

            this.RssSubscriptionList = RssSubscriptionList;
            labelSelectedElement.Text = string.Format("Selected elements {0}", this.RssSubscriptionList[0].Files.Count);
            _subscriptionListIndex = 0;

            foreach (var str in ListCategory)
            {
                comboBoxCategory.Items.Add(str);
            }
        }

        private void AddFeedDialogPage3_Load(object sender, EventArgs e)
        {
            RefreshList();
            GoogleAnalyticsHelper.TrackScreen("AddFeedPage3");
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Select all element in checkedListBox
        /// </summary>
        /// <remarks>
        /// Remove all element from Unselect
        /// </remarks>
        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            RssSubscription rssSubscription = RssSubscriptionList[_subscriptionListIndex];
            ReadOnlyCollection<DownloadFile> listAllFiles = rssSubscription.Files;

            for (int index = 0; index < checkedListBox1.Items.Count; index++)
            {
                checkedListBox1.SetItemChecked(index, true);

                // get item (fileHistory) form main list
                string strFileName = checkedListBox1.Items[index].ToString();

                var file = listAllFiles.FirstOrDefault((o) => o.FileName == strFileName);
                if (file == null)
                {
                    continue;
                }
                rssSubscription.SetFileNotDownloaded(file);
            }
        }

        /// <summary>
        /// deSelect all element present in checkListBox1
        /// </summary>
        private void buttonSelectNone_Click(object sender, EventArgs e)
        {
            RssSubscription rssSubscription = RssSubscriptionList[_subscriptionListIndex];
            ReadOnlyCollection<DownloadFile> listAllFiles = rssSubscription.Files;

            for (int index = 0; index < checkedListBox1.Items.Count; index++)
            {
                checkedListBox1.SetItemChecked(index, true);

                // get item (fileHistory) form main list
                string strFileName = checkedListBox1.Items[index].ToString();
                bool isChecked = checkedListBox1.GetItemChecked(index);

                Ed2kfile File = listAllFiles.FirstOrDefault((o) => o.FileName == strFileName);
                if (File == null)
                {
                    continue;
                }
                rssSubscription.SetFileDownloaded(File);
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            _subscriptionListIndex = Math.Min(_subscriptionListIndex + 1, RssSubscriptionList.Count);
            RefreshList();
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            _subscriptionListIndex = Math.Max(_subscriptionListIndex - 1, 0);
            RefreshList();
        }

        /// <summary>
        /// refresh gui with data ListRssChannel[index]
        /// </summary>
        private void RefreshList()
        {
            RssSubscription rssSubscription = RssSubscriptionList[_subscriptionListIndex];
            comboBoxCategory.Text = rssSubscription.Category;
            checkBoxPause.Checked = rssSubscription.PauseDownload;
            numericUpDownMaxSimulDown.Value = rssSubscription.MaxSimultaneousDownload;

            //// check max and min value allowed to numericUpDownMaxSimulDown
            decimal tempDec = Math.Max(rssSubscription.MaxSimultaneousDownload, numericUpDownMaxSimulDown.Minimum);
            tempDec = Math.Min(tempDec, numericUpDownMaxSimulDown.Maximum);
            numericUpDownMaxSimulDown.Value = tempDec;

            //// add file
            checkedListBox1.Items.Clear();
            List<Ed2kfile> listDownloadedFile = rssSubscription.GetDownloadedFiles();
            ReadOnlyCollection<DownloadFile> listAllFile = rssSubscription.Files;
            foreach (DownloadFile file in listAllFile)
            {
                int index = checkedListBox1.Items.Add(file.FileName);
                bool isDownloaded = listDownloadedFile.Contains(file);
                checkedListBox1.SetItemChecked(index, isDownloaded == false);
            }
            buttonNext.Enabled = (RssSubscriptionList.Count - 1) != _subscriptionListIndex;
            buttonPrevious.Enabled = _subscriptionListIndex != 0;
        }

        /// <summary>
        /// close dialog
        /// </summary>
        private void buttonFinish_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBox1_SelectedValueChanged(sender, e);
        }

        private void checkedListBox1_MouseClick(object sender, MouseEventArgs e)
        {
            checkedListBox1_SelectedValueChanged(sender, e);
        }

        private void checkedListBox1_KeyUp(object sender, KeyEventArgs e)
        {
            checkedListBox1_SelectedValueChanged(sender, e);
        }

        private void checkedListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            RssSubscription rssSubscription = RssSubscriptionList[_subscriptionListIndex];
            ReadOnlyCollection<DownloadFile> listAllFiles = rssSubscription.Files;

            for (int index = 0; index < checkedListBox1.Items.Count; index++)
            {
                // get item (fileHistory) form main list
                string strFileName = checkedListBox1.Items[index].ToString();
                bool isChecked = checkedListBox1.GetItemChecked(index);

                Ed2kfile file = listAllFiles.FirstOrDefault((o) => o.FileName == strFileName);
                if (file == null)
                {
                    continue;
                }
                //
                // not selected => to not download
                // selected => to download
                //
                if (isChecked == false)
                {
                    if (!rssSubscription.GetDownloadedFiles().Contains(file))
                    {
                        rssSubscription.SetFileDownloaded(file);
                    }
                }
                else
                {
                    if (rssSubscription.GetDownloadedFiles().Contains(file))
                    {
                        rssSubscription.SetFileNotDownloaded(file);
                    }
                }
            }
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            RssSubscriptionList[_subscriptionListIndex].Category = comboBoxCategory.Text;
        }

        private void checkBoxPause_CheckedChanged(object sender, EventArgs e)
        {
            RssSubscription rssSubscription = RssSubscriptionList[_subscriptionListIndex];
            rssSubscription.PauseDownload = checkBoxPause.Checked;
        }

        private void numericUpDownMaxSimulDown_ValueChanged(object sender, EventArgs e)
        {
            RssSubscription rssSubscription = RssSubscriptionList[_subscriptionListIndex];
            rssSubscription.MaxSimultaneousDownload = Convert.ToUInt32(numericUpDownMaxSimulDown.Value);
        }
    }
}