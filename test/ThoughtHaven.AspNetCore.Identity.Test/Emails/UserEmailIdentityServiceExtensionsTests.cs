using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Emails
{
    public class UserEmailIdentityServiceExtensionsTests
    {
        public class RetrieveMethod
        {
            public class IdentityAndEmailOverload
            {
                [Fact]
                public async Task NullIdentity_Throws()
                {
                    FakeIdentityService identity = null;

                    await Assert.ThrowsAsync<ArgumentNullException>("identity", async () =>
                    {
                        await identity.Retrieve(email: "some@email.com");
                    });
                }

                [Fact]
                public async Task NullEmail_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("email", async () =>
                    {
                        await Identity().Retrieve(email: null);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsRetrieveByEmailOnHelper()
                {
                    var identity = Identity();

                    await identity.Retrieve(email: "some@email.com");

                    Assert.Equal("some@email.com",
                        identity.Helper.FakeUserEmailStore.Retrieve_InputEmail);
                }

                [Fact]
                public async Task WhenCalled_ReturnsRetrieveByEmailOnHelper()
                {
                    var identity = Identity();

                    var result = await identity.Retrieve(email: "some@email.com");

                    Assert.Equal(identity.Helper.FakeUserEmailStore.Retrieve_Output, result);
                }
            }
        }

        public class UpdateEmailMethod
        {
            public class IdentityAndEmailOverload
            {
                [Fact]
                public async Task NullIdentity_Throws()
                {
                    FakeIdentityService identity = null;

                    await Assert.ThrowsAsync<ArgumentNullException>("identity", async () =>
                    {
                        await identity.UpdateEmail(
                            user: User(),
                            email: "some@email.com");
                    });
                }

                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await Identity().UpdateEmail(
                            user: null,
                            email: "some@email.com");
                    });
                }

                [Fact]
                public async Task NullEmail_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("email", async () =>
                    {
                        await Identity().UpdateEmail(
                            user: User(),
                            email: null);
                    });
                }

                [Fact]
                public async Task UserWithoutKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Identity().UpdateEmail(
                            user: new User() { Id = null },
                            email: "some@email.com");
                    });
                }

                [Fact]
                public async Task UserAlreadyOwnsEmail_ReturnsFailure()
                {
                    var user = User();
                    var identity = Identity();
                    identity.Helper.FakeUserEmailStore.Retrieve_Output.Id = user.Id;
                    identity.Helper.FakeUserEmailStore.Retrieve_Output.Email = user.Email;

                    var result = await identity.UpdateEmail(User(), user.Email);

                    Assert.Equal(identity.Helper.UserAlreadyOwnsEmail, result.Failure);
                }

                [Fact]
                public async Task DifferentUserOwnsEmail_ReturnsFailure()
                {
                    var user = User();
                    var identity = Identity();
                    identity.Helper.FakeUserEmailStore.Retrieve_Output.Id = "other-id";
                    identity.Helper.FakeUserEmailStore.Retrieve_Output.Email = user.Email;

                    var result = await identity.UpdateEmail(User(), user.Email);

                    Assert.Equal(identity.Helper.EmailNotAvailable, result.Failure);
                }

                [Fact]
                public async Task WhenCalled_CallsSetEmailOnHelper()
                {
                    var identity = Identity();
                    identity.Helper.FakeUserEmailStore.Retrieve_Output = null;
                    var user = User();

                    await identity.UpdateEmail(user, "some@email.com");

                    Assert.Equal(user, identity.Helper.SetEmail_InputUser);
                    Assert.Equal("some@email.com", identity.Helper.SetEmail_InputEmail);
                }

                [Fact]
                public async Task WhenCalled_CallsUpdateOnIdentity()
                {
                    var identity = Identity();
                    identity.Helper.FakeUserEmailStore.Retrieve_Output = null;
                    var user = User();

                    await identity.UpdateEmail(user, "some@email.com");

                    Assert.Equal(user, identity.Update_InputUser);
                }

                [Fact]
                public async Task WhenCalled_CallsCreateEmailVerificationCodeOnHelper()
                {
                    var identity = Identity();
                    identity.Helper.FakeUserEmailStore.Retrieve_Output = null;
                    var user = User();

                    await identity.UpdateEmail(user, "some@email.com");

                    Assert.Equal(user.Id,
                        identity.Helper.CreateEmailVerificationCode_InputUserKey);
                }

                [Fact]
                public async Task WhenCalled_ReturnsCreateEmailVerificationCodeOnHelper()
                {
                    var identity = Identity();
                    identity.Helper.FakeUserEmailStore.Retrieve_Output = null;
                    var user = User();

                    var result = await identity.UpdateEmail(user, "some@email.com");

                    Assert.Equal(identity.Helper.CreateEmailVerificationCode_Output,
                        result.Value);
                }
            }
        }

        public class ConfirmEmailMethod
        {
            public class IdentityAndUserAndCodeOverload
            {
                [Fact]
                public async Task NullIdentity_Throws()
                {
                    FakeIdentityService identity = null;

                    await Assert.ThrowsAsync<ArgumentNullException>("identity", async () =>
                    {
                        await identity.ConfirmEmail(
                            user: User(),
                            code: 1234);
                    });
                }

                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await Identity().ConfirmEmail(
                            user: null,
                            code: 1234);
                    });
                }

                [Fact]
                public async Task NullEmail_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("code", async () =>
                    {
                        await Identity().ConfirmEmail(
                            user: User(),
                            code: null);
                    });
                }

                [Fact]
                public async Task UserWithoutKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Identity().ConfirmEmail(
                            user: new User() { Id = null },
                            code: 1234);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsValidateEmailVericationCodeOnHelper()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.ConfirmEmail(user, 1234);

                    Assert.Equal(user.Id,
                        identity.Helper.ValidateEmailVerificationCode_InputUserKey);
                    Assert.Equal(1234,
                        identity.Helper.ValidateEmailVerificationCode_InputCode);
                }

                [Fact]
                public async Task ValidateEmailVericationCodeOnHelperReturnsFalse_ReturnsFailure()
                {
                    var identity = Identity();
                    identity.Helper.ValidateEmailVerificationCode_OutputOverride = false;
                    var user = User();

                    var result = await identity.ConfirmEmail(user, 1234);

                    Assert.Equal(identity.Helper.InvalidEmailVerificationCode, result.Failure);
                }

                [Fact]
                public async Task WhenCalled_CallsConfirmEmailOnHelper()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.ConfirmEmail(user, 1234);

                    Assert.Equal(user, identity.Helper.ConfirmEmail_InputUser);
                }

                [Fact]
                public async Task WhenCalled_CallsUpdateOnIdentity()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.ConfirmEmail(user, 1234);

                    Assert.Equal(user, identity.Update_InputUser);
                }

                [Fact]
                public async Task WhenCalled_ReturnsSuccess()
                {
                    var result = await Identity().ConfirmEmail(User(), 1234);

                    Assert.True(result.Success);
                }
            }
        }

        private static FakeIdentityService Identity() => new FakeIdentityService(
            new FakeUserStore(), new FakeUserValidators(),
            new FakeClaimsAuthenticationService1(), new FakeClaimsConverter(),
            new FakeUserHelper1());
        private static User User() => new User()
        {
            Id = "id",
            Email = "some@email.com",
        };
    }
}