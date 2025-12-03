namespace ReportContracts;

public interface IRDocWithChartLineC : IReportDocumentC
{
    /// Создание документа с линейной диаграммой в асинхронном режиме
    Task CreateDocumentAsync(
        string filePath,
        string header,
        string chartTitle, // Заголовок диаграммы
        Dictionary<string, List<(int Parameter, double Value)>> series
        // ^ Словарь серий с данными для линейной диаграммы
    );
}
