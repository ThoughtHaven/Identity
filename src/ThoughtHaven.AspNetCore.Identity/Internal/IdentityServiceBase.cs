using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using ThoughtHaven.AspNetCore.Identity.Claims;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity.Internal
{
    public class IdentityServiceBase<TUser> : IIdentityServiceBase<TUser> where TUser : class
    {
        private readonly ICrudStore<UserKey, TUser> _userStore;
        private readonly IEnumerable<IUserValidator<TUser>> _userValidators;
        private readonly IAuthenticationService<ClaimsPrincipal> _claimsAuthenticationService;
        private readonly IUserClaimsConverter<TUser> _claimsConverter;

        protected IdentityServiceBase(ICrudStore<UserKey, TUser> userStore,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IAuthenticationService<ClaimsPrincipal> claimsAuthenticationService,
            IUserClaimsConverter<TUser> claimsConverter)
        {
            this._userStore = Guard.Null(nameof(userStore), userStore);
            this._userValidators = Guard.Null(nameof(userValidators), userValidators);
            this._claimsAuthenticationService = Guard.Null(nameof(claimsAuthenticationService),
                claimsAuthenticationService);
            this._claimsConverter = Guard.Null(nameof(claimsConverter), claimsConverter);
        }

        public virtual Task<TUser?> Retrieve(UserKey key)
        {
            Guard.Null(nameof(key), key);

            return this._userStore.Retrieve(key);
        }

        public virtual async Task<TUser> Create(TUser user)
        {
            Guard.Null(nameof(user), user);

            await this.Validate(user).ConfigureAwait(false);

            return await this._userStore.Create(user).ConfigureAwait(false);
        }

        public virtual async Task<TUser> Update(TUser user)
        {
            Guard.Null(nameof(user), user);

            await this.Validate(user).ConfigureAwait(false);

            return await this._userStore.Update(user).ConfigureAwait(false);
        }

        public virtual Task Delete(UserKey key)
        {
            Guard.Null(nameof(key), key);

            return this._userStore.Delete(key);
        }

        public virtual async Task Login(TUser user, AuthenticationProperties properties)
        {
            Guard.Null(nameof(user), user);
            Guard.Null(nameof(properties), properties);

            var principal = await this._claimsConverter.Convert(user).ConfigureAwait(false);

            await this._claimsAuthenticationService.Login(principal, properties)
                .ConfigureAwait(false);
        }

        public virtual async Task<TUser?> Authenticate()
        {
            var principal = await this._claimsAuthenticationService.Authenticate()
                .ConfigureAwait(false);

            if (principal == null) { return null; }

            var key = await this._claimsConverter.Convert(principal);

            if (key is null) { return null; }

            return await this.Retrieve(key).ConfigureAwait(false);
        }

        public virtual Task Logout() => this._claimsAuthenticationService.Logout();

        private async Task Validate(TUser user)
        {
            foreach (var validator in this._userValidators)
            { await validator.Validate(user).ConfigureAwait(false); }
        }
    }
}