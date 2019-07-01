using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Claims
{
    public class UserClaimsConverterTests
    {
        public class Type
        {
            [Fact]
            public void ImplementsIUserClaimsConverter()
            {
                Assert.True(typeof(IUserClaimsConverter<User>)
                    .IsAssignableFrom(typeof(UserClaimsConverter<User>)));
            }

            [Fact]
            public void RequiresIUserIdentifierOfString()
            {
                var type = typeof(UserClaimsConverter<>);
                var tUser = type.GetGenericArguments()[0];

                Assert.True(typeof(IUserKey).IsAssignableFrom(tUser));
            }

            [Fact]
            public void RequiresIUserSecurityStamp()
            {
                var type = typeof(UserClaimsConverter<>);
                var tUser = type.GetGenericArguments()[0];

                Assert.True(typeof(IUserSecurityStamp).IsAssignableFrom(tUser));
            }
        }

        public class Constructor
        {
            public class OptionsAndClockOverload
            {
                [Fact]
                public void NullOptions_Throws()
                {
                    Assert.Throws<ArgumentNullException>("options", () =>
                    {
                        new UserClaimsConverter<User>(
                            options: null!,
                            clock: new SystemClock());
                    });
                }

                [Fact]
                public void NullClock_Throws()
                {
                    Assert.Throws<ArgumentNullException>("clock", () =>
                    {
                        new UserClaimsConverter<User>(
                            options: new ClaimOptions(),
                            clock: null!);
                    });
                }
            }
        }

        public class ConvertMethod
        {
            public class UserOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await Converter().Convert(user: null!);
                    });
                }

                [Fact]
                public async Task NullUserKey_Throws()
                {
                    var user = User();
                    user.Id = null;

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Converter().Convert(user);
                    });
                }

                [Fact]
                public async Task EmptyUserKey_Throws()
                {
                    var user = User();
                    user.Id = "";

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Converter().Convert(user);
                    });
                }

                [Fact]
                public async Task WhiteSpaceUserKey_Throws()
                {
                    var user = User();
                    user.Id = " ";

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Converter().Convert(user);
                    });
                }

                [Fact]
                public async Task NullSecurityStamp_Throws()
                {
                    var user = User();
                    user.SecurityStamp = null;

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Converter().Convert(user);
                    });
                }

                [Fact]
                public async Task EmptySecurityStamp_Throws()
                {
                    var user = User();
                    user.SecurityStamp = "";

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Converter().Convert(user);
                    });
                }

                [Fact]
                public async Task WhiteSpaceSecurityStamp_Throws()
                {
                    var user = User();
                    user.SecurityStamp = " ";

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await Converter().Convert(user);
                    });
                }

                [Fact]
                public async Task WhenCalled_ReturnsAuthenticatedPrincipal()
                {
                    var principal = await Converter().Convert(User());

                    var identity = GetIdentity(principal);

                    Assert.True(identity.IsAuthenticated);
                }

                [Fact]
                public async Task WhenCalled_SetsCorrectClaimIssuerForAllClaims()
                {
                    var options = new ClaimOptions() { Issuer = "Test Issuer" };

                    var principal = await Converter(options).Convert(User());

                    _ = GetIdentity(principal);

                    foreach (var claim in principal.Claims)
                    {
                        Assert.Equal("Test Issuer", claim.Issuer);
                    }
                }

                [Fact]
                public async Task WhenCalled_AddsUserIdentifierClaim()
                {
                    var options = Options();
                    var user = User();

                    var principal = await Converter(options).Convert(user);

                    var identity = GetIdentity(principal);

                    Assert.True(identity.HasClaim(options.ClaimTypes.UserKey, user.Id));
                }

                [Fact]
                public async Task WhenCalled_AddsSecurityStampValidatedClaim()
                {
                    var options = Options();
                    var clock = Clock();
                    var user = User();

                    var principal = await Converter(options, clock).Convert(user);

                    var identity = GetIdentity(principal);

                    Assert.True(identity.HasClaim(
                        options.ClaimTypes.SecurityStampValidated,
                        clock.UtcNow.UtcTicks.ToString()));
                }

                [Fact]
                public async Task WhenCalled_AddsSecurityStampClaim()
                {
                    var options = Options();
                    var user = User();

                    var principal = await Converter(options).Convert(user);

                    var identity = GetIdentity(principal);

                    Assert.True(identity.HasClaim(
                        options.ClaimTypes.SecurityStamp,
                        user.SecurityStamp));
                }
            }

            public class PrincipalOverload
            {
                [Fact]
                public async Task NullPrincipal_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("principal", async () =>
                    {
                        await Converter().Convert(principal: null!);
                    });
                }

                [Fact]
                public async Task PrincipalAuthenticatedWithCorrectIdentityButNoUserIdentifierClaim_ReturnsNull()
                {
                    var options = Options();
                    var identity = new ClaimsIdentity(options.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var userKey = await Converter(options).Convert(principal);

                    Assert.Null(userKey);
                }

                [Fact]
                public async Task PrincipalAuthenticatedWithCorrectIdentityAndUserIdentifierClaim_ReturnsCorrectUserKey()
                {
                    var options = new ClaimOptions();
                    var identity = new ClaimsIdentity(options.AuthenticationScheme);
                    identity.AddClaim(new Claim(options.ClaimTypes.UserKey, "user-id"));
                    var principal = new ClaimsPrincipal(identity);

                    var userKey = await Converter(options).Convert(principal);

                    Assert.NotNull(userKey);
                    Assert.Equal("user-id", userKey!.Value);
                }

                [Fact]
                public async Task PrincipalAuthenticatedWithWrongIdentity_ReturnsNull()
                {
                    var identity = new ClaimsIdentity("wrong");
                    var principal = new ClaimsPrincipal(identity);

                    var userKey = await Converter().Convert(principal);

                    Assert.Null(userKey);
                }

                [Fact]
                public async Task PrincipalNotAuthenticated_ReturnsNull()
                {
                    var principal = new ClaimsPrincipal();

                    var userKey = await Converter().Convert(principal);

                    Assert.Null(userKey);
                }
            }
        }

        private static ClaimOptions Options() => new ClaimOptions();
        private static FakeSystemClock Clock(DateTimeOffset? utcNow = null) =>
            new FakeSystemClock(utcNow ?? DateTimeOffset.UtcNow);
        private static UserClaimsConverter<User> Converter(ClaimOptions? options = null,
            FakeSystemClock? clock = null) =>
            new UserClaimsConverter<User>(options ?? Options(), clock ?? Clock());
        private static User User() => new User()
        {
            Id = "user-id",
            SecurityStamp = "stamp",
        };
        private static ClaimsIdentity GetIdentity(ClaimsPrincipal principal) =>
            principal.Identities
                .Where(i => i.AuthenticationType == Options().AuthenticationScheme).First();
    }
}