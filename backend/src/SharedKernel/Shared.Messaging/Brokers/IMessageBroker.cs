using Microsoft.Extensions.Logging;
using Shared.Contexts.Contexts;
using Shared.Contexts.Providers;
using Shared.Messaging.Publishers;

namespace Shared.Messaging.Brokers;

public interface IMessageBroker
{
    ValueTask PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage;
}

public class MessageBroker : IMessageBroker
{
    private readonly IMessagePublisher _publisher;
    private readonly IContextProvider _contextProvider;
    private readonly ILogger<MessageBroker> _logger;

    public MessageBroker(
        IMessagePublisher publisher,
        IContextProvider contextProvider,
        ILogger<MessageBroker> logger)
    {
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        _contextProvider = contextProvider ?? throw new ArgumentNullException(nameof(contextProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        where TMessage : class, IMessage
    {
        var messageId = Guid.NewGuid().ToString("N");
        var context = _contextProvider.Current();

        _logger.LogInformation("Sending a message: [ID: {MessageId}, TraceId: {TraceId} Correlation ID: {CorrelationId}]...",
            messageId, context.TraceId, context.CorrelationId);

        await _publisher.PublishAsync(new MessageEnvelope<TMessage>(message, new MessageContext(messageId, context)), cancellationToken);
    }
}