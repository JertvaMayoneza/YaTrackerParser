using Microsoft.AspNetCore.Mvc;
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
                // Процесс получения тикетов и записи их в файл
                await _getTicketsService.ProcessTicketsAsync();

                // Чтение отфильтрованных данных из файла
                var filteredTickets = await System.IO.File.ReadAllTextAsync("filtered_tickets.txt");

                // Возврат содержимого файла в ответе
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
            catch (Exception ex) // Для всех других исключений
            {
                return StatusCode(500, $"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
