using RabbitMQ.Client;

namespace RabbitMqDemo.Common;

public interface IRabbitMqConnectionFactory
{
    public IConnection Create();
}