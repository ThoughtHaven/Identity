using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using ThoughtHaven.AspNetCore.Identity.Keys;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public class UserPasswordHashHelperTests
    {
        public class InvalidPasswordProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void WhenCalled_ReturnsExpectedMessage()
                {
                    UserHelper helper = new FakeUserHelper1();

                    Assert.Equal("That password wasn't right.", helper.InvalidPassword.Message);
                }
            }
        }

        public class InvalidPasswordResetCodeProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void WhenCalled_ReturnsExpectedMessage()
                {
                    UserHelper helper = new FakeUserHelper1();

                    Assert.Equal("That password reset code wasn't right. It may have expired.",
                        helper.InvalidPasswordResetCode.Message);
                }
            }
        }

        public class SetPasswordHashMethod
        {
            public class UserAndPasswordOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    UserHelper helper = new FakeUserHelper1();

                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await helper.SetPasswordHash<User>(
                            user: null!,
                            password: new Password("ValidPassword"));
                    });
                }

                [Fact]
                public async Task NullPassword_Throws()
                {
                    UserHelper helper = new FakeUserHelper1();

                    await Assert.ThrowsAsync<ArgumentNullException>("password", async () =>
                    {
                        await helper.SetPasswordHash(
                            user: new User(),
                            password: null!);
                    });
                }

                [Fact]
                public async Task ValidPassword_ReturnsSuccess()
                {
                    var helper = new FakeUserHelper1();

                    var result = await helper.SetPasswordHash(
                        new User(),
                        new Password("ValidPassword"));

                    Assert.True(result.Success);
                }

                [Fact]
                public async Task ValidPassword_CallsPasswordHasher()
                {
                    var helper = new FakeUserHelper1();
                    var password = new Password("password");
                    var user = new User();

                    _ = await helper.SetPasswordHash(user, password);

                    Assert.Equal(password, helper.FakePasswordHasher.Hash_InputPassword);
                    Assert.Equal(helper.FakePasswordHasher.Hash_Output,
                        user.PasswordHash);
                }

                [Fact]
                public async Task InvalidPasswordStrength_ReturnsFailure()
                {
                    var helper = new FakeUserHelper1();

                    var result = await helper.SetPasswordHash(new User(),
                        new Password("1234567"));

                    Assert.False(result.Success);
                    Assert.Equal(
                        new FakeMinimumLengthPasswordStrengthValidator().InvalidPasswordStrength.Message,
                        result.Failure!.Message);
                }
            }
        }

        public class ValidatePasswordMethod
        {
            public class UserAndPasswordOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await new FakeUserHelper1().ValidatePassword<User>(
                            user: null!,
                            password: new Password("ValidPassword"));
                    });
                }

                [Fact]
                public async Task NullPassword_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("password", async () =>
                    {
                        await new FakeUserHelper1().ValidatePassword<User>(
                            user: new User()
                            {
                                PasswordHash = "Hash",
                            },
                            password: null!);
                    });
                }

                [Fact]
                public async Task NullPasswordHash_Throws()
                {
                    UserHelper helper = new FakeUserHelper1();

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await new FakeUserHelper1().ValidatePassword<User>(
                            user: new User()
                            {
                                PasswordHash = null,
                            },
                            password: new Password("Password"));
                    });
                }

                [Fact]
                public async Task EmptyPasswordHash_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await new FakeUserHelper1().ValidatePassword<User>(
                            user: new User()
                            {
                                PasswordHash = string.Empty,
                            },
                            password: new Password("Password"));
                    });
                }

                [Fact]
                public async Task WhiteSpacePasswordHash_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await new FakeUserHelper1().ValidatePassword<User>(
                            user: new User()
                            {
                                PasswordHash = " ",
                            },
                            password: new Password("Password"));
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsValidateOnPasswordHasher()
                {
                    var helper = new FakeUserHelper1();
                    helper.FakePasswordHasher.Validate_Output =
                        new PasswordValidateResult(valid: true);
                    var user = new User()
                    {
                        PasswordHash = "hash"
                    };

                    await helper.ValidatePassword(user, "password");

                    Assert.Equal(user.PasswordHash,
                        helper.FakePasswordHasher.Validate_InputHash);
                    Assert.Equal("password",
                        helper.FakePasswordHasher.Validate_InputPassword);
                }

                [Fact]
                public async Task WhenCalled_ReturnsValidateOnPasswordHasher()
                {
                    var helper = new FakeUserHelper1();
                    helper.FakePasswordHasher.Validate_Output =
                        new PasswordValidateResult(valid: true);
                    var user = new User()
                    {
                        PasswordHash = "hash"
                    };

                    var result = await helper.ValidatePassword(user, "password");

                    Assert.Equal(helper.FakePasswordHasher.Validate_Output, result);
                }
            }
        }

        public class CreatePasswordResetCodeMethod
        {
            public class UserKeyOverload
            {
                [Fact]
                public async Task NullUserKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("userKey", async () =>
                    {
                        await new FakeUserHelper1().CreatePasswordResetCode(userKey: null!);
                    });
                }

                [Fact]
                public async Task ValidUserKey_ReturnsResetCode()
                {
                    var userKey = new UserKey("key");

                    var code = await new FakeUserHelper1().CreatePasswordResetCode(userKey);

                    Assert.NotNull(code);
                }

                [Fact]
                public async Task ValidUserKey_Returns6CharacterResetCode()
                {
                    var userKey = new UserKey("key");

                    var code = await new FakeUserHelper1().CreatePasswordResetCode(userKey);

                    Assert.Equal(6, code.Value.ToString().Length);
                }

                [Fact]
                public async Task ValidUserKey_AddsSingleUseTokenToStore()
                {
                    var helper = new FakeUserHelper1();
                    var userKey = new UserKey("key");

                    var code = await helper.CreatePasswordResetCode(userKey);

                    Assert.True(helper.FakeSingleUseTokenService.Create_Called);
                    Assert.Equal(helper.FakeSingleUseTokenService.Create_InputToken!.Value,
                        $"pw-key-{code.Value}");
                }

                [Fact]
                public async Task ValidUserKey_SetsExpirationTo1Day()
                {
                    var helper = new FakeUserHelper1();
                    var userKey = new UserKey("key");

                    _ = await helper.CreatePasswordResetCode(userKey);

                    Assert.Equal(helper.FakeClock.UtcNow.AddDays(1),
                        helper.FakeSingleUseTokenService.Create_InputExpiration);
                }
            }
        }

        public class ValidatePasswordResetCodeMethod
        {
            public class UserKeyAndCodeOverload
            {
                [Fact]
                public async Task NullUserKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("userKey", async () =>
                    {
                        await new FakeUserHelper1().ValidatePasswordResetCode(
                            userKey: null!,
                            code: 1234);
                    });
                }

                [Fact]
                public async Task NullCode_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("code", async () =>
                    {
                        await new FakeUserHelper1().ValidatePasswordResetCode(
                            userKey: "key",
                            code: null!);
                    });
                }

                [Fact]
                public async Task ValidCode_ReturnsTrue()
                {
                    var helper = new FakeUserHelper1();

                    Assert.True(await helper.ValidatePasswordResetCode("key", 1234));
                }

                [Fact]
                public async Task InvalidCode_ReturnsFalse()
                {
                    var helper = new FakeUserHelper1();
                    helper.FakeSingleUseTokenService.Validate_Output = false;

                    Assert.False(await helper.ValidatePasswordResetCode("key", 1234));
                }
            }
        }
    }
}