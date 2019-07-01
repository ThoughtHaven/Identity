using System;
using System.Threading.Tasks;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Created
{
    public class UserCreatedRequiredValidatorTests
    {
        public class Type
        {
            [Fact]
            public void ImplementsIUserValidator()
            {
                Assert.True(typeof(IUserValidator<User>)
                    .IsAssignableFrom(typeof(UserCreatedRequiredValidator<User>)));
            }

            [Fact]
            public void RequiresIUserCreated()
            {
                var type = typeof(UserCreatedRequiredValidator<>);
                var tUser = type.GetGenericArguments()[0];

                Assert.True(typeof(IUserCreated).IsAssignableFrom(tUser));
            }
        }

        public class ValidateMethod
        {
            public class UserOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    var validator = new UserCreatedRequiredValidator<User>();

                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await validator.Validate(user: null!);
                    });
                }

                [Fact]
                public async Task DefaultCreated_Throws()
                {
                    var validator = new UserCreatedRequiredValidator<User>();
                    var user = new User() { Created = default };

                    await Assert.ThrowsAsync<ArgumentException>("user", async () =>
                    {
                        await validator.Validate(user);
                    });
                }

                [Fact]
                public async Task WhenCalled_Succeeds()
                {
                    var validator = new UserCreatedRequiredValidator<User>();
                    var user = new User() { Created = DateTimeOffset.UtcNow };

                    await validator.Validate(user);
                }
            }
        }
    }
}