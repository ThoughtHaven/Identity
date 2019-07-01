using System;
using System.Threading.Tasks;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Emails
{
    public class UserEmailRequiredValidatorTests
    {
        public class Type
        {
            [Fact]
            public void ImplementsIUserValidator()
            {
                Assert.True(typeof(IUserValidator<User>)
                    .IsAssignableFrom(typeof(UserEmailRequiredValidator<User>)));
            }

            [Fact]
            public void RequiresIUserEmail()
            {
                var type = typeof(UserEmailRequiredValidator<>);
                var tUser = type.GetGenericArguments()[0];

                Assert.True(typeof(IUserEmail).IsAssignableFrom(tUser));
            }
        }

        public class ValidateMethod
        {
            public class UserOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await new UserEmailRequiredValidator<User>().Validate(user: null!);
                    });
                }

                [Fact]
                public async Task NullEmail_Throws()
                {
                    var user = new User()
                    {
                        Email = null
                    };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await new UserEmailRequiredValidator<User>().Validate(user);
                    });
                }

                [Fact]
                public async Task EmptyEmail_Throws()
                {
                    var user = new User()
                    {
                        Email = string.Empty
                    };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await new UserEmailRequiredValidator<User>().Validate(user);
                    });
                }

                [Fact]
                public async Task WhiteSpaceEmail_Throws()
                {
                    var user = new User()
                    {
                        Email = " ",
                    };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await new UserEmailRequiredValidator<User>().Validate(user);
                    });
                }

                [Fact]
                public async Task WhenCalled_Succeeds()
                {
                    var user = new User()
                    {
                        Email = "some@email.com"
                    };

                    await new UserEmailRequiredValidator<User>().Validate(user);
                }
            }
        }
    }
}