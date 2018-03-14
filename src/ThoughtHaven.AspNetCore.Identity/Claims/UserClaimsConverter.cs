using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;

namespace ThoughtHaven.AspNetCore.Identity.Claims
{
    public class UserClaimsConverter<TUser> : IUserClaimsConverter<TUser>
        where TUser : IUserKey, IUserSecurityStamp
    {
        private readonly ClaimOptions _options;
        private readonly SystemClock _clock;

        public UserClaimsConverter(ClaimOptions options, SystemClock clock)
        {
            this._options = Guard.Null(nameof(options), options);
            this._clock = Guard.Null(nameof(clock), clock);
        }

        public virtual async Task<ClaimsPrincipal> Convert(TUser user)
        {
            Guard.Null(nameof(user), user);

            var claims = await this.CreateClaims(user).ConfigureAwait(false);

            var identity = new ClaimsIdentity(this._options.AuthenticationScheme);
            identity.AddClaims(claims);

            return new ClaimsPrincipal(identity);
        }

        protected virtual Task<IEnumerable<Claim>> CreateClaims(TUser user)
        {
            Guard.Null(nameof(user), user);

            var key = user.Key();
            if (key == null)
            {
                throw new ArgumentException(
                    paramName: nameof(user),
                    message: $"The {nameof(user)} argument's {nameof(user.Key)}() method cannot return null.");
            }

            if (string.IsNullOrWhiteSpace(user.SecurityStamp))
            {
                throw new ArgumentException(
                    paramName: nameof(user),
                    message: $"The {nameof(user)} argument's {nameof(IUserSecurityStamp.SecurityStamp)} property cannot be null or white space.");
            }

            var claims = new Claim[]
            {
                new Claim(this._options.ClaimTypes.UserKey, key.Value,
                    ClaimValueTypes.String, this._options.Issuer),
                new Claim(this._options.ClaimTypes.SecurityStamp, user.SecurityStamp,
                    ClaimValueTypes.String, this._options.Issuer),
                new Claim(this._options.ClaimTypes.SecurityStampValidated,
                    this._clock.UtcNow.UtcTicks.ToString(), ClaimValueTypes.String,
                    this._options.Issuer)
            };

            return Task.FromResult<IEnumerable<Claim>>(claims);
        }

        public virtual Task<UserKey> Convert(ClaimsPrincipal principal)
        {
            Guard.Null(nameof(principal), principal);

            UserKey result = null;

            foreach (var id in principal?.Identities)
            {
                if (id.AuthenticationType != this._options.AuthenticationScheme) { continue; }

                var identifierClaim = id.FindFirst(this._options.ClaimTypes.UserKey);

                if (identifierClaim != null) { result = new UserKey(identifierClaim.Value); }
            }

            return Task.FromResult(result);
        }
    }
}