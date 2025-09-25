using System.ComponentModel;
namespace ComponentLib;

public partial class CustomChoiceComponent : UserControl
{
    public CustomChoiceComponent()
    {
        InitializeComponent();
    }

    public event EventHandler ChangeSelection;

    // [ + ] Метод добавления
    public void AddItem(string item)
    {
        if (string.IsNullOrEmpty(item))
        {
            throw new ArgumentException("[ ! ] Item is null or empty.", nameof(item));
        }

        if (!ChoiceComboBox.Items.Contains(item)) // > проверка на уник.\несовпадение
        {
            ChoiceComboBox.Items.Add(item);
        }
        else
        {
            throw new ArgumentException("[ * ] Not unique.", nameof(item));
        }
    }

    public void btnAdd_Click(object sender, EventArgs e)
    {
        string itemToAdd = choiceTextBox.Text;
        AddItem(itemToAdd);
        choiceTextBox.Clear();
    }

    /* Что-то от лукавого, иначе не принимает отсутствие сериализации */
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

    // пуб. Свойство для установки (SET) и получения (GET)
    public string SelectedValue
    {
        get => ChoiceComboBox.SelectedItem?.ToString() ?? string.Empty;
        set
        {
            // Проверка: существует или пустое сущ.-ее значение
            if (ChoiceComboBox.Items.Contains(value))
            {
                ChoiceComboBox.SelectedItem = value;
            }
            else if (string.IsNullOrEmpty(value) && ChoiceComboBox.SelectedItem != null)
            {
                // Если передана пустая строка \ что-то было выбрано = снимаем выбор
                ChoiceComboBox.SelectedItem = null;
            }
        }
    }

    // Обработчик события
    private void ChoiceComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChangeSelection?.Invoke(this, e);
    }

    // [ - ] Метод очистки
    public void Clear() => ChoiceComboBox.Items.Clear();
}