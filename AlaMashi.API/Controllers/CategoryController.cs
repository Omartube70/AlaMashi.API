using Application.Categories.Commands;
using Application.Categories.Dtos;
using Application.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ISender _mediator;

    public CategoryController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        var CategoryDto = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCategoryById), new { categoryId = CategoryDto.CategoryId }, CategoryDto);
    }

    [HttpGet("{categoryId}")]
    [Authorize]
    public async Task<IActionResult> GetCategoryById(int categoryId)
    {
        var query = new GetCategoryByIdQuery() 
        { 
            CategoryId = categoryId 
        };

        var catgoryDto = await _mediator.Send(query);

        return Ok(new { status = "success", data = catgoryDto });
    }

    [HttpGet("tree")]
    [Authorize]
    public async Task<IActionResult> GetAllCategoriesTree()
    {
        var query = new GetAllCategoriesTreeQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("flat")]
    [Authorize]
    public async Task<IActionResult> GetAllCategoriesFlat()
    {
        var query = new GetAllCategoriesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{categoryId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] UpdateCategoryRequest request)
    {
        var command = new UpdateCategoryCommand()
        {
            CategoryId = categoryId,
            NewCategoryName = request.NewCategoryName,
        };

        await _mediator.Send(command);

        return Ok(new { status = "success", data = "Category Updated successfully" });
    }

    [HttpDelete("{categoryId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        var command = new DeleteCategoryCommand()
        {
            CategoryId = categoryId
        };

        await _mediator.Send(command);

        return Ok(new { status = "success", data = "Category Deleted successfully" });
    }
}