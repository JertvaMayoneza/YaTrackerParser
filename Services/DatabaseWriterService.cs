using YaTrackerParser.Contracts.DTO;
using YaTrackerParser.Contracts.Interfaces;
using YaTrackerParser.Data.Context.Entites;

namespace YaTrackerParser.Services;

/// <summary>
/// Класс для записи тикетов в БД
/// </summary>
public class DatabaseWriterService : IDatabaseWriterService
{
    private readonly IRepository<TicketEntity> _ticketrepository;

    /// <summary>
    /// Создание экземпляра
    /// </summary>
    /// <param name="context">DB context</param>
    public DatabaseWriterService(IRepository<TicketEntity> ticketrepository)
    {
        _ticketrepository = ticketrepository;
    }

    /// <summary>
    /// Метод для записи тикетов в БД
    /// </summary>
    /// <param name="tickets">Отфильтрованные тикеты после сервиса TicketService</param>
    /// <returns>Запись тикетов в БД</returns>
    public async Task WriteToDatabaseAsync(IEnumerable<TicketData> tickets)
    {
        foreach (var ticket in tickets)
        {
            if (string.IsNullOrWhiteSpace(ticket.TicketNumber))
                continue;

            var existingTicket = await _ticketrepository.GetOrCreateAsync(
                t => t.TicketNumber == ticket.TicketNumber);

            if (existingTicket != null)
            {
                existingTicket.Time = ticket.Time;
                existingTicket.UpdatedBy = ticket.UpdatedBy;
                await _ticketrepository.UpdateAsync(existingTicket);
            }
        }
        await _ticketrepository.SaveChangesAsync();
    }
}
