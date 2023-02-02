namespace Shared.Dal;

public sealed class MssqlOptions
{
    public string MssqlConnection { get; set; } = string.Empty;

    public int MaxRetryCount { get; set; }
    public int CommandTimeout { get; set; }
    public bool EnableDetailedErrors { get; set; }
    public bool EnableSensitiveDataLogging { get; set; }
}

public sealed class PostgresOptions
{
    public string PostgresConnection { get; set; } = string.Empty;
}

public sealed class RedisOptions
{
    public string RedisConnection { get; set; } = string.Empty;
}

public sealed class MongoOptions
{
    public string MongoConnection { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;
}