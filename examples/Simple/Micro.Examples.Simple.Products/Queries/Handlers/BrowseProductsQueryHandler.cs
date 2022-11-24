using Micro.CQRS.Abstractions.Queries;
using Micro.Examples.Simple.Products.DTO;
using Micro.Examples.Simple.Products.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Micro.Examples.Simple.Products.Queries.Handlers;

internal sealed class BrowseProductsQueryHandler : IQueryHandler<BrowseProductsQuery, IEnumerable<ProductDto>>
{
    private readonly ProductsDbContext _dbContext;

    public BrowseProductsQueryHandler(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ProductDto>> HandleAsync(BrowseProductsQuery query)
        => await _dbContext.Products.AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new ProductDto(x.Id, x.Name, x.Price))
            .ToListAsync();
}