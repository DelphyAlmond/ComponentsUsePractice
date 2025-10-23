using System.ComponentModel;
using System.Data;

using ComponentLib;
using ContractLib;
namespace ControlLib;

public partial class ReferenceBookComponent : UserControl
{
    private BindingList<string> _cities = new BindingList<string>();
    private readonly CompToolTipManager _toolTipManager = new CompToolTipManager();

    // > для хранения старого значения при редактировании
    private string _editingOriginalCityName = string.Empty;

    public ReferenceBookComponent()
    {
        InitializeComponent();
        InitializeDataGridView();
        LoadCities();
    }

    private void InitializeDataGridView()
    {
        DataGridView.AutoGenerateColumns = false;

        DataGridView.DataSource = _cities; // > привязка к BindingList<string>

        DataGridView.CellBeginEdit += DataGridView_CellBeginEdit;
        DataGridView.CellEndEdit += DataGridView_CellEndEdit;
        // > UserDeletingRow для обработки нажатия Delete на выделенной строке
        DataGridView.UserDeletingRow += DataGridView_UserDeletingRow;

        // + события клавиатуры UserControl для Insert и Delete [ ! ]
        DataGridView.KeyDown += optionsSelected;
    }

    // Загружает (или перезагружает) список городов из CityManager в DataGridView
    private void LoadCities()
    {
        _cities.Clear();
        //+ sorting*
        foreach (var city in CityManager.GetAllCities().OrderBy(c => c))
        {
            _cities.Add(city);
        }
        _cities.ResetBindings();
    }

    private void optionsSelected(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Insert)
        {
            addBtn_Click(sender, e); // > вызов логики кнопки "Добавить"
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Delete) // Удаление по клавише Delete
        {
            // ? есть ли выделенные строки > список выделенных
            if (DataGridView.SelectedRows.Count > 0)
            {
                var selectedCityNames = DataGridView.SelectedRows.Cast<DataGridViewRow>()
                                                      .Select(r => r.DataBoundItem as string)
                                                      .Where(name => !string.IsNullOrEmpty(name))
                                                      .ToList();
                if (selectedCityNames.Any())
                {
                    if (MessageBox.Show($"[ * ] Вы уверены, что хотите удалить следующие города ({selectedCityNames.Count}):" +
                        $"{Environment.NewLine}{string.Join(Environment.NewLine, selectedCityNames)}?",
                        "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            CityManager.DeleteCities(selectedCityNames);
                            LoadCities(); // [ * ] перезагрузка
                            _toolTipManager.ShowInfo(DataGridView, $"> Удалено {selectedCityNames.Count} городов.");
                        }
                        catch (Exception ex)
                        {
                            _toolTipManager.ShowError(DataGridView, $"[ ! ] Ошибка при удалении: {ex.Message}");
                        }
                    }
                }
            }
            e.Handled = true;
        }
    }

    // Обработка событий редактирования ячеек
    private void DataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
        // [ ! ] сохраняем исходное значение ячейки, чтобы использовать его для UpdateCity
        if (e.RowIndex >= 0 && e.ColumnIndex == 0)
        {
            _editingOriginalCityName = _cities[e.RowIndex];
        }
    }

    private void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex != 0) return;

        var newCityName = DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();

        if (string.IsNullOrWhiteSpace(newCityName))
        {
            _toolTipManager.ShowWarning(DataGridView, "[ ! ] Название города - пусто. Запись не сохранена.");
            // Откатить изменения:
            if (string.IsNullOrEmpty(_editingOriginalCityName)) // - новая (пустая) запись
            {
                _cities.RemoveAt(e.RowIndex); // removed*
            }
            else
            {
                _cities[e.RowIndex] = _editingOriginalCityName; // - старое имя
                _cities.ResetItem(e.RowIndex); // upd*
            }
            return;
        }

        try
        {
            if (string.IsNullOrEmpty(_editingOriginalCityName)) // new record [ ! ]
            {
                CityManager.AddCity(newCityName);
                _toolTipManager.ShowInfo(DataGridView, $"> Город '{newCityName}' добавлен.");
            }
            else // existing record [ ! ]
            {
                CityManager.UpdateCity(_editingOriginalCityName, newCityName);
                _toolTipManager.ShowInfo(DataGridView, $"> Город '{newCityName}' обновлен.");
            }
            LoadCities(); // reload*
        }
        catch (ArgumentException ex)
        {
            _toolTipManager.ShowError(DataGridView, ex.Message);
            LoadCities();
        }
        _editingOriginalCityName = string.Empty; // Сброс
    }

    // UserDeletingRow событие - для подтверждения удаления, когда пользователь жмет Delete
    private void DataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
        var cityName = e.Row.DataBoundItem as string;
        if (string.IsNullOrEmpty(cityName)) return;

        if (MessageBox.Show($"[ * ] Вы уверены, что хотите удалить город '{cityName}'?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        {
            e.Cancel = true; // Отменить
        }
        else
        {
            try
            {
                CityManager.DeleteCities(new List<string> { cityName });
                _toolTipManager.ShowInfo(DataGridView, $"> Город '{cityName}' удален.");
            }
            catch (Exception ex)
            {
                _toolTipManager.ShowError(DataGridView, $"[ ! ] Ошибка при удалении: {ex.Message}");
                e.Cancel = true;
            }
        }
    }

    private void addBtn_Click(object sender, EventArgs e)
    {
        _cities.Add(""); // > пустая строка для новой записи (дополнится)
        DataGridView.CurrentCell = DataGridView.Rows[DataGridView.Rows.Count - 1].Cells[0];
        DataGridView.BeginEdit(true); // > в режим редактирования
    }
}

