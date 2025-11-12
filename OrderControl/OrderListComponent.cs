using ComponentLib;

namespace OrderControl
{
    public partial class OrderListComponent : UserControl
    {
        private CustomQListComponent cQlistControl;
        private OrderDbConnection cDbConnection;

        // [ ? ] constructor parameters
        public OrderListComponent()
        {
            InitializeComponent();
            InitializeList();
        }

        private void InitializeList()
        {
            cQlistControl = new CustomQListComponent();
            cQlistControl.Dock = DockStyle.Fill;
            Controls.Add(cQlistControl);

            // [ * ] adapted : TemplateGen for Order entity
            cQlistControl.SetTemplateForDisplay("ID: {Id}, Город: {Destination}, ФИО: {FIO}, Дата получения: {ReceiveDate}, Статус: {MovementNotes}.");

            RefreshOrderList();
        }

        public void RefreshOrderList()
        {
            var data = cDbConnection.GetOrders();
            if (data.Count == 0)
            {
                return;
            }
            cQlistControl!.Clear();
            foreach (Order order in data) {
                cQlistControl.AddItem(order.ValueString);
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditAddForm addEditForm = new EditAddForm();
            addEditForm.RequestRefreshList += RefreshOrderList;
            addEditForm.Show();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var order = cQlistControl.GetItemFromSelected<Order>();
            order.MovementNotes = cDbConnection.GetOrder(order.Id.Version)!.MovementNotes;
            EditAddForm addEditForm = new EditAddForm(order);
            addEditForm.RequestRefreshList += RefreshOrderList;
            addEditForm.Show();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"> Вы уверены, что хотите удалить запись?", "Удаление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var order = cQlistControl!.GetItemFromSelected<Order>();
                cDbConnection.DeleteOrder(order.Id.Version);
                RefreshOrderList();
            }
        }
    }
}
