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
            groupBoxTemplate = new GroupBox();
            comboBoxTemplate = new ComboBox();
            generateButtonTemplate = new Button();
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
            mainFlowLayoutPanel.Size = new Size(603, 444);
            mainFlowLayoutPanel.TabIndex = 0;
            mainFlowLayoutPanel.WrapContents = false;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(32, 32);
            statusStrip.Location = new Point(0, 444);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(603, 22);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip1";
            // 
            // loadingLabel
            // 
            loadingLabel.AutoSize = true;
            loadingLabel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            loadingLabel.Location = new Point(250, 180);
            loadingLabel.Name = "loadingLabel";
            loadingLabel.Size = new Size(389, 37);
            loadingLabel.TabIndex = 2;
            loadingLabel.Text = "< showing while running >";
            loadingLabel.Visible = false;
            // 
            // groupBoxTemplate
            // 
            groupBoxTemplate.Location = new Point(0, 0);
            groupBoxTemplate.Name = "groupBoxTemplate";
            groupBoxTemplate.Size = new Size(580, 120);
            groupBoxTemplate.TabIndex = 0;
            groupBoxTemplate.TabStop = false;
            groupBoxTemplate.Visible = false;
            // 
            // comboBoxTemplate
            // 
            comboBoxTemplate.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTemplate.FormattingEnabled = true;
            comboBoxTemplate.Location = new Point(20, 40);
            comboBoxTemplate.Name = "comboBoxTemplate";
            comboBoxTemplate.Size = new Size(250, 40);
            comboBoxTemplate.TabIndex = 0;
            // 
            // generateButtonTemplate
            // 
            generateButtonTemplate.Location = new Point(290, 40);
            generateButtonTemplate.Name = "generateButtonTemplate";
            generateButtonTemplate.Size = new Size(200, 42);
            generateButtonTemplate.TabIndex = 1;
            generateButtonTemplate.Text = "Сгенерировать отчет";
            generateButtonTemplate.UseVisualStyleBackColor = true;
            // 
            // ReportPluginsForm
            // 
            AutoScaleDimensions = new SizeF(15F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(603, 466);
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
        private System.Windows.Forms.GroupBox groupBoxTemplate;
        private System.Windows.Forms.ComboBox comboBoxTemplate;
        private System.Windows.Forms.Button generateButtonTemplate;
    }
}