namespace Shared.Abstractions.Primitives;

public abstract class Entity :  IEquatable<Entity>, IAuditableEntity
{
    public Entity()
    {
        Gid = Guid.NewGuid();
    }

    public Guid Gid { get; private set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModified { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;

        if(obj.GetType() != GetType()) return false;

        if(obj is not Entity entity) return false;

        return entity.Gid == Gid;
    }

    public bool Equals(Entity? other)
    {
        if(other is null) return false;

        if(other.GetType() != GetType()) return false;

        return other.Gid == Gid;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() * 41;
    }
}
