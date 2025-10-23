namespace ControlLib
{
    partial class OrderReportComponent
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            dateTimePicker = new DateTimePicker();
            generateReportButton = new Button();
            dataGridView = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // dateTimePicker
            // 
            dateTimePicker.Location = new Point(30, 26);
            dateTimePicker.Name = "dateTimePicker1";
            dateTimePicker.Size = new Size(680, 39);
            dateTimePicker.TabIndex = 0;
            // 
            // makeReportBtn
            // 
            generateReportButton.Location = new Point(30, 83);
            generateReportButton.Name = "makeReportBtn";
            generateReportButton.Size = new Size(680, 46);
            generateReportButton.TabIndex = 1;
            generateReportButton.Text = "confirm report";
            generateReportButton.UseVisualStyleBackColor = true;
            // 
            // dataGridView
            // 
            dataGridView.BackgroundColor = Color.PaleTurquoise;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(30, 151);
            dataGridView.Name = "dataGridView1";
            dataGridView.RowHeadersWidth = 82;
            dataGridView.Size = new Size(680, 625);
            dataGridView.TabIndex = 2;
            // 
            // OrderReportComponent
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightCyan;
            Controls.Add(dataGridView);
            Controls.Add(generateReportButton);
            Controls.Add(dateTimePicker);
            Name = "OrderReportComponent";
            Size = new Size(739, 808);
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DateTimePicker dateTimePicker;
        private Button generateReportButton;
        private DataGridView dataGridView;
    }
}
