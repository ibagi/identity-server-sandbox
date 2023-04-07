using Duende.IdentityServer.Models;

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
    }
}
