using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using ComponentLib;
using ContractLib;

namespace ControlLib
{
    public partial class OrderEditor : Form
    {
        private readonly CompToolTipManager _toolTipManager = new CompToolTipManager();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OrderEntity orderEntity { get; private set; } // Для получения/установки данных
        private bool _isEditMode = false;
        private OrderEntity _originalOrder = new OrderEntity();
        private bool _isChanged = false; // Флаг изменения

        public OrderEditor() // > для созд. абсолютно нового
        {
            InitializeComponent();
            orderEntity = new OrderEntity();
            _isEditMode = false;
            InitializeControls();
        }

        public OrderEditor(OrderEntity orderToEdit) // > для редактирования
        {
            InitializeComponent();
            orderEntity = orderToEdit;
            _originalOrder = orderToEdit.DeepCopy();
            // reference on the original entity for checking etits
            _isEditMode = true;
            InitializeControls();
            LoadOrderData();
        }

        private void InitializeControls()
        {
            customChoiceComponent1.Clear();

            foreach (var city in CityManager.GetAllCities())
            {
                customChoiceComponent1.AddItem(city);
            }

            // [ ! ] Настройка CustomPatternComponent (Date)
            customPatternComponent1.ValidationPattern = new Regex(@"^\d{4}\.\d{2}\.\d{2}$"); // YYYY.MM.DD

            customerNameTb.TextChanged += MarkAsChanged;
            statusNoteTb.TextChanged += MarkAsChanged;
            customChoiceComponent1.ChangeSelection += (sender, e) => MarkAsChanged(sender, e);
            customPatternComponent1.ValueChanged += (sender, e) => MarkAsChanged(sender, e);
        }

        private void LoadOrderData()
        {
            customerNameTb.Text = orderEntity.CustomerFullName;
            statusNoteTb.Text = orderEntity.MovementNotes;
            customChoiceComponent1.SelectedValue = orderEntity.DestinationCity;
            customPatternComponent1.Value = orderEntity.OrderReceiveDate.ToShortDateString();
            _isChanged = false; // Сброс
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customerNameTb.Text))
                    throw new ArgumentException("[ * ] ФИО заказчика обязательно.");
                if (string.IsNullOrWhiteSpace(customChoiceComponent1.SelectedValue))
                    throw new ArgumentException("[ * ] Город назначения обязателен.");
                if (!customPatternComponent1.ValidationPattern.IsMatch(customPatternComponent1.Value))
                    throw new ArgumentException("[ * ] Дата получения заказа не соответствует шаблону.");

                DateTime receiveDate;
                if (!DateTime.TryParse(customPatternComponent1.Value, out receiveDate))
                    throw new ArgumentException("[ ! ] Некорректная дата получения заказа.");

                // Валидация диапазона 1-3 дня [!]
                if (receiveDate < DateTime.Today.AddDays(1) || receiveDate > DateTime.Today.AddDays(3))
                    throw new ArgumentException("[ ! ] Дата получения заказа должна быть в диапазоне от завтра до 3 дней от текущей даты.");

                // Перезатераем объект OrderEntity
                orderEntity.CustomerFullName = customerNameTb.Text;
                orderEntity.MovementNotes = statusNoteTb.Text;
                orderEntity.DestinationCity = customChoiceComponent1.SelectedValue;
                orderEntity.OrderReceiveDate = receiveDate;

                this.DialogResult = DialogResult.OK; // Закрыть форму с результатом OK
            }
            catch (Exception ex)
            {
                _toolTipManager.ShowError(this, ex.Message);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void OrderEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK && _isChanged)
            {
                if (MessageBox.Show("[ ! ] Несохраненные изменения. Закрыть без сохранения?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void MarkAsChanged(object? sender, EventArgs e)
        {
            _isChanged = true;
        }
    }
}
