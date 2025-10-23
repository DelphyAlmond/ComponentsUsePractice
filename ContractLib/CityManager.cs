namespace ContractLib;

public static class CityManager
{
    private static List<string> _cities = new List<string>();

    static CityManager()
    {
        // Предварительное заполнение для демонстрации
        AddCity("Москва");
        AddCity("Санкт-Петербург");
        AddCity("Казань");
        AddCity("Новосибирск");
    }

    public static IReadOnlyList<string> GetAllCities() => _cities.AsReadOnly();

    public static void AddCity(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            throw new ArgumentException("[ ! ] Название города не может быть пустым.");
        if (_cities.Any(c => c.Equals(cityName, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException($"[ * ] Город '{cityName}' уже существует.");

        _cities.Add(cityName);
    }

    public static void UpdateCity(string oldName, string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("[ ! ] Новое название города не может быть пустым.");

        var index = _cities.FindIndex(c => c.Equals(oldName, StringComparison.OrdinalIgnoreCase));
        if (index == -1)
            throw new KeyNotFoundException($"[ * ] Город '{oldName}' не найден.");

        // Проверка : среди других городов (исключая сам старый город)
        if (_cities.Any(c => !c.Equals(oldName, StringComparison.OrdinalIgnoreCase) && c.Equals(newName, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException($"[ * ] Город '{newName}' уже существует.");

        _cities[index] = newName;
    }

    public static void DeleteCities(IEnumerable<string> cityNames)
    {
        _cities.RemoveAll(c => cityNames.Contains(c, StringComparer.OrdinalIgnoreCase));
    }
}
