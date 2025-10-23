namespace ContractLib;

public class OrderEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CustomerFullName { get; set; } = string.Empty;

    // Отметки о движении заказа (не более 6, в виде одной строки)
    // > Создан \ Обработан \ В пути \ Доставлен \ Получен \ Завершен
    public string MovementNotes { get; set; } = string.Empty;

    // Город назначения (текстовое значение из справочника)
    public string DestinationCity { get; set; } = string.Empty;

    // Дата получения заказа (1-3 дня от текущей даты, но хранится конкретная дата)
    public DateTime OrderReceiveDate { get; set; }

    public OrderEntity DeepCopy()
    {
        return new OrderEntity
        {
            Id = this.Id, // [ ! ] GUID является значимым типом, копируется по значению
            CustomerFullName = this.CustomerFullName,
            MovementNotes = this.MovementNotes,
            DestinationCity = this.DestinationCity,
            OrderReceiveDate = this.OrderReceiveDate
        };
    }
}
