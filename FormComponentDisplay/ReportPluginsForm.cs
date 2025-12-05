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
        var pluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RPlugins");

        if (!Directory.Exists(pluginsPath))
        {
            Directory.CreateDirectory(pluginsPath);
            return;
        }

        LoadCommonDependencies();

        var pluginFiles = new[]
        {
        "ExcelReportLib.dll",
        "PdfReportLib.dll",
        "WordDocReportLib.dll"
        };

        foreach (var pluginFile in pluginFiles)
        {
            var dllPath = Path.Combine(pluginsPath, pluginFile);
            if (File.Exists(dllPath))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dllPath);
                    LoadPluginsFromAssembly(assembly);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки плагина {pluginFile}: {ex.Message}");
                }
            }
        }
    }

    private void LoadCommonDependencies()
    {
        var pluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RPlugins");

        if (!Directory.Exists(pluginsPath))
            return;

        // Порядок важен - сначала загружаем базовые зависимости
        var dependencies = new[]
        {
        "DocumentFormat.OpenXml",
        "EPPlus",
        "itext7",
        "BouncyCastle.Crypto" // Для iText7
        };

        foreach (var dep in dependencies)
        {
            var dllFiles = Directory.GetFiles(pluginsPath, $"{dep}*.dll");
            foreach (var dllPath in dllFiles)
            {
                try
                {
                    Assembly.LoadFrom(dllPath);
                    Console.WriteLine($"Загружена зависимость: {Path.GetFileName(dllPath)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не удалось загрузить {dep}: {ex.Message}");
                }
            }
        }
    }

    private void LoadPluginsFromAssembly(Assembly assembly)
    {
        try
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
                        Console.WriteLine($"Загружен плагин: {type.Name} ({plugin.DocumentFormat})");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка создания плагина {type.Name}: {ex.Message}");
                    }
                }
            }
        }
        catch (ReflectionTypeLoadException ex)
        {
            foreach (var loaderEx in ex.LoaderExceptions)
            {
                MessageBox.Show($"Ошибка загрузки типа: {loaderEx?.Message}");
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
            // Теперь передаем Dictionary<string, List<(string Date, double Value)>>
            await chartPlugin.CreateDocumentAsync(
                fileName,
                "Аналитический отчет по динамике заказов",
                "Динамика поступления заказов по городам",
                (Dictionary<string, List<(string Date, double Value)>>)data // string DateTime *
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
            ("Дата получения", "ReceiveDate", "ReceiveDate"),
            ("Статус заказа", "MovementNotes", "MovementNotes")
            };

            var columnsWidth = new List<int> { 40, 40, 25, 20, 25 };
            var rowsHeights = Enumerable.Repeat(20, Math.Max(reportData.Count + 2, 10)).ToList();

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
                Console.WriteLine($"Найдено заказов в БД: {orders.Count}");

                // 1. Таблица статусов
                var statusGroups = orders
                    .Where(o => !string.IsNullOrEmpty(o.MovementNotes))
                    .GroupBy(o => o.MovementNotes.Trim()) // Убираем пробелы
                    .OrderByDescending(g => g.Count())
                    .ToList();

                Console.WriteLine($"Уникальных статусов: {statusGroups.Count}");

                if (statusGroups.Any())
                {
                    string[,] statusTable = new string[statusGroups.Count + 1, 3];

                    // Заголовок
                    statusTable[0, 0] = "Статус заказа";
                    statusTable[0, 1] = "Количество";
                    statusTable[0, 2] = "Процент";

                    int totalOrders = orders.Count;

                    // Данные
                    for (int i = 0; i < statusGroups.Count; i++)
                    {
                        var group = statusGroups[i];
                        var statusName = group.Key;
                        var count = group.Count();

                        // Убедимся, что статус не пустой
                        if (string.IsNullOrWhiteSpace(statusName))
                            statusName = "Без статуса";

                        statusTable[i + 1, 0] = statusName;
                        statusTable[i + 1, 1] = count.ToString();

                        if (totalOrders > 0)
                        {
                            double percentage = (count * 100.0) / totalOrders;
                            statusTable[i + 1, 2] = $"{percentage:F1}%";
                        }
                        else
                        {
                            statusTable[i + 1, 2] = "0.0%";
                        }

                        Console.WriteLine($"  Статус: '{statusTable[i + 1, 0]}', Кол-во: {statusTable[i + 1, 1]}, %: {statusTable[i + 1, 2]}");
                    }

                    tables.Add(statusTable);
                    Console.WriteLine("Таблица статусов создана");
                }
                else
                {
                    Console.WriteLine("Нет данных по статусам");
                }

                // 2. Таблица городов
                var cityGroups = orders
                    .Where(o => !string.IsNullOrEmpty(o.Destination))
                    .GroupBy(o => o.Destination.Trim())
                    .OrderByDescending(g => g.Count())
                    .ToList();

                Console.WriteLine($"Уникальных городов: {cityGroups.Count}");

                if (cityGroups.Any())
                {
                    string[,] cityTable = new string[cityGroups.Count + 1, 3];

                    // Заголовок
                    cityTable[0, 0] = "Город назначения";
                    cityTable[0, 1] = "Количество заказов";
                    cityTable[0, 2] = "Доля";

                    int totalOrders = orders.Count;

                    // Данные
                    for (int i = 0; i < cityGroups.Count; i++)
                    {
                        var group = cityGroups[i];
                        var cityName = group.Key;
                        var count = group.Count();

                        if (string.IsNullOrWhiteSpace(cityName))
                            cityName = "Не указан";

                        cityTable[i + 1, 0] = cityName;
                        cityTable[i + 1, 1] = count.ToString();

                        if (totalOrders > 0)
                        {
                            double percentage = (count * 100.0) / totalOrders;
                            cityTable[i + 1, 2] = $"{percentage:F1}%";
                        }
                        else
                        {
                            cityTable[i + 1, 2] = "0.0%";
                        }

                        Console.WriteLine($"  Город: '{cityTable[i + 1, 0]}', Кол-во: {cityTable[i + 1, 1]}, %: {cityTable[i + 1, 2]}");
                    }

                    tables.Add(cityTable);
                    Console.WriteLine("Таблица городов создана");
                }
                else
                {
                    Console.WriteLine("Нет данных по городам");
                }
            }
            else
            {
                Console.WriteLine("БД пустая или ошибка получения данных");
                // Используем тестовые данные
                tables = GetFallbackTables();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            // Используем тестовые данные
            tables = GetFallbackTables();
        }

        Console.WriteLine($"Итого таблиц для PDF: {tables.Count}");

        return tables;
    }

    private List<string[,]> GetFallbackTables()
    {
        var tables = new List<string[,]>();

        Console.WriteLine("Используем тестовые данные для PDF");

        // Тестовая таблица статусов
        string[,] statusTable = new string[6, 3]
        {
        { "Статус заказа", "Количество", "Процент" },
        { "Создан", "12", "24.0%" },
        { "Обработан", "10", "20.0%" },
        { "В пути", "8", "16.0%" },
        { "Доставлен", "15", "30.0%" },
        { "Завершен", "5", "10.0%" }
        };

        // Тестовая таблица городов
        string[,] cityTable = new string[5, 3]
        {
        { "Город назначения", "Количество заказов", "Доля" },
        { "Москва", "18", "36.0%" },
        { "Санкт-Петербург", "12", "24.0%" },
        { "Новосибирск", "8", "16.0%" },
        { "Казань", "7", "14.0%" }
        };

        tables.Add(statusTable);
        tables.Add(cityTable);

        return tables;
    }

    private Dictionary<string, List<(string Date, double Value)>> PrepareChartData()
    {
        try
        {
            // Получаем данные из БД
            var data = _centralDb.GetOrdersByCityAndDateWithString();

            if (data == null || !data.Any())
            {
                // Если данных нет, создаем простые демо-данные
                return CreateSimpleDemoData();
            }

            // Сортируем данные по дате для каждого города
            foreach (var city in data.Keys.ToList())
            {
                var sortedData = data[city]
                    .OrderBy(x => x.Date)
                    .ToList();
                data[city] = sortedData;
            }

            return data;
        }
        catch (Exception ex)
        {
            // В случае ошибки - демо-данные
            return CreateSimpleDemoData();
        }
    }

    private Dictionary<string, List<(string Date, double Value)>> CreateSimpleDemoData()
    {
        var result = new Dictionary<string, List<(string Date, double Value)>>();

        // Данные на основе тестовых записей из БД
        result["Москва"] = new List<(string Date, double Value)>
        {
            ("15.01.2024", 1)
        };

        result["Санкт-Петербург"] = new List<(string Date, double Value)>
        {
            ("16.01.2024", 1)
        };

        result["Новосибирск"] = new List<(string Date, double Value)>
        {
            ("17.01.2024", 1)
        };

        result["Екатеринбург"] = new List<(string Date, double Value)>
        {
            ("18.01.2024", 1)
        };

        result["Казань"] = new List<(string Date, double Value)>
        {
            ("19.01.2024", 1)
        };

        result["Нижний Новгород"] = new List<(string Date, double Value)>
        {
            ("20.01.2024", 1)
        };

        result["Ульяновск"] = new List<(string Date, double Value)>
        {
            ("21.01.2024", 1)
        };

        result["Самара"] = new List<(string Date, double Value)>
        {
            ("22.01.2024", 1)
        };

        result["Омск"] = new List<(string Date, double Value)>
        {
            ("23.01.2024", 1)
        };

        result["Ростов-на-Дону"] = new List<(string Date, double Value)>
        {
            ("24.01.2024", 1)
        };

        return result;
    }

    private List<OrderReportDto> PrepareExcelReportData()
    {
        var reportData = new List<OrderReportDto>();
        try
        {
            var data = _centralDb.GetOrdersForExcelReport();

            // Форматируем даты для лучшей читаемости
            foreach (var item in data)
            {
                if (DateTime.TryParse(item.ReceiveDate, out DateTime date))
                {
                    item.ReceiveDate = date.ToString("dd.MM.yyyy");
                }
            }

            // Сортируем по городу, затем по дате
            return data
                .OrderBy(x => x.City)
                .ThenByDescending(x => x.ReceiveDate)
                .ToList();
        }
        catch (Exception ex)
        {
            // Fallback данные с реальными городами из вашей БД
            reportData = new List<OrderReportDto>
        {
            new OrderReportDto
            {
                Id = Guid.NewGuid(),
                CustomerName = "Иванов Иван Иванович",
                City = "Москва",
                ReceiveDate = "15.01.2024",
                MovementNotes = "Создан"
            },
            new OrderReportDto
            {
                Id = Guid.NewGuid(),
                CustomerName = "Петров Петр Петрович",
                City = "Санкт-Петербург",
                ReceiveDate = "16.01.2024",
                MovementNotes = "Обработан"
            },
            new OrderReportDto
            {
                Id = Guid.NewGuid(),
                CustomerName = "Сидорова Анна Владимировна",
                City = "Новосибирск",
                ReceiveDate = "17.01.2024",
                MovementNotes = "В пути"
            },
            new OrderReportDto
            {
                Id = Guid.NewGuid(),
                CustomerName = "Кузнецов Алексей Сергеевич",
                City = "Екатеринбург",
                ReceiveDate = "18.01.2024",
                MovementNotes = "Доставлен"
            },
            new OrderReportDto
            {
                Id = Guid.NewGuid(),
                CustomerName = "Смирнова Мария Дмитриевна",
                City = "Казань",
                ReceiveDate = "19.01.2024",
                MovementNotes = "Получен"
            }
        };

            return reportData;
        }
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

