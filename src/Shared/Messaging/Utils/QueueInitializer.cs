using RabbitMQ.Client;

namespace ECommerce.Shared.Messaging.Utils;
public class QueueInitializer
{
    private readonly IConnectionFactory _connectionFactory;

    public QueueInitializer(IConnectionFactory connectionFactory)
        => _connectionFactory = connectionFactory;

    public async Task EnsureQueueAsync(string queueName, CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken: cancellationToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken
            );
    }
}
