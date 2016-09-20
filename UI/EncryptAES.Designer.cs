namespace Leaves_FAT_Management.UI
{
    partial class EncryptAES
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxToEncrypt = new System.Windows.Forms.TextBox();
            this.textBoxEncrypted = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonCopyClipboard = new System.Windows.Forms.Button();
            this.toolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text to encrypt :";
            // 
            // textBoxToEncrypt
            // 
            this.textBoxToEncrypt.Location = new System.Drawing.Point(102, 20);
            this.textBoxToEncrypt.Name = "textBoxToEncrypt";
            this.textBoxToEncrypt.Size = new System.Drawing.Size(241, 20);
            this.textBoxToEncrypt.TabIndex = 1;
            this.textBoxToEncrypt.TextChanged += new System.EventHandler(this.textBoxToEncrypt_TextChanged);
            // 
            // textBoxEncrypted
            // 
            this.textBoxEncrypted.Location = new System.Drawing.Point(102, 46);
            this.textBoxEncrypted.Multiline = true;
            this.textBoxEncrypted.Name = "textBoxEncrypted";
            this.textBoxEncrypted.ReadOnly = true;
            this.textBoxEncrypted.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxEncrypted.Size = new System.Drawing.Size(241, 99);
            this.textBoxEncrypted.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Crypted text :";
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(268, 151);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "Cl&ose";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonCopyClipboard
            // 
            this.buttonCopyClipboard.Location = new System.Drawing.Point(150, 151);
            this.buttonCopyClipboard.Name = "buttonCopyClipboard";
            this.buttonCopyClipboard.Size = new System.Drawing.Size(112, 23);
            this.buttonCopyClipboard.TabIndex = 5;
            this.buttonCopyClipboard.Text = "&Copy clipboard";
            this.buttonCopyClipboard.UseVisualStyleBackColor = true;
            this.buttonCopyClipboard.Click += new System.EventHandler(this.buttonCopyClipboard_Click);
            // 
            // EncryptAES
            // 
            this.AcceptButton = this.buttonCopyClipboard;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(355, 183);
            this.Controls.Add(this.buttonCopyClipboard);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.textBoxEncrypted);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxToEncrypt);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EncryptAES";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Encrypt AES";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxToEncrypt;
        private System.Windows.Forms.TextBox textBoxEncrypted;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonCopyClipboard;
        private System.Windows.Forms.ToolTip toolTipInfo;
    }
}