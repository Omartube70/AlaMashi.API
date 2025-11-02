using Application.Products.Commands;
using Application.Products.Dtos;
using Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly ISender _mediator;

    public ProductsController(ISender mediator)
    {
        _mediator = mediator;
    }

    // POST: api/products/Create
    [HttpPost("Create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProduct([FromForm] CreateProductCommand command)
    {
        var createdProductDto = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProductById), new { productId = createdProductDto.ProductID }, createdProductDto);
    }

    // GET: api/products/{productId}
    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductById(int productId)
    {
        var query = new GetProductByIdQuery()
        {
            ProductId = productId
        };
        var productDto = await _mediator.Send(query);

        return Ok(new { status = "success", data = productDto });
    }

    // GET: api/products/all
    [HttpGet("all")]
    public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsQuery query)
    {
        var productDtos = await _mediator.Send(query);
        return Ok(new { status = "success", data = productDtos });
    }

    // GET: api/products/category/{categoryId}
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetProductsByCategory(int categoryId)
    {
        var query = new GetAllProductsByCategoryQuery() { CategoryId = categoryId };
        var productDtos = await _mediator.Send(query);
        return Ok(new { status = "success", data = productDtos });
    }

    // PATCH: api/products/{productId}
    [HttpPatch("{productId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct(int productId, [FromForm] UpdateProductPartialCommand command)
    {
        command.ProductId = productId;
        await _mediator.Send(command);

        return Ok(new { status = "success", message = "Product updated successfully" });
    }

    // DELETE: api/products/{productId}
    [HttpDelete("{productId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        var command = new DeleteProductCommand()
        {
            ProductID = productId
        };

        await _mediator.Send(command);

        return Ok(new { status = "success", message = "Product deleted successfully" });
    }

    // POST: api/products/subscribe-to-offer
    [HttpPost("subscribe-to-offer")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SubscribeToOffer([FromBody] SubscribeProductToOfferCommand command)
    {
        await _mediator.Send(command);

        return Ok(new { status = "success", message = "✅ Product subscribed to offer successfully" });
    }
}