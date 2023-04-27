using JobWrapper.Gateway.Infrastructure;
using Shared;

var builder = WebApplication
    .CreateBuilder(args)
    .AddShared();

builder.SetYarpConfiguration();

var app = builder.Build();

app.MapGet("/", () => "pong");

app.UseShared()
   .UseEndpoints(endpoints => endpoints.MapReverseProxy());

app.Run();