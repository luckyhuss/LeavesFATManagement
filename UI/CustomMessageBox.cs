using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Leaves_FAT_Management.UI
{
    public partial class CustomMessageBox : Form
    {
        public CustomMessageBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set the "Click here to open" link
        /// </summary>
        /// <param name="linkData">Link to be opened when clicked by the user</param>
        public void SetLinkToOpen(string linkData)
        {
            LinkLabel.Link link = new LinkLabel.Link();
            link.LinkData = linkData;
            linkLabelToOpen.Links.Add(link);
            linkLabelToOpen.Visible = true;

            ToolTip toolTip = new ToolTip();
            //The below are optional, of course,

            toolTip.ToolTipIcon = ToolTipIcon.Info;
            toolTip.IsBalloon = true;
            toolTip.ShowAlways = false;

            toolTip.SetToolTip(linkLabelToOpen, String.Format("Click the link to open \r\n {0}", linkData));
        }

        private void linkLabelToOpen_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Send the URL to the operating system.
            Process.Start(e.Link.LinkData as string);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
