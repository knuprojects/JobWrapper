namespace Shared.Abstractions.Primitives;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public abstract IEnumerable<object> GetValues();

    public bool Equals(ValueObject? other)
       => other is not null && ValuesArEqual(other);

    public override bool Equals(object? obj)
        => obj is ValueObject other && ValuesArEqual(other);

    public override int GetHashCode()
        => GetValues().Aggregate(default(int), HashCode.Combine);

    private bool ValuesArEqual(ValueObject other)
        => GetValues().SequenceEqual(other.GetValues());
}
