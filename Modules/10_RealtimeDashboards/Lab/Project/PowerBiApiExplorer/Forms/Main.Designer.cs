namespace PowerBiApiExplorer.Forms
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.applicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClearConsole = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.cmsDatasets = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuCreateDataset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmnuGetDatasets = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTables = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuGetTables = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuUpdateSchema = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmnuPushData = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuPushData_JSON = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuPushData_EventStream = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmnuClearAllRows = new System.Windows.Forms.ToolStripMenuItem();
            this.tslConnection = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.cmsService = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsGroups = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuGetGroups = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDashboards = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuGetDashboards = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuGetTiles = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuCopyEmbedUrl = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.cmsDatasets.SuspendLayout();
            this.cmsTables.SuspendLayout();
            this.cmsTable.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.cmsService.SuspendLayout();
            this.cmsGroups.SuspendLayout();
            this.cmsDashboards.SuspendLayout();
            this.cmsTiles.SuspendLayout();
            this.cmsTile.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applicationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 28);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // applicationToolStripMenuItem
            // 
            this.applicationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuConnect,
            this.toolStripSeparator1,
            this.mnuOptions,
            this.mnuAbout,
            this.mnuClearConsole,
            this.toolStripSeparator2,
            this.mnuExit});
            this.applicationToolStripMenuItem.Name = "applicationToolStripMenuItem";
            this.applicationToolStripMenuItem.Size = new System.Drawing.Size(98, 24);
            this.applicationToolStripMenuItem.Text = "&Application";
            // 
            // mnuConnect
            // 
            this.mnuConnect.Name = "mnuConnect";
            this.mnuConnect.Size = new System.Drawing.Size(175, 26);
            this.mnuConnect.Text = "&Connect...";
            this.mnuConnect.Click += new System.EventHandler(this.mnuConnect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(172, 6);
            // 
            // mnuOptions
            // 
            this.mnuOptions.Name = "mnuOptions";
            this.mnuOptions.Size = new System.Drawing.Size(175, 26);
            this.mnuOptions.Text = "&Options...";
            this.mnuOptions.Click += new System.EventHandler(this.mnuOptions_Click);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(175, 26);
            this.mnuAbout.Text = "&About...";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // mnuClearConsole
            // 
            this.mnuClearConsole.Name = "mnuClearConsole";
            this.mnuClearConsole.Size = new System.Drawing.Size(175, 26);
            this.mnuClearConsole.Text = "&Clear Console";
            this.mnuClearConsole.Click += new System.EventHandler(this.mnuClearConsole_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(172, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(175, 26);
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "power_bi_logo_black.gif");
            this.imageList1.Images.SetKeyName(1, "folder_16x16.gif");
            this.imageList1.Images.SetKeyName(2, "dataset_16x16.gif");
            this.imageList1.Images.SetKeyName(3, "table_16x16.gif");
            this.imageList1.Images.SetKeyName(4, "group_16x16.gif");
            this.imageList1.Images.SetKeyName(5, "individual_16x16.gif");
            this.imageList1.Images.SetKeyName(6, "dashboard_16x16.gif");
            this.imageList1.Images.SetKeyName(7, "tile_16x16.gif");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtConsole);
            this.splitContainer1.Size = new System.Drawing.Size(984, 511);
            this.splitContainer1.SplitterDistance = 220;
            this.splitContainer1.TabIndex = 7;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.ShowLines = false;
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(220, 511);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // txtConsole
            // 
            this.txtConsole.BackColor = System.Drawing.Color.Black;
            this.txtConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConsole.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConsole.ForeColor = System.Drawing.Color.White;
            this.txtConsole.Location = new System.Drawing.Point(0, 0);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConsole.Size = new System.Drawing.Size(760, 511);
            this.txtConsole.TabIndex = 5;
            // 
            // cmsDatasets
            // 
            this.cmsDatasets.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsDatasets.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuCreateDataset,
            this.toolStripMenuItem3,
            this.cmnuGetDatasets});
            this.cmsDatasets.Name = "cmsDatasets";
            this.cmsDatasets.Size = new System.Drawing.Size(192, 62);
            // 
            // cmnuCreateDataset
            // 
            this.cmnuCreateDataset.Name = "cmnuCreateDataset";
            this.cmnuCreateDataset.Size = new System.Drawing.Size(191, 26);
            this.cmnuCreateDataset.Text = "&Create Dataset...";
            this.cmnuCreateDataset.Click += new System.EventHandler(this.cmnuCreateDataset_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(188, 6);
            // 
            // cmnuGetDatasets
            // 
            this.cmnuGetDatasets.Name = "cmnuGetDatasets";
            this.cmnuGetDatasets.Size = new System.Drawing.Size(191, 26);
            this.cmnuGetDatasets.Text = "Get &Datasets";
            this.cmnuGetDatasets.Click += new System.EventHandler(this.cmnuGetDatasets_Click);
            // 
            // cmsTables
            // 
            this.cmsTables.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsTables.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuGetTables});
            this.cmsTables.Name = "cmsTables";
            this.cmsTables.Size = new System.Drawing.Size(155, 30);
            // 
            // cmnuGetTables
            // 
            this.cmnuGetTables.Name = "cmnuGetTables";
            this.cmnuGetTables.Size = new System.Drawing.Size(154, 26);
            this.cmnuGetTables.Text = "Get &Tables";
            this.cmnuGetTables.Click += new System.EventHandler(this.cmnuGetTables_Click);
            // 
            // cmsTable
            // 
            this.cmsTable.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuUpdateSchema,
            this.toolStripMenuItem1,
            this.cmnuPushData,
            this.toolStripMenuItem2,
            this.cmnuClearAllRows});
            this.cmsTable.Name = "cmsTable";
            this.cmsTable.Size = new System.Drawing.Size(199, 94);
            // 
            // cmnuUpdateSchema
            // 
            this.cmnuUpdateSchema.Name = "cmnuUpdateSchema";
            this.cmnuUpdateSchema.Size = new System.Drawing.Size(198, 26);
            this.cmnuUpdateSchema.Text = "&Update Schema...";
            this.cmnuUpdateSchema.Click += new System.EventHandler(this.cmnuUpdateSchema_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(195, 6);
            // 
            // cmnuPushData
            // 
            this.cmnuPushData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuPushData_JSON,
            this.cmnuPushData_EventStream});
            this.cmnuPushData.Name = "cmnuPushData";
            this.cmnuPushData.Size = new System.Drawing.Size(198, 26);
            this.cmnuPushData.Text = "Push &Data";
            // 
            // cmnuPushData_JSON
            // 
            this.cmnuPushData_JSON.Name = "cmnuPushData_JSON";
            this.cmnuPushData_JSON.Size = new System.Drawing.Size(201, 26);
            this.cmnuPushData_JSON.Text = "&JSON Document...";
            this.cmnuPushData_JSON.Click += new System.EventHandler(this.cmnuPushData_JSON_Click);
            // 
            // cmnuPushData_EventStream
            // 
            this.cmnuPushData_EventStream.Name = "cmnuPushData_EventStream";
            this.cmnuPushData_EventStream.Size = new System.Drawing.Size(201, 26);
            this.cmnuPushData_EventStream.Text = "Event &Stream...";
            this.cmnuPushData_EventStream.Click += new System.EventHandler(this.cmnuPushData_EventStream_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(195, 6);
            // 
            // cmnuClearAllRows
            // 
            this.cmnuClearAllRows.Name = "cmnuClearAllRows";
            this.cmnuClearAllRows.Size = new System.Drawing.Size(198, 26);
            this.cmnuClearAllRows.Text = "&Clear All Rows";
            this.cmnuClearAllRows.Click += new System.EventHandler(this.cmnuClearAllRows_Click);
            // 
            // tslConnection
            // 
            this.tslConnection.AutoSize = false;
            this.tslConnection.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.tslConnection.Name = "tslConnection";
            this.tslConnection.Size = new System.Drawing.Size(110, 17);
            this.tslConnection.Text = "Not Connected";
            this.tslConnection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslConnection});
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(984, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // cmsService
            // 
            this.cmsService.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsService.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuDisconnect});
            this.cmsService.Name = "cmsService";
            this.cmsService.Size = new System.Drawing.Size(158, 30);
            // 
            // cmnuDisconnect
            // 
            this.cmnuDisconnect.Name = "cmnuDisconnect";
            this.cmnuDisconnect.Size = new System.Drawing.Size(157, 26);
            this.cmnuDisconnect.Text = "&Disconnect";
            this.cmnuDisconnect.Click += new System.EventHandler(this.cmnuDisconnect_Click);
            // 
            // cmsGroups
            // 
            this.cmsGroups.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsGroups.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuGetGroups});
            this.cmsGroups.Name = "cmsDatasets";
            this.cmsGroups.Size = new System.Drawing.Size(223, 58);
            // 
            // cmnuGetGroups
            // 
            this.cmnuGetGroups.Name = "cmnuGetGroups";
            this.cmnuGetGroups.Size = new System.Drawing.Size(222, 26);
            this.cmnuGetGroups.Text = "Get &App Workspaces";
            this.cmnuGetGroups.Click += new System.EventHandler(this.cmnuGetGroups_Click);
            // 
            // cmsDashboards
            // 
            this.cmsDashboards.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsDashboards.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuGetDashboards});
            this.cmsDashboards.Name = "cmsDatasets";
            this.cmsDashboards.Size = new System.Drawing.Size(191, 30);
            // 
            // cmnuGetDashboards
            // 
            this.cmnuGetDashboards.Name = "cmnuGetDashboards";
            this.cmnuGetDashboards.Size = new System.Drawing.Size(190, 26);
            this.cmnuGetDashboards.Text = "Get &Dashboards";
            this.cmnuGetDashboards.Click += new System.EventHandler(this.cmnuGetDashboards_Click);
            // 
            // cmsTiles
            // 
            this.cmsTiles.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsTiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuGetTiles});
            this.cmsTiles.Name = "cmsTables";
            this.cmsTiles.Size = new System.Drawing.Size(142, 30);
            // 
            // cmnuGetTiles
            // 
            this.cmnuGetTiles.Name = "cmnuGetTiles";
            this.cmnuGetTiles.Size = new System.Drawing.Size(141, 26);
            this.cmnuGetTiles.Text = "Get &Tiles";
            this.cmnuGetTiles.Click += new System.EventHandler(this.cmnuGetTiles_Click);
            // 
            // cmnuCopyEmbedUrl
            // 
            this.cmnuCopyEmbedUrl.Name = "cmnuCopyEmbedUrl";
            this.cmnuCopyEmbedUrl.Size = new System.Drawing.Size(199, 26);
            this.cmnuCopyEmbedUrl.Text = "&Copy Embed URL";
            this.cmnuCopyEmbedUrl.Click += new System.EventHandler(this.cmnuCopyEmbedUrl_Click);
            // 
            // cmsTile
            // 
            this.cmsTile.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsTile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuCopyEmbedUrl});
            this.cmsTile.Name = "cmsTable";
            this.cmsTile.Size = new System.Drawing.Size(200, 30);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Power BI REST API Explorer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.cmsDatasets.ResumeLayout(false);
            this.cmsTables.ResumeLayout(false);
            this.cmsTable.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.cmsService.ResumeLayout(false);
            this.cmsGroups.ResumeLayout(false);
            this.cmsDashboards.ResumeLayout(false);
            this.cmsTiles.ResumeLayout(false);
            this.cmsTile.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem applicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuConnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuOptions;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem mnuClearConsole;
        private System.Windows.Forms.ContextMenuStrip cmsDatasets;
        private System.Windows.Forms.ToolStripMenuItem cmnuCreateDataset;
        private System.Windows.Forms.ToolStripMenuItem cmnuGetDatasets;
        private System.Windows.Forms.ToolStripMenuItem cmnuGetTables;
        private System.Windows.Forms.ContextMenuStrip cmsTable;
        private System.Windows.Forms.ToolStripMenuItem cmnuUpdateSchema;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cmnuPushData;
        private System.Windows.Forms.ToolStripMenuItem cmnuPushData_JSON;
        private System.Windows.Forms.ToolStripMenuItem cmnuPushData_EventStream;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem cmnuClearAllRows;
        internal System.Windows.Forms.TreeView treeView1;
        internal System.Windows.Forms.ContextMenuStrip cmsTables;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripStatusLabel tslConnection;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ContextMenuStrip cmsService;
        private System.Windows.Forms.ToolStripMenuItem cmnuDisconnect;
        internal System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.ContextMenuStrip cmsGroups;
        private System.Windows.Forms.ToolStripMenuItem cmnuGetGroups;
        private System.Windows.Forms.ContextMenuStrip cmsDashboards;
        private System.Windows.Forms.ToolStripMenuItem cmnuGetDashboards;
        internal System.Windows.Forms.ContextMenuStrip cmsTiles;
        private System.Windows.Forms.ToolStripMenuItem cmnuGetTiles;
        private System.Windows.Forms.ToolStripMenuItem cmnuCopyEmbedUrl;
        private System.Windows.Forms.ContextMenuStrip cmsTile;
    }
}

