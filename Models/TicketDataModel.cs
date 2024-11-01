namespace YaTrackerParser.Models;

/// <summary>
/// Модель тикетов для Excel
/// </summary>
public class TicketData
{
    public string TicketNumber { get; set; } = null!;
    public string Time { get; set; } = null!;
    public string Theme { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string UpdatedBy { get; set; } = null!;
}
