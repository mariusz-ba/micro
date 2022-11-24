var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/", () => "Micro.Examples.Simple.Products");
app.Run();