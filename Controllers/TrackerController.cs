using Microsoft.AspNetCore.Mvc;
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
                await _ticketProcessor.ProcessTicketsAsync();

                var filteredTickets = await System.IO.File.ReadAllTextAsync("filtered_tickets.txt");

                return Ok(filteredTickets);
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
