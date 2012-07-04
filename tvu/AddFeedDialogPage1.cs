using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;

namespace tvu
{
    public partial class AddFeedDialogPage1 : Form
    {

        public List<string> RssUrlList { private set; get; }

        public AddFeedDialogPage1()
        {
            InitializeComponent();
            this.RssUrlList = new List<string>();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            string textToParse;

            if (textUrl.Enabled == true)
            {   // Manual url
                textToParse = textUrl.Text;
            }
            else
            {   // oplm file
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

            Regex Pattern = new Regex(@"http://(www\.)?tvunderground.org.ru/rss.php\?se_id=\d{1,10}");
            //
            MatchCollection mc = Pattern.Matches(textToParse);
            foreach (Match p in mc)
            {
                RssUrlList.Add(p.Value);
                
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ButClose_Click(object sender, EventArgs e)
        {
            RssUrlList.Clear();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            buttonBrowse.Enabled = true;
            textBox1.Enabled = true;
            textUrl.Enabled = false;
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            buttonBrowse.Enabled = false;
            textBox1.Enabled = false;
            textUrl.Enabled = true;
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

       

    }

}
