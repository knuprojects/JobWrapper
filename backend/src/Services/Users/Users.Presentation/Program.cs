using Shared;
using Shared.Dal;
using Users.Persistence;

var builder = WebApplication
    .CreateBuilder(args)
    .AddShared();

//builder.Services.AddRedis();
builder.Services.AddMediator();
builder.Services.AddPersistence();


builder.Services.AddControllers();

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