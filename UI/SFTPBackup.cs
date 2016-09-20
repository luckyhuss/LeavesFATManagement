using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Tamir.SharpSsh;
using Ionic.Zip;
using System.IO;
using Leaves_FAT_Management.Core;
using System.Net;
using Leaves_FAT_Management.Common;

namespace Leaves_FAT_Management.UI
{
    public partial class SFTPBackup : Form
    {
        /// <summary>
        /// Set or get whether the upload has ended
        /// </summary>
        public bool IsUploadCompleted { get; set; }

        public SFTPBackup()
        {
            InitializeComponent();
            IsUploadCompleted = false; // default value
        }

        private void buttonAutoBackup_Click(object sender, EventArgs e)
        {
            // disable buttons
            buttonAutoBackup.Enabled = buttonCancel.Enabled = false;
            // clear log
            textBoxLogSFTP.Clear();
            // zip data
            string zipFilename = ZipData();
            // upload zip to SFTP
            UploadFTPFile(zipFilename);

            // check if auto-backup launched
            if (this.ShowInTaskbar)
            {
                LogMessage("=== Auto-backup ended ===");
                buttonCancel.Enabled = true;

                for (int i = 1; i <= 5; i++)
                {
                    buttonCancel.Text = String.Format("Closing in {0}s", 6 - i);
                    // force process Windows message in queue
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }

                // auto-backup done, so auto-close window                
                buttonCancel.PerformClick();
            }

            // enable buttons
            buttonAutoBackup.Enabled = buttonCancel.Enabled = true;
            IsUploadCompleted = true;
        }

        /// <summary>
        ///  Zip database on merle
        /// </summary>
        /// <returns>Zipped filename</returns>
        private string ZipData()
        {
            using (ZipFile zipFAT = new ZipFile())
            {
                string srcDatabase = Path.Combine(ConfigurationManager.AppSettings["Path.AppRepository"], "data");
                string destZipDatabase = Path.Combine(ConfigurationManager.AppSettings["Path.AppRepository"], "auto-backup");
                
                // add database file to this zip file
                zipFAT.AddFiles(new string[] { Path.Combine(srcDatabase, "leaves.accdb") }, "");

                string zipFileName = Path.Combine(destZipDatabase, "backup_leaves_" + DateTime.Today.ToString("yyyyMMdd") + ".zip");

                // save ZIP file on bulbul
                zipFAT.Save(zipFileName);

                LogMessage("Database zipped successfully");
                return zipFileName;
            }
        }

        /// <summary>
        /// Upload zipped data file to SFTP server
        /// </summary>
        /// <param name="fileToUpload">File to upload to SFTP server</param>
        private void UploadFTPFile(string fileToUpload)
        {
            try
            {
                textBoxLogSFTP.ForeColor = Color.Green;
                string host = ConfigurationManager.AppSettings["AutoBackup.Host"];
                SshTransferProtocolBase sftpBase = new Sftp(
                    host,
                    Utility.Decrypt(ConfigurationManager.AppSettings["AutoBackup.Username.CryptedAES"]),
                    Utility.Decrypt(ConfigurationManager.AppSettings["AutoBackup.Password.CryptedAES"]));

                sftpBase.OnTransferStart += new FileTransferEvent(sftpBase_OnTransferStart);
                sftpBase.OnTransferEnd += new FileTransferEvent(sftpBase_OnTransferEnd);
                LogMessage("Credentials decrypted");
                LogMessage(String.Format("Connecting to {0} ( IP: {1}) via SFTP ..", host, Dns.GetHostEntry(host).AddressList.FirstOrDefault()));
                sftpBase.Connect();
                LogMessage("Connected successfully !");
                string remoteFilePath = ConfigurationManager.AppSettings["AutoBackup.RemotePath"];

                // set path to be used later by Events
                textBoxLogSFTP.Tag = String.Concat("[", host, "] ", remoteFilePath, "/", Path.GetFileName(fileToUpload));

                // upload zipped file
                sftpBase.Put(fileToUpload, remoteFilePath);                
                LogMessage("Closing connection ..");
                sftpBase.Close();
                LogMessage("Disconnected.");
            }
            catch (Exception ex)
            {
                textBoxLogSFTP.ForeColor = Color.Red;
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Log message on screen
        /// </summary>
        /// <param name="messageToLog"></param>
        private void LogMessage(string messageToLog)
        {
            // SFTP
            textBoxLogSFTP.Text += textBoxLogSFTP.Text.Length == 0 ? messageToLog : string.Concat("\r\n", messageToLog);
            //move the caret to the end of the text
            textBoxLogSFTP.SelectionStart = textBoxLogSFTP.TextLength;
            //scroll to the caret
            textBoxLogSFTP.ScrollToCaret();
            textBoxLogSFTP.Invalidate();
            Application.DoEvents(); // Releases the current thread back to windows form
        }

        private void sftpBase_OnTransferStart(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            LogMessage("File transfer started ..");
        }

        private void sftpBase_OnTransferEnd(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            LogMessage("File transfer is in progress ..");
            LogMessage(transferredBytes.ToString() + " bytes transferred");
            LogMessage("Total uploaded : " + totalBytes.ToString() + " bytes");
            LogMessage("Uploaded to : " + textBoxLogSFTP.Tag);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            IsUploadCompleted = true;
            // close this window
            this.Close();
        }
    }
}
