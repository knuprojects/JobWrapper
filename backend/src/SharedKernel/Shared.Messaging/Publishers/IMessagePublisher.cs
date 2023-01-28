using RabbitMQ.Client;
using Shared.Abstractions.AdditionalAbstractions;
using Shared.Contexts.Accessors;
using Shared.Messaging.Connections;
using System.Text;
using System.Text.Json;

namespace Shared.Messaging.Publishers;

public interface IMessagePublisher
{
    Task PublishAsync<TEvent>(string exchange, string routingKey, MessageEnvelope<TEvent> message)
        where TEvent : class, IEvent;
}

public class MessagePublisher : IMessagePublisher
{
    private readonly IModel _channel;
    private readonly IMessageContextAccessor _messageContextAccessor;
    private readonly IMessageIdAccessor _messageIdAccessor;

    public MessagePublisher(IChannelFactory channelFactory,
                            IMessageContextAccessor messageContextAccessor,
                            IMessageIdAccessor messageIdAccessor)
    {
        _channel = channelFactory.Create();
        _messageContextAccessor = messageContextAccessor;
        _messageIdAccessor = messageIdAccessor;
    }

    public async Task PublishAsync<TEvent>(string exchange, string routingKey, MessageEnvelope<TEvent> message) where TEvent : class, IEvent
    {
        var messageContext = message.Context;
        _messageContextAccessor.MessageContext = messageContext;

        var json = JsonSerializer.Serialize(message.Message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = _channel.CreateBasicProperties();
        properties.MessageId = _messageIdAccessor.GetMessageId() ?? Guid.NewGuid().ToString("N");

        _channel.ExchangeDeclare(exchange, "topic", false, false);
        _channel.BasicPublish(exchange, routingKey, mandatory: true, properties, body);

        await Task.CompletedTask;
    }
}