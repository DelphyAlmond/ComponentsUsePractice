using System.Reflection;
namespace ComponentLib;

public partial class CustomQListComponent : UserControl
{
    private readonly CompToolTipManager _toolTipManager;
    // private List<List<string>> _dataObjects; // - Удалено
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

    // Добавляет в конец списка ListBox сопоставленную по текущему сконфигурированному шаблону
    public void AddItem(Dictionary<string, string> values) // Изменена сигнатура
    {
        if (values == null)
        {
            _toolTipManager.ShowWarning(dataListBox, "[ ! ] Словарь значений не может быть пустым.");
            return;
        }

        try
        {
            string row = _templateGenerator.FormatOutputString(values); // Проверка валидности и форматирование
            dataListBox.Items.Add(row); // полноценная фраза, сопоставленная шаблоном
            _toolTipManager.Hide(dataListBox);
        }
        catch (Exception ex)
        {
            _toolTipManager.ShowWarning(dataListBox, $"[ ! ] Не удалось добавить элемент: {ex.Message}");
        }
    }

    // [ ! ] Публичный параметризованный метод для получения объекта.
    // - получает выбранный элемент из ListBox (строку-фразу), преобразует ее в Dictionary<string, string> значений
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

        Dictionary<string, string> parsedValues;
        try
        {
            parsedValues = _templateGenerator.ParseFormattedString(selectedFormattedString);
        }
        catch (Exception ex)
        {
            _toolTipManager.ShowError(dataListBox, $"[ ! ] Ошибка парсинга выбранной строки '{selectedFormattedString}': {ex.Message}");
            throw new Exception($"[ ! ] Не удалось извлечь значения из выбранной строки: {ex.Message}", ex);
        }

        T obj = Activator.CreateInstance<T>();
        Type type = typeof(T);

        // Итерируем по всем извлеченным значениям из строки
        foreach (var entry in parsedValues)
        {
            string propertyOrFieldName = entry.Key;
            string valueToSet = entry.Value;

            // Используем рефлексию для поиска свойства/поля в объекте T
            PropertyInfo? property = type.GetProperty(propertyOrFieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            FieldInfo? field = null;

            if (property == null) // Если свойство не найдено, ищем поле
            {
                field = type.GetField(propertyOrFieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            }

            if (property != null || field != null)
            {
                Type targetType = property != null ? property.PropertyType : field!.FieldType;
                object? convertedValue = null;

                try
                {
                    if (targetType == typeof(string))
                    {
                        convertedValue = valueToSet;
                    }
                    else if (!string.IsNullOrEmpty(valueToSet))
                    {
                        convertedValue = Convert.ChangeType(valueToSet, targetType);
                    }
                    if (property != null && property.CanWrite)
                    {
                        property.SetValue(obj, convertedValue);
                    }
                    else if (field != null) // Поля всегда записываемы, если они не readonly
                    {
                        field!.SetValue(obj, convertedValue);
                    }
                    else
                    {
                        _toolTipManager.ShowWarning(dataListBox, $"[ * ] Свойство '{propertyOrFieldName}' найдено, но не записываемо. Значение '{valueToSet}' пропущено.");
                    }
                }
                catch (Exception ex)
                {
                    _toolTipManager.ShowError(dataListBox, $"[ ! ] Ошибка преобразования значения '{valueToSet}' для члена '{propertyOrFieldName}' в тип '{targetType.Name}'.");
                    throw new Exception($"[ ! ] Не удалось установить значение '{valueToSet}' для члена '{propertyOrFieldName}': {ex.Message}", ex);
                }
            }
            else
            {
                _toolTipManager.ShowWarning(dataListBox, $"[ * ] В классе '{type.Name}' не найдено публичное свойство или поле '{propertyOrFieldName}'. Значение '{valueToSet}' пропущено.");
            }
        }

        return obj;
    }

    // Устанавливает сопоставление имен плейсхолдеров с именами свойств
    public void SetCustomPropertyMapping(Dictionary<string, string> newMapping)
    {
        _templateGenerator.SetCustomPropertyMapping(newMapping);
        _toolTipManager.Hide(dataListBox);
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
