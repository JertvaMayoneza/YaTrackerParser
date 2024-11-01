using Microsoft.EntityFrameworkCore;
using YaTrackerParser.Interfaces;
using YaTrackerParser.Models;

namespace YaTrackerParser.Services;

/// <summary>
/// Класс для записи тикетов в БД
/// </summary>
public class DatabaseWriterService : IDatabaseWriterService
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Создание экземпляра
    /// </summary>
    /// <param name="context">DB context</param>
    public DatabaseWriterService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Метод для записи тикетов в БД
    /// </summary>
    /// <param name="tickets">Отфильтрованные тикеты после сервиса TicketService</param>
    /// <returns>void</returns>
    public async Task WriteToDatabaseAsync(IEnumerable<TicketData> tickets)
    {
        foreach (var ticket in tickets)
        {
            if (string.IsNullOrWhiteSpace(ticket.TicketNumber))
                continue;

            var existingTicket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.TicketNumber == ticket.TicketNumber)
                .ConfigureAwait(false);

            if (existingTicket != null)
            {
                existingTicket.Time = ticket.Time;
                existingTicket.Description = ticket.Description;
                existingTicket.UpdatedBy = ticket.UpdatedBy;
                existingTicket.Theme = ticket.Theme;
            }
            else
            {
                var newTicket = new TicketEntity
                {
                    TicketNumber = ticket.TicketNumber,
                    Time = ticket.Time,
                    Theme = ticket.Theme,
                    Description = ticket.Description,
                    UpdatedBy = ticket.UpdatedBy
                };
                await _context.Tickets.AddAsync(newTicket).ConfigureAwait(false);
            }
        }
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
