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
        
        
        public FormOPMLImporter()
        {
            InitializeComponent();
            ListRss = new List<Rss>();
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

                    foreach (string p in Feed)
                    {
                        string WebPage = WebFetch.Fetch(p);
                        Rss RssChannel = RssParserTVU.Parse(WebPage);
                        ListRss.Add(RssChannel);
                    }


                    ShowRssData("");
                    
                    

                    
                    

                    
                    


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        }


        private void ShowRssData(string feed)
        {

            foreach (Rss p in ListRss)
            {
                comboBox1.Items.Add(p.Title);
                
                foreach (RssItem Item in p.ListItem)
                {
                    checkedListBox2.Items.Add(Item.Title);
                }

                
            }

        }
    }
}
