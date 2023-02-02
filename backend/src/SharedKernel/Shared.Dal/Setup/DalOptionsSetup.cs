using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Shared.Dal.Setup;

public static class BaseOptionsSetup
{
    public static string ConfigurationSectionMssqlName => "mssqlDatabase";
    public static string ConfigurationSectionPostgresName => "postgresDatabase";
    public static string ConfigurationSectionRedisName => "redisDatabase";
    public static string ConfigurationSectionMongoName => "mongoDatabase";
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
            var connectionString = _configuration.GetConnectionString("mssqlConnection");

            if (!string.IsNullOrWhiteSpace(connectionString))
                options.MssqlConnection = connectionString;

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
            var connectionString = _configuration.GetConnectionString("postgresConnection");

            if (!string.IsNullOrWhiteSpace(connectionString))
                options.PostgresConnection = connectionString;

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
            var connectionString = _configuration.GetConnectionString("redisConnection");

            if (!string.IsNullOrWhiteSpace(connectionString))
                options.RedisConnection = connectionString;

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
            var connectionString = _configuration.GetConnectionString("mongoConnection");

            if (!string.IsNullOrWhiteSpace(connectionString))
                options.MongoConnection = connectionString;

            _configuration.GetSection(BaseOptionsSetup.ConfigurationSectionMongoName).Bind(options);
        }
    }
}

