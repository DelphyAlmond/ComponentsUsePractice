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

        if (!choiceComboBox.Items.Contains(item)) // > проверка на уник.\несовпадение
        {
            choiceComboBox.Items.Add(item);
        }
        else
        {
            throw new ArgumentException("[ * ] Not unique.", nameof(item));
        }
    }

    /* Что-то от лукавого, иначе не принимает отсутствие сериализации */
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

    // пуб. Свойство для установки (SET) и получения (GET)
    public string SelectedValue
    {
        get => choiceComboBox.SelectedItem?.ToString() ?? string.Empty;
        set
        {
            // Проверка: существует или пустое сущ.-ее значение
            if (choiceComboBox.Items.Contains(value))
            {
                choiceComboBox.SelectedItem = value;
            }
            else if (string.IsNullOrEmpty(value) && choiceComboBox.SelectedItem != null)
            {
                // Если передана пустая строка \ что-то было выбрано = снимаем выбор
                choiceComboBox.SelectedItem = null;
            }
        }
    }

    // Обработчик события
    private void ChoiceComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChangeSelection?.Invoke(this, e);
    }

    // [ - ] Метод очистки
    public void Clear() => choiceComboBox.Items.Clear();
}