using Micro.CQRS;
using Micro.Contexts;
using Micro.Domain;
using Micro.Examples.Web;

var assemblies = AssembliesProvider.GetAssemblies();

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddContexts()
    .AddCQRS(assemblies)
    .AddDomainEvents(assemblies);

var app = builder.Build();

app
    .UseExecutionContext()
    .UseAuthentication()
    .UseAuthorization()
    .UseIdentityContext();

app.MapGet("/", () => "Micro.Examples.Web");
app.Run();
