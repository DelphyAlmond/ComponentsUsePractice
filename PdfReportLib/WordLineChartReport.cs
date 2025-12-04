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
        Dictionary<string, List<(int Parameter, double Value)>> series)
    {
        await Task.Run(() =>
        {
            using (var wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                // Add main document part
                var mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                var body = mainPart.Document.AppendChild(new Body());

                // Add header
                var titlePara = new Paragraph();
                var titleRun = new Run();
                titleRun.AppendChild(new Text(header));
                titlePara.AppendChild(titleRun);
                body.AppendChild(titlePara);

                // Add chart title
                var chartTitlePara = new Paragraph();
                var chartTitleRun = new Run();
                chartTitleRun.AppendChild(new Text(chartTitle));
                chartTitlePara.AppendChild(chartTitleRun);
                body.AppendChild(chartTitlePara);

                // Note: Actual chart creation would require more complex OpenXML code
                // This is a simplified version

                body.AppendChild(new Paragraph(new Run(new Text("Line chart showing orders by city and date"))));

                // Create table with data summary
                var table = new Table();

                // Add headers
                var headerRow = new TableRow();
                headerRow.Append(new TableCell(new Paragraph(new Run(new Text("Город")))));
                headerRow.Append(new TableCell(new Paragraph(new Run(new Text("Кол-во заказов")))));
                headerRow.Append(new TableCell(new Paragraph(new Run(new Text("Даты")))));
                table.Append(headerRow);

                // Add data rows
                foreach (var citySeries in series)
                {
                    var dataRow = new TableRow();
                    dataRow.Append(new TableCell(new Paragraph(new Run(new Text(citySeries.Key)))));
                    dataRow.Append(new TableCell(new Paragraph(new Run(new Text(citySeries.Value.Count.ToString())))));

                    var dates = string.Join(", ", citySeries.Value.Select(x => x.Parameter.ToString()));
                    dataRow.Append(new TableCell(new Paragraph(new Run(new Text(dates)))));

                    table.Append(dataRow);
                }

                body.AppendChild(table);

                mainPart.Document.Save();
            }
        });
    }
}
