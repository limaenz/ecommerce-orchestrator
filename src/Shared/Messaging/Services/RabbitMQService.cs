
using System.Text;
using System.Text.Json;

using ECommerce.Shared.Messaging.Interfaces;

using RabbitMQ.Client;

namespace ECommerce.Shared.Services;

public class RabbitMQService : IRabbitMQService, IDisposable
{
    private readonly IConnectionFactory _connectionFactory;

    public RabbitMQService(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public void Dispose()
    {
    }

    public async Task PublishAsync(string message, string queue, CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken: cancellationToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);


        await channel.QueueDeclareAsync(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);

        var body = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties()
        {
            Persistent = true,
        };

        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queue, mandatory: false, basicProperties: properties, body: body, cancellationToken: cancellationToken);
    }

    public async Task PublishAsync<T>(T message, string queue, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(message);
        await PublishAsync(json, queue, cancellationToken);
    }
}