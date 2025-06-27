namespace Electronics.Application.Auth
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginDto dto);
        Task LogoutAsync();
    }
}
