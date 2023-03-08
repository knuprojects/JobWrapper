﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared.Dal.Initializers;
using Shared.Dal.Repositories;
using Shared.Dal.Setup;
using Shared.Dal.Utils;

namespace Shared.Dal;

public static class Extensions
{
    public static IServiceCollection AddPostgresDatabase<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        services.ConfigureOptions<DalOptionsPostgresSetup>();

        services.AddDbContext<TContext>((serviceProvider, dbContextOptionsBuilder) =>
        {
            var databaseOptions = serviceProvider.GetService<IOptions<PostgresOptions>>()!.Value;

            if (databaseOptions is not null)
                dbContextOptionsBuilder.UseNpgsql(databaseOptions.PostgresConnection);
        });

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddHostedService<DatabaseInitializer<TContext>>();
        services.AddHostedService<DataInitializer>();

        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();

        return services;
    }


    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        services.ConfigureOptions<MongoOptions>();

        services.AddSingleton<IMongoDatabase>(options =>
        {
            var settings = options.GetService<IOptions<MongoOptions>>()!.Value;
            var client = new MongoClient(settings.MongoConnection);
            return client.GetDatabase(settings.DatabaseName);
        });

        services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

        return services;
    }

    public static IServiceCollection AddInitializer<T>(this IServiceCollection services) where T : class, IDataInitializer
         => services.AddTransient<IDataInitializer, T>();
}
