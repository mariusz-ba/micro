using Micro.API.Swagger;
using Micro.Common;
using Micro.Observability.ApplicationInsights;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMicro(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddObservability()
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseSwaggerDocumentation(builder.Configuration);

app.MapReverseProxy();
app.MapGet("/", () => "Micro.Examples.Simple.Gateway");
app.Run();