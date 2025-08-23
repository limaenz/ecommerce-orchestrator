using ECommerce.Orders.Api.Models;
using ECommerce.Shared.Messaging.Interfaces;

namespace ECommerce.Orders.Api.Endpoints;

public static class OrderEndpoints
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.Map("/order", CreateOrder)
            .WithName("CreateOrder")
            .WithOpenApi();
    }

    private static async Task<IResult> CreateOrder(OrderDto orderDto, IRabbitMQService rabbitMQService)
    {
        await rabbitMQService.PublishAsync(orderDto, "OrderCreated");

        //TODO: CREATE PROCESS ORDER RULE

        return Results.Accepted();
    }

}
