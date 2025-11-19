using System.Text.RegularExpressions;

namespace OrderControl;

public partial class EditAddForm : Form
{
    private Order orderEntity;
    private OrderDbConnection DBconnection = new OrderDbConnection();
    public event Action RequestRefreshList;

    bool isModified = false;

    public EditAddForm(Order? order = null)
    {
        InitializeComponent();

        orderEntity = order ?? new Order(Guid.NewGuid(), string.Empty, string.Empty, string.Empty, string.Empty);
        LoadDataToForm();
        currentDestinationTB.Enabled = false;

        fioTB.TextChanged += (sender, e) => isModified = true;
        notesTB.TextChanged += (sender, e) => isModified = true;
    }

    private void LoadDataToForm()
    {
        fioTB.Text = orderEntity.FIO;
        notesTB.Text = orderEntity.MovementNotes;

        List<string> cities = DBconnection.GetCities();
        customChoiceComponent.Clear();
        foreach (string c in cities) customChoiceComponent.AddItem(c);

        if (cities.Contains(orderEntity.Destination))
        {
            currentDestinationTB.Text = orderEntity.Destination.ToString();
        }

        // Обработка даты
        if (orderEntity.GetRDate() != null)
        {
            ReceiveDTP.Value = orderEntity.GetRDate().Value;
            customPatternComponent.Value = orderEntity.GetSetReceiveDate;
        }
        else
        {
            ReceiveDTP.Value = DateTime.Today;
        }
    }

    private void EditAddForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (isModified)
        {
            var result = MessageBox.Show("[ * ] Есть несохраненные изменения. Вы действительно хотите выйти без сохранения?", "Да", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }

    private void confirmBtn_Click(object sender, EventArgs e)
    {
        try
        {
            orderEntity.FIO = fioTB.Text;
            orderEntity.MovementNotes = notesTB.Text;
            orderEntity.Destination = customChoiceComponent.Text;

            customPatternComponent.Value = ReceiveDTP.Value.ToString("yyyy.MM.dd");
            orderEntity.GetSetReceiveDate = customPatternComponent.Value;

            if (orderEntity.Id.Version == 0)
            {
                DBconnection.AddOrder(orderEntity);
            }
            else
            {
                DBconnection.UpdateOrder(orderEntity);
            }
            isModified = false;
            RequestRefreshList.Invoke();
        }
        catch (Exception ex)
        {
            throw new Exception("[ ! ] Couldn't confirm changes: " + ex.Message);
        }
    }
}
