using AlaMashi.BLL;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace YourApp.API.Controllers
{
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
        public IActionResult GetAll()
        {
            var users = UserBLL.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = UserBLL.FindByUserID(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] UserBLL user)
        {
            if (user.Save())
                return Ok(user);
            else
                return BadRequest("Failed to save user.");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserBLL user)
        {
            if(id != -1)
              user.UserID = id;

            if (user.Save())
                return Ok(user);
            else
                return BadRequest("Failed to update user.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = UserBLL.FindByUserID(id);
            if (user == null)
                return NotFound();
            
            if (user.DeleteUser())
                return Ok("Deleted.");
            else
                return BadRequest("Failed to delete user.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = UserBLL.GetUserByEmail(model.Email); // هتعمل ميثود دي فـ BLL

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                return Unauthorized("Invalid email or password.");

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
