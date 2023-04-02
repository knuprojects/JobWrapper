using Shared;
using Vacancies.Presentation.Infrastructure;

var builder = WebApplication
    .CreateBuilder(args)
    .AddShared();

builder.Services.AddDefault(builder.Configuration);

var app = builder.Build();

app.UseDefault();