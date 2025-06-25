using Electronics.Application.Interface;

namespace Electronics.Application.Role
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repo;

        public RoleService(IRoleRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<RoleDto>> GetAllAsync() => await _repo.GetAllRolesAsync();
        public async Task<RoleDto> GetByIdAsync(string id) => await _repo.GetRoleByIdAsync(id);
        public async Task<bool> CreateAsync(string name) => await _repo.CreateRoleAsync(name);
        public async Task<bool> UpdateAsync(string id, string name) => await _repo.UpdateRoleAsync(id, name);
        public async Task<bool> DeleteAsync(string id) => await _repo.DeleteRoleAsync(id);
    }
}
