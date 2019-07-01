using System;
using System.Security.Claims;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Claims
{
    public class ClaimOptionsTests
    {
        public class IssuerProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsCorrectDefault()
                {
                    var options = new ClaimOptions();

                    Assert.Equal(ClaimsIdentity.DefaultIssuer, options.Issuer);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void NullValue_Throws()
                {
                    var options = new ClaimOptions();

                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        options.Issuer = null!;
                    });
                }

                [Fact]
                public void EmptyValue_Throws()
                {
                    var options = new ClaimOptions();

                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        options.Issuer = string.Empty;
                    });
                }

                [Fact]
                public void WhiteSpaceValue_Throws()
                {
                    var options = new ClaimOptions();

                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        options.Issuer = " ";
                    });
                }

                [Fact]
                public void WhenCalled_UpdatesValue()
                {
                    var options = new ClaimOptions();
                    var value = Guid.NewGuid().ToString();

                    options.Issuer = value;

                    Assert.Equal(value, options.Issuer);
                }
            }
        }

        public class AuthenticationSchemeProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsCorrectDefault()
                {
                    var options = new ClaimOptions();

                    Assert.Equal("ThoughtHavenIdentity", options.AuthenticationScheme);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void NullValue_Throws()
                {
                    var options = new ClaimOptions();

                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        options.AuthenticationScheme = null!;
                    });
                }

                [Fact]
                public void EmptyValue_Throws()
                {
                    var options = new ClaimOptions();

                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        options.AuthenticationScheme = string.Empty;
                    });
                }

                [Fact]
                public void WhiteSpaceValue_Throws()
                {
                    var options = new ClaimOptions();

                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        options.AuthenticationScheme = " ";
                    });
                }

                [Fact]
                public void WhenCalled_UpdatesValue()
                {
                    var options = new ClaimOptions();
                    var value = Guid.NewGuid().ToString();

                    options.AuthenticationScheme = value;

                    Assert.Equal(value, options.AuthenticationScheme);
                }
            }
        }

        public class ValidateSecurityStampTimeSpanProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_Returns30MinutesDefault()
                {
                    var options = new ClaimOptions();

                    Assert.Equal(TimeSpan.FromMinutes(30), options.ValidateSecurityStampInterval);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void WhenCalled_UpdatesValue()
                {
                    var options = new ClaimOptions();
                    var value = TimeSpan.FromMinutes(60);

                    options.ValidateSecurityStampInterval = value;

                    Assert.Equal(value, options.ValidateSecurityStampInterval);
                }
            }
        }

        public class ClaimTypesProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsValue()
                {
                    var options = new ClaimOptions();

                    Assert.NotNull(options.ClaimTypes);
                }
            }
        }

        public class ClaimTypeOptionsTests
        {
            public class UserKeyProperty
            {
                public class GetAccessor
                {
                    [Fact]
                    public void DefaultValue_ReturnsCorrectDefault()
                    {
                        var options = new ClaimOptions.ClaimTypeOptions();

                        Assert.Equal("sub", options.UserKey);
                    }
                }

                public class SetAccessor
                {
                    [Fact]
                    public void NullValue_Throws()
                    {
                        var options = new ClaimOptions.ClaimTypeOptions();

                        Assert.Throws<ArgumentNullException>("value", () =>
                        {
                            options.UserKey = null!;
                        });
                    }

                    [Fact]
                    public void EmptyValue_Throws()
                    {
                        var options = new ClaimOptions.ClaimTypeOptions();

                        Assert.Throws<ArgumentException>("value", () =>
                        {
                            options.UserKey = string.Empty;
                        });
                    }

                    [Fact]
                    public void WhiteSpaceValue_Throws()
                    {
                        var options = new ClaimOptions.ClaimTypeOptions();

                        Assert.Throws<ArgumentException>("value", () =>
                        {
                            options.UserKey = " ";
                        });
                    }

                    [Fact]
                    public void WhenCalled_SetsValue()
                    {
                        var options = new ClaimOptions.ClaimTypeOptions();
                        var value = Guid.NewGuid().ToString();

                        options.UserKey = value;

                        Assert.Equal(value, options.UserKey);
                    }
                }
            }

            public class SecurityStampProperty
            {
                public class GetAccessor
                {
                    [Fact]
                    public void DefaultValue_ReturnsCorrectDefault()
                    {
                        var options = new ClaimOptions.ClaimTypeOptions();

                        Assert.Equal("securitystamp", options.SecurityStamp);
                    }
                }

                public class SetAccessor
                {
                    [Fact]
                    public void NullValue_Throws()
                    {
                        var options = new ClaimOptions.ClaimTypeOptions();

                        Assert.Throws<ArgumentNullException>("value", () =>
                        {
                            options.SecurityStamp = null!;
                        });
                    }

                    [Fact]
                    public void EmptyValue_Throws()
                    {
                        var options = new ClaimOptions.ClaimTypeOptions();

                        Assert.Throws<ArgumentException>("value", () =>
                        {
                            options.SecurityStamp = string.Empty;
                        });
                    }

                    [Fact]
                    public void WhiteSpaceValue_Throws()
                    {
                        var options = new ClaimOptions.ClaimTypeOptions();

                        Assert.Throws<ArgumentException>("value", () =>
                        {
                            options.SecurityStamp = " ";
                        });
                    }

                    [Fact]
                    public void WhenCalled_UpdatesValue()
                    {
                        var options = new ClaimOptions.ClaimTypeOptions();
                        var value = Guid.NewGuid().ToString();

                        options.SecurityStamp = value;

                        Assert.Equal(value, options.SecurityStamp);
                    }
                }
            }

            public class SecurityStampValidatedProperty
            {
                public class GetAccessor
                {
                    [Fact]
                    public void DefaultValue_ReturnsCorrectDefault()
                    {
                        var options = new ClaimOptions.ClaimTypeOptions();

                        Assert.Equal("securitystampvalidated", options.SecurityStampValidated);
                    }
                }

                public class SetAccessor
                {
                    [Fact]
                    public void NullValue_Throws()
                    {
                        var options = new ClaimOptions.ClaimTypeOptions();

                        Assert.Throws<ArgumentNullException>("value", () =>
                        {
                            options.SecurityStampValidated = null!;
                        });
                    }

                    [Fact]
                    public void EmptyValue_Throws()
                    {
                        var options = new ClaimOptions.ClaimTypeOptions();

                        Assert.Throws<ArgumentException>("value", () =>
                        {
                            options.SecurityStampValidated = string.Empty;
                        });
                    }

                    [Fact]
                    public void WhiteSpaceValue_Throws()
                    {
                        var options = new ClaimOptions.ClaimTypeOptions();

                        Assert.Throws<ArgumentException>("value", () =>
                        {
                            options.SecurityStampValidated = "   ";
                        });
                    }

                    [Fact]
                    public void WhenCalled_UpdatesValue()
                    {
                        var options = new ClaimOptions.ClaimTypeOptions();
                        var value = Guid.NewGuid().ToString();

                        options.SecurityStampValidated = value;

                        Assert.Equal(value, options.SecurityStampValidated);
                    }
                }
            }
        }
    }
}