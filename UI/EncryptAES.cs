using Leaves_FAT_Management.Common;
using Leaves_FAT_Management.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Leaves_FAT_Management.UI
{
    public partial class EncryptAES : Form
    {
        public EncryptAES()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxToEncrypt_TextChanged(object sender, EventArgs e)
        {
            // crypt the text
            textBoxEncrypted.Text = Utility.Encrypt(textBoxToEncrypt.Text);
        }

        private void buttonCopyClipboard_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxEncrypted.Text)) return;

            Clipboard.SetText(textBoxEncrypted.Text, TextDataFormat.Text);
           
            // show tooltip
            toolTipInfo.ToolTipIcon = ToolTipIcon.Info;
            toolTipInfo.IsBalloon = toolTipInfo.ShowAlways = true;
            toolTipInfo.Show("Copied", textBoxEncrypted, textBoxEncrypted.Location.X, textBoxEncrypted.Location.Y - 30);
        }
    }
}
