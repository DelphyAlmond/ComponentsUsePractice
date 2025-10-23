namespace ControlLib
{
    partial class OrderEditor
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
            customerNameTb = new TextBox();
            statusNoteTb = new TextBox();
            nameLabel = new Label();
            statusNoteLabel = new Label();
            customChoiceComponent1 = new ComponentLib.CustomChoiceComponent();
            cityLabel = new Label();
            customPatternComponent1 = new ComponentLib.CustomPatternComponent();
            submitBtn = new Button();
            cancelBtn = new Button();
            SuspendLayout();
            // 
            // customerNameTb
            // 
            customerNameTb.Location = new Point(176, 37);
            customerNameTb.Name = "customerNameTb";
            customerNameTb.Size = new Size(432, 39);
            customerNameTb.TabIndex = 0;
            // 
            // statusNoteTb
            // 
            statusNoteTb.Location = new Point(176, 107);
            statusNoteTb.Name = "statusNoteTb";
            statusNoteTb.Size = new Size(432, 39);
            statusNoteTb.TabIndex = 1;
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(36, 40);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(117, 32);
            nameLabel.TabIndex = 2;
            nameLabel.Text = "Customer";
            // 
            // statusNoteLabel
            // 
            statusNoteLabel.AutoSize = true;
            statusNoteLabel.Location = new Point(72, 110);
            statusNoteLabel.Name = "statusNoteLabel";
            statusNoteLabel.Size = new Size(81, 32);
            statusNoteLabel.TabIndex = 3;
            statusNoteLabel.Text = "status:\r\n";
            // 
            // customChoiceComponent1
            // 
            customChoiceComponent1.Location = new Point(28, 200);
            customChoiceComponent1.Name = "customChoiceComponent1";
            customChoiceComponent1.Size = new Size(630, 188);
            customChoiceComponent1.TabIndex = 4;
            // 
            // cityLabel
            // 
            cityLabel.AutoSize = true;
            cityLabel.Location = new Point(36, 181);
            cityLabel.Name = "cityLabel";
            cityLabel.Size = new Size(196, 32);
            cityLabel.TabIndex = 5;
            cityLabel.Text = "destination (city):";
            // 
            // customPatternComponent1
            // 
            customPatternComponent1.Location = new Point(57, 315);
            customPatternComponent1.Name = "customPatternComponent1";
            customPatternComponent1.Size = new Size(551, 188);
            customPatternComponent1.TabIndex = 6;
            // 
            // submitBtn
            // 
            submitBtn.Location = new Point(36, 509);
            submitBtn.Name = "submitBtn";
            submitBtn.Size = new Size(299, 68);
            submitBtn.TabIndex = 7;
            submitBtn.Text = "Submit";
            submitBtn.UseVisualStyleBackColor = true;
            // 
            // cancelBtn
            // 
            cancelBtn.Location = new Point(372, 509);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new Size(236, 68);
            cancelBtn.TabIndex = 8;
            cancelBtn.Text = "Cancel";
            cancelBtn.UseVisualStyleBackColor = true;
            // 
            // OrderEditor
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightCyan;
            ClientSize = new Size(663, 619);
            Controls.Add(customPatternComponent1);
            Controls.Add(cancelBtn);
            Controls.Add(submitBtn);
            Controls.Add(cityLabel);
            Controls.Add(statusNoteLabel);
            Controls.Add(nameLabel);
            Controls.Add(statusNoteTb);
            Controls.Add(customerNameTb);
            Controls.Add(customChoiceComponent1);
            Name = "OrderEditor";
            Text = "OrderEditor";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox customerNameTb;
        private TextBox statusNoteTb;
        private Label nameLabel;
        private Label statusNoteLabel;
        private ComponentLib.CustomChoiceComponent customChoiceComponent1;
        private Label cityLabel;
        private ComponentLib.CustomPatternComponent customPatternComponent1;
        private Button submitBtn;
        private Button cancelBtn;
    }
}