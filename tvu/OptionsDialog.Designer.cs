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
            this.label13 = new System.Windows.Forms.Label();
            this.numericUpDownMaxSimultaneousDownloadForFeed = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDownMinDownloadToStrarTEmule = new System.Windows.Forms.NumericUpDown();
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
            this.label14 = new System.Windows.Forms.Label();
            this.buttonCheckNow = new System.Windows.Forms.Button();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxServiceUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageEmail = new System.Windows.Forms.TabPage();
            this.buttonTestEmailNotification = new System.Windows.Forms.Button();
            this.textBoxMailSender = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxMailReceiver = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxServerSmtp = new System.Windows.Forms.TextBox();
            this.checkBoxEmailNotification = new System.Windows.Forms.CheckBox();
            this.tabPageUpdate = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.numericUpDownIntervalCheck = new System.Windows.Forms.NumericUpDown();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.checkBoxSaveLogToFile = new System.Windows.Forms.CheckBox();
            this.checkBoxVerbose = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoClear = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabPageGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxSimultaneousDownloadForFeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinDownloadToStrarTEmule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntervalTime)).BeginInit();
            this.tabPageNetwork.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageEmail.SuspendLayout();
            this.tabPageUpdate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntervalCheck)).BeginInit();
            this.tabPageLog.SuspendLayout();
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
            this.tabPageGeneral.Controls.Add(this.label13);
            this.tabPageGeneral.Controls.Add(this.numericUpDownMaxSimultaneousDownloadForFeed);
            this.tabPageGeneral.Controls.Add(this.label8);
            this.tabPageGeneral.Controls.Add(this.numericUpDownMinDownloadToStrarTEmule);
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
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 183);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(184, 13);
            this.label13.TabIndex = 55;
            this.label13.Text = "Max simultaneous Download for Feed";
            // 
            // numericUpDownMaxSimultaneousDownloadForFeed
            // 
            this.numericUpDownMaxSimultaneousDownloadForFeed.Location = new System.Drawing.Point(295, 181);
            this.numericUpDownMaxSimultaneousDownloadForFeed.Name = "numericUpDownMaxSimultaneousDownloadForFeed";
            this.numericUpDownMaxSimultaneousDownloadForFeed.Size = new System.Drawing.Size(47, 20);
            this.numericUpDownMaxSimultaneousDownloadForFeed.TabIndex = 54;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 157);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(144, 13);
            this.label8.TabIndex = 53;
            this.label8.Text = "Min Donwload to Start eMule";
            // 
            // numericUpDownMinDownloadToStrarTEmule
            // 
            this.numericUpDownMinDownloadToStrarTEmule.Location = new System.Drawing.Point(295, 155);
            this.numericUpDownMinDownloadToStrarTEmule.Name = "numericUpDownMinDownloadToStrarTEmule";
            this.numericUpDownMinDownloadToStrarTEmule.Size = new System.Drawing.Size(47, 20);
            this.numericUpDownMinDownloadToStrarTEmule.TabIndex = 52;
            // 
            // checkBoxCloseEmuleIfAllIsDone
            // 
            this.checkBoxCloseEmuleIfAllIsDone.AutoSize = true;
            this.checkBoxCloseEmuleIfAllIsDone.Location = new System.Drawing.Point(6, 65);
            this.checkBoxCloseEmuleIfAllIsDone.Name = "checkBoxCloseEmuleIfAllIsDone";
            this.checkBoxCloseEmuleIfAllIsDone.Size = new System.Drawing.Size(142, 17);
            this.checkBoxCloseEmuleIfAllIsDone.TabIndex = 50;
            this.checkBoxCloseEmuleIfAllIsDone.Text = "Close eMule if all is done";
            this.checkBoxCloseEmuleIfAllIsDone.UseVisualStyleBackColor = true;
            // 
            // textBoxEmuleExe
            // 
            this.textBoxEmuleExe.Location = new System.Drawing.Point(98, 134);
            this.textBoxEmuleExe.Name = "textBoxEmuleExe";
            this.textBoxEmuleExe.Size = new System.Drawing.Size(244, 20);
            this.textBoxEmuleExe.TabIndex = 47;
            // 
            // textBoxDefaultCategory
            // 
            this.textBoxDefaultCategory.Location = new System.Drawing.Point(98, 88);
            this.textBoxDefaultCategory.Name = "textBoxDefaultCategory";
            this.textBoxDefaultCategory.Size = new System.Drawing.Size(244, 20);
            this.textBoxDefaultCategory.TabIndex = 46;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 48;
            this.label5.Text = "eMule Exeguile";
            // 
            // checkBoxStartEmuleIfClose
            // 
            this.checkBoxStartEmuleIfClose.AutoSize = true;
            this.checkBoxStartEmuleIfClose.Location = new System.Drawing.Point(9, 114);
            this.checkBoxStartEmuleIfClose.Name = "checkBoxStartEmuleIfClose";
            this.checkBoxStartEmuleIfClose.Size = new System.Drawing.Size(116, 17);
            this.checkBoxStartEmuleIfClose.TabIndex = 49;
            this.checkBoxStartEmuleIfClose.Text = "Start Emule if close";
            this.checkBoxStartEmuleIfClose.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 91);
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
            this.numericUpDownIntervalTime.Location = new System.Drawing.Point(295, 47);
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
            this.label7.Size = new System.Drawing.Size(147, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "Check feed Interval time [min]";
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
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.buttonCheckNow);
            this.groupBox1.Controls.Add(this.textBoxPassword);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.textBoxServiceUrl);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 246);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Network";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(117, 32);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(213, 13);
            this.label14.TabIndex = 35;
            this.label14.Text = "Insert Emule Url like \"http://localhost:4000\"";
            // 
            // buttonCheckNow
            // 
            this.buttonCheckNow.Location = new System.Drawing.Point(261, 83);
            this.buttonCheckNow.Name = "buttonCheckNow";
            this.buttonCheckNow.Size = new System.Drawing.Size(75, 23);
            this.buttonCheckNow.TabIndex = 34;
            this.buttonCheckNow.Text = "Check Now";
            this.buttonCheckNow.UseVisualStyleBackColor = true;
            this.buttonCheckNow.Click += new System.EventHandler(this.buttonCheckNow_Click);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(108, 57);
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
            this.label2.Location = new System.Drawing.Point(6, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Password";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageNetwork);
            this.tabControl1.Controls.Add(this.tabPageEmail);
            this.tabControl1.Controls.Add(this.tabPageUpdate);
            this.tabControl1.Controls.Add(this.tabPageLog);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(365, 274);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageEmail
            // 
            this.tabPageEmail.Controls.Add(this.buttonTestEmailNotification);
            this.tabPageEmail.Controls.Add(this.textBoxMailSender);
            this.tabPageEmail.Controls.Add(this.label11);
            this.tabPageEmail.Controls.Add(this.textBoxMailReceiver);
            this.tabPageEmail.Controls.Add(this.label10);
            this.tabPageEmail.Controls.Add(this.label9);
            this.tabPageEmail.Controls.Add(this.textBoxServerSmtp);
            this.tabPageEmail.Controls.Add(this.checkBoxEmailNotification);
            this.tabPageEmail.Location = new System.Drawing.Point(4, 22);
            this.tabPageEmail.Name = "tabPageEmail";
            this.tabPageEmail.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEmail.Size = new System.Drawing.Size(357, 248);
            this.tabPageEmail.TabIndex = 2;
            this.tabPageEmail.Text = "Email Notification";
            this.tabPageEmail.UseVisualStyleBackColor = true;
            // 
            // buttonTestEmailNotification
            // 
            this.buttonTestEmailNotification.Location = new System.Drawing.Point(267, 120);
            this.buttonTestEmailNotification.Name = "buttonTestEmailNotification";
            this.buttonTestEmailNotification.Size = new System.Drawing.Size(75, 23);
            this.buttonTestEmailNotification.TabIndex = 7;
            this.buttonTestEmailNotification.Text = "Test Email";
            this.buttonTestEmailNotification.UseVisualStyleBackColor = true;
            this.buttonTestEmailNotification.Click += new System.EventHandler(this.buttonTestEmailNotification_Click);
            // 
            // textBoxMailSender
            // 
            this.textBoxMailSender.Location = new System.Drawing.Point(128, 57);
            this.textBoxMailSender.Name = "textBoxMailSender";
            this.textBoxMailSender.Size = new System.Drawing.Size(214, 20);
            this.textBoxMailSender.TabIndex = 6;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 60);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "Mail Sender";
            // 
            // textBoxMailReceiver
            // 
            this.textBoxMailReceiver.Location = new System.Drawing.Point(128, 83);
            this.textBoxMailReceiver.Name = "textBoxMailReceiver";
            this.textBoxMailReceiver.Size = new System.Drawing.Size(214, 20);
            this.textBoxMailReceiver.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 86);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Mail Receiver";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Server SMTP";
            // 
            // textBoxServerSmtp
            // 
            this.textBoxServerSmtp.Location = new System.Drawing.Point(128, 31);
            this.textBoxServerSmtp.Name = "textBoxServerSmtp";
            this.textBoxServerSmtp.Size = new System.Drawing.Size(214, 20);
            this.textBoxServerSmtp.TabIndex = 1;
            // 
            // checkBoxEmailNotification
            // 
            this.checkBoxEmailNotification.AutoSize = true;
            this.checkBoxEmailNotification.Location = new System.Drawing.Point(15, 11);
            this.checkBoxEmailNotification.Name = "checkBoxEmailNotification";
            this.checkBoxEmailNotification.Size = new System.Drawing.Size(107, 17);
            this.checkBoxEmailNotification.TabIndex = 0;
            this.checkBoxEmailNotification.Text = "Email Notification";
            this.checkBoxEmailNotification.UseVisualStyleBackColor = true;
            // 
            // tabPageUpdate
            // 
            this.tabPageUpdate.Controls.Add(this.label12);
            this.tabPageUpdate.Controls.Add(this.numericUpDownIntervalCheck);
            this.tabPageUpdate.Location = new System.Drawing.Point(4, 22);
            this.tabPageUpdate.Name = "tabPageUpdate";
            this.tabPageUpdate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUpdate.Size = new System.Drawing.Size(357, 248);
            this.tabPageUpdate.TabIndex = 3;
            this.tabPageUpdate.Text = "Software update ";
            this.tabPageUpdate.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 13);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(234, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Software update check interval days (0 = never)";
            // 
            // numericUpDownIntervalCheck
            // 
            this.numericUpDownIntervalCheck.Location = new System.Drawing.Point(277, 11);
            this.numericUpDownIntervalCheck.Name = "numericUpDownIntervalCheck";
            this.numericUpDownIntervalCheck.Size = new System.Drawing.Size(65, 20);
            this.numericUpDownIntervalCheck.TabIndex = 0;
            // 
            // tabPageLog
            // 
            this.tabPageLog.Controls.Add(this.checkBoxSaveLogToFile);
            this.tabPageLog.Controls.Add(this.checkBoxVerbose);
            this.tabPageLog.Controls.Add(this.checkBoxAutoClear);
            this.tabPageLog.Location = new System.Drawing.Point(4, 22);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLog.Size = new System.Drawing.Size(357, 248);
            this.tabPageLog.TabIndex = 4;
            this.tabPageLog.Text = "Log";
            this.tabPageLog.UseVisualStyleBackColor = true;
            // 
            // checkBoxSaveLogToFile
            // 
            this.checkBoxSaveLogToFile.AutoSize = true;
            this.checkBoxSaveLogToFile.Location = new System.Drawing.Point(252, 29);
            this.checkBoxSaveLogToFile.Name = "checkBoxSaveLogToFile";
            this.checkBoxSaveLogToFile.Size = new System.Drawing.Size(99, 17);
            this.checkBoxSaveLogToFile.TabIndex = 57;
            this.checkBoxSaveLogToFile.Text = "Save log to File";
            this.checkBoxSaveLogToFile.UseVisualStyleBackColor = true;
            // 
            // checkBoxVerbose
            // 
            this.checkBoxVerbose.AutoSize = true;
            this.checkBoxVerbose.Location = new System.Drawing.Point(127, 29);
            this.checkBoxVerbose.Name = "checkBoxVerbose";
            this.checkBoxVerbose.Size = new System.Drawing.Size(86, 17);
            this.checkBoxVerbose.TabIndex = 56;
            this.checkBoxVerbose.Text = "Verbose Log";
            this.checkBoxVerbose.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoClear
            // 
            this.checkBoxAutoClear.AutoSize = true;
            this.checkBoxAutoClear.Location = new System.Drawing.Point(6, 29);
            this.checkBoxAutoClear.Name = "checkBoxAutoClear";
            this.checkBoxAutoClear.Size = new System.Drawing.Size(96, 17);
            this.checkBoxAutoClear.TabIndex = 55;
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxSimultaneousDownloadForFeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinDownloadToStrarTEmule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntervalTime)).EndInit();
            this.tabPageNetwork.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageEmail.ResumeLayout(false);
            this.tabPageEmail.PerformLayout();
            this.tabPageUpdate.ResumeLayout(false);
            this.tabPageUpdate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntervalCheck)).EndInit();
            this.tabPageLog.ResumeLayout(false);
            this.tabPageLog.PerformLayout();
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
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBoxServiceUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDownMinDownloadToStrarTEmule;
        private System.Windows.Forms.TabPage tabPageEmail;
        private System.Windows.Forms.TextBox textBoxMailReceiver;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxServerSmtp;
        private System.Windows.Forms.CheckBox checkBoxEmailNotification;
        private System.Windows.Forms.TextBox textBoxMailSender;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonTestEmailNotification;
        private System.Windows.Forms.TabPage tabPageUpdate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numericUpDownIntervalCheck;
        private System.Windows.Forms.TabPage tabPageLog;
        private System.Windows.Forms.CheckBox checkBoxSaveLogToFile;
        private System.Windows.Forms.CheckBox checkBoxVerbose;
        private System.Windows.Forms.CheckBox checkBoxAutoClear;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxSimultaneousDownloadForFeed;
        private System.Windows.Forms.Label label14;
    }
}