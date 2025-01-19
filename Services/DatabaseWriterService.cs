using YaTrackerParser.Contracts.DTO;
using YaTrackerParser.Contracts.Interfaces;
using YaTrackerParser.Data.Context.Entites;
using YaTrackerParser.Factories;

namespace YaTrackerParser.Services;

/// <summary>
/// Класс для записи тикетов в БД
/// </summary>
public class DatabaseWriterService : IDatabaseWriterService
{
    private readonly IRepository<TicketEntity> _ticketrepository;
    private readonly TicketFactory _ticketFactory;

    /// <summary>
    /// Создание экземпляра
    /// </summary>
    /// <param name="context">DB context</param>
    public DatabaseWriterService(IRepository<TicketEntity> ticketrepository, TicketFactory ticketFactory)
    {
        _ticketrepository = ticketrepository;
        _ticketFactory = ticketFactory;
    }

    /// <summary>
    /// Метод для записи тикетов в БД
    /// </summary>
    /// <param name="tickets">Отфильтрованные тикеты после сервиса TicketFilterService</param>
    /// <returns>Запись тикетов в БД</returns>
    public async Task WriteToDatabaseAsync(IEnumerable<TicketData> tickets)
    {
        foreach (var ticket in tickets)
        {
            if (string.IsNullOrWhiteSpace(ticket.TicketNumber))
                continue;

            var existingTicket = await _ticketrepository.FindSingleAsync(
                t => t.TicketNumber == ticket.TicketNumber);

            if (existingTicket == null)
            {
                existingTicket = _ticketFactory.CreateFromTicketData(ticket);
                await _ticketrepository.AddAsync(existingTicket);
            }
            else
            {
                existingTicket.Time = ticket.Time;
                existingTicket.Theme = ticket.Theme;
                existingTicket.Description = ticket.Description;
                existingTicket.UpdatedBy = ticket.UpdatedBy;
                await _ticketrepository.UpdateAsync(existingTicket);
            }
        }
        await _ticketrepository.SaveChangesAsync();
    }
}
