using AlaMashi.BLL;
using AlaMashi.DAL; // يجب تضمين DAL لاستخدام UserData
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// DTOs (Data Transfer Objects)
// يمكنك وضعها في ملف منفصل أو كلاس منفصل لتنظيم أفضل
public class CreateUserDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public UserBLL.enPermissions Permissions { get; set; }
}

public class UpdateUserDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public UserBLL.enPermissions Permissions { get; set; }
}

public class ResponseUserDto
{
    public int UserID { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public UserBLL.enPermissions Permissions { get; set; }
}

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserDAL _userDAL;
    private readonly JwtService _jwtService;

    // حقن (Inject) UserDAL و JwtService مباشرةً
    public UsersController(UserDAL userDAL, JwtService jwtService)
    {
        _userDAL = userDAL;
        _jwtService = jwtService;
    }

    // --- CRUD Operations ---

    [HttpGet]
    public ActionResult<IEnumerable<ResponseUserDto>> GetAllUsers()
    {
        var users = UserBLL.GetAllUsers(_userDAL);

        if (users == null || !users.Any())
        {
            return NotFound("No users found.");
        }

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
    public ActionResult<ResponseUserDto> GetUserById(int UserID)
    {
        var user = UserBLL.FindByUserID(_userDAL, UserID);

        if (user == null)
        {
            return NotFound();
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

    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
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
            return BadRequest("Failed to create user.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{UserID}")]
    public IActionResult UpdateUser(int UserID, [FromBody] UpdateUserDto userDto)
    {
        try
        {
            var userToUpdate = UserBLL.FindByUserID(_userDAL, UserID);
            if (userToUpdate == null)
            {
                return NotFound("User not found.");
            }

            userToUpdate.UserName = userDto.UserName;
            userToUpdate.Email = userDto.Email;
            userToUpdate.Phone = userDto.Phone;
            userToUpdate.Password = userDto.Password; // سيتم تشفيره في الـ BLL
            userToUpdate.Permissions = userDto.Permissions;

            if (userToUpdate.Save())
            {
                return Ok("User updated successfully.");
            }
            return BadRequest("Failed to update user.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{UserID}")]
    public IActionResult DeleteUser(int UserID)
    {
        if (!UserBLL.isUserExist(_userDAL, UserID))
        {
            return NotFound("User not found.");
        }

        var success = UserBLL.DeleteUser(_userDAL, UserID);
        if (success)
        {
            return Ok("User deleted successfully.");
        }
        return BadRequest("Could not delete user.");
    }

    // --- Authentication ---

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        var user = UserBLL.GetUserByEmail(_userDAL, model.Email);

        if (user == null || !UserBLL.VerifyPassword(model.Password, user.PasswordHash))
        {
            return Unauthorized("Invalid email or password.");
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
}