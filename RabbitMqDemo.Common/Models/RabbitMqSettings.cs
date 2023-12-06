namespace RabbitMqDemo.Common.Models;

public class RabbitMqSettings
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string VirtualHost { get; set; }
    public required IReadOnlyCollection<string> Endpoints { get; set; }
    public int MaxInitialConnectionRetries { get; set; }
}
