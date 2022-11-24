using Micro.Common;
using Micro.Observability.ApplicationInsights;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMicro(builder.Configuration)
    .AddObservability()
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapReverseProxy();
app.MapGet("/", () => "Micro.Examples.Simple.Gateway");
app.Run();