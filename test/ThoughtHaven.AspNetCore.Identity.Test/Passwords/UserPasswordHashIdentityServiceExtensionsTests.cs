using System;
using Xunit;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Fakes;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public class UserPasswordHashIdentityServiceExtensionsTests
    {
        public class ValidatePasswordMethod
        {
            public class IdentityAndUserAndPasswordOverload
            {
                [Fact]
                public async Task NullIdentity_Throws()
                {
                    FakeIdentityService? identity = null;

                    await Assert.ThrowsAsync<ArgumentNullException>("identity", async () =>
                    {
                        await identity!.ValidatePassword(
                            user: User(),
                            password: "password");
                    });
                }

                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await Identity().ValidatePassword(
                            user: null!,
                            password: "password");
                    });
                }

                [Fact]
                public async Task NullPassword_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("password", async () =>
                    {
                        await Identity().ValidatePassword(
                            user: User(),
                            password: null!);
                    });
                }

                [Fact]
                public async Task NullUserPasswordHash_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Identity().ValidatePassword(
                            user: new User() { PasswordHash = null },
                            password: "password");
                    });
                }

                [Fact]
                public async Task EmptyUserPasswordHash_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Identity().ValidatePassword(
                            user: new User() { PasswordHash = "" },
                            password: "password");
                    });
                }

                [Fact]
                public async Task WhiteSpaceUserPasswordHash_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Identity().ValidatePassword(
                            user: new User() { PasswordHash = " " },
                            password: "password");
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsValidatePasswordOnHelper()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.ValidatePassword(user, "password");

                    Assert.Equal(user, identity.Helper.ValidatePassword_InputUser);
                    Assert.Equal("password", identity.Helper.ValidatePassword_InputPassword);
                }

                [Fact]
                public async Task ValidatePasswordOnHelperReturnsNotValid_ReturnsFailure()
                {
                    var identity = Identity();
                    identity.Helper.ValidatePassword_OutputOverride =
                        new PasswordValidateResult(valid: false);
                    var user = User();

                    var result = await identity.ValidatePassword(user, "password");

                    Assert.Equal(identity.Helper.InvalidPassword, result.Failure);
                }

                [Fact]
                public async Task ValidatePasswordOnHelperReturnsUpdateHash_CallsSetPasswordHashOnHelper()
                {
                    var identity = Identity();
                    identity.Helper.ValidatePassword_OutputOverride =
                        new PasswordValidateResult(valid: true, updateHash: true);
                    var user = User();

                    await identity.ValidatePassword(user, "password");

                    Assert.Equal(user, identity.Helper.SetPasswordHash_InputUser);
                    Assert.Equal("password", identity.Helper.SetPasswordHash_InputPassword);
                }

                [Fact]
                public async Task ValidatePasswordOnHelperReturnsUpdateHash_CallsUpdateOnIdentity()
                {
                    var identity = Identity();
                    identity.Helper.ValidatePassword_OutputOverride =
                        new PasswordValidateResult(valid: true, updateHash: true);
                    var user = User();

                    await identity.ValidatePassword(user, "password");

                    Assert.Equal(user, identity.Update_InputUser);
                }

                [Fact]
                public async Task ValidatePasswordOnHelperReturnsNotUpdateHash_DoesNotCallSetPasswordHashOnHelper()
                {
                    var identity = Identity();
                    identity.Helper.ValidatePassword_OutputOverride =
                        new PasswordValidateResult(valid: true, updateHash: false);
                    var user = User();

                    await identity.ValidatePassword(user, "password");

                    Assert.Null(identity.Helper.SetPasswordHash_InputUser);
                    Assert.Null(identity.Helper.SetPasswordHash_InputPassword);
                }

                [Fact]
                public async Task ValidatePasswordOnHelperReturnsNotUpdateHash_DoesNotCallUpdateOnIdentity()
                {
                    var identity = Identity();
                    identity.Helper.ValidatePassword_OutputOverride =
                        new PasswordValidateResult(valid: true, updateHash: false);
                    var user = User();

                    await identity.ValidatePassword(user, "password");

                    Assert.Null(identity.Update_InputUser);
                }

                [Fact]
                public async Task WhenCalled_ReturnsSuccess()
                {
                    var result = await Identity().ValidatePassword(User(), "password");

                    Assert.True(result.Success);
                }
            }
        }

        public class ForgotPasswordMethod
        {
            public class IdentityAndUserOverload
            {
                [Fact]
                public async Task NullIdentity_Throws()
                {
                    FakeIdentityService? identity = null;

                    await Assert.ThrowsAsync<ArgumentNullException>("identity", async () =>
                    {
                        await identity!.ForgotPassword(user: User());
                    });
                }

                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await Identity().ForgotPassword(user: null!);
                    });
                }

                [Fact]
                public async Task NullUserKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Identity().ForgotPassword(user: new User() { Id = null });
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsCreatePasswordResetCodeOnHelper()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.ForgotPassword(user);

                    Assert.Equal(user.Key(),
                        identity.Helper.CreatePasswordResetCode_InputUserKey);
                }

                [Fact]
                public async Task WhenCalled_ReturnsCreatePasswordResetCodeOnHelper()
                {
                    var identity = Identity();
                    var user = User();

                    var result = await identity.ForgotPassword(user);

                    Assert.Equal(identity.Helper.CreatePasswordResetCode_Output, result.Value);
                }
            }
        }

        public class UpdatePasswordMethod
        {
            public class IdentityAndUserAndCurrentAndUpdatedOverload
            {
                [Fact]
                public async Task NullIdentity_Throws()
                {
                    FakeIdentityService? identity = null;

                    await Assert.ThrowsAsync<ArgumentNullException>("identity", async () =>
                    {
                        await identity!.UpdatePassword(
                            user: User(),
                            current: "current",
                            updated: "updated");
                    });
                }

                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await Identity().UpdatePassword(
                            user: null!,
                            current: "current",
                            updated: "updated");
                    });
                }

                [Fact]
                public async Task NullCurrent_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("current", async () =>
                    {
                        await Identity().UpdatePassword(
                            user: User(),
                            current: null!,
                            updated: "updated");
                    });
                }

                [Fact]
                public async Task NullUpdated_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("updated", async () =>
                    {
                        await Identity().UpdatePassword(
                            user: User(),
                            current: "current",
                            updated: null!);
                    });
                }

                [Fact]
                public async Task NullUserPasswordHash_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Identity().UpdatePassword(
                            user: new User() { PasswordHash = null },
                            current: "current",
                            updated: "updated");
                    });
                }

                [Fact]
                public async Task EmptyUserPasswordHash_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Identity().UpdatePassword(
                            user: new User() { PasswordHash = "" },
                            current: "current",
                            updated: "updated");
                    });
                }

                [Fact]
                public async Task WhiteSpaceUserPasswordHash_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Identity().UpdatePassword(
                            user: new User() { PasswordHash = " " },
                            current: "current",
                            updated: "updated");
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsValidatePasswordOnIdentity()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.UpdatePassword(user, "current", "updated");

                    Assert.Equal(user, identity.Helper.ValidatePassword_InputUser);
                    Assert.Equal("current", identity.Helper.ValidatePassword_InputPassword);
                }

                [Fact]
                public async Task ValidatePasswordOnIdentityReturnsFailure_ReturnsFailure()
                {
                    var identity = Identity();
                    identity.Helper.ValidatePassword_OutputOverride =
                        new PasswordValidateResult(valid: false);
                    var user = User();

                    var result = await identity.UpdatePassword(user, "current", "updated");

                    Assert.Equal(identity.Helper.InvalidPassword, result.Failure);
                }

                [Fact]
                public async Task WhenCalled_CallsSetPasswordHashOnHelper()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.UpdatePassword(user, "current", "updated");

                    Assert.Equal(user, identity.Helper.SetPasswordHash_InputUser);
                    Assert.Equal("updated", identity.Helper.SetPasswordHash_InputPassword);
                }

                [Fact]
                public async Task SetPasswordHashOnHelperReturnsFailure_ReturnsFailure()
                {
                    var identity = Identity();
                    identity.Helper.SetPasswordHash_OutputFailure = "Failure";
                    var user = User();

                    var result = await identity.UpdatePassword(user, "current", "updated");

                    Assert.Equal(identity.Helper.SetPasswordHash_OutputFailure, result.Failure);
                }

                [Fact]
                public async Task WhenCalled_CallsRefreshSecurityStampOnHelper()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.UpdatePassword(user, "current", "valid-updated");

                    Assert.Equal(user, identity.Helper.RefreshSecurityStamp_InputUser);
                }

                [Fact]
                public async Task WhenCalled_CallsUpdateOnIdentity()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.UpdatePassword(user, "current", "valid-updated");

                    Assert.Equal(user, identity.Update_InputUser);
                }

                [Fact]
                public async Task WhenCalled_ReturnsSuccess()
                {
                    var identity = Identity();
                    var user = User();

                    var result = await identity.UpdatePassword(user, "current",
                        "valid-updated");

                    Assert.True(result.Success);
                }
            }

            public class IdentityAndUserAndCodeAndPasswordOverload
            {
                [Fact]
                public async Task NullIdentity_Throws()
                {
                    FakeIdentityService? identity = null;

                    await Assert.ThrowsAsync<ArgumentNullException>("identity", async () =>
                    {
                        await identity!.UpdatePassword(
                            user: User(),
                            code: 1234,
                            password: "password");
                    });
                }

                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await Identity().UpdatePassword(
                            user: null!,
                            code: 1234,
                            password: "password");
                    });
                }

                [Fact]
                public async Task NullCode_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("code", async () =>
                    {
                        await Identity().UpdatePassword(
                            user: User(),
                            code: null!,
                            password: "password");
                    });
                }

                [Fact]
                public async Task NullPassword_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("password", async () =>
                    {
                        await Identity().UpdatePassword(
                            user: User(),
                            code: 1234,
                            password: null!);
                    });
                }

                [Fact]
                public async Task NullUserKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Identity().UpdatePassword(
                            user: new User() { Id = null },
                            code: 1234,
                            password: "password");
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsValidatePasswordResetCodeOnHelper()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.UpdatePassword(user, 1234, "password");

                    Assert.Equal(user.Key(),
                        identity.Helper.ValidatePasswordResetCode_InputUserKey);
                    Assert.Equal(1234, identity.Helper.ValidatePasswordResetCode_InputCode);
                }

                [Fact]
                public async Task ValidatePasswordResetCodeOnHelperReturnsFalse_ReturnsFailure()
                {
                    var identity = Identity();
                    identity.Helper.ValidatePasswordResetCode_OutputOverride = false;
                    var user = User();

                    var result = await identity.UpdatePassword(user, 1234, "password");

                    Assert.Equal(identity.Helper.InvalidPasswordResetCode, result.Failure);
                }

                [Fact]
                public async Task WhenCalled_CallsSetPasswordHashOnHelper()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.UpdatePassword(user, 1234, "password");

                    Assert.Equal(user, identity.Helper.SetPasswordHash_InputUser);
                    Assert.Equal("password", identity.Helper.SetPasswordHash_InputPassword);
                }

                [Fact]
                public async Task SetPasswordHashOnHelperReturnsFailure_ReturnsFailure()
                {
                    var identity = Identity();
                    identity.Helper.SetPasswordHash_OutputFailure = "Failure";
                    var user = User();

                    var result = await identity.UpdatePassword(user, 1234, "password");

                    Assert.Equal(identity.Helper.SetPasswordHash_OutputFailure, result.Failure);
                }

                [Fact]
                public async Task WhenCalled_CallsRefreshSecurityStampOnHelper()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.UpdatePassword(user, 1234, "password");

                    Assert.Equal(user, identity.Helper.RefreshSecurityStamp_InputUser);
                }

                [Fact]
                public async Task WhenCalled_CallsUpdateOnIdentity()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.UpdatePassword(user, 1234, "password");

                    Assert.Equal(user, identity.Update_InputUser);
                }

                [Fact]
                public async Task WhenCalled_ReturnsSuccess()
                {
                    var result = await Identity().UpdatePassword(User(), 1234, "password");

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
            PasswordHash = "hash",
        };
    }
}