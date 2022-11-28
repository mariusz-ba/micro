namespace Micro.CQRS.Abstractions.Commands;

public readonly struct Unit
{
    private static readonly Unit Instance = new();

    public static ref readonly Unit Value => ref Instance;
}