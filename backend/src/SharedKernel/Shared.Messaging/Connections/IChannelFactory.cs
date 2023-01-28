using RabbitMQ.Client;

namespace Shared.Messaging.Connections;

public interface IChannelFactory
{
    IModel Create();
}

internal sealed class ChannelFactory : IChannelFactory
{
    private readonly IConnection _connection;
    private readonly ChannelAccessor _channelAccessor;

    public ChannelFactory(IConnection connection, ChannelAccessor channelAccessor)
    {
        _connection = connection;
        _channelAccessor = channelAccessor;
    }

    public IModel Create()
        => _channelAccessor.Channel ?? (_channelAccessor.Channel = _connection.CreateModel());
}