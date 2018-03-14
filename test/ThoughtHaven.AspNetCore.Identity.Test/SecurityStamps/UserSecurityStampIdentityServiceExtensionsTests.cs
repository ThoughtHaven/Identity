using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.SecurityStamps
{
    public class UserSecurityStampIdentityHelperServiceExtensionsTests
    {
        public class LogoutAsyncMethod
        {
            public class IdentityAndLogoutEverywhereOverload
            {
                [Fact]
                public async Task NullIdentity_Throws()
                {
                    IIdentityService<User> identity = null;

                    await Assert.ThrowsAsync<ArgumentNullException>("identity", async () =>
                    {
                        await identity.Logout(logoutEverywhere: false);
                    });
                }

                [Fact]
                public async Task LogoutEverywhereFalse_DoesNotCallAuthenticateOnIdentity()
                {
                    var identity = Identity();

                    await identity.Logout(logoutEverywhere: false);

                    Assert.False(identity.Authenticate_Called);
                }

                [Fact]
                public async Task LogoutEverywhereTrue_CallsAuthenticateOnIdentity()
                {
                    var identity = Identity();

                    await identity.Logout(logoutEverywhere: true);

                    Assert.True(identity.Authenticate_Called);
                }

                [Fact]
                public async Task AuthenticateOnIdentityReturnsUser_CallsRefreshSecurityStampOnHelper()
                {
                    var identity = Identity();

                    await identity.Logout(logoutEverywhere: true);

                    Assert.Equal(identity.UserStore.Retrieve_ByKey_Output,
                        identity.Helper.RefreshSecurityStamp_InputUser);
                }

                [Fact]
                public async Task AuthenticateOnIdentityReturnsUser_CallsUpdateOnIdentity()
                {
                    var identity = Identity();

                    await identity.Logout(logoutEverywhere: true);

                    Assert.Equal(identity.Helper.RefreshSecurityStamp_InputUser,
                        identity.Update_InputUser);
                }

                [Fact]
                public async Task LogoutEverywhereTrue_CallsLogoutOnIdentity()
                {
                    var identity = Identity();

                    await identity.Logout(logoutEverywhere: true);

                    Assert.True(identity.Logout_Called);
                }

                [Fact]
                public async Task LogoutEverywhereFalse_CallsLogoutOnIdentity()
                {
                    var identity = Identity();

                    await identity.Logout(logoutEverywhere: false);

                    Assert.True(identity.Logout_Called);
                }
            }
        }

        private static FakeIdentityService Identity() => new FakeIdentityService(
            new FakeUserStore(), new FakeUserValidators(),
            new FakeClaimsAuthenticationService1(), new FakeClaimsConverter(),
            new FakeUserHelper1());
    }
}