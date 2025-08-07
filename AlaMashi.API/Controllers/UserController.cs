using AlaMashi.BLL;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq; // يجب إضافة هذه المكتبة

namespace YourApp.API.Controllers
{
    // DTO لإنشاء مستخدم جديد
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public UserBLL.enPermissions Permissions { get; set; } = UserBLL.enPermissions.User;
    }

    // DTO لتحديث بيانات مستخدم موجود
    public class UpdateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public UserBLL.enPermissions Permissions { get; set; }
    }

    // DTO للاستجابة (يخفي البيانات الحساسة)
    public class ResponseUserDto
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public UserBLL.enPermissions Permissions { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public UsersController(IConfiguration config)
        {
            string secretKey = config["Jwt:Key"];
            string issuer = config["Jwt:Issuer"];
            _jwtService = new JwtService(secretKey, issuer);
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = UserBLL.GetAllUsers();

            // تحويل كل كائن BLL إلى DTO للاستجابة
            var usersDto = users.Select(user => new ResponseUserDto
            {
                UserID = user.UserID,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                Permissions = (UserBLL.enPermissions)user.Permissions
            }).ToList();

            return Ok(usersDto);
        }

        [HttpGet("{UserID}")]
        public IActionResult GetUserById(int UserID)
        {
            var user = UserBLL.FindByUserID(UserID);

            if (user == null)
            {
                return NotFound();
            }

            // تحويل كائن BLL إلى DTO للاستجابة
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

            var user = new UserBLL();
            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.Phone = userDto.Phone;
            user.Password = userDto.Password;
            user.Permissions = userDto.Permissions;

            try
            {
                if (user.Save())
                {
                    // تحويل كائن BLL إلى DTO قبل إرجاعه
                    var responseDto = new ResponseUserDto
                    {
                        UserID = user.UserID,
                        UserName = user.UserName,
                        Email = user.Email,
                        Phone = user.Phone,
                        Permissions = user.Permissions
                    };
                    return CreatedAtAction(nameof(GetUserById), new { UserID = user.UserID }, responseDto);
                }
                else
                {
                    return BadRequest("Failed to save user.");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{UserID}")]
        public IActionResult UpdateUser(int UserID, [FromBody] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = UserBLL.FindByUserID(UserID);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.Phone = userDto.Phone;
            user.Password = userDto.Password;
            user.Permissions = userDto.Permissions;

            try
            {
                if (user.Save())
                {
                    // تحويل كائن BLL إلى DTO قبل إرجاعه
                    var responseDto = new ResponseUserDto
                    {
                        UserID = user.UserID,
                        UserName = user.UserName,
                        Email = user.Email,
                        Phone = user.Phone,
                        Permissions = (UserBLL.enPermissions)user.Permissions
                    };
                    return Ok(responseDto);
                }
                else
                {
                    return BadRequest("Failed to update user.");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{UserID}")]
        public IActionResult DeleteUser(int UserID)
        {
            if (!UserBLL.isUserExist(UserID))
            {
                return NotFound();
            }

            if (UserBLL.DeleteUser(UserID))
            {
                return Ok(new { message = "User deleted successfully." });
            }
            else
            {
                return BadRequest("Failed to delete user.");
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = UserBLL.GetUserByEmail(model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _jwtService.GenerateToken(user.UserID, user.UserName, user.Permissions);

            return Ok(new
            {
                token,
                user = new
                {
                    user.UserID,
                    user.UserName,
                    user.Email,
                    user.Permissions
                }
            });
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}