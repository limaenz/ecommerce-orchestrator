using ECommerce.Shared.Messaging.Interfaces;
using ECommerce.Shared.Messaging.Utils;
using ECommerce.Shared.Services;

using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using RabbitMQ.Client;

var builder = FunctionsApplication.CreateBuilder(args);

var rabbitUri = builder.Configuration["RabbitMqConnection"]
                ?? "amqp://guest:guest@localhost:5672/%2F";

builder.Services.AddSingleton<IConnectionFactory>(_ =>
    new ConnectionFactory
    {
        Uri = new Uri(rabbitUri),
    });

builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();
builder.Services.AddSingleton<QueueInitializer>();

builder.ConfigureFunctionsWebApplication();

var host = builder.Build();

var initializer = host.Services.GetRequiredService<QueueInitializer>();
await initializer.EnsureQueueAsync("StockVerified");

await host.RunAsync();
