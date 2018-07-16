using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Claims;
using ThoughtHaven.Contacts;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.Lockouts;
using ThoughtHaven.Data;
using ThoughtHaven.Security.SingleUseTokens;
using Xunit;
using ThoughtHaven.AspNetCore.Identity.Passwords;

namespace ThoughtHaven.AspNetCore.Identity.Startup
{
    public class IdentityServiceCollectionExtensionsTests
    {
        public class AddThoughtHavenIdentityMethod
        {
            public class ServicesAndOptionsOverload
            {
                [Fact]
                public void NullServices_Throws()
                {
                    ServiceCollection services = null;

                    Assert.Throws<ArgumentNullException>("services", () =>
                    {
                        services.AddThoughtHavenIdentity<User, FakeUserStore, FakeSingleUseTokenService, FakeTimedLockoutStore>(
                            options: new IdentityOptions());
                    });
                }

                [Fact]
                public void NullOptions_Throws()
                {
                    var services = new ServiceCollection();

                    Assert.Throws<ArgumentNullException>("options", () =>
                    {
                        services.AddThoughtHavenIdentity<User, FakeUserStore, FakeSingleUseTokenService, FakeTimedLockoutStore>(
                            options: null);
                    });
                }

                [Fact]
                public void WhenCalled_AddsOptions()
                {
                    var services = new ServiceCollection();
                    var options = new IdentityOptions();

                    services.AddThoughtHavenIdentity<User, FakeUserStore, FakeSingleUseTokenService, FakeTimedLockoutStore>(
                        options);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IdentityOptions>());
                }

                [Fact]
                public void WhenCalled_AddsInfrastructure()
                {
                    var services = new ServiceCollection();
                    var options = new IdentityOptions();

                    services.AddThoughtHavenIdentity<User, FakeUserStore, FakeSingleUseTokenService, FakeTimedLockoutStore>(
                        options);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<SystemClock>());
                }

                [Fact]
                public void WhenCalled_AddsPasswords()
                {
                    var services = new ServiceCollection();
                    var options = new IdentityOptions();

                    services.AddThoughtHavenIdentity<User, FakeUserStore, FakeSingleUseTokenService, FakeTimedLockoutStore>(
                        options);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IPasswordHasher>());
                }

                [Fact]
                public void WhenCalled_AddsUserHelpers()
                {
                    var services = new ServiceCollection();
                    var options = new IdentityOptions();

                    services.AddThoughtHavenIdentity<User, FakeUserStore, FakeSingleUseTokenService, FakeTimedLockoutStore>(
                        options);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IUserHelper>());
                }

                [Fact]
                public void WhenCalled_AddsUserValidators()
                {
                    var services = new ServiceCollection();
                    var options = new IdentityOptions();

                    services.AddThoughtHavenIdentity<User, FakeUserStore, FakeSingleUseTokenService, FakeTimedLockoutStore>(
                        options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<User>>>();

                    Assert.True(validators.Count() > 0);
                }

                [Fact]
                public void WhenCalled_AddsUserStore()
                {
                    var services = new ServiceCollection();
                    var options = new IdentityOptions();

                    services.AddThoughtHavenIdentity<User, FakeUserStore, FakeSingleUseTokenService, FakeTimedLockoutStore>(
                        options);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<ICrudStore<UserKey, User>>());
                }

                [Fact]
                public void WhenCalled_AddsUserEmailStore()
                {
                    var services = new ServiceCollection();
                    var options = new IdentityOptions();

                    services.AddThoughtHavenIdentity<User, FakeUserStore, FakeSingleUseTokenService, FakeTimedLockoutStore>(
                        options);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(
                        provider.GetRequiredService<IRetrieveOperation<EmailAddress, User>>());
                }

                [Fact]
                public void WhenCalled_AddsSingleUseTokenService()
                {
                    var services = new ServiceCollection();
                    var options = new IdentityOptions();

                    services.AddThoughtHavenIdentity<User, FakeUserStore, FakeSingleUseTokenService, FakeTimedLockoutStore>(
                        options);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<ISingleUseTokenService>());
                }

                [Fact]
                public void WhenCalled_AddsTimedLockoutStore()
                {
                    var services = new ServiceCollection();
                    var options = new IdentityOptions();

                    services.AddThoughtHavenIdentity<User, FakeUserStore, FakeSingleUseTokenService, FakeTimedLockoutStore>(
                        options);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(
                        provider.GetRequiredService<ICrudStore<string, TimedLockout>>());
                }

                [Fact]
                public void WhenCalled_AddsUserClaims()
                {
                    var services = new ServiceCollection();
                    var options = new IdentityOptions();

                    services.AddThoughtHavenIdentity<User, FakeUserStore, FakeSingleUseTokenService, FakeTimedLockoutStore>(
                        options);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IUserClaimsConverter<User>>());
                }

                [Fact]
                public void WhenCalled_AddsIdentityServices()
                {
                    var services = new ServiceCollection();
                    var options = new IdentityOptions();

                    services.AddThoughtHavenIdentity<User, FakeUserStore, FakeSingleUseTokenService, FakeTimedLockoutStore>(
                        options);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IIdentityService<User>>());
                }

                [Fact]
                public void WhenCalled_AddsAuthentication()
                {
                    var services = new ServiceCollection();
                    var options = new IdentityOptions()
                    {
                        AuthenticationScheme = "TestAuth"
                    };

                    services.AddThoughtHavenIdentity<User, FakeUserStore, FakeSingleUseTokenService, FakeTimedLockoutStore>(
                        options);

                    var auth = services.BuildServiceProvider()
                        .GetRequiredService<IOptions<AuthenticationOptions>>().Value;

                    Assert.Equal(options.AuthenticationScheme, auth.DefaultAuthenticateScheme);
                    Assert.Equal(options.AuthenticationScheme, auth.DefaultChallengeScheme);
                }

                [Fact]
                public async Task WhenCalled_AddsCookieAuthentication()
                {
                    var services = new ServiceCollection();
                    services.AddSingleton<ILoggerFactory, FakeLoggerFactory>();
                    services.AddSingleton(UrlEncoder.Default);
                    var options = new IdentityOptions()
                    {
                        AuthenticationScheme = "CookieAuth"
                    };
                    options.Cookie.Cookie.Name = "TestCookie";

                    services.AddThoughtHavenIdentity<User, FakeUserStore, FakeSingleUseTokenService, FakeTimedLockoutStore>(
                        options);

                    var provider = services.BuildServiceProvider();

                    var handlers = provider.GetRequiredService<IAuthenticationHandlerProvider>();

                    var handler = await handlers.GetHandlerAsync(
                        new DefaultHttpContext()
                        {
                            RequestServices = provider
                        },
                        options.AuthenticationScheme) as CookieAuthenticationHandler;

                    foreach (var property in handler.Options.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (!property.CanRead || !property.CanWrite) continue;

                        var expected = property.GetValue(options.Cookie);
                        var actual = property.GetValue(handler.Options);

                        if (expected == null && actual != null) continue;

                        if (expected is string)
                        {
                            if (string.IsNullOrEmpty((string)expected) &&
                                !string.IsNullOrEmpty((string)actual)) continue;
                        }

                        if (expected is PathString)
                        {
                            if (string.IsNullOrEmpty((PathString)expected) &&
                                !string.IsNullOrEmpty((PathString)actual)) continue;
                        }

                        Assert.Equal(expected, actual);
                    }

                    Assert.Equal(options.Cookie.Cookie, handler.Options.Cookie);
                    Assert.Equal("TestCookie", handler.Options.Cookie.Name);
                }
            }
        }
    }
}