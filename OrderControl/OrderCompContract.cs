using ContractLib;

namespace OrderControl;
public class OrderCompContract : IComponentContract
{
    public string Id => "c2_order";
    public string MenuTitle => "< Заказы >";
    public string Category => "ComplexReference";
    public UserControl GetComponentControl => new OrderListComponent();
}
