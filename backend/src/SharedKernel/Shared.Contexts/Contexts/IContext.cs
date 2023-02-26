namespace Shared.Contexts.Contexts;

public interface IContext
{
    string TraceId { get; }
    string CorrelationId { get; }
    string UserId { get; }
}

public interface IMessageContext
{
    string MessageId { get; }
    IContext Context { get; }
}

public class Context : IContext
{
    public string TraceId { get; }

    public string CorrelationId { get; }

    public string UserId { get; }

    public Context()
    {
        TraceId = string.Empty;
        CorrelationId = Guid.NewGuid().ToString("N");
    }

    public Context(string traceId, string correlationId, string userId)
    {
        TraceId = traceId;
        CorrelationId = correlationId;
        UserId = userId;
    }
}

public class MessageContext : IMessageContext
{
    public string MessageId { get; set; }
    public IContext Context { get; }

    public MessageContext(
        string messageId,
        IContext context)
    {
        MessageId = messageId;
        Context = context;
    }

    public MessageContext(
        string messageId)
    {
        MessageId = messageId;
    }
}
