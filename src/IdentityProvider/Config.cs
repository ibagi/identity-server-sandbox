using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;
using System.Security.Claims;

namespace IdentityProvider
{
    static class Config
    {
        public static List<IdentityResource> IdentityResources => new()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
        };

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
            },

            new Client
            {
                ClientId = "ssr-client",
                ClientName= "SsrClient",
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets =
                {
                    new Secret("ssr-client-secret".Sha256())
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "api:read"
                },
                RedirectUris = 
                { 
                    "http://localhost:3000/api/auth/callback/duende-identityserver6" 
                },
                PostLogoutRedirectUris =
                {
                    "http://localhost:3000/"
                }
            }
        };

        public static List<UserSeed> Users => new()
        {
            new UserSeed("demo@email.com", "demo", "P@ssw0rd", new Claim[]
            {
                new Claim(JwtClaimTypes.Name, "demo"),
                new Claim(JwtClaimTypes.GivenName, "Demo User"),
                new Claim(JwtClaimTypes.Email, "demo@email.com")
            })
        };
    }
}
