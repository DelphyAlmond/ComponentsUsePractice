namespace ContractLib;

public interface IRDocWithChartLineC : IReportDocumentC
{
    /// Создание документа с линейной диаграммой в асинхронном режиме
    Task CreateDocumentAsync(
        string filePath,
        string header,
        string chartTitle,
        Dictionary<string, List<(string Date, double Value)>> series // for full DateTime *
    );
}