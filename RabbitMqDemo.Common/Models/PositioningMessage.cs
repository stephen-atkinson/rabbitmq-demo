namespace RabbitMqDemo.Common.Models;

public class PositioningMessage
{
    public DateTime DateTime { get; set; }
    public required IReadOnlyCollection<PlayerPositioning> Players { get; set; }
}