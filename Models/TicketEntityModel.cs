namespace YaTrackerParser.Models;

/// <summary>
/// Модель для записи тикетов в БД
/// </summary>
public class TicketEntity
{
    public int Id { get; set; }
    public string TicketNumber { get; set; }
    public string Time { get; set; }
    public string Theme { get; set; }
}
