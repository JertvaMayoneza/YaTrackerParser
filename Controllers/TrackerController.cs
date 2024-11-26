using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using YaTrackerParser.Interfaces;

namespace YaTrackerParser
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackerController : ControllerBase
    {
        private readonly IConnectionFactory _connectionFactory;

        public TrackerController(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        [HttpGet("tickets")]
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
