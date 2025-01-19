using System.Text;
using RabbitMQ.Client;
using YaTrackerParser.Contracts.Interfaces;

namespace YaTrackerParser.Infrastructure;

public class RabbitMQMessageBroker : IMessageBrokerService
{
    private readonly IConnectionFactory _connectionFactory;

    public RabbitMQMessageBroker(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task SendMessageAsync(string queueName, string message)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(message);
        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body);
    }
}