using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using YaTrackerParser.Interfaces;
using YaTrackerParser.Models;

public class TicketConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public TicketConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "ticket_queue", 
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var tickets = System.Text.Json.JsonSerializer.Deserialize<List<TicketData>>(message);

            using var scope = _serviceProvider.CreateScope();
            var filterService = scope.ServiceProvider.GetRequiredService<ITicketFilterService>();
            var dbWriterService = scope.ServiceProvider.GetRequiredService<IDatabaseWriterService>();
            var fileWriterService = scope.ServiceProvider.GetRequiredService<IFileWriterService>();

            // Обработка данных
            var filteredTickets = filterService.FilterTickets(tickets);
            await dbWriterService.WriteToDatabaseAsync(filteredTickets);
            fileWriterService.WriteToExcel(filteredTickets);
        };

        await channel.BasicConsumeAsync("ticket_queue", 
                                        autoAck: true,
                                        consumer: consumer);

        return Task.CompletedTask;
    }
}
