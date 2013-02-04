namespace tvu
{
    partial class AddFeedDialogPage3
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
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            this.checkBoxPause = new System.Windows.Forms.CheckBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonSelectNone = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonFinish = new System.Windows.Forms.Button();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.labelSelectedElement = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(731, 434);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
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
            this.comboBoxCategory.Location = new System.Drawing.Point(569, 405);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(231, 21);
            this.comboBoxCategory.TabIndex = 33;
            this.comboBoxCategory.SelectedIndexChanged += new System.EventHandler(this.comboBoxCategory_SelectedIndexChanged);
            // 
            // checkBoxPause
            // 
            this.checkBoxPause.AutoSize = true;
            this.checkBoxPause.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxPause.Location = new System.Drawing.Point(253, 407);
            this.checkBoxPause.Name = "checkBoxPause";
            this.checkBoxPause.Size = new System.Drawing.Size(118, 17);
            this.checkBoxPause.TabIndex = 32;
            this.checkBoxPause.Text = "Download in Pause";
            this.checkBoxPause.UseVisualStyleBackColor = true;
            this.checkBoxPause.CheckedChanged += new System.EventHandler(this.checkBoxPause_CheckedChanged);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(12, 33);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.ScrollAlwaysVisible = true;
            this.checkedListBox1.Size = new System.Drawing.Size(788, 349);
            this.checkedListBox1.TabIndex = 38;
            this.checkedListBox1.SelectedValueChanged += new System.EventHandler(this.checkedListBox1_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Episode to download";
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new System.Drawing.Point(377, 403);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(77, 23);
            this.buttonSelectAll.TabIndex = 42;
            this.buttonSelectAll.Text = "Download All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // buttonSelectNone
            // 
            this.buttonSelectNone.Location = new System.Drawing.Point(460, 403);
            this.buttonSelectNone.Name = "buttonSelectNone";
            this.buttonSelectNone.Size = new System.Drawing.Size(94, 23);
            this.buttonSelectNone.TabIndex = 43;
            this.buttonSelectNone.Text = "Download None";
            this.buttonSelectNone.UseVisualStyleBackColor = true;
            this.buttonSelectNone.Click += new System.EventHandler(this.buttonSelectNone_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(569, 434);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 44;
            this.buttonNext.Text = "->";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonFinish
            // 
            this.buttonFinish.Location = new System.Drawing.Point(650, 434);
            this.buttonFinish.Name = "buttonFinish";
            this.buttonFinish.Size = new System.Drawing.Size(75, 23);
            this.buttonFinish.TabIndex = 45;
            this.buttonFinish.Text = "Finish";
            this.buttonFinish.UseVisualStyleBackColor = true;
            this.buttonFinish.Click += new System.EventHandler(this.buttonFinish_Click);
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Location = new System.Drawing.Point(488, 434);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(75, 23);
            this.buttonPrevious.TabIndex = 46;
            this.buttonPrevious.Text = "<-";
            this.buttonPrevious.UseVisualStyleBackColor = true;
            this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
            // 
            // labelSelectedElement
            // 
            this.labelSelectedElement.AutoSize = true;
            this.labelSelectedElement.Location = new System.Drawing.Point(12, 408);
            this.labelSelectedElement.Name = "labelSelectedElement";
            this.labelSelectedElement.Size = new System.Drawing.Size(98, 13);
            this.labelSelectedElement.TabIndex = 47;
            this.labelSelectedElement.Text = "Selected element 0";
            // 
            // AddFeedDialogPage3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 469);
            this.ControlBox = false;
            this.Controls.Add(this.labelSelectedElement);
            this.Controls.Add(this.buttonPrevious);
            this.Controls.Add(this.buttonFinish);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonSelectNone);
            this.Controls.Add(this.buttonSelectAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxCategory);
            this.Controls.Add(this.checkBoxPause);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddFeedDialogPage3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Feed";
            this.Load += new System.EventHandler(this.AddFeedDialogPage3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxCategory;
        private System.Windows.Forms.CheckBox checkBoxPause;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonSelectNone;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonFinish;
        private System.Windows.Forms.Button buttonPrevious;
        private System.Windows.Forms.Label labelSelectedElement;
    }
}
