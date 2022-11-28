using Micro.CQRS.Abstractions.Commands;
using Micro.Domain.Abstractions.Exceptions;
using Micro.Examples.Simple.Products.Domain;
using Micro.Examples.Simple.Products.Persistence;
using Micro.Persistence.Abstractions.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Micro.Examples.Simple.Products.Commands.Handlers;

internal sealed class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductsDbContext _dbContext;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork, ProductsDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public Task<Unit> HandleAsync(DeleteProductCommand command) => _unitOfWork.ExecuteAsync(async () =>
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == command.Id);
        if (product is null)
        {
            throw new EntityNotFoundException(typeof(Product), command.Id.ToString());
        }

        _dbContext.Remove(product);
        return Unit.Value;
    });
}