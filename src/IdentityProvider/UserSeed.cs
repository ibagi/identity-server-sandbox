using System.Security.Claims;

namespace IdentityProvider
{
    record UserSeed(string Email, string UserName, string Password, IEnumerable<Claim> Claims);
}