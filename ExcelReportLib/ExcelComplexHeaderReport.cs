using OfficeOpenXml;

using ContractLib;

namespace ExcelReportLib;

public class ExcelComplexHeaderReport : IRDocWithTableColumnRowHeaderC
{
    public string DocumentFormat => "xlsx";

    public async Task CreateDocumentAsync<T>(
        string filePath,
        string header,
        List<int> columnsWidth,
        List<int> rowsHeights,
        bool isHeaderFirstRow,
        List<(string Header, string PropertyName, string FieldName)> headers,
        List<T> data)
    {
        await Task.Run(() =>
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // *

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Отчет по заказам");

                // Set header
                worksheet.Cells[1, 1].Value = header;
                worksheet.Cells[1, 1].Style.Font.Size = 16;
                worksheet.Cells[1, 1].Style.Font.Bold = true;

                int startRow = 3;

                if (isHeaderFirstRow)
                {
                    // Create complex header in first row
                    for (int i = 0; i < headers.Count; i++)
                    {
                        worksheet.Cells[startRow, i + 1].Value = headers[i].Header;
                        worksheet.Cells[startRow, i + 1].Style.Font.Bold = true;
                        worksheet.Cells[startRow, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }

                    startRow++;

                    // Fill data
                    for (int i = 0; i < data.Count; i++)
                    {
                        var item = data[i];
                        for (int j = 0; j < headers.Count; j++)
                        {
                            var property = typeof(T).GetProperty(headers[j].PropertyName);
                            if (property != null)
                            {
                                var value = property.GetValue(item)?.ToString() ?? "";
                                worksheet.Cells[startRow + i, j + 1].Value = value;
                            }
                        }
                    }
                }

                // Set column widths
                for (int i = 0; i < columnsWidth.Count; i++)
                {
                    worksheet.Column(i + 1).Width = columnsWidth[i];
                }

                // Set row heights
                for (int i = 0; i < rowsHeights.Count; i++)
                {
                    worksheet.Row(startRow + i).Height = rowsHeights[i];
                }

                // Group city and date under "Заказ" (as per your requirement)
                worksheet.Cells[startRow - 1, 3].Value = "Заказ";
                worksheet.Cells[startRow - 1, 3, startRow - 1, 4].Merge = true;
                worksheet.Cells[startRow - 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                package.SaveAs(new FileInfo(filePath));
            }
        });
    }
}
