using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using System.ComponentModel;
using Leaves_FAT_Management.UI;
using Leaves_FAT_Management.Persistence;
//using OfficeOpenXml;
using System.IO;
using NPOI.SS.Util;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

namespace Leaves_FAT_Management.Core
{
    /// <summary>
    /// Class to represent complément d'info
    /// </summary>
    public class InfoComplement
    {
        public InfoComplement(string projectName, string projectCode, string lot, string complement)
        {
            ProjectName = projectName;
            ProjectCode = projectCode;
            Lot = lot;
            Complement = complement;
        }

        /// <summary>
        /// e.g. Vivop ou Vivop - Aspin
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// e.g. VIVOP FCI Lot 6 - G5R4
        /// </summary>
        public string ProjectCode { set; get; }

        /// <summary>
        /// e.g. Lot1, Lot2, Lot3, Lot4, Lot5, Lot6
        /// </summary>
        public string Lot { set; get; }

        /// <summary>
        /// e.g. VIVOP-VIO-LOT-5-IDEA-041428
        /// </summary>
        public string Complement { set; get; }
    }

    /// <summary>
    /// Encapsulator for Enums and extension method
    /// </summary>
    public static class EnumClass
    {
        public enum LMSLeave
        {
            [Description("annual-leave")]
            AnnualLeave,
            [Description("compensation")]
            Compensation,
            [Description("exam-leave")]
            ExamLeave,
            [Description("maternity-leave")]
            MaternityLeave,
            [Description("paternity-leaves")]
            PaternityLeave,
            [Description("special-leave")]
            SpecialLeave,
            [Description("sick-leave")]
            SickLeave,
            [Description("special-permission")]
            SpecialPermission
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            object[] attribs = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attribs.Length > 0)
            {
                return ((DescriptionAttribute)attribs[0]).Description;
            }
            return string.Empty;
        }
    }

    /// <summary>
    /// ManipulateExcel
    /// </summary>
    public class ManipulateExcel
    {
        private object MissValue = System.Reflection.Missing.Value;
        private string[] fatNameDelimiter = { "_" };

        // CSV manipulation
        private const string csvDelimiter = ";";
        private const string csvSubDelimiter = "||"; // "¤";
        private const char csvCongesDelimiter = ',';

        /// <summary>
        /// Constructor
        /// </summary>
        public ManipulateExcel()
        {            
            //start EXCEL and get application object
            //processId = 0;
            //processBefore = Process.GetProcessesByName("EXCEL");
            //// TODO : oExcelApp = new Excel.Application();
            //processAfter = Process.GetProcessesByName("EXCEL");
            
            //// Parcourir les process apres création pour retrouver celui correspondant à cette instance
            //processId = GetProcessIDExcel(processBefore, processAfter);
        }

        public string CSVDelimiter
        {
            get
            {
                return csvDelimiter;
            }
        }

        public string CSVSubDelimiter
        {
            get
            {
                return csvSubDelimiter;
            }
        }

        /// <summary>
        /// Créez un nouveau "WorkBook"
        /// </summary>
        public ISheet GetWorkBook(string sFilePath)
        {
            if (String.IsNullOrEmpty(sFilePath)) return null;

            HSSFWorkbook workbook = new HSSFWorkbook();
            using (FileStream file = new FileStream(sFilePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new HSSFWorkbook(file);
            }

            // get sheet from Excel file
            ISheet worksheet = workbook.GetSheet(ConfigurationManager.AppSettings["AnalyseWorksheet"]); // analyse sheet            

            return worksheet;
        }

        /// <summary>
        /// Get the project Lot from project name
        /// </summary>
        /// <param name="projetLotFAT"></param>
        /// <returns></returns>
        public InfoComplement GetInfoComplementFromProject(string username, string clientName, string projetLotFAT)
        {
            InfoComplement infoComplement = new InfoComplement(
                GetProjectNameByUsername(username, clientName.ToLower().Trim()),
                projetLotFAT.Trim(),
                projetLotFAT.ToLower(),
                projetLotFAT.Trim());
            
            string stringPattern = @"lot\s*\d"; // cater for whitespaces too

            if (System.Text.RegularExpressions.Regex.IsMatch(infoComplement.Lot, stringPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                // contains a lot
                infoComplement.Lot = System.Text.RegularExpressions.Regex.Match(infoComplement.Lot, stringPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase).ToString();
            }
            else
            {
                // default Lot
                infoComplement.Lot = "lot 6";
            }

            // manipulate the project lot
            infoComplement.Lot = infoComplement.Lot.Replace(" ", string.Empty);

            // up case first caracter (faster method than using string itself)
            char[] a = infoComplement.Lot.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            infoComplement.Lot = new string(a); // allocate only one string rather than 2

            // replace found lot by Lot and reset Complement
            infoComplement.Complement = System.Text.RegularExpressions.Regex.Replace(infoComplement.Complement, stringPattern, infoComplement.Lot, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // remove spaces
            infoComplement.Complement = infoComplement.Complement.Replace(" ", "-");
            // remove excess -
            infoComplement.Complement = infoComplement.Complement.Replace("----", "-");
            // remove excess -
            infoComplement.Complement = infoComplement.Complement.Replace("---", "-");
            // remove excess - and upper the text
            infoComplement.Complement = infoComplement.Complement.Replace("--", "-").ToUpper();

            return infoComplement;
        }

        /// <summary>
        /// Get the project name by username 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private string GetProjectNameByUsername(string username, string clientName)
        {
            // ABO : 24/05/2016
            var projectMemberAspin = ConfigurationManager.AppSettings["ProjectMember.Aspin"];
            var clientNameOther = ConfigurationManager.AppSettings["ClientName.Other"];            
            var projectName = projectMemberAspin.Contains(username) ? "Vivop - Aspin" : "~CLIENTNOTFOUND~";
            
            Dictionary<string, string> clientDict = new Dictionary<string,string>();

            foreach (var client in clientNameOther.Split(';'))
            {
                // each client correspondant
                clientDict.Add(client.Split('|')[0].ToLower(), client.Split('|')[1]);
            }

            if (clientDict.ContainsKey(clientName))
            {
                projectName = clientDict[clientName];
            }

            return projectName;
        }

        /// <summary>
        /// Get the activité from Lot
        /// </summary>
        /// <param name="projectLot"></param>
        /// <returns></returns>
        public string GetActiviteFromLot(string projectLot)
        {
            string activite = "Développement"; // Default activité

            // reset activité depending on Lot
            switch (projectLot.ToLower())
            {
                case "lot1":
                    activite = "Initialisation";
                    break;
                case "lot2":
                    activite = "Soutien";
                    break;
                case "lot3":
                    activite = "Maintenance";
                    break;
                case "lot4":
                    activite = "Etudes et Analyses";
                    break;
                case "lot5":
                    activite = "Evolution";
                    break;
                case "lot6":
                    activite = "Développement";
                    break;
            }

            return activite;
        }

        /// <summary>
        /// Retrait des Dates de congés du fichier FAT
        /// </summary>
        public string DateCongesInfoComplementaire(int iRowIndex, ISheet oExcelWrkSheetfat, bool sprodV2)
        {
            StringBuilder dateConges = new StringBuilder();
            string sFirstRow, sSecondRow;

            for (int iColIndex = 10; iColIndex <= 49; iColIndex++)
            {
                sFirstRow = GetCellValue(oExcelWrkSheetfat, null, iRowIndex, iColIndex);
                sSecondRow = GetCellValue(oExcelWrkSheetfat, null, iRowIndex + 1, iColIndex);

                if (sFirstRow != null && sFirstRow.Equals("x", StringComparison.InvariantCultureIgnoreCase))
                {                    
                    dateConges.Append(GetCellValue(oExcelWrkSheetfat, null, 15, iColIndex));
                    dateConges.Append(csvCongesDelimiter);
                }
                else if (sSecondRow != null && sSecondRow.Equals("x", StringComparison.InvariantCultureIgnoreCase))
                {
                    dateConges.Append(GetCellValue(oExcelWrkSheetfat, null, 15, iColIndex));
                    dateConges.Append(csvCongesDelimiter);
                }
            }

            var dateFinal = dateConges.ToString().TrimEnd(csvCongesDelimiter).Trim();

            if (sprodV2)
            {
                // new format info_complémentaire = C{start_date}:C{end_date}
                if (!string.IsNullOrEmpty(dateFinal))
                {
                    var values = dateFinal.Split(csvCongesDelimiter);

                    // format date for SPROD V2
                    dateFinal = String.Format("C{0}:C{1}", values[0], values[values.Length - 1]);
                }
            }

            return dateFinal;
        }

        /// <summary>
        /// Retrait du nombre totale de jours pour une activité 
        /// </summary>
        public string NombreDeJour(string sJour)
        {
            string sJourNombre = Convert.ToString(sJour).Replace("j", "").Trim();
            char[] cArray = sJourNombre.ToCharArray();

            for (int i = 0; i < cArray.Length; i++)
            {
                if (cArray[i] == '.')
                {
                    if (cArray[i + 1] == '0')
                    {
                        sJourNombre = "0";
                        break;
                    }
                }
            }
            return sJourNombre.Trim();
        }

        /// <summary>
        /// Retraite de l'index du row d'un 'cell' spécifique
        /// </summary>
        private int GetRowIndex(ISheet oExcelWrkSheet, string col, string sCriteria)
        {
            int iRowIndex = 0;
            
            for (int iRow = 1; iRow <= oExcelWrkSheet.LastRowNum; iRow++)
            {
                if (GetCellValue(oExcelWrkSheet, string.Concat(col, iRow.ToString())) == sCriteria)
                {
                    iRowIndex = iRow;
                    break;
                }
            }

            return iRowIndex + 1;
        }

        /// <summary>
        /// Génération du fichier CSV SPROD ( d'imputation) depuis les données du fichier FAT
        /// </summary>
        /// <param name="lstbxFileName">List des fichiers FAT trouvées sur \\merle</param>
        /// <param name="textBoxLogSPROD">Texte de log à mettre à jour pendant l'exécution</param>
        /// <returns></returns>
        public string GenerateSPRODCSV(CheckedListBox lstbxFileName, TextBox textBoxLogSPROD, bool sprodV2, bool consoRaf = false)
        {
            string excelCellMaladie = ConfigurationManager.AppSettings["ExcelCellMaladie"]; // Maladie
            string excelCellCongesPayes = ConfigurationManager.AppSettings["ExcelCellCongesPayes"]; // Congés payés        
            string excelCellCongesExceptionnels = ConfigurationManager.AppSettings["ExcelCellCongesExceptionnels"]; // Congés exceptionnels (à préciser)
            string excelCellNombreJour = ConfigurationManager.AppSettings["ExcelColumnSubTotal"]; // Sub total per project
            string excelCellFormation = ConfigurationManager.AppSettings["ExcelCellFormation"]; //Formation
            string excelCellProject = ConfigurationManager.AppSettings["ExcelCellProject"]; // Project name
            string excelCellClient = ConfigurationManager.AppSettings["ExcelCellClient"]; // Client name

            StringBuilder sbSPRODcsv = new StringBuilder();

            // load LDAP username
            Dictionary<string, string> ldapUsername = new Dictionary<string, string>();
            // select uppercased NAME and lowercase ldap_username
            ldapUsername = Database.SelectAllLDAPUsername();

            for (int itemCount = 0; itemCount < lstbxFileName.CheckedItems.Count; itemCount++)
            {
                RessourceItem selectedItem = (RessourceItem)lstbxFileName.CheckedItems[itemCount];

                string collaborateur = GetCollaborateur(selectedItem.ToString(), ldapUsername);

                ISheet oExcelWrkSheetfat = GetWorkBook(selectedItem.GetFileName());

                if (oExcelWrkSheetfat == null)
                {
                    // NPOI 22/06/17
                    throw new Exception("Excel worksheet 'analyse' not found");
                }

                #region SAMPLE CSV
                                
                //   abuchoo;d;Vivop;Evolution;VIVOP3 - ASPIN - G4R1C0 - LOT 5||VIVOP3-ASPIN-G4R1C0-LOT5;7;;
                //   abuchoo;d;Vivop;Développement;VIVOP3 - ASPIN - G4R0C7 - LOT 6||VIVOP3-ASPIN-G4R0C7-LOT6;5;;
                //   abuchoo;d;Vivop;Soutien;VIVOP3 DSM VASTA - Lot 2.3||VIVOP3-DSM-VASTA-LOT2.3;4;;
                //   abuchoo;d;Vivop;Soutien;VIVOP3 - SPID - LOT 2.1||VIVOP3-SPID-LOT2.1;0.5;;
                //   abuchoo;d;Vivop;Développement;VIVOP3 PILPRO G4R7 Lot6||VIVOP3-PILPRO-G4R7-LOT6;2.5;;
                //   abuchoo;a;annual-leave;C27:C28;2

                #endregion

                string cellValue;

                //Verifie les activités sur projet
                for (int j = 19; j < 52; j += 4)
                {
                    cellValue = GetCellValue(oExcelWrkSheetfat, string.Format(excelCellNombreJour, j));

                    if (!string.IsNullOrWhiteSpace(Convert.ToString(cellValue)))
                    {
                        if (NombreDeJour(cellValue) != "0")
                        {
                            // nom collaborateur
                            sbSPRODcsv.Append(collaborateur).Append(csvDelimiter);

                            // dossier
                            sbSPRODcsv.Append("d").Append(csvDelimiter);

                            // get the complement d'info by project name / lot from FAT
                            InfoComplement infoComplement = GetInfoComplementFromProject(
                                collaborateur,
                                GetCellValue(oExcelWrkSheetfat, string.Format(excelCellClient, j - 1)),
                                GetCellValue(oExcelWrkSheetfat, string.Format(excelCellProject, j - 1)));

                            // nom du projet ( base centralisée ASTEK pour V2)
                            if (sprodV2)
                            {
                                sbSPRODcsv.Append(infoComplement.ProjectName).Append(csvDelimiter);
                            }
                            else
                            {
                                sbSPRODcsv.Append("FT VIVOP").Append(csvDelimiter);
                            }

                            // type d'activité
                            sbSPRODcsv.Append(
                                GetActiviteFromLot(infoComplement.Lot)).Append(csvDelimiter);

                            // code projet¤info complémentaire (max. 2000 chars)                            
                            if (sprodV2)
                            {
                                sbSPRODcsv.Append(
                                    string.Concat(infoComplement.ProjectCode, csvSubDelimiter, infoComplement.Complement)).Append(csvDelimiter);
                            }
                            else
                            {
                                sbSPRODcsv.Append(infoComplement.Complement).Append(csvDelimiter);
                            }

                            // nombre de jours facturables
                            sbSPRODcsv.Append(
                                 NombreDeJour(GetCellValue(oExcelWrkSheetfat, string.Format(excelCellNombreJour, j)))).Append(csvDelimiter);

                            // support et mec => zéro
                            sbSPRODcsv.AppendLine(csvDelimiter);
                        }
                    }
                }

                if (!consoRaf)
                {
                    //Verifie les Absences et Formation
                    for (int i = 1; i <= oExcelWrkSheetfat.LastRowNum; i++)
                    {
                        cellValue = GetCellValue(oExcelWrkSheetfat, "B" + i.ToString());

                        if (!string.IsNullOrWhiteSpace(cellValue)
                            && NombreDeJour(GetCellValue(oExcelWrkSheetfat, string.Format(excelCellNombreJour, i))) != "0"
                            && (cellValue.Equals(excelCellFormation) || cellValue.Equals(excelCellMaladie)
                            || cellValue.Equals(excelCellCongesPayes) || cellValue.Equals(excelCellCongesExceptionnels)))
                        {
                            // nom collaborateur
                            sbSPRODcsv.Append(collaborateur).Append(csvDelimiter);

                            // formation / mec générale
                            if (cellValue.Equals(excelCellFormation))
                            {
                                // hors-dossier
                                sbSPRODcsv.Append("hd").Append(csvDelimiter);

                                // type de hors-dossier
                                sbSPRODcsv.Append("Formation Interne - A4").Append(csvDelimiter);

                                // type de formation
                                sbSPRODcsv.Append(GetCellValue(oExcelWrkSheetfat,
                                    string.Format("B{0}", GetRowIndex(oExcelWrkSheetfat, "B", "Formation")))).Append(csvDelimiter);
                            } // if formation
                            else if (cellValue.Equals(excelCellMaladie)
                                || cellValue.Equals(excelCellCongesPayes)
                                || cellValue.Equals(excelCellCongesExceptionnels))
                            {
                                // absence
                                sbSPRODcsv.Append("a").Append(csvDelimiter);

                                if (cellValue.Equals(excelCellMaladie))
                                {
                                    // type de congé = Maladie
                                    if (sprodV2)
                                    {
                                        sbSPRODcsv.Append(EnumClass.LMSLeave.SickLeave.GetDescription()).Append(csvDelimiter);
                                    }
                                    else
                                    {
                                        sbSPRODcsv.Append("Maladie - C12").Append(csvDelimiter);
                                    }

                                }
                                else if (cellValue.Equals(excelCellCongesPayes))
                                {
                                    // type de congé = Congé payé
                                    if (sprodV2)
                                    {
                                        sbSPRODcsv.Append(EnumClass.LMSLeave.AnnualLeave.GetDescription()).Append(csvDelimiter);
                                    }
                                    else
                                    {
                                        sbSPRODcsv.Append("Congé payé - C8").Append(csvDelimiter);
                                    }
                                }
                                else if (cellValue.Equals(excelCellCongesExceptionnels))
                                {
                                    // type de congé = Autres
                                    if (sprodV2)
                                    {
                                        //  by default it's Maternity. For a guy we got a problem Houston !
                                        sbSPRODcsv.Append(EnumClass.LMSLeave.SpecialLeave.GetDescription()).Append(csvDelimiter);
                                    }
                                    else
                                    {
                                        sbSPRODcsv.Append("Absence exceptionnelle - C1").Append(csvDelimiter);
                                    }
                                }

                                // dates de congés
                                sbSPRODcsv.Append(
                                    DateCongesInfoComplementaire(i - 1, oExcelWrkSheetfat, sprodV2)).Append(csvDelimiter);
                            } // else absence

                            // nombre de jours de formation
                            sbSPRODcsv.AppendLine(
                                 NombreDeJour(GetCellValue(oExcelWrkSheetfat, string.Format(excelCellNombreJour, i))));
                        }
                    }
                }

                // reset text to -> OK
                selectedItem.SetName(String.Format("{0} {1}", "OK ->", selectedItem.ToString()));
                int currentIndex = lstbxFileName.Items.IndexOf(selectedItem);
                LogMessage(
                    String.Format("{0:00}/{1:00} FAT extracted", currentIndex + 1, lstbxFileName.Items.Count),
                    textBoxLogSPROD);
                lstbxFileName.Items[currentIndex] = selectedItem;
            }

            string csvContent = sbSPRODcsv.ToString();
            csvContent = csvContent.Substring(0, csvContent.LastIndexOf("\r\n"));

            return csvContent;
        }

        /// <summary>
        /// Log message on screen
        /// </summary>
        /// <param name="messageToLog"></param>
        private void LogMessage(string messageToLog, TextBox textBoxLogSPROD)
        {
            // SFTP
            textBoxLogSPROD.Text += textBoxLogSPROD.Text.Length == 0 ? messageToLog : string.Concat("\r\n", messageToLog);
            //move the caret to the end of the text
            textBoxLogSPROD.SelectionStart = textBoxLogSPROD.TextLength;
            //scroll to the caret
            textBoxLogSPROD.ScrollToCaret();
            textBoxLogSPROD.Invalidate();
            Application.DoEvents(); // Releases the current thread back to windows form
        }

        /// <summary>
        /// Map name from FULLNAME to LDAP Username
        /// </summary>
        /// <param name="nomFAT"></param>
        /// <param name="ldapUsername"></param>
        /// <returns></returns>
        private string GetCollaborateur(string nomFAT, Dictionary<string, string> ldapUsername)
        {            
            string ldapName = string.Concat("error -> ", nomFAT);
            string surname = nomFAT.Split(fatNameDelimiter, StringSplitOptions.RemoveEmptyEntries)[0];
            // TODO : add forename too to the dictionary key in case surname might not be unique one day (12/12/2013)

            // get LDAP Username from database
            if (!string.IsNullOrEmpty(ldapUsername[surname]))
            {
                ldapName = ldapUsername[surname];
            }

            return ldapName;
        }

        public static string GetCellValue(ISheet worksheet, string cellReference, int row = 0, int col = 0)
        {
            CellReference cellRef = null;

            if (!string.IsNullOrEmpty(cellReference))
            {
                cellRef = new CellReference(cellReference);
            }
            else
            {
                cellRef = new CellReference(row, col);
            }

            if (worksheet.GetRow(cellRef.Row).GetCell(cellRef.Col) == null) return null; // test on null instead of empty because a cell can be empty

            string retVal = string.Empty;
            ICell cell = worksheet.GetRow(cellRef.Row).GetCell(cellRef.Col);

            switch (cell.CellType)
            {
                case CellType.Blank:
                case CellType.String:
                    retVal = cell.StringCellValue;
                    break;
                case CellType.Formula:
                    switch (cell.CachedFormulaResultType)
                    {
                        case CellType.Numeric:
                            retVal = cell.NumericCellValue.ToString();
                            break;
                        case CellType.Blank:
                        case CellType.String:
                            retVal = cell.StringCellValue;
                            break;
                        case CellType.Error:
                            retVal = "Error in formula cell ..";
                            break;
                    }
                    break;
                case CellType.Numeric:
                    retVal = cell.NumericCellValue.ToString();
                    break;
                case CellType.Error:
                    retVal = "Error in cell ..";
                    break;
                case CellType.Boolean:
                    retVal = cell.BooleanCellValue.ToString();
                    break;
            }

            return retVal;
        }

    }
}
