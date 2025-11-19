using System.Text.RegularExpressions;

public class TemplateGen
{
    // Шаблонная строка
    private string _currentTemplateString = string.Empty;

    // Регулярное выражение типа {propertyName}
    private static readonly Regex _placeholderRegex = new Regex(@"\{(\w+)\}"); // static readonly

    // (разбитая шабл. строка) > Словарь для сопоставления, фраза - имя поля\св.-ва
    private Dictionary<string, string> _phraseMapping = new Dictionary<string, string>();

    public Dictionary<string, string> PhraseMapping
    {
        get => _phraseMapping;
        set
        {
            if (value == null) throw new ArgumentNullException(nameof(_phraseMapping),
                "[ ! ] Словарь сопоставления свойств не может быть null.");
            _phraseMapping = value;
        }
    }

    public string TemplateString
    {
        get => _currentTemplateString;
        set => SetNewTemplate(value);
    }

    public TemplateGen()
    {
        // [ default ] Устанавливаем шаблон при создании экземпляра:
        TemplateString = "Кол-во параметра {param} составляет {percent}%, а результат вычисления: {num}.";
    }

    public void SetCustomMapping(Dictionary<string, string> newMapping)
    {
        PhraseMapping = newMapping;
    }

    private void ValidateAndParseTemplate(string template)
    {
        // 1. Проверка на валидность регулярным выражением (например, отсутствие пустых скобок {})
        if (template.Contains("{}"))
        {
            throw new Exception("[ ! ] Шаблон не должен содержать пустые плейсхолдеры '{}'. Используйте именованные, например '{propertyName}'.");
        }

        // 2. Проверка на 2 свойства подряд ({prop1}{prop2} / {prop1} {prop2})
        if (template.Contains("}{"))
        {
            throw new Exception("[ ! ] Шаблон не должен содержать два свойства подряд без текста между ними.");
        }
        if (template.Contains("} {"))
        {
            throw new Exception("[ ! ] Шаблон не должен содержать два свойства подряд с пробелом между ними без текста.");
        }

        if (string.IsNullOrEmpty(template) || string.IsNullOrWhiteSpace(template))
        {
            throw new Exception("[ ! ] Шаблон не должен быть пустым.");
        }

        _currentTemplateString = template;
    }

    // Парсинг: > Dictionary<string, string>
    public void SetNewTemplate(string stringForParsing)
    {
        if (string.IsNullOrEmpty(stringForParsing)) throw new ArgumentNullException("[ ! ] Форматированная строка не может быть пустой.");

        try
        {
            ValidateAndParseTemplate(stringForParsing);

            // 3. [ ! ] Находим все плейсхолдеры и извлекаем их имена
            MatchCollection matches = _placeholderRegex.Matches(_currentTemplateString);
            List<string> fields4values = [];
            for (int i = 0; i < matches.Count; i++)
            {
                //    0   >> 1               2       > foreach Match match:
                // [ '{', 'field-atribute', '}' ] => match.Groups[1].Value - содержимое внутри скобок
                fields4values.Add(matches[i].Groups[1].Value);
            }

            // - то же с фразами
            List<string> phrases4keys = Regex.Split(_currentTemplateString, @"\{.*?\}")
                .Where(part => !string.IsNullOrEmpty(part))
                .ToList();

            // 4. Комбинируем попарно
            Dictionary<string, string> parsedConnectionsDict = new Dictionary<string, string>();
            for (int i = 0; i < phrases4keys.Count; i++)
            {
                // Ключами в словаре будут части фразы, значениями - имена полей (* для рефлексии)
                if (i <= fields4values.Count - 1) parsedConnectionsDict.Add(phrases4keys[i], fields4values[i]);
                else parsedConnectionsDict.Add(phrases4keys[i], "");
            }

            PhraseMapping = parsedConnectionsDict;
        }
        catch
        {
            throw new Exception("[ ! ] Шаблон не прошёл проверку.");
        }
    }
}