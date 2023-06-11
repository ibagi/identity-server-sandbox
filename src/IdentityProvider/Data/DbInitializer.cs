using IdentityProvider.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.Data
{
    static class DbInitializer
    {
        public static void ConfigureDb(IServiceScope scope, IEnumerable<UserSeed> initialUsers)
        {
            var dbContext = scope.ServiceProvider.GetService<IdentityProviderDbContext>()!;
            dbContext.Database.Migrate();

            CreateInitialUsers(dbContext, initialUsers);
        }

        private static void CreateInitialUsers(IdentityProviderDbContext dbContext, IEnumerable<UserSeed> seed)
        {
            var initialUsers =
                from user in seed
                let userId = Guid.NewGuid()
                let salt = BCrypt.Net.BCrypt.GenerateSalt()
                select new User
                {
                    Email = user.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password, salt),
                    PasswordSalt = salt,
                    Active = true,
                    Id = userId,
                    Subject = userId.ToString(),
                    UserName = user.UserName,
                    Claims = user.Claims.Select(c => new UserClaim
                    {
                        Id = Guid.NewGuid(),
                        Type = c.Type,
                        Value = c.Value,
                        UserId = userId
                    })
                    .ToList()
                };

            foreach (var user in initialUsers)
            {
                var userExists = dbContext.Users.Any(u => u.UserName == user.UserName);

                if (!userExists)
                {
                    dbContext.Add(user);
                }
            }

            dbContext.SaveChanges();
        }
    }
}
