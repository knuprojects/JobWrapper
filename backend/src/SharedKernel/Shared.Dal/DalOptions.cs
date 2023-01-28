namespace Shared.Dal;

public sealed class MssqlOptions
{
    public string DefaultConnection { get; set; } = string.Empty;

    public int MaxRetryCount { get; set; }
    public int CommandTimeout { get; set; }
    public bool EnableDetailedErrors { get; set; }
    public bool EnableSensitiveDataLogging { get; set; }
}

public sealed class PostgresOptions
{
    public string DefaultConnection { get; set; } = string.Empty;
}

public sealed class RedisOptions
{
    public string DefaultConnection { get; set; } = string.Empty;
}

public sealed class MongoOptions
{
    public string DefaultConnection { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;
}