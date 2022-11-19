using Micro.CQRS;
using Micro.Common;
using Micro.Contexts;
using Micro.Domain;
using Micro.Examples.Web;
using Micro.Observability.ApplicationInsights;

var assemblies = AssembliesProvider.GetAssemblies();

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMicro(builder.Configuration)
    .AddContexts()
    .AddCQRS(assemblies)
    .AddDomainEvents(assemblies)
    .AddObservability();

var app = builder.Build();

app
    .UseExecutionContext()
    .UseAuthentication()
    .UseAuthorization()
    .UseIdentityContext();

app.MapGet("/", () => "Micro.Examples.Web");
app.Run();
