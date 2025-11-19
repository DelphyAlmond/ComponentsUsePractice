namespace ReportsControl
{
    partial class ReportDataGridView
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView = new DataGridView();
            genBtn = new Button();
            cityFilterCB = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.BackgroundColor = Color.PaleTurquoise;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(18, 77);
            dataGridView.Name = "dataGridView";
            dataGridView.RowHeadersWidth = 82;
            dataGridView.Size = new Size(1426, 485);
            dataGridView.TabIndex = 0;
            // 
            // genBtn
            // 
            genBtn.Location = new Point(487, 19);
            genBtn.Name = "genBtn";
            genBtn.Size = new Size(434, 46);
            genBtn.TabIndex = 1;
            genBtn.Text = "Make report";
            genBtn.UseVisualStyleBackColor = true;
            genBtn.Click += buttonGenerate_Click;
            // 
            // cityFilterCB
            // 
            cityFilterCB.FormattingEnabled = true;
            cityFilterCB.Location = new Point(18, 23);
            cityFilterCB.Name = "cityFilterCB";
            cityFilterCB.Size = new Size(463, 40);
            cityFilterCB.TabIndex = 2;
            // 
            // ReportDataGridView
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightCyan;
            Controls.Add(cityFilterCB);
            Controls.Add(genBtn);
            Controls.Add(dataGridView);
            Name = "ReportDataGridView";
            Size = new Size(1463, 581);
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView;
        private Button genBtn;
        private ComboBox cityFilterCB;
    }
}
