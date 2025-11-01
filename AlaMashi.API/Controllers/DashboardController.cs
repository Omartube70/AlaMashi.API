using Application.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/Dashboard/summary
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
        {
            var summaryDto = await _mediator.Send(new GetEnhancedDashboardSummaryQuery(), cancellationToken);

            return Ok(new { status = "success"
                , data = summaryDto });

        }
    }
}