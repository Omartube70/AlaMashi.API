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
            CurrentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
            CurrentUserRole = User.FindFirstValue(ClaimTypes.Role)
        };
        var userDto = await _mediator.Send(query);

        return Ok(new { status = "success", data = userDto });
    }

    [HttpGet("all")]
    [Authorize] // Authorization is now handled inside the handler
    public async Task<IActionResult> GetAllUsers()
    {
        var query = new GetAllUsersQuery
        {
            CurrentUserRole = User.FindFirstValue(ClaimTypes.Role)
        };
        var users = await _mediator.Send(query);
        return Ok(users);
    }

    [HttpPatch("{UserID}")]
    [Authorize]
    public async Task<IActionResult> UpdateUserPartial(int UserID, [FromBody] JsonPatchDocument<UpdateUserDto> patchDoc)
    {
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

        var currentUserState = await _mediator.Send(new GetUserByIdQuery
        {
            UserId = UserID,
            CurrentUserId = currentUserId,
            CurrentUserRole = currentUserRole
        });

        var userToPatch = new UpdateUserDto
        {
            UserName = currentUserState.UserName,
            Email = currentUserState.Email,
            Phone = currentUserState.Phone
        };

        // 3. تطبيق التعديلات على كائن UpdateUserDto الجديد
        patchDoc.ApplyTo(userToPatch, ModelState);

        // 4. التحقق من صحة النموذج بعد التعديل
        if (!TryValidateModel(userToPatch))
        {
            return ValidationProblem(ModelState);
        }

        // 5. إنشاء وإرسال الـ Command بالبيانات المحدثة
        var command = new UpdateUserCommand
        {
            TargetUserId = UserID,
            CurrentUserId = currentUserId,
            CurrentUserRole = currentUserRole,
            UserName = userToPatch.UserName,
            Email = userToPatch.Email,
            Phone = userToPatch.Phone
        };

        await _mediator.Send(command);

        return Ok(new { status = "success", data = userToPatch });
    }

    [HttpDelete("{UserID}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(int UserID)
    {
        var command = new DeleteUserCommand
        {
            TargetUserId = UserID,
            CurrentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
            CurrentUserRole = User.FindFirstValue(ClaimTypes.Role)
        };
        await _mediator.Send(command);

        return Ok(new { status = "success", data = "User deleted successfully" });
    }

    // --- Authentication & Password Management ---

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(new { status = "success", response });
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
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

        var command = new RevokeTokenCommand
        {
            // إذا قام الأدمن بإرسال ID، استخدمه. وإلا، استخدم ID المستخدم الحالي.
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
            UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
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
        return Ok(new { message = "Password has been reset successfully." });
    }
}