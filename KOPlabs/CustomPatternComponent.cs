using System.ComponentModel;
using System.Text.RegularExpressions;
namespace ComponentLib;

public partial class CustomPatternComponent : UserControl
{
    private Regex _validationPattern;
    private readonly CompToolTipManager _toolTipManager;

    // Пуб. свойство для уст. и получ. регулярного выражения для шаблона

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

    public Regex ValidationPattern
    {
        get => _validationPattern;
        set
        {
            _validationPattern = value;
            _toolTipManager.Hide(inputTextBox);
        }
    }

    public CustomPatternComponent()
    {
        InitializeComponent();
        _toolTipManager = new CompToolTipManager();

        // > шаблон по умолчанию
        // pattern: YYYY.MM.DD
        ValidationPattern = new Regex(@"^\d{4}\.\d{2}\.\d{2}$");
    }

    public event EventHandler ValueChanged;

    // [***]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

    // Пуб. свойство для уст. и получ. введенного значения.
    public string Value
    {
        get
        {
            return inputTextBox.Text;
        }
        set
        {
            // > шаблон задан и значение удовлетворяет = вывести значение в resultLabel (status)
            if (ValidationPattern != null)
            {
                if (ValidationPattern.IsMatch(value))
                {
                    inputTextBox.Text = value;
                    _toolTipManager.Hide(inputTextBox);
                }
                else
                {
                    // > устанавливаемое значение не соответствует шаблону
                    resultLabel.Text = "failed";
                    _toolTipManager.ShowError(inputTextBox, $"[ * ] Set try of '{value}' value, NOT suitable for '{ValidationPattern.ToString()}'.");
                }
            }
            else
            {
                resultLabel.Text = "-";
                _toolTipManager.ShowError(inputTextBox, "[ * ] Pattern not stated, can't check the value consistency.");
            }
        }
    }

    private void InputTextBox_TextChanged(object sender, EventArgs e)
    {
        // > вызов публичного события ValueChanged
        // [ * ] событие вызывается при изменении текста, независимо от валидации (при Value)
        ValueChanged?.Invoke(this, e);
    }

    // Метод для уст. шаблона извне (из ComboBox)
    public void SetValidationPatternFromComboBox()
    {
        if (!string.IsNullOrEmpty(inputTextBox.Text) && ValidationPattern != null && !ValidationPattern.IsMatch(inputTextBox.Text))
        {
            _toolTipManager.ShowWarning(inputTextBox, $"[ ! ] Not consistent for current format.");
            resultLabel.Text = "failed";
        }
        else
        {
            _toolTipManager.Hide(inputTextBox);
        }
    }
}