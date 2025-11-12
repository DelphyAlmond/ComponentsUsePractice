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
        choiceComboBox = new ComboBox();
        SuspendLayout();
        // 
        // choiceComboBox
        // 
        choiceComboBox.FormattingEnabled = true;
        choiceComboBox.Location = new Point(14, 16);
        choiceComboBox.Name = "choiceComboBox";
        choiceComboBox.Size = new Size(529, 40);
        choiceComboBox.TabIndex = 0;
        choiceComboBox.SelectedIndexChanged += ChoiceComboBox_SelectedIndexChanged;
        // 
        // CustomChoiceComponent
        // 
        AutoScaleDimensions = new SizeF(13F, 32F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.Cornsilk;
        Controls.Add(choiceComboBox);
        Name = "CustomChoiceComponent";
        Size = new Size(556, 232);
        ResumeLayout(false);
    }

    #endregion

    private ComboBox choiceComboBox;
}