using ContractLib;

namespace ControlLib;

public class CityRefBookContract : IComponentContract
{
    public string ComponentId => "869e5d4d-f9f3-4e4b-a7e8-1c9f6a5b1d2e";
    public string ComponentName => "Справочник городов";
    public string ComponentCategory => "Справочники";
    public UserControl GetComponentControl() => new ReferenceBookComponent();
}
