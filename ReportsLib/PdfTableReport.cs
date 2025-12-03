using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

using ReportContracts;

namespace ReportsLib;

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
            using (var writer = new PdfWriter(filePath))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);

                    // Add header
                    document.Add(new Paragraph(header)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(18)
                        .SetBold()
                        .SetMarginBottom(20));

                    // Create table for each movement status
                    foreach (var tableData in tables)
                    {
                        var table = new Table(tableData.GetLength(1));
                        table.SetWidth(UnitValue.CreatePercentValue(100));

                        for (int i = 0; i < tableData.GetLength(0); i++)
                        {
                            for (int j = 0; j < tableData.GetLength(1); j++)
                            {
                                var cell = new Cell().Add(new Paragraph(tableData[i, j]));

                                // Style header row
                                if (i == 0)
                                {
                                    cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                                    cell.SetTextAlignment(TextAlignment.CENTER);
                                    cell.SetBold();
                                }

                                table.AddCell(cell);
                            }
                        }

                        document.Add(table);
                        document.Add(new Paragraph("\n"));
                    }

                    document.Close();
                }
            }
        });
    }
}
