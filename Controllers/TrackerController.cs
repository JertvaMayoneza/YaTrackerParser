using Microsoft.AspNetCore.Mvc;
using YaTrackerParser.Interfaces;

namespace YaTrackerParser;

/// <summary>
/// Контроллер API
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TrackerController : ControllerBase
{
    private readonly ITicketProcessor _ticketProcessor;

    /// <summary>
    /// Создание экземлпяра контроллера
    /// </summary>
    /// <param name="ticketProcessor">экземпляр TicketProcessor</param>
    public TrackerController(ITicketProcessor ticketProcessor)
    {
        _ticketProcessor = ticketProcessor;
    }

    /// <summary>
    /// Получить список тикетов.
    /// </summary>
    /// <returns>Список тикетов.</returns>
    [HttpGet("tickets")]
    public async Task<IActionResult> GetTickets()
    {
        try
        {
            var tickets = await _ticketProcessor.ProcessTicketsAsync();
            return Ok(tickets);
        }

        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
