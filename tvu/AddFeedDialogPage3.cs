using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TvUndergroundDownloader
{
    public partial class AddFeedDialogPage3 : Form
    {
        private int index;

        public List<fileHistory> UnselectedHistory { get; private set; }
        public List<fileHistory> GlobalListFileHisotry { get; private set; }
        public List<RssChannel> ListRssChannel { get; private set; }

        public AddFeedDialogPage3(List<RssChannel> ListRssChannel, List<fileHistory> GlobalListFileHisotry, List<string> ListCategory)
        {
            InitializeComponent();

            this.ListRssChannel = ListRssChannel;
            this.GlobalListFileHisotry = GlobalListFileHisotry;
            this.UnselectedHistory = new List<fileHistory>(); // select all input file
            labelSelectedElement.Text = string.Format("Selected elements {0}", this.GlobalListFileHisotry.Count);
            index = 0;


            foreach(var str in ListCategory)
            {
                comboBoxCategory.Items.Add(str);
            }
        }     

        private void AddFeedDialogPage3_Load(object sender, EventArgs e)
        {
            RefreshList();
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
            for (int index = 0; index < checkedListBox1.Items.Count; index++)
                checkedListBox1.SetItemChecked(index, true);

            List<fileHistory>  ListFH = GlobalListFileHisotry.FindAll(delegate(fileHistory t) { return t.FeedSource == ListRssChannel[index].Url; });

            foreach (fileHistory fh in ListFH)
            {
                UnselectedHistory.RemoveAll(delegate(fileHistory t) { return t == fh; });
            }

            labelSelectedElement.Text = string.Format("Selected elements {0}", this.GlobalListFileHisotry.Count - this.UnselectedHistory.Count);
            
        }
        /// <summary>
        /// deSelect all element present in checkListBox1 
        /// </summary>
        private void buttonSelectNone_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < checkedListBox1.Items.Count; index++)
                checkedListBox1.SetItemChecked(index, false);

            List<fileHistory> ListFH = GlobalListFileHisotry.FindAll(delegate(fileHistory t) { return t.FeedSource == ListRssChannel[index].Url; });
            foreach (fileHistory fh in ListFH)
            {
                UnselectedHistory.RemoveAll(delegate(fileHistory t) { return t == fh; });
            }
            
            labelSelectedElement.Text = string.Format("Selected elements {0}", this.GlobalListFileHisotry.Count - this.UnselectedHistory.Count);
        }


        private void buttonNext_Click(object sender, EventArgs e)
        {
            index = Math.Min(index + 1, ListRssChannel.Count);
            RefreshList();
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            index = Math.Max(index - 1, 0);
            RefreshList();
        }

        /// <summary>
        /// refresh gui with data ListRssChannel[index]
        /// </summary>
        private void RefreshList()
        {
            RssChannel rsschannel = ListRssChannel[index];
            comboBoxCategory.Text = rsschannel.Category;
            checkBoxPause.Checked = rsschannel.Pause;

            // check max and min value allowed to numericUpDownMaxSimulDown
            decimal tempDec = Math.Max(rsschannel.maxSimultaneousDownload, numericUpDownMaxSimulDown.Minimum);
            tempDec = Math.Min(tempDec, numericUpDownMaxSimulDown.Maximum);
            numericUpDownMaxSimulDown.Value = tempDec;


            // add file
            checkedListBox1.Items.Clear();
            List<fileHistory> listFileToDisplay = new List<fileHistory>();

            listFileToDisplay = GlobalListFileHisotry.FindAll(delegate(fileHistory t) { return t.FeedSource == rsschannel.Url; });

            foreach (fileHistory file in listFileToDisplay)
            {
                bool selected = true;

                if (UnselectedHistory.IndexOf(file) > -1)
                {
                    selected = false;
                }
                checkedListBox1.Items.Add(file.FileName, selected);
            }
            buttonNext.Enabled = (ListRssChannel.Count-1) != index;
            buttonPrevious.Enabled = index != 0;
        }
        /// <summary>
        /// close dialog
        /// </summary>
        private void buttonFinish_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void checkedListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            for (int index = 0; index < checkedListBox1.Items.Count; index++)
            {
                // get item (fileHistory) form main list
                string strItem = checkedListBox1.Items[index].ToString();
                fileHistory item = GlobalListFileHisotry.Find(delegate(fileHistory t) { return t.FileName == strItem; });

                // remove all data 
                UnselectedHistory.RemoveAll(delegate(fileHistory t) { return t == item; });

                if (checkedListBox1.GetItemChecked(index) == false)
                {
                    UnselectedHistory.Add(item);
                }
            }
            labelSelectedElement.Text = string.Format("Selected elements {0}", this.GlobalListFileHisotry.Count - this.UnselectedHistory.Count);
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListRssChannel[index].Category = comboBoxCategory.Text;
        }

        private void checkBoxPause_CheckedChanged(object sender, EventArgs e)
        {
            ListRssChannel[index].Pause = checkBoxPause.Checked;
        }

        private void numericUpDownMaxSimulDown_ValueChanged(object sender, EventArgs e)
        {
            ListRssChannel[index].maxSimultaneousDownload = Convert.ToInt16(numericUpDownMaxSimulDown.Value);
        }

   
    }
}
