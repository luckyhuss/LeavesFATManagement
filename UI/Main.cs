using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Globalization;
using System.Net;

using Leaves_FAT_Management.Core;

using Ionic.Zip;
using Leaves_FAT_Management.Common;
using Leaves_FAT_Management.Persistence;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Leaves_FAT_Management.UI
{
    /// <summary>
    /// This is the main class for the FAT management system
    /// Author: ABO
    /// </summary>
    public partial class Main : Form
    {
        #region Properties
        string srcFAT { set; get; }
        string destFAT { set; get; }
        int checkedIndex { set; get; }
        string versionText { set; get; }
        /// <summary>
        /// List used to manage and populate public holidays in Mauritius ( on-demand populate)
        /// </summary>
        List<DateTime> holidays { set; get; }
        // for locking mechanism
        private static object myLock = new object();
        #endregion

        #region Init
        public Main()
        {
            InitializeComponent();
        }

        private void main_Load(object sender, EventArgs e)
        {
            BindControls();            

            // Is not already working asynchronously, launch Worker
            if (!backgroundWorkerStartupProcessing.IsBusy)
            {
                // launch the processing
                backgroundWorkerStartupProcessing.RunWorkerAsync();
            }
        }


        /// <summary>
        /// Backup DB file on bulbul
        /// </summary>
        private void LoadHolidaysInCache(int year)
        {
            if (holidays == null) // 1st check
            {
                lock (myLock)
                {
                    if (holidays == null) // 2nd check
                    {
                        holidays = new List<DateTime>();
                    }
                }
            }

            // populate list with holidays for the given year
            holidays.AddRange(Database.SelectHolidayDateByYear(year));
        }

        /// <summary>
        /// Backup DB file on bulbul
        /// </summary>
        private void BackupDB()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["leavesConnectionString"].ConnectionString;
            connectionString = connectionString.Substring(connectionString.LastIndexOf("Source=") + 7);
            string dirPath = Path.GetDirectoryName(connectionString);
            string backupDBRemote = Path.Combine(dirPath, "leaves_backup" + Path.GetExtension(connectionString));
            string backupDBLocal = Path.Combine(Application.StartupPath, Path.GetFileName(connectionString));
            File.Copy(connectionString, backupDBRemote, true);
            backgroundWorkerStartupProcessing.ReportProgress(1, @"Database backed up on \\merle ..");
            System.Threading.Thread.Sleep(2000);
            //File.Copy(connectionString, backupDBLocal, true);
            backgroundWorkerStartupProcessing.ReportProgress(2, "Database backed up locally ( not anymore) ..");
            System.Threading.Thread.Sleep(2000);
        }

        /// <summary>
        /// Backup DB file on bulbul
        /// </summary>
        private void CheckVersion()
        {
            // check if user has the latest version of app.config locally
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["AutoBackup.RemotePath"]))
            {
                string urlToConfigFileRepo = Path.Combine(ConfigurationManager.AppSettings["Path.AppRepository"], "run_x86");
                CustomMessageBox cmb = new CustomMessageBox();
                cmb.labelMessage.Text = String.Format(
                    "You do not have the latest version of app.config\r\nKindly fetch it from\r\n\r\n{0}",
                    urlToConfigFileRepo);
                cmb.labelMessage.ForeColor = Color.Red;
                cmb.SetLinkToOpen(urlToConfigFileRepo);
                cmb.Text = @"/!\ WARNING: Important updates missing";
                
                backgroundWorkerStartupProcessing.ReportProgress(3, cmb);
            }

            string versionFile = Path.Combine(ConfigurationManager.AppSettings["Path.AppRepository"], "version.ini");
            // current version no.
            string currentVersion = Application.ProductVersion;

            // update main title            
            //this.Text = String.Format(this.Text, currentVersion);
            backgroundWorkerStartupProcessing.ReportProgress(4, String.Format(this.Text, currentVersion));

            backgroundWorkerStartupProcessing.ReportProgress(5, "You have the latest version of FAT Management System");

            if (File.Exists(versionFile))
            {
                backgroundWorkerStartupProcessing.ReportProgress(6, "Checking for latest version of FAT Management System ..");
                System.Threading.Thread.Sleep(2000);

                // read latest version no.
                string latestVersion = File.ReadAllText(versionFile, Encoding.Default);
                // replace . for comparison
                long latest = Convert.ToInt64(latestVersion.Replace(".", String.Empty));
                long current = Convert.ToInt64(currentVersion.Replace(".", String.Empty));

                if (current < latest)
                {
                    backgroundWorkerStartupProcessing.ReportProgress(7, @"There is a latest version of FAT Management System available, please download it from \\merle");
                }
                else if (current > latest)
                {
                    // if latest version no. is less, update with current version no.
                    File.WriteAllText(versionFile, currentVersion, Encoding.Default);
                    backgroundWorkerStartupProcessing.ReportProgress(8, "Updating the latest version of FAT Management System ..");
                    System.Threading.Thread.Sleep(2000);
                }
            }
            else
            {
                // create version file with current version no.
                File.WriteAllText(versionFile, currentVersion, Encoding.Default);
                backgroundWorkerStartupProcessing.ReportProgress(9, "Creating file for latest version of FAT Management System ..");
                System.Threading.Thread.Sleep(2000);
            }

            backgroundWorkerStartupProcessing.ReportProgress(10, textBoxVersion);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add month and year to srcPath and destPath
        /// </summary>
        /// <param name="dt"></param>
        private void ResetPaths(DateTime dt)
        {
            srcFAT = ConfigurationManager.AppSettings["Path.SourceFAT"];
            destFAT = Path.Combine(Application.StartupPath, "FAT");

            srcFAT = Path.Combine(srcFAT, dt.ToString("yyyy"), dt.ToString("yyyy-MM"));
            destFAT = Path.Combine(destFAT, dt.ToString("yyyy"), dt.ToString("yyyy-MM"));
        }

        /// <summary>
        /// Bind all data-driven controls
        /// </summary>
        private void BindControls()
        {
            // ressource            
            comboBoxRessource.DataSource = Database.SelectAllRessources().Tables[0];
            comboBoxRessource.ValueMember = "ID";
            comboBoxRessource.DisplayMember = "Name";
            comboBoxRessource.SelectedIndex = -1;

            // leave type
            comboBoxLeaveType.DataSource = Database.SelectAllLeaveType().Tables[0];
            comboBoxLeaveType.ValueMember = "ID";
            comboBoxLeaveType.DisplayMember = "LeaveName";

            // from and to dates
            int numberOfDaysInMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
            string dateRange = string.Format("#{0}/{1}/{3}# And #{0}/{2}/{3}#", DateTime.Today.Month, 1, numberOfDaysInMonth, DateTime.Today.Year);

            dateTimePickerFrom.Value = DateTime.Parse(string.Format("{0}/{1}/{2}", 1, DateTime.Today.Month, DateTime.Today.Year));
            dateTimePickerTo.Value = DateTime.Parse(string.Format("{0}/{1}/{2}", numberOfDaysInMonth, DateTime.Today.Month, DateTime.Today.Year));

            // reset dates to Today
            dateTimePickerLeaveDate.Value = dateTimePickerLeaveDateTo.Value = DateTime.Today;

            // reset FAT month
            dateTimePickerFATMonth.Value = DateTime.Today;

            // reset SPROD month
            dateTimePickerSPRODMonth.Value = DateTime.Today;

            // bind grids
            BindGridViews();

            // calculate no. of business days in the month and bind grid view FAT
            //ComputeWorkingDays();
        }

        /// <summary>
        /// Bind all grids
        /// </summary>
        private void BindGridViews()
        {
            // leave
            dataGridViewLeave.DataSource = Database.SelectAllLeaveByPeriod(dateTimePickerFrom.Value, dateTimePickerTo.Value).Tables[0];
        }

        /// <summary>
        /// Compute working days and bind grid view FAT
        /// </summary>
        private void ComputeWorkingDays()
        {
            buttonExportFAT.Enabled = false;

            // reset from and to
            DateTime fatMonth = dateTimePickerFATMonth.Value;
            dateTimePickerFrom.Value = DateTime.Parse(string.Format("{0}/{1}/{2}", 1, fatMonth.Month, fatMonth.Year));
            dateTimePickerTo.Value = DateTime.Parse(string.Format("{0}/{1}/{2}", DateTime.DaysInMonth(fatMonth.Year, fatMonth.Month), fatMonth.Month, fatMonth.Year));

            List<DateTime> holidaysInCurrentMonth = GetHolidaysByMonth(dateTimePickerFrom.Value, dateTimePickerTo.Value);

            int validHolidayCount = 0;
            foreach (DateTime dt in holidaysInCurrentMonth)
            {
                // check if not Sat nor Sun
                if (dt.DayOfWeek != DayOfWeek.Saturday
                    && dt.DayOfWeek != DayOfWeek.Sunday) // not sat or sun
                {
                    // valid public holidays, increment holidays count
                    validHolidayCount++;                    
                }
            }

            numericUpDownHolidays.Value = validHolidayCount;

            int totalDays = Utility.GetNoWorkingDays(dateTimePickerFATMonth.Value) - Convert.ToInt32(numericUpDownHolidays.Value);
            labelWorkingDays.Text = string.Format("Working days : {0}", totalDays);

            // leave FAT
            DataTable dtFAT = Database.CountLeaveRessource(dateTimePickerFrom.Value, dateTimePickerTo.Value).Tables[0];
            // add columns
            dtFAT.Columns.Add("Working\nDays", typeof(double));

            double totalFATTeam = 0.0;
            foreach (DataRow dr in dtFAT.Rows)
            {
                // data for columns
                double realWorkingDays = Convert.ToDouble(totalDays) - Convert.ToDouble(dr["Local"]) - Convert.ToDouble(dr["Sick"]);
                totalFATTeam += realWorkingDays;
                dr["Working\nDays"] = realWorkingDays;
            }

            // add a last row with total FAT for team
            DataRow totalDr = dtFAT.NewRow();
            totalDr["Name"] = "Total FAT";
            totalDr["Working\nDays"] = totalFATTeam;
            dtFAT.Rows.Add(totalDr);

            dataGridViewFAT.DataSource = dtFAT;
        }

        /// <summary>
        /// Get the date of Public Holidays for a given month
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<DateTime> GetHolidaysByMonth(DateTime start, DateTime end)
        {
            // compute total public holidays in the current month
            if (null == holidays ||
                !holidays.Contains(new DateTime(start.Year, 1, 1)))
            {
                // Cache is not loaded yet, need to load for the year computing
                LoadHolidaysInCache(start.Year);
            }

            // contains 1st January holiday of the year computing
            // therefore Cache is loaded correctly, so search for holidays
            List<DateTime> holidaysInCurrentMonth = (
                    from hol in holidays
                    where hol >= start && hol <= end
                    select hol
                ).ToList();
            return holidaysInCurrentMonth;
        }

        /// <summary>
        /// Download all FAT from \\merle, for current month
        /// </summary>
        private void DownloadFAT()
        {
            progressBarFileTransfer.Value = 0;
            textBoxLog.ResetText();
            try
            {
                if (Directory.Exists(destFAT))
                {
                    // delete repèretoire
                    Directory.Delete(destFAT, true);
                }

                Directory.CreateDirectory(destFAT);

                int totalFileCount = Directory.GetFiles(srcFAT, "*.xls", System.IO.SearchOption.AllDirectories).Length;
                progressBarFileTransfer.Maximum = totalFileCount;

                LogMessage(string.Format(@"Downloading {0} FAT from {1}...", totalFileCount, srcFAT.Substring(0, 15)));

                // copy all FAT from bulbul to local disks for comparison
                Utility.CopyDirectory(srcFAT, destFAT, true, progressBarFileTransfer);
            }
            catch (Exception ex)
            {
                LogMessage(string.Format(@"Error downloading FAT from {0}... : {1}", srcFAT.Substring(0, 15), ex.Message));
            }
        }

        /// <summary>
        /// Log message on screen
        /// </summary>
        /// <param name="messageToLog"></param>
        private void LogMessage(string messageToLog)
        {
            switch (tabControlMain.SelectedIndex)
            {
                case 0:
                    // Leave Management
                    break;
                case 1:
                    // FAT
                    textBoxLog.Text += textBoxLog.Text.Length == 0 ? messageToLog : string.Concat("\r\n", messageToLog);
                    //move the caret to the end of the text
                    textBoxLog.SelectionStart = textBoxLog.TextLength;
                    //scroll to the caret
                    textBoxLog.ScrollToCaret();
                    break;
                case 2:
                    // SPROD
                    textBoxLogSPROD.Text += textBoxLogSPROD.Text.Length == 0 ? messageToLog : string.Concat("\r\n", messageToLog);
                    //move the caret to the end of the text
                    textBoxLogSPROD.SelectionStart = textBoxLogSPROD.TextLength;
                    //scroll to the caret
                    textBoxLogSPROD.ScrollToCaret();
                    break;
            }

            textBoxLog.Invalidate();
            textBoxLogSPROD.Invalidate();
            Application.DoEvents(); // Releases the current thread back to windows form
        }

        /// <summary>
        /// Read FAT contents
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private FATCount ReadFAT(string fileName)
        {
            string excelCellTotal = ConfigurationManager.AppSettings["ExcelCellTotal"]; // grand total AY15
            string excelColumnSubTotal = ConfigurationManager.AppSettings["ExcelColumnSubTotal"]; // sub total AZ{0}
            string excelCellMaladie = ConfigurationManager.AppSettings["ExcelCellMaladie"]; // Maladie
            string excelCellCongesPayes = ConfigurationManager.AppSettings["ExcelCellCongesPayes"]; // Congés payés
            string excelCellFerie = ConfigurationManager.AppSettings["ExcelCellFerie"]; // Férié
            string excelCellCongesExceptionnels = ConfigurationManager.AppSettings["ExcelCellCongesExceptionnels"]; // Congés exceptionnels (à préciser)
            // ABO 22/03/2016
            string excelCellFormation = ConfigurationManager.AppSettings["ExcelCellFormation"]; //Formation
            
            // ABO 22/06/2017 - Using NPOI instead of Interop
            HSSFWorkbook workbook = new HSSFWorkbook();
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                workbook = new HSSFWorkbook(file);
            }

            // get sheet from Excel file
            ISheet worksheet = workbook.GetSheet(ConfigurationManager.AppSettings["AnalyseWorksheet"]); // analyse sheet

            // dynamically read the cell position of NON FAT section
            for (int i = 1; i <= worksheet.LastRowNum; i++)
            {
                string cellValue = ManipulateExcel.GetCellValue(worksheet, "B" + i.ToString());

                if (cellValue == null) continue;

                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    if (cellValue.Equals(excelCellMaladie))
                    {
                        excelCellMaladie = string.Format(excelColumnSubTotal, i);
                    }
                    else if (cellValue.Equals(excelCellCongesPayes))
                    {
                        excelCellCongesPayes = string.Format(excelColumnSubTotal, i);
                    }
                    else if (cellValue.Equals(excelCellFerie))
                    {
                        excelCellFerie = string.Format(excelColumnSubTotal, i);
                    }
                    else if (cellValue.Equals(excelCellCongesExceptionnels))
                    {
                        excelCellCongesExceptionnels = string.Format(excelColumnSubTotal, i);
                    }
                    else if (cellValue.Equals(excelCellFormation))
                    {
                        // ABO 22/03/2016
                        excelCellFormation = string.Format(excelColumnSubTotal, i);
                    }
                }
            }

            FATCount fatCount = new FATCount();

            //string currentValue = GetCellValue(worksheet, excelCellTotal);
            fatCount.GrandTotal = Convert.ToDouble(ManipulateExcel.GetCellValue(worksheet, excelCellTotal));
            
            fatCount.TotalMaladie =  Convert.ToDouble(ManipulateExcel.GetCellValue(worksheet, excelCellMaladie));

            //currentValue = worksheet.get_Range(excelCellCongesPayes);
            fatCount.TotalCongesPayes = Convert.ToDouble(ManipulateExcel.GetCellValue(worksheet, excelCellCongesPayes));

            //currentValue = worksheet.get_Range(excelCellCongesExceptionnels);
            fatCount.TotalCongesExceptionnels = Convert.ToDouble(ManipulateExcel.GetCellValue(worksheet, excelCellCongesExceptionnels));
            fatCount.TotalCongesPayes += fatCount.TotalCongesExceptionnels;

            // ABO 22/03/2016            
            fatCount.TotalFormation = Convert.ToDouble(ManipulateExcel.GetCellValue(worksheet, excelCellFormation));
            fatCount.TotalCongesPayes += fatCount.TotalFormation;

            fatCount.TotalFerie =  Convert.ToDouble(ManipulateExcel.GetCellValue(worksheet, excelCellFerie));

            return fatCount;
        }

        /// <summary>
        /// Zip all FAT on bulbul (current month) and mail to BPO & Paula
        /// </summary>
        /// <param name="fileNameTexte"></param>
        private void ZipAndMailFAT(string fileNameTexte)
        {
            using (ZipFile zipFAT = new ZipFile())
            {
                // get all FAT filnames from the src dir
                string[] fatNames = Directory.GetFiles(srcFAT, "*.xls", System.IO.SearchOption.AllDirectories);
                
                // get only validated FATs
                fatNames = fatNames.Where(t => t.Contains("Valid")).ToArray();
                
                // add all FAT files to this zip file
                zipFAT.AddFiles(fatNames, "");

                string zipFileName = Path.Combine(srcFAT, "FAT_VIVOP_" + dateTimePickerFATMonth.Value.ToString("yyyy-MM") + ".zip");

                // save ZIP file on bulbul
                zipFAT.Save(zipFileName);

                LogMessage(string.Format(@"{0} FAT zipped on \\merle => OK", fatNames.Length));

                // send mail to BPO
                string month = dateTimePickerFATMonth.Value.ToString("MMMM yyyy", CultureInfo.GetCultureInfo("fr-FR"));

                StringBuilder sbBody = new StringBuilder();
                sbBody.Append("<br />Bonjour,<br /><br />");
                sbBody.AppendFormat("Veuillez trouver ci-joint les FATs de {0} pour l’équipe VIVOP (COLORIS/FCI/ASPIN/SPID/VIO/VIE/PILPRO/ORCO).<br />",
                    month);
                sbBody.Append("Il y a un compte des jours facturables dans le fichier texte, autogénéré.<br /><br />");
                sbBody.Append("Cordialement,<br />");
                sbBody.Append(ConfigurationManager.AppSettings["Name.Launcher"]);

                // TODO: open ms outlook
                //Utility.OpenOutlookMail(
                //    ConfigurationManager.AppSettings["Email.BPO"],
                //    ConfigurationManager.AppSettings["Email.Director"],
                //    string.Format("FAT {0} - Equipe VIVOP", month),
                //    sbBody.ToString(),
                //    new string[] { zipFileName, fileNameTexte });
            }
        }
        #endregion

        #region Events handling
        private void buttonExportFAT_Click(object sender, EventArgs e)
        {
            // export Grand Total in text format
            LogMessage(string.Format(@"Exporting {0} FAT data on \\merle ..", dataGridViewFAT.Rows.Count - 2));

            try
            {
                // get datasource of FAT gridview
                DataTable dt = (DataTable)dataGridViewFAT.DataSource;

                StringBuilder exportContent = new StringBuilder();
                // add header
                exportContent.AppendFormat("\t==============================\r\n");
                exportContent.AppendFormat("\t| Autogenerated by FAT Mgmt  |\r\n");
                exportContent.AppendFormat("\t|                            |\r\n");
                exportContent.AppendFormat("\t| Date : {0}          |\r\n", DateTime.Today.ToString("dd/MM/yyyy"));
                exportContent.AppendFormat("\t==============================\r\n");

                // loop all data to export
                foreach (DataRow dr in dt.Rows)
                {
                    string name = dr["Name"].ToString().Trim();

                    if (name.Contains("FAT"))
                    {
                        exportContent.Append("\r\n");
                        exportContent.Append("\t==============================\r\n");
                    }
                    exportContent.AppendFormat("\t{0} :", name);

                    if (name.Length > 13)
                    {
                        exportContent.AppendFormat("\t");
                    }
                    else
                    {
                        exportContent.AppendFormat("\t\t");
                    }
                    exportContent.AppendFormat("{0}\r\n", dr["Grand\nTotal"].ToString().Replace("(X)", string.Empty).Replace("(OK)", string.Empty));

                    if (name.Contains("FAT"))
                    {
                        exportContent.Append("\t==============================");
                    }
                }

                // create a writer and open the file
                string fileNameTexte = Path.Combine(srcFAT, "FAT_" + dateTimePickerFATMonth.Value.ToString("yyyy-MM") + ".txt");
                TextWriter tw = new StreamWriter(fileNameTexte, false, Encoding.Default);

                // write a line of text to the file
                tw.WriteLine(exportContent.ToString());

                // close the stream
                tw.Close();

                LogMessage(string.Format(@"Exporting {0} FAT on \\merle completed => OK", dataGridViewFAT.Rows.Count - 2));

                // zip all FAT
                ZipAndMailFAT(fileNameTexte);
            }
            catch (Exception ex)
            {
                LogMessage(string.Format(@"Error exporting : {0}", ex.Message));
            }
        }

        private void buttonNotifyLeave_Click(object sender, EventArgs e)
        {
            if (comboBoxRessource.SelectedIndex == -1)
            {
                MessageBox.Show("Select a ressource first", "Create leave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] ressourceCP = comboBoxRessource.SelectedValue.ToString().Split('|');

            for (DateTime dt = dateTimePickerLeaveDate.Value; dt <= dateTimePickerLeaveDateTo.Value; dt = dt.AddDays(1))
            {
                if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday) // not saturday or sunday
                {
                    // insert leave
                    Database.InsertLeave(
                        Convert.ToInt32(ressourceCP[0]),
                        Convert.ToInt32(comboBoxLeaveType.SelectedValue),
                        dt,
                        checkBoxHalfDay.Checked);
                }
            }

            BindGridViews();

            if (comboBoxLeaveType.SelectedValue.Equals(2))
            {
                // case of sick leave
                if (radioButtonLMS.Checked)
                {
                    // open LMS
                    Process.Start(ConfigurationManager.AppSettings["URL.LMS"]);
                }
                else if (radioButtonBPO.Checked)
                {
                    // mail to BPO MU
                    StringBuilder sbBodyBPO = new StringBuilder();
                    sbBodyBPO.Append("<br />Bonjour,<br /><br />");
                    sbBodyBPO.AppendFormat("{0} est en arrêt maladie aujourd’hui {1}.<br /><br />",
                        comboBoxRessource.Text.Split(' ')[0],
                        DateTime.Today.ToString("dd/MM"));
                    sbBodyBPO.Append("Cordialement,<br />");
                    sbBodyBPO.Append(ConfigurationManager.AppSettings["Name.Launcher"]);

                    // TODO: open ms outlook
                    //Utility.OpenOutlookMail(
                    //    ConfigurationManager.AppSettings["Email.BPO"],
                    //    ConfigurationManager.AppSettings["Email.Director"],
                    //    string.Format("Absence {0}", ressourceCP[1]),
                    //    sbBodyBPO.ToString(),
                    //    null);
                }

                if (checkBoxSendMail.Checked)
                {
                    // mail to Chef de Projet Lyon
                    StringBuilder sbBodyCP = new StringBuilder();
                    sbBodyCP.AppendFormat("<br />Bonjour {0},<br /><br />", ressourceCP[4]);
                    sbBodyCP.AppendFormat("{0} est en arrêt maladie aujourd’hui {1}.<br /><br />",
                        comboBoxRessource.Text.Split(' ')[0],
                        DateTime.Today.ToString("dd/MM"));
                    sbBodyCP.Append("Cordialement,<br />");
                    sbBodyCP.Append(ConfigurationManager.AppSettings["Name.Launcher"]);

                    // TODO: open ms outlook
                    //Utility.OpenOutlookMail(
                    //    ressourceCP[2],
                    //    ressourceCP[3],
                    //    string.Format("Absence {0}", ressourceCP[1]),
                    //    sbBodyCP.ToString(),
                    //    null);
                }
            }
        }

        private void dateTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            BindGridViews();
        }

        private void dateTimePickerTo_ValueChanged(object sender, EventArgs e)
        {
            BindGridViews();
        }

        private void dateTimePickerFATMonth_ValueChanged(object sender, EventArgs e)
        {
            // To prevent Error : Year, Month, and Day parameters describe an un-representable DateTime
            var dtp = (DateTimePicker)sender;
            dtp.Value = new DateTime(dtp.Value.Year, dtp.Value.Month, 1);

            ComputeWorkingDays();

            ResetPaths(dateTimePickerFATMonth.Value);
        }

        private void numericUpDownHolidays_ValueChanged(object sender, EventArgs e)
        {
            // ComputeWorkingDays();
        }

        private void buttonCompareFAT_Click(object sender, EventArgs e)
        {
            // download FAT from \\merle
            buttonExportFAT.Enabled = buttonCompareFAT.Enabled = false;
            DownloadFAT();

            if (!Directory.Exists(destFAT))
            {
                MessageBox.Show(destFAT + " does not exists.");
                return;
            }

            // get all FAT in the dest dir
            string[] fatNames = Directory.GetFiles(destFAT, "*.xls", System.IO.SearchOption.AllDirectories);

            LogMessage(string.Format("Validating {0} FAT (locally) ..", fatNames.Length));

            // get datasource of FAT gridview
            DataTable dt = (DataTable)dataGridViewFAT.DataSource;

            // datagrid dynamic column names
            const string COLUMN_NAME_FAT_FOUND = "FAT\nFound";
            const string COLUMN_NAME_GRAND_TOTAL = "Grand\nTotal";
            const string COLUMN_NAME_LOCAL_FAT = "Local\nin FAT";
            const string COLUMN_NAME_SICK_FAT = "Sick\nin FAT";
            const string COLUMN_NAME_FERIE_FAT = "Férié\nin FAT";

            // delete columns if exist
            if (dt.Columns.Contains(COLUMN_NAME_FAT_FOUND))
            {
                dt.Columns.Remove(COLUMN_NAME_FAT_FOUND);
                dt.Columns.Remove(COLUMN_NAME_GRAND_TOTAL);
                dt.Columns.Remove(COLUMN_NAME_LOCAL_FAT);
                dt.Columns.Remove(COLUMN_NAME_SICK_FAT);
                dt.Columns.Remove(COLUMN_NAME_FERIE_FAT);
            }
            // add columns
            dt.Columns.Add(COLUMN_NAME_FAT_FOUND, typeof(string));
            dt.Columns.Add(COLUMN_NAME_GRAND_TOTAL, typeof(string));
            dt.Columns.Add(COLUMN_NAME_LOCAL_FAT, typeof(string));
            dt.Columns.Add(COLUMN_NAME_SICK_FAT, typeof(string));
            dt.Columns.Add(COLUMN_NAME_FERIE_FAT, typeof(string));
                        
            progressBarFileTransfer.Maximum = dt.Rows.Count;
            progressBarFileTransfer.Value = 0;
            int countRow = 0;
            double sumGrandTotal = 0.0;
            double sumLocalTotal = 0.0;
            double sumSickTotal = 0.0;
            int successPercentage = 0;

            foreach (DataRow dr in dt.Rows)
            {
                progressBarFileTransfer.PerformStep();
                Application.DoEvents();

                // check for last row
                if (dr["Name"].ToString().Contains("FAT"))
                {
                    // set sum of total in FAT
                    dr[COLUMN_NAME_GRAND_TOTAL] = sumGrandTotal;
                    dr[COLUMN_NAME_LOCAL_FAT] = sumLocalTotal;
                    dr[COLUMN_NAME_SICK_FAT] = sumSickTotal;
                    continue;
                }

                string foreName = dr["Name"].ToString().Split(' ')[0];
                foreName = string.Concat(foreName, "_FAT_", dateTimePickerFATMonth.Value.ToString("yyyyMM"));
                var found = fatNames.Where(name => name.Contains(foreName)).ToArray();

                if (found.Length > 0)
                {
                    if (found[0].Contains("Valid"))
                    {
                        dr[COLUMN_NAME_FAT_FOUND] = "OK (V)";
                    }
                    else
                    {
                        dr[COLUMN_NAME_FAT_FOUND] = "OK (Non V)";
                    }
                }
                else
                {
                    dr[COLUMN_NAME_FAT_FOUND] = "X";
                }


                // call excel extract here
                FATCount fatCount = new FATCount();

                if (found.Length > 0)
                {
                    // process each Excel file
                    fatCount = ReadFAT(found[0]);
                }

                dr[COLUMN_NAME_GRAND_TOTAL] = string.Format("{0} ({1})", fatCount.GrandTotal,
                    Convert.ToDouble(dr["Working\nDays"]) == fatCount.GrandTotal ? "OK" : "X");
                sumGrandTotal += fatCount.GrandTotal;

                dr[COLUMN_NAME_LOCAL_FAT] = string.Format("{0} ({1})", fatCount.TotalCongesPayes,
                    Convert.ToDouble(dr["Local"]) == fatCount.TotalCongesPayes ? "OK" : "X");
                if (fatCount.TotalCongesExceptionnels > 0.0)
                {
                    dr[COLUMN_NAME_LOCAL_FAT] = string.Concat(dr[COLUMN_NAME_LOCAL_FAT], " * X");
                }
                if (fatCount.TotalFormation > 0.0)
                {
                    dr[COLUMN_NAME_LOCAL_FAT] = string.Concat(dr[COLUMN_NAME_LOCAL_FAT], " * F");
                }
                sumLocalTotal += fatCount.TotalCongesPayes;

                dr[COLUMN_NAME_SICK_FAT] = string.Format("{0} ({1})", fatCount.TotalMaladie,
                    Convert.ToDouble(dr["Sick"]) == fatCount.TotalMaladie ? "OK" : "X");
                sumSickTotal += fatCount.TotalMaladie;

                dr[COLUMN_NAME_FERIE_FAT] = string.Format("{0} ({1})", fatCount.TotalFerie,
                    Convert.ToDouble(numericUpDownHolidays.Value) == fatCount.TotalFerie ? "OK" : "X");

                // change style if error found
                if (found.Length == 0 || dr[COLUMN_NAME_GRAND_TOTAL].ToString().Contains("X")
                    || dr[COLUMN_NAME_LOCAL_FAT].ToString().Contains("X") || dr[COLUMN_NAME_SICK_FAT].ToString().Contains("X")
                    || dr[COLUMN_NAME_FERIE_FAT].ToString().Contains("X"))
                {
                    // NOT OK
                    dataGridViewFAT.Rows[countRow].DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    // OK
                    dataGridViewFAT.Rows[countRow].DefaultCellStyle.BackColor = Color.Green;
                    dataGridViewFAT.Rows[countRow].DefaultCellStyle.ForeColor = Color.White;
                    successPercentage++;
                }
                countRow++;
            }

            if (fatNames.Length != 0)
            {
                successPercentage = Convert.ToInt32(Convert.ToDouble(successPercentage) / Convert.ToDouble(fatNames.Length) * 100);
            }

            LogMessage(string.Format("Validation of {0} FAT completed => OK", fatNames.Length));
            LogMessage(string.Format("{0}% of FAT are OK", successPercentage));
            
            // enable buttons
            buttonExportFAT.Enabled = buttonCompareFAT.Enabled = true;
        }

        private void linkLabelDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // loop and delete all leave
            ArrayList idLeave = new ArrayList();

            foreach (DataGridViewRow dgvr in dataGridViewLeave.Rows)
            {
                if (dgvr.Cells[0].Value != null && (bool)dgvr.Cells[0].Value)
                {
                    // add to arraylist
                    idLeave.Add(dgvr.Cells["ID Leave"].Value);
                }
            }

            if (idLeave.Count > 0 &&
                MessageBox.Show("Are you sure to delete?", "Delete leave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                // delete all selected rows
                Database.DeletetLeave(idLeave.ToArray());

                // rebind gridviews
                BindGridViews();
            } else
                if (idLeave.Count == 0)
                {
                    MessageBox.Show("Select leave(s) first", "Delete leave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private void dataGridViewLeave_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // disable columns
            foreach (DataGridViewColumn dgvc in dataGridViewLeave.Columns)
            {
                if (dgvc.HeaderText.Equals("Delete")) continue;
                dgvc.ReadOnly = true;
            }

            dataGridViewLeave.Columns["ID Leave"].Visible = false;
        }

        private void dataGridViewLeave_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                // first columns Delete
                foreach (DataGridViewRow dgvr in dataGridViewLeave.Rows)
                {
                    dgvr.Cells[0].Value = (dgvr.Cells[0].Value != null && (bool)dgvr.Cells[0].Value) ? false : true;
                }
            }
        }

        private void dateTimePickerLeaveDate_ValueChanged(object sender, EventArgs e)
        {
            dateTimePickerLeaveDateTo.Value = dateTimePickerLeaveDate.Value;
        }

        private void dateTimePickerLeaveDateTo_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerLeaveDateTo.Value < dateTimePickerLeaveDate.Value)
            {
                dateTimePickerLeaveDate.Value = dateTimePickerLeaveDateTo.Value;
            }
        }

        private void comboBoxLeaveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Local or Sick
            checkBoxSendMail.Enabled = checkBoxSendMail.Checked =
                radioButtonBPO.Enabled = radioButtonLMS.Enabled = comboBoxLeaveType.SelectedValue.ToString().Equals("2");

            if (checkBoxSendMail.Enabled)
            {
                radioButtonLMS.Checked = true;
            }
            else
            {
                radioButtonBPO.Checked = radioButtonLMS.Checked = false;                
            }
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControlMain.SelectedIndex)
            {
                case 0:
                    // Leave Management
                    // resize
                    this.Width = 566;
                    tabControlMain.Width = 531;
                    this.AcceptButton = buttonNotifyLeave;
                    break;
                case 1:
                    // FAT
                    this.Width = 701;
                    tabControlMain.Width = 666;
                    // reset all paths
                    ResetPaths(dateTimePickerFATMonth.Value);
                    this.AcceptButton = buttonCompareFAT;
                    break;
                case 2:
                    // SPROD
                    this.Width = 510;
                    tabControlMain.Width = 475;
                    // reset all paths
                    ResetPaths(dateTimePickerSPRODMonth.Value);
                    this.AcceptButton = buttonGetRessources;
                    break;
            }
        }

        private void buttonGetRessources_Click(object sender, EventArgs e)
        {
            // Get all filename
            GetFatFileNameSPROD();
        }

        private void buttonExportSPROD_Click(object sender, EventArgs e)
        {
            // disable all buttons
            buttonExportSPROD.Enabled = buttonGetRessources.Enabled = buttonConsoRAF.Enabled = false;

            // clear error messages
            textBoxLogSPROD.Clear();
            textBoxLogSPROD.ForeColor = Color.Green;
            LogMessage("=== START ===");

            try
            {
                // Définir le nom du fichier en se basant sur la date
                // sprod_vivop_012012.csv ou sprodv2_vivop_012012.csv
                string nomFichierSPROD = String.Format(
                    "sprod{0}_vivop_{1}.csv", 
                    checkBoxSPRODV2.Checked ? "v2" : String.Empty,
                    dateTimePickerSPRODMonth.Value.ToString("MMyyyy"));
                
                // read all the data required
                ManipulateExcel excelWrkBook = new ManipulateExcel();

                //Definir le chemin vers le répertoire du serveur
                string fileNameCSV = Path.Combine(srcFAT, nomFichierSPROD);

                // generate CSV file
                string sprodCSV = excelWrkBook.GenerateSPRODCSV(
                    checkedListBoxRessource, textBoxLogSPROD, checkBoxSPRODV2.Checked);
                
                TextWriter tw = new StreamWriter(fileNameCSV, false, Encoding.Default);

                // write a line of text to the file
                tw.Write(sprodCSV);

                // close the stream
                tw.Close();

                LogMessage(@"SPROD CSV exported on \\merle");
                
                if(sprodCSV.Contains("error -> "))
                {
                    textBoxLogSPROD.ForeColor = Color.Red;
                    LogMessage(">> ERROR : The CSV contains error. Please verify.");
                }

                // show save as dialog
                saveFileDialogSPROD.Title = "Save SPROD csv file ..";
                saveFileDialogSPROD.FileName = Path.GetFileName(fileNameCSV);
                saveFileDialogSPROD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (saveFileDialogSPROD.ShowDialog(this) == DialogResult.OK)
                {
                    // copy file from bulbul
                    File.Copy(fileNameCSV, saveFileDialogSPROD.FileName, true);

                    LogMessage("Downloaded locally");
                }
                else
                {
                    LogMessage(@"/!\ Download cancelled");
                }
            }
            catch (Exception ex)
            {
                textBoxLogSPROD.ForeColor = Color.Red;
                LogMessage(String.Format(">> ERROR : {0}", ex.ToString()));
            }
            
            LogMessage("===  END  ===");

            // enable button
            buttonGetRessources.Enabled = true;
        }

        protected void GetFatFileNameSPROD()
        {
            checkedListBoxRessource.Items.Clear();
            buttonExportSPROD.Enabled = buttonConsoRAF.Enabled = false;
            string sName = string.Empty;
            int countFAT = 0;

            if (Directory.Exists(srcFAT))
            {
                string[] allFATFiles = Directory.GetFiles(srcFAT, "*.xls", System.IO.SearchOption.AllDirectories);
                countFAT = allFATFiles.Length;

                foreach (string sFileName in allFATFiles)
                {
                    // Modification pour afficher seulement le nom du fichier
                    sName = Path.GetFileNameWithoutExtension(sFileName);
                    sName = sName.Replace("AstekRhoneAlpesLyon_", string.Empty);
                    sName = sName.Replace("FAT_", string.Empty).Trim();

                    // add the item
                    checkedListBoxRessource.Items.Add(new RessourceItem(sName, sFileName));
                }

                // select all items
                for (int i = 0; i < checkedListBoxRessource.Items.Count; i++)
                {
                    checkedListBoxRessource.SetItemChecked(i, true);
                }
            }
            else
            {
                textBoxLogSPROD.ForeColor = Color.Red;
                LogMessage(String.Format("No such directory exists : \r\n{0}", srcFAT));
            }

            if (countFAT == 0)
            {
                textBoxLogSPROD.ForeColor = Color.Red;
                LogMessage(String.Format("No FAT on : \r\n{0}", srcFAT));
            }
            else
            {
                buttonExportSPROD.Enabled = buttonConsoRAF.Enabled = checkBoxSelectAllSPROD.Checked = true;
            }
        }

        private void checkedListBoxRessource_MouseMove(object sender, MouseEventArgs e)
        {
            if (checkedIndex != checkedListBoxRessource.IndexFromPoint(e.Location))
            {
                ShowToolTip();
            }
        }

        private void ShowToolTip()
        {
            checkedIndex = checkedListBoxRessource.IndexFromPoint(checkedListBoxRessource.PointToClient(MousePosition));
            if (checkedIndex > -1)
            {
                Point p = PointToClient(MousePosition);
                toolTipSPROD.ToolTipTitle = "~ Full path ~";
                toolTipSPROD.SetToolTip(checkedListBoxRessource, ((RessourceItem)checkedListBoxRessource.Items[checkedIndex]).GetFileName());
            }
        }

        private void dateTimePickerSPRODMonth_ValueChanged(object sender, EventArgs e)
        {
            ResetPaths(dateTimePickerSPRODMonth.Value);
        }

        private void checkedListBoxRessource_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBoxRessource_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(checkedListBoxRessource.CheckedItems.Count.ToString());
        }

        private void timerVersion_Tick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxVersion.Text))
            {
                // set text message
                textBoxVersion.Text = versionText;
                timerVersion.Interval = 1000;
            }
            else
            {
                // empty text message
                textBoxVersion.Text = string.Empty;
                timerVersion.Interval = 500;
            }
        }

        private void createFATDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FatDir fatDir = new FatDir();
            fatDir.textBoxFATPath.Text = ConfigurationManager.AppSettings["Path.SourceFAT"];

            // show Modal Dialog
            fatDir.ShowDialog(this);

        }

        private void backupDataViaFTPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SFTPBackup sftpBackup = new SFTPBackup();

            // show Modal Dialog
            sftpBackup.ShowDialog(this);
        }

        private void buttonAutoBackup_Click(object sender, EventArgs e)
        {
            backupDataViaFTPToolStripMenuItem_Click(sender, e);
        }

        private void toolStripStatusLabelMain_TextChanged(object sender, EventArgs e)
        {
            toolStripTextBoxInformation.Text = toolStripStatusLabelMain.Text;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog(this);
        }

        private void encryptAESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EncryptAES encryptAES = new EncryptAES();
            encryptAES.ShowDialog(this);
        }
        
        #endregion

        #region Background worker

        private void backgroundWorkerStartupProcessing_DoWork(object sender, DoWorkEventArgs e)
        {
            // load all holidays for the current in cache, no more needed @ 25/11
            //LoadHolidaysInCache(DateTime.Today.Year);

            // reset all paths
            ResetPaths(DateTime.Today);

            // check if latest version running
            CheckVersion();

            // backup database
            BackupDB();

            // check if backup on SFTP needed
            BackupSFTP();
        }

        private void backgroundWorkerStartupProcessing_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // user state object sent by initiator of ReportProgress()
            var userState = e.UserState;

            switch (e.ProgressPercentage)
            {
                case 3:
                    CustomMessageBox cmb = (CustomMessageBox)userState;
                    // show Modal dialog
                    cmb.ShowDialog(this);
                    break;
                case 4:
                    this.Text = userState.ToString();
                    break;
                case 5:
                    textBoxVersion.Text = userState.ToString();
                    textBoxVersion.ForeColor = Color.Green;
                    textBoxVersion.BackColor = Color.White;
                    toolStripStatusLabelMain.Text = "";
                    break;
                case 7:
                    // if latest version no. is greater, display alert
                    textBoxVersion.Text = userState.ToString();
                    textBoxVersion.ForeColor = Color.Red;
                    // start blinking
                    timerVersion.Start();
                    break;
                case 10:
                    versionText = ((TextBox)userState).Text;
                    break;
                case 12:
                    if (MessageBox.Show(this,
                        "SFTP backup has not been done recently, it is recommended to do it now.\r\nDo you want to launch the auto-backup?",
                        "Auto-backup via SFTP",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                    {
                        toolStripStatusLabelMain.Text = "SFTP backup in progress, please wait ..";
                        SFTPBackup sftpBackup = (SFTPBackup)userState;
                        sftpBackup.ShowInTaskbar = true;

                        // show Modal Dialog
                        sftpBackup.Show();
                        sftpBackup.buttonAutoBackup.PerformClick();
                    }
                    else
                    {
                        SFTPBackup sftpBackup = (SFTPBackup)userState;
                        sftpBackup.IsUploadCompleted = true;
                    }
                    break;
                default:
                    toolStripStatusLabelMain.Text = userState.ToString();
                    break;
            }
            toolStripProgressBarMain.Value += 10;
        }

        private void backgroundWorkerStartupProcessing_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // worker thread completed now
            toolStripStatusLabelMain.Text = "Startup processing completed successfully @ " + DateTime.Now.ToShortTimeString();
            // set to 100%
            toolStripProgressBarMain.Value = toolStripProgressBarMain.Maximum;
        }

        #endregion
        
        private void BackupSFTP()
        {
            backgroundWorkerStartupProcessing.ReportProgress(11, "Checking if SFTP backup is required ..");
            var dataPath = Path.Combine(ConfigurationManager.AppSettings["Path.AppRepository"], "auto-backup");
            var minBackupDays = 3; // backup every 3 days

            // get all zip filnames from the src dir
            string[] zipNames = Directory.GetFiles(dataPath, "*.zip", System.IO.SearchOption.AllDirectories);
            var lastFilename = zipNames.LastOrDefault();

            if (!String.IsNullOrEmpty(lastFilename))
            {
                // check last date SFTP backup was done
                // split name - SAMPLE: backup_leaves_20131101.zip
                var filename = Path.GetFileNameWithoutExtension(lastFilename);
                var date = filename.Split('_').LastOrDefault();

                if (!String.IsNullOrEmpty(date))
                {
                    // parse date from filename ( not from Creation/Modified date)
                    DateTime dtLastBackup = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);

                    if (DateTime.Today.Subtract(dtLastBackup).Days > minBackupDays)
                    {
                        // do backup async ( outdated backup on SFTP)
                        DoSFTPBackupAsync();
                    }
                }
            }
            else
            {
                // do backup async ( never done before)
                DoSFTPBackupAsync();
            }
        }

        /// <summary>
        /// Do SFTP backup asynchronously
        /// </summary>
        private void DoSFTPBackupAsync()
        {
            SFTPBackup sftpBackup = new SFTPBackup();

            // do backup
            backgroundWorkerStartupProcessing.ReportProgress(12, sftpBackup);
            while (!sftpBackup.IsUploadCompleted)
            {
                // not yet completed, so make Worker thread to wait
                System.Threading.Thread.Sleep(500);
            }
        }

        private void publicHolidaysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PublicHol publicHol = new PublicHol();
            publicHol.Width = 390;
            publicHol.ShowDialog(this);
        }

        private void monthlyLeavesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MonthlyLeave monthlyLeave = new MonthlyLeave();
            monthlyLeave.ShowDialog(this);
        }

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ressourceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void buttonConsoRAF_Click(object sender, EventArgs e)
        {
            // disable all buttons
            buttonExportSPROD.Enabled = buttonGetRessources.Enabled = buttonConsoRAF.Enabled = false;
            
            // clear error messages
            textBoxLogSPROD.Clear();
            textBoxLogSPROD.ForeColor = Color.Green;
            LogMessage("=== START ===");

            ManipulateExcel excel = new ManipulateExcel();

            // data structure
            DataTable dt = new DataTable("ConsoRAF");
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Project", typeof(string));
            dt.Columns.Add("Days", typeof(float));
            
            // generate CSV file
            string consoRAFCSV = excel.GenerateSPRODCSV(
                checkedListBoxRessource, textBoxLogSPROD, true, true);

            //abuchoo;d;Vivop;Développement;VIVOP3 - ASPIN - G4R0C7 - LOT 6||VIVOP3-ASPIN-G4R0C7-LOT6;5;;
            foreach (var line in consoRAFCSV.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] subItems = line.Split(excel.CSVDelimiter[0]);                       

                // add new row to datatable
                DataRow dr = dt.NewRow();
                dr["Name"] = subItems[0];
                dr["Project"] = subItems[4].Split(new string[] { excel.CSVSubDelimiter }, StringSplitOptions.RemoveEmptyEntries)[1];
                dr["Days"] = subItems[5];

                dt.Rows.Add(dr);
            }

            ConsoRAF consoRAF = new ConsoRAF();

            consoRAF.dataGridViewConsoRAF.DataSource = dt;

            // show Modal Dialog
            consoRAF.ShowDialog(this);

            // enable button
            buttonGetRessources.Enabled = true;
        }

        private void checkBoxSelectAllSPROD_CheckedChanged(object sender, EventArgs e)
        {
            // select all items
            for (int i = 0; i < checkedListBoxRessource.Items.Count; i++)
            {
                checkedListBoxRessource.SetItemChecked(i, checkBoxSelectAllSPROD.Checked);
            }

            buttonExportSPROD.Enabled = buttonConsoRAF.Enabled = checkBoxSelectAllSPROD.Checked;
            if (checkBoxSelectAllSPROD.Checked)
            {
                checkBoxProjetsMOED.Checked = false;
            }
        }

        private void checkBoxProjetsMOED_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxProjetsMOED.Checked)
            {
                checkBoxSelectAllSPROD.Checked = false;
            }

            buttonExportSPROD.Enabled = buttonConsoRAF.Enabled = checkBoxProjetsMOED.Checked;

            // select all ressources in MOE-Déléguée projects (ASPI/SPID/SCOOP/SICLOP)
            for (int i = 0; i < checkedListBoxRessource.Items.Count; i++)
            {
                var filename = ((RessourceItem)checkedListBoxRessource.Items[i]).ToString().ToLower();

                if ((filename.Contains("buchoo")
                    || filename.Contains("foo")
                    || filename.Contains("maregadee")))
                {
                    checkedListBoxRessource.SetItemChecked(i, checkBoxProjetsMOED.Checked);
                }
            }
        }
    }

    #region RessourceItem class definition
    public class RessourceItem
    {
        string _text;
        string _value;

        public RessourceItem(string name, string filename)
        {
            _text = name;
            _value = filename;
        }

        public override string ToString()
        {
            return this._text;
        }

        public string GetFileName()
        {
            return this._value;
        }

        public void SetName(string name)
        {
            _text = name;
        }
    } 
    #endregion
}
