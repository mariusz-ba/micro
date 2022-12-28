using Microsoft.AspNetCore.Mvc;

namespace Micro.Examples.Simple.Products.Controllers;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public abstract class BaseController : ControllerBase
{
    protected ActionResult<T> OkOrNotFound<T>(T? value) => value is null ? NotFound() : Ok(value);
}