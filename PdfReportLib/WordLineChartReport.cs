using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using ContractLib;

namespace WordDocReportLib;

public class WordLineChartReport : IRDocWithChartLineC
{
    public string DocumentFormat => "docx";

    public async Task CreateDocumentAsync(
        string filePath,
        string header,
        string chartTitle,
        Dictionary<string, List<(string Date, double Value)>> series) // *
    {
        await Task.Run(() =>
        {
            using (var wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                var mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                var body = mainPart.Document.AppendChild(new Body());

                // 1. Заголовок
                AddCenteredParagraph(body, header, 24, true, "2E74B5");
                AddCenteredParagraph(body, $"Дата формирования: {DateTime.Now:dd.MM.yyyy HH:mm}", 10, false, "666666");
                body.AppendChild(new Paragraph());

                // 2. Заголовок графика
                var chartTitlePara = new Paragraph(
                    new Run(
                        new RunProperties(
                            new RunFonts() { Ascii = "Arial" },
                            new FontSize() { Val = "22" },
                            new Bold(),
                            new Color() { Val = "2E74B5" }
                        ),
                        new Text(chartTitle)
                    )
                );
                body.AppendChild(chartTitlePara);
                body.AppendChild(new Paragraph());

                // 3. Статистика
                AddStatisticsTable(body, series);
                body.AppendChild(new Paragraph());

                // 4. Детальные данные
                AddDetailedData(body, series);
                body.AppendChild(new Paragraph());

                // 5. Анализ
                AddAnalysis(body, series);

                mainPart.Document.Save();
            }
        });
    }

    private void AddCenteredParagraph(Body body, string text, int fontSize, bool isBold, string colorHex)
    {
        var para = new Paragraph(
            new ParagraphProperties(
                new Justification() { Val = JustificationValues.Center }
            ),
            new Run(
                new RunProperties(
                    new RunFonts() { Ascii = "Arial" },
                    new FontSize() { Val = (fontSize * 2).ToString() },
                    new Color() { Val = colorHex },
                    isBold ? new Bold() : null
                ),
                new Text(text)
            )
        );
        body.AppendChild(para);
    }

    private void AddStatisticsTable(Body body, Dictionary<string, List<(string Date, double Value)>> series)
    {
        if (series == null || !series.Any())
            return;

        var table = new Table();

        var tableProps = new TableProperties(
            new TableBorders(
                new TopBorder() { Val = BorderValues.Single, Size = 4, Color = "2E74B5" },
                new BottomBorder() { Val = BorderValues.Single, Size = 4, Color = "2E74B5" },
                new InsideHorizontalBorder() { Val = BorderValues.Single, Size = 1, Color = "DDDDDD" }
            ),
            new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct }
        );
        table.Append(tableProps);

        // Заголовок таблицы
        var headers = new[] { "Город", "Всего заказов", "Дней с данными", "Максимум за день", "Среднее за день" };
        var headerRow = new TableRow();

        foreach (var header in headers)
        {
            var cell = new TableCell(new Paragraph(
                new Run(
                    new RunProperties(
                        new RunFonts() { Ascii = "Arial" },
                        new FontSize() { Val = "20" },
                        new Bold(),
                        new Color() { Val = "FFFFFF" }
                    ),
                    new Text(header)
                )
            ));
            cell.TableCellProperties = new TableCellProperties(
                new Shading() { Fill = "2E74B5" }
            );
            headerRow.Append(cell);
        }
        table.Append(headerRow);

        // Данные
        foreach (var city in series.OrderByDescending(x => x.Value.Sum(v => v.Value)))
        {
            var data = city.Value;
            var total = data.Sum(v => v.Value);
            var daysCount = data.Count;
            var max = data.Count > 0 ? data.Max(v => v.Value) : 0;
            var avg = daysCount > 0 ? data.Average(v => v.Value) : 0;

            var dataRow = new TableRow();

            var rowData = new[]
            {
                city.Key,
                total.ToString("F0"),
                daysCount.ToString(),
                max.ToString("F0"),
                avg.ToString("F1")
            };

            for (int i = 0; i < rowData.Length; i++)
            {
                var cell = new TableCell(new Paragraph(
                    new Run(
                        new RunProperties(
                            new RunFonts() { Ascii = "Arial" },
                            new FontSize() { Val = "18" }
                        ),
                        new Text(rowData[i])
                    )
                ));

                cell.TableCellProperties = new TableCellProperties(
                    new Shading() { Fill = (dataRow.ChildElements.Count % 2 == 0) ? "F2F2F2" : "FFFFFF" }
                );

                dataRow.Append(cell);
            }

            table.Append(dataRow);
        }

