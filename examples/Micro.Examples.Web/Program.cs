using Micro.CQRS;
using Micro.Examples.Web;

var assemblies = AssembliesProvider.GetAssemblies();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCQRS(assemblies);

var app = builder.Build();
app.MapGet("/", () => "Micro.Examples.Web");

app.Run();