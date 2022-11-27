using Micro.BackgroundJobs.EntityFrameworkCore.Persistence;
using Micro.Examples.Simple.Products.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Micro.Examples.Simple.Products.Persistence;

internal class ProductsDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<BackgroundJob> BackgroundJobs => Set<BackgroundJob>();

    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("Micro.BackgroundJobs.EntityFrameworkCore"));
    }
}