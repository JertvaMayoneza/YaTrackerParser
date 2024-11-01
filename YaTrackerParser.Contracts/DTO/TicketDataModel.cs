namespace YaTrackerParser.Contracts.DTO;

/// <summary>
/// Модель тикетов для Excel
/// </summary>
public class TicketData
{/// <summary>
/// Номер тикета
/// </summary>
    public string TicketNumber { get; set; } = null!;
    /// <summary>
    /// Время последнего обновления
    /// </summary>
    public string Time { get; set; } = null!;
    /// <summary>
    /// Тема тикета
    /// </summary>
    public string Theme { get; set; } = null!;
    /// <summary>
    /// Описание тикета
    /// </summary>
    public string Description { get; set; } = null!;
    /// <summary>
    /// Имя последнего обновившего тикет
    /// </summary>
    public string UpdatedBy { get; set; } = null!;
}
