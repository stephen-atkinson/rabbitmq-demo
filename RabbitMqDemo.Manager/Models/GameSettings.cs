namespace RabbitMqDemo.Manager.Models;

public class GameSettings
{
    public required string Id { get; set; }
    public required string CompetitionId { get; set; }
    public required string SeasonId { get; set; }
    public required string LocationId { get; set; }
}