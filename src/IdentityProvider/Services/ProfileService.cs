using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using System.Security.Claims;

namespace IdentityProvider.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserService _userService;

        public ProfileService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var claims = await _userService.GetUserClaims(subjectId);

            context.AddRequestedClaims(claims.Select(c => new Claim(c.Type, c.Value)));
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
