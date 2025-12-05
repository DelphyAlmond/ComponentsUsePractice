using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

using ContractLib;

namespace PdfReportLib;

public class PdfTableReport : IRDocWithContextTablesC
{
    public string DocumentFormat => "pdf";

    public async Task CreateDocumentAsync(
        string filePath,
        string header,
        List<string[,]> tables)
    {
        await Task.Run(() =>
        {
            Console.WriteLine($"\nСоздание PDF: {filePath}");
            Console.WriteLine($"Заголовок: {header}");
            Console.WriteLine($"Таблиц для отображения: {tables?.Count ?? 0}");

            using (var writer = new PdfWriter(filePath))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);

                    // Простой шрифт
                    var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                    document.SetFont(font);

                    // Заголовок документа
                    document.Add(new Paragraph(header)
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBold()
                        .SetMarginBottom(10));

                    document.Add(new Paragraph($"Отчет сформирован: {DateTime.Now:dd.MM.yyyy HH:mm}")
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontColor(ColorConstants.GRAY)
                        .SetMarginBottom(20));

                    // Проверяем и обрабатываем таблицы
                    if (tables != null && tables.Any())
                    {
                        for (int tableIndex = 0; tableIndex < tables.Count; tableIndex++)
                        {
                            var tableData = tables[tableIndex];

                            if (tableData == null || tableData.GetLength(0) == 0 || tableData.GetLength(1) == 0)
                            {
                                Console.WriteLine($"Таблица {tableIndex + 1}: пустая или неверный формат");
                                continue;
                            }

                            Console.WriteLine($"Таблица {tableIndex + 1}: {tableData.GetLength(0)} строк, {tableData.GetLength(1)} колонок");

                            // Добавляем заголовок таблицы
                            string tableTitle = tableIndex == 0 ? "СТАТИСТИКА ПО СТАТУСАМ ЗАКАЗОВ" : "СТАТИСТИКА ПО ГОРОДАМ";
                            document.Add(new Paragraph(tableTitle)
                                .SetFontSize(14)
                                .SetBold()
                                .SetMarginBottom(10));

                            // Создаем таблицу
                            var table = new Table(tableData.GetLength(1));
                            table.SetWidth(UnitValue.CreatePercentValue(100));
                            table.SetMarginBottom(20);

                            // Заполняем таблицу
                            for (int row = 0; row < tableData.GetLength(0); row++)
                            {
                                for (int col = 0; col < tableData.GetLength(1); col++)
                                {
                                    // Получаем значение ячейки
                                    var cellValue = tableData[row, col] ?? "";
                                    if (string.IsNullOrEmpty(cellValue))
                                        cellValue = "-";

                                    var cell = new Cell().Add(new Paragraph(cellValue));

                                    // Стиль для заголовка (первая строка)
                                    if (row == 0)
                                    {
                                        cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                                        cell.SetBold();
                                        cell.SetTextAlignment(TextAlignment.CENTER);
                                        cell.SetFontSize(12);
                                    }
                                    else
                                    {
                                        // Для данных
                                        if (col == 0) // Первая колонка (названия)
                                        {
                                            cell.SetTextAlignment(TextAlignment.LEFT);
                                            cell.SetBold();
                                        }
                                        else if (col == 1) // Вторая колонка (количество)
                                        {
                                            cell.SetTextAlignment(TextAlignment.CENTER);
                                        }
                                        else if (col == 2) // Третья колонка (проценты)
                                        {
                                            cell.SetTextAlignment(TextAlignment.RIGHT);
                                        }

                                        // Чередование цветов строк
                                        if (row % 2 == 0)
                                        {
                                            cell.SetBackgroundColor(new DeviceRgb(248, 248, 248));
                                        }
                                    }

                                    cell.SetPadding(5);
                                    table.AddCell(cell);
                                }
                            }

                            document.Add(table);

                            // Добавляем разделитель между таблицами
                            if (tableIndex < tables.Count - 1)
                            {
                                document.Add(new Paragraph("\n"));
                            }
                        }

                        // Итоговая информация
                        document.Add(new Paragraph("--- Конец отчета ---")
                            .SetFontSize(10)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontColor(ColorConstants.GRAY)
                            .SetMarginTop(20));
                    }
                    else
                    {
                        Console.WriteLine("Нет данных для таблиц");
                        document.Add(new Paragraph("Нет данных для отображения")
                            .SetFontSize(12)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontColor(ColorConstants.RED));
                    }

                    document.Close();
                    Console.WriteLine("PDF успешно создан\n");
                }
            }
        });
    }
}