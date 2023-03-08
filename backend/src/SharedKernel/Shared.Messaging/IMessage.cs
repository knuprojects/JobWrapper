namespace Shared.Messaging;

// Marker
public interface IMessage
{
}

public interface IEmptyMessage : IMessage
{
}

public class EmptyMessage : IEmptyMessage
{
}
