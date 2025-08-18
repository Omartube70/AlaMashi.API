using AlaMashi.BLL;
using AlaMashi.DAL;
using AlaMashi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;

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
    public UserBLL.enPermissions Permissions { get; set; }
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


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserDAL _userDAL;
    private readonly JwtService _jwtService;
    private readonly EmailService _emailService;

    public UsersController(UserDAL userDAL, JwtService jwtService, EmailService emailService)
    {
        _userDAL = userDAL;
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
        var user = UserBLL.GetUserByEmail(_userDAL, model.Email);

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
        await _emailService.SendPasswordResetEmailAsync(model.Email, resetLink);

        return Ok(new { message = "If your email is registered, you will receive a password reset link." });
    }

    /// <summary>
    /// يعيد تعيين كلمة مرور المستخدم باستخدام الرمز المرسل.
    /// </summary>
    /// <param name="model">الرمز الجديد وكلمة المرور الجديدة.</param>
    [HttpPost("reset-password")]
    public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
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

        var user = UserBLL.GetUserByEmail(_userDAL, emailClaim.Value);
        if (user == null)
        {
            return Ok(new { message = "Password has been reset successfully." });
        }

        // تحديث كلمة المرور وحفظ التغييرات
        user.Password = model.NewPassword;
        if (user.Save())
        {
            return Ok(new { message = "Password has been reset successfully." });
        }

        throw new Exception("An error occurred while resetting password.");
    }



    [HttpGet("All")]
    [Authorize(Roles = "Admin")]
    public ActionResult<IEnumerable<ResponseUserDto>> GetAllUsers()
    {
        var users = UserBLL.GetAllUsers(_userDAL);

        var usersDto = users.Select(user => new ResponseUserDto
        {
            UserID = user.UserID,
            UserName = user.UserName,
            Email = user.Email,
            Phone = user.Phone,
            Permissions = user.Permissions
        }).ToList();

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

        var user = UserBLL.FindByUserID(_userDAL, UserID);

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {UserID} was not found.");
        }

        var userDto = new ResponseUserDto
        {
            UserID = user.UserID,
            UserName = user.UserName,
            Email = user.Email,
            Phone = user.Phone,
            Permissions = user.Permissions
        };

        return Ok(userDto);
    }

    [HttpPost("Create")]
    public IActionResult CreateUser([FromBody] CreateUserDto userDto)
    {
            var newUserBLL = new UserBLL(_userDAL)
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Phone = userDto.Phone,
                Password = userDto.Password,
                Permissions = userDto.Permissions
            };

            if (newUserBLL.Save())
            {
                var responseDto = new ResponseUserDto
                {
                    UserID = newUserBLL.UserID,
                    UserName = newUserBLL.UserName,
                    Email = newUserBLL.Email,
                    Phone = newUserBLL.Phone,
                    Permissions = newUserBLL.Permissions
                };

                return CreatedAtAction(nameof(GetUserById), new { UserID = newUserBLL.UserID }, responseDto);
            }
                throw new Exception("An error occurred while creating the user.");
    }

    [HttpPatch("{UserID}")]
    [Authorize]
    public IActionResult UpdateUser(int UserID, [FromBody] JsonPatchDocument<UpdateUserDto> patchDoc)
    {
        var (userIdFromToken, userRoleFromToken) = GetCurrentUser();

        // 2. التحقق من الصلاحيات (Authorization)
        // امنع الطلب فقط إذا كان المستخدم ليس مديراً ويحاول تحديث بيانات مستخدم آخر
        if (UserID != userIdFromToken && userRoleFromToken != "Admin")
        {
            return Forbid(); // يرجع 403 Forbidden
        }


         var userToUpdate = UserBLL.FindByUserID(_userDAL, UserID);
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


        if (userToUpdate.Save())
           {
             return NoContent();
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


        if (!UserBLL.isUserExist(_userDAL, UserID))
        {
            throw new KeyNotFoundException($"User with ID {UserID} was not found.");
        }

        if (UserBLL.DeleteUser(_userDAL, UserID))
        {
            return NoContent();
        }

        throw new Exception("An error occurred while deleting the user.");
    }

    // --- Authentication ---

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        var user = UserBLL.GetUserByEmail(_userDAL, model.Email);

        if (user == null || !UserBLL.VerifyPassword(model.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = _jwtService.GenerateToken(user.UserID, user.UserName, user.Permissions);

        var responseDto = new ResponseUserDto
        {
            UserID = user.UserID,
            UserName = user.UserName,
            Email = user.Email,
            Phone = user.Phone,
            Permissions = user.Permissions
        };

        return Ok(new
        {
            token,
            user = responseDto
        });
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
}