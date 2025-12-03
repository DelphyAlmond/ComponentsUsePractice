namespace FormComponentDisplay
{
    partial class ReportPluginsForm
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
            mainFlowLayoutPanel = new FlowLayoutPanel();
            statusStrip = new StatusStrip();
            loadingLabel = new Label();
            SuspendLayout();
            // 
            // mainFlowLayoutPanel
            // 
            mainFlowLayoutPanel.AutoScroll = true;
            mainFlowLayoutPanel.Dock = DockStyle.Fill;
            mainFlowLayoutPanel.FlowDirection = FlowDirection.TopDown;
            mainFlowLayoutPanel.Location = new Point(0, 0);
            mainFlowLayoutPanel.Name = "mainFlowLayoutPanel";
            mainFlowLayoutPanel.Padding = new Padding(10);
            mainFlowLayoutPanel.Size = new Size(600, 362);
            mainFlowLayoutPanel.TabIndex = 0;
            mainFlowLayoutPanel.WrapContents = false;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(32, 32);
            statusStrip.Location = new Point(0, 362);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(600, 38);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip1";
            // 
            // loadingLabel
            // 
            loadingLabel.AutoSize = true;
            loadingLabel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            loadingLabel.Location = new Point(250, 180);
            loadingLabel.Name = "loadingLabel";
            loadingLabel.Size = new Size(175, 37);
            loadingLabel.TabIndex = 2;
            loadingLabel.Text = "Загрузка...";
            loadingLabel.Visible = false;
            // 
            // ReportPluginsForm
            // 
            AutoScaleDimensions = new SizeF(15F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 400);
            Controls.Add(loadingLabel);
            Controls.Add(mainFlowLayoutPanel);
            Controls.Add(statusStrip);
            Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ReportPluginsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "< Плагины отчетов >";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel mainFlowLayoutPanel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Label loadingLabel;
    }
}