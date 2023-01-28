using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions;
using Shared.Api.Cors;
using Shared.Api.Environments;
using Shared.Api.Exceptions;
using Shared.Contexts;
using Shared.Http;
using Shared.Messaging;
using Shared.Observability.Logging;
using Shared.Security;
using System.Reflection;

namespace Shared;

public static class Extensions
{
    public static WebApplicationBuilder AddMicroFramework(this WebApplicationBuilder builder, Assembly assemblies)
    {
        var appOptions = builder.Configuration.GetSection("app").BindOptions<AppOptions>();
        var appInfo = new AppInfo(appOptions.Name, appOptions.Version);
        builder.Services.AddSingleton(appInfo);

        builder.Configuration.SetEnvironmentConfiguration(builder.Environment);

        builder
            .AddLogging()
            .Services
            .AddErrorHandling()
            .AddDispatchers(assemblies)
            .AddContexts()
            .AddHttpContextAccessor()
            .AddJwt(builder.Configuration)
            .AddCorsPolicy(builder.Configuration)
            .AddMessaging(builder.Configuration)
            .AddAbstractions()
            .AddLogger(builder.Configuration);

        builder.Services
            .AddCustomHttpClient(builder.Configuration)
            .AddContextHandler();

        return builder;
    }

    public static WebApplication UseShared(this WebApplication app)
    {
        app
           .UseCorsPolicy()
           .UseErrorHandling()
           .UseAuthentication()
           .UseRouting()
           .UseAuthorization()
           .UseContexts();

        return app;
    }
}