using ContractLib;

namespace CityControl;

internal class CityCompContract : IComponentContract
{
    public string Id => "c1_destination";
    public string MenuTitle => "Города";
    public string Category => "SimpleReference";
    public UserControl GetComponentControl => new CityListDropDown();
}
