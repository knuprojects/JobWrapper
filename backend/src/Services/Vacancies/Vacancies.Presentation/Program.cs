using MapsterMapper;
using MongoDB.Driver;
using Shared.Abstractions.Primitives.Mongo;
using Shared.Dal;
using Vacancies.Core;
using Vacancies.Core.Entities;
using Vacancies.Core.Services;
using Vacancies.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistence();
builder.Services.AddCore();
builder.Services.AddMapper();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
