using Electronics.Application.Role;

namespace Electronics.Application.Interface
{
    public interface IRoleRepository
    {
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(string id);
        Task<bool> CreateRoleAsync(string name);
        Task<bool> UpdateRoleAsync(string id, string name);
        Task<bool> DeleteRoleAsync(string id);
    }
}
