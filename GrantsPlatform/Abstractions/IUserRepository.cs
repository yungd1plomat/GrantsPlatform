using GrantsPlatform.Data;

namespace GrantsPlatform.Abstractions
{
    public interface IUserRepository
    {
        Task<ApplicationUser> AuthenticateAsync(string login, string password);

        Task<string> GenerateJwtTokenAsync(ApplicationUser user);
    }
}
