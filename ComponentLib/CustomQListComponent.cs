using System.Linq;
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

    public ListBox GetDataLB
    {
        get => dataListBox;
    }

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
            string resultInfoLine = "";
            int v = 0;
            foreach (var key in _templateGenerator.PhraseMapping.Keys)
            {
                if (values.Count < _templateGenerator.PhraseMapping.Keys.Count
                    && v <= _templateGenerator.PhraseMapping.Keys.Count - 2 ||
                    values.Count == _templateGenerator.PhraseMapping.Keys.Count
                    && v <= _templateGenerator.PhraseMapping.Keys.Count - 1)
                    resultInfoLine += string.Concat(key, " ", values[v]);
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

        string remainingText = selectedFormattedString;
        List<string> phrases = _templateGenerator.PhraseMapping.Keys.ToList();
        List<string> values = _templateGenerator.PhraseMapping.Values.ToList();

        for (int i = 0; i < values.Count - 1; i++)
        {
            string textPhrase = phrases[i].Trim();
            string fieldOrPropertyName = values[i].Trim();
            if (i + 1 < phrases.Count - 1)
            {
                string nextTextPhrase = phrases[i + 1].Trim();

                // > Позиции строк информации (для вычленения значений):

                int phrasePosition = remainingText.IndexOf(textPhrase[textPhrase.Length - 1]);
                // > [ ! ] The phrase txt is everything from the start of remainingText up to the value info
                // = удаление фразы (её конец) -> (начало) переход автоматически на позицию значения
                // > Видоизм.\перезапись состояния строки, лидирующий эл.-т на выходе от - value
                remainingText = remainingText.Substring(phrasePosition + 1).Trim();

                int nextPhrasePosition = remainingText.IndexOf(nextTextPhrase[0]);

                string value = remainingText.Substring(0, nextPhrasePosition).Trim();

                // ^^^ [ X ] remainingText = remainingText.Substring(nextPhrasePosition + nextTextPhrase.Length);

                // > поле\св.- во существует
                if (!string.IsNullOrEmpty(fieldOrPropertyName) && !string.IsNullOrEmpty(value))
                {
                    SetPropertyOrField(obj, type, fieldOrPropertyName, value);
                }
            }
            else
            {
                int phrasePosition = remainingText.IndexOf(textPhrase[textPhrase.Length - 1]);
                remainingText = remainingText.Substring(phrasePosition + 1).Trim();
                SetPropertyOrField(obj, type, fieldOrPropertyName, remainingText);
            }
        }

        // Process any remaining text after the last phrase as the final value
        if (!string.IsNullOrEmpty(remainingText.Trim()))
        {
            var lastMapping = _templateGenerator.PhraseMapping.Last();
            if (!string.IsNullOrEmpty(lastMapping.Value))
            {
                SetPropertyOrField(obj, type, lastMapping.Value, remainingText.Trim());
            }
        }

        return obj;
    }

    private void SetPropertyOrField<T>(T obj, Type type, string propertyName, string value)
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
                // > Handle GUID conversion specifically
                if (targetType == typeof(Guid))
                {
                    convertedValue = Guid.Parse(value);
                }
                else if (targetType == typeof(string))
                {
                    convertedValue = value;
                }
                else if (!string.IsNullOrEmpty(value))
                {
                    convertedValue = Convert.ChangeType(value, targetType);
                }

                // > Set the value
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
                // [ * ] Handle errors
            }
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
