namespace Shared.Dal;


{
    public string MssqlConnection { get; set; } = string.Empty;

}
    public string RedisConnection { get; set; } = string.Empty;
}

public sealed class MongoOptions
{
    public string MongoConnection { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;
}