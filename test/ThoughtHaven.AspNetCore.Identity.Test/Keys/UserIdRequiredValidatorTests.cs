using System;
using System.Threading.Tasks;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Keys
{
    public class UserIdRequiredValidatorTests
    {
        public class Type
        {
            [Fact]
            public void ImplementsIUserValidator()
            {
                Assert.True(typeof(IUserValidator<User>)
                    .IsAssignableFrom(typeof(UserIdRequiredValidator<User>)));
            }

            [Fact]
            public void RequiresIUserIdentifier()
            {
                var type = typeof(UserIdRequiredValidator<>);
                var tUser = type.GetGenericArguments()[0];

                Assert.True(typeof(IUserId).IsAssignableFrom(tUser));
            }
        }

        public class ValidateMethod
        {
            public class UserOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    var validator = new UserIdRequiredValidator<User>();

                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await validator.Validate(user: null!);
                    });
                }

                [Fact]
                public async Task NullId_Throws()
                {
                    var validator = new UserIdRequiredValidator<User>();
                    var user = new User()
                    {
                        Id = null
                    };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await validator.Validate(user);
                    });
                }

                [Fact]
                public async Task EmptyId_Throws()
                {
                    var validator = new UserIdRequiredValidator<User>();
                    var user = new User()
                    {
                        Id = string.Empty,
                    };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await validator.Validate(user);
                    });
                }

                [Fact]
                public async Task WhiteSpaceId_Throws()
                {
                    var validator = new UserIdRequiredValidator<User>();

                    var user = new User()
                    {
                        Id = " ",
                    };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await validator.Validate(user);
                    });
                }

                [Fact]
                public async Task WhenCalled_Succeeds()
                {
                    var validator = new UserIdRequiredValidator<User>();
                    var user = new User()
                    {
                        Id = "979"
                    };

                    await validator.Validate(user);
                }
            }
        }
    }
}