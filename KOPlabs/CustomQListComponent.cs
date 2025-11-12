using System.Reflection;
using System.Text.RegularExpressions;
namespace ComponentLib;

public partial class CustomQListComponent : UserControl
{
    private readonly CompToolTipManager _toolTipManager;
    private readonly TemplateGen _templateGenerator;

    public CustomQListComponent()
    {
        InitializeComponent();
        _toolTipManager = new CompToolTipManager();
        _templateGenerator = new TemplateGen();
        dataListBox.SelectedIndexChanged += DataListBox_SelectedIndexChanged;
    }

    public event EventHandler SelectionChanged;

    // Устанавливает : новый шаблон будет применяться только к добавляемым элементам
    public void SetTemplateForDisplay(string template)
    {
        try
        {
            _templateGenerator.TemplateString = template;
            _toolTipManager.Hide(dataListBox);
        }
        catch (Exception ex)
        {
            _toolTipManager.ShowError(dataListBox, ex.Message);
            throw;
        }
    }

    // Добавляет в конец списка ListBox строчку, сопоставленную по текущему сконфигурированному шаблону
    public void AddItem(string rowSepWithValues)
    {
        try
        {
            List<string> values = rowSepWithValues.Split(";").ToList();

            // Сопоставляем, подсовывая на места после фраз шаблона:
            string resultInfoLine = string.Empty;
            int v = 0;
            foreach (var key in _templateGenerator.PhraseMapping.Keys)
            {
                resultInfoLine += key + " ";
                resultInfoLine += values[v];
                v++;
            }

            dataListBox.Items.Add(resultInfoLine); // > полноценная фраза, сопоставленная шаблоном
            _toolTipManager.Hide(dataListBox);
        }
        catch (Exception ex)
        {
            _toolTipManager.ShowWarning(dataListBox, $"[ ! ] Не удалось добавить элемент: {ex.Message}");
        }
    }

    // [ ! ] Публичный параметризованный метод для получения объекта.
    // - получает выбранный элемент из ListBox (строку-фразу)
    // и заполняет соответствующие свойства/поля в созданном объекте типа T с использованием рефлексии
    public T GetItemFromSelected<T>() where T : new()
    {
        if (dataListBox.SelectedItem == null)
        {
            throw new Exception("[ ! ] В ListBox не выбран корректный элемент.");
        }

        string? selectedFormattedString = dataListBox.SelectedItem.ToString();
        if (string.IsNullOrEmpty(selectedFormattedString))
        {
            throw new Exception("[ ! ] Выбранный элемент ListBox пуст или недействителен.");
        }

        T obj = Activator.CreateInstance<T>();
        Type type = typeof(T);

        // Для значений параметров
        List<string> values = [];

        int valueIndex = 0;
        foreach (var pare in _templateGenerator.PhraseMapping)
        {
            string textPhrase = pare.Key;
            string fieldOrProperty = pare.Value;

            // Позиции строк информации (для вычленения значений)
            int phrasePosition = selectedFormattedString.IndexOf(textPhrase);

            // [ ! ] The value is everything from the start of remainingText up to the text phrase
            string value = selectedFormattedString.Substring(0, phrasePosition).Trim(); // удаление фразы, её конец -> позиция значения
            // Видоизм.\перезапись состояния строки, лидирующий эл.-т - value
            selectedFormattedString = selectedFormattedString.Substring(phrasePosition + textPhrase.Length);

            // > поле\св.-во существует
            if (!string.IsNullOrEmpty(fieldOrProperty) && !string.IsNullOrEmpty(value))
            {
                SetPropertyOrField(obj, type, fieldOrProperty, value, valueIndex);
            }

            valueIndex++;
        }

        // Process any remaining text after the last phrase as the final value
        if (!string.IsNullOrEmpty(selectedFormattedString.Trim()))
        {
            var lastMapping = _templateGenerator.PhraseMapping.Last();
            if (!string.IsNullOrEmpty(lastMapping.Value))
            {
                SetPropertyOrField(obj, type, lastMapping.Value, selectedFormattedString.Trim(), valueIndex);
            }
        }

        return obj;
    }

    private void SetPropertyOrField<T>(T obj, Type type, string propertyName, string value, int valueIndex)
    {
        PropertyInfo? property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        FieldInfo? field = null;

        if (property == null)
        {
            field = type.GetField(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        }

        if (property != null || field != null)
        {
            Type targetType = property != null ? property.PropertyType : field!.FieldType;
            object? convertedValue = null;

            try
            {
                if (targetType == typeof(string))
                {
                    convertedValue = value;
                }
                else if (!string.IsNullOrEmpty(value))
                {
                    convertedValue = Convert.ChangeType(value, targetType);
                }

                if (property != null && property.CanWrite)
                {
                    property.SetValue(obj, convertedValue);
                }
                else if (field != null)
                {
                    field.SetValue(obj, convertedValue);
                }
            }
            catch (Exception ex)
            {
                _toolTipManager.ShowWarning(dataListBox, $"[ ! ] Ошибка преобразования значения '{value}' для '{propertyName}': {ex.Message}");
            }
        }
        else
        {
            _toolTipManager.ShowWarning(dataListBox, $"[ * ] Не найдено свойство/поле '{propertyName}'. Значение '{value}' пропущено.");
        }
    }

    private void DataListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectionChanged?.Invoke(this, e);
        _toolTipManager.Hide(dataListBox);
    }

    public void Clear()
    {
        dataListBox.Items.Clear();
        _toolTipManager.Hide(dataListBox);
    }
}
