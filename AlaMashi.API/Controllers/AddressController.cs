using Application.Addresses.Commands;
using Application.Addresses.Dtos;
using Application.Addresses.Queries;
using Application.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize] // كل العمليات هنا تتطلب تسجيل الدخول
public class AddressesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AddressesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateAddress([FromBody] CreateAddressDto dto)
    {
        var command = new CreateAddressCommand
        {
            CurrentUserId = GetCurrentUserId(),
            Street = dto.Street,
            City = dto.City,
            AddressDetails = dto.AddressDetails,
            AddressType = dto.AddressType
        };
        AddressDto addressDto = await _mediator.Send(command);
        return Ok(new { status = "success", data = addressDto });
    }

    [HttpGet("all")] // يجلب عناوين المستخدم الحالي
    public async Task<IActionResult> GetMyAddresses()
    {
        var query = new GetUserAddressesQuery
        {
            UserId = GetCurrentUserId(),
            CurrentUserId = GetCurrentUserId(),
            CurrentUserRole = GetCurrentUserRole()
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpDelete("{AddressId}")]
    public async Task<IActionResult> DeleteAddress(int AddressId)
    {
        var command = new DeleteAddressCommand
        {
            AddressId = AddressId,
            CurrentUserId = GetCurrentUserId(),
        };
        await _mediator.Send(command);
        return Ok(new { status = "success", data = "Address deleted successfully" });
    }

    // --- Helper Methods ---
    private int GetCurrentUserId()
    {
        if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
        {
            return userId;
        }
        throw new InvalidOperationException("User ID not found in token.");
    }

    private string GetCurrentUserRole()
    {
        return User.FindFirstValue(ClaimTypes.Role) ??
               throw new InvalidOperationException("User Role not found in token.");
    }
}