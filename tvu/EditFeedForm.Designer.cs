namespace TvUndergroundDownloader
{
    partial class EditFeedForm
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.checkBoxDownloadinPause = new System.Windows.Forms.CheckBox();
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            this.labelRssTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.checkBoxEnable = new System.Windows.Forms.CheckBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(295, 139);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(214, 139);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // checkBoxDownloadinPause
            // 
            this.checkBoxDownloadinPause.AutoSize = true;
            this.checkBoxDownloadinPause.Location = new System.Drawing.Point(12, 79);
            this.checkBoxDownloadinPause.Name = "checkBoxDownloadinPause";
            this.checkBoxDownloadinPause.Size = new System.Drawing.Size(118, 17);
            this.checkBoxDownloadinPause.TabIndex = 2;
            this.checkBoxDownloadinPause.Text = "Download in Pause";
            this.checkBoxDownloadinPause.UseVisualStyleBackColor = true;
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.Location = new System.Drawing.Point(12, 25);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(358, 21);
            this.comboBoxCategory.TabIndex = 3;
            // 
            // labelRssTitle
            // 
            this.labelRssTitle.AutoSize = true;
            this.labelRssTitle.Location = new System.Drawing.Point(12, 9);
            this.labelRssTitle.Name = "labelRssTitle";
            this.labelRssTitle.Size = new System.Drawing.Size(54, 13);
            this.labelRssTitle.TabIndex = 4;
            this.labelRssTitle.Text = "Rss Title: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Max Simultaneous Downloads";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(167, 49);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown1.TabIndex = 7;
            this.numericUpDown1.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // checkBoxEnable
            // 
            this.checkBoxEnable.AutoSize = true;
            this.checkBoxEnable.Location = new System.Drawing.Point(12, 102);
            this.checkBoxEnable.Name = "checkBoxEnable";
            this.checkBoxEnable.Size = new System.Drawing.Size(59, 17);
            this.checkBoxEnable.TabIndex = 8;
            this.checkBoxEnable.Text = "Enable";
            this.checkBoxEnable.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // EditFeedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 174);
            this.Controls.Add(this.checkBoxEnable);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelRssTitle);
            this.Controls.Add(this.comboBoxCategory);
            this.Controls.Add(this.checkBoxDownloadinPause);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditFeedForm";
            this.Text = "Edit";
            this.Load += new System.EventHandler(this.EditFeedForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.CheckBox checkBoxDownloadinPause;
        private System.Windows.Forms.ComboBox comboBoxCategory;
        private System.Windows.Forms.Label labelRssTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.CheckBox checkBoxEnable;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}