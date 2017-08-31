namespace PowerBiApiExplorer.Forms
{
    partial class PushData_EventStream
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
            this.lblEventSource = new System.Windows.Forms.Label();
            this.cboEventSource = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslTimer = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslBatchesSent = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslRowsSent = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblBatchSize = new System.Windows.Forms.Label();
            this.nudBatchSize = new System.Windows.Forms.NumericUpDown();
            this.lblDelayBetweenRequests = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lblBatchSizeUnit = new System.Windows.Forms.Label();
            this.nudDelayBetweenRequests = new System.Windows.Forms.NumericUpDown();
            this.lblDelayBetweenRequestsUnit = new System.Windows.Forms.Label();
            this.txtFormIntroduction = new System.Windows.Forms.RichTextBox();
            this.pnlFormIntroduction = new System.Windows.Forms.Panel();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBatchSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelayBetweenRequests)).BeginInit();
            this.pnlFormIntroduction.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEventSource
            // 
            this.lblEventSource.AutoSize = true;
            this.lblEventSource.Location = new System.Drawing.Point(28, 92);
            this.lblEventSource.Name = "lblEventSource";
            this.lblEventSource.Size = new System.Drawing.Size(82, 15);
            this.lblEventSource.TabIndex = 0;
            this.lblEventSource.Text = "&Event Source:";
            // 
            // cboEventSource
            // 
            this.cboEventSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEventSource.FormattingEnabled = true;
            this.cboEventSource.Location = new System.Drawing.Point(31, 110);
            this.cboEventSource.Name = "cboEventSource";
            this.cboEventSource.Size = new System.Drawing.Size(447, 21);
            this.cboEventSource.TabIndex = 1;
            this.cboEventSource.SelectedIndexChanged += new System.EventHandler(this.cboEventSource_SelectedIndexChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslStatus,
            this.tslTimer,
            this.tslBatchesSent,
            this.tslRowsSent});
            this.statusStrip1.Location = new System.Drawing.Point(0, 262);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(514, 24);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tslStatus
            // 
            this.tslStatus.AutoSize = false;
            this.tslStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.tslStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tslStatus.Name = "tslStatus";
            this.tslStatus.Size = new System.Drawing.Size(60, 19);
            this.tslStatus.Text = "Stopped";
            // 
            // tslTimer
            // 
            this.tslTimer.AutoSize = false;
            this.tslTimer.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.tslTimer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tslTimer.Name = "tslTimer";
            this.tslTimer.Size = new System.Drawing.Size(60, 19);
            this.tslTimer.Text = "00:00:00";
            // 
            // tslBatchesSent
            // 
            this.tslBatchesSent.AutoSize = false;
            this.tslBatchesSent.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.tslBatchesSent.Name = "tslBatchesSent";
            this.tslBatchesSent.Size = new System.Drawing.Size(80, 19);
            this.tslBatchesSent.Text = "Batches: 0";
            this.tslBatchesSent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tslRowsSent
            // 
            this.tslRowsSent.AutoSize = false;
            this.tslRowsSent.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tslRowsSent.Name = "tslRowsSent";
            this.tslRowsSent.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.tslRowsSent.Size = new System.Drawing.Size(80, 19);
            this.tslRowsSent.Text = "Rows: 0";
            this.tslRowsSent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(241, 220);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "&Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(322, 220);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "Sto&p";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(403, 220);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblBatchSize
            // 
            this.lblBatchSize.AutoSize = true;
            this.lblBatchSize.Location = new System.Drawing.Point(28, 147);
            this.lblBatchSize.Name = "lblBatchSize";
            this.lblBatchSize.Size = new System.Drawing.Size(68, 15);
            this.lblBatchSize.TabIndex = 2;
            this.lblBatchSize.Text = "&Batch Size:";
            // 
            // nudBatchSize
            // 
            this.nudBatchSize.Enabled = false;
            this.nudBatchSize.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudBatchSize.Location = new System.Drawing.Point(179, 145);
            this.nudBatchSize.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudBatchSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBatchSize.Name = "nudBatchSize";
            this.nudBatchSize.Size = new System.Drawing.Size(70, 20);
            this.nudBatchSize.TabIndex = 3;
            this.nudBatchSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudBatchSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lblDelayBetweenRequests
            // 
            this.lblDelayBetweenRequests.AutoSize = true;
            this.lblDelayBetweenRequests.Location = new System.Drawing.Point(28, 176);
            this.lblDelayBetweenRequests.Name = "lblDelayBetweenRequests";
            this.lblDelayBetweenRequests.Size = new System.Drawing.Size(147, 15);
            this.lblDelayBetweenRequests.TabIndex = 5;
            this.lblDelayBetweenRequests.Text = "&Delay Between Requests:";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // lblBatchSizeUnit
            // 
            this.lblBatchSizeUnit.AutoSize = true;
            this.lblBatchSizeUnit.Location = new System.Drawing.Point(255, 147);
            this.lblBatchSizeUnit.Name = "lblBatchSizeUnit";
            this.lblBatchSizeUnit.Size = new System.Drawing.Size(50, 15);
            this.lblBatchSizeUnit.TabIndex = 4;
            this.lblBatchSizeUnit.Text = "event(s)";
            // 
            // nudDelayBetweenRequests
            // 
            this.nudDelayBetweenRequests.Enabled = false;
            this.nudDelayBetweenRequests.Location = new System.Drawing.Point(179, 174);
            this.nudDelayBetweenRequests.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudDelayBetweenRequests.Name = "nudDelayBetweenRequests";
            this.nudDelayBetweenRequests.Size = new System.Drawing.Size(70, 20);
            this.nudDelayBetweenRequests.TabIndex = 6;
            this.nudDelayBetweenRequests.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDelayBetweenRequestsUnit
            // 
            this.lblDelayBetweenRequestsUnit.AutoSize = true;
            this.lblDelayBetweenRequestsUnit.Location = new System.Drawing.Point(255, 176);
            this.lblDelayBetweenRequestsUnit.Name = "lblDelayBetweenRequestsUnit";
            this.lblDelayBetweenRequestsUnit.Size = new System.Drawing.Size(61, 15);
            this.lblDelayBetweenRequestsUnit.TabIndex = 7;
            this.lblDelayBetweenRequestsUnit.Text = "second(s)";
            // 
            // txtFormIntroduction
            // 
            this.txtFormIntroduction.BackColor = System.Drawing.Color.Gainsboro;
            this.txtFormIntroduction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFormIntroduction.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFormIntroduction.ForeColor = System.Drawing.Color.Black;
            this.txtFormIntroduction.Location = new System.Drawing.Point(31, 10);
            this.txtFormIntroduction.Margin = new System.Windows.Forms.Padding(5);
            this.txtFormIntroduction.Name = "txtFormIntroduction";
            this.txtFormIntroduction.ReadOnly = true;
            this.txtFormIntroduction.Size = new System.Drawing.Size(447, 50);
            this.txtFormIntroduction.TabIndex = 12;
            this.txtFormIntroduction.Text = "Use this window to add a stream of rows to an existing table, sourced from a sele" +
    "cted stored procedure.";
            // 
            // pnlFormIntroduction
            // 
            this.pnlFormIntroduction.BackColor = System.Drawing.Color.Gainsboro;
            this.pnlFormIntroduction.Controls.Add(this.txtFormIntroduction);
            this.pnlFormIntroduction.Location = new System.Drawing.Point(0, 0);
            this.pnlFormIntroduction.Name = "pnlFormIntroduction";
            this.pnlFormIntroduction.Size = new System.Drawing.Size(516, 70);
            this.pnlFormIntroduction.TabIndex = 13;
            // 
            // PushData_EventStream
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(514, 286);
            this.ControlBox = false;
            this.Controls.Add(this.pnlFormIntroduction);
            this.Controls.Add(this.lblDelayBetweenRequestsUnit);
            this.Controls.Add(this.nudDelayBetweenRequests);
            this.Controls.Add(this.lblBatchSizeUnit);
            this.Controls.Add(this.lblDelayBetweenRequests);
            this.Controls.Add(this.nudBatchSize);
            this.Controls.Add(this.lblBatchSize);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cboEventSource);
            this.Controls.Add(this.lblEventSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PushData_EventStream";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Push Data - Event Stream";
            this.Load += new System.EventHandler(this.PushData_EventStream_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBatchSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelayBetweenRequests)).EndInit();
            this.pnlFormIntroduction.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEventSource;
        private System.Windows.Forms.ComboBox cboEventSource;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslStatus;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolStripStatusLabel tslTimer;
        private System.Windows.Forms.ToolStripStatusLabel tslBatchesSent;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblBatchSize;
        private System.Windows.Forms.NumericUpDown nudBatchSize;
        private System.Windows.Forms.Label lblDelayBetweenRequests;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label lblBatchSizeUnit;
        private System.Windows.Forms.NumericUpDown nudDelayBetweenRequests;
        private System.Windows.Forms.Label lblDelayBetweenRequestsUnit;
        private System.Windows.Forms.ToolStripStatusLabel tslRowsSent;
        private System.Windows.Forms.RichTextBox txtFormIntroduction;
        private System.Windows.Forms.Panel pnlFormIntroduction;
    }
}