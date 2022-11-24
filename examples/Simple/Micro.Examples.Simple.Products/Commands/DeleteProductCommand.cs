using Micro.CQRS.Abstractions.Commands;

namespace Micro.Examples.Simple.Products.Commands;

public record DeleteProductCommand(Guid Id) : ICommand;