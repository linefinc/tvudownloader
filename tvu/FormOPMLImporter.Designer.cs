namespace tvu
{
    partial class FormOPMLImporter
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.buttonCancell = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.checkedListBox2 = new System.Windows.Forms.CheckedListBox();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 303);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(385, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 41);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(385, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // buttonCancell
            // 
            this.buttonCancell.Location = new System.Drawing.Point(322, 332);
            this.buttonCancell.Name = "buttonCancell";
            this.buttonCancell.Size = new System.Drawing.Size(75, 23);
            this.buttonCancell.TabIndex = 5;
            this.buttonCancell.Text = "Cancell";
            this.buttonCancell.UseVisualStyleBackColor = true;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(241, 332);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 6;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            // 
            // checkedListBox2
            // 
            this.checkedListBox2.FormattingEnabled = true;
            this.checkedListBox2.Location = new System.Drawing.Point(12, 68);
            this.checkedListBox2.Name = "checkedListBox2";
            this.checkedListBox2.Size = new System.Drawing.Size(385, 229);
            this.checkedListBox2.TabIndex = 7;
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(322, 12);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonOpen.TabIndex = 8;
            this.buttonOpen.Text = "Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // FormOPMLImporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 364);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.checkedListBox2);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonCancell);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.progressBar1);
            this.Name = "FormOPMLImporter";
            this.Text = "FormOPMLImporter";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button buttonCancell;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.CheckedListBox checkedListBox2;
        private System.Windows.Forms.Button buttonOpen;
    }
}