namespace YaTrackerParser.Data.Context.Entites;

/// <summary>
/// Модель для записи тикетов в БД
/// </summary>
public class TicketEntity
{/// <summary>
/// Primary key
/// </summary>
    public int Id { get; set; }
    /// <summary>
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
    /// Описание
    /// </summary>
    public string Description { get; set; } = null!;
    /// <summary>
    /// Имя последнего обновившего тикет
    /// </summary>
    public string UpdatedBy { get; set; } = null!;
}
