using Micro.CQRS.Abstractions.Queries;
using Micro.Examples.Simple.Products.DTO;
using Micro.Examples.Simple.Products.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Micro.Examples.Simple.Products.Queries.Handlers;

internal sealed class GetProductQueryHandler : IQueryHandler<GetProductQuery, ProductDto?>
{
    private readonly ProductsDbContext _dbContext;

    public GetProductQueryHandler(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<ProductDto?> HandleAsync(GetProductQuery query)
        => _dbContext.Products.AsNoTracking()
            .Where(x => x.Id == query.Id)
            .Select(x => new ProductDto(x.Id, x.Name, x.Price))
            .FirstOrDefaultAsync();
}