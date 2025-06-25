using Electronics.Application.Interface;

namespace Electronics.Application.AssignRole
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _repo;

        public UserRoleService(IUserRoleRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> AssignRoleAsync(AssignRoleDto dto) => await _repo.AssignRoleAsync(dto);
        public async Task<List<(string Id, string Name)>> GetUsersAsync() => await _repo.GetAllUsersAsync();
        public async Task<List<string>> GetRolesAsync() => await _repo.GetAllRolesAsync();
    }
}
