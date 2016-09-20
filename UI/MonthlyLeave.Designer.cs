namespace Leaves_FAT_Management.UI
{
    partial class MonthlyLeave
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePickerLeaveMonth = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewLeave = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLeave)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Month :";
            // 
            // dateTimePickerLeaveMonth
            // 
            this.dateTimePickerLeaveMonth.CustomFormat = "MMM yyyy";
            this.dateTimePickerLeaveMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerLeaveMonth.Location = new System.Drawing.Point(59, 6);
            this.dateTimePickerLeaveMonth.Name = "dateTimePickerLeaveMonth";
            this.dateTimePickerLeaveMonth.ShowUpDown = true;
            this.dateTimePickerLeaveMonth.Size = new System.Drawing.Size(83, 20);
            this.dateTimePickerLeaveMonth.TabIndex = 6;
            this.dateTimePickerLeaveMonth.Value = new System.DateTime(2011, 7, 1, 0, 0, 0, 0);
            this.dateTimePickerLeaveMonth.ValueChanged += new System.EventHandler(this.dateTimePickerLeaveMonth_ValueChanged);
            // 
            // dataGridViewLeave
            // 
            this.dataGridViewLeave.AllowUserToAddRows = false;
            this.dataGridViewLeave.AllowUserToDeleteRows = false;
            this.dataGridViewLeave.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewLeave.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewLeave.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLeave.Location = new System.Drawing.Point(15, 32);
            this.dataGridViewLeave.Name = "dataGridViewLeave";
            this.dataGridViewLeave.ReadOnly = true;
            this.dataGridViewLeave.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewLeave.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewLeave.Size = new System.Drawing.Size(774, 368);
            this.dataGridViewLeave.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label1.Location = new System.Drawing.Point(148, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(546, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Legend : S = Sick, L = Local, C = Compensation, M = Maternity, P = Public Holiday" +
    ", A = Autre Projet, F = Formation";
            // 
            // MonthlyLeave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 412);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewLeave);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dateTimePickerLeaveMonth);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MonthlyLeave";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Monthly Leaves of Ressources";
            this.Load += new System.EventHandler(this.MonthlyLeave_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLeave)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateTimePickerLeaveMonth;
        private System.Windows.Forms.DataGridView dataGridViewLeave;
        private System.Windows.Forms.Label label1;
    }
}