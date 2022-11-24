using Micro.CQRS.Abstractions.Commands;
using Micro.Domain.Abstractions.Exceptions;
using Micro.Examples.Simple.Products.Domain;
using Micro.Examples.Simple.Products.Persistence;
using Micro.Persistence.Abstractions.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Micro.Examples.Simple.Products.Commands.Handlers;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductsDbContext _dbContext;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, ProductsDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public Task HandleAsync(UpdateProductCommand command) => _unitOfWork.ExecuteAsync(async () =>
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == command.Id);
        if (product is null)
        {
            throw new EntityNotFoundException(typeof(Product), command.Id.ToString());
        }

        product.Update(command.Name, command.Price);
    });
}