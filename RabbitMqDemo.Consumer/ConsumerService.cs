using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqDemo.Common;
using RabbitMqDemo.Common.Models;

namespace RabbitMqDemo.Consumer;

public class ConsumerService(IOptions<ConsumerSettings> options, IConnection connection) : IHostedService, IDisposable
{
    private IModel? _model;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _model = connection.CreateModel();

        var consumer = new AsyncEventingBasicConsumer(_model);
        
        consumer.Received += ConsumerOnReceived;
        
        _model.BasicConsume(options.Value.Queue, true, consumer);
        
        return Task.CompletedTask;
    }

    private static Task ConsumerOnReceived(object sender, BasicDeliverEventArgs @event)
    {
        var body = @event.Body.ToArray();

        var positioningMessage = JsonSerializer.Deserialize<PositioningMessage>(body)!;
        
        Console.WriteLine(positioningMessage.DateTime);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _model?.Close();
        
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _model?.Dispose();
    }
}