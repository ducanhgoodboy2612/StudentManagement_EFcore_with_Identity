using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManageApp_Codef.Data.R_IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManageApp_Codef.Data.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<IdentityUser> RegisterAsync(string email, string password)
        {
            var user = new IdentityUser { UserName = email, Email = email };

            user.EmailConfirmed = false; 
            user.PhoneNumberConfirmed = false; 
            user.TwoFactorEnabled = false; 
            user.LockoutEnabled = true; 

            try
            {
                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create user: {errors}");
                }   
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Lỗi DbUpdateException: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("InnerException: " + ex.InnerException.Message);
                    Console.WriteLine("Stack Trace: " + ex.InnerException.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khác: " + ex.Message);
            }

            //if (!result.Succeeded)
            //{
            //    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            //    throw new Exception(errors);
            //}

            return user;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new UnauthorizedAccessException("Invalid email or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) throw new UnauthorizedAccessException("Invalid email or password.");

            // Generate JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<IdentityResult> AddGoogleLoginAsync(string email, string providerKey)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Nếu người dùng chưa tồn tại, tạo mới
                user = new IdentityUser { UserName = email, Email = email };
                var createUserResult = await _userManager.CreateAsync(user);
                if (!createUserResult.Succeeded)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Description = "Failed to create user during Google login."
                    });
                }
            }

            // Kiểm tra nếu login provider đã tồn tại
            var loginInfo = new UserLoginInfo("Google", providerKey, "Google");
            var existingLogins = await _userManager.GetLoginsAsync(user);
            if (!existingLogins.Any(l => l.LoginProvider == "Google" && l.ProviderKey == providerKey))
            {
                var addLoginResult = await _userManager.AddLoginAsync(user, loginInfo);
                if (!addLoginResult.Succeeded)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Description = "Failed to associate Google login."
                    });
                }
            }

            return IdentityResult.Success;
        }

        public async Task<string> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception("User not found.");

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            //  gửi token qua email)
            return resetToken;
        }

        public async Task ResetPasswordAsync(string email, string resetToken, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception("User not found.");

            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to reset password: {errors}");
            }
        }

        public IActionResult GetGoogleLoginProperties(string returnUrl)
        {
            var redirectUrl = $"/api/Account/GoogleCallback?returnUrl={returnUrl}";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        public async Task<IdentityResult> ProcessGoogleCallbackAsync(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Failed to retrieve external login info." });
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var providerKey = info.ProviderKey;

            // Kiểm tra hoặc tạo tài khoản Google
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser { Email = email, UserName = email };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded) return result;
            }

            var existingLogins = await _userManager.GetLoginsAsync(user);
            if (!existingLogins.Any(l => l.LoginProvider == "Google" && l.ProviderKey == providerKey))
            {
                var loginInfo = new UserLoginInfo("Google", providerKey, "Google");
                var loginResult = await _userManager.AddLoginAsync(user, loginInfo);
                if (!loginResult.Succeeded) return loginResult;
            }

            // Đăng nhập người dùng
            await _signInManager.SignInAsync(user, isPersistent: false);
            return IdentityResult.Success;
        }

    }


    public class PasswordHashChecker
    {
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;

        public PasswordHashChecker(IPasswordHasher<IdentityUser> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public bool VerifyPassword(string passwordHash, string plainPassword)
        {
            // Tạo một đối tượng giả cho user (IdentityUser không bắt buộc phải có dữ liệu thực tế ở đây).
            var dummyUser = new IdentityUser();

            // So sánh mật khẩu gốc với mật khẩu đã được hash.
            var result = _passwordHasher.VerifyHashedPassword(dummyUser, passwordHash, plainPassword);

            return result == PasswordVerificationResult.Success;
        }
    }
}
