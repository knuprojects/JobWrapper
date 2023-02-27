using Shared;
using Users.Presentation.Infrastructure;

var builder = WebApplication
    .CreateBuilder(args)
    .AddShared();

builder.Services.AddDefault();

var app = builder.Build();

app.UseDefault();