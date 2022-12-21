using Micro.API.Exceptions;
using Micro.API.Networking;
using Micro.API.Swagger;
using Micro.CQRS;
using Micro.Common;
using Micro.Contexts;
using Micro.Domain;
using Micro.Examples.Simple.Products.Persistence;
using Micro.Examples.Simple.Products;
using Micro.Hangfire.Messaging.Outbox;
using Micro.Hangfire;
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
    .AddExceptionsHandling(assemblies)
    .AddCQRS(assemblies)
    .AddDomainEvents(assemblies)
    .AddPersistence<ProductsDbContext>(builder.Configuration)
    .AddHangfire<ProductsDbContext>(builder.Configuration)
    .AddHangfireMessagingOutbox()
    .AddObservability()
    .AddRouting(options => options.LowercaseUrls = true)
    .AddControllers();

var app = builder.Build();

app.UseExceptionsHandling();
app.UseHeadersForwarding();
app.UseSwagger();
app.UseContexts();

app.MapControllers();
app.MapGet("/", () => "Micro.Examples.Simple.Products");
app.Run();