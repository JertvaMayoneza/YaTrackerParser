using Microsoft.AspNetCore.Mvc;
using YaTrackerParser.Contracts.Interfaces;

namespace YaTrackerParser.Controllers
{
    /// <summary>
    /// Контроллер для управления задачами обработки тикетов.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TrackerController : ControllerBase
    {
        private readonly IMessageBrokerService _messageBroker;

        /// <summary>
        /// Инициализирует экземпляр <see cref="TrackerController"/> с фабрикой подключений RabbitMQ.
        /// </summary>
        /// <param name="connectionFactory">Фабрика подключения RabbitMQ.</param>
        public TrackerController(IMessageBrokerService messageBroker)
        {
            _messageBroker = messageBroker;
        }

        /// <summary>
        /// Отправляет сообщение о начале обработки тикетов в очередь RabbitMQ.
        /// </summary>
        /// <returns>
        /// Возвращает HTTP 200 (OK) в случае успешной отправки сообщения, 
        /// или HTTP 400 (Bad Request) в случае ошибки.
        /// </returns>
        /// <response code="200">Задача обработки тикетов успешно отправлена в очередь.</response>
        /// <response code="400">Ошибка отправки задачи обработки тикетов.</response>
        [HttpGet("tickets")]
        public async Task<IActionResult> GetTickets()
        {
            try
            {
                var message = "Start processing tickets";
                Console.WriteLine($"[Controller] Sending message: {message}");

                await _messageBroker.SendMessageAsync("ticket_queue", message);

                return Ok("Tickets processing task has been sent to the queue.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Controller] Error: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
