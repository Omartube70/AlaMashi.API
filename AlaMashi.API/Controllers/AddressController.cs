using Application.Addresses.Commands;
using Application.Addresses.Dtos;
using Application.Addresses.Queries;
using Application.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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

    [HttpGet("all/ByUser")]
    public async Task<IActionResult> GetAllAddressesByUser()
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

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllAddresses()
    {
        var query = new GetAllAddressesQuery();

        var addressDtos = await _mediator.Send(query);

        return Ok(addressDtos);
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


    [HttpPatch("{AddressId}")]
    public async Task<IActionResult> UpdateAddressPartial(int AddressId, [FromBody] JsonPatchDocument<UpdateAddressDto> patchDoc)
    {
        var query = new GetAddressByIdQuery { AddressId = AddressId, CurrentUserId = GetCurrentUserId() };
        var currentAddressState = await _mediator.Send(query);

        var addressToPatch = new UpdateAddressDto
        {
            Street = currentAddressState.Street,
            City = currentAddressState.City,
            AddressDetails = currentAddressState.AddressDetails,
            AddressType = (Domain.Common.AddressType)Enum.Parse(typeof(Domain.Common.AddressType), currentAddressState.AddressType)
        };

        patchDoc.ApplyTo(addressToPatch, ModelState);
        if (!TryValidateModel(addressToPatch))
        {
            return ValidationProblem(ModelState);
        }

        var command = new UpdateAddressCommand
        {
            AddressId = AddressId,
            CurrentUserId = GetCurrentUserId(),
            Street = addressToPatch.Street,
            City = addressToPatch.City,
            AddressDetails = addressToPatch.AddressDetails,
            AddressType = addressToPatch.AddressType
        };

        await _mediator.Send(command);

        return Ok(new { status = "success", data = "Address Updated successfully" });
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