using AlaMashi.BLL;
using AlaMashi.DAL;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// DTOs (Data Transfer Objects)
public class CreateUserDto
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string? Phone { get; set; }

    [Required]
    public string Password { get; set; }
    public UserBLL.enPermissions Permissions { get; set; }
}

public class UpdateUserDto
{
    public string ?UserName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Password { get; set; }
    public UserBLL.enPermissions Permissions { get; set; }
}

public class ResponseUserDto
{
    public int UserID { get; set; }
    public string UserName { get; set; }

    [EmailAddress]
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Permissions { get; set; }
}


public class LoginModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

public class ForgotPasswordModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}

public class ResetPasswordModel
{
    [Required]
    public string Token { get; set; }

    [Required]
    public string NewPassword { get; set; }
}
public class RefreshTokenRequestDto
{
    [Required]
    public string RefreshToken { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly EmailService _emailService;

    public UsersController(JwtService jwtService, EmailService emailService)
    {
        _jwtService = jwtService;
        _emailService = emailService;
    }

    // --- CRUD Operations ---

    /// <summary>
    /// يرسل رابط لإعادة تعيين كلمة المرور إلى البريد الإلكتروني الخاص بالمستخدم.
    /// </summary>
    /// <param name="model">البريد الإلكتروني للمستخدم.</param>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
    {

        if (!await ValidationHelper.IsEmailValidAsync(model.Email))
        {
            throw new ArgumentException("Invalid Email Format.");
        }

        var user = UserBLL.GetUserByEmail(model.Email);

        // لمنع تسريب معلومات المستخدمين، نرسل ردًا عامًا حتى لو لم يتم العثور على المستخدم
        if (user == null)
        {
            return Ok(new { message = "If your email is registered, you will receive a password reset link." });
        }

        // إنشاء رمز فريد قصير الأجل (يمكن أن يكون JWT أو أي رمز آخر)
        var resetToken = _jwtService.GenerateResetToken(user.UserID, user.Email);

        // بناء الرابط الذي سيتم إرساله عبر البريد الإلكتروني
        var resetLink = $"https://your-app.com/reset-password?token={Uri.EscapeDataString(resetToken)}";

        // إرسال البريد الإلكتروني
        await _emailService.SendPasswordResetEmailAsync(user.UserName,model.Email, resetLink);

        return Ok(new { message = "If your email is registered, you will receive a password reset link." });
    }

    /// <summary>
    /// يعيد تعيين كلمة مرور المستخدم باستخدام الرمز المرسل.
    /// </summary>
    /// <param name="model">الرمز الجديد وكلمة المرور الجديدة.</param>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        // التحقق من صلاحية الرمز
        var principal = _jwtService.ValidateResetToken(model.Token);
        if (principal == null)
        {
            throw new ArgumentException("Invalid or expired token.");
        }

        var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        if (emailClaim == null)
        {
            throw new ArgumentException("Invalid token claims.");
        }

        var user = UserBLL.GetUserByEmail(emailClaim.Value);
        if (user == null)
        {
            return Ok(new { message = "Password has been reset successfully." });
        }

        // تحديث كلمة المرور وحفظ التغييرات
        user.Password = model.NewPassword;
        if (await user.SaveAsync())
        {
            return Ok(new { message = "Password has been reset successfully." });
        }

        throw new Exception("An error occurred while resetting password.");
    }



    [HttpGet("All")]
    [Authorize(Roles = "Admin")]
    public ActionResult<IEnumerable<ResponseUserDto>> GetAllUsers()
    {
        var users = UserBLL.GetAllUsers();

        var usersDto = users.Select(MapToResponseDto).ToList();

        return Ok(usersDto);
    }


    [HttpGet("{UserID}")]
    [Authorize]
    public ActionResult<ResponseUserDto> GetUserById(int UserID)
    {
        var (userIdFromToken, userRoleFromToken) = GetCurrentUser();
        if (UserID != userIdFromToken && userRoleFromToken != "Admin")
        {
            return Forbid();
        }

        var user = UserBLL.GetUserByUserID(UserID);

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {UserID} was not found.");
        }

        var userDto = MapToResponseDto(user);

