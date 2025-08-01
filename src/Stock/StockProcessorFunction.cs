using System.Text.Json;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ECommerce.Stock.Functions;

public class StockProcessorFunction
{
    private readonly ILogger _logger;

    public StockProcessorFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<StockProcessorFunction>();
    }

    [Function("CheckStock")]
    public void Run([RabbitMQTrigger("CrateOrder", ConnectionStringSetting = "RabbitMqConnection")] string orderMessage)
    {
        _logger.LogInformation($"Mensagem recebida: {orderMessage}");
    
        var order = JsonSerializer.Deserialize<OrderDto>(orderMessage);

        _logger.LogInformation($"Processando pedido ID: {order.Id}");
    }

    public record OrderDto(int Id, string ProductName, decimal Price, int Quantity);
}
