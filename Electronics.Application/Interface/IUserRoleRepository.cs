using Electronics.Application.AssignRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronics.Application.Interface
{
    public interface IUserRoleRepository
    {
        Task<bool> AssignRoleAsync(AssignRoleDto dto);
        Task<List<(string Id, string Name)>> GetAllUsersAsync();
        Task<List<string>> GetAllRolesAsync();
    }
}
