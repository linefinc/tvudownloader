namespace tvu
{
    partial class AddFeedDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.butUpdateCategory = new System.Windows.Forms.Button();
            this.butAdd = new System.Windows.Forms.Button();
            this.ButClose = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            this.checkBoxPause = new System.Windows.Forms.CheckBox();
            this.textUrl = new System.Windows.Forms.TextBox();
            this.buttonGetFeed = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // butUpdateCategory
            // 
            this.butUpdateCategory.Location = new System.Drawing.Point(725, 388);
            this.butUpdateCategory.Name = "butUpdateCategory";
            this.butUpdateCategory.Size = new System.Drawing.Size(75, 23);
            this.butUpdateCategory.TabIndex = 0;
            this.butUpdateCategory.Text = "Update Category";
            this.butUpdateCategory.UseVisualStyleBackColor = true;
            this.butUpdateCategory.Click += new System.EventHandler(this.butUpdateCategory_Click);
            // 
            // butAdd
            // 
            this.butAdd.Enabled = false;
            this.butAdd.Location = new System.Drawing.Point(650, 434);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(75, 23);
            this.butAdd.TabIndex = 1;
            this.butAdd.Text = "Add";
            this.butAdd.UseVisualStyleBackColor = true;
            this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // ButClose
            // 
            this.ButClose.Location = new System.Drawing.Point(731, 434);
            this.ButClose.Name = "ButClose";
            this.ButClose.Size = new System.Drawing.Size(75, 23);
            this.ButClose.TabIndex = 2;
            this.ButClose.Text = "Close";
            this.ButClose.UseVisualStyleBackColor = true;
            this.ButClose.Click += new System.EventHandler(this.ButClose_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-96, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "Feed Link";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-96, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "Category";
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.Location = new System.Drawing.Point(488, 390);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(231, 21);
            this.comboBoxCategory.TabIndex = 33;
            // 
            // checkBoxPause
            // 
            this.checkBoxPause.AutoSize = true;
            this.checkBoxPause.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxPause.Location = new System.Drawing.Point(12, 394);
            this.checkBoxPause.Name = "checkBoxPause";
            this.checkBoxPause.Size = new System.Drawing.Size(118, 17);
            this.checkBoxPause.TabIndex = 32;
            this.checkBoxPause.Text = "Download in Pause";
            this.checkBoxPause.UseVisualStyleBackColor = true;
            // 
            // textUrl
            // 
            this.textUrl.Location = new System.Drawing.Point(12, 27);
            this.textUrl.Multiline = true;
            this.textUrl.Name = "textUrl";
            this.textUrl.Size = new System.Drawing.Size(788, 29);
            this.textUrl.TabIndex = 30;
            this.textUrl.TextChanged += new System.EventHandler(this.textUrl_TextChanged);
            // 
            // buttonGetFeed
            // 
            this.buttonGetFeed.Location = new System.Drawing.Point(569, 434);
            this.buttonGetFeed.Name = "buttonGetFeed";
            this.buttonGetFeed.Size = new System.Drawing.Size(75, 23);
            this.buttonGetFeed.TabIndex = 37;
            this.buttonGetFeed.Text = "Get Feed";
            this.buttonGetFeed.UseVisualStyleBackColor = true;
            this.buttonGetFeed.Click += new System.EventHandler(this.buttonGetFeed_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(12, 78);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.ScrollAlwaysVisible = true;
            this.checkedListBox1.Size = new System.Drawing.Size(788, 304);
            this.checkedListBox1.TabIndex = 38;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 434);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(551, 23);
            this.progressBar1.TabIndex = 39;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Episode to download";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 41;
            this.label2.Text = "RSS link";
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new System.Drawing.Point(305, 388);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(77, 23);
            this.buttonSelectAll.TabIndex = 42;
            this.buttonSelectAll.Text = "Download All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(388, 388);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 23);
            this.button2.TabIndex = 43;
            this.button2.Text = "Download None";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // AddFeedDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 469);
            this.ControlBox = false;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonSelectAll);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.buttonGetFeed);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxCategory);
            this.Controls.Add(this.checkBoxPause);
            this.Controls.Add(this.textUrl);
            this.Controls.Add(this.ButClose);
            this.Controls.Add(this.butAdd);
            this.Controls.Add(this.butUpdateCategory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddFeedDialog";
            this.Text = "Add Feed";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butUpdateCategory;
        private System.Windows.Forms.Button butAdd;
        private System.Windows.Forms.Button ButClose;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxCategory;
        private System.Windows.Forms.CheckBox checkBoxPause;
        private System.Windows.Forms.TextBox textUrl;
        private System.Windows.Forms.Button buttonGetFeed;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button button2;
    }
}