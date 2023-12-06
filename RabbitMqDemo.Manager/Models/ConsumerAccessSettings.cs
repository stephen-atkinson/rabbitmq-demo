namespace RabbitMqDemo.Manager.Models;

public class ConsumerAccessSettings
{
    public required string Username { get; set; }
    public string? CompetitionId { get; set; }
    public string? SeasonId { get; set; }
    public string? LocationId { get; set; }
    public string? GameId { get; set; }
    public MessageGroup? Group { get; set; }
    public MessageType? Type { get; set; }
}