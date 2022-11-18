using System.Reflection;

namespace Micro.Domain.Abstractions.BuildingBlocks;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public bool Equals(ValueObject? other)
        => other is not null && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((ValueObject)obj);
    }

    public override int GetHashCode()
        => GetEqualityComponents()
            .Select(x => x is not null ? x.GetHashCode() : 0)
            .Aggregate(HashCode.Combine);
    
    protected virtual IEnumerable<object?> GetEqualityComponents()
        => GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(property => property.GetValue(this))
            .ToArray();

    public static bool operator ==(ValueObject lhs, ValueObject rhs) => lhs.Equals(rhs);
    public static bool operator !=(ValueObject lhs, ValueObject rhs) => !lhs.Equals(rhs);
}