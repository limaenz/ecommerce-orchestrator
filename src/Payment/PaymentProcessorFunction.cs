using ECommerce.Shared.Messaging.Interfaces;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ECommerce.Payment.Functions;

public class PaymentProcessorFunction
{
    private readonly ILogger _logger;
    private readonly IRabbitMQService _rabbitMQService;

    public PaymentProcessorFunction(ILoggerFactory loggerFactory, IRabbitMQService rabbitMQService)
    {
        _logger = loggerFactory.CreateLogger<PaymentProcessorFunction>();
        _rabbitMQService = rabbitMQService;
    }

    [Function("ProcessPayment")]
    public async Task RunAsync([RabbitMQTrigger("StockVerified", ConnectionStringSetting = "RabbitMqConnection")] string stockMessage)
    {
        //TODO: CREATE PROCESS PAYMENT RULE

        await _rabbitMQService.PublishAsync(stockMessage, "PaymentApproved");
    }
}