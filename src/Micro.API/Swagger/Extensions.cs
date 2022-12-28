using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Micro.API.Swagger;

public static class Extensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
    {
        var swaggerOptions = configuration.GetSection(SwaggerOptions.SectionName).Get<SwaggerOptions>();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(swaggerOptions.Document.Version, new OpenApiInfo
            {
                Title = swaggerOptions.Document.Title,
                Version = swaggerOptions.Document.Version,
                Description = swaggerOptions.Document.Description
            });

            options.ConfigureDocumentationComments();
            options.SupportNonNullableReferenceTypes();
        });

        return services;
    }
    
    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IConfiguration configuration)
    {
        var swaggerOptions = configuration.GetSection(SwaggerOptions.SectionName).Get<SwaggerOptions>();
        
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var endpoint in swaggerOptions.Endpoints)
            {
                options.SwaggerEndpoint(endpoint.Url, endpoint.Name);
            }
        });

        return app;
    }
    
    private static void ConfigureDocumentationComments(this SwaggerGenOptions options)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var fileName = Assembly.GetEntryAssembly()!.GetName().Name + ".xml";
        var filePath = Path.Combine(basePath, fileName);
        options.IncludeXmlComments(filePath);
    }
}