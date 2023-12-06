using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMqDemo.Common.Models;

namespace RabbitMqDemo.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection serviceCollection, IConfiguration configuration) => serviceCollection
        .Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"))
        .AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>()
        .AddSingleton<IConnection>(s => s.GetRequiredService<IRabbitMqConnectionFactory>().Create());
}