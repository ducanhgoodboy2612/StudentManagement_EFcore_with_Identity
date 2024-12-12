using Microsoft.AspNetCore.Identity;

namespace StudentManageApp_Codef.Data.R_IRepository
{
    public partial interface IClaimRepository
    {
        Task<IdentityResult> AddClaimToRoleAsync(string roleName, string claimType, string claimValue);
        Task<List<IdentityRoleClaim<string>>> GetClaimsForRoleAsync(string roleName);
        Task<IdentityResult> RemoveClaimFromRoleAsync(string roleName, string claimType, string claimValue);


     }
}
