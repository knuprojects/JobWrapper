using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Shared.Dal.Setup;

public static class BaseOptionsSetup
{
    public static string ConfigurationSectionPostgresName => "postgresDatabase";
    public static string ConfigurationSectionRedisName => "redisDatabase";
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

