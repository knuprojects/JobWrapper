using Shared.Contexts;
using Yarp.ReverseProxy.Transforms;

namespace JobWrapper.Gateway.Infrastructure;

public static class SetYarpConfig
{
    public static WebApplicationBuilder SetYarpConfiguration(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
            builder.Host.ConfigureAppConfiguration(cfg => cfg.AddJsonFile("yarp.dev.json", false));
        else
            builder.Host.ConfigureAppConfiguration(cfg => cfg.AddJsonFile("yarp.prod.json", false));

        builder.Services
                    .AddReverseProxy()
                    .LoadFromConfig(builder.Configuration.GetRequiredSection("reverseProxy"))
                    .AddTransforms(builderContext =>
                    {
                        builderContext.AddRequestTransform(transformContext =>
                        {
                            var correlationId = transformContext.HttpContext.GetCorrelationId() ?? Guid.NewGuid().ToString("N");
                            transformContext.ProxyRequest.Headers.Add("correlation-id", correlationId);
                            return ValueTask.CompletedTask;
                        });
                    });

        return builder;
    }

}