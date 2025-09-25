namespace ComponentLib;

public class CompToolTipManager
{
    private readonly ToolTip _toolTip;

    public CompToolTipManager()
    {
        _toolTip = new ToolTip();
        _toolTip.IsBalloon = true;
        _toolTip.AutoPopDelay = 5000; // исчезновение через 5 секунд
    }

    public void ShowError(Control control, string message)
    {
        /* _toolTip.ToolTipIcon = ToolTipIcon.Warning;
        _toolTip.ToolTipTitle = "[ Error ]"; */
        SetToolTipProperties(ToolTipIcon.Error, "[ Error ]");
        ShowToolTip(control, message);
    }

    public void ShowWarning(Control control, string message)
    {
        SetToolTipProperties(ToolTipIcon.Warning, "[ Warn ]");
        ShowToolTip(control, message);
    }

    public void ShowInfo(Control control, string message)
    {
        SetToolTipProperties(ToolTipIcon.Info, "[ Info ]");
        ShowToolTip(control, message);
    }

    public void ShowPlain(Control control, string message)
    {
        SetToolTipProperties(ToolTipIcon.None, string.Empty);
        ShowToolTip(control, message);
    }

    public void Hide(Control control)
    {
        _toolTip.Hide(control);
    }

    private void SetToolTipProperties(ToolTipIcon icon, string title)
    {
        _toolTip.ToolTipIcon = icon;
        _toolTip.ToolTipTitle = title;
    }

    private void ShowToolTip(Control control, string message)
    {
        _toolTip.SetToolTip(control, message);
        // 1. по центру элемента управления;
        // 2. рядом с элементом или курсором;
        // Если он появится при наведении мышью (дэфолт),
        // то _toolTip.SetToolTip() достаточно, и Show() не нужен
        // Для немедленного показа ошибка/предупреждение) лучше вызвать Show():
        _toolTip.Show(message, control, control.Width / 2, control.Height / 2, _toolTip.AutoPopDelay);
    }
}
