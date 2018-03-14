using System;
using System.Threading.Tasks;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public class UserPasswordHashRequiredValidatorTests
    {
        public class Type
        {
            [Fact]
            public void ImplementsIUserValidator()
            {
                Assert.True(typeof(IUserValidator<User>)
                    .IsAssignableFrom(typeof(UserPasswordHashRequiredValidator<User>)));
            }

            [Fact]
            public void RequiresIUserPasswordHash()
            {
                var type = typeof(UserPasswordHashRequiredValidator<>);
                var tUser = type.GetGenericArguments()[0];

                Assert.True(typeof(IUserPasswordHash).IsAssignableFrom(tUser));
            }
        }

        public class ValidateAsyncMethod
        {
            public class UserOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    var validator = new UserPasswordHashRequiredValidator<User>();

                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await validator.Validate(user: null);
                    });
                }

                [Fact]
                public async Task NullPasswordHash_Throws()
                {
                    var validator = new UserPasswordHashRequiredValidator<User>();
                    var user = new User()
                    {
                        PasswordHash = null
                    };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await validator.Validate(user);
                    });
                }

                [Fact]
                public async Task EmptyPasswordHash_Throws()
                {
                    var validator = new UserPasswordHashRequiredValidator<User>();
                    var user = new User()
                    {
                        PasswordHash = string.Empty,
                    };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await validator.Validate(user);
                    });
                }

                [Fact]
                public async Task WhiteSpacePasswordHash_Throws()
                {
                    var validator = new UserPasswordHashRequiredValidator<User>();

                    var user = new User()
                    {
                        PasswordHash = " ",
                    };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await validator.Validate(user);
                    });
                }

                [Fact]
                public async Task ValidUser_Succeeds()
                {
                    var validator = new UserPasswordHashRequiredValidator<User>();
                    var user = new User()
                    {
                        PasswordHash = "hash"
                    };

                    await validator.Validate(user);
                }
            }
        }
    }
}