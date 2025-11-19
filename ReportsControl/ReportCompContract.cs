using ContractLib;

namespace ReportsControl;

public class ReportCompContract : IComponentContract
{
    public string Id => "c3_report";

    public string MenuTitle => "Отчет по заказам";

    public string Category => "Report";

    public UserControl GetComponentControl => new ReportDataGridView();
}
