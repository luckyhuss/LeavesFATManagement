using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Leaves_FAT_Management.UI
{
    public partial class ConsoRAF : Form
    {
        public ConsoRAF()
        {
            InitializeComponent();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConsoRAF_Load(object sender, EventArgs e)
        {

        }

        private void dataGridViewConsoRAF_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // resize columns
            dataGridViewConsoRAF.Columns["Name"].Width = 100;
            dataGridViewConsoRAF.Columns["Project"].Width = 200;
            dataGridViewConsoRAF.Columns["Days"].Width = 50;

            // sort by project
            dataGridViewConsoRAF.Sort(dataGridViewConsoRAF.Columns["Project"], ListSortDirection.Ascending);

            var previousVal = string.Empty;
            Color previousColor = Color.Empty;

            // change colors
            foreach (DataGridViewRow dgvr in dataGridViewConsoRAF.Rows)
            {
                if (previousVal.Equals(dgvr.Cells["Project"].Value))
                {
                    // get previous color
                    dgvr.DefaultCellStyle.ForeColor = previousColor;
                }
                else
                {
                    // get a new color
                    previousColor = GetNewColor();
                    dgvr.DefaultCellStyle.ForeColor = previousColor;
                }

                // set previous Project
                previousVal = dgvr.Cells["Project"].Value.ToString();
            }
        }

        Random r = new Random();

        private Color GetNewColor()
        {
            return Color.FromArgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));
        }
    }
}
