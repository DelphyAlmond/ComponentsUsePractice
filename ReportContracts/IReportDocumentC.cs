namespace ReportContracts;

public interface IReportDocumentC
{
    string DocumentFormat { get; } // [ * ] "docx", "xlsx", "pdf"
}
