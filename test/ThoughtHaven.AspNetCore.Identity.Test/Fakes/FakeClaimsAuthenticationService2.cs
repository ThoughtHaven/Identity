using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using ThoughtHaven.AspNetCore.Identity.Claims;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeClaimsAuthenticationService2 : ClaimsAuthenticationService<User>
    {
        public FakeClaimsAuthenticationService2(FakeHttpContextAccessor httpContextAccessor,
            FakeUserStore userStore, ClaimOptions options, FakeSystemClock clock)
            : base(httpContextAccessor, userStore, options, clock)
        { }

        new public Task RefreshLogin(ClaimsPrincipal principal) => base.RefreshLogin(principal);

        new public Task<ClaimsPrincipal?> Authenticate(string authenticationScheme) =>
            base.Authenticate(authenticationScheme);

        new public Task<AuthenticateResult> AuthenticateContext(string authenticationScheme) =>
            base.AuthenticateContext(authenticationScheme);
    }
}