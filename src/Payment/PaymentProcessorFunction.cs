using ECommerce.Shared.Messaging.Interfaces;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ECommerce.Payment.Functions;

public class PaymentProcessorFunction
{
    private readonly ILogger _logger;
    private readonly IRabbitMQService _rabbitMqService;

    public PaymentProcessorFunction(ILoggerFactory loggerFactory, IRabbitMQService rabbitMqService)
    {
        _logger = loggerFactory.CreateLogger<PaymentProcessorFunction>();
        _rabbitMqService = rabbitMqService;
    }

    [Function("ProcessPayment")]
    public async Task RunAsync([RabbitMQTrigger("StockVerified", ConnectionStringSetting = "RabbitMqConnection")] string stockMessage)
    {
        //TODO: CREATE PROCESS PAYMENT RULE

        await _rabbitMqService.PublishAsync(stockMessage, "PaymentApproved");
    }
}