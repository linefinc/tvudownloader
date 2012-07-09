namespace tvu
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
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(295, 54);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(214, 54);
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
            this.checkBoxDownloadinPause.Location = new System.Drawing.Point(12, 52);
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
            // EditFeedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 89);
            this.Controls.Add(this.labelRssTitle);
            this.Controls.Add(this.comboBoxCategory);
            this.Controls.Add(this.checkBoxDownloadinPause);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditFeedForm";
            this.Text = "Edit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.CheckBox checkBoxDownloadinPause;
        private System.Windows.Forms.ComboBox comboBoxCategory;
        private System.Windows.Forms.Label labelRssTitle;
    }
}