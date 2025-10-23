namespace ContractLib;

public static class OrderManager
{
    private static List<OrderEntity> _orders = new List<OrderEntity>();

    static OrderManager()
    {
        // Предварительное заполнение для демонстрации
        AddOrder(new OrderEntity
        {
            CustomerFullName = "Иванов И.И.",
            MovementNotes = "Создан;В пути",
            DestinationCity = "Москва",
            OrderReceiveDate = DateTime.Today.AddDays(2)
        });
        AddOrder(new OrderEntity
        {
            CustomerFullName = "Петрова А.С.",
            MovementNotes = "Создан",
            DestinationCity = "Санкт-Петербург",
            OrderReceiveDate = DateTime.Today.AddDays(1)
        });
    }

    public static IReadOnlyList<OrderEntity> GetAllOrders() => _orders.AsReadOnly();

    public static void AddOrder(OrderEntity order)
    {
        if (string.IsNullOrWhiteSpace(order.CustomerFullName))
            throw new ArgumentException("[ ! ] ФИО заказчика не может быть пустым.");

        _orders.Add(order);
    }

    public static void UpdateOrder(OrderEntity updatedOrder)
    {
        if (string.IsNullOrWhiteSpace(updatedOrder.CustomerFullName))
            throw new ArgumentException("[ ! ] ФИО заказчика не может быть пустым.");

        var existingOrder = _orders.FirstOrDefault(o => o.Id == updatedOrder.Id);
        if (existingOrder == null)
            throw new KeyNotFoundException($"Заказ с ID '{updatedOrder.Id}' не найден.");

        existingOrder.CustomerFullName = updatedOrder.CustomerFullName;
        existingOrder.MovementNotes = updatedOrder.MovementNotes;
        existingOrder.DestinationCity = updatedOrder.DestinationCity;
        existingOrder.OrderReceiveDate = updatedOrder.OrderReceiveDate;
    }

    public static void DeleteOrder(Guid orderId)
    {
        _orders.RemoveAll(o => o.Id == orderId);
    }

    public static IReadOnlyList<OrderEntity> GetOrdersByReceiveDate(DateTime date)
    {
        return _orders.Where(o => o.OrderReceiveDate.Date == date.Date).ToList().AsReadOnly();
    }
}
