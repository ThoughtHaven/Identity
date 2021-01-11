using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Emails;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.Internal;
using ThoughtHaven.Data;
using ThoughtHaven.Contacts;

namespace ThoughtHaven.AspNetCore.Identity
{
    public abstract partial class UserHelper
    {
        public virtual UiMessage UserAlreadyOwnsEmail { get; } = "This email is already associated with this account.";

        public virtual UiMessage EmailNotAvailable { get; } = "This email is not available. Is there a different one you can use?";

        public virtual UiMessage InvalidEmailVerificationCode { get; } = "We couldn't confirm your email. The link we sent you may have expired. You'll need to try confirming your email again.";

        protected abstract IRetrieveOperation<EmailAddress, TUser> UserEmailStore<TUser>()
            where TUser : class;

        public virtual Task<TUser?> Retrieve<TUser>(EmailAddress email)
            where TUser : class, IUserEmail
        {
            Guard.Null(nameof(email), email);

            return this.UserEmailStore<TUser>().Retrieve(email);
        }

        public virtual Task SetEmail<TUser>(TUser user, EmailAddress email)
            where TUser : IUserEmail
        {
            Guard.Null(nameof(user), user);
            Guard.Null(nameof(email), email);

            user.Email = email.Value.Trim().ToUpperInvariant();
            user.EmailConfirmed = false;

            return Task.CompletedTask;
        }

        public virtual Task ConfirmEmail<TUser>(TUser user) where TUser : IUserEmail
        {
            Guard.Null(nameof(user), user);

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException(
                    paramName: nameof(user),
                    message: $"The {nameof(user)} argument's {nameof(user.Email)} property cannot be null or white space.");
            }

            user.EmailConfirmed = true;

            return Task.CompletedTask;
        }

        public virtual async Task<VerificationCode> CreateEmailVerificationCode(UserKey userKey)
        {
            Guard.Null(nameof(userKey), userKey);

            var code = new VerificationCode((int)this._random.GenerateNumber(6));

            await this.SingleUseTokenService.Create(
                CreateSingleUseToken("em", userKey, code),
                expiration: new UtcDateTime(this.Clock.UtcNow.ToOffset().AddDays(7)))
                .ConfigureAwait(false);
            
            return code;
        }

        public virtual Task<bool> ValidateEmailVerificationCode(UserKey userKey,
            VerificationCode code)
        {
            Guard.Null(nameof(userKey), userKey);
            Guard.Null(nameof(code), code);

            return this.SingleUseTokenService.Validate(
                CreateSingleUseToken("em", userKey, code));
        }
    }
}