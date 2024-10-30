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
        var existingTickets = await _context.Tickets
            .OrderByDescending(t => t.Time)
            .Take(30)
            .ToListAsync();

        foreach (var ticket in tickets)
        {
            var existingTicket = existingTickets
                .FirstOrDefault(t => t.TicketNumber == ticket.TicketNumber);

            if (existingTicket != null)
            {
                existingTicket.Time = ticket.Time;
            }
            else
            {
                var newTicket = new TicketEntity
                {
                    TicketNumber = ticket.TicketNumber,
                    Time = ticket.Time,
                    Theme = ticket.Theme
                };
                await _context.Tickets.AddAsync(newTicket);
            }
        }
        await _context.SaveChangesAsync();
    }
}
