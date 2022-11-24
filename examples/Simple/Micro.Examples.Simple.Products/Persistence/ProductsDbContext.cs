using Micro.Examples.Simple.Products.Domain;
using Microsoft.EntityFrameworkCore;

namespace Micro.Examples.Simple.Products.Persistence;

internal class ProductsDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}