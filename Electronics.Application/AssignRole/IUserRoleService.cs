namespace Electronics.Application.AssignRole
{
    public interface IUserRoleService
    {
        Task<bool> AssignRoleAsync(AssignRoleDto dto);
        Task<List<(string Id, string Name)>> GetUsersAsync();
        Task<List<string>> GetRolesAsync();
    }
}
