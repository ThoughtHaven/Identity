using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity.Claims
{
    public abstract class ClaimsAuthenticationServiceBase<TUser>
        : IAuthenticationService<ClaimsPrincipal>
        where TUser : class, IUserKey, IUserSecurityStamp
    {
        private readonly IRetrieveOperation<UserKey, TUser> _store;
        private readonly ClaimOptions _options;
        private readonly SystemClock _clock;

        protected ClaimsAuthenticationServiceBase(IRetrieveOperation<UserKey, TUser> userStore,
            ClaimOptions options, SystemClock clock)
        {
            this._store = Guard.Null(nameof(userStore), userStore);
            this._options = Guard.Null(nameof(options), options);
            this._clock = Guard.Null(nameof(clock), clock);
        }

        public abstract Task Login(ClaimsPrincipal principal,
            AuthenticationProperties properties);

        protected abstract Task RefreshLogin(ClaimsPrincipal principal);

        public virtual async Task<ClaimsPrincipal?> Authenticate()
        {
            var principal = await this.Authenticate(this._options.AuthenticationScheme)
                .ConfigureAwait(false);

            if (principal == null) { return null; }

            var claims = new UserClaims(principal.Identities.Where(
                    i => i.AuthenticationType == this._options.AuthenticationScheme).Single(),
                this._options.ClaimTypes);

            if (!claims.IsValid)
            {
                await this.Logout().ConfigureAwait(false);

                return null;
            }

            var timeSinceCheck = this._clock.UtcNow - claims.SecurityStampValidated;

            if (timeSinceCheck < this._options.ValidateSecurityStampInterval)
            { return principal; }

            var user = await this._store.Retrieve(claims.UserKey!).ConfigureAwait(false);

            if (user?.SecurityStamp == claims.SecurityStamp)
            {
                principal = this.UpdateSecurityStampValidatedClaim(principal);

                await this.RefreshLogin(principal).ConfigureAwait(false);

                return principal;
            }
            else
            {
                await this.Logout().ConfigureAwait(false);

                return null;
            }
        }

        protected abstract Task<ClaimsPrincipal?> Authenticate(string authenticationScheme);

        public abstract Task Logout();

        protected virtual ClaimsPrincipal UpdateSecurityStampValidatedClaim(
            ClaimsPrincipal principal)
        {
            Guard.Null(nameof(principal), principal);

            var identities = new List<ClaimsIdentity>();

            foreach (var id in principal.Identities)
            {
                if (id.AuthenticationType == this._options.AuthenticationScheme)
                {
                    var claim = id.FindFirst(this._options.ClaimTypes.SecurityStampValidated);

                    id.RemoveClaim(claim);
                    id.AddClaim(new Claim(this._options.ClaimTypes.SecurityStampValidated,
                        this._clock.UtcNow.UtcTicks.ToString()));
                }

                identities.Add(id);
            }

            return new ClaimsPrincipal(identities);
        }

        protected class UserClaims
        {
            public bool IsValid =>
                !(this.UserKey is null)
                && !string.IsNullOrWhiteSpace(this.SecurityStamp)
                && this.SecurityStampValidated.HasValue;
            public UserKey? UserKey { get; }
            public string? SecurityStamp { get; }
            public DateTimeOffset? SecurityStampValidated { get; }

            public UserClaims(ClaimsIdentity identity, ClaimOptions.ClaimTypeOptions options)
            {
                var keyClaim = identity.FindFirst(options.UserKey);
                var stampClaim = identity.FindFirst(options.SecurityStamp);
                var stampValidatedClaim = identity.FindFirst(options.SecurityStampValidated);

                this.UserKey = keyClaim != null ? new UserKey(keyClaim.Value) : null;
                this.SecurityStamp = stampClaim?.Value;
                this.SecurityStampValidated = stampValidatedClaim != null
                    ? (DateTimeOffset?)new DateTimeOffset(
                        ticks: long.Parse(stampValidatedClaim.Value),
                        offset: TimeSpan.Zero)
                    : null;
            }
        }
    }
}