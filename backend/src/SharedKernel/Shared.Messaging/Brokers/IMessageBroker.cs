using Shared.Abstractions.AdditionalAbstractions;
using Shared.Contexts.Contexts;
using Shared.Contexts.Providers;
using Shared.Messaging.Publishers;

namespace Shared.Messaging.Brokers;

public interface IMessageBroker
{
    Task PublishAsync<TEvent>(string exchange, string routingKey, TEvent message, CancellationToken cancellationToken = default)
        where TEvent : class, IEvent;
}

public class MessageBroker : IMessageBroker
{
    private readonly IMessagePublisher _publisher;
    private readonly IContextProvider _contextProvider;

    public MessageBroker(IMessagePublisher publisher,
                         IContextProvider contextProvider)
    {
        _publisher = publisher;
        _contextProvider = contextProvider;
    }

    public async Task PublishAsync<TEvent>(string exchange, string routingKey, TEvent message, CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        var messageId = Guid.NewGuid().ToString("N");
        var context = _contextProvider.Current();
        await _publisher.PublishAsync(exchange, routingKey, new MessageEnvelope<TEvent>(message, new MessageContext(messageId, context)));
    }
}