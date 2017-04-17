using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TvUndergroundDownloader
{
    public partial class AddFeedDialogPage3 : Form
    {
        private int subscriptionListIndex;
        private RssSubscription currenctView = null;
        public List<RssSubscription> rssSubscriptionList { get; private set; }

        public AddFeedDialogPage3(List<RssSubscription> RssSubscriptionList, List<string> ListCategory)
        {
            InitializeComponent();

            this.rssSubscriptionList = RssSubscriptionList;
            labelSelectedElement.Text = string.Format("Selected elements {0}", this.rssSubscriptionList[0].GetAllFile().Count);
            subscriptionListIndex = 0;
            currenctView = this.rssSubscriptionList[0];

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
            RssSubscription rssSubscription = rssSubscriptionList[subscriptionListIndex];
            List<Ed2kfile> listDownloadedFile = rssSubscription.GetDownloadedFiles();
            List<Ed2kfile> listAllFiles = rssSubscription.GetAllFile();

            for (int index = 0; index < checkedListBox1.Items.Count; index++)
            {
                checkedListBox1.SetItemChecked(index, true);

                // get item (fileHistory) form main list
                string strFileName = checkedListBox1.Items[index].ToString();
                bool isChecked = checkedListBox1.GetItemChecked(index);

                Ed2kfile File = listAllFiles.Find((temp) => temp.FileName == strFileName);
                if (File == null)
                {
                    continue;
                }
                rssSubscription.SetFileNotDownloaded(File);
            }
        }

        /// <summary>
        /// deSelect all element present in checkListBox1
        /// </summary>
        private void buttonSelectNone_Click(object sender, EventArgs e)
        {
            RssSubscription rssSubscription = rssSubscriptionList[subscriptionListIndex];
            List<Ed2kfile> listDownloadedFile = rssSubscription.GetDownloadedFiles();
            List<Ed2kfile> listAllFiles = rssSubscription.GetAllFile();

            for (int index = 0; index < checkedListBox1.Items.Count; index++)
            {
                checkedListBox1.SetItemChecked(index, true);

                // get item (fileHistory) form main list
                string strFileName = checkedListBox1.Items[index].ToString();
                bool isChecked = checkedListBox1.GetItemChecked(index);

                Ed2kfile File = listAllFiles.Find((temp) => temp.FileName == strFileName);
                if (File == null)
                {
                    continue;
                }
                rssSubscription.SetFileDownloaded(File);
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            subscriptionListIndex = Math.Min(subscriptionListIndex + 1, rssSubscriptionList.Count);
            RefreshList();
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            subscriptionListIndex = Math.Max(subscriptionListIndex - 1, 0);
            RefreshList();
        }

        /// <summary>
        /// refresh gui with data ListRssChannel[index]
        /// </summary>
        private void RefreshList()
        {
            RssSubscription rssSubscription = rssSubscriptionList[subscriptionListIndex];
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
            List<Ed2kfile> listAllFile = rssSubscription.GetAllFile();
            foreach (Ed2kfile file in listAllFile)
            {
                int index = checkedListBox1.Items.Add(file.FileName);
                bool isDownloaded = listDownloadedFile.Contains(file);
                checkedListBox1.SetItemChecked(index, isDownloaded == false);
            }
            buttonNext.Enabled = (rssSubscriptionList.Count - 1) != subscriptionListIndex;
            buttonPrevious.Enabled = subscriptionListIndex != 0;
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
            RssSubscription rssSubscription = rssSubscriptionList[subscriptionListIndex];
            List<Ed2kfile> listDownloadedFile = rssSubscription.GetDownloadedFiles();
            List<Ed2kfile> listAllFiles = rssSubscription.GetAllFile();

            for (int index = 0; index < checkedListBox1.Items.Count; index++)
            {
                // get item (fileHistory) form main list
                string strFileName = checkedListBox1.Items[index].ToString();
                bool isChecked = checkedListBox1.GetItemChecked(index);

                Ed2kfile File = listAllFiles.Find((temp) => temp.FileName == strFileName);
                if (File == null)
                {
                    continue;
                }
                //
                // not selected => to not download
                // selected => to download
                //
                if (isChecked == false)
                {
                    if (!rssSubscription.GetDownloadedFiles().Contains(File))
                    {
                        rssSubscription.SetFileDownloaded(File);
                    }
                }
                else
                {
                    if (rssSubscription.GetDownloadedFiles().Contains(File))
                    {
                        rssSubscription.SetFileNotDownloaded(File);
                    }
                }
            }
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            rssSubscriptionList[subscriptionListIndex].Category = comboBoxCategory.Text;
        }

        private void checkBoxPause_CheckedChanged(object sender, EventArgs e)
        {
            RssSubscription rssSubscription = rssSubscriptionList[subscriptionListIndex];
            rssSubscription.PauseDownload = checkBoxPause.Checked;
        }

        private void numericUpDownMaxSimulDown_ValueChanged(object sender, EventArgs e)
        {
            RssSubscription rssSubscription = rssSubscriptionList[subscriptionListIndex];
            rssSubscription.MaxSimultaneousDownload = Convert.ToUInt32(numericUpDownMaxSimulDown.Value);
        }
    }
}