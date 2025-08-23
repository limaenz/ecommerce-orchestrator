namespace ECommerce.Shared.Messaging.Interfaces;
public interface IRabbitMQService
{
    Task PublishAsync(string message, string queue, CancellationToken cancellationToken = default);
    Task PublishAsync<T>(T message, string queue, CancellationToken cancellationToken = default);
}
