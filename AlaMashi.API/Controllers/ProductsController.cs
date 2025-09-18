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

    // GET: api/products
    [HttpGet("all")]
    public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsQuery query)
    {
        var productDtos = await _mediator.Send(query);
        return Ok(productDtos);
    }

    // GET: api/products/category/{categoryId}
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetProductsByCategory(int categoryId)
    {
        var query = new GetAllProductsByCategoryQuery() { CategoryId = categoryId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // PUT: api/products/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct(int productId, [FromBody] UpdateProductDto Dto)
    {
        var command = new UpdateProductCommand
        {
            ProductID = productId,
            ProductName = Dto.ProductName,
            ProductDescription = Dto.ProductDescription,
            Price = Dto.Price
        };

        await _mediator.Send(command);

        return Ok(new { status = "success", data = "Product Updated successfully" });
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
}

