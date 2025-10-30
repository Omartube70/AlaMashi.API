using Application.Products.Commands;
using Application.Products.Dtos;
using Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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

    // GET: api/products
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

    // PUT: api/products/{id}
    [HttpPatch("{productId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProductPartial(int productId,[FromBody] JsonPatchDocument<UpdateProductDto> patchDoc)
    {
        var command = new UpdateProductPartialCommand
        {
            ProductId = productId,
            PatchDoc = patchDoc
        };

        await _mediator.Send(command);

        return Ok(new { status = "success", data = "Product updated successfully" });
    }

    // DELETE: api/products/{id}
    [HttpDelete("{productId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        var command = new DeleteProductCommand() 
        {
            ProductID = productId 
        };

         await _mediator.Send(command);

        return Ok(new { status = "success", data = "Product deleted successfully" });
    }

    /// <summary>
    /// يربط منتج بعرض (اشتراك المنتج في العرض)
    /// </summary>
    [HttpPost("subscribe-to-offer")]
    public async Task<IActionResult> SubscribeToOffer([FromBody] SubscribeProductToOfferCommand command)
    {
        await _mediator.Send(command);

        return Ok(new { status = "success", data = "✅ Product subscribed to offer successfully" });
    }
}

