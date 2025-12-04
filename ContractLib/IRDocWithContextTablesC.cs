namespace ContractLib;

public interface IRDocWithContextTablesC : IReportDocumentC
{
    /// Создание документа с простыми таблицами в асинхронном режиме
    Task CreateDocumentAsync(
        string filePath,          // Путь до файла
        string header,            // Заголовок документа
        List<string[,]> tables    // Список данных для таблиц
    );
}
