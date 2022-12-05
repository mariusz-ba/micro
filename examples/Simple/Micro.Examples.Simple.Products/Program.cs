using Micro.API.Networking;
using Micro.API.Swagger;
using Micro.BackgroundJobs.SqlServer;
using Micro.CQRS;
using Micro.Common;
using Micro.Contexts;
using Micro.Domain;
using Micro.Examples.Simple.Products.Persistence;
using Micro.Examples.Simple.Products;
using Micro.Messaging.Abstractions;
using Micro.Messaging.RabbitMQ;
using Micro.Observability.ApplicationInsights;
using Micro.Persistence.SqlServer;

var assemblies = AssembliesProvider.GetAssemblies();

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMicro(builder.Configuration)
    .AddContexts()
    .AddMessaging()
    .AddRabbitMq(builder.Configuration)
    .AddHeadersForwarding(builder.Configuration)
    .AddSwaggerDocumentation(builder.Configuration)
    .AddCQRS(assemblies)
    .AddDomainEvents(assemblies)
    .AddPersistence<ProductsDbContext>(builder.Configuration)
    .AddBackgroundJobs<ProductsDbContext>(builder.Configuration.GetSection("BackgroundJobs:MessageOutbox"))
    .AddObservability()
    .AddRouting(options => options.LowercaseUrls = true)
    .AddControllers();

var app = builder.Build();

app.MigrateDatabase<ProductsDbContext>();

app.UseHeadersForwarding();
app.UseSwagger();
app.UseContexts();

app.MapControllers();
app.MapGet("/", () => "Micro.Examples.Simple.Products");
app.Run();