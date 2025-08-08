using System.Text;
using System.Text.Json;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

using RabbitMQ.Client;

namespace ECommerce.Stock.Functions;

public class StockProcessorFunction
{
    private readonly ILogger _logger;

    public StockProcessorFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<StockProcessorFunction>();
    }

    [Function("CheckStock")]
    public async Task RunAsync([RabbitMQTrigger("CrateOrder", ConnectionStringSetting = "RabbitMqConnection")] string orderMessage)
    {
        _logger.LogInformation($"Mensagem recebida: {orderMessage}");

        var order = JsonSerializer.Deserialize<OrderDto>(orderMessage);

        _logger.LogInformation($"Processando pedido ID: {order.Id}");

        var factory = new ConnectionFactory { HostName = "localhost" };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "StockVerified",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var json = JsonSerializer.Serialize(order);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties()
        {
            Persistent = true,
        };

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: "StockVerified",
            mandatory: false,
            basicProperties: properties,
            body: body
            );

        _logger.LogInformation($"Pedido ID: {order.Id} SUCESSO!!!");
    }

    public record OrderDto(int Id, string ProductName, decimal Price, int Quantity);
}
