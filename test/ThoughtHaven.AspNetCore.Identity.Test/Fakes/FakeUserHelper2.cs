using ThoughtHaven.AspNetCore.Identity.Passwords;
using ThoughtHaven.Data;
using ThoughtHaven.Messages.Emails;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeUserHelper2 : UserHelper<User>
    {
        public FakeUserHelper2(FakeUserEmailStore userEmailStore = null)
            : base(userEmailStore ?? new FakeUserEmailStore(), new FakeSingleUseTokenService(),
                  new FakeTimedLockoutStore(), new FakePasswordHasher(),
                  new IPasswordStrengthValidator[]
                  { new FakeMinimumLengthPasswordStrengthValidator() },
                  new FakeSystemClock())
        { }

        new public IRetrieveOperation<EmailAddress, TEmailUser> UserEmailStore<TEmailUser>() =>
            base.UserEmailStore<TEmailUser>();
    }
}