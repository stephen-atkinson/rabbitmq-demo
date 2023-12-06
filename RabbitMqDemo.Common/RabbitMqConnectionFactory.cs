using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMqDemo.Common.Models;

namespace RabbitMqDemo.Common;

public class RabbitMqConnectionFactory(IOptions<RabbitMqSettings> options, ILogger<RabbitMqConnectionFactory> logger) : IRabbitMqConnectionFactory
{
    public IConnection Create()
    {
        var settings = options.Value;

        var connectionFactory = new ConnectionFactory
        {
            UserName = settings.Username,
            Password = settings.Password,
            VirtualHost = settings.VirtualHost,
            DispatchConsumersAsync = true,
            ClientProvidedName = $"{nameof(RabbitMqDemo)}.{Environment.MachineName}",
        };

        var endpoints = settings.Endpoints
            .Select(e => new AmqpTcpEndpoint(new Uri(e))).ToArray();

        foreach (var i in Enumerable.Range(1, settings.MaxInitialConnectionRetries))
        {
            try
            {
                return connectionFactory.CreateConnection(endpoints);
            }
            catch (BrokerUnreachableException ex)
            {
                logger.LogError(ex, "Failed to connect to RabbitMq. Attempt: {Attempt}/{MaxInitialConnectionRetries}.", i, settings.MaxInitialConnectionRetries);

                if (i == settings.MaxInitialConnectionRetries)
                {
                    throw;
                }
                
                Thread.Sleep(5000);
            }
        }

        throw new UnreachableException();
    }
}