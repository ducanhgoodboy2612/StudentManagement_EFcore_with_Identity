using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManageApp_Codef.Data.R_IRepository;
using StudentManageApp_Codef.Data.Repository;
using StudentManageApp_Codef.Service;

namespace StudentManageApp_Codef.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authService;

        public AuthController(IAuthRepository authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterModel model)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    var user = await _authService.RegisterAsync(model.Email, model.Password);
        //    return Ok(new { user.Email });
        //}

        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (succeeded, errors, user) = await _authService.RegisterAsync(model.Email, model.Password);

            if (!succeeded)
            {
                return BadRequest(new
                {
                    Message = "User registration failed.",
                    Errors = errors
                });
            }

            return Ok(new
            {
                Message = "User registered successfully.",
                User = new { user.Email }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var token = await _authService.LoginAsync(model.Email, model.Password);
            return Ok(new { Token = token });
        }

        

        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OTPVerifyModel model)
        {
            if (string.IsNullOrWhiteSpace(model.email) || string.IsNullOrWhiteSpace(model.otp))
            {
                return BadRequest(new { Message = "Email and OTP are required." });
            }

            var token = await _authService.ValidateOtpForLogin(model.email, model.otp);
            if (token == null)
            {
                return BadRequest(new { Message = "Error" });
            }

            return Ok(new { Token = token });
        }




        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required.");

            var resetToken = await _authService.ForgotPasswordAsync(email);
            return Ok(new { Message = "Password reset token generated.", ResetToken = resetToken });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string email, string token, string newPassword)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(newPassword))
                return BadRequest("Email, token, and new password are required.");

            await _authService.ResetPasswordAsync(email, token, newPassword);
            return Ok(new { Message = "Password has been reset successfully." });
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var (success, message, errors) = await _authService.ConfirmEmailAsync(userId, token);

            if (!success)
            {
                return BadRequest(new { message, errors });
            }

            return Ok(new { message });
        }



        [HttpGet("LoginWithGoogle")]
        public IActionResult LoginWithGoogle(string returnUrl = "/")
        {
            return _authService.GetGoogleLoginProperties(returnUrl);
        }

        [HttpGet("GoogleCallback")]
        public async Task<IActionResult> GoogleCallback(string returnUrl = "/")
        {
            var result = await _authService.ProcessGoogleCallbackAsync(returnUrl);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Failed to process Google login.", errors = result.Errors });
            }
            return Ok(new { message = "Google login successful", returnUrl });
        }

    }

    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class OTPVerifyModel
    {
        public string email { get; set; }
        public string otp { get; set; }
    }
}
