namespace FormComponentDisplay;

using System.Collections.Generic;
using System.IO; //> для файловой системы
using System.Linq; // > для OrderBy
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

using ContractLib;
using System.Configuration;

public partial class FormMain : Form
{
    private readonly Dictionary<string, UserControl> _controls = new Dictionary<string, UserControl>();
    private static IConfiguration? _configuration;

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
                    OpenControl(extension.Id, extension.MenuTitle, extension.GetComponentControl);
                };
                DirectoriesToolStripMenuItem.DropDownItems.Add(menu);
            }

            foreach (var extension in extensions.Where(e => e.Category == "Report"))
            {
                _controls.Add(extension.Id, extension.GetComponentControl);
                var menu = new ToolStripMenuItem
                {
                    Text = extension.MenuTitle
                };
                menu.Click += (sender, e) =>
                {
                    OpenControl(extension.Id, extension.MenuTitle, extension.GetComponentControl);
                };
                ReportsToolStripMenuItem.DropDownItems.Add(menu);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка при загрузке компонентов", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void OpenControl(string id, string title, UserControl control)
    {
        foreach (TabPage tab in tabControls.TabPages)
        {
            if (tab.Name == $"tabPage{id}")
            {
                tabControls.SelectedTab = tab;
                return;
            }
        }

        var tabPage = new TabPage
        {
            Location = new Point(4, 24),
            Name = $"tabPage{id}",
            Padding = new Padding(3),
            Size = new Size(792, 398),
            TabIndex = 0,
            Text = title,
            UseVisualStyleBackColor = true
        };

        tabPage.Controls.Add(control);
        tabControls.TabPages.Add(tabPage);
        tabControls.SelectedTab = tabPage;
    }

    private List<IComponentContract> LoadExtensions()
    {
        var components = new List<IComponentContract>();

        string componentsPath = _configuration!["ComponentsPath"]!;
        if (string.IsNullOrEmpty(componentsPath))
            throw new ConfigurationErrorsException("Путь к компонентам не указан в конфигурации");
        if (!Directory.Exists(componentsPath))
            throw new DirectoryNotFoundException($"Каталог компонентов не найден: {componentsPath}");

        string licenseFile = _configuration!["LicenseFile"]!;
        if (string.IsNullOrEmpty(licenseFile))
            throw new ConfigurationErrorsException("Имя файла лицензии не указано в конфигурации");

        string licensePath = licenseFile;
        LicenseLevel licenseLevel = GetLicenseLevel(licensePath);

        foreach (string file in Directory.GetFiles(componentsPath, "*.dll", SearchOption.AllDirectories))
        {
            if (file.Contains(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar))
            {
                continue;
            }

            try
            {
                Assembly assembly = Assembly.LoadFrom(file);
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsClass && typeof(IComponentContract).IsAssignableFrom(type))
                    {
                        IComponentContract component = (IComponentContract)Activator.CreateInstance(type)!;

                        if (IsComponentAllowed(component, licenseLevel))
                        {
                            components.Add(component);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, $"Ошибка загрузки сборки {file}: {ex.Message}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        return components;
    }

    private LicenseLevel GetLicenseLevel(string licensePath)
    {
        if (!File.Exists(licensePath))
            throw new FileNotFoundException("Файл лицензии не найден", licensePath);

        string licenseContent = File.ReadAllText(licensePath);
        licenseContent = licenseContent.Trim().ToLower();

        switch (licenseContent)
        {
            case "minimal":
                return LicenseLevel.Minimal;
            case "basic":
                return LicenseLevel.Basic;
            case "advanced":
                return LicenseLevel.Advanced;
            default:
                throw new InvalidDataException("Некорректный уровень лицензии в файле");
        }
    }

    private bool IsComponentAllowed(IComponentContract component, LicenseLevel licenseLevel)
    {
        if (component.Category == "SimpleReference")
            return licenseLevel >= LicenseLevel.Minimal;

        if (component.Category == "ComplexReference")
            return licenseLevel >= LicenseLevel.Basic;

        if (component.Category == "Report")
            return licenseLevel >= LicenseLevel.Advanced;

        return false;
    }

    private void TabControls_DoubleClick(object sender, EventArgs e)
    {
        if (tabControls.SelectedTab is null)
        {
            return;
        }
        tabControls.TabPages.Remove(tabControls.SelectedTab);
    }
}