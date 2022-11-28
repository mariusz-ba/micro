using Micro.BackgroundJobs.SqlServer;
using Micro.CQRS;
using Micro.Common.ReverseProxy;
using Micro.Common;
using Micro.Contexts;
using Micro.Domain;
using Micro.Examples.Simple.Products.Persistence;
using Micro.Examples.Simple.Products;
using Micro.Observability.ApplicationInsights;
using Micro.Persistence.SqlServer;

var assemblies = AssembliesProvider.GetAssemblies();

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMicro(builder.Configuration)
    .AddContexts()
    .AddCQRS(assemblies)
    .AddDomainEvents(assemblies)
    .AddPersistence<ProductsDbContext>(builder.Configuration)
    .AddBackgroundJobs<ProductsDbContext>(builder.Configuration.GetSection("BackgroundJobs"))
    .AddObservability()
    .AddControllers();

var app = builder.Build();

app.MigrateDatabase<ProductsDbContext>();

app.UseReverseProxyHeaders();
app.UseContexts();

app.MapControllers();
app.MapGet("/", () => "Micro.Examples.Simple.Products");
app.Run();