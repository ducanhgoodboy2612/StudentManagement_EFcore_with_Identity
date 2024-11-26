using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManageApp_Codef.Service;
using StudentManageApp_Codef.Data.R_IRepository;
namespace StudentManageApp_Codef.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _res;
        private readonly UserService _userService;
        private readonly string _path;
        private readonly IWebHostEnvironment _env;

        public UserController(IUserRepository res, UserService userService, IConfiguration configuration, IWebHostEnvironment env)
        {
            _res = res;
            _userService = userService;
            _path = configuration["AppSettings:PATH"];
            _env = env;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string email = null;
                if (formData.ContainsKey("email") && formData["email"] != null)
                {
                    email = formData["email"].ToString();
                }

                string password = null;
                if (formData.ContainsKey("password") && formData["password"] != null)
                {
                    password = formData["password"].ToString();
                }

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    return BadRequest("Email and password are required.");
                }

                var user = _userService.Login(email, password);

                if (user == null)
                    return Unauthorized(new { message = "Username or password is incorrect" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    details = ex.StackTrace // Chi tiết stack trace của lỗi
                });
            }
        }
    }
}
