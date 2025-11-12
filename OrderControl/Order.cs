namespace OrderControl;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FIO { get; set; }

    // Отметки о движении заказа (не более 6, в виде одной строки)
    // > Создан \ Обработан \ В пути \ Доставлен \ Получен \ Завершен
    public string MovementNotes { get; set; } = "Создан / Обработан";

    // Город назначения (> текстовое значение из справочника)
    // [ * ] filter criteria in reports
    public string Destination { get; set; }

    // Дата получения заказа (* 1-3 дня от текущей даты, но хранится конкретная дата)
    private string _receiveDate;
    public string ReceiveDate
    {
        get => _receiveDate;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                ValidateReceiveDate(value);
            }
            _receiveDate = value;
        }
    }

    // Validate date is within 1-3 days from current date
    private void ValidateReceiveDate(string dateString)
    {
        if (DateTime.TryParse(dateString, out DateTime date))
        {
            DateTime today = DateTime.Today;
            DateTime minDate = today.AddDays(1);  // Minimum: tomorrow
            DateTime maxDate = today.AddDays(3);  // Maximum: 3 days from today

            if (date.Date < minDate || date.Date > maxDate)
            {
                throw new ArgumentException(
                    $"[ ! ] Дата получения должна быть в диапазоне от {minDate:dd.MM.yyyy} до {maxDate:dd.MM.yyyy}");
            }
        }
        else
        {
            throw new ArgumentException("[ ! ] Некорректный формат даты");
        }
    }

    public Order() { }

    public string ValueString
    {
        get => string.Join(";", new List<string> { Id.ToString(), Destination, FIO, ReceiveDate, MovementNotes});
    }

    public Order(Guid id, string orderer, string notes, string city, string arrivingDate)
    {
        Id = id;
        FIO = orderer;
        MovementNotes = notes;
        Destination = city;
        ReceiveDate = arrivingDate;
    }

    public bool HasRDate() => ReceiveDate != null;
    // \ !string.IsNullOrEmpty(...)

    public DateTime? GetRDate()
    {
        if (HasRDate()) return DateTime.Parse(ReceiveDate);
        return null;
    }

    public void SetRDate(DateTime? date)
    {
        ReceiveDate = date?.ToString("YYYY.MM.DD") ?? string.Empty;
    }
}

