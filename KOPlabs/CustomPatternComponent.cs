using System.ComponentModel;
using System.Text.RegularExpressions;
namespace ComponentLib;

public partial class CustomPatternComponent : UserControl
{
    private Regex _validationPattern;
    private string _patternExample;
    private readonly CompToolTipManager _toolTipManager;

    // [***]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    // Пуб. свойство для уст. и получ. шаблона валидации
    public string ValidationPatternString
    {
        get => _validationPattern?.ToString() ?? string.Empty;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _validationPattern = new Regex(value);
            }
            else
            {
                _validationPattern = null; // > не задан
            }
        }
    }

    List<object> dateFormats = new List<object>
    {
        new { Name = "YYYY.MM.DD", Pattern = @"^\d{4}\.\d{2}\.\d{2}$", Example = "2025.09.21" },
        new { Name = "DD/MM/YYYY", Pattern = @"^\d{2}\/\d{2}\/\d{4}$", Example = "21/09/2025" },
        new { Name = "MM-DD-YYYY", Pattern = @"^\d{2}-\d{2}-\d{4}$", Example = "09-21-2025" },
        new { Name = "YYYY-MM-DD", Pattern = @"^\d{4}-\d{2}-\d{2}$", Example = "2025-09-21" }
    };

    public CustomPatternComponent()
    {
        InitializeComponent();
        _toolTipManager = new CompToolTipManager();

        // > шаблон по умолчанию
        // > /^(\d{4})\.(0[1-9]|1[0-2])\.(0[1-9]|[12]\d|3[01])$/
        // pattern: YYYY.MM.DD
        ValidationPatternString = @"^\d{4}\.\d{2}\.\d{2}$";
        SetPatternExample("2025.09.21");

        patternComboBox.DisplayMember = "Name"; // Что отображается в списке
        patternComboBox.ValueMember = "Pattern"; // Какое значение ассоциировано
        patternComboBox.DataSource = dateFormats;

        // Устанавливаем выбранный по умолчанию элемент
        patternComboBox.SelectedIndex = 0; // YYYY.MM.DD
    }

    public event EventHandler ValueChanged;

    public void SetPatternExample(string example)
    {
        _patternExample = example;
    }

    private Regex ValidationPattern
    {
        get => _validationPattern;
        set
        {
            _validationPattern = value;
            _toolTipManager.Hide(inputTextBox);
        }
    }

    // [***]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

    // Пуб. свойство для уст. и получ. введенного значения.
    public string SelectedValue
    {
        get
        {
            if (ValidationPattern == null)
            {
                _toolTipManager.ShowError(inputTextBox, "[ * ] Pattern is not stated.");
                resultLabel.Text = "-";
            }
            // > значение не соответствует шаблону
            if (!ValidationPattern.IsMatch(inputTextBox.Text))
            {
                _toolTipManager.ShowError(inputTextBox, $"[ ! ] Value: '{inputTextBox.Text}' | not equal: '{ValidationPatternString}'.");
                resultLabel.Text = "failed";
            }
            _toolTipManager.Hide(inputTextBox);
            return inputTextBox.Text;
        }
        set
        {
            // SET:
            // > шаблон задан и значение удовлетворяет = вывести значение в resultLabel (status)
            if (ValidationPattern != null)
            {
                if (ValidationPattern.IsMatch(value))
                {
                    resultLabel.Text = value;
                    _toolTipManager.Hide(inputTextBox);
                }
                else
                {
                    // > устанавливаемое значение не соответствует шаблону
                    resultLabel.Text = "failed";
                    _toolTipManager.ShowError(inputTextBox, $"[ * ] Set try of '{value}' value, NOT suitable for '{ValidationPatternString}'.");
                }
            }
            else
            {
                inputTextBox.Text = value;
                resultLabel.Text = "-";
                _toolTipManager.ShowError(inputTextBox, "[ * ] Pattern not stated, can't check the value consistency.");
            }
        }
    }

    private void InputTextBox_TextChanged(object sender, EventArgs e)
    {
        _toolTipManager.Hide(inputTextBox);

        // > проверка соответствия шаблону (на кажд. изменение)
        if (ValidationPattern != null && !ValidationPattern.IsMatch(inputTextBox.Text))
        {
            _toolTipManager.ShowWarning(inputTextBox, $"[ ! ] Not consistent for format: {_patternExample}");
            resultLabel.Text = "failed";
        }
        else if (ValidationPattern == null && !string.IsNullOrEmpty(inputTextBox.Text))
        {
            _toolTipManager.ShowWarning(inputTextBox, "[ * ] No pattern stated.");
            resultLabel.Text = "-";
        }

        resultLabel.Text = inputTextBox.Text;
        // > вызов публичного события ValueChanged
        // [ * ] событие вызывается при изменении текста, независимо от валидации (при SelectedValue)
        ValueChanged?.Invoke(this, e);
    }

    // Метод для уст. шаблона извне, из ComboBox
    public void SetValidationPatternFromComboBox(string pattern, string example)
    {
        ValidationPatternString = pattern;
        SetPatternExample(example);
        if (!string.IsNullOrEmpty(inputTextBox.Text) && ValidationPattern != null && !ValidationPattern.IsMatch(inputTextBox.Text))
        {
            _toolTipManager.ShowWarning(inputTextBox, $"[ ! ] Not consistent for format: {_patternExample}");
            resultLabel.Text = "failed";
        }
        else
        {
            _toolTipManager.Hide(inputTextBox);
        }
    }
}