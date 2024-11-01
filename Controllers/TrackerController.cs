using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace YaTrackerParser.Controllers
{
    /// <summary>
    /// Контроллер для управления задачами обработки тикетов.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TrackerController : ControllerBase
    {
        private readonly IConnectionFactory _connectionFactory;

        /// <summary>
        /// Инициализирует экземпляр <see cref="TrackerController"/> с фабрикой подключений RabbitMQ.
        /// </summary>
        /// <param name="connectionFactory">Фабрика подключений RabbitMQ.</param>
        public TrackerController(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
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
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTickets()
        {
            try
            {
                var message = "Start processing tickets";
                Console.WriteLine($"[Controller] Sending message: {message}");

                using var connection = await _connectionFactory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(queue: "ticket_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync(exchange: string.Empty,
                                                 routingKey: "ticket_queue",
                                                 body: body);

                Console.WriteLine("[Controller] Message sent successfully!");

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
