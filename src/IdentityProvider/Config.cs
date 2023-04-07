using Duende.IdentityServer.Models;
using IdentityModel;
using IdentityProvider.Models;
using System.Security.Claims;

namespace IdentityProvider
{
    static class Config
    {
        public static List<ApiScope> ApiScopes => new()
        {
            new ApiScope
            {
                Name = "api:read",
                DisplayName= "ApiRead",
            }
        };

        public static List<Client> Clients => new()
        {
            new Client
            {
                ClientId = "console-client",
                ClientName= "ConsoleClient",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("console-client-secret".Sha256())
                },
                AllowedScopes =
                {
                    "api:read"
                }
            }
        };

        public static List<UserSeed> Users => new()
        {
            new UserSeed("demo@email.com", "demo", "P@ssw0rd", new Claim[]
            {
                new Claim(JwtClaimTypes.Name, "Demo User"),
                new Claim(JwtClaimTypes.GivenName, "Demo"),
                new Claim(JwtClaimTypes.FamilyName, "User"),
                new Claim(JwtClaimTypes.WebSite, "http://demo.user.com")
            })
        };
    }
}
