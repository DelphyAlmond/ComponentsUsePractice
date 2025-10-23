using ContractLib;

namespace ControlLib;

public class OrderManagementContract : IComponentContract
{
    public string ComponentId => "1d8b6c7a-4e3f-4d2c-8a1b-0e9f8a7b6c5d"; // Уникальный GUID
    public string ComponentName => "Управление заказами";
    public string ComponentCategory => "Справочники"; // Или "Операции"
    public UserControl GetComponentControl() => new OrderManagementComponent();
}
