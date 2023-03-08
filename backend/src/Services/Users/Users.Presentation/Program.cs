using Shared;
using Users.Persistence;

var builder = WebApplication
    .CreateBuilder(args)
    .AddShared();

builder.Services.AddMediator();
builder.Services.AddPersistence();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseShared();

app.MapControllers();

app.Run();