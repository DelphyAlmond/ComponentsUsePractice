namespace ComponentLib;

public class StringFactory
{
    // Внутренние поля для хранения конфигурации шаблона
    private string _currentTemplateString = string.Empty;
    private char _propertyStartChar = '{';
    private char _propertyEndChar = '}';

    // Список фраз : изменяемый шаблон бляяяяя [ ! ]
    public List<string> _phrasesInTemplate = new List<string>
    {
        "Параметр измерения ... :",
        "Величина ... :",
        "Данные:",
        "Значение:",
        "Результат чего-то ... :",
        "Индекс:",
        "Прочий текст со смысловой нагрузкой о каких-то вычислениях:"
    };

    // Публичное свойство для установки и получения шаблона.
    // При установке автоматически вызывает ConfigureTemplate для валидации.
    public string TemplateString
    {
        get => _currentTemplateString;
        set => ValidateAndParseTemplate(value);
    }

    public void ValidateAndParseTemplate(string template)
    {
        _currentTemplateString = template ?? string.Empty;
        _phrasesInTemplate.Clear(); // очистка старых фраз

        // 1. Проверка : на четность скобок
        int openBraceCount = _currentTemplateString.Count(c => c == _propertyStartChar);
        int closeBraceCount = _currentTemplateString.Count(c => c == _propertyEndChar);

        if (openBraceCount != closeBraceCount)
        {
            throw new Exception("[ ! ] Количество открывающих и закрывающих скобок не совпадает.");
        }

        // 2. Проверка: шаблон не должен начинаться со свойства (т.е. с пустой скобки)
        if (_currentTemplateString.StartsWith(_propertyStartChar.ToString() + _propertyEndChar.ToString()))
        {
            throw new Exception("[ ! ] Шаблон не должен начинаться со свойства.");
        }

        // 3. Проверка: в строке не должно идти 2 свойства подряд
        string consecutivePattern = _propertyEndChar.ToString() + _propertyStartChar.ToString();
        if (_currentTemplateString.Contains(consecutivePattern))
        {
            throw new Exception("[ ! ] Шаблон не должен содержать два свойства подряд. Между ними должен быть текст.");
        }

        // Разбор шаблона на текстовые куски
        string[] parts = _currentTemplateString.Split(new[] { _propertyStartChar.ToString() + _propertyEndChar.ToString() }, StringSplitOptions.None);

        // Если шаблон пуст или не содержит {}, просто добавляем его целиком
        if (parts.Length == 1 && !_currentTemplateString.Contains(consecutivePattern))
        {
            throw new Exception("[ ! ] Шаблон должен быть содержательным. Добавьте '{}'.");
        }
        else
        {
            foreach (var part in parts)
            {
                _phrasesInTemplate.Add(part);
            }
        }

        // [ * ] шаблон заканчивается на {} - Split оставит последнюю часть пустой
        if (_currentTemplateString.EndsWith(_propertyStartChar.ToString() + _propertyEndChar.ToString()) && parts.Length > 0 && parts.Last() != "")
        {
            // *** В текущей логике: если "A{}B{}", то _phrasesInTemplate будет ["A", "B", ""].
            // "A{}B" -> ["A", "B"].
            // "A" -> ["A"].
        }
    }

    public List<string> AllPhrases => _phrasesInTemplate;

    // Значения сопоставляются плейсхолдерам
    // [ ! ] только те пары "Фраза: Значение", для которых есть данные
    public string FormatOutputString(List<string> values)
    {
        if (values == null || values.Count < 2)
        {
            throw new ArgumentException("[ ! ] Количество значений должно быть от 2");
        }

        int expectedPlaceholders = (_phrasesInTemplate.Count > 0) ? (_phrasesInTemplate.Count - 1) : 0;

        if (values.Count != expectedPlaceholders)
        {
            throw new Exception($"[ ! ] Количество значений ({values.Count}) не соответствует количеству плейсхолдеров в шаблоне ({expectedPlaceholders}).");
        }

        List<string> parts = new List<string>();
        for (int i = 0; i < values.Count; i++)
        {
            if (i < _phrasesInTemplate.Count)
            {
                parts.Add($"{_phrasesInTemplate[i]}: {values[i]}");
            }
        }
        return string.Join(", ", parts);
    }
}
