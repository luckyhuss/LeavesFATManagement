namespace Leaves_FAT_Management.UI
{
    partial class ConsoRAF
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.dataGridViewConsoRAF = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConsoRAF)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(221, 342);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "C&lose";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonCreate
            // 
            this.buttonCreate.Enabled = false;
            this.buttonCreate.Location = new System.Drawing.Point(98, 342);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(75, 23);
            this.buttonCreate.TabIndex = 2;
            this.buttonCreate.Text = "&Copy";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // dataGridViewConsoRAF
            // 
            this.dataGridViewConsoRAF.AllowUserToAddRows = false;
            this.dataGridViewConsoRAF.AllowUserToDeleteRows = false;
            this.dataGridViewConsoRAF.AllowUserToOrderColumns = true;
            this.dataGridViewConsoRAF.AllowUserToResizeRows = false;
            this.dataGridViewConsoRAF.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewConsoRAF.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewConsoRAF.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewConsoRAF.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewConsoRAF.Name = "dataGridViewConsoRAF";
            this.dataGridViewConsoRAF.ReadOnly = true;
            this.dataGridViewConsoRAF.RowHeadersVisible = false;
            this.dataGridViewConsoRAF.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewConsoRAF.Size = new System.Drawing.Size(367, 324);
            this.dataGridViewConsoRAF.TabIndex = 6;
            this.dataGridViewConsoRAF.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridViewConsoRAF_DataBindingComplete);
            this.dataGridViewConsoRAF.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridViewConsoRAF_MouseUp);
            // 
            // ConsoRAF
            // 
            this.AcceptButton = this.buttonCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(393, 377);
            this.Controls.Add(this.dataGridViewConsoRAF);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonCreate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConsoRAF";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Conso / RAF (fin de mois)";
            this.Load += new System.EventHandler(this.ConsoRAF_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConsoRAF)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonCreate;
        public System.Windows.Forms.DataGridView dataGridViewConsoRAF;
    }
}