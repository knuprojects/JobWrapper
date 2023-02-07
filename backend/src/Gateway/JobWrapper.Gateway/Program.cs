using JobWrapper.Gateway.Infrastructure;
using Shared;
using System.Reflection;

var builder = WebApplication
    .CreateBuilder(args)
    .AddShared(Assembly.GetExecutingAssembly());

builder.SetYarpConfiguration();

var app = builder.Build();

app.MapGet("/", () => "pong");

app.UseShared()
   .UseEndpoints(endpoints => endpoints.MapReverseProxy());

app.Run();