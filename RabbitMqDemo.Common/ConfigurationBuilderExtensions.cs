using Microsoft.Extensions.Configuration;

namespace RabbitMqDemo.Common;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddRabbitMq(this IConfigurationBuilder builder) =>
        builder.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "rabbitmqsettings.json"), false, false);
}