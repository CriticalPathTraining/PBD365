namespace PowerBiApiExplorer.Forms
{
    partial class Options
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.lblClientId = new System.Windows.Forms.Label();
            this.lblRedirectUri = new System.Windows.Forms.Label();
            this.lblResourceUri = new System.Windows.Forms.Label();
            this.lblAuthoriyUri = new System.Windows.Forms.Label();
            this.lblPowerBiApiUri = new System.Windows.Forms.Label();
            this.txtClientId = new System.Windows.Forms.TextBox();
            this.txtRedirectUri = new System.Windows.Forms.TextBox();
            this.txtResourceUri = new System.Windows.Forms.TextBox();
            this.txtAuthorityUri = new System.Windows.Forms.TextBox();
            this.txtPowerBiApiUri = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.pbxPowerBiApiUri = new System.Windows.Forms.PictureBox();
            this.pbxAuthorityUri = new System.Windows.Forms.PictureBox();
            this.pbxResourceUri = new System.Windows.Forms.PictureBox();
            this.pbxRedirectUri = new System.Windows.Forms.PictureBox();
            this.pbxClientId = new System.Windows.Forms.PictureBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlFormIntroduction = new System.Windows.Forms.Panel();
            this.txtFormIntroduction = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPowerBiApiUri)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxAuthorityUri)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxResourceUri)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxRedirectUri)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxClientId)).BeginInit();
            this.pnlFormIntroduction.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblClientId
            // 
            this.lblClientId.AutoSize = true;
            this.lblClientId.Location = new System.Drawing.Point(20, 92);
            this.lblClientId.Name = "lblClientId";
            this.lblClientId.Size = new System.Drawing.Size(50, 13);
            this.lblClientId.TabIndex = 0;
            this.lblClientId.Text = "Client ID:";
            // 
            // lblRedirectUri
            // 
            this.lblRedirectUri.AutoSize = true;
            this.lblRedirectUri.Location = new System.Drawing.Point(20, 122);
            this.lblRedirectUri.Name = "lblRedirectUri";
            this.lblRedirectUri.Size = new System.Drawing.Size(72, 13);
            this.lblRedirectUri.TabIndex = 2;
            this.lblRedirectUri.Text = "Redirect URI:";
            // 
            // lblResourceUri
            // 
            this.lblResourceUri.AutoSize = true;
            this.lblResourceUri.Location = new System.Drawing.Point(20, 152);
            this.lblResourceUri.Name = "lblResourceUri";
            this.lblResourceUri.Size = new System.Drawing.Size(78, 13);
            this.lblResourceUri.TabIndex = 4;
            this.lblResourceUri.Text = "Resource URI:";
            // 
            // lblAuthoriyUri
            // 
            this.lblAuthoriyUri.AutoSize = true;
            this.lblAuthoriyUri.Location = new System.Drawing.Point(20, 182);
            this.lblAuthoriyUri.Name = "lblAuthoriyUri";
            this.lblAuthoriyUri.Size = new System.Drawing.Size(73, 13);
            this.lblAuthoriyUri.TabIndex = 6;
            this.lblAuthoriyUri.Text = "Authority URI:";
            // 
            // lblPowerBiApiUri
            // 
            this.lblPowerBiApiUri.AutoSize = true;
            this.lblPowerBiApiUri.Location = new System.Drawing.Point(20, 212);
            this.lblPowerBiApiUri.Name = "lblPowerBiApiUri";
            this.lblPowerBiApiUri.Size = new System.Drawing.Size(95, 13);
            this.lblPowerBiApiUri.TabIndex = 8;
            this.lblPowerBiApiUri.Text = "Power BI API URI:";
            // 
            // txtClientId
            // 
            this.txtClientId.Location = new System.Drawing.Point(121, 89);
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Size = new System.Drawing.Size(326, 20);
            this.txtClientId.TabIndex = 1;
            // 
            // txtRedirectUri
            // 
            this.txtRedirectUri.Location = new System.Drawing.Point(121, 119);
            this.txtRedirectUri.Name = "txtRedirectUri";
            this.txtRedirectUri.Size = new System.Drawing.Size(326, 20);
            this.txtRedirectUri.TabIndex = 3;
            // 
            // txtResourceUri
            // 
            this.txtResourceUri.Enabled = false;
            this.txtResourceUri.Location = new System.Drawing.Point(121, 149);
            this.txtResourceUri.Name = "txtResourceUri";
            this.txtResourceUri.Size = new System.Drawing.Size(326, 20);
            this.txtResourceUri.TabIndex = 5;
            // 
            // txtAuthorityUri
            // 
            this.txtAuthorityUri.Enabled = false;
            this.txtAuthorityUri.Location = new System.Drawing.Point(121, 179);
            this.txtAuthorityUri.Name = "txtAuthorityUri";
            this.txtAuthorityUri.Size = new System.Drawing.Size(326, 20);
            this.txtAuthorityUri.TabIndex = 7;
            // 
            // txtPowerBiApiUri
            // 
            this.txtPowerBiApiUri.Enabled = false;
            this.txtPowerBiApiUri.Location = new System.Drawing.Point(121, 209);
            this.txtPowerBiApiUri.Name = "txtPowerBiApiUri";
            this.txtPowerBiApiUri.Size = new System.Drawing.Size(326, 20);
            this.txtPowerBiApiUri.TabIndex = 9;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(398, 256);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pbxPowerBiApiUri
            // 
            this.pbxPowerBiApiUri.Cursor = System.Windows.Forms.Cursors.Help;
            this.pbxPowerBiApiUri.Image = global::PowerBiApiExplorer.Properties.Resources.helpIcon;
            this.pbxPowerBiApiUri.Location = new System.Drawing.Point(453, 209);
            this.pbxPowerBiApiUri.Name = "pbxPowerBiApiUri";
            this.pbxPowerBiApiUri.Size = new System.Drawing.Size(20, 20);
            this.pbxPowerBiApiUri.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxPowerBiApiUri.TabIndex = 14;
            this.pbxPowerBiApiUri.TabStop = false;
            // 
            // pbxAuthorityUri
            // 
            this.pbxAuthorityUri.Cursor = System.Windows.Forms.Cursors.Help;
            this.pbxAuthorityUri.Image = global::PowerBiApiExplorer.Properties.Resources.helpIcon;
            this.pbxAuthorityUri.Location = new System.Drawing.Point(453, 179);
            this.pbxAuthorityUri.Name = "pbxAuthorityUri";
            this.pbxAuthorityUri.Size = new System.Drawing.Size(20, 20);
            this.pbxAuthorityUri.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxAuthorityUri.TabIndex = 13;
            this.pbxAuthorityUri.TabStop = false;
            // 
            // pbxResourceUri
            // 
            this.pbxResourceUri.Cursor = System.Windows.Forms.Cursors.Help;
            this.pbxResourceUri.Image = global::PowerBiApiExplorer.Properties.Resources.helpIcon;
            this.pbxResourceUri.Location = new System.Drawing.Point(453, 149);
            this.pbxResourceUri.Name = "pbxResourceUri";
            this.pbxResourceUri.Size = new System.Drawing.Size(20, 20);
            this.pbxResourceUri.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxResourceUri.TabIndex = 12;
            this.pbxResourceUri.TabStop = false;
            // 
            // pbxRedirectUri
            // 
            this.pbxRedirectUri.Cursor = System.Windows.Forms.Cursors.Help;
            this.pbxRedirectUri.Image = global::PowerBiApiExplorer.Properties.Resources.helpIcon;
            this.pbxRedirectUri.Location = new System.Drawing.Point(453, 119);
            this.pbxRedirectUri.Name = "pbxRedirectUri";
            this.pbxRedirectUri.Size = new System.Drawing.Size(20, 20);
            this.pbxRedirectUri.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxRedirectUri.TabIndex = 11;
            this.pbxRedirectUri.TabStop = false;
            // 
            // pbxClientId
            // 
            this.pbxClientId.Cursor = System.Windows.Forms.Cursors.Help;
            this.pbxClientId.Image = global::PowerBiApiExplorer.Properties.Resources.helpIcon;
            this.pbxClientId.Location = new System.Drawing.Point(453, 89);
            this.pbxClientId.Name = "pbxClientId";
            this.pbxClientId.Size = new System.Drawing.Size(20, 20);
            this.pbxClientId.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxClientId.TabIndex = 10;
            this.pbxClientId.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(317, 256);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pnlFormIntroduction
            // 
            this.pnlFormIntroduction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFormIntroduction.BackColor = System.Drawing.Color.Gainsboro;
            this.pnlFormIntroduction.Controls.Add(this.txtFormIntroduction);
            this.pnlFormIntroduction.Location = new System.Drawing.Point(0, 0);
            this.pnlFormIntroduction.Name = "pnlFormIntroduction";
            this.pnlFormIntroduction.Size = new System.Drawing.Size(492, 70);
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
            this.txtFormIntroduction.Size = new System.Drawing.Size(456, 50);
            this.txtFormIntroduction.TabIndex = 0;
            this.txtFormIntroduction.Text = resources.GetString("txtFormIntroduction.Text");
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(492, 298);
            this.ControlBox = false;
            this.Controls.Add(this.pnlFormIntroduction);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pbxPowerBiApiUri);
            this.Controls.Add(this.pbxAuthorityUri);
            this.Controls.Add(this.pbxResourceUri);
            this.Controls.Add(this.pbxRedirectUri);
            this.Controls.Add(this.pbxClientId);
            this.Controls.Add(this.txtPowerBiApiUri);
            this.Controls.Add(this.txtAuthorityUri);
            this.Controls.Add(this.txtResourceUri);
            this.Controls.Add(this.txtRedirectUri);
            this.Controls.Add(this.txtClientId);
            this.Controls.Add(this.lblPowerBiApiUri);
            this.Controls.Add(this.lblAuthoriyUri);
            this.Controls.Add(this.lblResourceUri);
            this.Controls.Add(this.lblRedirectUri);
            this.Controls.Add(this.lblClientId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Options";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.Activated += new System.EventHandler(this.Options_Activated);
            this.Load += new System.EventHandler(this.Options_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxPowerBiApiUri)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxAuthorityUri)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxResourceUri)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxRedirectUri)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxClientId)).EndInit();
            this.pnlFormIntroduction.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblClientId;
        private System.Windows.Forms.Label lblRedirectUri;
        private System.Windows.Forms.Label lblResourceUri;
        private System.Windows.Forms.Label lblAuthoriyUri;
        private System.Windows.Forms.Label lblPowerBiApiUri;
        private System.Windows.Forms.PictureBox pbxClientId;
        private System.Windows.Forms.PictureBox pbxRedirectUri;
        private System.Windows.Forms.PictureBox pbxResourceUri;
        private System.Windows.Forms.PictureBox pbxAuthorityUri;
        private System.Windows.Forms.PictureBox pbxPowerBiApiUri;
        private System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.TextBox txtClientId;
        internal System.Windows.Forms.TextBox txtRedirectUri;
        internal System.Windows.Forms.TextBox txtResourceUri;
        internal System.Windows.Forms.TextBox txtAuthorityUri;
        internal System.Windows.Forms.TextBox txtPowerBiApiUri;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel pnlFormIntroduction;
        private System.Windows.Forms.RichTextBox txtFormIntroduction;
    }
}