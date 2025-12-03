using OrderControl;
using ReportContracts;
using System.Reflection;

namespace FormComponentDisplay;

public partial class ReportPluginsForm : Form
{
    private List<IReportDocumentC> _loadedPlugins = new List<IReportDocumentC>();

    private OrderDbConnection _centralDb;

    public ReportPluginsForm()
    {
        InitializeComponent();
        _centralDb = new OrderDbConnection(); // < initialize

        LoadReportPlugins();
        InitializePluginUI();
    }

    private void LoadReportPlugins()
    {
        var pluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RPlugins");

        if (!Directory.Exists(pluginsPath))
        {
            Directory.CreateDirectory(pluginsPath);
            return;
        }

        foreach (var dllFile in Directory.GetFiles(pluginsPath, "*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllFile);
                LoadPluginsFromAssembly(assembly);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки плагина {dllFile}: {ex.Message}");
            }
        }
    }

    private void LoadPluginsFromAssembly(Assembly assembly)
    {
        var reportInterfaceTypes = new Type[]
        {
            typeof(IRDocWithContextTablesC),
            typeof(IRDocWithChartLineC),
            typeof(IRDocWithTableColumnRowHeaderC)
        };

        foreach (var type in assembly.GetTypes())
        {
            foreach (var interfaceType in reportInterfaceTypes)
            {
                if (interfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    try
                    {
                        var plugin = (IReportDocumentC)Activator.CreateInstance(type);
                        _loadedPlugins.Add(plugin);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating plugin {type.Name}: {ex.Message}");
                    }
                }
            }
        }
    }

    private void InitializePluginUI()
    {
        var mainPanel = (FlowLayoutPanel)this.Controls[0];

        // Group plugins by type
        var tablePlugins = _loadedPlugins.OfType<IRDocWithContextTablesC>().ToList();
        var chartPlugins = _loadedPlugins.OfType<IRDocWithChartLineC>().ToList();
        var complexTablePlugins = _loadedPlugins.OfType<IRDocWithTableColumnRowHeaderC>().ToList();

        // Create UI blocks for each plugin type
        if (tablePlugins.Any())
            AddPluginBlock(mainPanel, "> Таблицы PDF", tablePlugins.Cast<IReportDocumentC>().ToList(),
                (plugin) => GenerateTableReport((IRDocWithContextTablesC)plugin));

        if (chartPlugins.Any())
            AddPluginBlock(mainPanel, "> Линейные диаграммы Word", chartPlugins.Cast<IReportDocumentC>().ToList(),
                (plugin) => GenerateChartReport((IRDocWithChartLineC)plugin));

        if (complexTablePlugins.Any())
            AddPluginBlock(mainPanel, "> Таблицы Excel", complexTablePlugins.Cast<IReportDocumentC>().ToList(),
                (plugin) => GenerateComplexTableReport((IRDocWithTableColumnRowHeaderC)plugin));
    }

