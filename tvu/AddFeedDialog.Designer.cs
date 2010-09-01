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
            this.SuspendLayout();
            // 
            // butUpdateCategory
            // 
            this.butUpdateCategory.Location = new System.Drawing.Point(12, 176);
            this.butUpdateCategory.Name = "butUpdateCategory";
            this.butUpdateCategory.Size = new System.Drawing.Size(100, 41);
            this.butUpdateCategory.TabIndex = 0;
            this.butUpdateCategory.Text = "Update Category";
            this.butUpdateCategory.UseVisualStyleBackColor = true;
            this.butUpdateCategory.Click += new System.EventHandler(this.butUpdateCategory_Click);
            // 
            // butAdd
            // 
            this.butAdd.Location = new System.Drawing.Point(118, 176);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(100, 41);
            this.butAdd.TabIndex = 1;
            this.butAdd.Text = "Add";
            this.butAdd.UseVisualStyleBackColor = true;
            this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // ButClose
            // 
            this.ButClose.Location = new System.Drawing.Point(224, 176);
            this.ButClose.Name = "ButClose";
            this.ButClose.Size = new System.Drawing.Size(100, 41);
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
            this.comboBoxCategory.Location = new System.Drawing.Point(12, 126);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(312, 21);
            this.comboBoxCategory.TabIndex = 33;
            // 
            // checkBoxPause
            // 
            this.checkBoxPause.AutoSize = true;
            this.checkBoxPause.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxPause.Location = new System.Drawing.Point(12, 153);
            this.checkBoxPause.Name = "checkBoxPause";
            this.checkBoxPause.Size = new System.Drawing.Size(118, 17);
            this.checkBoxPause.TabIndex = 32;
            this.checkBoxPause.Text = "Download in Pause";
            this.checkBoxPause.UseVisualStyleBackColor = true;
            // 
            // textUrl
            // 
            this.textUrl.Location = new System.Drawing.Point(12, 12);
            this.textUrl.Multiline = true;
            this.textUrl.Name = "textUrl";
            this.textUrl.Size = new System.Drawing.Size(312, 108);
            this.textUrl.TabIndex = 30;
            // 
            // AddFeedDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 231);
            this.ControlBox = false;
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
            this.Text = "AddFeedDialog";
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
    }
}