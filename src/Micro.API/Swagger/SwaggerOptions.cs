namespace Micro.API.Swagger;

internal class SwaggerOptions
{
    public const string SectionName = "Swagger";
    public SwaggerDocumentOptions Document { get; set; } = new();
    public List<SwaggerEndpointOptions> Endpoints { get; set; } = new();
}

internal class SwaggerDocumentOptions
{
    public string Title { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

internal class SwaggerEndpointOptions
{
    public string Url { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}