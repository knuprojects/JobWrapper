using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.Abstractions.Serialization;

public interface IJsonSerializer
{
    string Serialize<TEntity>(TEntity entity);
    TEntity? Deserialize<TEntity>(string value);
    object? Deserialize(string value, Type type);
}

public class TextJsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = {new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)}
    };

    public string Serialize<TEntity>(TEntity entity) => JsonSerializer.Serialize(entity, _options);

    public TEntity? Deserialize<TEntity>(string value) => JsonSerializer.Deserialize<TEntity>(value, _options);

    public object? Deserialize(string value, Type type) => JsonSerializer.Deserialize(value, type, _options);
}
