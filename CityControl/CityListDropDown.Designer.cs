namespace CityControl
{
    partial class CityListDropDown
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
            citiesDGV = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)citiesDGV).BeginInit();
            SuspendLayout();
            // 
            // citiesDGV
            // 
            citiesDGV.BackgroundColor = Color.PaleTurquoise;
            citiesDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            citiesDGV.Location = new Point(19, 20);
            citiesDGV.Name = "citiesDGV";
            citiesDGV.RowHeadersWidth = 82;
            citiesDGV.Size = new Size(401, 478);
            citiesDGV.TabIndex = 0;
            // 
            // CityListDropDown
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightCyan;
            Controls.Add(citiesDGV);
            Name = "CityListDropDown";
            Size = new Size(440, 522);
            ((System.ComponentModel.ISupportInitialize)citiesDGV).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView citiesDGV;
    }
}
