using Leaves_FAT_Management.Core;
using Leaves_FAT_Management.Persistence;
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
    public partial class MonthlyLeave : Form
    {
        private const char delimiterDateType = '¤';
        private const char delimiterValue = ',';

        public MonthlyLeave()
        {
            InitializeComponent();
        }

        private void MonthlyLeave_Load(object sender, EventArgs e)
        {
            dateTimePickerLeaveMonth.Value = DateTime.Today;
        }

        /// <summary>
        /// Bind all grids
        /// </summary>
        private void BindGridView()
        {
            var from = DateTime.Parse(string.Format("{0}/{1}/{2}", 1, dateTimePickerLeaveMonth.Value.Month, dateTimePickerLeaveMonth.Value.Year));
            var to = DateTime.Parse(string.Format("{0}/{1}/{2}", 
                DateTime.DaysInMonth(dateTimePickerLeaveMonth.Value.Year, dateTimePickerLeaveMonth.Value.Month), dateTimePickerLeaveMonth.Value.Month, dateTimePickerLeaveMonth.Value.Year));
            
            // monthly leave
            var monthlyLeave = Database.SelectMonthlyLeave(from, to).Tables[0];
            
            // process all leaves into a "flat" calendar view
            var consolidatedLeave = new DataTable();

            Dictionary<string, string> allLeaves = new Dictionary<string, string>();
            foreach (DataRow row in monthlyLeave.Rows)
            {
                if (!allLeaves.ContainsKey(row[0].ToString()))
                {
                    // add unique entry for Ressource
                    allLeaves.Add(row[0].ToString(), String.Concat((Convert.ToDateTime(row[1])).Day, delimiterDateType, row[2]));
                }
                else
                {
                    // update entry's value
                    allLeaves[row[0].ToString()] = String.Concat(allLeaves[row[0].ToString()], delimiterValue,
                        String.Concat((Convert.ToDateTime(row[1])).Day, delimiterDateType, row[2]));
                }
            }

            int numberOfDays = DateTime.DaysInMonth(dateTimePickerLeaveMonth.Value.Year, dateTimePickerLeaveMonth.Value.Month);

            consolidatedLeave.Columns.Add("Name");
            for (int i = 1; i <= numberOfDays; i++)
            {
                var day = DateTime.Parse(string.Format("{0}/{1}/{2}", 
                    i, dateTimePickerLeaveMonth.Value.Month, dateTimePickerLeaveMonth.Value.Year));

                var columnName = String.Format("{0} {1}", i, day.DayOfWeek.ToString().Substring(0, 1));
                // add columns
                consolidatedLeave.Columns.Add(columnName);
            }

            foreach (var keyValuePair in allLeaves)
            {
                DataRow dr = consolidatedLeave.NewRow();
                dr[0] = keyValuePair.Key;

                foreach (var leaveDates in keyValuePair.Value.Split(delimiterValue))
                {
                    var leaveDateType = leaveDates.Split(delimiterDateType);
                    dr[Convert.ToInt32(leaveDateType[0])] = leaveDateType[1];
                }

                consolidatedLeave.Rows.Add(dr);
            }

            dataGridViewLeave.DataSource = consolidatedLeave;
            dataGridViewLeave.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewLeave.Columns[0].Width = 120;
            
            // weekends
            foreach (DataGridViewColumn column in dataGridViewLeave.Columns)
            {
                // weekend ( Saturday or Sunday)
                if (column.HeaderText.Contains("S"))
                {
                    column.DefaultCellStyle.BackColor = Color.Gray;
                }
            }

            // get Public Holidays for the current month
            var mainForm = this.Owner as UI.Main;
            List<DateTime> holidaysInCurrentMonth = mainForm.GetHolidaysByMonth(from, to);

            foreach (var dateHol in holidaysInCurrentMonth)
            {
                var columnName = String.Format("{0} {1}", dateHol.Day, dateHol.DayOfWeek.ToString().Substring(0, 1));

                dataGridViewLeave.Columns[columnName].DefaultCellStyle.BackColor = Color.Green;
                dataGridViewLeave.Columns[columnName].DefaultCellStyle.ForeColor = Color.White;

                // update all rows
                foreach (DataGridViewRow dr in dataGridViewLeave.Rows)
                {
                    dr.Cells[columnName].Value = "P";
                }
            }
        }

        private void dateTimePickerLeaveMonth_ValueChanged(object sender, EventArgs e)
        {
            // To prevent Error : Year, Month, and Day parameters describe an un-representable DateTime
            var dtp = (DateTimePicker)sender;
            dtp.Value = new DateTime(dtp.Value.Year, dtp.Value.Month, 1);

            BindGridView();
        }
    }
}
