namespace ReportsControl;

public partial class ReportDataGridView : UserControl
{
    private ReportDbConnection reportDb = new ReportDbConnection();

    public ReportDataGridView()
    {
        InitializeComponent();
        LoadCities();

        dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView.AllowUserToAddRows = false;
        dataGridView.AllowUserToDeleteRows = false;
        dataGridView.RowHeadersVisible = false;
        dataGridView.ReadOnly = true;
    }

    private void LoadCities()
    {
        var positions = reportDb.GetDestination();
        cityFilterCB.Items.AddRange(positions.ToArray());
    }

    private void ConfigureDataGridView()
    {
        dataGridView.Columns["Id"]!.Visible = false;
        dataGridView.Columns["FIO"]!.HeaderText = "ФИО";
        dataGridView.Columns["MovementNotes"]!.HeaderText = "Карта заказа";
        dataGridView.Columns["Destination"]!.HeaderText = "Город";
        dataGridView.Columns["ReceiveDate"]!.HeaderText = "Дата доставки";
    }

    private void buttonGenerate_Click(object sender, EventArgs e)
    {
        var selectedStatus = cityFilterCB.SelectedItem?.ToString();
        if (string.IsNullOrEmpty(selectedStatus))
        {
            MessageBox.Show("[ ! ] Не выбран город");
        }

        var customers = reportDb.GetEmployeesByPosition(selectedStatus!);
        dataGridView.DataSource = customers;

        ConfigureDataGridView();
    }
}
