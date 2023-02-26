using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contexts.Accessors;

namespace Shared.Messaging.Publishers;

public interface IMessagePublisher
{
    ValueTask PublishAsync<TMessage>(MessageEnvelope<TMessage> message, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage;
}

public class MessagePublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMessageContextAccessor _messageContextAccessor;
    private readonly IMessageIdAccessor _messageIdAccessor;
    private readonly ILogger<MessagePublisher> _logger;

    public MessagePublisher(
        IPublishEndpoint publishEndpoint,
        IMessageContextAccessor messageContextAccessor,
        IMessageIdAccessor messageIdAccessor,
        ILogger<MessagePublisher> logger)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _messageContextAccessor = messageContextAccessor ?? throw new ArgumentNullException(nameof(messageContextAccessor));
        _messageIdAccessor = messageIdAccessor ?? throw new ArgumentNullException(nameof(messageIdAccessor));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask PublishAsync<TMessage>(MessageEnvelope<TMessage> message, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
    {
        var messageContext = message.Context;
        _messageContextAccessor.MessageContext = messageContext;

        messageContext.MessageId = _messageIdAccessor.GetMessageId() ?? Guid.NewGuid().ToString("N");

        await _publishEndpoint.Publish(message.Message, cancellationToken);

        _logger.LogInformation($"Message with: {messageContext.MessageId}, {messageContext.Context.CorrelationId} was published!");

        await Task.CompletedTask;
    }
}
