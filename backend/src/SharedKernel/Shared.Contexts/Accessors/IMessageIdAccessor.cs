namespace Shared.Contexts.Accessors;

public interface IMessageIdAccessor
{
    string GetMessageId();
    void SetMessageId(string messageId);
}

internal sealed class MessageIdAccessor : IMessageIdAccessor
{
    private readonly AsyncLocal<string> _value = new();

    public string GetMessageId() => _value.Value;

    public void SetMessageId(string messageId) => _value.Value = messageId;
}
