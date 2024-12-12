using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManageApp_Codef.Data.R_IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using StudentManageApp_Codef.Service;

namespace StudentManageApp_Codef.Data.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;

        private readonly EmailService _emailService;



        public AuthRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
            _emailService = emailService;
        }

        //public async Task<IdentityUser> RegisterAsync(string email, string password)
        //{
        //    var user = new IdentityUser { UserName = email, Email = email };

        //    user.EmailConfirmed = false; 
        //    user.PhoneNumberConfirmed = false; 
        //    user.TwoFactorEnabled = false; 
        //    user.LockoutEnabled = true; 

        //    try
        //    {
        //        var result = await _userManager.CreateAsync(user, password);

        //        if (!result.Succeeded)
        //        {
        //            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
        //            throw new Exception($"Failed to create user: {errors}");
        //        }   
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        Console.WriteLine("Lỗi DbUpdateException: " + ex.Message);
        //        if (ex.InnerException != null)
        //        {
        //            Console.WriteLine("InnerException: " + ex.InnerException.Message);
        //            Console.WriteLine("Stack Trace: " + ex.InnerException.StackTrace);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Lỗi khác: " + ex.Message);
        //    }

        //if (!result.Succeeded)
        //{
        //    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
        //    throw new Exception(errors);
        //}

        //    return user;
        //}

        //public async Task<(bool Succeeded, string[] Errors, IdentityUser? User)> RegisterAsync(string email, string password)
        //{
        //    var user = new IdentityUser
        //    {
        //        UserName = email,
        //        Email = email,
        //        EmailConfirmed = false,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnabled = true
        //    };

        //    var result = await _userManager.CreateAsync(user, password);

        //    if (!result.Succeeded)
        //    {
        //        return (false, result.Errors.Select(e => e.Description).ToArray(), null);
        //    }

        //    return (true, Array.Empty<string>(), user);
        //}

        public async Task<(bool Succeeded, string[] Errors, IdentityUser? User)> RegisterAsync(string email, string password)
        {
            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = false, // Chưa xác nhận email
                TwoFactorEnabled = true // Bật xác thực hai bước
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return (false, result.Errors.Select(e => e.Description).ToArray(), null);
            }

            // create otp
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // send email
            var callbackUrl = $"https://localhost:44378/api/Auth/ConfirmEmail?userId={user.Id}&token={Uri.EscapeDataString(token)}";
            await _emailService.SendEmailAsync(
                email,
                "Confirm your email",
                $"<p>Please confirm your account by clicking the link below:</p><a href='{callbackUrl}'>Confirm Email</a>"
            );

            return (true, Array.Empty<string>(), user);
        }

        public async Task<string> ValidateOtpForLogin(string email, string otp)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return ("User not found.");
            }

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, otp);
            if (!isValid)
            {
                return ("Invalid OTP.");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

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
            //return (true, "Login successful.");
        }

        public async Task<string> SendOtpAfterLogin(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new UnauthorizedAccessException("User not found.");

            if (user.TwoFactorEnabled)
            {
                var otp = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Your OTP Code",
                    $"<p>Xin chào {user.UserName},</p>" +
                    $"<p>Bạn đã yêu cầu xác thực OTP. Vui lòng click vào link dưới đây để xác thực:</p>" +
                    $"<p><a href='{"http://localhost:3000/students"}'>Xác thực OTP</a></p>" +
                    $"<p>Mã OTP: <strong>{otp}</strong></p>"
                );

                return "OTP has been sent to your email.";
            }

            throw new InvalidOperationException("Two-factor authentication is not enabled for this user.");
        }


        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new UnauthorizedAccessException("Invalid email or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) throw new UnauthorizedAccessException("Invalid email or password.");

            if (user.TwoFactorEnabled)
            {
                var message = await SendOtpAfterLogin(email);
                return message;
            }

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


        public async Task<(bool Success, string Message, IEnumerable<IdentityError> Errors)> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, "Invalid user.", null);
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return (false, "Failed to confirm email.", result.Errors);
            }

            return (true, "Email confirmed successfully!", null);
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
            var redirectUrl = "https://localhost:44378/dashboard";
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
