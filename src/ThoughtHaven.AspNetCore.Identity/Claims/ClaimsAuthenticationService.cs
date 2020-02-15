using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity.Claims
{
    public class ClaimsAuthenticationService<TUser>
        : ClaimsAuthenticationServiceBase<TUser>
        where TUser : class, IUserKey, IUserSecurityStamp
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimOptions _options;
        private readonly SystemClock _clock;

        public ClaimsAuthenticationService(IHttpContextAccessor httpContextAccessor,
            IRetrieveOperation<UserKey, TUser> userStore,
            ClaimOptions options, SystemClock clock)
            : base(userStore, options, clock)
        {
            this._httpContextAccessor = Guard.Null(nameof(httpContextAccessor),
                httpContextAccessor);
            this._options = Guard.Null(nameof(options), options);
            this._clock = Guard.Null(nameof(clock), clock);
        }
        
        public override Task Login(ClaimsPrincipal principal,
            AuthenticationProperties properties)
        {
            Guard.Null(nameof(principal), principal);
            Guard.Null(nameof(properties), properties);

            if (properties.AllowRefresh == null) { properties.AllowRefresh = true; }

            if (properties.IssuedUtc == null)
            { properties.IssuedUtc = this._clock.UtcNow.ToOffset(); }

            return this._httpContextAccessor.HttpContext.SignInAsync(
                this._options.AuthenticationScheme, principal, properties);
        }

        protected override async Task RefreshLogin(ClaimsPrincipal principal)
        {
            Guard.Null(nameof(principal), principal);

            var result = await this.AuthenticateContext(this._options.AuthenticationScheme)
                .ConfigureAwait(false);

            if (!result.Succeeded)
            { throw new InvalidOperationException($"No authenticated principal for the following authentication scheme: {this._options.AuthenticationScheme}"); }

            await this.Login(principal, result.Properties).ConfigureAwait(false);
        }

        protected override async Task<ClaimsPrincipal?> Authenticate(
            string authenticationScheme)
        {
            Guard.NullOrWhiteSpace(nameof(authenticationScheme), authenticationScheme);

            var result = await this.AuthenticateContext(authenticationScheme)
                .ConfigureAwait(false);

            return result.Succeeded ? result.Principal : null;
        }

        protected virtual Task<AuthenticateResult> AuthenticateContext(
            string authenticationScheme)
        {
            Guard.NullOrWhiteSpace(nameof(authenticationScheme), authenticationScheme);

            return this._httpContextAccessor.HttpContext.AuthenticateAsync(authenticationScheme);
        }

        public override Task Logout() =>
            this._httpContextAccessor.HttpContext.SignOutAsync(
                this._options.AuthenticationScheme);
    }
}