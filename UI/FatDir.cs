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
    public partial class FatDir : Form
    {
        public FatDir()
        {
            InitializeComponent();
        }

        private void FatDir_Load(object sender, EventArgs e)
        {
            numericUpDownYear.Value = DateTime.Today.Year + 1;
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            string srcFat = textBoxFATPath.Text.Trim();
            if (!String.IsNullOrEmpty(srcFat))
            {
                string subDirYear = Path.Combine(srcFat, numericUpDownYear.Value.ToString());

                // create Parent directory
                Directory.CreateDirectory(subDirYear);

                DateTime dtCurrentMonth = new DateTime((int)numericUpDownYear.Value, 1, 1);
                for (int i = 1; i <= 12; i++)
                {
                    // create Month subfolder
                    DirectoryInfo di = Directory.CreateDirectory(Path.Combine(subDirYear, dtCurrentMonth.ToString("yyyy-MM")));

                    // create Validées (inner) subfolder
                    Directory.CreateDirectory(Path.Combine(di.FullName, "Validées"));

                    // update Month
                    dtCurrentMonth = dtCurrentMonth.AddMonths(1); 
                }
                MessageBox.Show("Template created successfully", "Create FAT Template", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
