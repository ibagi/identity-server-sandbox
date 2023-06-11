using IdentityProvider.Data;
using IdentityProvider.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.Services
{
    public class UserService : IUserService
    {
        private readonly IdentityProviderDbContext _dbContext;

        public UserService(IdentityProviderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            var user = await GetByUsernameOrEmail(username);

            return user switch
            {
                null => false,
                { Active: false } => false,
                _ => BCrypt.Net.BCrypt.HashPassword(password, user.PasswordSalt) == user.Password
            };
        }

        public Task<User?> GetByUsernameOrEmail(string usernameOrEmail)
        {
            return _dbContext.Users.FirstOrDefaultAsync(u =>
                u.UserName == usernameOrEmail ||
                u.Email == usernameOrEmail);
        }

        public async Task<List<UserClaim>> GetUserClaims(string subject)
        {
            var claims = await _dbContext.Users
                 .Where(u => u.Subject == subject)
                 .Select(u => u.Claims)
                 .FirstOrDefaultAsync();

            return claims?.ToList() ?? new List<UserClaim>();
        }
    }
}
