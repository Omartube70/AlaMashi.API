using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Users.Commands;
using Application.Users.Queries;
using Application.Users.Dtos;
using Application.Authentication.Commands;
using Application.Authentication.Dtos;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // --- User Management ---

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserCommand command)
    {
        var createdUserDto = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUserById), new { UserID = createdUserDto.UserId }, createdUserDto);
    }

    [HttpGet("{UserID}")]
    [Authorize]
    public async Task<IActionResult> GetUserById(int UserID)
    {
        var query = new GetUserByIdQuery
        {
            UserId = UserID,
            CurrentUserId = GetCurrentUserId(),
            CurrentUserRole = GetCurrentUserRole()
        };
        var userDto = await _mediator.Send(query);

        return Ok(new { status = "success", data = userDto });
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")] 
    public async Task<IActionResult> GetAllUsers()
    {
        var query = new GetAllUsersQuery();
        var users = await _mediator.Send(query);
        return Ok(users);
    }

    [HttpPost("{UserID}/promote-to-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PromoteUserToAdmin(int UserID)
    {
        var command = new PromoteUserToAdminCommand { UserId = UserID };
        await _mediator.Send(command);

        return Ok(new { status = "success", message = $"User {UserID} has been promoted to Admin." });
    }

    [HttpPatch("{userId}")]
    [Authorize]
    public async Task<IActionResult> UpdateUserPartial(int userId, [FromBody] JsonPatchDocument<UpdateUserDto> patchDoc)
    {
        var command = new UpdateUserPartialCommand
        {
            TargetUserId = userId,
            PatchDoc = patchDoc, 
            CurrentUserId = GetCurrentUserId(),
            CurrentUserRole = GetCurrentUserRole()
        };

        var updatedUserDto = await _mediator.Send(command);

        return Ok(new { status = "success", data = updatedUserDto });
    }

    [HttpDelete("{UserID}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(int UserID)
    {
        var command = new DeleteUserCommand
        {
            TargetUserId = UserID,
            CurrentUserId = GetCurrentUserId(),
            CurrentUserRole = GetCurrentUserRole()
        };
        await _mediator.Send(command);

        return Ok(new { status = "success", data = "User deleted successfully" });
    }

    // --- Authentication & Password Management ---

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var LoginResponseDto = await _mediator.Send(command);
        return Ok(new { status = "success", LoginResponseDto});
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(new { status = "success", response });
    }

    [Authorize]
    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDto dto)
    {
        var currentUserId = GetCurrentUserId();
        var currentUserRole = GetCurrentUserRole();

        var command = new RevokeTokenCommand
        {
            TargetUserId = dto.UserId ?? currentUserId,
            CurrentUserId = currentUserId,
            CurrentUserRole = currentUserRole
        };

        await _mediator.Send(command);
        return Ok(new {status = "success", message = "Token revoked successfully." });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var command = new ChangePasswordCommand
        {
            UserId = GetCurrentUserId(),
            OldPassword = dto.OldPassword,
            NewPassword = dto.NewPassword
        };
        await _mediator.Send(command);
        return Ok(new { message = "Password changed successfully." });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "If your email is registered, you will receive a password reset link." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { status = "success", message = "Password has been reset successfully." });
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