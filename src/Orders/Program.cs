using ECommerce.Orders.Api.Endpoints;
using ECommerce.Shared.Messaging.Interfaces;
using ECommerce.Shared.Services;

using RabbitMQ.Client;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var rabbitUri = builder.Configuration["RabbitMqConnection"]
                ?? "amqp://guest:guest@localhost:5672/%2F";

builder.Services.AddSingleton<IConnectionFactory>(_ =>
    new ConnectionFactory
    {
        Uri = new Uri(rabbitUri),
    });

builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => options.RouteTemplate = "/openapi/{documentName}.json");
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

var apiGroup = app.MapGroup("api");
OrderEndpoints.Map(apiGroup);

app.Run();
