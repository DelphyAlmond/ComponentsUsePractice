namespace OrderControl
{
    partial class EditAddForm
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
            customChoiceComponent = new ComponentLib.CustomChoiceComponent();
            customPatternComponent = new ComponentLib.CustomPatternComponent();
            fioTB = new TextBox();
            labelFIO = new Label();
            labelDate = new Label();
            labelCity = new Label();
            confirmBtn = new Button();
            notesTB = new RichTextBox();
            label1 = new Label();
            ReceiveDTP = new DateTimePicker();
            SuspendLayout();
            // 
            // customChoiceComponent
            // 
            customChoiceComponent.BackColor = Color.Cornsilk;
            customChoiceComponent.Location = new Point(216, 208);
            customChoiceComponent.Name = "customChoiceComponent";
            customChoiceComponent.Size = new Size(556, 120);
            customChoiceComponent.TabIndex = 0;
            // 
            // customPatternComponent
            // 
            customPatternComponent.BackColor = Color.Cornsilk;
            customPatternComponent.Location = new Point(216, 419);
            customPatternComponent.Name = "customPatternComponent";
            customPatternComponent.Size = new Size(556, 119);
            customPatternComponent.TabIndex = 1;
            // 
            // fioTB
            // 
            fioTB.Location = new Point(250, 54);
            fioTB.Name = "fioTB";
            fioTB.Size = new Size(522, 39);
            fioTB.TabIndex = 2;
            // 
            // labelFIO
            // 
            labelFIO.AutoSize = true;
            labelFIO.Location = new Point(32, 54);
            labelFIO.Name = "labelFIO";
            labelFIO.Size = new Size(217, 32);
            labelFIO.TabIndex = 3;
            labelFIO.Text = "Orderer Full Name:";
            // 
            // labelDate
            // 
            labelDate.AutoSize = true;
            labelDate.Location = new Point(32, 330);
            labelDate.Name = "labelDate";
            labelDate.Size = new Size(173, 64);
            labelDate.TabIndex = 4;
            labelDate.Text = "Planned\r\nreceiving Date:";
            // 
            // labelCity
            // 
            labelCity.AutoSize = true;
            labelCity.Location = new Point(32, 230);
            labelCity.Name = "labelCity";
            labelCity.Size = new Size(141, 32);
            labelCity.TabIndex = 5;
            labelCity.Text = "Destination:";
            // 
            // confirmBtn
            // 
            confirmBtn.Location = new Point(32, 563);
            confirmBtn.Name = "confirmBtn";
            confirmBtn.Size = new Size(740, 46);
            confirmBtn.TabIndex = 6;
            confirmBtn.Text = "Confirm";
            confirmBtn.UseVisualStyleBackColor = true;
            confirmBtn.Click += confirmBtn_Click;
            // 
            // notesTB
            // 
            notesTB.Location = new Point(216, 109);
            notesTB.Name = "notesTB";
            notesTB.Size = new Size(556, 79);
            notesTB.TabIndex = 7;
            notesTB.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(32, 156);
            label1.Name = "label1";
            label1.Size = new Size(172, 32);
            label1.TabIndex = 8;
            label1.Text = "Notes on path:";
            // 
            // ReceiveDTP
            // 
            ReceiveDTP.Location = new Point(216, 355);
            ReceiveDTP.Name = "ReceiveDTP";
            ReceiveDTP.Size = new Size(556, 39);
            ReceiveDTP.TabIndex = 9;
            // 
            // EditAddForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightCyan;
            ClientSize = new Size(800, 640);
            Controls.Add(ReceiveDTP);
            Controls.Add(label1);
            Controls.Add(notesTB);
            Controls.Add(confirmBtn);
            Controls.Add(labelCity);
            Controls.Add(labelDate);
            Controls.Add(labelFIO);
            Controls.Add(fioTB);
            Controls.Add(customPatternComponent);
            Controls.Add(customChoiceComponent);
            Name = "EditAddForm";
            Text = "Добавление/редактирование заказа";
            FormClosing += EditAddForm_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComponentLib.CustomChoiceComponent customChoiceComponent;
        private ComponentLib.CustomPatternComponent customPatternComponent;
        private TextBox fioTB;
        private Label labelFIO;
        private Label labelDate;
        private Label labelCity;
        private Button confirmBtn;
        private RichTextBox notesTB;
        private Label label1;
        private DateTimePicker ReceiveDTP;
    }
}