using Micro.CQRS.Abstractions.Commands;

namespace Micro.Examples.Simple.Products.Commands;

public record UpdateProductCommand(Guid Id, string Name, decimal Price) : ICommand;