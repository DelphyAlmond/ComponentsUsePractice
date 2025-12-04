using OrderControl;
using System.Reflection;

using ContractLib;

namespace FormComponentDisplay;

public partial class ReportPluginsForm : Form
{
    private List<IReportDocumentC> _loadedPlugins = new List<IReportDocumentC>();
    private OrderDbConnection _centralDb;

    public ReportPluginsForm()
    {
        InitializeComponent();

        try
        {
            _centralDb = new OrderDbConnection();
            // Проверяем подключение
            var test = _centralDb.GetOrders()?.Count ?? 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка подключения к БД: {ex.Message}\nБудут использованы тестовые данные.");
        }

        LoadReportPlugins();
        InitializePluginUI();
    }

    private void LoadReportPlugins()
    {
        // Загружаем из папки сборки самого плагина
        var pluginsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        if (!Directory.Exists(pluginsPath))
            return;

        foreach (var dllFile in Directory.GetFiles(pluginsPath, "*Plugin*.dll"))
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

    private void LoadCommonDependencies()
    {
        var dependencies = new[]
        {
        "DocumentFormat.OpenXml",
        "EPPlus",
        "itext7"
    };

        var pluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RPlugins");

        foreach (var dep in dependencies)
        {
            var dllPath = Path.Combine(pluginsPath, $"{dep}.dll");
            if (File.Exists(dllPath))
            {
                try
                {
                    Assembly.LoadFrom(dllPath);
                }
                catch
                {
                    // Игнорируем ошибки загрузки зависимостей
                }
            }
        }
    }

    private void LoadPluginsFromAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (typeof(IReportDocumentC).IsAssignableFrom(type) &&
                !type.IsInterface && !type.IsAbstract)
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

    private void InitializePluginUI()
    {
        mainFlowLayoutPanel.Controls.Clear();

        var groupedPlugins = _loadedPlugins
            .GroupBy(p => GetPluginType(p))
            .Where(g => g.Any());

        foreach (var group in groupedPlugins)
        {
            AddPluginBlock(mainFlowLayoutPanel, group.Key, group.ToList());
        }
    }

    private string GetPluginType(IReportDocumentC plugin)
    {
        if (plugin is IRDocWithContextTablesC) return "Таблицы PDF";
        if (plugin is IRDocWithChartLineC) return "Линейные диаграммы Word";
        if (plugin is IRDocWithTableColumnRowHeaderC) return "Таблицы Excel";
        return "Другие плагины";
    }

    private void AddPluginBlock(FlowLayoutPanel parent, string title, List<IReportDocumentC> plugins)
    {
        // Clone template controls
        var groupBox = new GroupBox
        {
            Text = $"> {title}",
            Size = groupBoxTemplate.Size,
            Margin = new Padding(10),
            Tag = plugins
        };

        var formatCombo = new ComboBox
        {
            Location = comboBoxTemplate.Location,
            Size = comboBoxTemplate.Size,
            DropDownStyle = comboBoxTemplate.DropDownStyle
        };

        var generateButton = new Button
        {
            Location = generateButtonTemplate.Location,
            Size = generateButtonTemplate.Size,
            Text = generateButtonTemplate.Text,
            Font = generateButtonTemplate.Font
        };

        // Populate combo box
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

        // Wire up event
        generateButton.Click += (sender, e) =>
        {
            if (formatCombo.SelectedItem is PluginComboItem selectedItem)
            {
                GenerateReport(selectedItem.Plugin);
            }
        };

        groupBox.Controls.Add(formatCombo);
        groupBox.Controls.Add(generateButton);
        parent.Controls.Add(groupBox);
    }

    private void GenerateReport(IReportDocumentC plugin)
    {
        using (var saveDialog = new SaveFileDialog())
        {
            saveDialog.Filter = $"{plugin.DocumentFormat.ToUpper()} files|*.{plugin.DocumentFormat}";
            saveDialog.FileName = $"Отчет.{plugin.DocumentFormat}";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                loadingLabel.Visible = true;
                loadingLabel.Text = "Генерация отчета...";

                Task.Run(async () =>
                {
                    try
                    {
                        object data = GetReportData(plugin);
                        await ExecutePluginReport(plugin, saveDialog.FileName, data);

                        this.Invoke(new Action(() =>
                        {
                            loadingLabel.Visible = false;
                            MessageBox.Show($"Отчет успешно создан: {saveDialog.FileName}");
                        }));
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() =>
                        {
                            loadingLabel.Visible = false;
                            MessageBox.Show($"Ошибка создания отчета: {ex.Message}");
                        }));
                    }
                });
            }
        }
    }

    private async Task ExecutePluginReport(IReportDocumentC plugin, string fileName, object data)
    {
        if (plugin is IRDocWithContextTablesC tablePlugin)
        {
            await tablePlugin.CreateDocumentAsync(
                fileName,
                "Отчет по продвижению заказов",
                (List<string[,]>)data
            );
        }
        else if (plugin is IRDocWithChartLineC chartPlugin)
        {
            await chartPlugin.CreateDocumentAsync(
                fileName,
                "Отчет по заказам по городам и датам",
                "Динамика поступления заказов по городам",
                (Dictionary<string, List<(int Parameter, double Value)>>)data
            );
        }
        else if (plugin is IRDocWithTableColumnRowHeaderC excelPlugin)
        {
            var reportData = (List<OrderReportDto>)data;
            var headers = new List<(string Header, string PropertyName, string FieldName)>
            {
                ("Идентификатор", "Id", "Id"),
                ("ФИО заказчика", "CustomerName", "CustomerName"),
                ("Город назначения", "City", "City"),
                ("Дата получения", "ReceiveDate", "ReceiveDate")
            };

            var columnsWidth = new List<int> { 40, 30, 20, 15 };
            var rowsHeights = Enumerable.Repeat(20, reportData.Count + 2).ToList();

            await excelPlugin.CreateDocumentAsync(
                fileName,
                "Полный отчет по всем заказам",
                columnsWidth,
                rowsHeights,
                true,
                headers,
                reportData
            );
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
                // Группируем по MovementNotes - исправляем фильтрацию
                var statusGroups = orders
                    .GroupBy(o => o.MovementNotes ?? "Без статуса") // Добавляем обработку null
                    .Where(g => !string.IsNullOrEmpty(g.Key)) // Исключаем пустые
                    .ToList();

                // Создаем таблицу: +1 для заголовка
                string[,] movementTable = new string[statusGroups.Count + 1, 3];

                // Заголовок
                movementTable[0, 0] = "Статус движения";
                movementTable[0, 1] = "Количество";
                movementTable[0, 2] = "Процент (%)";

                int totalOrders = orders.Count;

                // Заполняем данные
                for (int i = 0; i < statusGroups.Count; i++)
                {
                    var group = statusGroups[i];
                    movementTable[i + 1, 0] = group.Key;
                    movementTable[i + 1, 1] = group.Count().ToString();

                    // Исправляем расчет процента
                    if (totalOrders > 0)
                    {
                        double percentage = (group.Count() * 100.0) / totalOrders;
                        movementTable[i + 1, 2] = percentage.ToString("F2") + "%";
                    }
                    else
                    {
                        movementTable[i + 1, 2] = "0.00%";
                    }
                }

                tables.Add(movementTable);
            }
        }
        catch (Exception ex)
        {
            // Fallback данные - исправляем структуру
            string[,] movementTable = new string[7, 3]
            {
            { "Статус движения", "Количество", "Процент (%)" },
            { "Создан", "10", "20.00%" },
            { "Обработан", "15", "30.00%" },
            { "В пути", "8", "16.00%" },
            { "Доставлен", "12", "24.00%" },
            { "Получен", "3", "6.00%" },
            { "Завершен", "2", "4.00%" }
            };
            tables.Add(movementTable);
        }

        return tables;
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

    private object GetReportData(IReportDocumentC plugin)
    {
        if (plugin is IRDocWithContextTablesC)
        {
            return PrepareMovementTableData();
        }
        else if (plugin is IRDocWithChartLineC)
        {
            return PrepareChartData();
        }
        else if (plugin is IRDocWithTableColumnRowHeaderC)
        {
            return PrepareExcelReportData();
        }

        return null;
    }

    private class PluginComboItem
    {
        public string Text { get; set; }
        public IReportDocumentC Plugin { get; set; }
        public override string ToString() => Text;
    }
}

// DTO for Excel report : already in OrderDbConnection

