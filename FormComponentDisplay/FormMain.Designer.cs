namespace FormComponentDisplay
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControls = new TabControl();
            menuStrip = new MenuStrip();
            DirectoriesToolStripMenuItem = new ToolStripMenuItem();
            ReportsToolStripMenuItem = new ToolStripMenuItem();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // tabControls
            // 
            tabControls.Dock = DockStyle.Fill;
            tabControls.Location = new Point(0, 40);
            tabControls.Name = "tabControls";
            tabControls.SelectedIndex = 0;
            tabControls.Size = new Size(1423, 924);
            tabControls.TabIndex = 0;
            tabControls.DoubleClick += TabControls_DoubleClick;
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(32, 32);
            menuStrip.Items.AddRange(new ToolStripItem[] { DirectoriesToolStripMenuItem, ReportsToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1423, 40);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menu";
            // 
            // DirectoriesToolStripMenuItem
            // 
            DirectoriesToolStripMenuItem.Name = "DirectoriesToolStripMenuItem";
            DirectoriesToolStripMenuItem.Size = new Size(184, 36);
            DirectoriesToolStripMenuItem.Text = "Справочники";
            // 
            // ReportsToolStripMenuItem
            // 
            ReportsToolStripMenuItem.Name = "ReportsToolStripMenuItem";
            ReportsToolStripMenuItem.Size = new Size(116, 36);
            ReportsToolStripMenuItem.Text = "Отчеты";
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1423, 964);
            Controls.Add(tabControls);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "FormMain";
            Text = "FormComponentsKeeper";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabControls;
        private MenuStrip menuStrip;
        private ToolStripMenuItem DirectoriesToolStripMenuItem;
        private ToolStripMenuItem ReportsToolStripMenuItem;
    }
}
