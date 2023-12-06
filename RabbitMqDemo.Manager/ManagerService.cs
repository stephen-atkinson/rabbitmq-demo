using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMqDemo.Manager.Models;

namespace RabbitMqDemo.Manager;

public class ManagerService(
    IConnection connection,
    IOptions<ManagerSettings> managerOptions,
    IOptions<List<ConsumerAccessSettings>> consumerAccessOptions,
    IOptions<List<GameSettings>> gameOptions)
{
    private static readonly IDictionary<string, object> DefaultQueueArguments = new Dictionary<string, object>
        { { "x-queue-type", "quorum" }, };

    public Task SetupAsync()
    {
        using var model = connection.CreateModel();
        
        SetupBasic(model);
        SetupExchanges(model);
        SetupQueues(model);

        model.Close();

        return Task.CompletedTask;
    }

    private static void SetupBasic(IModel model)
    {
        model.ExchangeDeclare("basic", ExchangeType.Direct, true);
        model.QueueDeclare("basic", true, false, false, DefaultQueueArguments);
        model.QueueBind("basic", "basic", "basic");
    }

    private void SetupExchanges(IModel model)
    {
        model.ExchangeDeclare(managerOptions.Value.PositioningExchangeName, ExchangeType.Topic, true);

        foreach (var consumer in consumerAccessOptions.Value.GroupBy(s => s.Username))
        {
            var consumerExchangeName = GetConsumerExchangeName(consumer.First());
            var deadLetterExchangeName = GetConsumerDeadLetterExchangeName(consumer.First());
            var deadLetterQueueName = $"{consumerExchangeName}.dead-letter";

            model.ExchangeDeclare(consumerExchangeName, ExchangeType.Topic, true);
            model.ExchangeDeclare(deadLetterExchangeName, ExchangeType.Fanout, true);

            model.QueueDeclare(deadLetterQueueName, true, false, false, DefaultQueueArguments);
            model.QueueBind(deadLetterQueueName, deadLetterExchangeName, string.Empty);

            foreach (var accessSettings in consumer)
            {
                var routingKey = CreateConsumerAccessRoutingKey(accessSettings);

                model.ExchangeBind(consumerExchangeName, managerOptions.Value.PositioningExchangeName, routingKey);
            }
        }
    }

    private void SetupQueues(IModel model)
    {
        foreach (var gameSettings in gameOptions.Value)
        {
            foreach (var messageGroup in Enum.GetValues<MessageGroup>())
            {
                foreach (var messageType in Enum.GetValues<MessageType>())
                {
                    var messageRoutingKey = CreateMessageRoutingKey(gameSettings, messageGroup, messageType);
                    var messageRoutingKeyRegex =
                        CreateMessageRoutingKeyAccessRegex(gameSettings, messageGroup, messageType);

                    foreach (var consumerAccessSettings in consumerAccessOptions.Value)
                    {
                        var consumerAccessRoutingKey = CreateConsumerAccessRoutingKey(consumerAccessSettings);

                        if (!messageRoutingKeyRegex.IsMatch(consumerAccessRoutingKey))
                        {
                            continue;
                        }

                        var queueArguments = new Dictionary<string, object>(DefaultQueueArguments)
                        {
                            { "x-dead-letter-exchange", GetConsumerDeadLetterExchangeName(consumerAccessSettings) },
                        };

                        var exchangeName = GetConsumerExchangeName(consumerAccessSettings);
                        var queueName = GetQueueName(consumerAccessSettings, gameSettings, messageGroup, messageType);

                        model.QueueDeclare(queueName, true, false, false, queueArguments);
                        model.QueueBind(queueName, exchangeName, messageRoutingKey);
                    }
                }
            }
        }
    }

    private static string GetConsumerExchangeName(ConsumerAccessSettings settings) => settings.Username.ToLower();

    private static string GetConsumerDeadLetterExchangeName(ConsumerAccessSettings settings) =>
        $"{GetConsumerExchangeName(settings)}.dlx";

    private static string GetQueueName(ConsumerAccessSettings consumerAccessSettings, GameSettings gameSettings,
        MessageGroup messageGroup, MessageType messageType) =>
        new StringBuilder()
            .Append(consumerAccessSettings.Username)
            .Append($".{gameSettings.LocationId}")
            .Append($".{messageGroup}")
            .Append($".{messageType}")
            .ToString()
            .ToLower();

    private static string CreateMessageRoutingKey(GameSettings gameSettings, MessageGroup messageGroup,
        MessageType messageType) =>
        new StringBuilder()
            .Append(gameSettings.CompetitionId)
            .Append($".{gameSettings.SeasonId}")
            .Append($".{gameSettings.LocationId}")
            .Append($".{gameSettings.Id}")
            .Append($".{messageGroup}")
            .Append($".{messageType}")
            .ToString()
            .ToLower();

    private static string CreateConsumerAccessRoutingKey(ConsumerAccessSettings settings) =>
        new StringBuilder()
            .Append($"{settings.CompetitionId ?? "*"}")
            .Append($".{settings.SeasonId ?? "*"}")
            .Append($".{settings.LocationId ?? "*"}")
            .Append($".{settings.GameId ?? "*"}")
            .Append($".{settings.Group?.ToString() ?? "*"}")
            .Append($".{settings.Type?.ToString() ?? "*"}")
            .ToString()
            .ToLower();

    private static Regex CreateMessageRoutingKeyAccessRegex(GameSettings gameSettings, MessageGroup messageGroup,
        MessageType messageType)
    {
        var pattern = new StringBuilder()
            .Append('^')
            .Append($"({gameSettings.CompetitionId}|\\*)")
            .Append($".({gameSettings.SeasonId}|\\*)")
            .Append($".({gameSettings.LocationId}|\\*)")
            .Append($".({gameSettings.Id}|\\*)")
            .Append($".({messageGroup}|\\*)")
            .Append($".({messageType}|\\*)")
            .Append('$')
            .ToString();

        return new Regex(pattern, RegexOptions.IgnoreCase);
    }
}