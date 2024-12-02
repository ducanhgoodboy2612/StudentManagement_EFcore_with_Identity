using Microsoft.AspNetCore.Identity;

namespace StudentManageApp_Codef.Data.R_IRepository
{
    public partial interface IRoleRepository
    {
        Task<List<IdentityRole>> GetAllRolesAsync();
        Task<IdentityRole> GetRoleByIdAsync(string roleId);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> UpdateRoleAsync(string roleId, string newRoleName);
        Task<IdentityResult> DeleteRoleAsync(string roleId);
        Task<IdentityResult> AssignRoleToUserAsync(string userId, string roleName);
    }
}
