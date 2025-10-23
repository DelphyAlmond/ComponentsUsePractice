using ContractLib;

namespace ControlLib;

public class OrderReportContract : IComponentContract
{
    public string ComponentId => "c2b1a0f9-8e7d-4c6b-9a5d-4f3a2b1c0d9e"; // Уникальный GUID
    public string ComponentName => "Отчет по заказам на дату";
    public string ComponentCategory => "Отчеты";
    public UserControl GetComponentControl() => new OrderReportComponent();
}
