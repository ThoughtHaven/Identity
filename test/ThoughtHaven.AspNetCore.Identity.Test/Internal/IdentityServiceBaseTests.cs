using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using ThoughtHaven.AspNetCore.Identity.Keys;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Internal
{
    public class IdentityServiceBaseTests
    {
        public class Type
        {
            [Fact]
            public void ImplementsIIdentityService()
            {
                Assert.True(typeof(IIdentityServiceBase<User>)
                    .IsAssignableFrom(typeof(IdentityServiceBase<User>)));
            }
        }

        public class Constructor
        {
            [Fact]
            public void NullUserStore_Throws()
            {
                Assert.Throws<ArgumentNullException>("userStore", () =>
                {
                    new FakeIdentityServiceBase(
                        userStore: null!,
                        userValidators: new FakeUserValidators(),
                        claimsAuthenticationService: new FakeClaimsAuthenticationService1(),
                        claimsConverter: new FakeClaimsConverter());
                });
            }

            [Fact]
            public void NullUserValidators_Throws()
            {
                Assert.Throws<ArgumentNullException>("userValidators", () =>
                {
                    new FakeIdentityServiceBase(
                        userStore: new FakeUserStore(),
                        userValidators: null!,
                        claimsAuthenticationService: new FakeClaimsAuthenticationService1(),
                        claimsConverter: new FakeClaimsConverter());
                });
            }

            [Fact]
            public void NullClaimsAuthenticationService_Throws()
            {
                Assert.Throws<ArgumentNullException>("claimsAuthenticationService", () =>
                {
                    new FakeIdentityServiceBase(
                        userStore: new FakeUserStore(),
                        userValidators: new FakeUserValidators(),
                        claimsAuthenticationService: null!,
                        claimsConverter: new FakeClaimsConverter());
                });
            }

            [Fact]
            public void NullClaimsConverter_Throws()
            {
                Assert.Throws<ArgumentNullException>("claimsConverter", () =>
                {
                    new FakeIdentityServiceBase(
                        userStore: new FakeUserStore(),
                        userValidators: new FakeUserValidators(),
                        claimsAuthenticationService: new FakeClaimsAuthenticationService1(),
                        claimsConverter: null!);
                });
            }
        }

        public class RetrieveMethod
        {
            public class KeyOverload
            {
                [Fact]
                public async Task NullKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("key", async () =>
                    {
                        await Identity().Retrieve(key: null!);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsUserStore()
                {
                    var identity = Identity();

                    _ = await identity.Retrieve("key");

                    Assert.Equal("key", identity.UserStore.Retrieve_ByKey_InputUserKey);
                }

                [Fact]
                public async Task WhenCalled_ReturnsUserFromUserStore()
                {
                    var identity = Identity();

                    var user = await identity.Retrieve("key");

                    Assert.Equal(identity.UserStore.Retrieve_ByKey_Output, user);
                }
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
                public async Task WhenCalled_CallsValidators()
                {
                    var identity = Identity();
                    var user = new User();

                    await identity.Create(user);

                    Assert.True(identity.UserValidators.AllValidatorsCalled);
                }

                [Fact]
                public async Task WhenCalled_CallsUserStore()
                {
                    var identity = Identity();
                    var user = new User();

                    await identity.Create(user);

                    Assert.Equal(user, identity.UserStore.Create_InputUser);
                }

                [Fact]
                public async Task WhenCalled_ReturnsUserFromUserStore()
                {
                    var identity = Identity();
                    var user = new User();

                    var result = await identity.Create(user);

                    Assert.Equal(identity.UserStore.Create_Output, result);
                }

                [Fact]
                public async Task InvalidUser_Throws()
                {
                    var validators = UserValidators(invalidCount: 1);
                    var identity = Identity(userValidators: validators);
                    var user = new User();

                    var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    {
                        await identity.Create(user);
                    });

                    foreach (var validator in validators)
                    {
                        Assert.Equal(user, validator.Validate_UserInput);
                    }
                }
            }
        }

        public class UpdateMethod
        {
            public class UserOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await Identity().Update(user: null!);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsValidators()
                {
                    var identity = Identity();
                    var user = new User();

                    await identity.Update(user);

                    Assert.True(identity.UserValidators.AllValidatorsCalled);
                }

                [Fact]
                public async Task WhenCalled_CallsUserStore()
                {
                    var identity = Identity();
                    var user = new User();

                    await identity.Update(user);

                    Assert.Equal(user, identity.UserStore.Update_InputUser);
                }

                [Fact]
                public async Task WhenCalled_ReturnsUserFromUserStore()
                {
                    var identity = Identity();
                    var user = new User();

                    var result = await identity.Update(user);

                    Assert.Equal(identity.UserStore.Update_Output, result);
                }

                [Fact]
                public async Task InvalidUser_Throws()
                {
                    var validators = UserValidators(invalidCount: 1);
                    var identity = Identity(userValidators: validators);
                    var user = new User();

                    var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    {
                        await identity.Update(user);
                    });

                    foreach (var validator in validators)
                    {
                        Assert.Equal(user, validator.Validate_UserInput);
                    }
                }
            }
        }

        public class DeleteAsyncMethod
        {
            public class KeyOverload
            {
                [Fact]
                public async Task NullKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("key", async () =>
                    {
                        await Identity().Delete(key: null!);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsUserStore()
                {
                    var identity = Identity();

                    await identity.Delete("123");

                    Assert.Equal("123", identity.UserStore.Deleted_InputUserKey);
                }
            }
        }

        public class LoginAsyncMethod
        {
            public class UserOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await Identity().Login(
                            user: null!,
                            properties: new AuthenticationProperties());
                    });
                }

                [Fact]
                public async Task NullAuthenticationProperties_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("properties", async () =>
                    {
                        await Identity().Login(
                            user: new User(),
                            properties: null!);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsClaimsConverter()
                {
                    var identity = Identity();
                    var user = new User();
                    var properties = new AuthenticationProperties();

                    await identity.Login(user, properties);

                    Assert.Equal(user, identity.ClaimsConverter.Convert_FromUser_InputUser);
                }

                [Fact]
                public async Task WhenCalled_CallsAuthenticationService()
                {
                    var identity = Identity();
                    var user = new User();
                    var properties = new AuthenticationProperties();

                    await identity.Login(user, properties);

                    Assert.True(identity.ClaimsAuthenticationService.Login_Called);
                    Assert.Equal(identity.ClaimsConverter.Convert_FromUser_Output,
                        identity.ClaimsAuthenticationService.Login_InputPrincipal);
                    Assert.Equal(properties,
                        identity.ClaimsAuthenticationService.Login_InputProperties);
                }
            }
        }

        public class AuthenticateAsyncMethod
        {
            public class EmptyOverload
            {
                [Fact]
                public async Task WhenCalled_CallsClaimsAuthenticationService()
                {
                    var identity = Identity();

                    await identity.Authenticate();

                    Assert.True(identity.ClaimsAuthenticationService.Authenticate_Called);
                }

                [Fact]
                public async Task ClaimsAuthenticationServiceReturnsNull_ReturnsNull()
                {
                    var identity = Identity();
                    identity.ClaimsAuthenticationService.Authenticate_OutputPrincipal = null;

                    var result = await identity.Authenticate();

                    Assert.Null(result);
                }

                [Fact]
                public async Task WhenCalled_CallsConvertOnClaimsConverter()
                {
                    var identity = Identity();

                    await identity.Authenticate();

                    Assert.Equal(
                        identity.ClaimsAuthenticationService.Authenticate_OutputPrincipal,
                        identity.ClaimsConverter.Convert_FromPrincipal_InputPrincipal);
                }

                [Fact]
                public async Task ClaimsConverterReturnsNull_ReturnsNull()
                {
                    var identity = Identity();
                    identity.ClaimsConverter.Convert_FromPrincipal_Output = null;

                    var result = await identity.Authenticate();

                    Assert.Null(result);
                }

                [Fact]
                public async Task WhenCalled_CallsRetrieveOnUserStore()
                {
                    var identity = Identity();

                    await identity.Authenticate();

                    Assert.Equal(identity.ClaimsConverter.Convert_FromPrincipal_Output,
                        identity.UserStore.Retrieve_ByKey_InputUserKey);
                }

                [Fact]
                public async Task WhenCalled_ReturnsUserFromUserStore()
                {
                    var identity = Identity();

                    var result = await identity.Authenticate();

                    Assert.Equal(identity.UserStore.Retrieve_ByKey_Output, result);
                }
            }
        }

        public class LogoutAsyncMethod
        {
            public class EmptyOverload
            {
                [Fact]
                public async Task CallsClaimsAuthenticationService()
                {
                    var identity = Identity();

                    await identity.Logout();

                    Assert.True(identity.ClaimsAuthenticationService.Logout_Called);
                }
            }
        }

        private static FakeUserStore UserStore() => new FakeUserStore();
        private static FakeUserValidators UserValidators(byte validCount = 1,
            byte invalidCount = 0) => new FakeUserValidators(validCount, invalidCount);
        private static FakeClaimsAuthenticationService1 ClaimsAuthenticationService() =>
            new FakeClaimsAuthenticationService1();
        private static FakeClaimsConverter ClaimsConverter() => new FakeClaimsConverter();
        private static FakeIdentityServiceBase Identity(FakeUserStore? userStore = null,
            FakeUserValidators? userValidators = null,
            FakeClaimsAuthenticationService1? claimsAuthenticationService = null,
            FakeClaimsConverter? claimsConverter = null) =>
            new FakeIdentityServiceBase(userStore ?? UserStore(),
                userValidators ?? UserValidators(),
                claimsAuthenticationService ?? ClaimsAuthenticationService(),
                claimsConverter ?? ClaimsConverter());
    }
}