using System.Text;

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

    await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
        arguments: null);

    const string message = "Hello World!";
    var body = Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);

    return Results.Accepted();
})
.WithName("CreateOrder")
.WithOpenApi();

app.Run();

public record OrderDto(string Product, int Quantity);