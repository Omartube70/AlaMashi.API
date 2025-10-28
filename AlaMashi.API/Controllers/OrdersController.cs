using Application.Orders.Commands;
using Application.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AlaMashi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            command.CurrentUserId = GetCurrentUserId();

            var orderDto = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetOrderById), new { orderId = orderDto.OrderId }, orderDto);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var query = new GetOrderByIdQuery
            {
                OrderId = orderId,
                CurrentUserId = GetCurrentUserId(),
                CurrentUserRole = GetCurrentUserRole()
            };
            var orderDto = await _mediator.Send(query);

            return Ok(new { status = "success", data = orderDto });
        }

        [HttpPost("{orderId}/payments")]
        public async Task<IActionResult> CreatePayment(int orderId, [FromBody] CreatePaymentCommand command)
        {
            command.OrderId = orderId;
            command.CurrentUserId = GetCurrentUserId();

            var paymentDto = await _mediator.Send(command);

            return Ok(new { status = "success", data = paymentDto });
        }

        private int GetCurrentUserId()
        {
            if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
                return userId;
            throw new UnauthorizedAccessException("User ID not found in token.");
        }

        private string GetCurrentUserRole()
        {
            return User.FindFirstValue(ClaimTypes.Role)
                ?? throw new UnauthorizedAccessException("User Role not found in token.");
        }
    }
}