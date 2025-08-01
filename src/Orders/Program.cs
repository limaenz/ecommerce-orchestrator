using System.Text;
using System.Text.Json;

using RabbitMQ.Client;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

var apiGroup = app.MapGroup("api");

apiGroup.MapPost("/order", async (OrderDto orderDto) =>
{
    var factory = new ConnectionFactory { HostName = "localhost" };

    using var connection = await factory.CreateConnectionAsync();
    using var channel = await connection.CreateChannelAsync();

    await channel.QueueDeclareAsync(
        queue: "OrderCreated",
        durable: true,
        exclusive: false,
        autoDelete: false,
        arguments: null
    );

    var json = JsonSerializer.Serialize(orderDto);
    var body = Encoding.UTF8.GetBytes(json);

    var properties = new BasicProperties()
    {
        Persistent = true,
    };

    await channel.BasicPublishAsync(
        exchange: string.Empty,
        routingKey: "OrderCreated",
        mandatory: false,
        basicProperties: properties,
        body: body
        );

    return Results.Accepted();
})
.WithName("CreateOrder")
.WithOpenApi();

app.Run();

public record OrderDto(int Id, string ProductName, decimal Price, int Quantity);