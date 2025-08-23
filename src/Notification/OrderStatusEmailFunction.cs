using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ECommerce.Notification.Functions;

public class OrderStatusEmailFunction
{
    private readonly ILogger _logger;

    public OrderStatusEmailFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<OrderStatusEmailFunction>();
    }

    [Function("SendEmail")]
    public void Run([RabbitMQTrigger("PaymentApproved", ConnectionStringSetting = "RabbitMqConnection")] string myQueueItem)
    {
        //TODO: ENVIAR NOTIFIACAO SIGNALR AO FRONT END
    }
}