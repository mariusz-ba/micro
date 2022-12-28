using Micro.CQRS.Abstractions.Commands;
using Micro.CQRS.Abstractions.Queries;
using Micro.Examples.Simple.Products.Commands;
using Micro.Examples.Simple.Products.DTO;
using Micro.Examples.Simple.Products.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Examples.Simple.Products.Controllers;

[Route("[controller]")]
public class ProductsController : BaseController
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public ProductsController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    /// <summary>
    /// Get list of all products.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> BrowseProducts()
        => Ok(await _queryDispatcher.QueryAsync(new BrowseProductsQuery()));

    /// <summary>
    /// Get single product.
    /// </summary>
    [HttpGet("{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProduct(Guid productId)
        => OkOrNotFound(await _queryDispatcher.QueryAsync(new GetProductQuery(productId)));

    /// <summary>
    /// Create new product.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct(ProductDto productDto)
    {
        var command = new CreateProductCommand(Guid.NewGuid(), productDto.Name, productDto.Price);
        await _commandDispatcher.SendAsync(command);
        return CreatedAtAction(nameof(GetProduct), new { productId = command.Id }, null);
    }

    /// <summary>
    /// Update existing product.
    /// </summary>
    [HttpPut("{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(Guid productId, ProductDto productDto)
    {
        var command = new UpdateProductCommand(productId, productDto.Name, productDto.Price);
        await _commandDispatcher.SendAsync(command);
        return Ok();
    }

    /// <summary>
    /// Delete existing product.
    /// </summary>
    [HttpDelete("{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        var command = new DeleteProductCommand(productId);
        await _commandDispatcher.SendAsync(command);
        return NoContent();
    }
}