namespace ControlLib
{
    partial class OrderManagementComponent
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
            components = new System.ComponentModel.Container();
            customqListComponent1 = new ComponentLib.CustomQListComponent();
            CMenuOperations = new ContextMenuStrip(components);
            addToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            CMenuOperations.SuspendLayout();
            SuspendLayout();
            // 
            // customqListComponent1
            // 
            customqListComponent1.Dock = DockStyle.Bottom;
            customqListComponent1.Location = new Point(0, 58);
            customqListComponent1.Name = "customqListComponent1";
            customqListComponent1.Size = new Size(542, 826);
            customqListComponent1.TabIndex = 0;
            // 
            // CMenuOperations
            // 
            CMenuOperations.BackColor = Color.LightCyan;
            CMenuOperations.ImageScalingSize = new Size(32, 32);
            CMenuOperations.Items.AddRange(new ToolStripItem[] { addToolStripMenuItem, editToolStripMenuItem, deleteToolStripMenuItem });
            CMenuOperations.Name = "ContextMenuStripC2";
            CMenuOperations.Size = new Size(159, 118);
            CMenuOperations.Text = "operations";
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            addToolStripMenuItem.Size = new Size(158, 38);
            addToolStripMenuItem.Text = "Add";
            addToolStripMenuItem.Click += (s, e) => AddNewOrder();
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.U;
            editToolStripMenuItem.Size = new Size(158, 38);
            editToolStripMenuItem.Text = "Edit";
            editToolStripMenuItem.Click += (s, e) => EditSelectedOrder();
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D;
            deleteToolStripMenuItem.Size = new Size(158, 38);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += (s, e) => DeleteSelectedOrder();
            // 
            // OrderManagementComponent
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightCyan;
            Controls.Add(customqListComponent1);
            Name = "OrderManagementComponent";
            Size = new Size(542, 884);
            CMenuOperations.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ComponentLib.CustomQListComponent customqListComponent1;
        private ContextMenuStrip CMenuOperations;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
    }
}
