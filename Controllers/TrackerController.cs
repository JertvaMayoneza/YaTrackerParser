using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using YaTrackerParser.Services;

namespace YaTrackerParser
{
    [ApiController]
    [Route("[controller]")]
    public class TrackerController : ControllerBase
    {
        private readonly TicketProcessor _ticketProcessor;

        public TrackerController(TicketProcessor ticketProcessor)
        {
            _ticketProcessor = ticketProcessor;
        }

        [HttpGet("tickets")]
        public async Task<IActionResult> GetTickets()
        {
            try
            {
                var tickets = await _ticketProcessor.ProcessTicketsAsync();

                // Возвращаем список тикетов для проверки форматов
                return Ok(tickets);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка: {ex.Message}");
            }
        }


    }
}
