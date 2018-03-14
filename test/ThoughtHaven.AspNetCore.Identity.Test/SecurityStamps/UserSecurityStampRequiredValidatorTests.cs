using System;
using System.Threading.Tasks;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.SecurityStamps
{
    public class UserSecurityStampRequiredValidatorTests
    {
        public class Type
        {
            [Fact]
            public void ImplementsIUserValidator()
            {
                Assert.True(typeof(IUserValidator<User>)
                    .IsAssignableFrom(typeof(UserSecurityStampRequiredValidator<User>)));
            }

            [Fact]
            public void RequiresIUserPasswordHash()
            {
                var type = typeof(UserSecurityStampRequiredValidator<>);
                var tUser = type.GetGenericArguments()[0];

                Assert.True(typeof(IUserSecurityStamp).IsAssignableFrom(tUser));
            }
        }

        public class ValidateMethod
        {
            public class UserOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    var validator = new UserSecurityStampRequiredValidator<User>();

                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await validator.Validate(user: null);
                    });
                }

                [Fact]
                public async Task NullSecurityStamp_Throws()
                {
                    var validator = new UserSecurityStampRequiredValidator<User>();
                    var user = new User()
                    {
                        SecurityStamp = null
                    };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await validator.Validate(user);
                    });
                }

                [Fact]
                public async Task EmptySecurityStamp_Throws()
                {
                    var validator = new UserSecurityStampRequiredValidator<User>();
                    var user = new User()
                    {
                        SecurityStamp = string.Empty,
                    };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await validator.Validate(user);
                    });
                }

                [Fact]
                public async Task WhiteSpaceSecurityStamp_Throws()
                {
                    var validator = new UserSecurityStampRequiredValidator<User>();

                    var user = new User()
                    {
                        SecurityStamp = " ",
                    };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await validator.Validate(user);
                    });
                }

                [Fact]
                public async Task ValidUser_Succeeds()
                {
                    var validator = new UserSecurityStampRequiredValidator<User>();
                    var user = new User()
                    {
                        SecurityStamp = "stamp"
                    };

                    await validator.Validate(user);
                }
            }
        }
    }
}