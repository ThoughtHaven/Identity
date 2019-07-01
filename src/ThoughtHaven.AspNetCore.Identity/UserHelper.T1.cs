using System.Collections.Generic;
using ThoughtHaven.AspNetCore.Identity.Lockouts;
using ThoughtHaven.AspNetCore.Identity.Passwords;
using ThoughtHaven.Data;
using ThoughtHaven.Contacts;
using ThoughtHaven.Security.SingleUseTokens;

namespace ThoughtHaven.AspNetCore.Identity
{
    public class UserHelper<TUser> : UserHelper
        where TUser : class
    {
        private readonly IRetrieveOperation<EmailAddress, TUser> _userEmailStore;

        protected override ISingleUseTokenService SingleUseTokenService { get; }
        protected override ICrudStore<string, TimedLockout> TimedLockoutStore { get; }
        protected override IPasswordHasher PasswordHasher { get; }
        protected override IEnumerable<IPasswordStrengthValidator> PasswordStrengthValidators { get; }
        protected override SystemClock Clock { get; }

        public UserHelper(IRetrieveOperation<EmailAddress, TUser> userEmailStore,
            ISingleUseTokenService singleUseTokenService,
            ICrudStore<string, TimedLockout> timedLockoutStore, IPasswordHasher passwordHasher,
            IEnumerable<IPasswordStrengthValidator> passwordStrengthValidators,
            SystemClock clock)
            : base()
        {
            this._userEmailStore = Guard.Null(nameof(userEmailStore), userEmailStore);
            this.SingleUseTokenService = Guard.Null(nameof(singleUseTokenService),
                singleUseTokenService);
            this.TimedLockoutStore = Guard.Null(nameof(timedLockoutStore), timedLockoutStore);
            this.PasswordHasher = Guard.Null(nameof(passwordHasher), passwordHasher);
            this.PasswordStrengthValidators = Guard.Null(nameof(passwordStrengthValidators),
                passwordStrengthValidators);
            this.Clock = Guard.Null(nameof(clock), clock);
        }

        protected override IRetrieveOperation<EmailAddress, TEmailUser> UserEmailStore<TEmailUser>() =>
            (IRetrieveOperation<EmailAddress, TEmailUser>)this._userEmailStore;
    }
}