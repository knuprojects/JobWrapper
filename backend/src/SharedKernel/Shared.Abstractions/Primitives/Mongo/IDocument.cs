using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Abstractions.Primitives.Mongo;

public interface IDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    ObjectId Id { get; set; }

    DateTime CreatedAt { get; }
}

public abstract class BaseDocument : IDocument
{
    public ObjectId Id { get; set; }

    public DateTime CreatedAt => Id.CreationTime;
}