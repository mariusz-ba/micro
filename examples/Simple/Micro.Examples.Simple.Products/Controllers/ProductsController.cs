using Micro.CQRS.Abstractions.Commands;
using Micro.CQRS.Abstractions.Queries;
using Micro.Examples.Simple.Products.Commands;
using Micro.Examples.Simple.Products.DTO;
using Micro.Examples.Simple.Products.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Examples.Simple.Products.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public ProductsController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> BrowseProducts()
        => Ok(await _queryDispatcher.QueryAsync(new BrowseProductsQuery()));

    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<ProductDto>> GetProduct(Guid productId)
        => OkOrNotFound(await _queryDispatcher.QueryAsync(new GetProductQuery(productId)));

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductDto productDto)
    {
        var command = new CreateProductCommand(Guid.NewGuid(), productDto.Name, productDto.Price);
        await _commandDispatcher.SendAsync(command);
        return CreatedAtAction(nameof(GetProduct), new { productId = command.Id }, null);
    }

    [HttpPut("{productId:guid}")]
    public async Task<IActionResult> UpdateProduct(Guid productId, ProductDto productDto)
    {
        var command = new UpdateProductCommand(productId, productDto.Name, productDto.Price);
        await _commandDispatcher.SendAsync(command);
        return Ok();
    }

    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        var command = new DeleteProductCommand(productId);
        await _commandDispatcher.SendAsync(command);
        return NoContent();
    }

    private ActionResult<T> OkOrNotFound<T>(T? value) => value is null ? NotFound() : Ok(value);
}