        return Ok(new { status = "success", data = userDto });
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto userDto)
    {
            var newUserBLL = new UserBLL()
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Phone = userDto.Phone,
                Password = userDto.Password,
                Permissions = userDto.Permissions
            };

            if (await newUserBLL.SaveAsync())
            {
                var responseDto = MapToResponseDto(newUserBLL);

                return CreatedAtAction(nameof(GetUserById), new { UserID = newUserBLL.UserID }, responseDto);
            }
                throw new Exception("An error occurred while creating the user.");
    }

    [HttpPatch("{UserID}")]
    [Authorize]
    public async Task<IActionResult> UpdateUserAsync(int UserID, [FromBody] JsonPatchDocument<UpdateUserDto> patchDoc)
    {
        var (userIdFromToken, userRoleFromToken) = GetCurrentUser();

        // 2. التحقق من الصلاحيات (Authorization)
        // امنع الطلب فقط إذا كان المستخدم ليس مديراً ويحاول تحديث بيانات مستخدم آخر
        if (UserID != userIdFromToken && userRoleFromToken != "Admin")
        {
            return Forbid(); // يرجع 403 Forbidden
        }


         var userToUpdate = UserBLL.GetUserByUserID(UserID);
         if (userToUpdate == null)
            {
                throw new KeyNotFoundException($"User with ID {UserID} was not found.");
            }

        var userToPatchDto = new UpdateUserDto
        {
            UserName = userToUpdate.UserName,
            Email = userToUpdate.Email,
            Phone = userToUpdate.Phone,
            Permissions = userToUpdate.Permissions,
            // لا نضع كلمة المرور هنا
        };

        patchDoc.ApplyTo(userToPatchDto, this.ModelState);
        

        if (!TryValidateModel(userToPatchDto))
        {
            return ValidationProblem(ModelState);
        }

        // الخطوة د: نقل القيم المحدثة من الـ DTO إلى الـ Entity الأصلية
        userToUpdate.UserName = userToPatchDto.UserName;
        userToUpdate.Email = userToPatchDto.Email;
        userToUpdate.Phone = userToPatchDto.Phone;
        userToUpdate.Permissions = userToPatchDto.Permissions;

        // تحديث كلمة المرور فقط إذا تم إرسالها في الطلب
        if (!string.IsNullOrEmpty(userToPatchDto.Password))
        {
            userToUpdate.Password = userToPatchDto.Password; // سيتم تشفيره في الـ BLL
        }


        if (await userToUpdate.SaveAsync())
         {
            return Ok(new { status = "success", data = userToPatchDto });
        }

        throw new Exception("An error occurred while updating the user.");
    }


    [HttpDelete("{UserID}")]
    [Authorize]
    public IActionResult DeleteUser(int UserID)
    {
        // استخراج البيانات من الـ Token
        var (userIdFromToken, userRoleFromToken) = GetCurrentUser();

        if (UserID != userIdFromToken && userRoleFromToken != "Admin")
        {
            // إذا كان المستخدم ليس مديراً ويحاول حذف مستخدم آخر، امنعه
            return Forbid(); // يرجع 403 Forbidden
        }


        if (!UserBLL.isUserExist(UserID))
        {
            throw new KeyNotFoundException($"User with ID {UserID} was not found.");
        }

        if (UserBLL.DeleteUser(UserID))
        {
            return Ok(new { status = "success", data = "User deleted successfully" });
        }

        throw new Exception("An error occurred while deleting the user.");
    }

    // --- Authentication ---

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        var user = UserBLL.GetUserByEmail(model.Email);

        if (user == null || !UserBLL.VerifyPassword(model.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var accessToken = _jwtService.GenerateToken(user.UserID, user.UserName, user.Permissions);
        var (refreshToken, refreshTokenExpiry) = _jwtService.GenerateRefreshToken();
        UserBLL.SaveRefreshToken(user.UserID, refreshToken, refreshTokenExpiry);

        var responseDto = MapToResponseDto(user);


        return Ok(new
        {
            status = "success",
            data = new
            {
                accessToken,
                refreshToken,
                user = responseDto
            }
        });
    }


    [HttpPost("refresh")]
    public IActionResult RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            throw new ArgumentException("Refresh token is required.");
        }

        var user = UserBLL.GetUserByRefreshToken(request.RefreshToken);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        var newAccessToken = _jwtService.GenerateToken(user.UserID, user.UserName, user.Permissions);
        var (newRefreshToken, newRefreshTokenExpiry) = _jwtService.GenerateRefreshToken();
        UserBLL.SaveRefreshToken(user.UserID, newRefreshToken, newRefreshTokenExpiry);

        return Ok(new
        {
            status = "success",
            data = new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            }
        });
    }


    [Authorize]
    [HttpPost("revoke")]
    public IActionResult RevokeToken()
    {
        var (userId, _) = GetCurrentUser();
        UserBLL.SaveRefreshToken(userId, null, DateTime.UtcNow.AddYears(-1));

        return Ok(new { status = "success", data = "Token revoked successfully." });
    }

    // دالة مساعدة لتجنب تكرار الكود
    private (int, string) GetCurrentUser()
    {
        var userIdString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            // هذا الخطأ سيتم التقاطه بواسطة الـ Middleware كـ 500 Internal Server Error
            // لأنه خطأ غير متوقع يدل على وجود مشكلة في الـ Token نفسه
            throw new UnauthorizedAccessException("Invalid Token: UserID is missing or invalid.");
        }

        return (userId, userRole);
    }

    private ResponseUserDto MapToResponseDto(UserBLL bll)
    {
       return  new ResponseUserDto
        {
            UserID   = bll.UserID,
            UserName = bll.UserName,
            Email    = bll.Email,
            Phone    = bll.Phone,
            Permissions = bll.Permissions.ToString()
        };
    }
}