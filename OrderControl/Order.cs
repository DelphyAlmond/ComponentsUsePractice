using System.ComponentModel.DataAnnotations.Schema;

namespace OrderControl;

public class Order
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("fio")]

    public string FIO { get; set; }

    // Отметки о движении заказа (не более 6, в виде одной строки)
    // > Создан \ Обработан \ В пути \ Доставлен \ Получен \ Завершен
    [Column("movementnotes")]
    public string MovementNotes { get; set; }

    // Город назначения (> текстовое значение из справочника)
    // [ * ] filter criteria in reports
    [Column("destination")]
    public string Destination { get; set; }

    // Дата получения заказа (* 1-3 дня от текущей даты, но хранится конкретная дата)
    [Column("receivedate")]
    private string ReceiveDate;
    public string GetSetReceiveDate
    {
        get => ReceiveDate;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                ValidateReceiveDate(value);
            }
            ReceiveDate = value;
        }
    }

    // Validate date is within 1-3 days from current date
    private static void ValidateReceiveDate(string dateString)
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

    public string ValueString
    {
        get => string.Join(";", new List<string> { Id.ToString() ?? " ", Destination ?? " ", FIO ?? " ", GetSetReceiveDate ?? " ", MovementNotes ?? " " });
    }

    public Order(Guid id, string orderer, string notes, string city, string arrivingDate)
    {
        Id = id;
        FIO = orderer;
        MovementNotes = notes;
        Destination = city;
        GetSetReceiveDate = arrivingDate;
    }

    public Order()
    {
        Id = Guid.NewGuid();
    }

    public bool HasRDate() => GetSetReceiveDate != null;
    // \ !string.IsNullOrEmpty(...)

    public DateTime? GetRDate()
    {
        if (HasRDate()) return DateTime.Parse(GetSetReceiveDate);
        return null;
    }

    public void SetRDate(DateTime? date)
    {
        GetSetReceiveDate = date?.ToString("YYYY.MM.DD") ?? string.Empty;
    }
}

