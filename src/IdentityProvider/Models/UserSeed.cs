using System.Security.Claims;

namespace IdentityProvider.Models
{
    record UserSeed(string Email, string UserName, string Password, IEnumerable<Claim> Claims);
}
