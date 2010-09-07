namespace tvu
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button2 = new System.Windows.Forms.Button();
            this.ButtonSaveConfig = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Log = new System.Windows.Forms.TabPage();
            this.ClearButton = new System.Windows.Forms.Button();
            this.CheckButton = new System.Windows.Forms.Button();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.Main = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.button4 = new System.Windows.Forms.Button();
            this.Config = new System.Windows.Forms.TabPage();
            this.checkBoxStartWithWindows = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxStartMinimized = new System.Windows.Forms.CheckBox();
            this.checkBoxCloseWhenAllDone = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxAutoStartEmule = new System.Windows.Forms.CheckBox();
            this.textBoxEmuleExec = new System.Windows.Forms.TextBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.ServiceUrlTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.Log.SuspendLayout();
            this.Main.SuspendLayout();
            this.Config.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(431, 61);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 36);
            this.button2.TabIndex = 4;
            this.button2.Text = "Verify config";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ButtonSaveConfig
            // 
            this.ButtonSaveConfig.Location = new System.Drawing.Point(436, 413);
            this.ButtonSaveConfig.Name = "ButtonSaveConfig";
            this.ButtonSaveConfig.Size = new System.Drawing.Size(100, 36);
            this.ButtonSaveConfig.TabIndex = 14;
            this.ButtonSaveConfig.Text = "Save config";
            this.ButtonSaveConfig.UseVisualStyleBackColor = true;
            this.ButtonSaveConfig.Click += new System.EventHandler(this.ButtonSaveConfig_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Log);
            this.tabControl1.Controls.Add(this.Main);
            this.tabControl1.Controls.Add(this.Config);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(547, 478);
            this.tabControl1.TabIndex = 21;
            // 
            // Log
            // 
            this.Log.Controls.Add(this.ClearButton);
            this.Log.Controls.Add(this.CheckButton);
            this.Log.Controls.Add(this.LogTextBox);
            this.Log.Location = new System.Drawing.Point(4, 22);
            this.Log.Name = "Log";
            this.Log.Padding = new System.Windows.Forms.Padding(3);
            this.Log.Size = new System.Drawing.Size(539, 452);
            this.Log.TabIndex = 1;
            this.Log.Text = "Log";
            this.Log.UseVisualStyleBackColor = true;
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(112, 410);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(100, 36);
            this.ClearButton.TabIndex = 18;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // CheckButton
            // 
            this.CheckButton.Location = new System.Drawing.Point(6, 410);
            this.CheckButton.Name = "CheckButton";
            this.CheckButton.Size = new System.Drawing.Size(100, 36);
            this.CheckButton.TabIndex = 17;
            this.CheckButton.Text = "Check Now";
            this.CheckButton.UseVisualStyleBackColor = true;
            this.CheckButton.Click += new System.EventHandler(this.CheckButton_Click);
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(6, 6);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.Size = new System.Drawing.Size(527, 398);
            this.LogTextBox.TabIndex = 3;
            // 
            // Main
            // 
            this.Main.Controls.Add(this.button1);
            this.Main.Controls.Add(this.listView2);
            this.Main.Controls.Add(this.listView1);
            this.Main.Controls.Add(this.button4);
            this.Main.Location = new System.Drawing.Point(4, 22);
            this.Main.Name = "Main";
            this.Main.Padding = new System.Windows.Forms.Padding(3);
            this.Main.Size = new System.Drawing.Size(539, 452);
            this.Main.TabIndex = 0;
            this.Main.Text = "Feed";
            this.Main.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 248);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 37);
            this.button1.TabIndex = 25;
            this.button1.Text = "Add ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5});
            this.listView2.Location = new System.Drawing.Point(6, 291);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(527, 155);
            this.listView2.TabIndex = 24;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "File";
            this.columnHeader5.Width = 500;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.Location = new System.Drawing.Point(6, 6);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(527, 236);
            this.listView1.TabIndex = 23;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 180;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Category";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Pause";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "RSS link";
            this.columnHeader4.Width = 300;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(433, 248);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 36);
            this.button4.TabIndex = 19;
            this.button4.Text = "Delete feed";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // Config
            // 
            this.Config.Controls.Add(this.groupBox2);
            this.Config.Controls.Add(this.groupBox1);
            this.Config.Controls.Add(this.ButtonSaveConfig);
            this.Config.Location = new System.Drawing.Point(4, 22);
            this.Config.Name = "Config";
            this.Config.Size = new System.Drawing.Size(539, 452);
            this.Config.TabIndex = 2;
            this.Config.Text = "Config";
            this.Config.UseVisualStyleBackColor = true;
            // 
            // checkBoxStartWithWindows
            // 
            this.checkBoxStartWithWindows.AutoSize = true;
            this.checkBoxStartWithWindows.Location = new System.Drawing.Point(9, 42);
            this.checkBoxStartWithWindows.Name = "checkBoxStartWithWindows";
            this.checkBoxStartWithWindows.Size = new System.Drawing.Size(117, 17);
            this.checkBoxStartWithWindows.TabIndex = 34;
            this.checkBoxStartWithWindows.Text = "Start with Windows";
            this.checkBoxStartWithWindows.UseVisualStyleBackColor = true;
            this.checkBoxStartWithWindows.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxStartWithWindows);
            this.groupBox2.Controls.Add(this.checkBoxStartMinimized);
            this.groupBox2.Controls.Add(this.checkBoxCloseWhenAllDone);
            this.groupBox2.Controls.Add(this.textBoxEmuleExec);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.checkBoxAutoStartEmule);
            this.groupBox2.Controls.Add(this.numericUpDown1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(5, 113);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(531, 148);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "General";
            // 
            // checkBoxStartMinimized
            // 
            this.checkBoxStartMinimized.AutoSize = true;
            this.checkBoxStartMinimized.Location = new System.Drawing.Point(9, 19);
            this.checkBoxStartMinimized.Name = "checkBoxStartMinimized";
            this.checkBoxStartMinimized.Size = new System.Drawing.Size(97, 17);
            this.checkBoxStartMinimized.TabIndex = 31;
            this.checkBoxStartMinimized.Text = "Start Minimized";
            this.checkBoxStartMinimized.UseVisualStyleBackColor = true;
            // 
            // checkBoxCloseWhenAllDone
            // 
            this.checkBoxCloseWhenAllDone.AutoSize = true;
            this.checkBoxCloseWhenAllDone.Location = new System.Drawing.Point(9, 65);
            this.checkBoxCloseWhenAllDone.Name = "checkBoxCloseWhenAllDone";
            this.checkBoxCloseWhenAllDone.Size = new System.Drawing.Size(142, 17);
            this.checkBoxCloseWhenAllDone.TabIndex = 29;
            this.checkBoxCloseWhenAllDone.Text = "Close eMule if all is done";
            this.checkBoxCloseWhenAllDone.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 118);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "eMule Exeguile";
            // 
            // checkBoxAutoStartEmule
            // 
            this.checkBoxAutoStartEmule.AutoSize = true;
            this.checkBoxAutoStartEmule.Location = new System.Drawing.Point(9, 88);
            this.checkBoxAutoStartEmule.Name = "checkBoxAutoStartEmule";
            this.checkBoxAutoStartEmule.Size = new System.Drawing.Size(116, 17);
            this.checkBoxAutoStartEmule.TabIndex = 28;
            this.checkBoxAutoStartEmule.Text = "Start Emule if close";
            this.checkBoxAutoStartEmule.UseVisualStyleBackColor = true;
            // 
            // textBoxEmuleExec
            // 
            this.textBoxEmuleExec.Location = new System.Drawing.Point(90, 115);
            this.textBoxEmuleExec.Name = "textBoxEmuleExec";
            this.textBoxEmuleExec.Size = new System.Drawing.Size(435, 20);
            this.textBoxEmuleExec.TabIndex = 26;
            this.textBoxEmuleExec.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(405, 17);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            720,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(86, 20);
            this.numericUpDown1.TabIndex = 22;
            this.numericUpDown1.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(303, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Interval Time [min]";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PasswordTextBox);
            this.groupBox1.Controls.Add(this.ServiceUrlTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Location = new System.Drawing.Point(5, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(531, 104);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Network";
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(108, 35);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(417, 20);
            this.PasswordTextBox.TabIndex = 21;
            this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
            // 
            // ServiceUrlTextBox
            // 
            this.ServiceUrlTextBox.Location = new System.Drawing.Point(108, 9);
            this.ServiceUrlTextBox.Name = "ServiceUrlTextBox";
            this.ServiceUrlTextBox.Size = new System.Drawing.Size(417, 20);
            this.ServiceUrlTextBox.TabIndex = 20;
            this.ServiceUrlTextBox.TextChanged += new System.EventHandler(this.ServiceUrlTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Emule Service Url";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Password";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.linkLabel1);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(539, 452);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "About";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(295, 307);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(307, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Version 0.03";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(307, 56);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(137, 13);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://linefinc.blogspot.com";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(307, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "TvUnderground Downloader";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 502);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "TvUnderground Downloader";
            this.tabControl1.ResumeLayout(false);
            this.Log.ResumeLayout(false);
            this.Log.PerformLayout();
            this.Main.ResumeLayout(false);
            this.Config.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

       

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button ButtonSaveConfig;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Main;
        private System.Windows.Forms.TabPage Log;
        private System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.TabPage Config;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.TextBox ServiceUrlTextBox;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button CheckButton;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.CheckBox checkBoxAutoStartEmule;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxEmuleExec;
        private System.Windows.Forms.CheckBox checkBoxCloseWhenAllDone;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBoxStartMinimized;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox checkBoxStartWithWindows;
    }
}

