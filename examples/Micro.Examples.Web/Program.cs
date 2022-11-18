using Micro.CQRS;
using Micro.Domain;
using Micro.Examples.Web;

var assemblies = AssembliesProvider.GetAssemblies();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCQRS(assemblies);
builder.Services.AddDomainEvents(assemblies);

var app = builder.Build();
app.MapGet("/", () => "Micro.Examples.Web");

app.Run();