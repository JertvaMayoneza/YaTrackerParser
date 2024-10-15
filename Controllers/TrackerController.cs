using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YaTrackerParser.Services;

namespace YaTrackerParser
{
    [ApiController]
    [Route("[controller]")]
    public class TrackerController : ControllerBase
    {
        private readonly GetTicketsService _getTicketsService;

        public TrackerController(GetTicketsService getTicketsService)
        {
            _getTicketsService = getTicketsService;
        }

        [HttpGet("tickets")]
        public async Task<IActionResult> GetTickets()
        {
            try
            {
                var tickets = await _getTicketsService.GetTicketsAsync();
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
        }
    }
}
