using Microsoft.AspNetCore.Mvc;
using YaTrackerParser.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace YaTrackerParser;


[ApiController]
[Route("api/[controller]")]
public class TrackerController : ControllerBase
{
    private readonly ITicketProcessor _ticketProcessor;

    public TrackerController(ITicketProcessor ticketProcessor)
    {
        _ticketProcessor = ticketProcessor;
    }

    [HttpGet("tickets")]
    public async Task<IActionResult> GetTickets()
    {
        try
        {
            var tickets = await _ticketProcessor.ProcessTicketsAsync();

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "ticket_queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var message = System.Text.Json.JsonSerializer.Serialize(tickets);
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty,
                                 routingKey: "ticket_queue",
                                 body: body);

            return Ok("Tickets are being processed.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}

