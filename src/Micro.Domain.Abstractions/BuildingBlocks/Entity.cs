namespace Micro.Domain.Abstractions.BuildingBlocks;

public abstract class Entity<TEntityId>
{
    public TEntityId Id { get; }

    protected Entity(TEntityId id)
    {
        Id = id;
    }
}