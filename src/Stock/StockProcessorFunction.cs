using System.Text.Json;

using ECommerce.Shared.Messaging.Interfaces;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ECommerce.Stock.Functions;

public class StockProcessorFunction
{
    private readonly ILogger _logger;
    private readonly IRabbitMQService _rabbitMqService;

    public StockProcessorFunction(ILoggerFactory loggerFactory, IRabbitMQService rabbitMqService)
    {
        _logger = loggerFactory.CreateLogger<StockProcessorFunction>();
        _rabbitMqService = rabbitMqService;
    }

    [Function("CheckStock")]
    public async Task RunAsync([RabbitMQTrigger("OrderCreated", ConnectionStringSetting = "RabbitMqConnection")] string orderMessage)
    {
        //TODO: CREATE PROCESS STOCK RULE

        await _rabbitMqService.PublishAsync(orderMessage, "StockVerified");
    }
}
