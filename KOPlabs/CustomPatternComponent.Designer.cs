namespace ComponentLib;

partial class CustomPatternComponent
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
        inputTextBox = new TextBox();
        patternComboBox = new ComboBox();
        resultLabel = new Label();
        SuspendLayout();
        // 
        // inputTextBox
        // 
        inputTextBox.Location = new Point(45, 42);
        inputTextBox.Name = "inputTextBox";
        inputTextBox.Size = new Size(401, 39);
        inputTextBox.TabIndex = 2;
        inputTextBox.TextChanged += InputTextBox_TextChanged;
        // 
        // patternComboBox
        // 
        patternComboBox.FormattingEnabled = true;
        patternComboBox.Location = new Point(45, 104);
        patternComboBox.Name = "patternComboBox";
        patternComboBox.Size = new Size(401, 40);
        patternComboBox.TabIndex = 3;
        // 
        // resultLabel
        // 
        resultLabel.AutoSize = true;
        resultLabel.Location = new Point(45, 280);
        resultLabel.Name = "resultLabel";
        resultLabel.Size = new Size(314, 32);
        resultLabel.TabIndex = 4;
        resultLabel.Text = "status : if passed - displayed";
        // 
        // CustomPatternComponent
        // 
        AutoScaleDimensions = new SizeF(13F, 32F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(resultLabel);
        Controls.Add(patternComboBox);
        Controls.Add(inputTextBox);
        Name = "CustomPatternComponent";
        Size = new Size(500, 351);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private TextBox inputTextBox;
    private ComboBox patternComboBox;
    private Label resultLabel;
}
