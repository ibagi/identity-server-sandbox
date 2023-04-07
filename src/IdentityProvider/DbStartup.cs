using IdentityProvider.Data;
using IdentityProvider.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider
{
    static class DbStartup
    {
        public static async Task ConfigureDb(IServiceScope scope, IEnumerable<UserSeed> initialUsers)
        {
            var dbContext = scope.ServiceProvider.GetService<IdentityProviderDbContext>()!;
            var userManager = scope.ServiceProvider.GetService<UserManager<User>>()!;

            dbContext.Database.Migrate();
            await CreateInitialUsers(userManager, initialUsers);
        }

        private static async Task CreateInitialUsers(UserManager<User> userManager, IEnumerable<UserSeed> seed)
        {
            foreach (var user in seed)
            {
                var existingUser = await userManager.FindByEmailAsync(user.Email);

                if (existingUser is not null)
                {
                    continue;
                }

                var applicationUser = new User
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(applicationUser, user.Password);
                await userManager.AddClaimsAsync(applicationUser, user.Claims);
            }
        }
    }
}
