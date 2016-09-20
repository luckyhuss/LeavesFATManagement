namespace Leaves_FAT_Management.UI
{
    partial class CustomMessageBox
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelMessage = new System.Windows.Forms.Label();
            this.linkLabelToOpen = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(251, 97);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.ForeColor = System.Drawing.Color.Green;
            this.labelMessage.Location = new System.Drawing.Point(12, 9);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(314, 85);
            this.labelMessage.TabIndex = 1;
            this.labelMessage.Text = "label1";
            // 
            // linkLabelToOpen
            // 
            this.linkLabelToOpen.AutoSize = true;
            this.linkLabelToOpen.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelToOpen.Location = new System.Drawing.Point(12, 102);
            this.linkLabelToOpen.Name = "linkLabelToOpen";
            this.linkLabelToOpen.Size = new System.Drawing.Size(93, 13);
            this.linkLabelToOpen.TabIndex = 2;
            this.linkLabelToOpen.TabStop = true;
            this.linkLabelToOpen.Text = "Click here to open";
            this.linkLabelToOpen.Visible = false;
            this.linkLabelToOpen.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelToOpen_LinkClicked);
            // 
            // CustomMessageBox
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(338, 128);
            this.Controls.Add(this.linkLabelToOpen);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomMessageBox";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Message Box";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        public System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.LinkLabel linkLabelToOpen;
    }
}