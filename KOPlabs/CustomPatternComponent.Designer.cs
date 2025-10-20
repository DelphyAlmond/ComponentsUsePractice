﻿namespace ComponentLib;

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
        resultLabel = new Label();
        SuspendLayout();
        // 
        // inputTextBox
        // 
        inputTextBox.Location = new Point(45, 42);
        inputTextBox.Name = "inputTextBox";
        inputTextBox.Size = new Size(447, 39);
        inputTextBox.TabIndex = 2;
        inputTextBox.TextChanged += InputTextBox_TextChanged;
        // 
        // resultLabel
        // 
        resultLabel.AutoSize = true;
        resultLabel.Location = new Point(45, 110);
        resultLabel.Name = "resultLabel";
        resultLabel.Size = new Size(447, 32);
        resultLabel.TabIndex = 4;
        resultLabel.Text = "status : if passed - displayed/shown here";
        // 
        // CustomPatternComponent
        // 
        AutoScaleDimensions = new SizeF(13F, 32F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(resultLabel);
        Controls.Add(inputTextBox);
        Name = "CustomPatternComponent";
        Size = new Size(535, 186);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private TextBox inputTextBox;
    private Label resultLabel;
}
