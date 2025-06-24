namespace Electronics.Application.User
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
    }
}
