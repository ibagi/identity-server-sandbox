using IdentityProvider.Entities;

namespace IdentityProvider.Services
{
    public interface IUserService
    {
        Task<User?> GetByUsernameOrEmail(string usernameOrEmail);
        Task<List<UserClaim>> GetUserClaims(string subject);
        Task<bool> ValidateCredentialsAsync(string username, string password);
    }
}