using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.Passwords;

namespace ThoughtHaven.AspNetCore.Identity
{
    public abstract partial class UserHelper
    {
        public virtual UserMessage InvalidPassword { get; } = "That password wasn't right.";

        public virtual UserMessage InvalidPasswordResetCode { get; }
            = "That password reset code wasn't right. It may have expired.";

        protected abstract IEnumerable<IPasswordStrengthValidator> PasswordStrengthValidators { get; }

        protected abstract IPasswordHasher PasswordHasher { get; }

        public virtual async Task<Result<UserMessage>> SetPasswordHash<TUser>(TUser user,
            Password password)
            where TUser : IUserPasswordHash
        {
            Guard.Null(nameof(user), user);
            Guard.Null(nameof(password), password);

            Result<UserMessage> result = new Result<UserMessage>();

            foreach (var validator in this.PasswordStrengthValidators)
            {
                result = await validator.Validate(password).ConfigureAwait(false);

                if (!result.Success) { break; }
            }

            if (result.Success)
            {
                user.PasswordHash = this.PasswordHasher.Hash(password);
            }

            return result;
        }

        public virtual Task<PasswordValidateResult> ValidatePassword<TUser>(TUser user,
            Password password)
            where TUser : IUserPasswordHash
        {
            Guard.Null(nameof(user), user);
            Guard.Null(nameof(password), password);

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                throw new ArgumentException(
                    paramName: nameof(user),
                    message: $"The {nameof(user)} argument's {nameof(user.PasswordHash)} property cannot be null or white space.");
            }

            return Task.FromResult(this.PasswordHasher.Validate(user.PasswordHash!, password));
        }

        public virtual async Task<PasswordResetCode> CreatePasswordResetCode(UserKey userKey)
        {
            Guard.Null(nameof(userKey), userKey);

            var code = new PasswordResetCode((int)this._random.GenerateNumber(6));

            await this.SingleUseTokenService.Create(
                CreateSingleUseToken("pw", userKey, code),
                expiration: new UtcDateTime(this.Clock.UtcNow.ToOffset().AddDays(1)))
                .ConfigureAwait(false);

            return code;
        }

        public virtual Task<bool> ValidatePasswordResetCode(UserKey userKey,
            PasswordResetCode code)
        {
            Guard.Null(nameof(userKey), userKey);
            Guard.Null(nameof(code), code);

            return this.SingleUseTokenService.Validate(
                CreateSingleUseToken("pw", userKey, code));
        }
    }
}