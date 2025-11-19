using System.Windows.Forms;

namespace CityControl
{
    public partial class CityListDropDown : UserControl
    {
        private DestinationDbConnection cDbConnection = new DestinationDbConnection();
        private List<string> currentCityList = new List<string>();

        public CityListDropDown()
        {
            InitializeComponent();
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            citiesDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            citiesDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            citiesDGV.AllowUserToAddRows = false;
            citiesDGV.AllowUserToDeleteRows = false;
            citiesDGV.RowHeadersVisible = false;

            var column = new DataGridViewTextBoxColumn();
            column.HeaderText = "Город";
            column.Name = "DestinationColumn";
            citiesDGV.Columns.Add(column);

            // * citiesDGV.CellEndEdit += EditingCell!;

            RefreshDataFromDatabase();
        }

        private void RefreshDataFromDatabase()
        {
            try
            {
                currentCityList = cDbConnection.ReadCities();
                citiesDGV.Rows.Clear();
                foreach (var position in currentCityList)
                {
                    citiesDGV.Rows.Add(position);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных из базы: {ex.Message}");
            }
        }
    }
}
