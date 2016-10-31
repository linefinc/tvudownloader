using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TvUndergroundDownloader
{
    public partial class AddFeedDialogPage3 : Form
    {
        private int index;

        public List<FileHistory> UnselectedFile { get; private set; }
        public List<FileHistory> GlobalListFileHisotry { get; private set; }
        public List<RssChannel> ListRssChannel { get; private set; }

        public AddFeedDialogPage3(List<RssChannel> ListRssChannel, List<FileHistory> GlobalListFileHisotry, List<string> ListCategory)
        {
            InitializeComponent();

            this.ListRssChannel = ListRssChannel;
            this.GlobalListFileHisotry = GlobalListFileHisotry;
            this.UnselectedFile = new List<FileHistory>(); // select all input file
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

            List<FileHistory>  ListFH = GlobalListFileHisotry.FindAll(delegate(FileHistory t) { return t.FeedSource == ListRssChannel[index].Url; });

            foreach (FileHistory fh in ListFH)
            {
                UnselectedFile.RemoveAll(delegate(FileHistory t) { return t == fh; });
            }

            labelSelectedElement.Text = string.Format("Selected elements {0}", this.GlobalListFileHisotry.Count - this.UnselectedFile.Count);
            
        }
        /// <summary>
        /// deSelect all element present in checkListBox1 
        /// </summary>
        private void buttonSelectNone_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < checkedListBox1.Items.Count; index++)
                checkedListBox1.SetItemChecked(index, false);

            List<FileHistory> ListFH = GlobalListFileHisotry.FindAll(delegate(FileHistory t) { return t.FeedSource == ListRssChannel[index].Url; });
            foreach (FileHistory fh in ListFH)
            {
                UnselectedFile.RemoveAll(delegate(FileHistory t) { return t == fh; });
            }

            UnselectedFile.AddRange(ListFH);
            
            labelSelectedElement.Text = string.Format("Selected elements {0}", this.GlobalListFileHisotry.Count - this.UnselectedFile.Count);
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
            List<FileHistory> listFileToDisplay = new List<FileHistory>();

            listFileToDisplay = GlobalListFileHisotry.FindAll(delegate(FileHistory t) { return t.FeedSource == rsschannel.Url; });

            foreach (FileHistory file in listFileToDisplay)
            {
                bool selected = true;

                if (UnselectedFile.IndexOf(file) > -1)
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
            for (int index = 0; index < checkedListBox1.Items.Count; index++)
            {
                // get item (fileHistory) form main list
                string strItem = checkedListBox1.Items[index].ToString();
                FileHistory item = GlobalListFileHisotry.Find(x => x.FileName == strItem);

                // remove all data 
                UnselectedFile.RemoveAll(delegate(FileHistory t) { return t == item; });

                if (checkedListBox1.GetItemChecked(index) == false)
                {
                    UnselectedFile.Add(item);
                }
            }
            labelSelectedElement.Text = string.Format("Selected elements {0}", this.GlobalListFileHisotry.Count - this.UnselectedFile.Count);
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
