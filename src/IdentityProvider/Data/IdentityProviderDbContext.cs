using IdentityProvider.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IdentityProvider.Data
{
    public class IdentityProviderDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserSecret> UserSecrets { get; set; }

        public IdentityProviderDbContext(DbContextOptions<IdentityProviderDbContext> options)
            : base(options)
        {

        }
    }

    public class IdentityProviderDbContextDesignTimeFactory : IDesignTimeDbContextFactory<IdentityProviderDbContext>
    {
        public IdentityProviderDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<IdentityProviderDbContext>()
                .UseSqlite("Data Source=IdentityProvider.db;");

            return new IdentityProviderDbContext(builder.Options);
        }
    }
}
