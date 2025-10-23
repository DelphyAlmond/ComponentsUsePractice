namespace ControlLib
{
    partial class ReferenceBookComponent
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
            DataGridView = new DataGridView();
            CityNameColumn = new DataGridViewTextBoxColumn();
            addBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)DataGridView).BeginInit();
            SuspendLayout();
            // 
            // DataGridView
            // 
            DataGridView.AllowUserToAddRows = false;
            DataGridView.AllowUserToDeleteRows = false;
            DataGridView.BackgroundColor = Color.LightCyan;
            DataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridView.Columns.AddRange(new DataGridViewColumn[] { CityNameColumn });
            DataGridView.GridColor = Color.Teal;
            DataGridView.Location = new Point(23, 24);
            DataGridView.Name = "DataGridView";
            DataGridView.RowHeadersVisible = false;
            DataGridView.RowHeadersWidth = 82;
            DataGridView.Size = new Size(470, 665);
            DataGridView.TabIndex = 0;
            // 
            // CityNameColumn
            // 

            // [ ! ] > CityNameColumn установлен в null или убран
            // если CityNameColumn был сгенерирован дизайнером с DataPropertyName = "ReferenceBookComponent",
            // то это неверно для BindingList<string>. Нужно исправить в дизайнере или коде.
            CityNameColumn.DataPropertyName = ""; // Или полностью удалить эту строку.
            CityNameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CityNameColumn.HeaderText = "Город";
            CityNameColumn.MinimumWidth = 10;
            CityNameColumn.Name = "CityNameColumn";
            // 
            // addBtn
            // 
            addBtn.Location = new Point(23, 695);
            addBtn.Name = "addBtn";
            addBtn.Size = new Size(470, 46);
            addBtn.TabIndex = 1;
            addBtn.Text = "Insert";
            addBtn.UseVisualStyleBackColor = true;
            addBtn.Click += addBtn_Click;
            // 
            // ReferenceBookComponent
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PaleTurquoise;
            Controls.Add(addBtn);
            Controls.Add(DataGridView);
            Name = "ReferenceBookComponent";
            Size = new Size(517, 760);
            ((System.ComponentModel.ISupportInitialize)DataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView DataGridView;
        private DataGridViewTextBoxColumn CityNameColumn;
        private Button addBtn;
    }
}
