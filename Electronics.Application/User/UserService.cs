using Electronics.Application.Interface;

namespace Electronics.Application.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return users.Select(user => new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email
            }).ToList();
        }
        public async Task<bool> CreateUserAsync(CreateUserDto dto)
        {
            return await _userRepository.CreateUserAsync(dto);
        }
    }
}
