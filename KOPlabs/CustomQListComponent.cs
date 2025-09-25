using System.Reflection;
namespace ComponentLib;

public partial class CustomQListComponent : UserControl
{
    private readonly CompToolTipManager _toolTipManager;
    private List<List<string>> _dataObjects = new List<List<string>>();
    private StringFactory currentPattern;

    public CustomQListComponent()
    {
        InitializeComponent();
        _toolTipManager = new CompToolTipManager();
        currentPattern = new StringFactory();
        dataListBox.SelectedIndexChanged += DataListBox_SelectedIndexChanged;
    }

    public event EventHandler SelectionChanged;

    // Форматирует список значений в строку согласно текущему шаблону StringFactory
    private string FormatObject(List<string> values)
    {
        if (values == null) return string.Empty;

        try
        {
            return currentPattern.FormatOutputString(values);
        }
        catch (ArgumentException ex)
        {
            _toolTipManager.ShowError(dataListBox, ex.Message);
            return "[ Error ] Ошибка форматирования: " + ex.Message;
        }
    }

    // [ ! ] Обновляет отображение всех элементов в ListBox.
    // Используется при изменении шаблона.
    private void RefreshListBoxDisplay()
    {
        dataListBox.Items.Clear();
        foreach (var valuesList in _dataObjects)
        {
            dataListBox.Items.Add(FormatObject(valuesList));
        }
        dataListBox.Update(); // [ ? ]
    }

    private void DataListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectionChanged?.Invoke(this, e);
        _toolTipManager.Hide(dataListBox);
    }

    // заполнение ListBox:
    // >> Добавление одной входной строки в конец списка
    // (Строка с пробел-разделенными значениями)
    private void BtnAddToList_Click(object sender, EventArgs e)
    {
        currentPattern.TemplateString = regexTextBox.Text;

        if (string.IsNullOrWhiteSpace(valuesTextBox.Text)) return;
        List<string> values = valuesTextBox.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        try
        {
            // Проверка на корректность количества значений
            currentPattern.FormatOutputString(values);
            _dataObjects.Add(values);
            dataListBox.Items.Add(FormatObject(values));

            RefreshListBoxDisplay();
        }
        catch (ArgumentException ex)
        {
            _toolTipManager.ShowWarning(dataListBox, $"[ ! ] Не удалось добавить строку со значениями: '{valuesTextBox.Text}': {ex.Message}");
        }
    }

    private void BtnRemove_Click(object sender, EventArgs e)
    {
        _dataObjects.Clear();
        dataListBox.Items.Clear();
        _toolTipManager.Hide(dataListBox);

        RefreshListBoxDisplay();
    }

    /*
    private void regexTextBox_TextChanged(object sender, EventArgs e)
    {
        currentPattern.TemplateString = regexTextBox.Text;  ^^^  [ * ] in Add-operation
    }
    */
}
