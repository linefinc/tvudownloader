namespace tvu
{
    partial class OptionsDialog
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
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.DafeultCategoryTextBox = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxEmuleExec = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.checkBoxCloseEmuleIfAllIsDone = new System.Windows.Forms.CheckBox();
            this.textBoxEmuleExe = new System.Windows.Forms.TextBox();
            this.textBoxDefaultCategory = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxStartEmuleIfClose = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxStartWithWindows = new System.Windows.Forms.CheckBox();
            this.checkBoxStartMinimized = new System.Windows.Forms.CheckBox();
            this.numericUpDownIntervalTime = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPageNetwork = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonCheckNow = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxServiceUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.checkBoxAutoClear = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabPageGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntervalTime)).BeginInit();
            this.tabPageNetwork.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(202, 292);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(283, 292);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // DafeultCategoryTextBox
            // 
            this.DafeultCategoryTextBox.Location = new System.Drawing.Point(126, 157);
            this.DafeultCategoryTextBox.Name = "DafeultCategoryTextBox";
            this.DafeultCategoryTextBox.Size = new System.Drawing.Size(399, 20);
            this.DafeultCategoryTextBox.TabIndex = 36;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 160);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(86, 13);
            this.label17.TabIndex = 35;
            this.label17.Text = "Default Category";
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
            // textBoxEmuleExec
            // 
            this.textBoxEmuleExec.Location = new System.Drawing.Point(90, 115);
            this.textBoxEmuleExec.Name = "textBoxEmuleExec";
            this.textBoxEmuleExec.Size = new System.Drawing.Size(435, 20);
            this.textBoxEmuleExec.TabIndex = 26;
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
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.checkBoxAutoClear);
            this.tabPageGeneral.Controls.Add(this.checkBoxCloseEmuleIfAllIsDone);
            this.tabPageGeneral.Controls.Add(this.textBoxEmuleExe);
            this.tabPageGeneral.Controls.Add(this.textBoxDefaultCategory);
            this.tabPageGeneral.Controls.Add(this.label5);
            this.tabPageGeneral.Controls.Add(this.checkBoxStartEmuleIfClose);
            this.tabPageGeneral.Controls.Add(this.label4);
            this.tabPageGeneral.Controls.Add(this.checkBoxStartWithWindows);
            this.tabPageGeneral.Controls.Add(this.checkBoxStartMinimized);
            this.tabPageGeneral.Controls.Add(this.numericUpDownIntervalTime);
            this.tabPageGeneral.Controls.Add(this.label7);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(357, 248);
            this.tabPageGeneral.TabIndex = 1;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // checkBoxCloseEmuleIfAllIsDone
            // 
            this.checkBoxCloseEmuleIfAllIsDone.AutoSize = true;
            this.checkBoxCloseEmuleIfAllIsDone.Location = new System.Drawing.Point(6, 151);
            this.checkBoxCloseEmuleIfAllIsDone.Name = "checkBoxCloseEmuleIfAllIsDone";
            this.checkBoxCloseEmuleIfAllIsDone.Size = new System.Drawing.Size(142, 17);
            this.checkBoxCloseEmuleIfAllIsDone.TabIndex = 50;
            this.checkBoxCloseEmuleIfAllIsDone.Text = "Close eMule if all is done";
            this.checkBoxCloseEmuleIfAllIsDone.UseVisualStyleBackColor = true;
            // 
            // textBoxEmuleExe
            // 
            this.textBoxEmuleExe.Location = new System.Drawing.Point(98, 124);
            this.textBoxEmuleExe.Name = "textBoxEmuleExe";
            this.textBoxEmuleExe.Size = new System.Drawing.Size(244, 20);
            this.textBoxEmuleExe.TabIndex = 47;
            // 
            // textBoxDefaultCategory
            // 
            this.textBoxDefaultCategory.Location = new System.Drawing.Point(98, 75);
            this.textBoxDefaultCategory.Name = "textBoxDefaultCategory";
            this.textBoxDefaultCategory.Size = new System.Drawing.Size(244, 20);
            this.textBoxDefaultCategory.TabIndex = 46;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 48;
            this.label5.Text = "eMule Exeguile";
            // 
            // checkBoxStartEmuleIfClose
            // 
            this.checkBoxStartEmuleIfClose.AutoSize = true;
            this.checkBoxStartEmuleIfClose.Location = new System.Drawing.Point(6, 101);
            this.checkBoxStartEmuleIfClose.Name = "checkBoxStartEmuleIfClose";
            this.checkBoxStartEmuleIfClose.Size = new System.Drawing.Size(116, 17);
            this.checkBoxStartEmuleIfClose.TabIndex = 49;
            this.checkBoxStartEmuleIfClose.Text = "Start Emule if close";
            this.checkBoxStartEmuleIfClose.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 45;
            this.label4.Text = "Default Category";
            // 
            // checkBoxStartWithWindows
            // 
            this.checkBoxStartWithWindows.AutoSize = true;
            this.checkBoxStartWithWindows.Location = new System.Drawing.Point(6, 29);
            this.checkBoxStartWithWindows.Name = "checkBoxStartWithWindows";
            this.checkBoxStartWithWindows.Size = new System.Drawing.Size(117, 17);
            this.checkBoxStartWithWindows.TabIndex = 44;
            this.checkBoxStartWithWindows.Text = "Start with Windows";
            this.checkBoxStartWithWindows.UseVisualStyleBackColor = true;
            // 
            // checkBoxStartMinimized
            // 
            this.checkBoxStartMinimized.AutoSize = true;
            this.checkBoxStartMinimized.Location = new System.Drawing.Point(6, 6);
            this.checkBoxStartMinimized.Name = "checkBoxStartMinimized";
            this.checkBoxStartMinimized.Size = new System.Drawing.Size(97, 17);
            this.checkBoxStartMinimized.TabIndex = 43;
            this.checkBoxStartMinimized.Text = "Start Minimized";
            this.checkBoxStartMinimized.UseVisualStyleBackColor = true;
            // 
            // numericUpDownIntervalTime
            // 
            this.numericUpDownIntervalTime.Location = new System.Drawing.Point(105, 47);
            this.numericUpDownIntervalTime.Maximum = new decimal(new int[] {
            720,
            0,
            0,
            0});
            this.numericUpDownIntervalTime.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownIntervalTime.Name = "numericUpDownIntervalTime";
            this.numericUpDownIntervalTime.Size = new System.Drawing.Size(47, 20);
            this.numericUpDownIntervalTime.TabIndex = 37;
            this.numericUpDownIntervalTime.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "Interval Time [min]";
            // 
            // tabPageNetwork
            // 
            this.tabPageNetwork.Controls.Add(this.groupBox1);
            this.tabPageNetwork.Location = new System.Drawing.Point(4, 22);
            this.tabPageNetwork.Name = "tabPageNetwork";
            this.tabPageNetwork.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNetwork.Size = new System.Drawing.Size(357, 248);
            this.tabPageNetwork.TabIndex = 0;
            this.tabPageNetwork.Text = "Network";
            this.tabPageNetwork.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonCheckNow);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.textBoxPassword);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.textBoxServiceUrl);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 89);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Network";
            // 
            // buttonCheckNow
            // 
            this.buttonCheckNow.Location = new System.Drawing.Point(261, 60);
            this.buttonCheckNow.Name = "buttonCheckNow";
            this.buttonCheckNow.Size = new System.Drawing.Size(75, 23);
            this.buttonCheckNow.TabIndex = 34;
            this.buttonCheckNow.Text = "Check Now";
            this.buttonCheckNow.UseVisualStyleBackColor = true;
            this.buttonCheckNow.Click += new System.EventHandler(this.buttonCheckNow_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Controls.Add(this.checkBox2);
            this.groupBox3.Controls.Add(this.checkBox3);
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.checkBox4);
            this.groupBox3.Controls.Add(this.numericUpDown2);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Location = new System.Drawing.Point(0, 110);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(531, 256);
            this.groupBox3.TabIndex = 33;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "General";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(126, 157);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(399, 20);
            this.textBox1.TabIndex = 36;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 160);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(86, 13);
            this.label18.TabIndex = 35;
            this.label18.Text = "Default Category";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 42);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(117, 17);
            this.checkBox1.TabIndex = 34;
            this.checkBox1.Text = "Start with Windows";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(9, 19);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(97, 17);
            this.checkBox2.TabIndex = 31;
            this.checkBox2.Text = "Start Minimized";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(9, 65);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(142, 17);
            this.checkBox3.TabIndex = 29;
            this.checkBox3.Text = "Close eMule if all is done";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(90, 115);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(435, 20);
            this.textBox2.TabIndex = 26;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 118);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(79, 13);
            this.label19.TabIndex = 27;
            this.label19.Text = "eMule Exeguile";
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(9, 88);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(116, 17);
            this.checkBox4.TabIndex = 28;
            this.checkBox4.Text = "Start Emule if close";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(405, 17);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            720,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(86, 20);
            this.numericUpDown2.TabIndex = 22;
            this.numericUpDown2.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(303, 19);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(93, 13);
            this.label20.TabIndex = 25;
            this.label20.Text = "Interval Time [min]";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(108, 35);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(228, 20);
            this.textBoxPassword.TabIndex = 21;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(431, 410);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 36);
            this.button3.TabIndex = 14;
            this.button3.Text = "Save config";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // textBoxServiceUrl
            // 
            this.textBoxServiceUrl.Location = new System.Drawing.Point(108, 9);
            this.textBoxServiceUrl.Name = "textBoxServiceUrl";
            this.textBoxServiceUrl.Size = new System.Drawing.Size(228, 20);
            this.textBoxServiceUrl.TabIndex = 20;
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
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageNetwork);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(365, 274);
            this.tabControl1.TabIndex = 0;
            // 
            // checkBoxAutoClear
            // 
            this.checkBoxAutoClear.AutoSize = true;
            this.checkBoxAutoClear.Location = new System.Drawing.Point(6, 174);
            this.checkBoxAutoClear.Name = "checkBoxAutoClear";
            this.checkBoxAutoClear.Size = new System.Drawing.Size(96, 17);
            this.checkBoxAutoClear.TabIndex = 51;
            this.checkBoxAutoClear.Text = "Auto Clear Log";
            this.checkBoxAutoClear.UseVisualStyleBackColor = true;
            // 
            // OptionsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 320);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "OptionsDialog";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntervalTime)).EndInit();
            this.tabPageNetwork.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox DafeultCategoryTextBox;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxEmuleExec;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.CheckBox checkBoxCloseEmuleIfAllIsDone;
        private System.Windows.Forms.TextBox textBoxEmuleExe;
        private System.Windows.Forms.TextBox textBoxDefaultCategory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxStartEmuleIfClose;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxStartWithWindows;
        private System.Windows.Forms.CheckBox checkBoxStartMinimized;
        private System.Windows.Forms.NumericUpDown numericUpDownIntervalTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabPageNetwork;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonCheckNow;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBoxServiceUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.CheckBox checkBoxAutoClear;
    }
}