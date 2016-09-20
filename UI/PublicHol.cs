using Leaves_FAT_Management.Common;
using Leaves_FAT_Management.Core;
using Leaves_FAT_Management.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Leaves_FAT_Management.UI
{
    public partial class PublicHol : Form
    {
        public PublicHol()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            this.Width += 210;
            buttonImport.Enabled = false;
            this.AcceptButton = null; // reset to null to be able to type in multiline textbox ( CR/LF)
        }

        private void PublicHol_Load(object sender, EventArgs e)
        {
            numericUpDownYear.Value = DateTime.Today.Year;
            buttonValidate.Text = String.Format(buttonValidate.Tag.ToString(), numericUpDownYear.Value);

            // bind all grids
            BindGridViews();

        }

        /// <summary>
        /// Bind all grids
        /// </summary>
        private void BindGridViews()
        {
            // Databind holiday
            dataGridViewHoliday.DataSource = Database.SelectHolidayByYear(Convert.ToInt32(numericUpDownYear.Value)).Tables[0];

            var lossHoliday = 0;
            var totalHoliday = dataGridViewHoliday.Rows.Count;

            // change grid colors
            for (int countRow = 0; countRow < totalHoliday; countRow++)
            {
                if (null == dataGridViewHoliday.Rows[countRow].Cells["Date"].Value) return;

                var holidayDate = dataGridViewHoliday.Rows[countRow].Cells[1].Value.ToString();
                if (holidayDate.Contains("Sat") || holidayDate.Contains("Sun"))
                {
                    dataGridViewHoliday.Rows[countRow].DefaultCellStyle.ForeColor = Color.Red;
                    lossHoliday++;
                }
                else
                {
                    dataGridViewHoliday.Rows[countRow].DefaultCellStyle.BackColor = Color.Green;
                    dataGridViewHoliday.Rows[countRow].DefaultCellStyle.ForeColor = Color.White;
                }
            }

            // update holiday counts
            labelHolidayCount.Text = String.Format("{0} / {1} holidays ({2}%)",
                totalHoliday - lossHoliday, totalHoliday,
                totalHoliday > 0 ? (((totalHoliday - lossHoliday) * 100) / totalHoliday) : 0); // cater for divide-by-zero exception

            // reset default columns size or hide
            // column Name
            dataGridViewHoliday.Columns[0].Width = 190;
            // column Date
            dataGridViewHoliday.Columns[1].Width = 110;
            // column HolidayID
            dataGridViewHoliday.Columns[2].Visible = false;
        }

        private void numericUpDownYear_ValueChanged(object sender, EventArgs e)
        {
            buttonValidate.Text = String.Format(buttonValidate.Tag.ToString(), numericUpDownYear.Value);

            // bind all grids
            BindGridViews();
        }

        private void textBoxHolidayImport_TextChanged(object sender, EventArgs e)
        {
            textBoxHolidayImport.Text = textBoxHolidayImport.Text.Replace(".", String.Empty);
        }

        private void buttonValidate_Click(object sender, EventArgs e)
        {
            int holidayReplaced = -1;

            if (String.IsNullOrWhiteSpace(textBoxHolidayImport.Text))
            {
                MessageBox.Show(this, "Please type data to input in the textbox", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dataGridViewHoliday.Rows.Count > 0)
            {
                // already imported for this year
                if (MessageBox.Show(this, "Holidays have already been imported for this year.\nDo you want to delete and replace data?",
                    "Confirm replacement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                else
                {
                    // delete data from database and re-import new data
                    holidayReplaced = Database.DeletetHolidayByYear(Convert.ToInt32(numericUpDownYear.Value));
                }
            }

            var holidaysToImport = textBoxHolidayImport.Text.Replace(".", String.Empty);
            var firstPartHolidayName = String.Empty;
            List<Utility.Holiday> listHoliday = new List<Utility.Holiday>();

            foreach (var s in holidaysToImport.Split(Environment.NewLine.ToCharArray()))
            {
                // check if input text contains valid date
                Regex regex = new Regex(Utility.HolidayImportDateFormat);

                if (!String.IsNullOrEmpty(s))
                {
                    if (regex.IsMatch(s))
                    {
                        var holidayData = s;
                        // check if this is the second part of holiday name
                        if (!String.IsNullOrEmpty(firstPartHolidayName))
                        {
                            holidayData = String.Concat(firstPartHolidayName, " ", holidayData);
                        }

                        Utility.Holiday holiday = Utility.GetHoliday(holidayData, Convert.ToInt32(numericUpDownYear.Value));
                        // add to list of holidays
                        listHoliday.Add(holiday);
                        firstPartHolidayName = String.Empty;
                    } // if Regex match
                    else
                    {
                        // contains first part of holiday name ( MEF Online PDF document)
                        // SAMPLE : 
                        //  Arrival of Indentured 
                        //  Labourers ... ................................. Saturday 2 November
                        //  Assumption of the Blessed
                        //  Virgin Mary ... ............................... Friday 15 August

                        // save first part of holiday name for later use
                        firstPartHolidayName = s;
                    } // else if Regex match
                } // if s is not null
            } // loop holidays

            if (listHoliday.Count > 0)
            {
                // holiday list is OK
                int insertCount = Database.InsertHolidays(listHoliday);

                if (insertCount > 0)
                {
                    if (holidayReplaced == -1)
                    {
                        MessageBox.Show("Holidays updated successfully. Total lines inserted : " + insertCount.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Holidays replaced successfully. Total lines deleted : " + holidayReplaced + "\nTotal lines inserted : " + insertCount.ToString());
                    }
                    textBoxHolidayImport.Clear();
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            // show dialog
            ModifyHol modifyHol = new ModifyHol(null);
            modifyHol.ShowDialog(this);
        }

        private void buttonModifyDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewHoliday.SelectedRows.Count == 1)
            {
                DataGridViewRow dgvr = dataGridViewHoliday.SelectedRows[0];
                if (null != dgvr.Cells[2].Value)
                {
                    DateTime holidayDate = DateTime.ParseExact(dgvr.Cells[1].Value.ToString(), "(ddd) dd MMM yyyy", null);

                    Utility.Holiday holiday = new Utility.Holiday(holidayDate, holidayDate.Year, dgvr.Cells[0].Value.ToString());
                    holiday.HolidayID = (int)dgvr.Cells[2].Value;

                    // show dialog
                    ModifyHol modifyHol = new ModifyHol(holiday);
                    modifyHol.ShowDialog(this);
                }
            }
            else
            {
                MessageBox.Show("Select a holiday first", "Modify/DEL holiday", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return;
            //// 


            //else
            //    if (idLeave.Count == 0)
            //    {

            //    }        
        }

        private void dataGridViewHoliday_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridViewHoliday.SelectedRows.Count == 1)
            {
                buttonModifyDelete_Click(sender, e);
            }
        }
    }
}
