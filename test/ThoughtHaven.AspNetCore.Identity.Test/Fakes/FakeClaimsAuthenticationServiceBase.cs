using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using ThoughtHaven.AspNetCore.Identity.Claims;
using ThoughtHaven.AspNetCore.Identity.Keys;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeClaimsAuthenticationServiceBase : ClaimsAuthenticationServiceBase<User>
    {
        public FakeClaimsAuthenticationServiceBase(FakeUserStore userStore,
            ClaimOptions options, FakeSystemClock clock)
            : base(userStore, options, clock)
        { }

        public ClaimsPrincipal? Login_InputPrincipal;
        public AuthenticationProperties? Login_InputProperties;
        public override Task Login(ClaimsPrincipal principal,
            AuthenticationProperties properties)
        {
            this.Login_InputPrincipal = principal;
            this.Login_InputProperties = properties;

            return Task.CompletedTask;
        }

        public bool Logout_Called;
        public override Task Logout()
        {
            this.Logout_Called = true;

            return Task.CompletedTask;
        }

        public static ClaimsPrincipal Principal(bool hasUserKey = true,
            bool hasSecurityStamp = true, bool hasSecurityStampValidated = true,
            FakeSystemClock? clock = null)
        {
            clock ??= new FakeSystemClock(DateTimeOffset.UtcNow);
            var options = new ClaimOptions();
            var claims = new List<Claim>();

            if (hasUserKey) { claims.Add(new Claim(options.ClaimTypes.UserKey, "key")); }

            if (hasSecurityStamp)
            { claims.Add(new Claim(options.ClaimTypes.SecurityStamp, "stamp")); }

            if (hasSecurityStampValidated)
            {
                claims.Add(new Claim(options.ClaimTypes.SecurityStampValidated,
                    clock.UtcNow.Ticks.ToString()));
            }

            return new ClaimsPrincipal(
                new ClaimsIdentity(claims, options.AuthenticationScheme));
        }
        public string? Authenticate_InputAuthenticationScheme;
        public UserKey? Authenticate_OutputUserKey =>
            this.Authenticate_Output?.FindFirst(new ClaimOptions().ClaimTypes.UserKey)!.Value!;
        public UserKey? Authenticate_OutputSecurityStamp =>
            this.Authenticate_Output?.FindFirst(new ClaimOptions().ClaimTypes.SecurityStamp)!.Value!;
        public ClaimsPrincipal? Authenticate_Output = Principal();
        protected override Task<ClaimsPrincipal?> Authenticate(string authenticationScheme)
        {
            this.Authenticate_InputAuthenticationScheme = authenticationScheme;

            return Task.FromResult(this.Authenticate_Output);
        }

        public ClaimsPrincipal? RefreshLogin_InputPrincipal;
        protected override Task RefreshLogin(ClaimsPrincipal principal)
        {
            this.RefreshLogin_InputPrincipal = principal;

            return Task.CompletedTask;
        }
    }
}