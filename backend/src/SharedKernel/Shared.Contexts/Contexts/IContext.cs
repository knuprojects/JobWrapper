namespace Shared.Contexts.Contexts;

public interface IContext
{
    string TraceId { get; }
    string CorrelationId { get; }
    string UserId { get; }
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

public record MessageContext(string MessageId, IContext Context);