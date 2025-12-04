namespace ContractLib;

public interface IRDocWithTableColumnRowHeaderC : IReportDocumentC
{
    /// Создание документа с таблицей, у которой сложносоставная шапка
    Task CreateDocumentAsync<T>(
        string filePath,
        string header,
        List<int> columnsWidth,
        List<int> rowsHeights,
        bool isHeaderFirstRow,
        List<(string Header, string PropertyName, string FieldName)> headers,
        List<T> data
    );
}