    private void AddPluginBlock(FlowLayoutPanel parent, string title, List<IReportDocumentC> plugins, Action<IReportDocumentC> generateAction)
    {
        var groupBox = new GroupBox
        {
            Text = title,
            Size = new Size(550, 100),
            Margin = new Padding(10)
        };

        var formatCombo = new ComboBox
        {
            Location = new Point(20, 30),
            Size = new Size(200, 25),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        foreach (var plugin in plugins)
        {
            formatCombo.Items.Add(new PluginComboItem
            {
                Text = $"{plugin.DocumentFormat.ToUpper()} документ",
                Plugin = plugin
            });
        }

        if (formatCombo.Items.Count > 0)
            formatCombo.SelectedIndex = 0;

        var generateButton = new Button
        {
            Text = "Сгенерировать отчет",
            Location = new Point(250, 30),
            Size = new Size(150, 25)
        };

        generateButton.Click += (sender, e) =>
        {
            if (formatCombo.SelectedItem is PluginComboItem selectedItem)
            {
                generateAction(selectedItem.Plugin);
            }
        };

        groupBox.Controls.Add(formatCombo);
        groupBox.Controls.Add(generateButton);
        parent.Controls.Add(groupBox);
    }

    private void GenerateTableReport(IRDocWithContextTablesC plugin)
    {
        using (var saveDialog = new SaveFileDialog())
        {
            saveDialog.Filter = $"{plugin.DocumentFormat.ToUpper()} files|*.{plugin.DocumentFormat}";
            saveDialog.FileName = $"Отчет_по_движению_заказов.{plugin.DocumentFormat}";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                // Prepare data: movement marks (1-6) without customer names
                var movementData = PrepareMovementTableData();

                Task.Run(async () =>
                {
                    await plugin.CreateDocumentAsync(
                        saveDialog.FileName,
                        "Отчет по продвижению заказов",
                        movementData
                    );

                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Отчет успешно создан: {saveDialog.FileName}");
                    }));
                });
            }
        }
    }

    private List<string[,]> PrepareMovementTableData()
    {
        var tables = new List<string[,]>();

        try
        {
            var orders = _centralDb.GetOrders();

            if (orders != null && orders.Any())
            {
                // Group by movement status
                var statusGroups = orders
                    .Where(o => !string.IsNullOrEmpty(o.MovementNotes))
                    .GroupBy(o => o.MovementNotes)
                    .ToList();

                // Create table data
                string[,] movementTable = new string[statusGroups.Count + 1, 3];

                // Header row
                movementTable[0, 0] = "Статус движения";
                movementTable[0, 1] = "Количество";
                movementTable[0, 2] = "Процент (%)";

                int totalOrders = orders.Count;

                // Data rows
                for (int i = 0; i < statusGroups.Count; i++)
                {
                    var group = statusGroups[i];
                    movementTable[i + 1, 0] = group.Key;
                    movementTable[i + 1, 1] = group.Count().ToString();
                    double percentage = (group.Count() * 100.0) / totalOrders;
                    movementTable[i + 1, 2] = percentage.ToString("0.00") + "%";
                }

                tables.Add(movementTable);
            }
        }
        catch (Exception ex)
        {
            // Fallback to example data if database connection fails
            string[,] movementTable = new string[7, 3]
            {
                    { "Статус", "Количество", "Процент" },
                    { "Создан", "10", "20%" },
                    { "Обработан", "15", "30%" },
                    { "В пути", "8", "16%" },
                    { "Доставлен", "12", "24%" },
                    { "Получен", "3", "6%" },
                    { "Завершен", "2", "4%" }
            };

            tables.Add(movementTable);
        }

        return tables;
    }

    private void GenerateChartReport(IRDocWithChartLineC plugin)
    {
        using (var saveDialog = new SaveFileDialog())
        {
            saveDialog.Filter = $"{plugin.DocumentFormat.ToUpper()} files|*.{plugin.DocumentFormat}";
            saveDialog.FileName = $"Отчет_по_городам_и_датам.{plugin.DocumentFormat}";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                // Prepare data for line chart: city -> [(day, count), ...]
                var chartData = PrepareChartData();

                Task.Run(async () =>
                {
                    await plugin.CreateDocumentAsync(
                        saveDialog.FileName,
                        "Отчет по заказам по городам и датам",
                        "Динамика поступления заказов по городам",
                        chartData
                    );

                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Отчет успешно создан: {saveDialog.FileName}");
                    }));
                });
            }
        }
    }

    private Dictionary<string, List<(int Parameter, double Value)>> PrepareChartData()
    {
        var chartData = new Dictionary<string, List<(int, double)>>();

        try
        {
            return _centralDb.GetOrdersByCityAndDate();
        }
        catch (Exception ex)
        {
            // Fallback to example data
            chartData["Москва"] = new List<(int, double)>
                {
                    (1, 5), (2, 8), (3, 12), (4, 7), (5, 10),
                    (6, 15), (7, 18), (8, 14), (9, 16), (10, 20)
                };

            chartData["Санкт-Петербург"] = new List<(int, double)>
                {
                    (1, 3), (2, 5), (3, 8), (4, 6), (5, 9),
                    (6, 12), (7, 15), (8, 11), (9, 13), (10, 17)
                };

            chartData["Новосибирск"] = new List<(int, double)>
                {
                    (1, 2), (2, 4), (3, 6), (4, 5), (5, 7),
                    (6, 9), (7, 11), (8, 8), (9, 10), (10, 12)
                };
        }

        return chartData;
    }

    private void GenerateComplexTableReport(IRDocWithTableColumnRowHeaderC plugin)
    {
        using (var saveDialog = new SaveFileDialog())
        {
            saveDialog.Filter = $"{plugin.DocumentFormat.ToUpper()} files|*.{plugin.DocumentFormat}";
            saveDialog.FileName = $"Полный_отчет_по_заказам.{plugin.DocumentFormat}";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                // Prepare data for Excel report with complex headers
                var reportData = PrepareExcelReportData();

                Task.Run(async () =>
                {
                    // Define headers with PropertyName mapping
                    var headers = new List<(string Header, string PropertyName, string FieldName)>
                        {
                            ("Идентификатор", "Id", "Id"),
                            ("ФИО заказчика", "CustomerName", "CustomerName"),
                            ("Город назначения", "City", "City"),
                            ("Дата получения", "ReceiveDate", "ReceiveDate")
                        };

                    // Define column widths (in characters)
                    var columnsWidth = new List<int> { 40, 30, 20, 15 };

                    // Define row heights (in points)
                    var rowsHeights = new List<int>();
                    for (int i = 0; i < reportData.Count + 2; i++) // +2 for header rows
                    {
                        rowsHeights.Add(20);
                    }

                    await plugin.CreateDocumentAsync(
                        saveDialog.FileName,
                        "Полный отчет по всем заказам",
                        columnsWidth,
                        rowsHeights,
                        true, // isHeaderFirstRow
                        headers,
                        reportData
                    );

                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Отчет успешно создан: {saveDialog.FileName}");
                    }));
                });
            }
        }
    }

    private List<OrderReportDto> PrepareExcelReportData()
    {
        var reportData = new List<OrderReportDto>();
        try
        {
            return _centralDb.GetOrdersForExcelReport();
        }
        catch (Exception ex)
        {
            // Fallback to example data
            reportData = new List<OrderReportDto>
            {
                new OrderReportDto
                {
                    Id = Guid.NewGuid(),
                    CustomerName = "Иванов Иван Иванович",
                    City = "Москва",
                    ReceiveDate = "2024.01.15",
                    MovementNotes = "Создан"
                },
                new OrderReportDto
                {
                    Id = Guid.NewGuid(),
                    CustomerName = "Петров Петр Петрович",
                    City = "Санкт-Петербург",
                    ReceiveDate = "2024.01.16",
                    MovementNotes = "Обработан"
                },
                new OrderReportDto
                {
                    Id = Guid.NewGuid(),
                    CustomerName = "Сидорова Анна Владимировна",
                    City = "Новосибирск",
                    ReceiveDate = "2024.01.17",
                    MovementNotes = "В пути"
                }
            };
        }

        return reportData;
    }
    
    private class PluginComboItem
    {
        public string Text { get; set; }
        public IReportDocumentC Plugin { get; set; }

        public override string ToString() => Text;
    }
}

// DTO for Excel report : already in OrderDbConnection

