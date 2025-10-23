namespace FormComponentDisplay;

using System.Collections.Generic;
using System.IO; //> ��� �������� �������
using System.Linq; // > ��� OrderBy
using System.Reflection;
using System.Windows.Forms;

using ContractLib;

public partial class FormMain : Form
{
    // private Dictionary<string, UserControl>
    // _loadedControls = new Dictionary<string, UserControl>();
    // [ * ] -> tabControls.TabPages.ContainsKey, ��� ��� tabControls ��� ������ ���

    private string _customComponentsPath = Path.Combine(AppContext.BaseDirectory, "ControlLib");

    public FormMain()
    {
        InitializeComponent();
        try
        {
            var extensions = LoadExtensions(); // ����� �������� ��������

            // ���������� ���� "�����������" & "������"
            foreach (var component in extensions.Where(c => c.ComponentCategory == "�����������"))
            {
                var menu = new ToolStripMenuItem { Text = component.ComponentName };
                menu.Click += (sender, e) => { OpenControl(component); }; // > �������� ��������� � OpenControl
                DirectoriesToolStripMenuItem.DropDownItems.Add(menu);
            }
            foreach (var component in extensions.Where(c => c.ComponentCategory == "������")) // > �������
            {
                // _controls.Add(extension.Id, extension.Control);
                // [ * ] -> ������ UserControl ��������� ��� �������� �������
                var menu = new ToolStripMenuItem { Text = component.ComponentName };
                menu.Click += (sender, e) => { OpenControl(component); };
                ReportsToolStripMenuItem.DropDownItems.Add(menu);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "[ ! ] ������ ��� �������� ���������", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ���������� ������ ��������
    private void OpenControl(IComponentContract component) // > ��������� ������ ����������
    {
        string tabPageKey = component.ComponentId;

        // ��������, ��� ����� ������� ��� �� ��� ��������
        if (tabControls.TabPages.ContainsKey(tabPageKey))
        {
            tabControls.SelectedTab = tabControls.TabPages[tabPageKey];
            return;
        }

        UserControl componentControl = component.GetComponentControl(); // �������� UserControl �� ���������
        componentControl.Dock = DockStyle.Fill; // > ������������ �� ��� �������

        var tabPage = new TabPage
        {
            Name = tabPageKey,
            Text = component.ComponentName, // > ComponentName ��� ��������� �������
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

    // �������� ���������� ����������
    private List<IComponentContract> LoadExtensions() // ���������� ������
    {
        List<IComponentContract> components = new List<IComponentContract>();
        string componentsPath = GetComponentsPathFromConfig();
        // �����. ���� � �����\������� � ���.-���� ������������

        if (!Directory.Exists(componentsPath))
        {
            throw new DirectoryNotFoundException($"[ ! ] ���������� ����������� �� �������: {componentsPath}");
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
                        IComponentContract component = (IComponentContract)Activator.CreateInstance(type)!; // ������� ���������
                        components.Add(component);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[ ! ] ������ �������� ���������� {dllFile}: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        return components.ToList(); //+ OrderBy(c => c.Order) *
    }

    // �������� �������
    private void TabControls_DoubleClick(object sender, EventArgs e)
    {
        if (tabControls.SelectedTab is null)
        {
            return;
        }
        tabControls.TabPages.Remove(tabControls.SelectedTab);
    }
}