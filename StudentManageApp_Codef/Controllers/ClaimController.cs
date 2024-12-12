using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManageApp_Codef.Data.R_IRepository;

namespace StudentManageApp_Codef.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimRepository _roleClaimRepository;

        public ClaimController(IClaimRepository roleClaimRepository)
        {
            _roleClaimRepository = roleClaimRepository;
        }

        // Thêm claim cho một vai trò
        [HttpPost("AddClaimToRole")]
        public async Task<IActionResult> AddClaimToRole(string roleName, string claimType, string claimValue)
        {
            var result = await _roleClaimRepository.AddClaimToRoleAsync(roleName, claimType, claimValue);
            if (result.Succeeded)
            {
                return Ok(new { message = "Claim added successfully" });
            }

            return BadRequest(result.Errors);
        }

        // Lấy tất cả các claims của một vai trò
        [HttpGet("GetClaimsForRole/{roleName}")]
        public async Task<IActionResult> GetClaimsForRole(string roleName)
        {
            var claims = await _roleClaimRepository.GetClaimsForRoleAsync(roleName);
            if (claims == null)
            {
                return NotFound(new { message = "Role not found" });
            }

            return Ok(claims);
        }

        // Xóa claim của một vai trò
        [HttpDelete("RemoveClaimFromRole")]
        public async Task<IActionResult> RemoveClaimFromRole(string roleName, string claimType, string claimValue)
        {
            var result = await _roleClaimRepository.RemoveClaimFromRoleAsync(roleName, claimType, claimValue);
            if (result.Succeeded)
            {
                return Ok(new { message = "Claim removed successfully" });
            }

            return BadRequest(result.Errors);
        }
    }
}