        body.AppendChild(table);
    }

    private void AddDetailedData(Body body, Dictionary<string, List<(string Date, double Value)>> series)
    {
        if (series == null || !series.Any())
            return;

        var intro = new Paragraph(
            new Run(
                new RunProperties(
                    new RunFonts() { Ascii = "Arial" },
                    new FontSize() { Val = "18" },
                    new Bold()
                ),
                new Text("Детальные данные по дням:")
            )
        );
        body.AppendChild(intro);
        body.AppendChild(new Paragraph());

        // Создаем таблицу с детальными данными
        var table = new Table();
        var tableProps = new TableProperties(
            new TableBorders(
                new TopBorder() { Val = BorderValues.Single, Size = 2, Color = "666666" },
                new BottomBorder() { Val = BorderValues.Single, Size = 2, Color = "666666" },
                new InsideHorizontalBorder() { Val = BorderValues.Single, Size = 1, Color = "CCCCCC" }
            ),
            new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct }
        );
        table.Append(tableProps);

        // Получаем все уникальные даты (первые 15 для читаемости)
        var allDates = series.SelectMany(x => x.Value.Select(v => v.Date))
                            .Distinct()
                            .OrderBy(d => d)
                            .Take(15)
                            .ToList();

        if (!allDates.Any())
            return;

        // Заголовки - даты
        var headerRow = new TableRow();
        headerRow.Append(new TableCell(new Paragraph(
            new Run(
                new RunProperties(new Bold()),
                new Text("Город")
            )
        )));

        foreach (var date in allDates)
        {
            headerRow.Append(new TableCell(new Paragraph(
                new Run(
                    new RunProperties(new Bold()),
                    new Text(date)
                )
            )));
        }
        table.Append(headerRow);

        // Данные по городам
        foreach (var city in series.OrderByDescending(x => x.Value.Sum(v => v.Value)))
        {
            var dataRow = new TableRow();
            dataRow.Append(new TableCell(new Paragraph(
                new Run(
                    new Text(city.Key)
                )
            )));

            foreach (var date in allDates)
            {
                var value = city.Value.FirstOrDefault(v => v.Date == date).Value;
                dataRow.Append(new TableCell(new Paragraph(
                    new Run(
                        new Text(value > 0 ? value.ToString("F0") : "-")
                    )
                )));
            }

            table.Append(dataRow);
        }

        body.AppendChild(table);

        var totalDates = series.SelectMany(x => x.Value.Select(v => v.Date)).Distinct().Count();
        if (totalDates > 15)
        {
            var info = new Paragraph(
                new Run(
                    new RunProperties(
                        new RunFonts() { Ascii = "Arial" },
                        new FontSize() { Val = "14" }
                    ),
                    new Text($"Показано 15 из {totalDates} дней.")
                )
            );
            body.AppendChild(info);
        }
    }

    private void AddAnalysis(Body body, Dictionary<string, List<(string Date, double Value)>> series)
    {
        if (series == null || !series.Any())
            return;

        var analysisHeader = new Paragraph(
            new Run(
                new RunProperties(
                    new RunFonts() { Ascii = "Arial" },
                    new FontSize() { Val = "18" },
                    new Bold()
                ),
                new Text("Анализ и выводы:")
            )
        );
        body.AppendChild(analysisHeader);

        var totalAll = series.Sum(x => x.Value.Sum(v => v.Value));
        var leader = series.OrderByDescending(x => x.Value.Sum(v => v.Value)).First();
        var leaderTotal = leader.Value.Sum(v => v.Value);
        var leaderPercent = totalAll > 0 ? (leaderTotal / totalAll * 100) : 0;

        // Находим самый активный день
        var allData = series.SelectMany(x => x.Value);
        var busiestDay = allData
            .GroupBy(x => x.Date)
            .Select(g => new { Date = g.Key, Total = g.Sum(x => x.Value) })
            .OrderByDescending(x => x.Total)
            .FirstOrDefault();

        var analysis = new Paragraph(
            new Run(
                new RunProperties(
                    new RunFonts() { Ascii = "Arial" },
                    new FontSize() { Val = "16" }
                ),
                new Text($"📊 Общая статистика:\n" +
                       $"• Всего заказов за период: {totalAll:F0}\n" +
                       $"• Город-лидер: {leader.Key} ({leaderPercent:F1}% всех заказов)\n" +
                       $"• Среднее количество заказов в день: {series.Average(x => x.Value.Count > 0 ? x.Value.Average(v => v.Value) : 0):F1}\n" +
                       $"• Городов в анализе: {series.Count}\n" +
                       $"• Самый активный день: {(busiestDay != null ? $"{busiestDay.Date} ({busiestDay.Total:F0} заказов)" : "нет данных")}\n\n")
            )
        );

        body.AppendChild(analysis);
    }
}