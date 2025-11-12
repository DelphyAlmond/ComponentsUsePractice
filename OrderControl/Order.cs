namespace OrderControl;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FIO { get; set; }

    // Отметки о движении заказа (не более 6, в виде одной строки)
    // > Создан \ Обработан \ В пути \ Доставлен \ Получен \ Завершен
    public string MovementNotes { get; set; } = "Создан / Обработан";

    // Город назначения (> текстовое значение из справочника)
    public string Destination { get; set; }
    
    // Дата получения заказа (* 1-3 дня от текущей даты, но хранится конкретная дата)
    public string ReceiveDate { get; set; } // [ * ] filter criteria in reports

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

