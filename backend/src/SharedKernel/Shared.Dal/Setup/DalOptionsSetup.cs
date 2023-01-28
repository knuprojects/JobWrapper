using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Shared.Dal.Setup;

public static class BaseOptionsSetup
{
    public static string ConfigurationSectionMssqlName => "mssql-database";
    public static string ConfigurationSectionPostgresName => "postgres-database";
    public static string ConfigurationSectionRedisName => "redis-database";
    public static string ConfigurationSectionMongoName => "mongo-database";
}

internal sealed class DalOptionsMssqlSetup : IConfigureOptions<MssqlOptions>
{
    private readonly IConfiguration _configuration;

    public DalOptionsMssqlSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(MssqlOptions options)
    {
        if (options is not null)
        {
            var connectionString = _configuration.GetConnectionString("mssql-connection");

            if (!string.IsNullOrWhiteSpace(connectionString))
                options.DefaultConnection = connectionString;

            _configuration.GetSection(BaseOptionsSetup.ConfigurationSectionMssqlName).Bind(options);
        }
    }
}

internal sealed class DalOptionsPostgresSetup : IConfigureOptions<PostgresOptions>
{
    private readonly IConfiguration _configuration;

    public DalOptionsPostgresSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(PostgresOptions options)
    {
        if (options is not null)
        {
            var connectionString = _configuration.GetConnectionString("postgres-connection");

            if (!string.IsNullOrWhiteSpace(connectionString))
                options.DefaultConnection = connectionString;

            _configuration.GetSection(BaseOptionsSetup.ConfigurationSectionPostgresName).Bind(options);
        }
    }
}

internal sealed class DalRedisOptionsSetup : IConfigureOptions<RedisOptions>
{
    private readonly IConfiguration _configuration;

    public DalRedisOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(RedisOptions options)
    {
        if (options is not null)
        {
            var connectionString = _configuration.GetConnectionString("redis-connection");

            if (!string.IsNullOrWhiteSpace(connectionString))
                options.DefaultConnection = connectionString;

            _configuration.GetSection(BaseOptionsSetup.ConfigurationSectionRedisName).Bind(options);
        }
    }
}

internal sealed class DalMongoOptionsSetup : IConfigureOptions<MongoOptions>
{
    private readonly IConfiguration _configuration;

    public DalMongoOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(MongoOptions options)
    {
        if (options is not null)
        {
            var connectionString = _configuration.GetConnectionString("mongo-connection");

            if (!string.IsNullOrWhiteSpace(connectionString))
                options.DefaultConnection = connectionString;

            _configuration.GetSection(BaseOptionsSetup.ConfigurationSectionMongoName).Bind(options);
        }
    }
}

