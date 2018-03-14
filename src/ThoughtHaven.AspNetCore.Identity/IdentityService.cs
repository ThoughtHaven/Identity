using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using ThoughtHaven.AspNetCore.Identity.Claims;
using ThoughtHaven.AspNetCore.Identity.Created;
using ThoughtHaven.AspNetCore.Identity.Internal;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.LastLogins;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity
{
    public class IdentityService<TUser> : IdentityServiceBase<TUser>, IIdentityService<TUser>
        where TUser : class, IUserKey, IUserId, IUserCreated,
        IUserSecurityStamp, IUserLastLogin
    {
        public IUserHelper Helper { get; }

        public IdentityService(ICrudStore<UserKey, TUser> userStore,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IAuthenticationService<ClaimsPrincipal> claimsAuthenticationService,
            IUserClaimsConverter<TUser> claimsConverter, IUserHelper helper)
            : base(userStore, userValidators, claimsAuthenticationService, claimsConverter)
        {
            this.Helper = Guard.Null(nameof(helper), helper);
        }

        public override async Task<TUser> Create(TUser user)
        {
            Guard.Null(nameof(user), user);

            if (string.IsNullOrWhiteSpace(user.Id))
            { await this.Helper.AssignUserId(user).ConfigureAwait(false); }

            if (string.IsNullOrWhiteSpace(user.SecurityStamp))
            { await this.Helper.RefreshSecurityStamp(user).ConfigureAwait(false); }

            await this.Helper.SetCreated(user).ConfigureAwait(false);

            user.LastLogin = null;

            return await base.Create(user).ConfigureAwait(false);
        }

        public override async Task Login(TUser user, AuthenticationProperties properties)
        {
            Guard.Null(nameof(user), user);
            Guard.Null(nameof(properties), properties);

            await this.Helper.SetLastLogin(user).ConfigureAwait(false);

            await this.Update(user).ConfigureAwait(false);

            await base.Login(user, properties).ConfigureAwait(false);
        }
    }
}