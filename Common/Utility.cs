using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Leaves_FAT_Management.Common
{
    public class Utility
    {
        // Used for Encryption and Decryption
        private static readonly string PasswordHash = "@st3kPa$$w0rd%";
        private static readonly string SaltKey = "f@TMgmT$@ltk3y";
        private static readonly string VIKey = "@1B2C3D4E5F6G7H8";

        // Date formatting
        public static readonly string HolidayImportDateFormat = @"(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)\s*(0?[1-9]|[1-2][0-9]|3[01])\s*(January|February|March|April|May|June|July|August|September|October|November|December)";

        /// <summary>
        /// Get the num. working days in a month
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetNoWorkingDays(DateTime dt)
        {
            int month = dt.Month;
            int year = dt.Year;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endEnd = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            int workingDays = 0;

            while (startDate <= endEnd)
            {
                if (startDate.DayOfWeek != DayOfWeek.Saturday
                    && startDate.DayOfWeek != DayOfWeek.Sunday) // not sat or sun
                {
                    workingDays++;
                }
                startDate = startDate.AddDays(1);
            }

            return workingDays;
        }

        /// <summary>
        /// Copy all files and sub-folders of a directory
        /// </summary>
        /// <param name="SourcePath"></param>
        /// <param name="DestinationPath"></param>
        /// <param name="overwriteexisting"></param>
        /// <returns></returns>
        public static bool CopyDirectory(string SourcePath, string DestinationPath, 
            bool overwriteexisting, ProgressBar progressBarFileTransfer)
        {
            bool ret = false;
            try
            {
                SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
                DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

                if (Directory.Exists(SourcePath))
                {
                    if (Directory.Exists(DestinationPath) == false)
                        Directory.CreateDirectory(DestinationPath);

                    foreach (string fls in Directory.GetFiles(SourcePath))
                    {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                        
                        progressBarFileTransfer.PerformStep();
                    }
                    foreach (string drs in Directory.GetDirectories(SourcePath))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (CopyDirectory(drs, DestinationPath + drinfo.Name, overwriteexisting, progressBarFileTransfer) == false)
                            ret = false;
                    }
                }
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.Message);
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// Open MS outlook window to send mail
        /// </summary>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="subject"></param>
        /// <param name="htmlBody"></param>
        /// <param name="attachments"></param>
        //public static void OpenOutlookMail(string to, string cc, string subject, string htmlBody, string[] attachments)
        //{
        //    // Create outlook application object.
        //    var outlookApplication = new Microsoft.Office.Interop.Outlook.Application();

        //    // Create mail message.
        //    var newMail = (Microsoft.Office.Interop.Outlook.MailItem)outlookApplication.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);

        //    newMail.To = to;
        //    newMail.CC = cc;
        //    newMail.Subject = subject;
        //    newMail.BodyFormat = Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatHTML;
        //    newMail.HTMLBody = htmlBody;            

        //    // add all attachments (if any)
        //    if (attachments != null)
        //    {
        //        foreach (string attachment in attachments)
        //        {
        //            newMail.Attachments.Add(attachment);
        //        }
        //    }

        //    // open mail window
        //    newMail.Display(false);
        //}

        /// <summary>
        /// Encrypt a string
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <returns>Encrypted text</returns>
        public static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        /// <summary>
        /// Decrypt an encrypted string
        /// </summary>
        /// <param name="encryptedText">Encrypted text to decrypt</param>
        /// <returns>Decrypted text</returns>
        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

        /// <summary>
        /// Get the Holiday object from text
        /// </summary>
        /// <param name="inputText">Text from MEF Online or gov.mu PDF document</param>
        /// <param name="year">Year for DB update</param>
        /// <returns>Holiday object ( nullable)</returns>
        public static Holiday GetHoliday(string inputText, int year)
        {
            Regex regex = new Regex(HolidayImportDateFormat);
            string holidayDate = String.Empty;
            Holiday holiday = null;
            var yearText = String.Concat(" ", year);

            if (regex.IsMatch(inputText))
            {
                // extract the holiday date from text
                holidayDate = regex.Match(inputText).Value;
                holidayDate = String.Concat(holidayDate, yearText);
            }

            if (!String.IsNullOrEmpty(holidayDate))
            {
                var holidayName = holidayDate.Replace(yearText, String.Empty);

                inputText = inputText.Replace(holidayName, String.Empty);
                
                // import from gov.mu
                var indexOfDash = inputText.LastIndexOf("-");
                if (indexOfDash > 0 && indexOfDash == inputText.Trim().Length - 1)
                {
                    // found last dash and is not *Eid-Ul-Fitr
                    inputText = inputText.Substring(0, indexOfDash);
                }

                // SAMPLE : Friday 9 August 2013
                holiday = new Holiday(
                    DateTime.ParseExact(holidayDate, "dddd d MMMM yyyy", CultureInfo.InvariantCulture),
                    year, inputText.Trim());
            }

            // nullable
            return holiday;
        }

        /// <summary>
        /// Holiday class definition
        /// </summary>
        public class Holiday
        {
            /// <summary>
            /// Holiday date
            /// </summary>
            public DateTime HolidayDate { get; set; }
            /// <summary>
            /// Holiday year ( for DB)
            /// </summary>
            public int HolidayYear { get; set; }
            /// <summary>
            /// Holiday name / description
            /// </summary>
            public string HolidayName { get; set; }
            /// <summary>
            /// Holiday ID (used only for UPDATE)
            /// </summary>
            public int HolidayID { get; set; }

            /// <summary>
            /// Overloaded constructor
            /// </summary>
            /// <param name="holidayDate">Holiday date</param>
            /// <param name="holidayYear">Holiday year ( for DB)</param>
            /// <param name="holidayName">Holiday name / description</param>
            public Holiday(DateTime holidayDate, int holidayYear, string holidayName)
            {
                HolidayDate = holidayDate;
                HolidayYear = holidayYear;
                HolidayName = holidayName;
                // default value
                HolidayID = -1;
            }
        }
    }
}
