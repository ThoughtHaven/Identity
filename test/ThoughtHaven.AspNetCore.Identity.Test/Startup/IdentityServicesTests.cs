using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PwnedPasswords.Client;
using ThoughtHaven.AspNetCore.Identity.Claims;
using ThoughtHaven.AspNetCore.Identity.Created;
using ThoughtHaven.AspNetCore.Identity.Emails;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using ThoughtHaven.AspNetCore.Identity.Internal;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.Lockouts;
using ThoughtHaven.AspNetCore.Identity.Passwords;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;
using ThoughtHaven.Data;
using ThoughtHaven.Messages.Emails;
using ThoughtHaven.Security.SingleUseTokens;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Startup
{
    public class IdentityServicesTests
    {
        public class AddOptionsMethod
        {
            public class ServicesAndOptionsOverload
            {
                [Fact]
                public void NullServices_Throws()
                {
                    Assert.Throws<ArgumentNullException>("services", () =>
                    {
                        IdentityServices.AddOptions(
                            services: null,
                            options: new IdentityOptions());
                    });
                }

                [Fact]
                public void NullOptions_Throws()
                {
                    Assert.Throws<ArgumentNullException>("options", () =>
                    {
                        IdentityServices.AddOptions(
                            services: new ServiceCollection(),
                            options: null);
                    });
                }

                [Fact]
                public void WhenCalled_AddsIdentityOptions()
                {
                    var services = new ServiceCollection();
                    var options = new IdentityOptions();

                    IdentityServices.AddOptions(services, options);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IdentityOptions>());
                }

                [Fact]
                public void WhenCalled_AddsClaimOptions()
                {
                    var services = new ServiceCollection();
                    var options = new IdentityOptions();

                    IdentityServices.AddOptions(services, options);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<ClaimOptions>());
                }

                [Fact]
                public void WhenCalled_AddsUserOptions()
                {
                    var services = new ServiceCollection();
                    var options = new IdentityOptions();

                    IdentityServices.AddOptions(services, options);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<UserOptions>());
                }
            }
        }

        public class AddInfrastructureMethod
        {
            public class ServicesOverload
            {
                [Fact]
                public void NullServices_Throws()
                {
                    Assert.Throws<ArgumentNullException>("services", () =>
                    {
                        IdentityServices.AddInfrastructure(services: null);
                    });
                }

                [Fact]
                public void WhenCalled_AddsSystemClock()
                {
                    var services = new ServiceCollection();

                    IdentityServices.AddInfrastructure(services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<SystemClock>());
                }

                [Fact]
                public void WhenCalled_AddsHttpContextAccessor()
                {
                    var services = new ServiceCollection();

                    IdentityServices.AddInfrastructure(services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IHttpContextAccessor>());
                    Assert.True(provider.GetRequiredService<IHttpContextAccessor>() is HttpContextAccessor);
                }
            }
        }

        public class AddPasswordsMethod
        {
            public class ServicesOverload
            {
                [Fact]
                public void NullServices_Throws()
                {
                    Assert.Throws<ArgumentNullException>("services", () =>
                    {
                        IdentityServices.AddPasswords(services: null);
                    });
                }

                [Fact]
                public void WhenCalled_AddsPwnedPasswordHttpClient()
                {
                    var services = new ServiceCollection();

                    IdentityServices.AddPasswords(services);

                    var provider = services.BuildServiceProvider();

                    var service = provider.GetRequiredService<IPwnedPasswordsClient>();

                    Assert.NotNull(service);
                    Assert.IsType<PwnedPasswordsClient>(service);
                }

                [Fact]
                public void WhenCalled_AddsMinimumLengthPasswordStrengthValidator()
                {
                    var services = new ServiceCollection();

                    IdentityServices.AddPasswords(services);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IPasswordStrengthValidator>>();

                    Assert.NotNull(validators);
                    Assert.Equal(2, validators.Count());
                    Assert.IsType<MinimumLengthPasswordStrengthValidator>(
                        validators.ElementAt(index: 0));
                }

                [Fact]
                public void WhenCalled_AddsPwnedPasswordStrengthValidator()
                {
                    var services = new ServiceCollection();

                    IdentityServices.AddPasswords(services);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IPasswordStrengthValidator>>();

                    Assert.NotNull(validators);
                    Assert.Equal(2, validators.Count());
                    Assert.IsType<PwnedPasswordStrengthValidator>(
                        validators.ElementAt(index: 1));
                }

                [Fact]
                public void WhenCalled_AddsPasswordHasher()
                {
                    var services = new ServiceCollection();

                    IdentityServices.AddPasswords(services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IPasswordHasher>());
                    Assert.True(provider.GetRequiredService<IPasswordHasher>() is Pbkdf2PasswordHasher);
                }
            }
        }

        public class AddUserHelpersMethod
        {
            public class ServicesOverload
            {
                [Fact]
                public void NullServices_Throws()
                {
                    Assert.Throws<ArgumentNullException>("services", () =>
                    {
                        IdentityServices.AddUserHelpers<User>(services: null);
                    });
                }

                [Fact]
                public void WhenCalled_AddsIUserHelper()
                {
                    var services = Services();
                    IdentityServices.AddInfrastructure(services);
                    IdentityServices.AddPasswords(services);
                    IdentityServices.AddUserEmailStore<User, FakeUserEmailStore>(services);
                    IdentityServices.AddSingleUseTokenService<FakeSingleUseTokenService>(
                        services);
                    IdentityServices.AddTimedLockoutStore<FakeTimedLockoutStore>(services);

                    // Act
                    IdentityServices.AddUserHelpers<User>(services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IUserHelper>());
                    Assert.True(provider.GetRequiredService<IUserHelper>() is UserHelper<User>);
                }
            }
        }

        public class AddUserValidatorsMethod
        {
            public class ServicesOverload
            {
                [Fact]
                public void NullServices_Throws()
                {
                    Assert.Throws<ArgumentNullException>("services", () =>
                    {
                        IdentityServices.AddUserValidators<User>(
                            services: null,
                            options: new UserOptions());
                    });
                }

                [Fact]
                public void NullOptions_Throws()
                {
                    Assert.Throws<ArgumentNullException>("options", () =>
                    {
                        IdentityServices.AddUserValidators<User>(
                            services: new ServiceCollection(),
                            options: null);
                    });
                }

                [Fact]
                public void RequireIdWithBlankUser_DoesNotAddIdValidator()
                {
                    var services = new ServiceCollection();
                    var options = new UserOptions()
                    {
                        RequireId = true
                    };

                    IdentityServices.AddUserValidators<object>(services, options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<object>>>();

                    Assert.Empty(validators.Where(v => v is UserIdRequiredValidator<User>));
                }

                [Fact]
                public void RequireIdWithImplementingUser_AddsIdValidator()
                {
                    var services = new ServiceCollection();
                    var options = new UserOptions()
                    {
                        RequireId = true
                    };

                    IdentityServices.AddUserValidators<User>(services, options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<User>>>();

                    Assert.Single(validators.Where(v => v is UserIdRequiredValidator<User>));
                }

                [Fact]
                public void NotRequireIdWithImplementingUser_DoesNotAddIdValidator()
                {
                    var services = new ServiceCollection();
                    var options = new UserOptions()
                    {
                        RequireId = false
                    };

                    IdentityServices.AddUserValidators<User>(services, options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<User>>>();

                    Assert.Empty(validators.Where(v => v is UserIdRequiredValidator<User>));
                }

                [Fact]
                public void RequireCreatedWithBlankUser_DoesNotAddCreatedValidator()
                {
                    var services = new ServiceCollection();
                    var options = new UserOptions()
                    {
                        RequireCreated = true
                    };

                    IdentityServices.AddUserValidators<object>(services, options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<User>>>();

                    Assert.Empty(validators.Where(v => v is UserCreatedRequiredValidator<User>));
                }

                [Fact]
                public void RequireCreatedWithImplementingUser_AddsCreatedValidator()
                {
                    var services = new ServiceCollection();
                    var options = new UserOptions()
                    {
                        RequireCreated = true
                    };

                    IdentityServices.AddUserValidators<User>(services, options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<User>>>();

                    Assert.Single(validators.Where(v => v is UserCreatedRequiredValidator<User>));
                }

                [Fact]
                public void NotRequireCreatedWithImplementingUser_DoesNotAddCreatedValidator()
                {
                    var services = new ServiceCollection();
                    var options = new UserOptions()
                    {
                        RequireCreated = false
                    };

                    IdentityServices.AddUserValidators<User>(services, options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<User>>>();

                    Assert.Empty(validators.Where(v => v is UserCreatedRequiredValidator<User>));
                }

                [Fact]
                public void RequireSecurityStampWithBlankUser_DoesNotAddCreatedValidator()
                {
                    var services = new ServiceCollection();
                    var options = new UserOptions()
                    {
                        RequireSecurityStamp = true
                    };

                    IdentityServices.AddUserValidators<object>(services, options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<User>>>();

                    Assert.Empty(validators.Where(v => v is UserSecurityStampRequiredValidator<User>));
                }

                [Fact]
                public void RequireSecurityStampWithImplementingUser_AddsSecurityStampValidator()
                {
                    var services = new ServiceCollection();
                    var options = new UserOptions()
                    {
                        RequireSecurityStamp = true
                    };

                    IdentityServices.AddUserValidators<User>(services, options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<User>>>();

                    Assert.Single(validators.Where(v => v is UserSecurityStampRequiredValidator<User>));
                }

                [Fact]
                public void NotRequireSecurityStampWithImplementingUser_DoesNotAddSecurityStampValidator()
                {
                    var services = new ServiceCollection();
                    var options = new UserOptions()
                    {
                        RequireSecurityStamp = false
                    };

                    IdentityServices.AddUserValidators<User>(services, options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<User>>>();

                    Assert.Empty(validators.Where(v => v is UserSecurityStampRequiredValidator<User>));
                }

                [Fact]
                public void RequireEmailWithBlankUser_DoesNotAddEmailValidator()
                {
                    var services = new ServiceCollection();
                    var options = new UserOptions()
                    {
                        RequireEmail = true
                    };

                    IdentityServices.AddUserValidators<object>(services, options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<User>>>();

                    Assert.Empty(validators.Where(v => v is UserEmailRequiredValidator<User>));
                }

                [Fact]
                public void RequireEmailWithImplementingUser_AddsEmailValidator()
                {
                    var services = new ServiceCollection();
                    var options = new UserOptions()
                    {
                        RequireEmail = true
                    };

                    IdentityServices.AddUserValidators<User>(services, options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<User>>>();

                    Assert.Single(validators.Where(v => v is UserEmailRequiredValidator<User>));
                }

                [Fact]
                public void NotRequireEmailWithImplementingUser_DoesNotAddEmailValidator()
                {
                    var services = new ServiceCollection();
                    var options = new UserOptions()
                    {
                        RequireEmail = false
                    };

                    IdentityServices.AddUserValidators<User>(services, options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<User>>>();

                    Assert.Empty(validators.Where(v => v is UserEmailRequiredValidator<User>));
                }

                [Fact]
                public void RequirePasswordWithBlankUser_DoesNotAddPasswordValidator()
                {
                    var services = new ServiceCollection();
                    var options = new UserOptions()
                    {
                        RequirePassword = true
                    };

                    IdentityServices.AddUserValidators<object>(services, options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<User>>>();

                    Assert.Empty(validators.Where(v => v is UserPasswordHashRequiredValidator<User>));
                }

                [Fact]
                public void RequirePasswordWithImplementingUser_AddsPasswordValidator()
                {
                    var services = new ServiceCollection();
                    var options = new UserOptions()
                    {
                        RequirePassword = true
                    };

                    IdentityServices.AddUserValidators<User>(services, options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<User>>>();

                    Assert.Single(validators.Where(v => v is UserPasswordHashRequiredValidator<User>));
                }

                [Fact]
                public void NotRequirePasswordWithImplementingUser_DoesNotAddPasswordValidator()
                {
                    var services = new ServiceCollection();
                    var options = new UserOptions()
                    {
                        RequirePassword = false
                    };

                    IdentityServices.AddUserValidators<User>(services, options);

                    var provider = services.BuildServiceProvider();

                    var validators = provider.GetRequiredService<IEnumerable<IUserValidator<User>>>();

                    Assert.Empty(validators.Where(v => v is UserPasswordHashRequiredValidator<User>));
                }
            }
        }

        public class AddUserStoreMethod
        {
            public class ServicesOverload
            {
                [Fact]
                public void NullServices_Throws()
                {
                    Assert.Throws<ArgumentNullException>("services", () =>
                    {
                        IdentityServices.AddUserStore<User, FakeUserStore>(services: null);
                    });
                }

                [Fact]
                public void WhenCalled_AddsRetrieveOperation()
                {
                    var services = new ServiceCollection();

                    IdentityServices.AddUserStore<User, FakeUserStore>(services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IRetrieveOperation<UserKey, User>>());
                }

                [Fact]
                public void WhenCalled_AddsCreateOperation()
                {
                    var services = new ServiceCollection();

                    IdentityServices.AddUserStore<User, FakeUserStore>(services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<ICreateOperation<User>>());
                }

                [Fact]
                public void WhenCalled_AddsUpdateOperation()
                {
                    var services = new ServiceCollection();

                    IdentityServices.AddUserStore<User, FakeUserStore>(services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IUpdateOperation<User>>());
                }

                [Fact]
                public void WhenCalled_AddsDeleteOperation()
                {
                    var services = new ServiceCollection();

                    IdentityServices.AddUserStore<User, FakeUserStore>(services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IDeleteOperation<UserKey>>());
                }

                [Fact]
                public void WhenCalled_AddsCrudStore()
                {
                    var services = new ServiceCollection();

                    IdentityServices.AddUserStore<User, FakeUserStore>(services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<ICrudStore<UserKey, User>>());
                }
            }
        }

        public class AddUserEmailStoreMethod
        {
            public class ServicesOverload
            {
                [Fact]
                public void NullServices_Throws()
                {
                    Assert.Throws<ArgumentNullException>("services", () =>
                    {
                        IdentityServices.AddUserEmailStore<User, FakeUserEmailStore>(
                            services: null);
                    });
                }

                [Fact]
                public void WhenCalled_AddsRetrieveOperation()
                {
                    var services = new ServiceCollection();

                    IdentityServices.AddUserEmailStore<User, FakeUserEmailStore>(services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IRetrieveOperation<EmailAddress, User>>());
                }
            }
        }

        public class AddSingleUseTokenServiceMethod
        {
            public class ServicesOverload
            {
                [Fact]
                public void NullServices_Throws()
                {
                    Assert.Throws<ArgumentNullException>("services", () =>
                    {
                        IdentityServices.AddSingleUseTokenService<FakeSingleUseTokenService>(
                            services: null);
                    });
                }

                [Fact]
                public void WhenCalled_AddsSingleUseTokenService()
                {
                    var services = new ServiceCollection();

                    IdentityServices.AddSingleUseTokenService<FakeSingleUseTokenService>(
                        services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<ISingleUseTokenService>());
                    Assert.True(provider.GetRequiredService<ISingleUseTokenService>() is FakeSingleUseTokenService);
                }
            }
        }

        public class AddTimedLockoutStoreMethod
        {
            public class ServicesOverload
            {
                [Fact]
                public void NullServices_Throws()
                {
                    Assert.Throws<ArgumentNullException>("services", () =>
                    {
                        IdentityServices.AddTimedLockoutStore<FakeTimedLockoutStore>(
                            services: null);
                    });
                }

                [Fact]
                public void WhenCalled_AddsSingleUseTokenService()
                {
                    var services = new ServiceCollection();

                    IdentityServices.AddTimedLockoutStore<FakeTimedLockoutStore>(
                        services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<ICrudStore<string, TimedLockout>>());
                    Assert.True(provider.GetRequiredService<ICrudStore<string, TimedLockout>>() is FakeTimedLockoutStore);
                }
            }
        }

        public class AddUserClaimsMethod
        {
            public class ServicesOverload
            {
                [Fact]
                public void NullServices_Throws()
                {
                    Assert.Throws<ArgumentNullException>("services", () =>
                    {
                        IdentityServices.AddUserClaims<User>(services: null);
                    });
                }

                [Fact]
                public void WhenCalled_AddsUserClaimsConverter()
                {
                    var services = new ServiceCollection();
                    IdentityServices.AddOptions(services, new IdentityOptions());
                    IdentityServices.AddInfrastructure(services);

                    // Act
                    IdentityServices.AddUserClaims<User>(services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IUserClaimsConverter<User>>());
                    Assert.True(provider.GetRequiredService<IUserClaimsConverter<User>>() is UserClaimsConverter<User>);
                }

                [Fact]
                public void WhenCalled_AddsAuthenticationService()
                {
                    var services = new ServiceCollection();
                    IdentityServices.AddOptions(services, new IdentityOptions());
                    IdentityServices.AddInfrastructure(services);
                    IdentityServices.AddUserStore<User, FakeUserStore>(services);

                    // Act
                    IdentityServices.AddUserClaims<User>(services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IAuthenticationService<ClaimsPrincipal>>());
                    Assert.True(provider.GetRequiredService<IAuthenticationService<ClaimsPrincipal>>() is ClaimsAuthenticationService<User>);
                }
            }
        }

        public class AddIdentityServicesMethod
        {
            public class ServicesOverload
            {
                [Fact]
                public void NullServices_Throws()
                {
                    Assert.Throws<ArgumentNullException>("services", () =>
                    {
                        IdentityServices.AddIdentityServices<User>(services: null);
                    });
                }

                [Fact]
                public void NotNullServices_AddsIAuthenticationService()
                {
                    var services = Services();
                    IdentityServices.AddOptions(services, new IdentityOptions());
                    IdentityServices.AddInfrastructure(services);
                    IdentityServices.AddPasswords(services);
                    IdentityServices.AddUserHelpers<User>(services);
                    IdentityServices.AddUserValidators<User>(services, new UserOptions());
                    IdentityServices.AddSingleUseTokenService<FakeSingleUseTokenService>(
                        services);
                    IdentityServices.AddTimedLockoutStore<FakeTimedLockoutStore>(services);
                    IdentityServices.AddUserEmailStore<User, FakeUserEmailStore>(services);
                    IdentityServices.AddUserStore<User, FakeUserStore>(services);
                    IdentityServices.AddUserClaims<User>(services);

                    // Act
                    IdentityServices.AddIdentityServices<User>(services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IAuthenticationService<User>>());
                    Assert.True(provider.GetRequiredService<IAuthenticationService<User>>() is IdentityService<User>);
                }

                [Fact]
                public void NotNullServices_AddsIIdentityServiceBase()
                {
                    var services = new ServiceCollection();
                    IdentityServices.AddOptions(services, new IdentityOptions());
                    IdentityServices.AddInfrastructure(services);
                    IdentityServices.AddPasswords(services);
                    IdentityServices.AddUserHelpers<User>(services);
                    IdentityServices.AddUserValidators<User>(services, new UserOptions());
                    IdentityServices.AddSingleUseTokenService<FakeSingleUseTokenService>(
                        services);
                    IdentityServices.AddTimedLockoutStore<FakeTimedLockoutStore>(services);
                    IdentityServices.AddUserEmailStore<User, FakeUserEmailStore>(services);
                    IdentityServices.AddUserStore<User, FakeUserStore>(services);
                    IdentityServices.AddUserClaims<User>(services);

                    // Act
                    IdentityServices.AddIdentityServices<User>(services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IIdentityServiceBase<User>>());
                    Assert.True(provider.GetRequiredService<IIdentityServiceBase<User>>() is IdentityService<User>);
                }

                [Fact]
                public void NotNullServices_AddsIIdentityService()
                {
                    var services = new ServiceCollection();
                    IdentityServices.AddOptions(services, new IdentityOptions());
                    IdentityServices.AddInfrastructure(services);
                    IdentityServices.AddPasswords(services);
                    IdentityServices.AddUserHelpers<User>(services);
                    IdentityServices.AddUserValidators<User>(services, new UserOptions());
                    IdentityServices.AddSingleUseTokenService<FakeSingleUseTokenService>(
                        services);
                    IdentityServices.AddTimedLockoutStore<FakeTimedLockoutStore>(services);
                    IdentityServices.AddUserEmailStore<User, FakeUserEmailStore>(services);
                    IdentityServices.AddUserStore<User, FakeUserStore>(services);
                    IdentityServices.AddUserClaims<User>(services);

                    // Act
                    IdentityServices.AddIdentityServices<User>(services);

                    var provider = services.BuildServiceProvider();

                    Assert.NotNull(provider.GetRequiredService<IIdentityService<User>>());
                    Assert.True(provider.GetRequiredService<IIdentityService<User>>() is IdentityService<User>);
                }
            }
        }

        private static ServiceCollection Services() => new ServiceCollection();
    }
}