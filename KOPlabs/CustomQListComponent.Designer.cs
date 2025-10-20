namespace ComponentLib;

partial class CustomQListComponent
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
        dataListBox = new ListBox();
        SuspendLayout();
        // 
        // dataListBox
        // 
        dataListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dataListBox.FormattingEnabled = true;
        dataListBox.Location = new Point(42, 39);
        dataListBox.Name = "dataListBox";
        dataListBox.Size = new Size(537, 676);
        dataListBox.TabIndex = 0;
        // 
        // CustomQListComponent
        // 
        AutoScaleDimensions = new SizeF(13F, 32F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(dataListBox);
        Name = "CustomQListComponent";
        Size = new Size(625, 784);
        ResumeLayout(false);
    }

    #endregion

    private ListBox dataListBox;
}
