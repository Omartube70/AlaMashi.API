using Application.Orders.Commands;
using Application.Orders.Dtos;
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

        // ===== CUSTOMER ENDPOINTS =====

        /// <summary>
        /// إنشاء طلب جديد
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var command = new CreateOrderCommand
            {
                CurrentUserId = GetCurrentUserId(),
                AddressId = dto.AddressId,
                DeliveryDate = dto.DeliveryDate,
                DeliveryTimeSlot = dto.DeliveryTimeSlot,
                Items = dto.Items
            };

            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetOrderById), new { orderId = result.OrderId }, new { status = "success", data = result });
        }

        /// <summary>
        /// جلب طلب محدد
        /// </summary>
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var query = new GetOrderByIdQuery
            {
                OrderId = orderId,
                CurrentUserId = GetCurrentUserId(),
                CurrentUserRole = GetCurrentUserRole()
            };

            var result = await _mediator.Send(query);

            return Ok(new { status = "success", data = result });
        }

        /// <summary>
        /// جلب طلبات المستخدم الحالي
        /// </summary>
        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var query = new GetUserOrdersQuery
            {
                UserId = GetCurrentUserId(),
                CurrentUserId = GetCurrentUserId(),
                CurrentUserRole = GetCurrentUserRole()
            };

            var result = await _mediator.Send(query);

            return Ok(new { status = "success", data = result });
        }

        /// <summary>
        /// إلغاء طلب
        /// </summary>
        [HttpPatch("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var command = new UpdateOrderStatusCommand
            {
                OrderId = orderId,
                NewStatus = "Canceled"
            };

            await _mediator.Send(command);

            return Ok(new { status = "success", message = "Order canceled successfully" });
        }

        // ===== ADMIN ENDPOINTS =====

        /// <summary>
        /// جلب كل الطلبات (Admin فقط)
        /// </summary>
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders([FromQuery] string? status = null)
        {
            var query = new GetAllOrdersQuery { Status = status };

            var result = await _mediator.Send(query);

            return Ok(new { status = "success", data = result });
        }

        /// <summary>
        /// جلب طلبات مستخدم محدد (Admin فقط)
        /// </summary>
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserOrders(int userId)
        {
            var query = new GetUserOrdersQuery
            {
                UserId = userId,
                CurrentUserId = GetCurrentUserId(),
                CurrentUserRole = GetCurrentUserRole()
            };

            var result = await _mediator.Send(query);

            return Ok(new { status = "success", data = result });
        }

        /// <summary>
        /// تحديث حالة الطلب (Admin فقط)
        /// </summary>
        [HttpPatch("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto dto)
        {
            var command = new UpdateOrderStatusCommand
            {
                OrderId = orderId,
                NewStatus = dto.NewStatus
            };

            await _mediator.Send(command);

            return Ok(new { status = "success", message = $"Order status updated to {dto.NewStatus}" });
        }

        /// <summary>
        /// تأكيد الطلب (Admin فقط)
        /// </summary>
        [HttpPost("{orderId}/confirm")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            var command = new UpdateOrderStatusCommand
            {
                OrderId = orderId,

                NewStatus = "Confirmed"
            };

            await _mediator.Send(command);
            return Ok(new { status = "success", message = "Order confirmed successfully" });
        }

        /// <summary>
        /// جلب جميع دفعات الطلبات (أو دفعات طلب محدد)
        /// </summary>
        [HttpGet("payments/all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPayments([FromQuery] int? orderId = null)
        {
            var query = new GetAllPaymentsQuery
            {
                OrderId = orderId
            };

            var result = await _mediator.Send(query);

            return Ok(new { status = "success", data = result });
        }

        /// <summary>
        /// جلب دفعات طلب محدد
        /// </summary>
        [HttpGet("{orderId}/payments")]
        public async Task<IActionResult> GetOrderPayments(int orderId)
        {
            var query = new GetAllPaymentsQuery
            {
                OrderId = orderId
            };

            var result = await _mediator.Send(query);

            return Ok(new { status = "success", data = result });
        }

        // ============================================
        // UPDATE ORDER DELIVERY DETAILS ENDPOINT
        // ============================================

        /// <summary>
        /// تحديث تاريخ التوصيل و/أو العنوان للطلب
        /// </summary>
        [HttpPatch("{orderId}/delivery-details")]
        public async Task<IActionResult> UpdateOrderDeliveryDetails(int orderId, [FromBody] UpdateOrderDeliveryDto dto)
        {
            if (dto == null)
                return BadRequest("Request body cannot be empty.");

            var command = new UpdateOrderDeliveryDetailsCommand
            {
                OrderId = orderId,
                NewDeliveryDate = dto.NewDeliveryDate,
                NewDeliveryTimeSlot = dto.NewDeliveryTimeSlot,
                NewAddressId = dto.NewAddressId
            };

            await _mediator.Send(command);

            return Ok(new { status = "success", message = "Order delivery details updated successfully" });
        }


        // --- Helper Methods ---
        private int GetCurrentUserId()
        {
            if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User ID not found or invalid in token.");
        }

        private string GetCurrentUserRole()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (string.IsNullOrEmpty(role))
            {
                throw new UnauthorizedAccessException("User Role not found in token.");
            }
            return role;
        }
    }
}