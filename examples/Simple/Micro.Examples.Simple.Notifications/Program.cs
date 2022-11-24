using Micro.Common.ReverseProxy;
using Micro.Common;
using Micro.Contexts;
using Micro.Observability.ApplicationInsights;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMicro(builder.Configuration)
    .AddContexts()
    .AddObservability();
    
var app = builder.Build();

app.UseReverseProxyHeaders();
app.UseContexts();

app.MapGet("/", () => "Micro.Examples.Simple.Notifications");
app.Run();