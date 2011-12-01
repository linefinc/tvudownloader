using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace tvu
{
    public partial class FormOPMLImporter : Form
    {
        private List<Rss> ListRss;
        private List<string> myItemChecked;
        private bool secureSelect = false;
        public FormOPMLImporter()
        {
            InitializeComponent();
            ListRss = new List<Rss>();
            myItemChecked = new List<string>();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string strOPML ="";
            List<string> Feed = new List<string>();


            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*|OPML (*.opml)|*.opml";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader frStream = new StreamReader(openFileDialog1.FileName);
                    //
                    string input;
                    while ((input = frStream.ReadLine()) != null)
                    {
                        strOPML += input;   
                    }
                    frStream.Close();

                    Regex Pattern = new Regex(@"http://tvunderground.org.ru/rss.php\?se_id=\d{1,10}");

                    MatchCollection ppp = Pattern.Matches(strOPML);
                    foreach (Match p in ppp)
                    {
                        Feed.Add(p.Value);
                    }

                    Pattern = new Regex(@"http://www.tvunderground.org.ru/rss.php\?se_id=\d{1,10}");
                    ppp = Pattern.Matches(strOPML);
                    foreach (Match p in ppp)
                    {
                        Feed.Add(p.Value);
                    }


                    foreach (string p in Feed)
                    {
                        string WebPage = WebFetch.Fetch(p,true);
                        Rss RssChannel = RssParserTVU.Parse(WebPage);
                        ListRss.Add(RssChannel);
                    }

                    ShowRssFilter();
                    ShowRssData("");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        }

        private void ShowRssFilter()
        {
            comboBox1.Items.Clear();

            comboBox1.Items.Add("");
            foreach (Rss p in ListRss)
            {
                comboBox1.Items.Add(p.Title);
            }
        }


        private void ShowRssData(string feed)
        {
            
            checkedListBox2.Items.Clear();
            
            foreach (Rss p in ListRss)
            {
                
                if (feed == "")
                {
                    foreach (RssItem Item in p.ListItem)
                    {
                        checkedListBox2.Items.Add(Item.Title);
                        
                    }
                }

                if ((feed == p.Title)&&(feed != ""))
                {
                    foreach (RssItem Item in p.ListItem)
                    {
                        checkedListBox2.Items.Add(Item.Title);
                    }
                }
            }

            foreach (string name in myItemChecked)
            {
                int index = checkedListBox2.FindString(name);
                if (index != -1)
                {
                    checkedListBox2.SetItemCheckState(index, CheckState.Checked);
                }
            }

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {

        }

       

        private void checkedListBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (secureSelect == true)
            {
                return;
            }
            
            if (checkedListBox2.SelectedIndex == -1)
            {
                return;
            }
                
            string curItem = checkedListBox2.SelectedItem.ToString();
                
            // Find the string in ListBox2.
            int index = checkedListBox2.FindString(curItem);


            if (checkedListBox2.CheckedIndices.IndexOf(index) == -1)    //unchecked -> select and add to list
            {
                myItemChecked.Add(curItem);
            }
            else // checked -> deselect and remove from list
            {
                myItemChecked.Remove(curItem);
            }
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {

            checkedListBox2.BeginUpdate();
            for (int i = 0; i < checkedListBox2.Items.Count; i++ )
            {
                checkedListBox2.SetItemCheckState(i, CheckState.Checked);
            }
            checkedListBox2.EndUpdate();
        }

        private void buttonSelectNone_Click(object sender, EventArgs e)
        {
            checkedListBox2.BeginUpdate();
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemCheckState(i,CheckState.Unchecked);
            }
            checkedListBox2.EndUpdate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int index = comboBox1.SelectedIndex;
            string strSelect = comboBox1.SelectedItem.ToString();
            ShowRssData(strSelect);
        }
    }
}
