namespace Micro.Domain.Abstractions.BuildingBlocks;

public interface IEntity<out TEntityId>
{
    TEntityId Id { get; }
}