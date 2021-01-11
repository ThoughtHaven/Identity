using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Claims
{
    public class ClaimsAuthenticationServiceBaseTests
    {
        public class Type
        {
            [Fact]
            public void ImplementsIAuthenticationService()
            {
                Assert.True(typeof(IAuthenticationService<ClaimsPrincipal>)
                    .IsAssignableFrom(typeof(ClaimsAuthenticationServiceBase<User>)));
            }
        }

        public class Constructor
        {
            [Fact]
            public void NullUserStore_Throws()
            {
                Assert.Throws<ArgumentNullException>("userStore", () =>
                {
                    new FakeClaimsAuthenticationServiceBase(
                        userStore: null!,
                        options: Options(),
                        clock: Clock());
                });
            }

            [Fact]
            public void NullOptions_Throws()
            {
                Assert.Throws<ArgumentNullException>("options", () =>
                {
                    new FakeClaimsAuthenticationServiceBase(
                        userStore: UserStore(),
                        options: null!,
                        clock: Clock());
                });
            }

            [Fact]
            public void NullClock_Throws()
            {
                Assert.Throws<ArgumentNullException>("clock", () =>
                {
                    new FakeClaimsAuthenticationServiceBase(
                        userStore: UserStore(),
                        options: Options(),
                        clock: null!);
                });
            }
        }

        public class AuthenticateMethod
        {
            public class EmptyOverload
            {
                [Fact]
                public async Task WhenCalled_CallsAuthenticateOnAbstract()
                {
                    var options = Options();
                    var authentication = Authentication(options: options);

                    await authentication.Authenticate();

                    Assert.Equal(options.AuthenticationScheme,
                        authentication.Authenticate_InputAuthenticationScheme);
                }

                [Fact]
                public async Task AuthenticateOnAbstractReturnsNull_ReturnsNull()
                {
                    var authentication = Authentication();
                    authentication.Authenticate_Output = null;

                    var result = await authentication.Authenticate();

                    Assert.Null(result);
                }

                [Fact]
                public async Task NoUserKeyClaim_CallsLogoutOnAbstract()
                {
                    var authentication = Authentication();
                    authentication.Authenticate_Output = Principal(hasUserKey: false);

                    await authentication.Authenticate();

                    Assert.True(authentication.Logout_Called);
                }

                [Fact]
                public async Task NoUserKeyClaim_ReturnsNull()
                {
                    var authentication = Authentication();
                    authentication.Authenticate_Output = Principal(hasUserKey: false);

                    var result = await authentication.Authenticate();

                    Assert.Null(result);
                }

                [Fact]
                public async Task NoSecurityStampClaim_CallsLogoutOnAbstract()
                {
                    var authentication = Authentication();
                    authentication.Authenticate_Output = Principal(hasSecurityStamp: false);

                    await authentication.Authenticate();

                    Assert.True(authentication.Logout_Called);
                }

                [Fact]
                public async Task NoSecurityStampClaim_ReturnsNull()
                {
                    var authentication = Authentication();
                    authentication.Authenticate_Output = Principal(hasSecurityStamp: false);

                    var result = await authentication.Authenticate();

                    Assert.Null(result);
                }

                [Fact]
                public async Task NoSecurityStampValidatedClaim_CallsLogoutOnAbstract()
                {
                    var authentication = Authentication();
                    authentication.Authenticate_Output = Principal(
                        hasSecurityStampValidated: false);

                    await authentication.Authenticate();

                    Assert.True(authentication.Logout_Called);
                }

                [Fact]
                public async Task NoSecurityStampValidatedClaim_ReturnsNull()
                {
                    var authentication = Authentication();
                    authentication.Authenticate_Output = Principal(
                        hasSecurityStampValidated: false);

                    var result = await authentication.Authenticate();

                    Assert.Null(result);
                }

                [Fact]
                public async Task SecurityStampValidatedRecently_DoesNotCallRetrieveOnStore()
                {
                    var store = UserStore();
                    var authentication = Authentication(store);

                    await authentication.Authenticate();

                    Assert.Null(store.Retrieve_ByKey_InputUserKey);
                }

                [Fact]
                public async Task SecurityStampValidatedRecently_ReturnsPrincipal()
                {
                    var authentication = Authentication();

                    var result = await authentication.Authenticate();

                    Assert.Equal(authentication.Authenticate_Output, result);
                }

                [Fact]
                public async Task SecurityStampValidatedOutsideInterval_CallsRetrieveOnUserStore()
                {
                    var store = UserStore();
                    var clock = Clock();
                    var authentication = Authentication(store, clock: clock);
                    authentication.Authenticate_Output = Principal(
                        clock: Clock(clock.UtcNow.ToOffset().AddDays(-1)));

                    await authentication.Authenticate();

                    Assert.Equal(authentication.Authenticate_OutputUserKey,
                        store.Retrieve_ByKey_InputUserKey);
                }

                [Fact]
                public async Task RetrieveOnUserStoreReturnsNull_CallsLogoutOnAbstract()
                {
                    var store = UserStore();
                    store.Retrieve_ByKey_Output = null;
                    var clock = Clock();
                    var authentication = Authentication(store, clock: clock);
                    authentication.Authenticate_Output = Principal(
                        clock: Clock(clock.UtcNow.ToOffset().AddDays(-1)));

                    await authentication.Authenticate();

                    Assert.True(authentication.Logout_Called);
                }

                [Fact]
                public async Task RetrieveOnUserStoreReturnsNull_ReturnsNull()
                {
                    var store = UserStore();
                    store.Retrieve_ByKey_Output = null;
                    var clock = Clock();
                    var authentication = Authentication(store, clock: clock);
                    authentication.Authenticate_Output = Principal(
                        clock: Clock(clock.UtcNow.ToOffset().AddDays(-1)));

                    var result = await authentication.Authenticate();

                    Assert.Null(result);
                }

                [Fact]
                public async Task AuthenticatedSecurityStampDoesNotMatch_CallsLogoutOnAbstract()
                {
                    var store = UserStore();
                    store.Retrieve_ByKey_Output!.SecurityStamp = "different";
                    var clock = Clock();
                    var authentication = Authentication(store, clock: clock);
                    authentication.Authenticate_Output = Principal(
                        clock: Clock(clock.UtcNow.ToOffset().AddDays(-1)));

                    await authentication.Authenticate();

                    Assert.True(authentication.Logout_Called);
                }

                [Fact]
                public async Task AuthenticatedSecurityStampDoesNotMatch_ReturnsNull()
                {
                    var store = UserStore();
                    store.Retrieve_ByKey_Output!.SecurityStamp = "different";
                    var clock = Clock();
                    var authentication = Authentication(store, clock: clock);
                    authentication.Authenticate_Output = Principal(
                        clock: Clock(clock.UtcNow.ToOffset().AddDays(-1)));

                    var result = await authentication.Authenticate();

                    Assert.Null(result);
                }

                [Fact]
                public async Task SecurityStampsMatch_CallsRefreshLoginOnAbstractWithUpdatedSecurityStampValidatedClaim()
                {
                    var options = Options();
                    var clock = Clock();
                    var authentication = Authentication(options: options, clock: clock);
                    authentication.Authenticate_Output = Principal(
                        clock: Clock(clock.UtcNow.ToOffset().AddDays(-1)));

                    await authentication.Authenticate();

                    Assert.NotNull(authentication.RefreshLogin_InputPrincipal);

                    var stampValidated = new DateTimeOffset(
                        ticks: long.Parse(authentication.RefreshLogin_InputPrincipal!.FindFirst(
                            options.ClaimTypes.SecurityStampValidated)!.Value),
                        offset: TimeSpan.Zero);

                    Assert.Equal(clock.UtcNow.ToOffset(), stampValidated);
                }

                [Fact]
                public async Task SecurityStampsMatch_ReturnsPrincipal()
                {
                    var options = Options();
                    var clock = Clock();
                    var authentication = Authentication(options: options, clock: clock);
                    authentication.Authenticate_Output = Principal(
                        clock: Clock(clock.UtcNow.ToOffset().AddDays(-1)));

                    var result = await authentication.Authenticate();

                    Assert.Equal(authentication.RefreshLogin_InputPrincipal, result);
                }
            }
        }

        private static ClaimsPrincipal Principal(bool hasUserKey = true,
            bool hasSecurityStamp = true, bool hasSecurityStampValidated = true,
            FakeSystemClock? clock = null) =>
            FakeClaimsAuthenticationServiceBase.Principal(hasUserKey, hasSecurityStamp,
                hasSecurityStampValidated, clock);
        private static FakeUserStore UserStore() => new FakeUserStore();
        private static ClaimOptions Options() => new ClaimOptions();
        private static FakeSystemClock Clock(DateTimeOffset? utcNow = null) =>
            new FakeSystemClock(utcNow ?? DateTimeOffset.UtcNow);
        private static FakeClaimsAuthenticationServiceBase Authentication(
            FakeUserStore? userStore = null, ClaimOptions? options = null,
            FakeSystemClock? clock = null) =>
            new FakeClaimsAuthenticationServiceBase(userStore ?? UserStore(),
                options ?? Options(), clock ?? Clock());
    }
}