namespace Leaves_FAT_Management.UI
{
    partial class ModifyHol
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxHolidayID = new System.Windows.Forms.TextBox();
            this.textBoxHolidayName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxYear = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePickerHolidayDate = new System.Windows.Forms.DateTimePicker();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Holiday ID : ";
            // 
            // textBoxHolidayID
            // 
            this.textBoxHolidayID.Location = new System.Drawing.Point(100, 12);
            this.textBoxHolidayID.Name = "textBoxHolidayID";
            this.textBoxHolidayID.ReadOnly = true;
            this.textBoxHolidayID.Size = new System.Drawing.Size(75, 20);
            this.textBoxHolidayID.TabIndex = 1;
            // 
            // textBoxHolidayName
            // 
            this.textBoxHolidayName.Location = new System.Drawing.Point(100, 41);
            this.textBoxHolidayName.Name = "textBoxHolidayName";
            this.textBoxHolidayName.Size = new System.Drawing.Size(204, 20);
            this.textBoxHolidayName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Holiday Name : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Holiday Date : ";
            // 
            // textBoxYear
            // 
            this.textBoxYear.Location = new System.Drawing.Point(100, 95);
            this.textBoxYear.Name = "textBoxYear";
            this.textBoxYear.ReadOnly = true;
            this.textBoxYear.Size = new System.Drawing.Size(75, 20);
            this.textBoxYear.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Holiday Year : ";
            // 
            // dateTimePickerHolidayDate
            // 
            this.dateTimePickerHolidayDate.CustomFormat = "(ddd) dd MMM yyyy";
            this.dateTimePickerHolidayDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerHolidayDate.Location = new System.Drawing.Point(100, 67);
            this.dateTimePickerHolidayDate.Name = "dateTimePickerHolidayDate";
            this.dateTimePickerHolidayDate.Size = new System.Drawing.Size(139, 20);
            this.dateTimePickerHolidayDate.TabIndex = 8;
            this.dateTimePickerHolidayDate.ValueChanged += new System.EventHandler(this.dateTimePickerHolidayDate_ValueChanged);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(83, 145);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 9;
            this.buttonUpdate.Text = "&Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(164, 145);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 10;
            this.buttonDelete.Text = "&Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(118, 174);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 11;
            this.buttonAdd.Text = "&Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // ModifyHol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 209);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.dateTimePickerHolidayDate);
            this.Controls.Add(this.textBoxYear);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxHolidayName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxHolidayID);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModifyHol";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Modify / DEL Holiday";
            this.Load += new System.EventHandler(this.ModifyHol_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxHolidayID;
        private System.Windows.Forms.TextBox textBoxHolidayName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxYear;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePickerHolidayDate;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
    }
}