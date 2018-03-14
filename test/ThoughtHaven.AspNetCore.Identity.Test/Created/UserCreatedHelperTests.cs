using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Created
{
    public class UserCreatedHelperTests
    {
        public class SetCreatedMethod
        {
            public class UserOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    var helper = new FakeUserHelper1();

                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await helper.SetCreated<User>(user: null);
                    });
                }

                [Fact]
                public async Task WhenCalled_SetsCreated()
                {
                    var helper = new FakeUserHelper1();
                    var user = new User();

                    await helper.SetCreated(user);

                    Assert.Equal(helper.FakeClock.UtcNow, user.Created);
                }
            }
        }

        private static FakeSystemClock Clock() => new FakeSystemClock(DateTimeOffset.UtcNow);
    }
}