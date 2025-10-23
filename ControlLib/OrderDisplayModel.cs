namespace ControlLib;

// Вспомогательная модель, чтобы GetItemFromSelected<T>() мог работать с OrderEntity
// (OrderEntity не имеет свойств с именами "city", "id" напрямую)
// ИЛИ можно адаптировать GetItemFromSelected так, чтобы он напрямую маппил на OrderEntity

public class OrderDisplayModel
{
    public string DestinationCity { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string CustomerFullName { get; set; } = string.Empty;
    public string OrderReceiveDate { get; set; } = string.Empty;
    public string MovementNotes { get; set; } = string.Empty;
}
