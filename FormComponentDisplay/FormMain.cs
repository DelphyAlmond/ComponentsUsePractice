namespace FormComponentDisplay;

using System.Collections.Generic;
using System.IO; //> для файловой системы
using System.Linq; // > для OrderBy
using System.Reflection;
using System.Windows.Forms;

using ContractLib;

public partial class FormMain : Form
{
    // private Dictionary<string, UserControl>
    // _loadedControls = new Dictionary<string, UserControl>();
    // [ * ] -> tabControls.TabPages.ContainsKey, так как tabControls уже хранит все

    private string _customComponentsPath = Path.Combine(AppContext.BaseDirectory, "ControlLib");

    public FormMain()
    {
        InitializeComponent();
        try
        {
            var extensions = LoadExtensions(); // Вызов реальной загрузки

            // Заполнение меню "Справочники" & "Отчеты"
            foreach (var component in extensions.Where(c => c.ComponentCategory == "Справочники"))
            {
                var menu = new ToolStripMenuItem { Text = component.ComponentName };
                menu.Click += (sender, e) => { OpenControl(component); }; // > передача компонент в OpenControl
                DirectoriesToolStripMenuItem.DropDownItems.Add(menu);
            }
            foreach (var component in extensions.Where(c => c.ComponentCategory == "Отчеты")) // > выборка
            {
                // _controls.Add(extension.Id, extension.Control);
                // [ * ] -> теперь UserControl создается при открытии вкладки
                var menu = new ToolStripMenuItem { Text = component.ComponentName };
                menu.Click += (sender, e) => { OpenControl(component); };
                ReportsToolStripMenuItem.DropDownItems.Add(menu);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "[ ! ] Ошибка при загрузке компонент", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Добавление нового контрола
    private void OpenControl(IComponentContract component) // > принимает объект компонента
    {
        string tabPageKey = component.ComponentId;

        // Проверка, что такой контрол еще не был добавлен
        if (tabControls.TabPages.ContainsKey(tabPageKey))
        {
            tabControls.SelectedTab = tabControls.TabPages[tabPageKey];
            return;
        }

        UserControl componentControl = component.GetComponentControl(); // Получаем UserControl из контракта
        componentControl.Dock = DockStyle.Fill; // > расстягиваем на всю вкладку

        var tabPage = new TabPage
        {
            Name = tabPageKey,
            Text = component.ComponentName, // > ComponentName как заголовок вкладки
            UseVisualStyleBackColor = true
        };
        tabPage.Controls.Add(componentControl);
        tabControls.TabPages.Add(tabPage);
        tabControls.SelectedTab = tabPage; // def: active
    }

    private string GetComponentsPathFromConfig()
    {
        return _customComponentsPath;
    }

    // Загрузка реализаций контрактов
    private List<IComponentContract> LoadExtensions() // Возвращает список
    {
        List<IComponentContract> components = new List<IComponentContract>();
        string componentsPath = GetComponentsPathFromConfig();
        // Получ. путь к папке\проекту с исп.-мыми компонентами

        if (!Directory.Exists(componentsPath))
        {
            throw new DirectoryNotFoundException($"[ ! ] Директория компонентов не найдена: {componentsPath}");
        }

        foreach (string dllFile in Directory.GetFiles(componentsPath, "*.dll"))
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(dllFile);
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IComponentContract).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        IComponentContract component = (IComponentContract)Activator.CreateInstance(type)!; // Создаем экземпляр
                        components.Add(component);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[ ! ] Ошибка загрузки компонента {dllFile}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        return components.ToList(); //+ OrderBy(c => c.Order) *
    }

    // Закрытие вкладки
    private void TabControls_DoubleClick(object sender, EventArgs e)
    {
        if (tabControls.SelectedTab is null)
        {
            return;
        }
        tabControls.TabPages.Remove(tabControls.SelectedTab);
    }
}