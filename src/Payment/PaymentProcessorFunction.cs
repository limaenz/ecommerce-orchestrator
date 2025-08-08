using System.Text;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

using RabbitMQ.Client;

namespace ECommerce.Payment.Functions;

public class PaymentProcessorFunction
{
    private readonly ILogger _logger;

    public PaymentProcessorFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<PaymentProcessorFunction>();
    }

    [Function("ProcessPayment")]
    public async Task RunAsync([RabbitMQTrigger("StockVerified", ConnectionStringSetting = "RabbitMqConnection")] string stockMessage)
    {
        _logger.LogInformation("C# Queue trigger function processed: {item}", stockMessage);

        var factory = new ConnectionFactory { HostName = "localhost" };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "PaymentApproved",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var body = Encoding.UTF8.GetBytes("TESTE");

        var properties = new BasicProperties()
        {
            Persistent = true,
        };

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: "PaymentApproved",
            mandatory: false,
            basicProperties: properties,
            body: body
            );
    }
}