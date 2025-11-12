using ComponentLib;
using ContractLib;

namespace ControlLib;

public partial class OrderManagementComponent : UserControl
{
    private readonly CompToolTipManager _toolTipManager = new CompToolTipManager();
    private readonly CustomQListComponent _customQListComponent;

    // [ + ] constructor parameters
    public OrderManagementComponent(string? displayTemplate, Dictionary<string, string>? fieldMap)
    {
        InitializeComponent();

        _customQListComponent = new CustomQListComponent();
        _customQListComponent.Dock = DockStyle.Fill;
        Controls.Add(_customQListComponent);

        // [ * ] enable parameters settling from outside
        _customQListComponent.SetTemplateForDisplay(displayTemplate);

        _customQListComponent.SelectionChanged += (sender, e) => _toolTipManager.Hide(_customQListComponent);
    }

    private void LoadOrders()
    {
        _customQListComponent.Clear(); // Очищаем старые записи
        foreach (var order in OrderManager.GetAllOrders())
        {
            // Для CustomQListComponent нужны строки, поэтому преобразуем OrderEntity
            var values = new Dictionary<string, string>
            {
                { "city", order.DestinationCity },
                { "id", order.Id.ToString() }, // Guid в строку
                { "customer", order.CustomerFullName },
                { "receiveDate", order.OrderReceiveDate.ToShortDateString() } // Дата в строку
            };
            _customQListComponent.AddItem(values);
        }
    }

    // > Добавить
    private void AddNewOrder()
    {
        using (var form = new OrderEditor())
        {
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    OrderManager.AddOrder(form.orderEntity);
                    LoadOrders(); // Перезагрузить список
                    _toolTipManager.ShowInfo(_customQListComponent, "Заказ успешно добавлен.");
                }
                catch (Exception ex)
                {
                    _toolTipManager.ShowError(_customQListComponent, $"Ошибка при добавлении заказа: {ex.Message}");
                }
            }
        }
    }

    // > Редактировать
    private void EditSelectedOrder()
    {
        if (_customQListComponent.dataListBox.SelectedItem == null)
        {
            _toolTipManager.ShowWarning(_customQListComponent, "Выберите заказ для редактирования.");
            return;
        }

        try
        {
            // > словарь значений из выбранной строки
            var selectedValuesDict = _customQListComponent.GetItemFromSelected<OrderDisplayModel>();

            // [ ! ] Преобразуем в OrderEntity
            var orderToEdit = new OrderEntity
            {
                Id = Guid.Parse(selectedValuesDict.Id),
                CustomerFullName = selectedValuesDict.CustomerFullName,
                MovementNotes = selectedValuesDict.MovementNotes, // Предполагаем, что CustomQListComponent может вернуть это
                DestinationCity = selectedValuesDict.DestinationCity,
                OrderReceiveDate = DateTime.Parse(selectedValuesDict.OrderReceiveDate)
            };

            using (var form = new OrderEditor(orderToEdit)) // Передаем существующий заказ
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        OrderManager.UpdateOrder(form.orderEntity);
                        LoadOrders(); // Перезагрузить список
                        _toolTipManager.ShowInfo(_customQListComponent, "Заказ успешно обновлен.");
                    }
                    catch (Exception ex)
                    {
                        _toolTipManager.ShowError(_customQListComponent, $"Ошибка при обновлении заказа: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _toolTipManager.ShowError(_customQListComponent, $"Ошибка при загрузке заказа для редактирования: {ex.Message}");
        }
    }

    // > Удалить
    private void DeleteSelectedOrder()
    {
        if (_customQListComponent.dataListBox.SelectedItem == null)
        {
            _toolTipManager.ShowWarning(_customQListComponent, "Выберите заказ для удаления.");
            return;
        }

        if (MessageBox.Show("Вы уверены, что хотите удалить выбранный заказ?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            try
            {
                // Для удаления нужен ID. Нужно спарсить строку и извлечь ID
                var selectedValues = _customQListComponent.GetItemFromSelected<OrderDisplayModel>();
                Guid orderId = Guid.Parse(selectedValues.Id); // Убедитесь, что ID корректно парсится

                OrderManager.DeleteOrder(orderId);
                LoadOrders(); // Перезагрузить список
                _toolTipManager.ShowInfo(_customQListComponent, "Заказ успешно удален.");
            }
            catch (Exception ex)
            {
                _toolTipManager.ShowError(_customQListComponent, $"Ошибка при удалении заказа: {ex.Message}");
            }
        }
    }
}
