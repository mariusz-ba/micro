using Micro.CQRS.Abstractions.Queries;
using Micro.Examples.Simple.Products.DTO;

namespace Micro.Examples.Simple.Products.Queries;

public record GetProductQuery(Guid Id) : IQuery<ProductDto?>;