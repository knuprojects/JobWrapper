using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Abstractions.AdditionalAbstractions;
using Shared.Contexts.Accessors;
using Shared.Messaging.Connections;
using System.Text;
using System.Text.Json;

namespace Shared.Messaging.Subscribers;

public interface IMessageSubscriber
{
    IMessageSubscriber SubscribeMessage<TEvent>(string queue, string routingKey, string exchange,
        Func<TEvent, BasicDeliverEventArgs, Task> handle) where TEvent : class, IEvent;
}

internal sealed class MessageSubscriber : IMessageSubscriber
{
    private readonly IMessageIdAccessor _messageIdAccessor;
    private readonly IModel _channel;

    public MessageSubscriber(IChannelFactory channelFactory, IMessageIdAccessor messageIdAccessor)
    {
        _channel = channelFactory.Create();
        _messageIdAccessor = messageIdAccessor;
    }

    public IMessageSubscriber SubscribeMessage<TEvent>(string queue, string routingKey, string exchange, Func<TEvent, BasicDeliverEventArgs, Task> handle)
        where TEvent : class, IEvent
    {
        _channel.ExchangeDeclare(exchange, "topic", durable: false, autoDelete: false, null);
        _channel.QueueDeclare(queue, durable: false, autoDelete: false, exclusive: false);
        _channel.QueueBind(queue, exchange, routingKey);

        _channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = JsonSerializer.Deserialize<TEvent>(Encoding.UTF8.GetString(body));

            _messageIdAccessor.SetMessageId(ea.BasicProperties.MessageId);

            await handle(message, ea);

            _channel.BasicAck(ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(queue, autoAck: false, consumer: consumer);

        return this;
    }
}