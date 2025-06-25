using Electronics.Application.User;
using Electronics.Domain.Entities;

namespace Electronics.Application.Interface
{
    public interface IUserRepository
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<bool> CreateUserAsync(CreateUserDto dto);
    }
}
