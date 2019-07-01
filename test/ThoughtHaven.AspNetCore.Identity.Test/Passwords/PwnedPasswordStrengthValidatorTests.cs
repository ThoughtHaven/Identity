using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public class PwnedPasswordStrengthValidatorTests
    {
        public class Type
        {
            [Fact]
            public void ImplementsIPasswordStrengthValidator()
            {
                Assert.True(typeof(IPasswordStrengthValidator).IsAssignableFrom(
                    typeof(PwnedPasswordStrengthValidator)));
            }
        }

        public class Constructor
        {
            public class PwnedOverload
            {
                [Fact]
                public void NullPwned_Throws()
                {
                    Assert.Throws<ArgumentNullException>("pwned", () =>
                    {
                        new PwnedPasswordStrengthValidator(pwned: null!);
                    });
                }
            }
        }

        public class ValidateMethod
        {
            public class PasswordOverload
            {
                [Fact]
                public async Task NullPassword_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("password", async () =>
                    {
                        await Validator().Validate(password: null!);
                    });
                }

                [Fact]
                public async Task PwnedReturnsTrue_ReturnsFailure()
                {
                    var pwned = Pwned();
                    pwned.HasPasswordBeenPwned_Output = true;

                    var result = await Validator(pwned).Validate("Password");

                    Assert.False(result.Success);
                    Assert.Equal("This password is insecure because it was exposed in an online data breach. You'll need a stronger password to protect your account. You can learn more at https://haveibeenpwned.com.",
                        result.Failure!.Message);
                }

                [Fact]
                public async Task PwnedReturnsFalse_ReturnsSuccess()
                {
                    var pwned = Pwned();
                    pwned.HasPasswordBeenPwned_Output = false;

                    var result = await Validator(pwned).Validate("Password");

                    Assert.True(result.Success);
                }
            }
        }

        private static FakePwnedPasswordClient Pwned() => new FakePwnedPasswordClient();
        private static PwnedPasswordStrengthValidator Validator(
            FakePwnedPasswordClient? pwned = null) =>
            new PwnedPasswordStrengthValidator(pwned ?? new FakePwnedPasswordClient());
    }
}