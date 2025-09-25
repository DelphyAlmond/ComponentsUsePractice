namespace FormComponentDisplay;
// using ComponentLib;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

public partial class FormMain : Form
{
    public FormMain()
    {
        InitializeComponent();
        try
        {
            var extensions = LoadExtensions();
            foreach (var extension in //выборка контролов для пункта меню «Справочники») 
   {
                var menu = new ToolStripMenuItem
                {
                    Text = //заголовок подпункта меню
                };
                menu.Click += (sender, e) =>
                {
                    OpenControl(); 
    };
                DirectoriesToolStripMenuItem.DropDownItems.Add(menu);
            }
            foreach (var extension in //выборка контролов для пункта меню «Отчеты») 
   {
                _controls.Add(extension.Id, extension.Control);
                var menu = new ToolStripMenuItem
                {
                    Text = //заголовок 
            подпункта меню
                };
                menu.Click += (sender, e) =>
                {
                    OpenControl() //передаем что потребуется; 
    };
                ReportsToolStripMenuItem.DropDownItems.Add(menu);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка при загрузке компонент",
         MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// Добавление нового контрола 
    private void OpenControl() //передаем что потребуется
 {
        //проверка, что такой контрол еще не был добавлен в TabControl 
        //получение UserControl   
        var tabPage = new TabPage
        {
            Location = new Point(4, 24),
            Name = $"TabC: {title}",
            Padding = new Padding(3),
            Size = new Size(792, 398),
            TabIndex = 0,
            Text = title,
            UseVisualStyleBackColor = true
        };
        tabPage.Controls.Add(UserControl);
        tabControls.TabPages.Add(tabPage);
    }

    /// Загрузка реализаций контрактов
    private static List<IComponentContract> LoadExtensions()
    {
        //получение списка реализаций контрактов 
        return list;
    }

    /// Закрытие вкладки
    private void TabControls_DoubleClick(object sender, EventArgs e)
    {
        if (tabControls.SelectedTab is null)
        {
            return;
        }
        tabControls.TabPages.Remove(tabControls.SelectedTab);
    }
}
