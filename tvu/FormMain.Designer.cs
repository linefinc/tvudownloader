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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.timerRssCheck = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.tabLog = new System.Windows.Forms.TabPage();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.dataGridViewMain = new System.Windows.Forms.DataGridView();
            this.DataGridViewTextBoxColumnTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewImageColumnDubLanguage = new System.Windows.Forms.DataGridViewImageColumn();
            this.DataGridViewTextBoxColumnTotalDownloads = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewTextBoxColumnLastUpdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewTextBoxColumnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewTextBoxColumnEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripFeed = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.enableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.listViewFeedFilesList = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripFileFeed = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabRecentActivity = new System.Windows.Forms.TabPage();
            this.dataGridViewRecentActivity = new System.Windows.Forms.DataGridView();
            this.FileNameDataGridColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdateDataGridColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPending = new System.Windows.Forms.TabPage();
            this.listBoxPending = new System.Windows.Forms.ListBox();
            this.contextMenuStripPending = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.excludeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelCheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.globalOptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oPMLExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoClearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.verboseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.reportABugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionCheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.timerAutoClose = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddFeed = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCheckNow = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabLog.SuspendLayout();
            this.tabMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMain)).BeginInit();
            this.contextMenuStripFeed.SuspendLayout();
            this.contextMenuStripFileFeed.SuspendLayout();
            this.tabRecentActivity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRecentActivity)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPending.SuspendLayout();
            this.contextMenuStripPending.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerRssCheck
            // 
            this.timerRssCheck.Enabled = true;
            this.timerRssCheck.Interval = 60000;
            this.timerRssCheck.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
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
            this.tabMain.Controls.Add(this.listViewFeedFilesList);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(648, 441);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "Feeds";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // dataGridViewMain
            // 
            this.dataGridViewMain.AllowUserToAddRows = false;
            this.dataGridViewMain.AllowUserToDeleteRows = false;
            this.dataGridViewMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DataGridViewTextBoxColumnTitle,
            this.DataGridViewImageColumnDubLanguage,
            this.DataGridViewTextBoxColumnTotalDownloads,
            this.DataGridViewTextBoxColumnLastUpdate,
            this.DataGridViewTextBoxColumnStatus,
            this.DataGridViewTextBoxColumnEnabled});
            this.dataGridViewMain.ContextMenuStrip = this.contextMenuStripFeed;
            this.dataGridViewMain.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewMain.Name = "dataGridViewMain";
            this.dataGridViewMain.ReadOnly = true;
            this.dataGridViewMain.RowHeadersVisible = false;
            this.dataGridViewMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMain.Size = new System.Drawing.Size(642, 242);
            this.dataGridViewMain.TabIndex = 38;
            this.dataGridViewMain.SelectionChanged += new System.EventHandler(this.dataGridViewMain_SelectionChanged);
            // 
            // DataGridViewTextBoxColumnTitle
            // 
            this.DataGridViewTextBoxColumnTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DataGridViewTextBoxColumnTitle.DataPropertyName = "Title";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.DataGridViewTextBoxColumnTitle.DefaultCellStyle = dataGridViewCellStyle1;
            this.DataGridViewTextBoxColumnTitle.HeaderText = "Title";
            this.DataGridViewTextBoxColumnTitle.Name = "DataGridViewTextBoxColumnTitle";
            this.DataGridViewTextBoxColumnTitle.ReadOnly = true;
            // 
            // DataGridViewImageColumnDubLanguage
            // 
            this.DataGridViewImageColumnDubLanguage.DataPropertyName = "DubLanguage";
            this.DataGridViewImageColumnDubLanguage.HeaderText = "Dub";
            this.DataGridViewImageColumnDubLanguage.Name = "DataGridViewImageColumnDubLanguage";
            this.DataGridViewImageColumnDubLanguage.ReadOnly = true;
            this.DataGridViewImageColumnDubLanguage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridViewImageColumnDubLanguage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.DataGridViewImageColumnDubLanguage.Width = 33;
            // 
            // DataGridViewTextBoxColumnTotalDownloads
            // 
            this.DataGridViewTextBoxColumnTotalDownloads.DataPropertyName = "TotalDownloads";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DataGridViewTextBoxColumnTotalDownloads.DefaultCellStyle = dataGridViewCellStyle2;
            this.DataGridViewTextBoxColumnTotalDownloads.HeaderText = "Total Downloads";
            this.DataGridViewTextBoxColumnTotalDownloads.Name = "DataGridViewTextBoxColumnTotalDownloads";
            this.DataGridViewTextBoxColumnTotalDownloads.ReadOnly = true;
            this.DataGridViewTextBoxColumnTotalDownloads.Width = 40;
            // 
            // DataGridViewTextBoxColumnLastUpdate
            // 
            this.DataGridViewTextBoxColumnLastUpdate.DataPropertyName = "LastUpgrade";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DataGridViewTextBoxColumnLastUpdate.DefaultCellStyle = dataGridViewCellStyle3;
            this.DataGridViewTextBoxColumnLastUpdate.HeaderText = "Last Upgrade";
            this.DataGridViewTextBoxColumnLastUpdate.Name = "DataGridViewTextBoxColumnLastUpdate";
            this.DataGridViewTextBoxColumnLastUpdate.ReadOnly = true;
            this.DataGridViewTextBoxColumnLastUpdate.Width = 40;
            // 
            // DataGridViewTextBoxColumnStatus
            // 
            this.DataGridViewTextBoxColumnStatus.DataPropertyName = "Status";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DataGridViewTextBoxColumnStatus.DefaultCellStyle = dataGridViewCellStyle4;
            this.DataGridViewTextBoxColumnStatus.HeaderText = "Status";
            this.DataGridViewTextBoxColumnStatus.Name = "DataGridViewTextBoxColumnStatus";
            this.DataGridViewTextBoxColumnStatus.ReadOnly = true;
            this.DataGridViewTextBoxColumnStatus.Width = 62;
            // 
            // DataGridViewTextBoxColumnEnabled
            // 
            this.DataGridViewTextBoxColumnEnabled.DataPropertyName = "Enabled";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DataGridViewTextBoxColumnEnabled.DefaultCellStyle = dataGridViewCellStyle5;
            this.DataGridViewTextBoxColumnEnabled.HeaderText = "Enabled";
            this.DataGridViewTextBoxColumnEnabled.Name = "DataGridViewTextBoxColumnEnabled";
            this.DataGridViewTextBoxColumnEnabled.ReadOnly = true;
            this.DataGridViewTextBoxColumnEnabled.Width = 60;
            // 
            // contextMenuStripFeed
            // 
            this.contextMenuStripFeed.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAdd,
            this.toolStripMenuItemEdit,
            this.toolStripMenuItemDelete,
            this.toolStripSeparator5,
            this.enableToolStripMenuItem,
            this.disableToolStripMenuItem});
            this.contextMenuStripFeed.Name = "contextMenuStrip1";
            this.contextMenuStripFeed.Size = new System.Drawing.Size(113, 120);
            // 
            // toolStripMenuItemAdd
            // 
            this.toolStripMenuItemAdd.Name = "toolStripMenuItemAdd";
            this.toolStripMenuItemAdd.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItemAdd.Text = "Add";
            this.toolStripMenuItemAdd.Click += new System.EventHandler(this.toolStripMenuItemAdd_Click);
            // 
            // toolStripMenuItemEdit
            // 
            this.toolStripMenuItemEdit.Name = "toolStripMenuItemEdit";
            this.toolStripMenuItemEdit.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItemEdit.Text = "Edit";
            this.toolStripMenuItemEdit.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // toolStripMenuItemDelete
            // 
            this.toolStripMenuItemDelete.Name = "toolStripMenuItemDelete";
            this.toolStripMenuItemDelete.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItemDelete.Text = "Delete";
            this.toolStripMenuItemDelete.Click += new System.EventHandler(this.toolStripMenuItemDelete_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(109, 6);
            // 
            // enableToolStripMenuItem
            // 
            this.enableToolStripMenuItem.Name = "enableToolStripMenuItem";
            this.enableToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.enableToolStripMenuItem.Text = "Enable";
            this.enableToolStripMenuItem.Click += new System.EventHandler(this.enableToolStripMenuItem_Click);
            // 
            // disableToolStripMenuItem
            // 
            this.disableToolStripMenuItem.Name = "disableToolStripMenuItem";
            this.disableToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.disableToolStripMenuItem.Text = "Disable";
            this.disableToolStripMenuItem.Click += new System.EventHandler(this.disableToolStripMenuItem_Click);
            // 
            // labelMaxSimultaneousDownloads
            // 
            this.labelMaxSimultaneousDownloads.AutoSize = true;
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
            this.labelLastDownloadDate.Location = new System.Drawing.Point(106, 289);
            this.labelLastDownloadDate.Name = "labelLastDownloadDate";
            this.labelLastDownloadDate.Size = new System.Drawing.Size(41, 13);
            this.labelLastDownloadDate.TabIndex = 33;
            this.labelLastDownloadDate.Text = "label14";
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
            this.labelFeedUrl.Location = new System.Drawing.Point(106, 276);
            this.labelFeedUrl.Name = "labelFeedUrl";
            this.labelFeedUrl.Size = new System.Drawing.Size(41, 13);
            this.labelFeedUrl.TabIndex = 31;
            this.labelFeedUrl.Text = "label12";
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
            this.labelFeedPauseDownload.Location = new System.Drawing.Point(106, 263);
            this.labelFeedPauseDownload.Name = "labelFeedPauseDownload";
            this.labelFeedPauseDownload.Size = new System.Drawing.Size(41, 13);
            this.labelFeedPauseDownload.TabIndex = 29;
            this.labelFeedPauseDownload.Text = "label10";
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
            this.labelFeedCategory.Location = new System.Drawing.Point(106, 248);
            this.labelFeedCategory.Name = "labelFeedCategory";
            this.labelFeedCategory.Size = new System.Drawing.Size(35, 13);
            this.labelFeedCategory.TabIndex = 27;
            this.labelFeedCategory.Text = "label8";
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
            // listViewFeedFilesList
            // 
            this.listViewFeedFilesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader1,
            this.columnHeader7});
            this.listViewFeedFilesList.ContextMenuStrip = this.contextMenuStripFileFeed;
            this.listViewFeedFilesList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listViewFeedFilesList.Location = new System.Drawing.Point(3, 319);
            this.listViewFeedFilesList.Name = "listViewFeedFilesList";
            this.listViewFeedFilesList.Size = new System.Drawing.Size(642, 119);
            this.listViewFeedFilesList.TabIndex = 24;
            this.listViewFeedFilesList.UseCompatibleStateImageBehavior = false;
            this.listViewFeedFilesList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "File";
            this.columnHeader5.Width = 400;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Date";
            this.columnHeader7.Width = 100;
            // 
            // contextMenuStripFileFeed
            // 
            this.contextMenuStripFileFeed.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem1});
            this.contextMenuStripFileFeed.Name = "contextMenuStrip2";
            this.contextMenuStripFileFeed.Size = new System.Drawing.Size(141, 26);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(140, 22);
            this.deleteToolStripMenuItem1.Text = "Redownload";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem1_Click);
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
            this.listBoxPending.ContextMenuStrip = this.contextMenuStripPending;
            this.listBoxPending.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPending.FormattingEnabled = true;
            this.listBoxPending.Location = new System.Drawing.Point(3, 3);
            this.listBoxPending.Name = "listBoxPending";
            this.listBoxPending.Size = new System.Drawing.Size(642, 435);
            this.listBoxPending.Sorted = true;
            this.listBoxPending.TabIndex = 0;
            // 
            // contextMenuStripPending
            // 
            this.contextMenuStripPending.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.excludeToolStripMenuItem});
            this.contextMenuStripPending.Name = "contextMenuStripPending";
            this.contextMenuStripPending.Size = new System.Drawing.Size(115, 26);
            // 
            // excludeToolStripMenuItem
            // 
            this.excludeToolStripMenuItem.Enabled = false;
            this.excludeToolStripMenuItem.Name = "excludeToolStripMenuItem";
            this.excludeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.excludeToolStripMenuItem.Text = "Exclude";
            this.excludeToolStripMenuItem.Click += new System.EventHandler(this.excludeToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.channelToolStripMenuItem,
            this.logToolStripMenuItem,
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
            this.oPMLExportToolStripMenuItem,
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
            // oPMLExportToolStripMenuItem
            // 
            this.oPMLExportToolStripMenuItem.Name = "oPMLExportToolStripMenuItem";
            this.oPMLExportToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.oPMLExportToolStripMenuItem.Text = "OPML Export";
            this.oPMLExportToolStripMenuItem.Click += new System.EventHandler(this.oPMLExportToolStripMenuItem_Click);
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
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.autoClearToolStripMenuItem,
            this.toolStripSeparator4,
            this.verboseToolStripMenuItem,
            this.openLogFileToolStripMenuItem});
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.logToolStripMenuItem.Text = "Logs";
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // autoClearToolStripMenuItem
            // 
            this.autoClearToolStripMenuItem.Name = "autoClearToolStripMenuItem";
            this.autoClearToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.autoClearToolStripMenuItem.Text = "AutoClear";
            this.autoClearToolStripMenuItem.Click += new System.EventHandler(this.autoClearToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(144, 6);
            // 
            // verboseToolStripMenuItem
            // 
            this.verboseToolStripMenuItem.Name = "verboseToolStripMenuItem";
            this.verboseToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.verboseToolStripMenuItem.Text = "Verbose";
            this.verboseToolStripMenuItem.Click += new System.EventHandler(this.verboseToolStripMenuItem_Click);
            // 
            // openLogFileToolStripMenuItem
            // 
            this.openLogFileToolStripMenuItem.Name = "openLogFileToolStripMenuItem";
            this.openLogFileToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.openLogFileToolStripMenuItem.Text = "Open Log File";
            this.openLogFileToolStripMenuItem.Click += new System.EventHandler(this.openLogFileToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.reportABugToolStripMenuItem,
            this.forumToolStripMenuItem,
            this.versionCheckToolStripMenuItem,
            this.aboutToolStripMenuItem1});
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
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(148, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
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
            // columnHeader1
            // 
            this.columnHeader1.Text = "Pub Date";
            this.columnHeader1.Width = 100;
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
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabLog.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMain)).EndInit();
            this.contextMenuStripFeed.ResumeLayout(false);
            this.contextMenuStripFileFeed.ResumeLayout(false);
            this.tabRecentActivity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRecentActivity)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPending.ResumeLayout(false);
            this.contextMenuStripPending.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       

        #endregion

        private System.Windows.Forms.Timer timerRssCheck;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timer2;
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
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoClearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem globalOptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoStartEMuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem autoCloseEMuleToolStripMenuItem;
        private System.Windows.Forms.Timer timerAutoClose;
        private System.Windows.Forms.ToolStripMenuItem verboseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem testAutoCloseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testAutoStartToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFeed;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAdd;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFileFeed;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openLogFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelCheckToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportABugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem oPMLExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEdit;
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
        private System.Windows.Forms.ListView listViewFeedFilesList;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem enableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem versionCheckToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileNameDataGridColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdateDataGridColumn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripPending;
        private System.Windows.Forms.ToolStripMenuItem excludeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importDataToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.DataGridView dataGridViewMain;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewTextBoxColumnTitle;
        private System.Windows.Forms.DataGridViewImageColumn DataGridViewImageColumnDubLanguage;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewTextBoxColumnTotalDownloads;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewTextBoxColumnLastUpdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewTextBoxColumnStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewTextBoxColumnEnabled;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}

