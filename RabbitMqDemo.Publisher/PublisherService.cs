using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMqDemo.Common.Models;

namespace RabbitMqDemo.Publisher;

public class PublisherService(IOptions<PublisherSettings> options, IConnection connection) : IHostedService, IDisposable
{
    private Timer? _timer;
    private IModel? _model;

    public Task StartAsync(CancellationToken stoppingToken)
    {
        var period = TimeSpan.FromMilliseconds(1000d / options.Value.Fps);
        
        _model = connection.CreateModel();
        _timer = new Timer(Publish, null, TimeSpan.Zero, period);

        return Task.CompletedTask;
    }

    private void Publish(object? state)
    {
        var players = Enumerable.Range(0, 5).Select(i => new PlayerPositioning
        {
            Id = i,
            Latitude = new decimal(Random.Shared.NextDouble()),
            Longitude = new decimal(Random.Shared.NextDouble())
        }).ToArray();
        
        var message = new PositioningMessage
        {
            DateTime = DateTime.UtcNow,
            Players = players
        };
        
        var messageBodyBytes = JsonSerializer.SerializeToUtf8Bytes(message);
        
        _model!.BasicPublish(options.Value.Exchange, options.Value.RoutingKey, null, messageBodyBytes);
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        _model?.Close();

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        _model?.Dispose();
    }
}