using System.Windows.Forms;

namespace ContractEntityProj
{
    public interface IComponentContract
    {
        string ComponentId { get; } // Уник. id компонента (GUID)
        string ComponentName { get; } // Название ком.-та для отображения в меню ("Города", "Заказы", "Отчет по дате")
        string ComponentCategory { get; } // Категория ("Справочники" / "Отчеты")
        UserControl GetComponentControl(); // [ ! ] метод получения основного UserControl компонента
    }
}
