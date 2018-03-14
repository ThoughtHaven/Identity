using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using ThoughtHaven.AspNetCore.Identity.Passwords;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Credentials
{
    public class EmailAndPasswordIdentityHelperServiceExtensionsTests
    {
        public class CreateMethod
        {
            public class IdentityAndEmailAndPasswordOverload
            {
                [Fact]
                public async Task NullEmail_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("email", async () =>
                    {
                        await Identity().Create(
                            email: null,
                            password: "password");
                    });
                }

                [Fact]
                public async Task NullPassword_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("password", async () =>
                    {
                        await Identity().Create(
                            email: "some@email.com",
                            password: null);
                    });
                }

                [Fact]
                public async Task RetrieveByEmailReturnsUser_ReturnsFailure()
                {
                    var identity = Identity();

                    var result = await identity.Create("some@email.com", "password");

                    Assert.False(result.Success);
                    Assert.Equal(identity.Helper.EmailNotAvailable, result.Failure);
                }

                [Fact]
                public async Task WhenCalled_CallsSetEmailOnHelper()
                {
                    var identity = Identity();
                    identity.Helper.FakeUserEmailStore.Retrieve_Output = null;

                    var result = await identity.Create("some@email.com", "password");

                    Assert.Equal("some@email.com", identity.Helper.SetEmail_InputEmail);
                }

                [Fact]
                public async Task WhenCalled_CallsSetPasswordHashOnHelper()
                {
                    var identity = Identity();
                    identity.Helper.FakeUserEmailStore.Retrieve_Output = null;

                    var result = await identity.Create("some@email.com", "password");

                    Assert.Equal("password", identity.Helper.SetPasswordHash_InputPassword);
                }

                [Fact]
                public async Task SetPasswordHashOnHelperReturnsFailure_ReturnsFailure()
                {
                    var identity = Identity();
                    identity.Helper.FakeUserEmailStore.Retrieve_Output = null;
                    identity.Helper.SetPasswordHash_OutputFailure = "Failure";

                    var result = await identity.Create("some@email.com", "password");

                    Assert.Equal(identity.Helper.SetPasswordHash_OutputFailure, result.Failure);
                }

                [Fact]
                public async Task WhenCalled_CallsCreateOnIdentity()
                {
                    var identity = Identity();
                    identity.Helper.FakeUserEmailStore.Retrieve_Output = null;

                    await identity.Create("some@email.com", "password");

                    Assert.Equal(identity.Helper.SetEmail_InputUser,
                        identity.Create_InputUser);
                }
            }
        }

        public class LoginMethod
        {
            public class IdentityAndEmailAndPasswordAndPropertiesOverload
            {
                [Fact]
                public async Task NullIdentity_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("identity", async () =>
                    {
                        await ((IIdentityService<User>)null).Login(
                            email: "some@email.com",
                            password: "password",
                            properties: Properties());
                    });
                }

                [Fact]
                public async Task NullEmail_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("email", async () =>
                    {
                        await Identity().Login(
                            email: null,
                            password: "password",
                            properties: Properties());
                    });
                }

                [Fact]
                public async Task NullPassword_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("password", async () =>
                    {
                        await Identity().Login(
                            email: "some@email.com",
                            password: null,
                            properties: Properties());
                    });
                }

                [Fact]
                public async Task NullProperties_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("properties", async () =>
                    {
                        await Identity().Login(
                            email: "some@email.com",
                            password: "password",
                            properties: null);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsIsLockedOutOnHelper()
                {
                    var identity = Identity();

                    await identity.Login("some@email.com", "password", Properties());

                    Assert.Equal("some@email.com", identity.Helper.IsLockedOut_InputKey);
                }

                [Fact]
                public async Task IsLockedOutOnHelperReturnsTrue_ReturnsFailure()
                {
                    var identity = Identity();
                    identity.Helper.IsLockedOut_OutputOverride = true;

                    var result = await identity.Login("some@email.com", "password",
                        Properties());

                    Assert.Equal(identity.Helper.LockedOut, result.Failure);
                }

                [Fact]
                public async Task WhenCalled_CallsRetrieveByEmailOnIdentity()
                {
                    var identity = Identity();

                    await identity.Login("some@email.com", "password", Properties());

                    Assert.Equal("some@email.com",
                        identity.Helper.FakeUserEmailStore.Retrieve_InputEmail);
                }

                [Fact]
                public async Task RetrieveByEmailOnIdentityReturnsNull_ReturnsFailure()
                {
                    var identity = Identity();
                    identity.Helper.FakeUserEmailStore.Retrieve_Output = null;

                    var result = await identity.Login("some@email.com", "password",
                        Properties());

                    Assert.Equal(identity.Helper.InvalidCredentials, result.Failure);
                }

                [Fact]
                public async Task WhenCalled_CallsValidatePasswordOnHelper()
                {
                    var identity = Identity();

                    await identity.Login("some@email.com", "password", Properties());

                    Assert.Equal(identity.Helper.FakeUserEmailStore.Retrieve_Output,
                        identity.Helper.ValidatePassword_InputUser);
                    Assert.Equal("password",
                        identity.Helper.ValidatePassword_InputPassword);
                }

                [Fact]
                public async Task ValidatePasswordOnHelperReturnsNotValid_ReturnsFailure()
                {
                    var identity = Identity();
                    identity.Helper.ValidatePassword_OutputOverride =
                        new PasswordValidateResult(valid: false);

                    var result = await identity.Login("some@email.com", "password",
                        Properties());

                    Assert.Equal(identity.Helper.InvalidCredentials, result.Failure);
                }

                [Fact]
                public async Task ValidatePasswordOnHelperReturnsUpdateHash_CallsSetPasswordHashOnHelper()
                {
                    var identity = Identity();
                    identity.Helper.ValidatePassword_OutputOverride =
                        new PasswordValidateResult(valid: true, updateHash: true);

                    await identity.Login("some@email.com", "password", Properties());

                    Assert.Equal(identity.Helper.ValidatePassword_InputUser,
                        identity.Helper.SetPasswordHash_InputUser);
                    Assert.Equal("password", identity.Helper.SetPasswordHash_InputPassword);
                }

                [Fact]
                public async Task ValidatePasswordOnHelperReturnsNotUpdateHash_DoesNotCallSetPasswordHashOnHelper()
                {
                    var identity = Identity();
                    identity.Helper.ValidatePassword_OutputOverride =
                        new PasswordValidateResult(valid: true, updateHash: false);

                    await identity.Login("some@email.com", "password", Properties());

                    Assert.Null(identity.Helper.SetPasswordHash_InputUser);
                    Assert.Null(identity.Helper.SetPasswordHash_InputPassword);
                }

                [Fact]
                public async Task WhenCalled_CallsLoginUserOnIdentity()
                {
                    var identity = Identity();
                    var properties = Properties();

                    await identity.Login("some@email.com", "password", properties);

                    Assert.Equal(identity.Helper.FakeUserEmailStore.Retrieve_Output,
                        identity.Login_InputUser);
                    Assert.Equal(properties, identity.Login_InputProperties);
                }

                [Fact]
                public async Task WhenCalled_ReturnsUser()
                {
                    var identity = Identity();

                    var result = await identity.Login("some@email.com", "password",
                        Properties());
                    
                    Assert.Equal(identity.Helper.FakeUserEmailStore.Retrieve_Output,
                        result.Value);
                }
            }

            public class IdentityAndEmailAndPasswordAndPersistentOverload
            {
                [Fact]
                public async Task NullIdentity_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("identity", async () =>
                    {
                        await ((IIdentityService<User>)null).Login(
                            email: "some@email.com",
                            password: "password");
                    });
                }

                [Fact]
                public async Task NullEmail_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("email", async () =>
                    {
                        await Identity().Login(
                            email: null,
                            password: "password");
                    });
                }

                [Fact]
                public async Task NullPassword_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("password", async () =>
                    {
                        await Identity().Login(
                            email: "some@email.com",
                            password: null);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsIsLockedOutOnHelper()
                {
                    var identity = Identity();

                    await identity.Login("some@email.com", "password");

                    Assert.Equal("some@email.com", identity.Helper.IsLockedOut_InputKey);
                }

                [Fact]
                public async Task IsLockedOutOnHelperReturnsTrue_ReturnsFailure()
                {
                    var identity = Identity();
                    identity.Helper.IsLockedOut_OutputOverride = true;

                    var result = await identity.Login("some@email.com", "password");

                    Assert.Equal(identity.Helper.LockedOut, result.Failure);
                }

                [Fact]
                public async Task WhenCalled_CallsRetrieveByEmailOnIdentity()
                {
                    var identity = Identity();

                    await identity.Login("some@email.com", "password");

                    Assert.Equal("some@email.com",
                        identity.Helper.FakeUserEmailStore.Retrieve_InputEmail);
                }

                [Fact]
                public async Task RetrieveByEmailOnIdentityReturnsNull_ReturnsFailure()
                {
                    var identity = Identity();
                    identity.Helper.FakeUserEmailStore.Retrieve_Output = null;

                    var result = await identity.Login("some@email.com", "password");

                    Assert.Equal(identity.Helper.InvalidCredentials, result.Failure);
                }

                [Fact]
                public async Task WhenCalled_CallsValidatePasswordOnHelper()
                {
                    var identity = Identity();

                    await identity.Login("some@email.com", "password");

                    Assert.Equal(identity.Helper.FakeUserEmailStore.Retrieve_Output,
                        identity.Helper.ValidatePassword_InputUser);
                    Assert.Equal("password",
                        identity.Helper.ValidatePassword_InputPassword);
                }

                [Fact]
                public async Task ValidatePasswordOnHelperReturnsFalse_ReturnsFailure()
                {
                    var identity = Identity();
                    identity.Helper.ValidatePassword_OutputOverride =
                        new PasswordValidateResult(valid: false);

                    var result = await identity.Login("some@email.com", "password");

                    Assert.Equal(identity.Helper.InvalidCredentials, result.Failure);
                }

                [Fact]
                public async Task DefaultPersistent_CallsLoginUserOnIdentityWithIsPersistentFalseOnProperties()
                {
                    var identity = Identity();

                    await identity.Login("some@email.com", "password");

                    Assert.Equal(identity.Helper.FakeUserEmailStore.Retrieve_Output,
                        identity.Login_InputUser);
                    Assert.False(identity.Login_InputProperties.IsPersistent);
                }

                [Fact]
                public async Task PersistentFalse_CallsLoginUserOnIdentityWithIsPersistentFalseOnProperties()
                {
                    var identity = Identity();

                    await identity.Login("some@email.com", "password", persistent: false);

                    Assert.Equal(identity.Helper.FakeUserEmailStore.Retrieve_Output,
                        identity.Login_InputUser);
                    Assert.False(identity.Login_InputProperties.IsPersistent);
                }

                [Fact]
                public async Task PersistentTrue_CallsLoginUserOnIdentityWithIsPersistentTrueOnProperties()
                {
                    var identity = Identity();

                    await identity.Login("some@email.com", "password", persistent: true);

                    Assert.Equal(identity.Helper.FakeUserEmailStore.Retrieve_Output,
                        identity.Login_InputUser);
                    Assert.True(identity.Login_InputProperties.IsPersistent);
                }

                [Fact]
                public async Task WhenCalled_ReturnsUser()
                {
                    var identity = Identity();

                    var result = await identity.Login("some@email.com", "password");

                    Assert.Equal(identity.Helper.FakeUserEmailStore.Retrieve_Output,
                        result.Value);
                }
            }
        }

        private static FakeIdentityService Identity() =>
            new FakeIdentityService(new FakeUserStore(), new FakeUserValidators(),
                new FakeClaimsAuthenticationService1(), new FakeClaimsConverter(),
                new FakeUserHelper1());
        private static AuthenticationProperties Properties() => new AuthenticationProperties();
    }
}