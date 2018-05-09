namespace PowerBiApiExplorer.Forms
{
    partial class CreateDataset
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboDefaultRetentionPolicy = new System.Windows.Forms.ComboBox();
            this.lblJsonContent = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtJsonContent = new System.Windows.Forms.RichTextBox();
            this.gbxCreateMethod = new System.Windows.Forms.GroupBox();
            this.cboEventSource = new System.Windows.Forms.ComboBox();
            this.rbtnEventSource = new System.Windows.Forms.RadioButton();
            this.rbtnDirectEntry = new System.Windows.Forms.RadioButton();
            this.pbxDefaultRetentionPolicy = new System.Windows.Forms.PictureBox();
            this.pnlFormIntroduction = new System.Windows.Forms.Panel();
            this.txtFormIntroduction = new System.Windows.Forms.RichTextBox();
            this.gbxCreateMethod.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxDefaultRetentionPolicy)).BeginInit();
            this.pnlFormIntroduction.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 507);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Default Retention Policy:";
            // 
            // cboDefaultRetentionPolicy
            // 
            this.cboDefaultRetentionPolicy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboDefaultRetentionPolicy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDefaultRetentionPolicy.FormattingEnabled = true;
            this.cboDefaultRetentionPolicy.Items.AddRange(new object[] {
            "None",
            "basicFIFO"});
            this.cboDefaultRetentionPolicy.Location = new System.Drawing.Point(144, 504);
            this.cboDefaultRetentionPolicy.Name = "cboDefaultRetentionPolicy";
            this.cboDefaultRetentionPolicy.Size = new System.Drawing.Size(100, 21);
            this.cboDefaultRetentionPolicy.TabIndex = 4;
            // 
            // lblJsonContent
            // 
            this.lblJsonContent.AutoSize = true;
            this.lblJsonContent.Location = new System.Drawing.Point(20, 214);
            this.lblJsonContent.Name = "lblJsonContent";
            this.lblJsonContent.Size = new System.Drawing.Size(126, 13);
            this.lblJsonContent.TabIndex = 1;
            this.lblJsonContent.Text = "Dataset Schema (JSON):";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(390, 547);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(309, 547);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtJsonContent
            // 
            this.txtJsonContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtJsonContent.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJsonContent.Location = new System.Drawing.Point(21, 233);
            this.txtJsonContent.Name = "txtJsonContent";
            this.txtJsonContent.Size = new System.Drawing.Size(442, 260);
            this.txtJsonContent.TabIndex = 2;
            this.txtJsonContent.Text = "";
            this.txtJsonContent.TextChanged += new System.EventHandler(this.txtJsonContent_TextChanged);
            // 
            // gbxCreateMethod
            // 
            this.gbxCreateMethod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxCreateMethod.Controls.Add(this.cboEventSource);
            this.gbxCreateMethod.Controls.Add(this.rbtnEventSource);
            this.gbxCreateMethod.Controls.Add(this.rbtnDirectEntry);
            this.gbxCreateMethod.Location = new System.Drawing.Point(21, 92);
            this.gbxCreateMethod.Name = "gbxCreateMethod";
            this.gbxCreateMethod.Size = new System.Drawing.Size(442, 110);
            this.gbxCreateMethod.TabIndex = 0;
            this.gbxCreateMethod.TabStop = false;
            this.gbxCreateMethod.Text = "Method";
            // 
            // cboEventSource
            // 
            this.cboEventSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboEventSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEventSource.Enabled = false;
            this.cboEventSource.FormattingEnabled = true;
            this.cboEventSource.Location = new System.Drawing.Point(35, 72);
            this.cboEventSource.Name = "cboEventSource";
            this.cboEventSource.Size = new System.Drawing.Size(381, 21);
            this.cboEventSource.TabIndex = 2;
            this.cboEventSource.SelectedIndexChanged += new System.EventHandler(this.cboEventSource_SelectedIndexChanged);
            // 
            // rbtnEventSource
            // 
            this.rbtnEventSource.AutoSize = true;
            this.rbtnEventSource.Location = new System.Drawing.Point(16, 48);
            this.rbtnEventSource.Name = "rbtnEventSource";
            this.rbtnEventSource.Size = new System.Drawing.Size(93, 17);
            this.rbtnEventSource.TabIndex = 1;
            this.rbtnEventSource.TabStop = true;
            this.rbtnEventSource.Text = "&Event Source:";
            this.rbtnEventSource.UseVisualStyleBackColor = true;
            // 
            // rbtnDirectEntry
            // 
            this.rbtnDirectEntry.AutoSize = true;
            this.rbtnDirectEntry.Checked = true;
            this.rbtnDirectEntry.Location = new System.Drawing.Point(16, 25);
            this.rbtnDirectEntry.Name = "rbtnDirectEntry";
            this.rbtnDirectEntry.Size = new System.Drawing.Size(80, 17);
            this.rbtnDirectEntry.TabIndex = 0;
            this.rbtnDirectEntry.TabStop = true;
            this.rbtnDirectEntry.Text = "&Direct Entry";
            this.rbtnDirectEntry.UseVisualStyleBackColor = true;
            this.rbtnDirectEntry.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // pbxDefaultRetentionPolicy
            // 
            this.pbxDefaultRetentionPolicy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbxDefaultRetentionPolicy.Cursor = System.Windows.Forms.Cursors.Help;
            this.pbxDefaultRetentionPolicy.Image = global::PowerBiApiExplorer.Properties.Resources.helpIcon;
            this.pbxDefaultRetentionPolicy.Location = new System.Drawing.Point(250, 505);
            this.pbxDefaultRetentionPolicy.Name = "pbxDefaultRetentionPolicy";
            this.pbxDefaultRetentionPolicy.Size = new System.Drawing.Size(20, 20);
            this.pbxDefaultRetentionPolicy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxDefaultRetentionPolicy.TabIndex = 11;
            this.pbxDefaultRetentionPolicy.TabStop = false;
            // 
            // pnlFormIntroduction
            // 
            this.pnlFormIntroduction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFormIntroduction.BackColor = System.Drawing.Color.Gainsboro;
            this.pnlFormIntroduction.Controls.Add(this.txtFormIntroduction);
            this.pnlFormIntroduction.Location = new System.Drawing.Point(0, 0);
            this.pnlFormIntroduction.Name = "pnlFormIntroduction";
            this.pnlFormIntroduction.Size = new System.Drawing.Size(486, 70);
            this.pnlFormIntroduction.TabIndex = 14;
            // 
            // txtFormIntroduction
            // 
            this.txtFormIntroduction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFormIntroduction.BackColor = System.Drawing.Color.Gainsboro;
            this.txtFormIntroduction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFormIntroduction.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFormIntroduction.ForeColor = System.Drawing.Color.Black;
            this.txtFormIntroduction.Location = new System.Drawing.Point(23, 10);
            this.txtFormIntroduction.Margin = new System.Windows.Forms.Padding(5);
            this.txtFormIntroduction.Name = "txtFormIntroduction";
            this.txtFormIntroduction.ReadOnly = true;
            this.txtFormIntroduction.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtFormIntroduction.Size = new System.Drawing.Size(440, 50);
            this.txtFormIntroduction.TabIndex = 12;
            this.txtFormIntroduction.Text = "Use this window to create a dataset based on a schema definition entered directly" +
    ", or derived from a selected stored procedure. The Default Retention Policy prop" +
    "erty may be set to \"basic FIFO\".";
            // 
            // CreateDataset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(484, 586);
            this.ControlBox = false;
            this.Controls.Add(this.pnlFormIntroduction);
            this.Controls.Add(this.pbxDefaultRetentionPolicy);
            this.Controls.Add(this.gbxCreateMethod);
            this.Controls.Add(this.txtJsonContent);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblJsonContent);
            this.Controls.Add(this.cboDefaultRetentionPolicy);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(500, 625);
            this.Name = "CreateDataset";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Dataset";
            this.Load += new System.EventHandler(this.CreateDataset_Load);
            this.gbxCreateMethod.ResumeLayout(false);
            this.gbxCreateMethod.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxDefaultRetentionPolicy)).EndInit();
            this.pnlFormIntroduction.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboDefaultRetentionPolicy;
        private System.Windows.Forms.Label lblJsonContent;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RichTextBox txtJsonContent;
        private System.Windows.Forms.GroupBox gbxCreateMethod;
        private System.Windows.Forms.ComboBox cboEventSource;
        private System.Windows.Forms.RadioButton rbtnEventSource;
        private System.Windows.Forms.RadioButton rbtnDirectEntry;
        private System.Windows.Forms.PictureBox pbxDefaultRetentionPolicy;
        private System.Windows.Forms.Panel pnlFormIntroduction;
        private System.Windows.Forms.RichTextBox txtFormIntroduction;
    }
}