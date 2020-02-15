using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Claims
{
    public class ClaimsAuthenticationServiceTests
    {
        public class Type
        {
            [Fact]
            public void ImplementsClaimsAuthenticationServiceBase()
            {
                Assert.True(typeof(ClaimsAuthenticationServiceBase<User>).IsAssignableFrom(
                    typeof(ClaimsAuthenticationService<User>)));
            }
        }

        public class Constructor
        {
            public class PrimaryOverload
            {
                [Fact]
                public void NullHttpContextAccessor_Throws()
                {
                    Assert.Throws<ArgumentNullException>("httpContextAccessor", () =>
                    {
                        new ClaimsAuthenticationService<User>(
                            httpContextAccessor: null!,
                            userStore: UserStore(),
                            options: Options(),
                            clock: Clock());
                    });
                }

                [Fact]
                public void NullUserStore_Throws()
                {
                    Assert.Throws<ArgumentNullException>("userStore", () =>
                    {
                        new ClaimsAuthenticationService<User>(
                            httpContextAccessor: HttpContextAccessor(),
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
                        new ClaimsAuthenticationService<User>(
                            httpContextAccessor: HttpContextAccessor(),
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
                        new ClaimsAuthenticationService<User>(
                            httpContextAccessor: HttpContextAccessor(),
                            userStore: UserStore(),
                            options: Options(),
                            clock: null!);
                    });
                }
            }
        }

        public class LoginMethod
        {
            public class PrincipalAndPropertiesOverload
            {
                [Fact]
                public async Task NullPrincipal_Throw()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("principal", async () =>
                    {
                        await Authentication().Login(
                            principal: null!,
                            properties: Properties());
                    });
                }

                [Fact]
                public async Task NullProperties_Throw()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("properties", async () =>
                    {
                        await Authentication().Login(
                            principal: Principal(),
                            properties: null!);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsSignInOnHttpContext()
                {
                    var context = HttpContextAccessor();
                    var options = Options();
                    var principal = Principal();
                    var properties = Properties();

                    await Authentication(context, options: options).Login(principal,
                        properties);

                    Assert.Equal(context.Authentication.SignInAsync_InputContext,
                        context.HttpContext);
                    Assert.Equal(context.Authentication.SignInAsync_InputScheme,
                        options.AuthenticationScheme);
                    Assert.Equal(context.Authentication.SignInAsync_InputPrincipal,
                        principal);
                    Assert.Equal(context.Authentication.SignInAsync_InputProperties,
                        properties);
                }

                [Fact]
                public async Task PropertiesAllowRefreshNotSet_SetsAllowRefresh()
                {
                    var context = HttpContextAccessor();
                    var options = Options();
                    var principal = Principal();
                    var properties = Properties();

                    await Authentication(context, options: options).Login(principal,
                        properties);

                    Assert.True(properties.AllowRefresh);
                    Assert.True(
                        context.Authentication.SignInAsync_InputProperties!.AllowRefresh);
                }

                [Fact]
                public async Task PropertiesIssuedUtcNotSet_SetsIssuedUtc()
                {
                    var now = DateTimeOffset.UtcNow;

                    var context = HttpContextAccessor();
                    var options = Options();
                    var clock = Clock(new DateTimeOffset(now.Year, now.Month, now.Day, now.Hour,
                        now.Minute, now.Second, TimeSpan.Zero));
                    var principal = Principal();
                    var properties = Properties();

                    await Authentication(context, options: options, clock: clock)
                        .Login(principal, properties);
                    
                    Assert.Equal(clock.UtcNow.ToOffset(),
                        context.Authentication.SignInAsync_InputProperties!.IssuedUtc);
                }
            }
        }

        public class RefreshLoginMethod
        {
            public class PrincipalOverload
            {
                [Fact]
                public async Task NullPrincipal_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("principal", async () =>
                    {
                        await Authentication().RefreshLogin(principal: null!);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsAuthenticateOnHttpContext()
                {
                    var accessor = HttpContextAccessor();
                    var principal = Principal();

                    await Authentication(accessor).RefreshLogin(principal);

                    Assert.Equal(Options().AuthenticationScheme,
                        accessor.Authentication.AuthenticateAsync_InputScheme);
                    Assert.Equal(accessor.HttpContext,
                        accessor.Authentication.AuthenticateAsync_InputContext);
                }

                [Fact]
                public async Task AuthenticateOnHttpContextReturnsFailure_Throws()
                {
                    var accessor = HttpContextAccessor();
                    accessor.Authentication.AuthenticateAsync_Output = AuthenticateResult.Fail(
                        "Error");
                    var principal = Principal();

                    var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                    {
                        await Authentication(accessor).RefreshLogin(principal);
                    });

                    Assert.Equal($"No authenticated principal for the following authentication scheme: {Options().AuthenticationScheme}",
                        exception.Message);
                }

                [Fact]
                public async Task WhenCalled_CallsLogin()
                {
                    var accessor = HttpContextAccessor();
                    var principal = Principal();
                    var authentication = Authentication(accessor);

                    await authentication.RefreshLogin(principal);

                    Assert.Equal(accessor.HttpContext,
                        accessor.Authentication.SignInAsync_InputContext);
                    Assert.Equal(principal,
                        accessor.Authentication.SignInAsync_InputPrincipal);
                    Assert.Equal(accessor.Authentication.AuthenticateAsync_Output!.Properties,
                        accessor.Authentication.SignInAsync_InputProperties);
                }
            }
        }

        public class AuthenticateMethod
        {
            public class AuthenticationSchemeOverload
            {
                [Fact]
                public async Task NullAuthenticationScheme_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("authenticationScheme", async () =>
                    {
                        await Authentication().Authenticate(authenticationScheme: null!);
                    });
                }

                [Fact]
                public async Task EmptyAuthenticationScheme_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("authenticationScheme", async () =>
                    {
                        await Authentication().Authenticate(authenticationScheme: "");
                    });
                }

                [Fact]
                public async Task WhiteSpaceAuthenticationScheme_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("authenticationScheme", async () =>
                    {
                        await Authentication().Authenticate(authenticationScheme: " ");
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsAuthenticateOnHttpContext()
                {
                    var options = Options();
                    var accessor = HttpContextAccessor();

                    await Authentication(accessor, options: options).Authenticate(
                        options.AuthenticationScheme);

                    Assert.Equal(accessor.HttpContext,
                        accessor.Authentication.AuthenticateAsync_InputContext);
                    Assert.Equal(options.AuthenticationScheme,
                        accessor.Authentication.AuthenticateAsync_InputScheme);
                }

                [Fact]
                public async Task AuthenticateOnHttpContextReturnsFailure_ReturnsNull()
                {
                    var options = Options();
                    var accessor = HttpContextAccessor();
                    accessor.Authentication.AuthenticateAsync_Output = AuthenticateResult.Fail(
                        "Error");

                    var result = await Authentication(accessor, options: options).Authenticate(
                        options.AuthenticationScheme);

                    Assert.Null(result);
                }

                [Fact]
                public async Task WhenCalled_ReturnsPrincipalFromHttpContext()
                {
                    var accessor = HttpContextAccessor();

                    var result = await Authentication(accessor).Authenticate();

                    Assert.Equal(result,
                        accessor.Authentication.AuthenticateAsync_Output!.Principal);
                }
            }
        }

        public class AuthenticateContextMethod
        {
            public class AuthenticationSchemeOverload
            {
                [Fact]
                public async Task NullAuthenticationScheme_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("authenticationScheme", async () =>
                    {
                        await Authentication().AuthenticateContext(authenticationScheme: null!);
                    });
                }

                [Fact]
                public async Task EmptyAuthenticationScheme_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("authenticationScheme", async () =>
                    {
                        await Authentication().AuthenticateContext(authenticationScheme: "");
                    });
                }

                [Fact]
                public async Task WhiteSpaceAuthenticationScheme_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("authenticationScheme", async () =>
                    {
                        await Authentication().AuthenticateContext(authenticationScheme: " ");
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsAuthenticateOnHttpContext()
                {
                    var options = Options();
                    var accessor = HttpContextAccessor();

                    await Authentication(accessor, options: options).AuthenticateContext(
                        options.AuthenticationScheme);

                    Assert.Equal(accessor.HttpContext,
                        accessor.Authentication.AuthenticateAsync_InputContext);
                    Assert.Equal(options.AuthenticationScheme,
                        accessor.Authentication.AuthenticateAsync_InputScheme);
                }

                [Fact]
                public async Task WhenCalled_ReturnsResultFromHttpContext()
                {
                    var options = Options();
                    var accessor = HttpContextAccessor();

                    var result = await Authentication(accessor, options: options)
                        .AuthenticateContext(options.AuthenticationScheme);

                    Assert.Equal(accessor.Authentication.AuthenticateAsync_Output, result);
                }
            }
        }

        public class LogoutMethod
        {
            public class EmptyOverload
            {
                [Fact]
                public async Task WhenCalled_CallsSignOutOnHttpContext()
                {
                    var httpContextAccessor = new FakeHttpContextAccessor();
                    var options = Options();

                    await Authentication(httpContextAccessor, options: options).Logout();

                    Assert.True(httpContextAccessor.Authentication.SignOutAsync_Called);
                    Assert.Equal(httpContextAccessor.HttpContext,
                        httpContextAccessor.Authentication.SignOutAsync_InputContext);
                    Assert.Equal(options.AuthenticationScheme,
                        httpContextAccessor.Authentication.SignOutAsync_InputScheme);
                    Assert.Null(httpContextAccessor.Authentication
                        .SignOutAsync_InputProperties);
                }
            }
        }

        private static FakeHttpContextAccessor HttpContextAccessor() =>
            new FakeHttpContextAccessor();
        private static FakeUserStore UserStore() => new FakeUserStore();
        private static ClaimOptions Options() => new ClaimOptions();
        private static FakeSystemClock Clock(DateTimeOffset? utcNow = null) =>
            new FakeSystemClock(utcNow ?? DateTimeOffset.UtcNow);
        private static FakeClaimsAuthenticationService2 Authentication(
            FakeHttpContextAccessor? httpContextAccessor = null,
            FakeUserStore? userStore = null, ClaimOptions? options = null,
            FakeSystemClock? clock = null) =>
            new FakeClaimsAuthenticationService2(httpContextAccessor ?? HttpContextAccessor(),
                userStore ?? UserStore(), options ?? Options(), clock ?? Clock());
        private static ClaimsPrincipal Principal() => new ClaimsPrincipal();
        private static AuthenticationProperties Properties() => new AuthenticationProperties();
    }
}