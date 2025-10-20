using System.Text.RegularExpressions;

public class TemplateGen
{
    private string _currentTemplateString = string.Empty;
    // Регулярное выражение типа {propertyName}
    private static readonly Regex _placeholderRegex = new Regex(@"\{(\w+)\}"); // static readonly

    // Имена свойств, извлеченные из шаблона, в порядке их появления
    private List<string> _templatePropertyNames = new List<string>();
    // Словарь для сопоставления
    private Dictionary<string, string> _propertyMapping = new Dictionary<string, string>();

    public Dictionary<string, string> PropertyMapping
    {
        get => _propertyMapping;
        set
        {
            if (value == null) throw new ArgumentNullException(nameof(PropertyMapping), "Словарь сопоставления свойств не может быть null.");
            _propertyMapping = value;
        }
    }

    public string TemplateString
    {
        get => _currentTemplateString;
        set => ValidateAndParseTemplate(value);
    }

    public TemplateGen()
    {
        SetDefaultPropertyMapping();
        // Устанавливаем шаблон при создании экземпляра:
        TemplateString = "Кол-во параметра {param} составляет {percent}%, а результат вычисления: {num}.";
    }

    private void SetDefaultPropertyMapping()
    {
        PropertyMapping = new Dictionary<string, string>
        {
            { "param", "Parameter" },    // * {param} в шаблоне -> свойство Parameter в объекте
            { "percent", "Percentage" }, // {percent} в шаблоне -> свойство Percentage в объекте
            { "num", "Number" }          // {num} в шаблоне -> свойство Number в объекте
        };
    }

    public void SetCustomPropertyMapping(Dictionary<string, string> newMapping)
    {
        PropertyMapping = newMapping;
    }

    private void ValidateAndParseTemplate(string template)
    {
        _currentTemplateString = template ?? string.Empty;
        _templatePropertyNames.Clear();

        // 1. Проверка на валидность регулярным выражением (например, отсутствие пустых скобок {})
        if (_currentTemplateString.Contains("{}"))
        {
            throw new Exception("[ ! ] Шаблон не должен содержать пустые плейсхолдеры '{}'. Используйте именованные, например '{propertyName}'.");
        }

        // Находим все плейсхолдеры и извлекаем их имена
        MatchCollection matches = _placeholderRegex.Matches(_currentTemplateString);
        foreach (Match match in matches)
        {
            _templatePropertyNames.Add(match.Groups[1].Value); // - содержимое скобок
        }

        if (matches.Count == 0 && (_currentTemplateString.Contains("{") || _currentTemplateString.Contains("}")))
        {
            throw new Exception("[ ! ] Шаблон содержит незакрытые или некорректно сформированные плейсхолдеры.");
        }
        if (matches.Count == 0 && string.IsNullOrWhiteSpace(_currentTemplateString))
        {
            throw new Exception("[ ! ] Шаблон не должен быть пустым.");
        }

        // 2. Проверка на 2 свойства подряд ({prop1}{prop2} / {prop1} {prop2})
        if (_currentTemplateString.Contains("}{"))
        {
            throw new Exception("[ ! ] Шаблон не должен содержать два свойства подряд без текста между ними.");
        }
        if (_currentTemplateString.Contains("} {"))
        {
            throw new Exception("[ ! ] Шаблон не должен содержать два свойства подряд с пробелом между ними без текста.");
        }
    }

    // Форматирование: Dictionary<string, string> [ ! ]
    public string FormatOutputString(Dictionary<string, string> values)
    {
        if (values == null) throw new ArgumentNullException(nameof(values), "[ ! ] Словарь значений не может быть null.");

        string formattedString = _currentTemplateString;
        foreach (var placeholderName in _templatePropertyNames)
        {
            string valueKey = placeholderName; // Изначально ключ - это имя плейсхолдера

            // Проверяем, есть ли прямой ключ в словаре значений
            if (!values.ContainsKey(valueKey))
            {
                string mappedKey = PropertyMapping.ContainsKey(placeholderName) ? PropertyMapping[placeholderName] : placeholderName;
                if (!values.ContainsKey(mappedKey))
                {
                    throw new ArgumentException($"[ ! ] Для плейсхолдера '{placeholderName}' (или сопоставленного имени '{mappedKey}') не найдено значение в словаре.");
                }
                valueKey = mappedKey;
            }

            // Заменяем плейсхолдер на значение
            formattedString = formattedString.Replace("{" + placeholderName + "}", values[valueKey] ?? string.Empty);
        }
        return formattedString;
    }

    // Парсинг: возвращает Dictionary<string, string>
    public Dictionary<string, string> ParseFormattedString(string formattedString)
    {
        if (string.IsNullOrEmpty(formattedString)) throw new ArgumentNullException(nameof(formattedString), "[ ! ] Форматированная строка не может быть пустой.");
        if (_templatePropertyNames.Count == 0)
        {
            if (formattedString != _currentTemplateString)
            {
                throw new Exception("[ ! ] Шаблон не содержит именованных плейсхолдеров для парсинга, но отформатированная строка не совпадает с шаблоном.");
            }
            return new Dictionary<string, string>(); // Возвращаем пустой словарь, если нет плейсхолдеров и строка соответствует
        }

        // [ ! ] паттерн для парсинга, заменяя {propertyName} на (.*?) для нежадного соответствия
        string pattern = Regex.Replace(_currentTemplateString, @"\{(\w+)\}", "(.*?)");
        Match match = Regex.Match(formattedString, pattern);

        if (!match.Success || match.Groups.Count - 1 != _templatePropertyNames.Count)
        {
            throw new Exception($"[ ! ] Отформатированная строка не соответствует шаблону или количество значений не совпадает. Ожидалось {_templatePropertyNames.Count} значений.");
        }

        Dictionary<string, string> parsedValues = new Dictionary<string, string>();
        for (int i = 0; i < _templatePropertyNames.Count; i++)
        {
            string placeholderName = _templatePropertyNames[i];
            string value = match.Groups[i + 1].Value;

            // Ключами в словаре должны быть те имена, которые будут использоваться для рефлексии
            string keyForReflection = PropertyMapping.ContainsKey(placeholderName) ? PropertyMapping[placeholderName] : placeholderName;
            parsedValues.Add(keyForReflection, value);
        }
        return parsedValues;
    }
}