namespace RabbitMqDemo.Publisher;

public class PublisherSettings
{
    public required string Exchange { get; set; }
    public required string RoutingKey { get; set; }
    public int Fps { get; set; }
}