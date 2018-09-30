using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using ThoughtHaven.AspNetCore.Identity.Claims;
using ThoughtHaven.AspNetCore.Identity.Internal;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Startup
{
    public class IdentityOptionsTests
    {
        public class AuthenticationSchemeProperty
        {
            public class GetOperator
            {
                [Fact]
                public void DefaultValue_EqualsAuthenticationSchemeFromClaims()
                {
                    Assert.Equal(new ClaimOptions().AuthenticationScheme,
                        new IdentityOptions().AuthenticationScheme);
                }
            }

            public class SetOperator
            {
                [Fact]
                public void NullValue_Throws()
                {
                    var options = new IdentityOptions();

                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        options.AuthenticationScheme = null;
                    });
                }

                [Fact]
                public void EmptyValue_Throws()
                {
                    var options = new IdentityOptions();

                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        options.AuthenticationScheme = string.Empty;
                    });
                }

                [Fact]
                public void WhiteSpaceValue_Throws()
                {
                    var options = new IdentityOptions();

                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        options.AuthenticationScheme = " ";
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var options = new IdentityOptions();
                    var scheme = "Scheme";

                    options.AuthenticationScheme = scheme;

                    Assert.Equal(scheme, options.AuthenticationScheme);
                }

                [Fact]
                public void WhenCalled_SetsClaimsAuthenticationScheme()
                {
                    var options = new IdentityOptions();
                    var scheme = "Scheme";

                    options.AuthenticationScheme = scheme;

                    Assert.Equal(scheme, options.Claims.AuthenticationScheme);
                }
            }
        }

        public class ClaimsProperty
        {
            public class GetOperator
            {
                [Fact]
                public void Get_NotNullByDefault()
                {
                    var options = new IdentityOptions();

                    Assert.NotNull(options.Claims);
                }
            }

            public class SetOperator
            {
                [Fact]
                public void NullValue_Throws()
                {
                    var options = new IdentityOptions();

                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        options.Claims = null;
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var options = new IdentityOptions();
                    var claimOptions = new ClaimOptions();

                    options.Claims = claimOptions;

                    Assert.Equal(claimOptions, options.Claims);
                }
            }
        }

        public class UserProperty
        {
            public class GetOperator
            {
                [Fact]
                public void DefaultValue_NotNull()
                {
                    var options = new IdentityOptions();

                    Assert.NotNull(options.User);
                }
            }

            public class SetOperator
            {
                [Fact]
                public void NullValue_Throws()
                {
                    var options = new IdentityOptions();

                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        options.User = null;
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var options = new IdentityOptions();
                    var userOptions = new UserOptions();

                    options.User = userOptions;

                    Assert.Equal(userOptions, options.User);
                }
            }
        }

        public class CookieProperty
        {
            public class GetOperator
            {
                [Fact]
                public void DefaultValue_NotNull()
                {
                    var options = new IdentityOptions();

                    Assert.NotNull(options.Cookie);
                }

                [Fact]
                public void DefaultValueCookieNameProperty_EqualsDotIdentity()
                {
                    var options = new IdentityOptions();

                    Assert.Equal(".identity", options.Cookie.Cookie.Name);
                }

                [Fact]
                public void DefaultValueCookieHttpOnlyProperty_EqualsTrue()
                {
                    var options = new IdentityOptions();

                    Assert.True(options.Cookie.Cookie.HttpOnly);
                }

                [Fact]
                public void DefaultValueCookieSecurePolicyProperty_EqualsAlways()
                {
                    var options = new IdentityOptions();

                    Assert.Equal(CookieSecurePolicy.Always,
                        options.Cookie.Cookie.SecurePolicy);
                }

                [Fact]
                public void DefaultValueExpireTimeSpanProperty_Equals7Days()
                {
                    var options = new IdentityOptions();

                    Assert.Equal(TimeSpan.FromDays(7), options.Cookie.ExpireTimeSpan);
                }

                [Fact]
                public void DefaultValueSlidingExpirationProperty_EqualsTrue()
                {
                    var options = new IdentityOptions();

                    Assert.True(options.Cookie.SlidingExpiration);
                }

                [Fact]
                public void DefaultValueIsEssentialProperty_EqualsTrue()
                {
                    var options = new IdentityOptions();

                    Assert.True(options.Cookie.Cookie.IsEssential);
                }

                [Fact]
                public void DefaultValueSameSiteProperty_EqualsNone()
                {
                    var options = new IdentityOptions();

                    Assert.Equal(SameSiteMode.None, options.Cookie.Cookie.SameSite);
                }
            }

            public class SetOperator
            {
                [Fact]
                public void NullValue_Throws()
                {
                    var options = new IdentityOptions();

                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        options.Cookie = null;
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var options = new IdentityOptions();
                    var cookieOptions = new CookieAuthenticationOptions();

                    options.Cookie = cookieOptions;

                    Assert.Equal(cookieOptions, options.Cookie);
                }
            }
        }

        public class Constructor
        {
            public class EmptyOverload
            {
                [Fact]
                public void WhenCalled_SetsAuthenticationSchemeToClaimsDefault()
                {
                    var options = new IdentityOptions();

                    Assert.Equal(new ClaimOptions().AuthenticationScheme,
                        options.AuthenticationScheme);
                }
            }
        }
    }
}