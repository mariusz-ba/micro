using Micro.CQRS.Abstractions.Commands;
using Micro.Examples.Simple.Products.Domain;
using Micro.Examples.Simple.Products.Persistence;
using Micro.Persistence.Abstractions.UnitOfWork;

namespace Micro.Examples.Simple.Products.Commands.Handlers;

internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductsDbContext _dbContext;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, ProductsDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<Unit> HandleAsync(CreateProductCommand command)
    {
        var product = Product.Create(command.Id, command.Name, command.Price);
        _dbContext.Add(product);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}