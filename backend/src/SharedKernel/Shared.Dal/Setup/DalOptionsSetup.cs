using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Shared.Dal.Setup;

public static class BaseOptionsSetup
{
    public static string ConfigurationSectionPostgresName => "postgresDatabase";
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



