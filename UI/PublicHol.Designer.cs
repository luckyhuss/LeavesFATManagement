namespace Leaves_FAT_Management.UI
{
    partial class PublicHol
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
            this.buttonImport = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownYear = new System.Windows.Forms.NumericUpDown();
            this.textBoxHolidayImport = new System.Windows.Forms.TextBox();
            this.buttonValidate = new System.Windows.Forms.Button();
            this.textBoxDateFormat = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridViewHoliday = new System.Windows.Forms.DataGridView();
            this.labelHolidayCount = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonModifyDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHoliday)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonImport
            // 
            this.buttonImport.Location = new System.Drawing.Point(279, 4);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(83, 23);
            this.buttonImport.TabIndex = 0;
            this.buttonImport.Text = "&Import >>";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(287, 427);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "&Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Year : ";
            // 
            // numericUpDownYear
            // 
            this.numericUpDownYear.Location = new System.Drawing.Point(56, 4);
            this.numericUpDownYear.Maximum = new decimal(new int[] {
            2070,
            0,
            0,
            0});
            this.numericUpDownYear.Minimum = new decimal(new int[] {
            2010,
            0,
            0,
            0});
            this.numericUpDownYear.Name = "numericUpDownYear";
            this.numericUpDownYear.ReadOnly = true;
            this.numericUpDownYear.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownYear.TabIndex = 3;
            this.numericUpDownYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownYear.Value = new decimal(new int[] {
            2030,
            0,
            0,
            0});
            this.numericUpDownYear.ValueChanged += new System.EventHandler(this.numericUpDownYear_ValueChanged);
            // 
            // textBoxHolidayImport
            // 
            this.textBoxHolidayImport.Location = new System.Drawing.Point(389, 46);
            this.textBoxHolidayImport.Multiline = true;
            this.textBoxHolidayImport.Name = "textBoxHolidayImport";
            this.textBoxHolidayImport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxHolidayImport.Size = new System.Drawing.Size(183, 375);
            this.textBoxHolidayImport.TabIndex = 4;
            this.textBoxHolidayImport.TextChanged += new System.EventHandler(this.textBoxHolidayImport_TextChanged);
            // 
            // buttonValidate
            // 
            this.buttonValidate.Location = new System.Drawing.Point(423, 427);
            this.buttonValidate.Name = "buttonValidate";
            this.buttonValidate.Size = new System.Drawing.Size(113, 23);
            this.buttonValidate.TabIndex = 5;
            this.buttonValidate.Tag = "&Validate for {0}";
            this.buttonValidate.Text = "&Validate for {0}";
            this.buttonValidate.UseVisualStyleBackColor = true;
            this.buttonValidate.Click += new System.EventHandler(this.buttonValidate_Click);
            // 
            // textBoxDateFormat
            // 
            this.textBoxDateFormat.Location = new System.Drawing.Point(389, 20);
            this.textBoxDateFormat.Name = "textBoxDateFormat";
            this.textBoxDateFormat.ReadOnly = true;
            this.textBoxDateFormat.Size = new System.Drawing.Size(183, 20);
            this.textBoxDateFormat.TabIndex = 6;
            this.textBoxDateFormat.Text = "National Day Tuesday 12 March";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(386, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Sample data format for import :";
            // 
            // dataGridViewHoliday
            // 
            this.dataGridViewHoliday.AllowUserToAddRows = false;
            this.dataGridViewHoliday.AllowUserToDeleteRows = false;
            this.dataGridViewHoliday.AllowUserToResizeColumns = false;
            this.dataGridViewHoliday.AllowUserToResizeRows = false;
            this.dataGridViewHoliday.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewHoliday.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewHoliday.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHoliday.Location = new System.Drawing.Point(12, 30);
            this.dataGridViewHoliday.MultiSelect = false;
            this.dataGridViewHoliday.Name = "dataGridViewHoliday";
            this.dataGridViewHoliday.Size = new System.Drawing.Size(350, 391);
            this.dataGridViewHoliday.TabIndex = 8;
            this.dataGridViewHoliday.DoubleClick += new System.EventHandler(this.dataGridViewHoliday_DoubleClick);
            // 
            // labelHolidayCount
            // 
            this.labelHolidayCount.AutoSize = true;
            this.labelHolidayCount.Location = new System.Drawing.Point(123, 6);
            this.labelHolidayCount.Name = "labelHolidayCount";
            this.labelHolidayCount.Size = new System.Drawing.Size(41, 13);
            this.labelHolidayCount.TabIndex = 9;
            this.labelHolidayCount.Text = "Count :";
            // 
            // buttonAdd
            // 
            this.buttonAdd.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonAdd.Location = new System.Drawing.Point(12, 427);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 10;
            this.buttonAdd.Text = "&Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonModifyDelete
            // 
            this.buttonModifyDelete.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonModifyDelete.Location = new System.Drawing.Point(93, 427);
            this.buttonModifyDelete.Name = "buttonModifyDelete";
            this.buttonModifyDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonModifyDelete.TabIndex = 11;
            this.buttonModifyDelete.Text = "&Modify/DEL";
            this.buttonModifyDelete.UseVisualStyleBackColor = true;
            this.buttonModifyDelete.Click += new System.EventHandler(this.buttonModifyDelete_Click);
            // 
            // PublicHol
            // 
            this.AcceptButton = this.buttonImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(584, 462);
            this.Controls.Add(this.buttonModifyDelete);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.labelHolidayCount);
            this.Controls.Add(this.dataGridViewHoliday);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxDateFormat);
            this.Controls.Add(this.buttonValidate);
            this.Controls.Add(this.textBoxHolidayImport);
            this.Controls.Add(this.numericUpDownYear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonImport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PublicHol";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Public Holidays";
            this.Load += new System.EventHandler(this.PublicHol_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHoliday)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownYear;
        private System.Windows.Forms.TextBox textBoxHolidayImport;
        private System.Windows.Forms.Button buttonValidate;
        private System.Windows.Forms.TextBox textBoxDateFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridViewHoliday;
        private System.Windows.Forms.Label labelHolidayCount;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonModifyDelete;
    }
}