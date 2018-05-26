using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Internal;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.Lockouts;
using ThoughtHaven.AspNetCore.Identity.Passwords;
using ThoughtHaven.Data;
using ThoughtHaven.Messages.Emails;
using ThoughtHaven.Security.SingleUseTokens;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeUserHelper1 : UserHelper
    {
        public FakeSystemClock FakeClock = new FakeSystemClock(DateTimeOffset.UtcNow);
        protected override SystemClock Clock => FakeClock;

        public FakeSingleUseTokenService FakeSingleUseTokenService = new FakeSingleUseTokenService();
        protected override ISingleUseTokenService SingleUseTokenService => FakeSingleUseTokenService;

        public FakeTimedLockoutStore FakeTimedLockoutStore = new FakeTimedLockoutStore();
        protected override ICrudStore<string, TimedLockout> TimedLockoutStore =>
            FakeTimedLockoutStore;

        protected override IEnumerable<IPasswordStrengthValidator> PasswordStrengthValidators =>
            new IPasswordStrengthValidator[] { new MinimumLengthPasswordStrengthValidator() };

        public FakePasswordHasher FakePasswordHasher = new FakePasswordHasher();
        protected override IPasswordHasher PasswordHasher => FakePasswordHasher;

        public FakeUserEmailStore FakeUserEmailStore = new FakeUserEmailStore();
        protected override IRetrieveOperation<EmailAddress, TUser> UserEmailStore<TUser>() =>
            (IRetrieveOperation<EmailAddress, TUser>)FakeUserEmailStore;

        public object AssignUserId_InputUser;
        public override Task AssignUserId<TUser>(TUser user)
        {
            this.AssignUserId_InputUser = user;

            return base.AssignUserId(user);
        }

        public object SetCreated_InputUser;
        public override Task SetCreated<TUser>(TUser user)
        {
            this.SetCreated_InputUser = user;

            return base.SetCreated(user);
        }

        public object SetEmail_InputUser;
        public EmailAddress SetEmail_InputEmail;
        public override Task SetEmail<TUser>(TUser user, EmailAddress email)
        {
            this.SetEmail_InputUser = user;
            this.SetEmail_InputEmail = email;

            return base.SetEmail(user, email);
        }

        public object ConfirmEmail_InputUser;
        public override Task ConfirmEmail<TUser>(TUser user)
        {
            this.ConfirmEmail_InputUser = user;

            return base.ConfirmEmail(user);
        }

        public object SetPasswordHash_InputUser;
        public Password SetPasswordHash_InputPassword;
        public UserMessage SetPasswordHash_OutputFailure;
        public override Task<Result<UserMessage>> SetPasswordHash<TUser>(TUser user,
            Password password)
        {
            this.SetPasswordHash_InputUser = user;
            this.SetPasswordHash_InputPassword = password;

            return this.SetPasswordHash_OutputFailure == null
                ? base.SetPasswordHash(user, password)
                : Task.FromResult<Result<UserMessage>>(this.SetPasswordHash_OutputFailure);
        }

        public object ValidatePassword_InputUser;
        public Password ValidatePassword_InputPassword;
        public PasswordValidateResult? ValidatePassword_OutputOverride;
        public override Task<PasswordValidateResult> ValidatePassword<TUser>(TUser user,
            Password password)
        {
            this.ValidatePassword_InputUser = user;
            this.ValidatePassword_InputPassword = password;

            return !ValidatePassword_OutputOverride.HasValue
                ? base.ValidatePassword(user, password)
                : Task.FromResult(this.ValidatePassword_OutputOverride.Value);
        }

        public string IsLockedOut_InputKey;
        public bool? IsLockedOut_OutputOverride;
        public override Task<bool> IsLockedOut(string key)
        {
            this.IsLockedOut_InputKey = key;

            return this.IsLockedOut_OutputOverride == null ? base.IsLockedOut(key)
                : Task.FromResult(this.IsLockedOut_OutputOverride.Value);
        }

        public string ResetLockedOut_InputKey;
        public override Task ResetLockedOut(string key)
        {
            this.ResetLockedOut_InputKey = key;

            return base.ResetLockedOut(key);
        }

        public UserKey CreatePasswordResetCode_InputUserKey;
        public PasswordResetCode CreatePasswordResetCode_Output;
        public override async Task<PasswordResetCode> CreatePasswordResetCode(UserKey userKey)
        {
            this.CreatePasswordResetCode_InputUserKey = userKey;
            this.CreatePasswordResetCode_Output = await base.CreatePasswordResetCode(userKey);

            return this.CreatePasswordResetCode_Output;
        }

        public UserKey CreateEmailVerificationCode_InputUserKey;
        public VerificationCode CreateEmailVerificationCode_Output;
        public override async Task<VerificationCode> CreateEmailVerificationCode(
            UserKey userKey)
        {
            this.CreateEmailVerificationCode_InputUserKey = userKey;
            this.CreateEmailVerificationCode_Output = await base.CreateEmailVerificationCode(
                userKey);

            return this.CreateEmailVerificationCode_Output;
        }

        public UserKey ValidateEmailVerificationCode_InputUserKey;
        public VerificationCode ValidateEmailVerificationCode_InputCode;
        public bool? ValidateEmailVerificationCode_OutputOverride;
        public override Task<bool> ValidateEmailVerificationCode(UserKey userKey,
            VerificationCode code)
        {
            this.ValidateEmailVerificationCode_InputUserKey = userKey;
            this.ValidateEmailVerificationCode_InputCode = code;

            return !this.ValidateEmailVerificationCode_OutputOverride.HasValue
                ? base.ValidateEmailVerificationCode(userKey, code)
                : Task.FromResult(this.ValidateEmailVerificationCode_OutputOverride.Value);
        }

        public UserKey ValidatePasswordResetCode_InputUserKey;
        public PasswordResetCode ValidatePasswordResetCode_InputCode;
        public bool? ValidatePasswordResetCode_OutputOverride;
        public override Task<bool> ValidatePasswordResetCode(UserKey userKey,
            PasswordResetCode code)
        {
            this.ValidatePasswordResetCode_InputUserKey = userKey;
            this.ValidatePasswordResetCode_InputCode = code;

            return !this.ValidatePasswordResetCode_OutputOverride.HasValue
                ? base.ValidatePasswordResetCode(userKey, code)
                : Task.FromResult(this.ValidatePasswordResetCode_OutputOverride.Value);
        }

        public object RefreshSecurityStamp_InputUser;
        public override Task RefreshSecurityStamp<TUser>(TUser user)
        {
            this.RefreshSecurityStamp_InputUser = user;

            return base.RefreshSecurityStamp(user);
        }

        public object SetLastLogin_InputUser;
        public override Task SetLastLogin<TUser>(TUser user)
        {
            this.SetLastLogin_InputUser = user;

            return base.SetLastLogin(user);
        }
    }
}