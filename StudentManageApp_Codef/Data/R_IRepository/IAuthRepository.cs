using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace StudentManageApp_Codef.Data.R_IRepository
{
    public partial interface IAuthRepository
    {
        //Task<IdentityUser> RegisterAsync(string email, string password);
        Task<(bool Succeeded, string[] Errors, IdentityUser? User)> RegisterAsync(string email, string password);
        Task<string> LoginAsync(string email, string password);
        Task<string> ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(string email, string resetToken, string newPassword);
        IActionResult GetGoogleLoginProperties(string returnUrl);
        Task<IdentityResult> ProcessGoogleCallbackAsync(string returnUrl);
        Task<(bool Success, string Message, IEnumerable<IdentityError> Errors)> ConfirmEmailAsync(string userId, string token);
        Task<string> ValidateOtpForLogin(string email, string otp);
    }
}
