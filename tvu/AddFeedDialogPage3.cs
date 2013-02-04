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
    public partial class AddFeedDialogPage3 : Form
    {
        private int index;
        
        public List<fileHistory> SelectedHistory { get; private set; }
        public List<fileHistory> GlobalListFileHisotry { get; private set; }

        public List<RssChannel> ListRssChannel { get; private set; }
        
        public AddFeedDialogPage3( List<RssChannel> ListRssChannel, List<fileHistory> GlobalListFileHisotry)
        {
            InitializeComponent();

            this.ListRssChannel = ListRssChannel;
            this.GlobalListFileHisotry = GlobalListFileHisotry;
            this.SelectedHistory = new List<fileHistory>(GlobalListFileHisotry); // select all input file
            labelSelectedElement.Text = string.Format("Selected elements {0}", this.SelectedHistory.Count);


            index = 0;

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
   
        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < checkedListBox1.Items.Count; index++)
                checkedListBox1.SetItemChecked(index, true);


            List<fileHistory>  ListFH = GlobalListFileHisotry.FindAll(delegate(fileHistory t) { return t.FeedSource == ListRssChannel[index].Url; });

            foreach (fileHistory fh in ListFH)
            {
                SelectedHistory.RemoveAll(delegate(fileHistory t) { return t == fh; });
            }

            SelectedHistory.AddRange(ListFH);
            labelSelectedElement.Text = string.Format("Selected elements {0}", this.SelectedHistory.Count);
           
            
        }
        /// <summary>
        /// deSelect all element present in checkListBox1 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectNone_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < checkedListBox1.Items.Count; index++)
                checkedListBox1.SetItemChecked(index, false);

            List<fileHistory> ListFH = GlobalListFileHisotry.FindAll(delegate(fileHistory t) { return t.FeedSource == ListRssChannel[index].Url; });
            foreach (fileHistory fh in ListFH)
            {
                SelectedHistory.RemoveAll(delegate(fileHistory t) { return t == fh; });
            }

            labelSelectedElement.Text = string.Format("Selected elements {0}", this.SelectedHistory.Count);
            
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

        private void RefreshList()
        {
            
            RssChannel rsschannel = ListRssChannel[index];
            comboBoxCategory.Text = rsschannel.Category;
            checkBoxPause.Checked = ListRssChannel[index].Pause;

            // add file
            checkedListBox1.Items.Clear();
            List<fileHistory> listFileToDisplay = new List<fileHistory>();

            listFileToDisplay = GlobalListFileHisotry.FindAll(delegate(fileHistory t) { return t.FeedSource == rsschannel.Url; });

            foreach (fileHistory file in listFileToDisplay)
            {
                bool selected = false;

                if (SelectedHistory.IndexOf(file) > -1)
                {
                    selected = true;
                 
                }
                
                checkedListBox1.Items.Add(file.FileName, selected);
            }
            
            
            buttonNext.Enabled = (ListRssChannel.Count-1) != index;
            buttonPrevious.Enabled = index != 0;


        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void checkedListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            
            
            for(int index =0; index < checkedListBox1.Items.Count; index++)
            {
                bool checkedstated = checkedListBox1.GetItemChecked(index);

                fileHistory item = GlobalListFileHisotry.Find(delegate(fileHistory t) { return t.FileName == checkedListBox1.Items[index]; });

                SelectedHistory.RemoveAll(delegate(fileHistory t) { return t == item; });

                if (checkedstated == true)
                {
                    SelectedHistory.Add(item);
                }

            }

            labelSelectedElement.Text = string.Format("Selected elements {0}", this.SelectedHistory.Count);

        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListRssChannel[index].Category = comboBoxCategory.Text;
        }

        private void checkBoxPause_CheckedChanged(object sender, EventArgs e)
        {
            ListRssChannel[index].Pause = checkBoxPause.Checked;
        }

   
    }
}
