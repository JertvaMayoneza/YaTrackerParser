using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using YaTrackerParser.Interfaces;

public class TicketConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnectionFactory _connectionFactory;
    private const string QueueName = "ticket_queue";

    public TicketConsumer(IServiceProvider serviceProvider, IConnectionFactory connectionFactory)
    {
        _serviceProvider = serviceProvider;
        _connectionFactory = connectionFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = _connectionFactory;

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        Console.WriteLine($" [*] Waiting for messages in '{QueueName}' queue...");

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($" [x] Received: {message}");

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var ticketProcessor = scope.ServiceProvider.GetRequiredService<ITicketProcessor>();

                if (message == "Start processing tickets")
                {
                    Console.WriteLine(" [*] Starting ticket processing...");
                    await ticketProcessor.ProcessTicketsAsync();
                    Console.WriteLine(" [*] Ticket processing completed!");
                }

                await channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [!] Error processing message: {ex.Message}");
            }
        };

        await channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumer: consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
