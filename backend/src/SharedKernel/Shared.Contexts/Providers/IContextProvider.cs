using Microsoft.AspNetCore.Http;
using Shared.Contexts.Accessors;
using Shared.Contexts.Contexts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contexts.Providers;

public interface IContextProvider
{
    IContext Current();
}

internal sealed class ContextProvider : IContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IContextAccessor _contextAccessor;

    public ContextProvider(
        IHttpContextAccessor httpContextAccessor,
        IContextAccessor contextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _contextAccessor = contextAccessor;
    }

    public IContext Current()
    {
        if (_contextAccessor.Context is not null)
        {
            return _contextAccessor.Context;
        }

        IContext context;
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is not null)
        {
            var traceId = httpContext.TraceIdentifier;
            var correlationId = httpContext.GetCorrelationId() ?? Guid.NewGuid().ToString("N");
            var userId = httpContext.User.Identity?.Name;
            context = new Context(traceId, correlationId, userId);
        }
        else
        {
            context = new Context(Activity.Current?.Id ?? ActivityTraceId.CreateRandom().ToString(),
                string.Empty, Guid.NewGuid().ToString("N"));
        }

        _contextAccessor.Context = context;

        return context;
    }
}