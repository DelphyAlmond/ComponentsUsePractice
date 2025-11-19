using ComponentLib;

namespace OrderControl
{
    public partial class OrderListComponent : UserControl
    {
        private CustomQListComponent customListC;
        private OrderDbConnection cDbConnection = new OrderDbConnection();

        // [ ? ] constructor parameters
        public OrderListComponent()
        {
            InitializeComponent();
            InitializeList();
        }

        private void InitializeList()
        {
            customListC = new CustomQListComponent { Dock = DockStyle.Fill };
            // 
            // customListC
            // 
            customListC.BackColor = Color.Cornsilk;
            customListC.Location = new Point(3, 0);
            customListC.Name = "customListC";
            customListC.Size = new Size(668, 741);
            customListC.TabIndex = 0;

            listPanel.Controls.Add(customListC);

            // [ * ] adapted : TemplateGen for Order entity
            customListC.SetTemplateForDisplay("ID: {Id}, Город: {Destination}, ФИО: {FIO}, Дата получения: {ReceiveDate}, Статус: {MovementNotes}.");

            RefreshOrderList();
        }

        public void RefreshOrderList()
        {
            var data = cDbConnection.GetOrders();
            if (data.Count == 0)
            {
                return;
            }
            customListC!.Clear();
            foreach (Order order in data) {
                customListC.AddItem(order.ValueString);
            }
        }

        private void addEditBtn_Click(object sender, EventArgs e)
        {
            EditAddForm addEditForm;
            if (customListC.GetDataLB.SelectedIndex != -1)
            {
                var order = customListC.GetItemFromSelected<Order>();

                var dbOrder = OrderDbConnection.GetOrder(order.Id);

                // DEBUG: Check what GUID we have
                if (dbOrder == null)
                {
                    MessageBox.Show($"> Order with ID {order.Id} not found in database!");

                    // CHECK : Show what orders actually exist in DB
                    var allOrders = new OrderDbConnection().GetOrders();
                    if (allOrders != null)
                    {
                        string existingIds = string.Join("\n", allOrders.Select(o => o.Id));
                        MessageBox.Show($"> Existing orders in DB:\n{existingIds}");
                    }
                    return;
                }
                addEditForm = new EditAddForm(order);
            }
            else
            {
                addEditForm = new EditAddForm();
            }

            addEditForm.RequestRefreshList += RefreshOrderList;
            addEditForm.Show();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"> Вы уверены, что хотите удалить запись?", "Удаление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var order = customListC!.GetItemFromSelected<Order>();
                cDbConnection.DeleteOrder(order.Id);
                RefreshOrderList();
            }
        }
    }
}
