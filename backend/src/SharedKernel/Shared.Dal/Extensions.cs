﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared.Dal.Initializers;
using Shared.Dal.Repositories;
using Shared.Dal.Setup;
using Shared.Dal.Utils;
using Shared.Dal.Utils.Services;
using StackExchange.Redis;

namespace Shared.Dal;

public static class Extensions
{
    public static IServiceCollection AddMssqlDatabase<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        services.ConfigureOptions<DalOptionsMssqlSetup>();

        services.AddDbContext<TContext>((serviceProvider, dbContextOptionsBuilder) =>
        {
            var databaseOptions = serviceProvider.GetService<IOptions<MssqlOptions>>()!.Value;

            if (databaseOptions is not null)
            {
                dbContextOptionsBuilder.UseSqlServer(databaseOptions.MssqlConnection, sqlServerAction =>
                {
                    sqlServerAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount);

                    sqlServerAction.CommandTimeout(databaseOptions.CommandTimeout);
                });

                dbContextOptionsBuilder.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);

                dbContextOptionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
            }
        });

        services.AddHostedService<DatabaseInitializer<TContext>>();
        services.AddHostedService<DataInitializer>();

        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();

        return services;
    }

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


    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        services.ConfigureOptions<RedisOptions>();

        services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
        {
            var redisOptions = serviceProvider.GetService<IOptions<RedisOptions>>()!.Value;

            return ConnectionMultiplexer.Connect(redisOptions.RedisConnection);
        });

        services.AddScoped<ICacheService, CacheService>();

        return services;
    }

    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        services.ConfigureOptions<MongoOptions>();

        services.AddSingleton<IMongoDatabase>(options => {
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
