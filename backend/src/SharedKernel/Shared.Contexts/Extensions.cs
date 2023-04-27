﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contexts.Accessors;
using Shared.Contexts.Providers;

namespace Shared.Contexts;

public static class Extensions
{
    private const string CorrelationIdKey = "correlation-id";

    public static IServiceCollection AddContexts(this IServiceCollection services)
        => services
                 .AddSingleton<IContextProvider, ContextProvider>()
                 .AddSingleton<IContextAccessor, ContextAccessor>()
                 .AddSingleton<IMessageIdAccessor, MessageIdAccessor>()
                 .AddSingleton<IMessageContextAccessor, MessageContextAccessor>()
                 .AddTransient<ContextHttpHandler>();

    public static IHttpClientBuilder AddContextHandler(this IHttpClientBuilder builder)
        => builder.AddHttpMessageHandler<ContextHttpHandler>();

    public static IApplicationBuilder UseContexts(this IApplicationBuilder app)
        => app.Use((ctx, next) =>
        {
            if (!ctx.Request.Headers.TryGetValue(CorrelationIdKey, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString("N");
            }

            ctx.Items.Add(CorrelationIdKey, correlationId);
            return next();
        });

    public static string? GetCorrelationId(this HttpContext context)
        => context.Items.TryGetValue(CorrelationIdKey, out var correlationId) ? correlationId?.ToString() : null;
}
