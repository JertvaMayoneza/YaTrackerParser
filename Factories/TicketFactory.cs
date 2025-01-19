using YaTrackerParser.Contracts.DTO;
using YaTrackerParser.Data.Context.Entites;

namespace YaTrackerParser.Factories;

/// <summary>
/// Фабрика для создания тикетов
/// </summary>
public class TicketFactory
{
    /// <summary>
    /// Создание тикета по умолчанию
    /// </summary>
    /// <param name="ticketData">Данные полученные после филтрации</param>
    /// <returns>Новый тикет</returns>
    public TicketEntity CreateFromTicketData(TicketData ticketData)
    {
        return new TicketEntity
        {
            TicketNumber = ticketData.TicketNumber,
            Time = ticketData.Time,
            Theme = ticketData.Theme,
            Description = ticketData.Description,
            UpdatedBy = ticketData.UpdatedBy
        };
    }
}
