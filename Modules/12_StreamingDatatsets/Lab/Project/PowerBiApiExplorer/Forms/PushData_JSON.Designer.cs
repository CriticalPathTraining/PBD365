namespace PowerBiApiExplorer.Forms
{
    partial class PushData_JSON
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
            this.lblJsonContent = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtJsonContent = new System.Windows.Forms.RichTextBox();
            this.pnlFormIntroduction = new System.Windows.Forms.Panel();
            this.txtFormIntroduction = new System.Windows.Forms.RichTextBox();
            this.pnlFormIntroduction.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblJsonContent
            // 
            this.lblJsonContent.AutoSize = true;
            this.lblJsonContent.Location = new System.Drawing.Point(20, 92);
            this.lblJsonContent.Name = "lblJsonContent";
            this.lblJsonContent.Size = new System.Drawing.Size(135, 13);
            this.lblJsonContent.TabIndex = 0;
            this.lblJsonContent.Text = "New Table Row(s) (JSON):";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(390, 374);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(309, 374);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
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
            this.txtJsonContent.Location = new System.Drawing.Point(23, 111);
            this.txtJsonContent.Name = "txtJsonContent";
            this.txtJsonContent.Size = new System.Drawing.Size(442, 245);
            this.txtJsonContent.TabIndex = 1;
            this.txtJsonContent.Text = "";
            this.txtJsonContent.TextChanged += new System.EventHandler(this.txtJsonContent_TextChanged);
            // 
            // pnlFormIntroduction
            // 
            this.pnlFormIntroduction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFormIntroduction.BackColor = System.Drawing.Color.Gainsboro;
            this.pnlFormIntroduction.Controls.Add(this.txtFormIntroduction);
            this.pnlFormIntroduction.Location = new System.Drawing.Point(0, 0);
            this.pnlFormIntroduction.Name = "pnlFormIntroduction";
            this.pnlFormIntroduction.Size = new System.Drawing.Size(484, 70);
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
            this.txtFormIntroduction.Size = new System.Drawing.Size(442, 50);
            this.txtFormIntroduction.TabIndex = 12;
            this.txtFormIntroduction.Text = "Use this window to add rows to an existing table.";
            // 
            // PushData_JSON
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(484, 416);
            this.ControlBox = false;
            this.Controls.Add(this.pnlFormIntroduction);
            this.Controls.Add(this.txtJsonContent);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblJsonContent);
            this.MinimumSize = new System.Drawing.Size(500, 455);
            this.Name = "PushData_JSON";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Push Data - JSON Document";
            this.Load += new System.EventHandler(this.PushData_JSON_Load);
            this.pnlFormIntroduction.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblJsonContent;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RichTextBox txtJsonContent;
        private System.Windows.Forms.Panel pnlFormIntroduction;
        private System.Windows.Forms.RichTextBox txtFormIntroduction;
    }
}