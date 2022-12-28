using Micro.API.Exceptions;
using Micro.API.Networking;
using Micro.API.Swagger;
using Micro.Common;
using Micro.Contexts;
using Micro.Messaging.Abstractions;
using Micro.Messaging.RabbitMQ;
using Micro.Observability.ApplicationInsights;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMicro(builder.Configuration)
    .AddContexts()
    .AddMessaging()
    .AddRabbitMq(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddHeadersForwarding(builder.Configuration)
    .AddSwaggerDocumentation(builder.Configuration)
    .AddExceptionsHandling()
    .AddObservability()
    .AddRouting(options => options.LowercaseUrls = true);
    
var app = builder.Build();

app.UseExceptionsHandling();
app.UseHeadersForwarding();
app.UseSwagger();
app.UseContexts();

app.MapGet("/", () => "Micro.Examples.Simple.Notifications").ExcludeFromDescription();
app.Run();