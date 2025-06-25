using Electronics.Application.AssignRole;
using Electronics.Application.Interface;
using Electronics.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Electronics.Infrastructure.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AssignRoleAsync(AssignRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return false;

            if (!await _roleManager.RoleExistsAsync(dto.RoleName))
                return false;
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                    return false;
            }
            var addResult = await _userManager.AddToRoleAsync(user, dto.RoleName);
            return addResult.Succeeded;
        }

        public async Task<List<(string Id, string Name)>> GetAllUsersAsync()
        {
            return await Task.FromResult(_userManager.Users.ToList().Select(u => (u.Id, u.UserName)).ToList());
        }

        public async Task<List<string>> GetAllRolesAsync()
        {
            return await Task.FromResult(_roleManager.Roles.Select(r => r.Name).ToList());
        }
    }
}
