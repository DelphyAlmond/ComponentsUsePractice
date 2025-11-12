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

        orderEntity = order ?? new Order(Guid.NewGuid(), string.Empty, string.Empty, string.Empty, null);
        LoadDataToForm();

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
            customChoiceComponent.Text = orderEntity.Destination;
        }

        // Обработка даты
        if (!string.IsNullOrEmpty(orderEntity.ReceiveDate))
        {
            ReceiveDTP.Value = orderEntity.GetRDate()!.Value;
            ReceiveDTP.Enabled = true;
        }
        else
        {
            ReceiveDTP.Enabled = false;
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
            orderEntity.ReceiveDate = customPatternComponent.Value;

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
