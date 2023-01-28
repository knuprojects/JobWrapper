namespace Shared.Abstractions.Primitives;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    DateTime? LastModified { get; set; }
}
