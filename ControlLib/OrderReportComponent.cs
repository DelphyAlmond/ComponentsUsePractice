using ComponentLib;
using ContractLib;

namespace ControlLib
{
    public partial class OrderReportComponent : UserControl
    {
        private readonly CompToolTipManager _toolTipManager = new CompToolTipManager();
        public OrderReportComponent()
        {
            InitializeComponent();
            InitializeDataGridView();
            dateTimePicker.Value = DateTime.Today; // default: сегодня
        }

        private void InitializeDataGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            // + колонки 
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CustomerFullName", HeaderText = "ФИО Заказчика", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DestinationCity", HeaderText = "Город", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "OrderReceiveDate", HeaderText = "Дата получения", AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MovementNotes", HeaderText = "Движение заказа", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        }

        private void GenerateReportButton_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime selectedDate = dateTimePicker.Value.Date;
                var orders = OrderManager.GetOrdersByReceiveDate(selectedDate);
                dataGridView.DataSource = orders.ToList(); // ToList() для BindingSource или прямого присваивания
                _toolTipManager.ShowInfo(generateReportButton, $"Отчет сформирован. Найдено {orders.Count} заказов на {selectedDate.ToShortDateString()}.");
            }
            catch (Exception ex)
            {
                _toolTipManager.ShowError(generateReportButton, $"Ошибка при формировании отчета: {ex.Message}");
            }
        }
    }
}
