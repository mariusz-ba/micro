using Micro.CQRS.Abstractions.Commands;

namespace Micro.Examples.Simple.Products.Commands;

public record CreateProductCommand(Guid Id, string Name, decimal Price) : ICommand;