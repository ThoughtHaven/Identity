using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using ThoughtHaven.AspNetCore.Identity.Internal;
using ThoughtHaven.AspNetCore.Identity.Keys;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Emails
{
    public class UserEmailHelperTests
    {
        public class UserAlreadyOwnsEmailProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void WhenCalled_ReturnsExpectedMessage()
                {
                    UserHelper helper = new FakeUserHelper1();

                    Assert.Equal("This email is already associated with this account.",
                        helper.UserAlreadyOwnsEmail.Message);
                }
            }
        }

        public class EmailNotAvailableProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void WhenCalled_ReturnsExpectedMessage()
                {
                    UserHelper helper = new FakeUserHelper1();

                    Assert.Equal("This email is not available. Is there a different one you can use?",
                        helper.EmailNotAvailable.Message);
                }
            }
        }

        public class InvalidEmailVerificationTokenProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void WhenCalled_ReturnsExpectedMessage()
                {
                    UserHelper helper = new FakeUserHelper1();

                    Assert.Equal("We couldn't confirm your email. The link we sent you may have expired. You'll need to try confirming your email again.",
                        helper.InvalidEmailVerificationCode.Message);
                }
            }
        }

        public class RetrieveMethod
        {
            public class EmailOverload
            {
                [Fact]
                public async Task NullEmail_Throws()
                {
                    UserHelper helper = new FakeUserHelper1();

                    await Assert.ThrowsAsync<ArgumentNullException>("email", async () =>
                    {
                        await helper.Retrieve<User>(email: null!);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsUserEmailStore()
                {
                    var helper = new FakeUserHelper1();

                    await helper.Retrieve<User>(email: "some@email.com");

                    Assert.True(helper.FakeUserEmailStore.Retrieve_Called);
                }

                [Fact]
                public async Task UserEmailStoreReturnsNull_ReturnsNull()
                {
                    var helper = new FakeUserHelper1();
                    helper.FakeUserEmailStore.Retrieve_Output = null;

                    var user = await helper.Retrieve<User>("some@email.com");

                    Assert.Null(user);
                }

                [Fact]
                public async Task UserEmailStoreReturnsUser_ReturnsUser()
                {
                    var helper = new FakeUserHelper1();

                    var user = await helper.Retrieve<User>("some@email.com");

                    Assert.Equal(helper.FakeUserEmailStore.Retrieve_Output, user);
                }
            }
        }

        public class SetEmailMethod
        {
            public class UserAndEmailOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    UserHelper helper = new FakeUserHelper1();

                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await helper.SetEmail(
                            user: (User)null!,
                            email: "some@email.com");
                    });
                }

                [Fact]
                public async Task NullEmail_Throws()
                {
                    UserHelper helper = new FakeUserHelper1();

                    await Assert.ThrowsAsync<ArgumentNullException>("email", async () =>
                    {
                        await helper.SetEmail(
                            user: new User(),
                            email: null!);
                    });
                }

                [Fact]
                public async Task ValidEmail_SetsEmailToUppercaseInvariant()
                {
                    var user = new User();

                    await new FakeUserHelper1().SetEmail(user, "some@email.com");

                    Assert.Equal("SOME@EMAIL.COM", user.Email);
                }

                [Fact]
                public async Task ValidEmail_SetsEmailConfirmedToFalse()
                {
                    var user = new User()
                    {
                        EmailConfirmed = true
                    };

                    await new FakeUserHelper1().SetEmail(user, email: "some@email.com");

                    Assert.False(user.EmailConfirmed);
                }
            }
        }

        public class ConfirmEmailMethod
        {
            public class UserOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await new FakeUserHelper1().ConfirmEmail(user: (User)null!);
                    });
                }

                [Fact]
                public async Task UserWithNullEmail_Throws()
                {
                    var user = new User()
                    {
                        Email = null,
                        EmailConfirmed = false,
                    };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await new FakeUserHelper1().ConfirmEmail(user);
                    });
                }

                [Fact]
                public async Task UserWithEmptyEmail_Throws()
                {
                    var user = new User()
                    {
                        Email = string.Empty,
                        EmailConfirmed = false,
                    };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await new FakeUserHelper1().ConfirmEmail(user);
                    });
                }

                [Fact]
                public async Task UserWithWhiteSpaceEmail_Throws()
                {
                    var user = new User()
                    {
                        Email = " ",
                        EmailConfirmed = false,
                    };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await new FakeUserHelper1().ConfirmEmail(user);
                    });
                }

                [Fact]
                public async Task ConfirmedFalse_SetsEmailConfirmedToTrue()
                {
                    var user = new User()
                    {
                        Email = "some@email.com",
                        EmailConfirmed = false,
                    };

                    await new FakeUserHelper1().ConfirmEmail(user);

                    Assert.True(user.EmailConfirmed);
                }

                [Fact]
                public async Task ConfirmedTrue_SetsEmailConfirmedToTrue()
                {
                    var user = new User()
                    {
                        Email = "some@email.com",
                        EmailConfirmed = true,
                    };

                    await new FakeUserHelper1().ConfirmEmail(user);

                    Assert.True(user.EmailConfirmed);
                }
            }
        }

        public class CreateEmailVerificationCodeMethod
        {
            public class UserKeyOverload
            {
                [Fact]
                public async Task NullUserKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("userKey", async () =>
                    {
                        await new FakeUserHelper1().CreateEmailVerificationCode(userKey: null!);
                    });
                }

                [Fact]
                public async Task WhenCalled_ReturnsVerificationCode()
                {
                    var helper = new FakeUserHelper1();
                    var userKey = new UserKey("1");

                    var token = await helper.CreateEmailVerificationCode(userKey);

                    Assert.NotNull(token);
                }

                [Fact]
                public async Task WhenCalled_Returns6DigitVerificationToken()
                {
                    var userKey = new UserKey("1");

                    var token = await new FakeUserHelper1().CreateEmailVerificationCode(userKey);

                    Assert.Equal(6, token.Value.ToString().Length);
                }

                [Fact]
                public async Task WhenCalled_CallsCreateOnTokenService()
                {
                    var helper = new FakeUserHelper1();
                    var userKey = new UserKey("1");

                    var token = await helper.CreateEmailVerificationCode(userKey);

                    Assert.True(helper.FakeSingleUseTokenService.Create_Called);
                    Assert.Equal(helper.FakeSingleUseTokenService.Create_InputToken!.Value,
                        $"em-1-{token.Value}");
                }

                [Fact]
                public async Task WhenCalled_SetsExpirationTo7Days()
                {
                    var helper = new FakeUserHelper1();
                    var userKey = new UserKey("1");

                    _ = await helper.CreateEmailVerificationCode(userKey);

                    Assert.Equal(helper.FakeClock.UtcNow.ToOffset().AddDays(7),
                        helper.FakeSingleUseTokenService.Create_InputExpiration);
                }
            }
        }

        public class ValidateEmailVerificationCodeMethod
        {
            public class UserKeyAndCodeOverload
            {
                [Fact]
                public async Task NullUserKey_Throws()
                {
                    UserHelper helper = new FakeUserHelper1();

                    await Assert.ThrowsAsync<ArgumentNullException>("userKey", async () =>
                    {
                        await helper.ValidateEmailVerificationCode(
                            userKey: null!,
                            code: 1234);
                    });
                }

                [Fact]
                public async Task NullCode_Throws()
                {
                    UserHelper helper = new FakeUserHelper1();

                    await Assert.ThrowsAsync<ArgumentNullException>("code", async () =>
                    {
                        await helper.ValidateEmailVerificationCode(
                            userKey: "1",
                            code: null!);
                    });
                }

                [Fact]
                public async Task WhenCalled_ReturnsTrue()
                {
                    var helper = new FakeUserHelper1();
                    helper.FakeSingleUseTokenService.Validate_Output = true;

                    var result = await helper.ValidateEmailVerificationCode(
                        new UserKey("1"), 1111);

                    Assert.True(result);
                }

                [Fact]
                public async Task InvalidToken_ReturnsFalse()
                {
                    var helper = new FakeUserHelper1();
                    helper.FakeSingleUseTokenService.Validate_Output = false;

                    var result = await helper.ValidateEmailVerificationCode(
                        new UserKey("1"), 1111);

                    Assert.False(result);
                }
            }
        }
    }
}