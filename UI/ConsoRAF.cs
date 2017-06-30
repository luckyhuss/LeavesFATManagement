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
            dataGridViewConsoRAF.Columns["Days"].Width = 40;

            // sort by project
            dataGridViewConsoRAF.Sort(dataGridViewConsoRAF.Columns["Project"], ListSortDirection.Ascending);

            var previousVal = string.Empty;
            Color previousColor = Color.Empty;
            
            // change colors
            foreach (DataGridViewRow dgvr in dataGridViewConsoRAF.Rows)
            {
                // set font to white
                dgvr.DefaultCellStyle.ForeColor = Color.WhiteSmoke;

                if (previousVal.Equals(dgvr.Cells["Project"].Value))
                {
                    // get previous color
                    dgvr.DefaultCellStyle.BackColor = previousColor;
                }
                else
                {
                    // get a new color
                    previousColor = GetNewColor();
                    dgvr.DefaultCellStyle.BackColor = previousColor;
                }

                // set previous Project
                previousVal = dgvr.Cells["Project"].Value.ToString();
            }
        }

        Random r = new Random();

        private Color GetNewColor()
        {
            return Color.FromArgb(r.Next(50, 200), r.Next(50, 200), r.Next(50, 200));
        }

        private void dataGridViewConsoRAF_MouseUp(object sender, MouseEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            int columnIndex = -1;

            foreach (DataGridViewCell dgvc in dataGridViewConsoRAF.SelectedCells)
            {
                columnIndex = dgvc.ColumnIndex;
                switch (dgvc.ColumnIndex)
                {
                    case 0:
                        // generate comment for Excel RAF
                        sb.Append(dgvc.Value).AppendFormat(": {0}j", dataGridViewConsoRAF.Rows[dgvc.RowIndex].Cells[2].Value).AppendLine();
                        break;
                    case 2:
                        // add for Excel RAF
                        sb.Append(dgvc.Value).Append("+");
                        break;
                }
            }

            // if there's any content
            if (sb.Length > 0)
            {
                if (columnIndex == 2)
                {
                    sb = sb.Insert(0, "=");
                    sb = sb.Remove(sb.Length - 1, 1);
                }

                Clipboard.SetText(sb.ToString());
            }
        }
    }
}
