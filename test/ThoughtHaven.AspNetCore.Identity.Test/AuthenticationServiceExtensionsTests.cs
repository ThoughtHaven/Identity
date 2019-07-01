using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity
{
    public class AuthenticationServiceExtensionsTests
    {
        public class LoginMethod
        {
            public class AuthenticationAndLoginOverload
            {
                [Fact]
                public async Task NullAuthentication_Throws()
                {
                    IAuthenticationService<User>? authentication = null;

                    await Assert.ThrowsAsync<ArgumentNullException>("authentication", async () =>
                    {
                        await authentication!.Login(User());
                    });
                }

                [Fact]
                public async Task NullLogin_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("login", async () =>
                    {
                        await Authentication().Login(login: null!);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsLoginOnAuthentication()
                {
                    var authentication = Authentication();
                    var user = User();

                    await authentication.Login(user);

                    Assert.Equal(user,
                        authentication.ClaimsConverter.Convert_FromUser_InputUser);
                    Assert.Equal(authentication.ClaimsConverter.Convert_FromUser_Output,
                        authentication.ClaimsAuthenticationService.Login_InputPrincipal);
                    Assert.NotNull(authentication.ClaimsAuthenticationService
                        .Login_InputProperties);
                }
            }

            public class AuthenticationAndLoginAndPersistentOverload
            {
                [Fact]
                public async Task NullAuthentication_Throws()
                {
                    IAuthenticationService<User>? authentication = null;

                    await Assert.ThrowsAsync<ArgumentNullException>("authentication", async () =>
                    {
                        await authentication!.Login(User(), persistent: false);
                    });
                }

                [Fact]
                public async Task NullLogin_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("login", async () =>
                    {
                        await Authentication().Login(login: null!, persistent: false);
                    });
                }

                [Fact]
                public async Task PersistentFalse_CallsLoginOnAuthentication()
                {
                    var authentication = Authentication();
                    var user = User();

                    await authentication.Login(user, persistent: false);

                    Assert.Equal(user,
                        authentication.ClaimsConverter.Convert_FromUser_InputUser);
                    Assert.Equal(authentication.ClaimsConverter.Convert_FromUser_Output,
                        authentication.ClaimsAuthenticationService.Login_InputPrincipal);
                    Assert.False(authentication.ClaimsAuthenticationService
                        .Login_InputProperties.IsPersistent);
                }

                [Fact]
                public async Task PersistentTrue_CallsLoginOnAuthentication()
                {
                    var authentication = Authentication();
                    var user = User();

                    await authentication.Login(user, persistent: true);

                    Assert.Equal(user,
                        authentication.ClaimsConverter.Convert_FromUser_InputUser);
                    Assert.Equal(authentication.ClaimsConverter.Convert_FromUser_Output,
                        authentication.ClaimsAuthenticationService.Login_InputPrincipal);
                    Assert.True(authentication.ClaimsAuthenticationService
                        .Login_InputProperties.IsPersistent);
                }
            }
        }

        private static FakeIdentityServiceBase Authentication() =>
            new FakeIdentityServiceBase(new FakeUserStore(), new FakeUserValidators(),
                new FakeClaimsAuthenticationService1(), new FakeClaimsConverter());
        private static User User() => new User();
    }
}