namespace Leaves_FAT_Management.UI
{
    partial class SFTPBackup
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
            this.buttonAutoBackup = new System.Windows.Forms.Button();
            this.textBoxLogSFTP = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonAutoBackup
            // 
            this.buttonAutoBackup.AutoSize = true;
            this.buttonAutoBackup.Location = new System.Drawing.Point(37, 212);
            this.buttonAutoBackup.Name = "buttonAutoBackup";
            this.buttonAutoBackup.Size = new System.Drawing.Size(179, 23);
            this.buttonAutoBackup.TabIndex = 0;
            this.buttonAutoBackup.Text = "Auto-backup data on &SFTP server";
            this.buttonAutoBackup.UseVisualStyleBackColor = true;
            this.buttonAutoBackup.Click += new System.EventHandler(this.buttonAutoBackup_Click);
            // 
            // textBoxLogSFTP
            // 
            this.textBoxLogSFTP.BackColor = System.Drawing.Color.White;
            this.textBoxLogSFTP.ForeColor = System.Drawing.Color.Green;
            this.textBoxLogSFTP.Location = new System.Drawing.Point(12, 12);
            this.textBoxLogSFTP.Multiline = true;
            this.textBoxLogSFTP.Name = "textBoxLogSFTP";
            this.textBoxLogSFTP.ReadOnly = true;
            this.textBoxLogSFTP.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLogSFTP.Size = new System.Drawing.Size(327, 189);
            this.textBoxLogSFTP.TabIndex = 20;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(222, 212);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(117, 23);
            this.buttonCancel.TabIndex = 21;
            this.buttonCancel.Text = "C&ancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // SFTPBackup
            // 
            this.AcceptButton = this.buttonAutoBackup;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(351, 247);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBoxLogSFTP);
            this.Controls.Add(this.buttonAutoBackup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SFTPBackup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto-backup via SFTP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxLogSFTP;
        private System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.Button buttonAutoBackup;
    }
}