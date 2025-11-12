namespace FormComponentDisplay;

using System.Collections.Generic;
using System.IO; //> для файловой системы
using System.Linq; // > для OrderBy
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

using ContractLib;

public partial class FormMain : Form
{
    // private Dictionary<string, UserControl>
    // [ * ] -> tabControls.TabPages.ContainsKey, так как tabControls уже хранит все

    private readonly Dictionary<string, UserControl> _loadedControls = new Dictionary<string, UserControl>();
    private static IConfiguration? _configuration;

    private string _customComponentsPath = Path.Combine(AppContext.BaseDirectory, "ControlLib");

    public FormMain()
    {
        InitializeComponent();

        _configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        try
        {
            var extensions = LoadExtensions();
            foreach (var extension in extensions.Where(e => e.Category == "SimpleReference" || e.Category == "ComplexReference"))
            {
                var menu = new ToolStripMenuItem
                {
                    Text = extension.MenuTitle
                };
                menu.Click += (sender, e) =>
                {
                    OpenControl((IComponentContract)extension.GetComponentControl);
                };
                DirectoriesToolStripMenuItem.DropDownItems.Add(menu);
            }

            foreach (var extension in extensions.Where(e => e.Category == "Report"))
            {
                _loadedControls.Add(extension.Id, extension.GetComponentControl);
                var menu = new ToolStripMenuItem
                {
                    Text = extension.MenuTitle
                };
                menu.Click += (sender, e) =>
                {
                    OpenControl((IComponentContract)extension.GetComponentControl);
                };
                ReportsToolStripMenuItem.DropDownItems.Add(menu);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "[ Error ] Ошибка при загрузке компонентов", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Добавление нового контрола
    private void OpenControl(IComponentContract component) // > принимает объект компонента
    {
        string tabPageKey = component.Id;

        // Проверка, что такой контрол еще не был добавлен
        if (tabControls.TabPages.ContainsKey(tabPageKey))
        {
            tabControls.SelectedTab = tabControls.TabPages[tabPageKey];
            return;
        }

        UserControl componentControl = component.GetComponentControl; // Получаем UserControl из контракта
        componentControl.Dock = DockStyle.Fill; // > расстягиваем на всю вкладку

        var tabPage = new TabPage
        {
            Name = tabPageKey,
            Text = component.MenuTitle, // > как заголовок вкладки
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