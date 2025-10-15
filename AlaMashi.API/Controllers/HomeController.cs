using Application.Home.Dtos;
using Application.Home.Queries;
using Application.Home.Queries.Application.Home.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<HomeDataDto>> GetHomeData()
    {
        var result = await _mediator.Send(new GetHomeDataQuery());
        return Ok(new { status = "success", data = result });
    }
}
