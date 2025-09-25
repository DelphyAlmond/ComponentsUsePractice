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
            foreach (var extension in //������� ��������� ��� ������ ���� ������������) 
   {
                var menu = new ToolStripMenuItem
                {
                    Text = //��������� ��������� ����
                };
                menu.Click += (sender, e) =>
                {
                    OpenControl(); 
    };
                DirectoriesToolStripMenuItem.DropDownItems.Add(menu);
            }
            foreach (var extension in //������� ��������� ��� ������ ���� ��������) 
   {
                _controls.Add(extension.Id, extension.Control);
                var menu = new ToolStripMenuItem
                {
                    Text = //��������� 
            ��������� ����
                };
                menu.Click += (sender, e) =>
                {
                    OpenControl() //�������� ��� �����������; 
    };
                ReportsToolStripMenuItem.DropDownItems.Add(menu);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "������ ��� �������� ���������",
         MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// ���������� ������ �������� 
    private void OpenControl() //�������� ��� �����������
 {
        //��������, ��� ����� ������� ��� �� ��� �������� � TabControl 
        //��������� UserControl   
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

    /// �������� ���������� ����������
    private static List<IComponentContract> LoadExtensions()
    {
        //��������� ������ ���������� ���������� 
        return list;
    }

    /// �������� �������
    private void TabControls_DoubleClick(object sender, EventArgs e)
    {
        if (tabControls.SelectedTab is null)
        {
            return;
        }
        tabControls.TabPages.Remove(tabControls.SelectedTab);
    }
}
