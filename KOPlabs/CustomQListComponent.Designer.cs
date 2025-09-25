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
        BtnAddToList = new Button();
        BtnRemove = new Button();
        regexTextBox = new TextBox();
        valuesTextBox = new TextBox();
        SuspendLayout();
        // 
        // dataListBox
        // 
        dataListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dataListBox.FormattingEnabled = true;
        dataListBox.Location = new Point(42, 39);
        dataListBox.Name = "dataListBox";
        dataListBox.Size = new Size(967, 164);
        dataListBox.TabIndex = 0;
        // 
        // BtnAddToList
        // 
        BtnAddToList.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        BtnAddToList.Location = new Point(42, 329);
        BtnAddToList.Name = "BtnAddToList";
        BtnAddToList.Size = new Size(239, 53);
        BtnAddToList.TabIndex = 1;
        BtnAddToList.Text = "Add to list";
        BtnAddToList.UseVisualStyleBackColor = true;
        BtnAddToList.Click += BtnAddToList_Click;
        // 
        // BtnRemove
        // 
        BtnRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnRemove.Location = new Point(747, 329);
        BtnRemove.Name = "BtnRemove";
        BtnRemove.Size = new Size(262, 53);
        BtnRemove.TabIndex = 2;
        BtnRemove.Text = "Remove & Clean";
        BtnRemove.UseVisualStyleBackColor = true;
        BtnRemove.Click += BtnRemove_Click;
        // 
        // regexTextBox
        // 
        regexTextBox.Location = new Point(404, 254);
        regexTextBox.Name = "regexTextBox";
        regexTextBox.Size = new Size(605, 39);
        regexTextBox.TabIndex = 3;
        // * regexTextBox.TextChanged += regexTextBox_TextChanged;

        // 
        // valuesTextBox
        // 
        valuesTextBox.Location = new Point(42, 254);
        valuesTextBox.Name = "valuesTextBox";
        valuesTextBox.Size = new Size(239, 39);
        valuesTextBox.TabIndex = 4;
        // 
        // CustomQListComponent
        // 
        AutoScaleDimensions = new SizeF(13F, 32F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(valuesTextBox);
        Controls.Add(regexTextBox);
        Controls.Add(BtnRemove);
        Controls.Add(BtnAddToList);
        Controls.Add(dataListBox);
        Name = "CustomQListComponent";
        Size = new Size(1046, 418);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ListBox dataListBox;
    private Button BtnAddToList;
    private Button BtnRemove;
    private TextBox regexTextBox;
    private TextBox valuesTextBox;
}
