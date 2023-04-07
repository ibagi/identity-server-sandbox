using IdentityModel;
using IdentityProvider.Data;
using IdentityProvider.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IdentityProvider
{
    public static class DbStartup
    {
        public static async Task ConfigureDb(IServiceScope scope)
        {
            var dbContext = scope.ServiceProvider.GetService<IdentityProviderDbContext>()!;
            var userManager = scope.ServiceProvider.GetService<UserManager<User>>()!;

            dbContext.Database.Migrate();
            await EnsureDemoUserIsCreated(userManager);
        }

        private static async Task EnsureDemoUserIsCreated(UserManager<User> userManager)
        {
            var email = "demo@email.com";
            var existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser is not null)
            {
                return;
            }

            var demoUser = new User
            {
                Email = email,
                UserName = "demo",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(demoUser);

            await userManager.AddClaimsAsync(demoUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, "Demo User"),
                new Claim(JwtClaimTypes.GivenName, "Demo"),
                new Claim(JwtClaimTypes.FamilyName, "User"),
                new Claim(JwtClaimTypes.WebSite, "http://demo.user.com")
            });
        }
    }
}
