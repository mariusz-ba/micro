using System.ComponentModel.DataAnnotations;

namespace Micro.Examples.Simple.Products.DTO;

public record ProductDto(Guid Id, [StringLength(20, MinimumLength = 3)] string Name, decimal Price);