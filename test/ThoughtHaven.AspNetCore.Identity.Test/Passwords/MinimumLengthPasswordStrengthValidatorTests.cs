using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public class MinimumLengthPasswordStrengthValidatorTests
    {
        public class Type
        {
            [Fact]
            public void ImplementsIPasswordStrengthValidator()
            {
                Assert.True(typeof(IPasswordStrengthValidator)
                    .IsAssignableFrom(typeof(MinimumLengthPasswordStrengthValidator)));
            }
        }

        public class MinimumLengthProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void WhenCalled_Returns8ByDefault()
                {
                    var validator = new FakeMinimumLengthPasswordStrengthValidator();

                    Assert.Equal(8, validator.MinimumLength);
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
                    var validator = new MinimumLengthPasswordStrengthValidator();

                    await Assert.ThrowsAsync<ArgumentNullException>("password", async () =>
                    {
                        await validator.Validate(password: null);
                    });
                }

                [Fact]
                public async Task PassedEqualTo8Characters_ReturnsSuccess()
                {
                    var validator = new MinimumLengthPasswordStrengthValidator();

                    var result = await validator.Validate(new Password("12345678"));

                    Assert.True(result.Success);
                }

                [Fact]
                public async Task PasswordUnder8Characters_ReturnsExpectedFailure()
                {
                    var validator = new FakeMinimumLengthPasswordStrengthValidator();

                    var result = await validator.Validate(new Password("1234567"));

                    Assert.False(result.Success);
                    Assert.Equal(validator.InvalidPasswordStrength.Message,
                        result.Failure.Message);
                }
            }
        }
    }
}