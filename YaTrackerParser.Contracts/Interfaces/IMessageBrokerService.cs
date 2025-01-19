namespace YaTrackerParser.Contracts.Interfaces;

public interface IMessageBrokerService
{
    Task SendMessageAsync(string queueName, string message);
}