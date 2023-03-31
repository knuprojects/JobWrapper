using Shared;
using Vacancies.Presentation.Infrastructure;

var builder = WebApplication
    .CreateBuilder(args)
    .AddShared();

builder.Services.AddDefault();

var app = builder.Build();

app.UseDefault();