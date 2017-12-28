namespace TvUndergroundDownloader
{
    partial class FormMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.timerRssCheck = new System.Windows.Forms.Timer(this.components);
            this.timerDelayStartup = new System.Windows.Forms.Timer(this.components);
            this.tabLog = new System.Windows.Forms.TabPage();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            this.rssSubscriptionListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.checkBoxFeedPauseDownload = new System.Windows.Forms.CheckBox();
            this.domainUpDownMaxSilumtaiusDownload = new System.Windows.Forms.DomainUpDown();
            this.dataGridViewFeedFiles = new System.Windows.Forms.DataGridView();
            this.fileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.publicationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.downloadDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripFileFeed = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.markAsDownloadedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewMain = new System.Windows.Forms.DataGridView();
            this.ColumnTitleCompact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDubLanguage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTotalDownloads = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.contextMenuStripFeed = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.enableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemUpdateStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.labelMaxSimultaneousDownloads = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelTotalFiles = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.labelLastDownloadDate = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.labelFeedUrl = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.labelFeedPauseDownload = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelFeedCategory = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabRecentActivity = new System.Windows.Forms.TabPage();
            this.dataGridViewRecentActivity = new System.Windows.Forms.DataGridView();
            this.FileNameDataGridColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdateDataGridColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPending = new System.Windows.Forms.TabPage();
            this.listBoxPending = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelCheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.globalOptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.testAutoStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testAutoCloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoStartEMuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoCloseEMuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.importDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.channelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.reportABugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionCheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.timerAutoClose = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddFeed = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCheckNow = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabLog.SuspendLayout();
            this.tabMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rssSubscriptionListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFeedFiles)).BeginInit();
            this.contextMenuStripFileFeed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMain)).BeginInit();
            this.contextMenuStripFeed.SuspendLayout();
            this.tabRecentActivity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRecentActivity)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPending.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerRssCheck
            // 
            this.timerRssCheck.Enabled = true;
            this.timerRssCheck.Interval = 60000;
            this.timerRssCheck.Tick += new System.EventHandler(this.timerRssCheck_Tick);
            // 
            // timerDelayStartup
            // 
            this.timerDelayStartup.Enabled = true;
            this.timerDelayStartup.Interval = 1000;
            this.timerDelayStartup.Tick += new System.EventHandler(this.timerDelayStartup_Tick);
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.richTextBoxLog);
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(648, 441);
            this.tabLog.TabIndex = 1;
            this.tabLog.Text = "Log";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLog.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(642, 435);
            this.richTextBoxLog.TabIndex = 0;
            this.richTextBoxLog.Text = "";
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.comboBoxCategory);
            this.tabMain.Controls.Add(this.checkBoxFeedPauseDownload);
            this.tabMain.Controls.Add(this.domainUpDownMaxSilumtaiusDownload);
            this.tabMain.Controls.Add(this.dataGridViewFeedFiles);
            this.tabMain.Controls.Add(this.dataGridViewMain);
            this.tabMain.Controls.Add(this.labelMaxSimultaneousDownloads);
            this.tabMain.Controls.Add(this.label1);
            this.tabMain.Controls.Add(this.labelTotalFiles);
            this.tabMain.Controls.Add(this.label15);
            this.tabMain.Controls.Add(this.labelLastDownloadDate);
            this.tabMain.Controls.Add(this.label13);
            this.tabMain.Controls.Add(this.labelFeedUrl);
            this.tabMain.Controls.Add(this.label11);
            this.tabMain.Controls.Add(this.labelFeedPauseDownload);
            this.tabMain.Controls.Add(this.label9);
            this.tabMain.Controls.Add(this.labelFeedCategory);
            this.tabMain.Controls.Add(this.label7);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(648, 441);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "Feeds";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.rssSubscriptionListBindingSource, "Category", true));
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.Location = new System.Drawing.Point(374, 286);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCategory.TabIndex = 42;
            this.comboBoxCategory.SelectedIndexChanged += new System.EventHandler(this.comboBoxCategory_SelectedIndexChanged);
            // 
            // rssSubscriptionListBindingSource
            // 
            this.rssSubscriptionListBindingSource.DataSource = typeof(TvUndergroundDownloaderLib.RssSubscriptionList);
            // 
            // checkBoxFeedPauseDownload
            // 
            this.checkBoxFeedPauseDownload.AutoSize = true;
            this.checkBoxFeedPauseDownload.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.rssSubscriptionListBindingSource, "PauseDownload", true));
            this.checkBoxFeedPauseDownload.Location = new System.Drawing.Point(501, 290);
            this.checkBoxFeedPauseDownload.Name = "checkBoxFeedPauseDownload";
            this.checkBoxFeedPauseDownload.Size = new System.Drawing.Size(107, 17);
            this.checkBoxFeedPauseDownload.TabIndex = 41;
            this.checkBoxFeedPauseDownload.Text = "Pause Download";
            this.checkBoxFeedPauseDownload.UseVisualStyleBackColor = true;
            this.checkBoxFeedPauseDownload.CheckedChanged += new System.EventHandler(this.checkBoxFeedPauseDownload_CheckedChanged);
            // 
            // domainUpDownMaxSilumtaiusDownload
            // 
            this.domainUpDownMaxSilumtaiusDownload.Location = new System.Drawing.Point(501, 264);
            this.domainUpDownMaxSilumtaiusDownload.Name = "domainUpDownMaxSilumtaiusDownload";
            this.domainUpDownMaxSilumtaiusDownload.Size = new System.Drawing.Size(120, 20);
            this.domainUpDownMaxSilumtaiusDownload.TabIndex = 40;
            this.domainUpDownMaxSilumtaiusDownload.Text = "domainUpDown1";
            // 
            // dataGridViewFeedFiles
            // 
            this.dataGridViewFeedFiles.AutoGenerateColumns = false;
            this.dataGridViewFeedFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFeedFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fileNameDataGridViewTextBoxColumn,
            this.publicationDateDataGridViewTextBoxColumn,
            this.downloadDateDataGridViewTextBoxColumn});
            this.dataGridViewFeedFiles.ContextMenuStrip = this.contextMenuStripFileFeed;
            this.dataGridViewFeedFiles.DataSource = this.filesBindingSource;
            this.dataGridViewFeedFiles.Location = new System.Drawing.Point(9, 327);
            this.dataGridViewFeedFiles.Name = "dataGridViewFeedFiles";
            this.dataGridViewFeedFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewFeedFiles.Size = new System.Drawing.Size(631, 106);
            this.dataGridViewFeedFiles.TabIndex = 39;
            // 
            // fileNameDataGridViewTextBoxColumn
            // 
            this.fileNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fileNameDataGridViewTextBoxColumn.DataPropertyName = "FileName";
            this.fileNameDataGridViewTextBoxColumn.HeaderText = "FileName";
            this.fileNameDataGridViewTextBoxColumn.Name = "fileNameDataGridViewTextBoxColumn";
            this.fileNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // publicationDateDataGridViewTextBoxColumn
            // 
            this.publicationDateDataGridViewTextBoxColumn.DataPropertyName = "PublicationDate";
            this.publicationDateDataGridViewTextBoxColumn.HeaderText = "PublicationDate";
            this.publicationDateDataGridViewTextBoxColumn.Name = "publicationDateDataGridViewTextBoxColumn";
            // 
            // downloadDateDataGridViewTextBoxColumn
            // 
            this.downloadDateDataGridViewTextBoxColumn.DataPropertyName = "DownloadDate";
            this.downloadDateDataGridViewTextBoxColumn.HeaderText = "DownloadDate";
            this.downloadDateDataGridViewTextBoxColumn.Name = "downloadDateDataGridViewTextBoxColumn";
            // 
            // contextMenuStripFileFeed
            // 
            this.contextMenuStripFileFeed.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem1,
            this.markAsDownloadedToolStripMenuItem});
            this.contextMenuStripFileFeed.Name = "contextMenuStrip2";
            this.contextMenuStripFileFeed.Size = new System.Drawing.Size(186, 48);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
            this.deleteToolStripMenuItem1.Text = "Redownload";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItemRedownload_Click);
            // 
            // markAsDownloadedToolStripMenuItem
            // 
            this.markAsDownloadedToolStripMenuItem.Name = "markAsDownloadedToolStripMenuItem";
            this.markAsDownloadedToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.markAsDownloadedToolStripMenuItem.Text = "Mark as Downloaded";
            this.markAsDownloadedToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItemMarkAsDownload_Click);
            // 
            // filesBindingSource
            // 
            this.filesBindingSource.DataMember = "Files";
            this.filesBindingSource.DataSource = this.rssSubscriptionListBindingSource;
            // 
            // dataGridViewMain
            // 
            this.dataGridViewMain.AllowUserToAddRows = false;
            this.dataGridViewMain.AllowUserToDeleteRows = false;
            this.dataGridViewMain.AutoGenerateColumns = false;
            this.dataGridViewMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTitleCompact,
            this.ColumnDubLanguage,
            this.ColumnTotalDownloads,
            this.ColumnStatus,
            this.CheckBoxColumn});
            this.dataGridViewMain.ContextMenuStrip = this.contextMenuStripFeed;
            this.dataGridViewMain.DataSource = this.rssSubscriptionListBindingSource;
            this.dataGridViewMain.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewMain.Name = "dataGridViewMain";
            this.dataGridViewMain.ReadOnly = true;
            this.dataGridViewMain.RowHeadersVisible = false;
            this.dataGridViewMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMain.Size = new System.Drawing.Size(642, 242);
            this.dataGridViewMain.TabIndex = 38;
            this.dataGridViewMain.SelectionChanged += new System.EventHandler(this.dataGridViewMain_SelectionChanged);
            // 
            // ColumnTitleCompact
            // 
            this.ColumnTitleCompact.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnTitleCompact.DataPropertyName = "TitleCompact";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ColumnTitleCompact.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnTitleCompact.HeaderText = "Title";
            this.ColumnTitleCompact.Name = "ColumnTitleCompact";
            this.ColumnTitleCompact.ReadOnly = true;
            // 
            // ColumnDubLanguage
            // 
            this.ColumnDubLanguage.DataPropertyName = "DubLanguage";
            this.ColumnDubLanguage.HeaderText = "Dub";
            this.ColumnDubLanguage.Name = "ColumnDubLanguage";
            this.ColumnDubLanguage.ReadOnly = true;
            this.ColumnDubLanguage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnDubLanguage.Width = 33;
            // 
            // ColumnTotalDownloads
            // 
            this.ColumnTotalDownloads.DataPropertyName = "TotalFilesDownloaded";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnTotalDownloads.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnTotalDownloads.HeaderText = "Total Downloads";
            this.ColumnTotalDownloads.Name = "ColumnTotalDownloads";
            this.ColumnTotalDownloads.ReadOnly = true;
            this.ColumnTotalDownloads.Width = 40;
            // 
            // ColumnStatus
            // 
            this.ColumnStatus.DataPropertyName = "CurrentTVUStatus";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnStatus.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnStatus.HeaderText = "Status";
            this.ColumnStatus.Name = "ColumnStatus";
            this.ColumnStatus.ReadOnly = true;
            this.ColumnStatus.Width = 62;
            // 
            // CheckBoxColumn
            // 
            this.CheckBoxColumn.DataPropertyName = "Enabled";
            this.CheckBoxColumn.HeaderText = "Enabled";
            this.CheckBoxColumn.Name = "CheckBoxColumn";
            this.CheckBoxColumn.ReadOnly = true;
            // 
            // contextMenuStripFeed
            // 
            this.contextMenuStripFeed.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAdd,
            this.toolStripMenuItemDelete,
            this.toolStripSeparator5,
            this.enableToolStripMenuItem,
            this.disableToolStripMenuItem,
            this.toolStripSeparator7,
            this.toolStripMenuItemUpdateStatus});
            this.contextMenuStripFeed.Name = "contextMenuStrip1";
            this.contextMenuStripFeed.Size = new System.Drawing.Size(153, 148);
            // 
            // toolStripMenuItemAdd
            // 
            this.toolStripMenuItemAdd.Name = "toolStripMenuItemAdd";
            this.toolStripMenuItemAdd.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemAdd.Text = "Add";
            this.toolStripMenuItemAdd.Click += new System.EventHandler(this.toolStripMenuItemAdd_Click);
            // 
            // toolStripMenuItemDelete
            // 
            this.toolStripMenuItemDelete.Name = "toolStripMenuItemDelete";
            this.toolStripMenuItemDelete.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemDelete.Text = "Delete";
            this.toolStripMenuItemDelete.Click += new System.EventHandler(this.toolStripMenuItemDelete_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(149, 6);
            // 
            // enableToolStripMenuItem
            // 
            this.enableToolStripMenuItem.Name = "enableToolStripMenuItem";
            this.enableToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.enableToolStripMenuItem.Text = "Enable";
            this.enableToolStripMenuItem.Click += new System.EventHandler(this.enableToolStripMenuItem_Click);
            // 
            // disableToolStripMenuItem
            // 
            this.disableToolStripMenuItem.Name = "disableToolStripMenuItem";
            this.disableToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.disableToolStripMenuItem.Text = "Disable";
            this.disableToolStripMenuItem.Click += new System.EventHandler(this.disableToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(149, 6);
            // 
            // toolStripMenuItemUpdateStatus
            // 
            this.toolStripMenuItemUpdateStatus.Name = "toolStripMenuItemUpdateStatus";
            this.toolStripMenuItemUpdateStatus.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemUpdateStatus.Text = "Update Status";
            this.toolStripMenuItemUpdateStatus.Click += new System.EventHandler(this.toolStripMenuItemUpdateStatus_Click);
            // 
            // labelMaxSimultaneousDownloads
            // 
            this.labelMaxSimultaneousDownloads.AutoSize = true;
            this.labelMaxSimultaneousDownloads.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.rssSubscriptionListBindingSource, "MaxSimultaneousDownload", true));
            this.labelMaxSimultaneousDownloads.Location = new System.Drawing.Point(498, 248);
            this.labelMaxSimultaneousDownloads.Name = "labelMaxSimultaneousDownloads";
            this.labelMaxSimultaneousDownloads.Size = new System.Drawing.Size(53, 13);
            this.labelMaxSimultaneousDownloads.TabIndex = 37;
            this.labelMaxSimultaneousDownloads.Text = "labelMSD";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(289, 248);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 13);
            this.label1.TabIndex = 36;
            this.label1.Text = "Max simultaneous downloads";
            // 
            // labelTotalFiles
            // 
            this.labelTotalFiles.AutoSize = true;
            this.labelTotalFiles.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.rssSubscriptionListBindingSource, "TotalFilesDownloaded", true));
            this.labelTotalFiles.Location = new System.Drawing.Point(106, 300);
            this.labelTotalFiles.Name = "labelTotalFiles";
            this.labelTotalFiles.Size = new System.Drawing.Size(74, 13);
            this.labelTotalFiles.TabIndex = 35;
            this.labelTotalFiles.Text = "labelTotalFiles";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 300);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(51, 13);
            this.label15.TabIndex = 34;
            this.label15.Text = "Totat File";
            // 
            // labelLastDownloadDate
            // 
            this.labelLastDownloadDate.AutoSize = true;
            this.labelLastDownloadDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.rssSubscriptionListBindingSource, "LastDownload", true));
            this.labelLastDownloadDate.Location = new System.Drawing.Point(106, 287);
            this.labelLastDownloadDate.Name = "labelLastDownloadDate";
            this.labelLastDownloadDate.Size = new System.Drawing.Size(120, 13);
            this.labelLastDownloadDate.TabIndex = 33;
            this.labelLastDownloadDate.Text = "labelLastDownloadDate";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 287);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 13);
            this.label13.TabIndex = 32;
            this.label13.Text = "Last Download";
            // 
            // labelFeedUrl
            // 
            this.labelFeedUrl.AutoSize = true;
            this.labelFeedUrl.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.rssSubscriptionListBindingSource, "Url", true));
            this.labelFeedUrl.Location = new System.Drawing.Point(106, 274);
            this.labelFeedUrl.Name = "labelFeedUrl";
            this.labelFeedUrl.Size = new System.Drawing.Size(66, 13);
            this.labelFeedUrl.TabIndex = 31;
            this.labelFeedUrl.Text = "labelFeedUrl";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 274);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "RSS link";
            // 
            // labelFeedPauseDownload
            // 
            this.labelFeedPauseDownload.AutoSize = true;
            this.labelFeedPauseDownload.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.rssSubscriptionListBindingSource, "PauseDownload", true));
            this.labelFeedPauseDownload.Location = new System.Drawing.Point(106, 261);
            this.labelFeedPauseDownload.Name = "labelFeedPauseDownload";
            this.labelFeedPauseDownload.Size = new System.Drawing.Size(131, 13);
            this.labelFeedPauseDownload.TabIndex = 29;
            this.labelFeedPauseDownload.Text = "labelFeedPauseDownload";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 261);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "Pause";
            // 
            // labelFeedCategory
            // 
            this.labelFeedCategory.AutoSize = true;
            this.labelFeedCategory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.rssSubscriptionListBindingSource, "Category", true));
            this.labelFeedCategory.Location = new System.Drawing.Point(106, 248);
            this.labelFeedCategory.Name = "labelFeedCategory";
            this.labelFeedCategory.Size = new System.Drawing.Size(95, 13);
            this.labelFeedCategory.TabIndex = 27;
            this.labelFeedCategory.Text = "labelFeedCategory";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 248);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Category";
            // 
            // tabRecentActivity
            // 
            this.tabRecentActivity.Controls.Add(this.dataGridViewRecentActivity);
            this.tabRecentActivity.Controls.Add(this.progressBar1);
            this.tabRecentActivity.Location = new System.Drawing.Point(4, 22);
            this.tabRecentActivity.Name = "tabRecentActivity";
            this.tabRecentActivity.Size = new System.Drawing.Size(648, 441);
            this.tabRecentActivity.TabIndex = 4;
            this.tabRecentActivity.Text = "Recent Activity";
            this.tabRecentActivity.UseVisualStyleBackColor = true;
            // 
            // dataGridViewRecentActivity
            // 
            this.dataGridViewRecentActivity.AllowUserToAddRows = false;
            this.dataGridViewRecentActivity.AllowUserToDeleteRows = false;
            this.dataGridViewRecentActivity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRecentActivity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileNameDataGridColumn,
            this.LastUpdateDataGridColumn});
            this.dataGridViewRecentActivity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRecentActivity.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewRecentActivity.Name = "dataGridViewRecentActivity";
            this.dataGridViewRecentActivity.ReadOnly = true;
            this.dataGridViewRecentActivity.RowHeadersVisible = false;
            this.dataGridViewRecentActivity.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewRecentActivity.Size = new System.Drawing.Size(648, 441);
            this.dataGridViewRecentActivity.TabIndex = 37;
            // 
            // FileNameDataGridColumn
            // 
            this.FileNameDataGridColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FileNameDataGridColumn.DataPropertyName = "FileName";
            this.FileNameDataGridColumn.HeaderText = "FileName";
            this.FileNameDataGridColumn.Name = "FileNameDataGridColumn";
            this.FileNameDataGridColumn.ReadOnly = true;
            // 
            // LastUpdateDataGridColumn
            // 
            this.LastUpdateDataGridColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.LastUpdateDataGridColumn.DataPropertyName = "LastUpdate";
            this.LastUpdateDataGridColumn.HeaderText = "Last Update";
            this.LastUpdateDataGridColumn.Name = "LastUpdateDataGridColumn";
            this.LastUpdateDataGridColumn.ReadOnly = true;
            this.LastUpdateDataGridColumn.Width = 90;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(4, 425);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(535, 23);
            this.progressBar1.TabIndex = 36;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabRecentActivity);
            this.tabControl1.Controls.Add(this.tabMain);
            this.tabControl1.Controls.Add(this.tabLog);
            this.tabControl1.Controls.Add(this.tabPending);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 49);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 27, 3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(656, 467);
            this.tabControl1.TabIndex = 21;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPending
            // 
            this.tabPending.Controls.Add(this.listBoxPending);
            this.tabPending.Location = new System.Drawing.Point(4, 22);
            this.tabPending.Name = "tabPending";
            this.tabPending.Padding = new System.Windows.Forms.Padding(3);
            this.tabPending.Size = new System.Drawing.Size(648, 441);
            this.tabPending.TabIndex = 5;
            this.tabPending.Text = "Pending";
            this.tabPending.UseVisualStyleBackColor = true;
            // 
            // listBoxPending
            // 
            this.listBoxPending.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPending.FormattingEnabled = true;
            this.listBoxPending.Location = new System.Drawing.Point(3, 3);
            this.listBoxPending.Name = "listBoxPending";
            this.listBoxPending.Size = new System.Drawing.Size(642, 435);
            this.listBoxPending.Sorted = true;
            this.listBoxPending.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.channelToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(656, 24);
            this.menuStrip1.TabIndex = 22;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkNowToolStripMenuItem,
            this.loginToolStripMenuItem,
            this.cancelCheckToolStripMenuItem,
            this.optionToolStripMenuItem,
            this.toolStripSeparator2,
            this.hideToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // checkNowToolStripMenuItem
            // 
            this.checkNowToolStripMenuItem.Name = "checkNowToolStripMenuItem";
            this.checkNowToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.checkNowToolStripMenuItem.Text = "Check Now";
            this.checkNowToolStripMenuItem.Click += new System.EventHandler(this.checkNowToolStripMenuItem_Click);
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.loginToolStripMenuItem.Text = "Login";
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // cancelCheckToolStripMenuItem
            // 
            this.cancelCheckToolStripMenuItem.Enabled = false;
            this.cancelCheckToolStripMenuItem.Name = "cancelCheckToolStripMenuItem";
            this.cancelCheckToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.cancelCheckToolStripMenuItem.Text = "Cancel Check";
            this.cancelCheckToolStripMenuItem.Click += new System.EventHandler(this.cancelCheckToolStripMenuItem_Click);
            // 
            // optionToolStripMenuItem
            // 
            this.optionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.globalOptionToolStripMenuItem,
            this.toolStripSeparator3,
            this.testAutoStartToolStripMenuItem,
            this.testAutoCloseToolStripMenuItem,
            this.autoStartEMuleToolStripMenuItem,
            this.autoCloseEMuleToolStripMenuItem,
            this.toolStripSeparator6,
            this.importDataToolStripMenuItem,
            this.exportDataToolStripMenuItem});
            this.optionToolStripMenuItem.Name = "optionToolStripMenuItem";
            this.optionToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.optionToolStripMenuItem.Text = "Options";
            // 
            // globalOptionToolStripMenuItem
            // 
            this.globalOptionToolStripMenuItem.Name = "globalOptionToolStripMenuItem";
            this.globalOptionToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.globalOptionToolStripMenuItem.Text = "Global Option";
            this.globalOptionToolStripMenuItem.Click += new System.EventHandler(this.globalOptionToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(165, 6);
            // 
            // testAutoStartToolStripMenuItem
            // 
            this.testAutoStartToolStripMenuItem.Name = "testAutoStartToolStripMenuItem";
            this.testAutoStartToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.testAutoStartToolStripMenuItem.Text = "Test Auto Start";
            this.testAutoStartToolStripMenuItem.Click += new System.EventHandler(this.testAutoStartToolStripMenuItem_Click);
            // 
            // testAutoCloseToolStripMenuItem
            // 
            this.testAutoCloseToolStripMenuItem.Name = "testAutoCloseToolStripMenuItem";
            this.testAutoCloseToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.testAutoCloseToolStripMenuItem.Text = "Test Auto Close";
            this.testAutoCloseToolStripMenuItem.Click += new System.EventHandler(this.testAutoCloseToolStripMenuItem_Click);
            // 
            // autoStartEMuleToolStripMenuItem
            // 
            this.autoStartEMuleToolStripMenuItem.Name = "autoStartEMuleToolStripMenuItem";
            this.autoStartEMuleToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.autoStartEMuleToolStripMenuItem.Text = "Auto Start eMule";
            this.autoStartEMuleToolStripMenuItem.Click += new System.EventHandler(this.autoStartEMuleToolStripMenuItem_Click);
            // 
            // autoCloseEMuleToolStripMenuItem
            // 
            this.autoCloseEMuleToolStripMenuItem.Name = "autoCloseEMuleToolStripMenuItem";
            this.autoCloseEMuleToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.autoCloseEMuleToolStripMenuItem.Text = "Auto Close eMule";
            this.autoCloseEMuleToolStripMenuItem.Click += new System.EventHandler(this.autoCloseEMuleToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(165, 6);
            // 
            // importDataToolStripMenuItem
            // 
            this.importDataToolStripMenuItem.Name = "importDataToolStripMenuItem";
            this.importDataToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.importDataToolStripMenuItem.Text = "Import Data";
            this.importDataToolStripMenuItem.Click += new System.EventHandler(this.importDataToolStripMenuItem_Click);
            // 
            // exportDataToolStripMenuItem
            // 
            this.exportDataToolStripMenuItem.Name = "exportDataToolStripMenuItem";
            this.exportDataToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.exportDataToolStripMenuItem.Text = "Export Data";
            this.exportDataToolStripMenuItem.Click += new System.EventHandler(this.exportDataToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
            // 
            // hideToolStripMenuItem
            // 
            this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            this.hideToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.hideToolStripMenuItem.Text = "Hide";
            this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // channelToolStripMenuItem
            // 
            this.channelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.channelToolStripMenuItem.Name = "channelToolStripMenuItem";
            this.channelToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.channelToolStripMenuItem.Text = "Feeds";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.reportABugToolStripMenuItem,
            this.forumToolStripMenuItem,
            this.versionCheckToolStripMenuItem,
            this.toolStripMenuItemAbout});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(148, 22);
            this.helpToolStripMenuItem1.Text = "Help";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // reportABugToolStripMenuItem
            // 
            this.reportABugToolStripMenuItem.Name = "reportABugToolStripMenuItem";
            this.reportABugToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.reportABugToolStripMenuItem.Text = "Report a Bug";
            this.reportABugToolStripMenuItem.Click += new System.EventHandler(this.reportABugToolStripMenuItem_Click);
            // 
            // forumToolStripMenuItem
            // 
            this.forumToolStripMenuItem.Name = "forumToolStripMenuItem";
            this.forumToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.forumToolStripMenuItem.Text = "Forum";
            this.forumToolStripMenuItem.Click += new System.EventHandler(this.forumToolStripMenuItem_Click);
            // 
            // versionCheckToolStripMenuItem
            // 
            this.versionCheckToolStripMenuItem.Name = "versionCheckToolStripMenuItem";
            this.versionCheckToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.versionCheckToolStripMenuItem.Text = "Version Check";
            this.versionCheckToolStripMenuItem.Click += new System.EventHandler(this.versionCheckToolStripMenuItem_Click);
            // 
            // toolStripMenuItemAbout
            // 
            this.toolStripMenuItemAbout.Name = "toolStripMenuItemAbout";
            this.toolStripMenuItemAbout.Size = new System.Drawing.Size(148, 22);
            this.toolStripMenuItemAbout.Text = "About";
            this.toolStripMenuItemAbout.Click += new System.EventHandler(this.toolStripMenuItemAbout_Click);
            // 
            // timerAutoClose
            // 
            this.timerAutoClose.Interval = 60000;
            this.timerAutoClose.Tick += new System.EventHandler(this.timerAutoClose_Tick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddFeed,
            this.toolStripButtonCheckNow,
            this.toolStripButtonStop});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(656, 25);
            this.toolStrip1.TabIndex = 23;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonAddFeed
            // 
            this.toolStripButtonAddFeed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddFeed.Image = global::TvUndergroundDownloader.Properties.Resources.icon_add;
            this.toolStripButtonAddFeed.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddFeed.Name = "toolStripButtonAddFeed";
            this.toolStripButtonAddFeed.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAddFeed.Text = "Add feed";
            this.toolStripButtonAddFeed.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripButtonAddFeed.Click += new System.EventHandler(this.toolStripButtonAddFeed_Click);
            // 
            // toolStripButtonCheckNow
            // 
            this.toolStripButtonCheckNow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCheckNow.Image = global::TvUndergroundDownloader.Properties.Resources.icon_update;
            this.toolStripButtonCheckNow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCheckNow.Name = "toolStripButtonCheckNow";
            this.toolStripButtonCheckNow.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonCheckNow.Text = "Check Now";
            this.toolStripButtonCheckNow.ToolTipText = "Check Now";
            this.toolStripButtonCheckNow.Click += new System.EventHandler(this.toolStripButtonCheckNow_Click);
            // 
            // toolStripButtonStop
            // 
            this.toolStripButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStop.Enabled = false;
            this.toolStripButtonStop.Image = global::TvUndergroundDownloader.Properties.Resources.icon_stop;
            this.toolStripButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStop.Name = "toolStripButtonStop";
            this.toolStripButtonStop.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonStop.Text = "Stop";
            this.toolStripButtonStop.Click += new System.EventHandler(this.toolStripButtonStop_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CurrentTVUStatus";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewTextBoxColumn1.HeaderText = "Status";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 62;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "CurrentTVUStatus";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn2.HeaderText = "Status";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 62;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "CurrentTVUStatus";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn3.HeaderText = "Status";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 62;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 516);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "TvUnderground Downloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.tabLog.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rssSubscriptionListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFeedFiles)).EndInit();
            this.contextMenuStripFileFeed.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.filesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMain)).EndInit();
            this.contextMenuStripFeed.ResumeLayout(false);
            this.tabRecentActivity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRecentActivity)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPending.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       

        #endregion

        private System.Windows.Forms.Timer timerRssCheck;
        private System.Windows.Forms.Timer timerDelayStartup;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.Label labelTotalFiles;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label labelLastDownloadDate;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelFeedUrl;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelFeedPauseDownload;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelFeedCategory;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabRecentActivity;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkNowToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem channelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem globalOptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoStartEMuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem autoCloseEMuleToolStripMenuItem;
        private System.Windows.Forms.Timer timerAutoClose;
        private System.Windows.Forms.ToolStripMenuItem testAutoCloseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testAutoStartToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFeed;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAdd;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFileFeed;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cancelCheckToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportABugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.Label labelMaxSimultaneousDownloads;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPending;
        private System.Windows.Forms.ListBox listBoxPending;
        private System.Windows.Forms.ToolStripMenuItem forumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddFeed;
        private System.Windows.Forms.ToolStripButton toolStripButtonCheckNow;
        private System.Windows.Forms.ToolStripButton toolStripButtonStop;
        private System.Windows.Forms.DataGridView dataGridViewRecentActivity;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem enableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem versionCheckToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileNameDataGridColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdateDataGridColumn;
        private System.Windows.Forms.ToolStripMenuItem exportDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importDataToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.DataGridView dataGridViewMain;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem markAsDownloadedToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUpdateStatus;
        private System.Windows.Forms.BindingSource rssSubscriptionListBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTitleCompact;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDubLanguage;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTotalDownloads;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStatus;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridView dataGridViewFeedFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn publicationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn downloadDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource filesBindingSource;
        private System.Windows.Forms.DomainUpDown domainUpDownMaxSilumtaiusDownload;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.ComboBox comboBoxCategory;
        private System.Windows.Forms.CheckBox checkBoxFeedPauseDownload;
    }
}

