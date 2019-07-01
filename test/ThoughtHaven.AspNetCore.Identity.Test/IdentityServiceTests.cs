using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using ThoughtHaven.AspNetCore.Identity.Internal;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity
{
    public class IdentityServiceTests
    {
        public class Type
        {
            [Fact]
            public void InheritsIdentityServiceBase()
            {
                Assert.True(typeof(IdentityServiceBase<User>)
                    .IsAssignableFrom(typeof(IdentityService<User>)));
            }

            [Fact]
            public void InheritsIIdentityHelperService()
            {
                Assert.True(typeof(IIdentityService<User>)
                    .IsAssignableFrom(typeof(IdentityService<User>)));
            }
        }

        public class Constructor
        {
            [Fact]
            public void NullUserStore_Throws()
            {
                Assert.Throws<ArgumentNullException>("userStore", () =>
                {
                    new IdentityService<User>(
                        userStore: null!,
                        userValidators: new FakeUserValidators(),
                        claimsAuthenticationService: new FakeClaimsAuthenticationService1(),
                        claimsConverter: new FakeClaimsConverter(),
                        helper: new FakeUserHelper1());
                });
            }

            [Fact]
            public void NullUserValidators_Throws()
            {
                Assert.Throws<ArgumentNullException>("userValidators", () =>
                {
                    new IdentityService<User>(
                        userStore: new FakeUserStore(),
                        userValidators: null!,
                        claimsAuthenticationService: new FakeClaimsAuthenticationService1(),
                        claimsConverter: new FakeClaimsConverter(),
                        helper: new FakeUserHelper1());
                });
            }

            [Fact]
            public void NullClaimsAuthenticationService_Throws()
            {
                Assert.Throws<ArgumentNullException>("claimsAuthenticationService", () =>
                {
                    new IdentityService<User>(
                        userStore: new FakeUserStore(),
                        userValidators: new FakeUserValidators(),
                        claimsAuthenticationService: null!,
                        claimsConverter: new FakeClaimsConverter(),
                        helper: new FakeUserHelper1());
                });
            }

            [Fact]
            public void NullClaimsConverter_Throws()
            {
                Assert.Throws<ArgumentNullException>("claimsConverter", () =>
                {
                    new IdentityService<User>(
                        userStore: new FakeUserStore(),
                        userValidators: new FakeUserValidators(),
                        claimsAuthenticationService: new FakeClaimsAuthenticationService1(),
                        claimsConverter: null!,
                        helper: new FakeUserHelper1());
                });
            }

            [Fact]
            public void NullHelper_Throws()
            {
                Assert.Throws<ArgumentNullException>("helper", () =>
                {
                    new IdentityService<User>(
                        userStore: new FakeUserStore(),
                        userValidators: new FakeUserValidators(),
                        claimsAuthenticationService: new FakeClaimsAuthenticationService1(),
                        claimsConverter: new FakeClaimsConverter(),
                        helper: null!);
                });
            }

            [Fact]
            public void WhenCalled_SetsHelper()
            {
                var helper = new FakeUserHelper1();

                var service = new IdentityService<User>(
                    userStore: new FakeUserStore(),
                    userValidators: new FakeUserValidators(),
                    claimsAuthenticationService: new FakeClaimsAuthenticationService1(),
                    claimsConverter: new FakeClaimsConverter(),
                    helper: helper);

                Assert.Equal(helper, service.Helper);
            }
        }

        public class CreateMethod
        {
            public class UserOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await Identity().Create(user: null!);
                    });
                }

                [Fact]
                public async Task NullUserId_CallsAssignUserIdOnHelper()
                {
                    var identity = Identity();
                    var user = User();
                    user.Id = null;

                    await identity.Create(user);

                    Assert.NotNull(user.Id);
                    Assert.Equal(user, identity.Helper.AssignUserId_InputUser);
                }

                [Fact]
                public async Task EmptyUserId_CallsAssignUserIdOnHelper()
                {
                    var identity = Identity();
                    var user = User();
                    user.Id = "";

                    await identity.Create(user);

                    Assert.NotNull(user.Id);
                    Assert.Equal(user, identity.Helper.AssignUserId_InputUser);
                }

                [Fact]
                public async Task WhiteSpaceUserId_CallsAssignUserIdOnHelper()
                {
                    var identity = Identity();
                    var user = User();
                    user.Id = " ";

                    await identity.Create(user);

                    Assert.NotNull(user.Id);
                    Assert.Equal(user, identity.Helper.AssignUserId_InputUser);
                }

                [Fact]
                public async Task UserIdHasValue_DoesNotCallAssignUserIdOnHelper()
                {
                    var identity = Identity();
                    var user = User();
                    user.Id = "id";

                    await identity.Create(user);

                    Assert.Equal("id", user.Id);
                    Assert.Null(identity.Helper.AssignUserId_InputUser);
                }

                [Fact]
                public async Task NullSecurityStamp_CallsRefreshSecurityStampOnHelper()
                {
                    var identity = Identity();
                    var user = User();
                    user.SecurityStamp = null;

                    await identity.Create(user);

                    Assert.NotNull(user.SecurityStamp);
                    Assert.Equal(user, identity.Helper.RefreshSecurityStamp_InputUser);
                }

                [Fact]
                public async Task EmptySecurityStamp_CallsRefreshSecurityStampOnHelper()
                {
                    var identity = Identity();
                    var user = User();
                    user.SecurityStamp = "";

                    await identity.Create(user);

                    Assert.NotNull(user.SecurityStamp);
                    Assert.Equal(user, identity.Helper.RefreshSecurityStamp_InputUser);
                }

                [Fact]
                public async Task WhiteSpaceSecurityStamp_CallsRefreshSecurityStampOnHelper()
                {
                    var identity = Identity();
                    var user = User();
                    user.SecurityStamp = " ";

                    await identity.Create(user);

                    Assert.NotNull(user.SecurityStamp);
                    Assert.Equal(user, identity.Helper.RefreshSecurityStamp_InputUser);
                }

                [Fact]
                public async Task SecurityStampHasValue_DoesNotCallRefreshSecurityStampOnHelper()
                {
                    var identity = Identity();
                    var user = User();
                    user.SecurityStamp = "stamp";

                    await identity.Create(user);

                    Assert.Equal("stamp", user.SecurityStamp);
                    Assert.Null(identity.Helper.RefreshSecurityStamp_InputUser);
                }

                [Fact]
                public async Task WhenCalled_CallsSetCreatedOnHelper()
                {
                    var identity = Identity();
                    var user = User();
                    user.Created = DateTimeOffset.MinValue;

                    await identity.Create(user);

                    Assert.NotEqual(DateTimeOffset.MinValue, user.Created);
                    Assert.Equal(identity.Helper.FakeClock.UtcNow, user.Created);
                    Assert.Equal(user, identity.Helper.SetCreated_InputUser);
                }

                [Fact]
                public async Task WhenCalled_SetsLastLoginToNull()
                {
                    var identity = Identity();
                    var user = User();
                    user.LastLogin = DateTimeOffset.UtcNow;

                    await identity.Create(user);

                    Assert.Null(user.LastLogin);
                }

                [Fact]
                public async Task WhenCalled_CallsCreateOnBase()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.Create(user);

                    Assert.Equal(user, identity.UserStore.Create_InputUser);
                }

                [Fact]
                public async Task WhenCalled_ReturnsCreateOnBase()
                {
                    var identity = Identity();
                    var user = User();

                    var result = await identity.Create(user);

                    Assert.Equal(identity.UserStore.Create_Output, result);
                }
            }
        }

        public class LoginMethod
        {
            public class UserAndPropertiesOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await Identity().Login(
                            user: null!,
                            properties: Properties());
                    });
                }

                [Fact]
                public async Task NullProperties_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("properties", async () =>
                    {
                        await Identity().Login(
                            user: User(),
                            properties: null!);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsSetLastLoginOnHelper()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.Login(user, Properties());

                    Assert.Equal(user, identity.Helper.SetLastLogin_InputUser);
                }

                [Fact]
                public async Task WhenCalled_CallsUpdateOnIdentity()
                {
                    var identity = Identity();
                    var user = User();

                    await identity.Login(user, Properties());

                    Assert.Equal(user, identity.Update_InputUser);
                }

                [Fact]
                public async Task WhenCalled_CallsLoginOnBase()
                {
                    var identity = Identity();
                    var user = User();
                    var properties = Properties();

                    await identity.Login(user, properties);

                    Assert.Equal(user, identity.Login_InputUser);
                    Assert.Equal(properties, identity.Login_InputProperties);
                }
            }
        }

        private static FakeIdentityService Identity() =>
            new FakeIdentityService(new FakeUserStore(), new FakeUserValidators(),
                new FakeClaimsAuthenticationService1(), new FakeClaimsConverter(),
                new FakeUserHelper1());
        private static User User() => new User()
        {
            Id = "id",
            Email = "some@email.com",
            PasswordHash = "hash",
            SecurityStamp = "stamp",
        };
        private static AuthenticationProperties Properties() => new AuthenticationProperties();
    }
}