using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using YaTrackerParser.Contracts.Interfaces;

/// <summary>
/// Служба для обработки сообщений из очереди RabbitMQ.
/// </summary>
public class TicketConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnectionFactory _connectionFactory;
    private const string QueueName = "ticket_queue";

    /// <summary>
    /// Инициализирует экземпляр <see cref="TicketConsumer"/> с предоставленным сервис-провайдером и фабрикой подключений RabbitMQ.
    /// </summary>
    /// <param name="serviceProvider">Провайдер сервисов для создания зависимостей в скоупе.</param>
    /// <param name="connectionFactory">Фабрика подключений RabbitMQ.</param>
    public TicketConsumer(IServiceProvider serviceProvider, IConnectionFactory connectionFactory)
    {
        _serviceProvider = serviceProvider;
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Выполняет основной цикл обработки сообщений из очереди RabbitMQ.
    /// </summary>
    /// <param name="stoppingToken">Токен отмены, используемый для остановки службы.</param>
    /// <returns>Задача, представляющая выполнение службы.</returns>
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
