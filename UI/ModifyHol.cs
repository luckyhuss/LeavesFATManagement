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
using System.Windows.Forms;

namespace Leaves_FAT_Management.UI
{
    public partial class ModifyHol : Form
    {
        private Utility.Holiday original_holiday { set; get; }

        public ModifyHol(Utility.Holiday _holiday)
        {
            InitializeComponent();

            if (null != _holiday)
            {
                original_holiday = _holiday;
            }
            else
            {
                original_holiday = new Utility.Holiday(DateTime.Today, DateTime.Today.Year, String.Empty);
            }
        }

        private void ModifyHol_Load(object sender, EventArgs e)
        {
            if (original_holiday.HolidayID != -1)
            {
                // update form
                buttonAdd.Visible = false;
                buttonDelete.Visible = buttonUpdate.Visible = !buttonAdd.Visible;
            }
            else
            {
                // add form
                buttonAdd.Visible = true;
                buttonDelete.Visible = buttonUpdate.Visible = !buttonAdd.Visible;
            }

            if (null != original_holiday)
            {
                textBoxHolidayID.Text = original_holiday.HolidayID.ToString();
                textBoxHolidayName.Text = original_holiday.HolidayName;
                dateTimePickerHolidayDate.Value = original_holiday.HolidayDate;
            }
        }

        private void dateTimePickerHolidayDate_ValueChanged(object sender, EventArgs e)
        {
            textBoxYear.Text = dateTimePickerHolidayDate.Value.Year.ToString();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (original_holiday.HolidayID != -1 && 
                MessageBox.Show("Are you sure to delete?", "Delete holiday", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                // delete holiday entry
                Database.DeleteHoliday(original_holiday.HolidayID);

                this.Close();
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (null != original_holiday &&
                !String.IsNullOrEmpty(textBoxHolidayName.Text) && !String.IsNullOrEmpty(textBoxYear.Text))
            {
                original_holiday.HolidayDate = dateTimePickerHolidayDate.Value;
                original_holiday.HolidayYear = Convert.ToInt32(textBoxYear.Text);
                original_holiday.HolidayName =  textBoxHolidayName.Text.Trim();

                // update holiday entry
                Database.UpdateHoliday(original_holiday);

                this.Close();
            }            
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxHolidayName.Text) && !String.IsNullOrEmpty(textBoxYear.Text))
            {
                List<Utility.Holiday> listHoliday = new List<Utility.Holiday>();
                listHoliday.Add(new Utility.Holiday(
                    dateTimePickerHolidayDate.Value, Convert.ToInt32(textBoxYear.Text), textBoxHolidayName.Text.Trim()));

                // insert holiday entry
                Database.InsertHolidays(listHoliday);

                this.Close();
            }
        }
    }
}
