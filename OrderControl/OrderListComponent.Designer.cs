namespace OrderControl
{
    partial class OrderListComponent
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
            addEditBtn = new Button();
            deleteBtn = new Button();
            listPanel = new Panel();
            SuspendLayout();
            // 
            // addEditBtn
            // 
            addEditBtn.Location = new Point(17, 762);
            addEditBtn.Name = "addEditBtn";
            addEditBtn.Size = new Size(342, 46);
            addEditBtn.TabIndex = 0;
            addEditBtn.Text = "Add\\Edit";
            addEditBtn.UseVisualStyleBackColor = true;
            addEditBtn.Click += addEditBtn_Click;
            // 
            // deleteBtn
            // 
            deleteBtn.Location = new Point(365, 762);
            deleteBtn.Name = "deleteBtn";
            deleteBtn.Size = new Size(240, 46);
            deleteBtn.TabIndex = 1;
            deleteBtn.Text = "Remove";
            deleteBtn.UseVisualStyleBackColor = true;
            deleteBtn.Click += deleteBtn_Click;
            // 
            // listPanel
            // 
            listPanel.Location = new Point(3, 15);
            listPanel.Name = "listPanel";
            listPanel.Size = new Size(1622, 735);
            listPanel.TabIndex = 2;
            // 
            // OrderListComponent
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightCyan;
            Controls.Add(listPanel);
            Controls.Add(deleteBtn);
            Controls.Add(addEditBtn);
            Name = "OrderListComponent";
            Size = new Size(1628, 824);
            ResumeLayout(false);
        }

        #endregion

        private Button addEditBtn;
        private Button deleteBtn;
        private Panel listPanel;
    }
}
