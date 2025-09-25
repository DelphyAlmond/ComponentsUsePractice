namespace ComponentLib;

partial class CustomChoiceComponent
{
    // Required designer variable.
    private System.ComponentModel.IContainer components = null;


    // Clean up any resources being used.
    // disposing - true if managed resources should be disposed; otherwise, false.
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    private void InitializeComponent()
    {
        ChoiceComboBox = new ComboBox();
        choiceTextBox = new TextBox();
        btnAdd = new Button();
        SuspendLayout();
        // 
        // ChoiceComboBox
        // 
        ChoiceComboBox.FormattingEnabled = true;
        ChoiceComboBox.Location = new Point(48, 42);
        ChoiceComboBox.Name = "ChoiceComboBox";
        ChoiceComboBox.Size = new Size(529, 40);
        ChoiceComboBox.TabIndex = 0;
        ChoiceComboBox.SelectedIndexChanged += ChoiceComboBox_SelectedIndexChanged;
        // 
        // choiceTextBox
        // 
        choiceTextBox.Location = new Point(235, 455);
        choiceTextBox.Name = "choiceTextBox";
        choiceTextBox.Size = new Size(342, 39);
        choiceTextBox.TabIndex = 1;
        // 
        // btnAdd
        // 
        btnAdd.Location = new Point(48, 448);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(170, 46);
        btnAdd.TabIndex = 2;
        btnAdd.Text = "Add choice";
        btnAdd.UseVisualStyleBackColor = true;
        btnAdd.Click += this.btnAdd_Click;
        // 
        // CustomChoiceComponent
        // 
        AutoScaleDimensions = new SizeF(13F, 32F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(btnAdd);
        Controls.Add(choiceTextBox);
        Controls.Add(ChoiceComboBox);
        Name = "CustomChoiceComponent";
        Size = new Size(629, 545);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ComboBox ChoiceComboBox;
    private TextBox choiceTextBox;
    private Button btnAdd;
}
