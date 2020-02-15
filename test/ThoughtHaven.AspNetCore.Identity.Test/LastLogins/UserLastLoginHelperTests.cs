using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.LastLogins
{
    public class UserLastLoginHelperTests
    {
        public class SetLastLoginMethod
        {
            public class UserOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await new FakeUserHelper1().SetLastLogin<User>(user: null!);
                    });
                }

                [Fact]
                public async Task WhenCalled_SetsLastLogin()
                {
                    var helper = new FakeUserHelper1();
                    var user = new User();

                    await helper.SetLastLogin(user);

                    Assert.Equal(helper.FakeClock.UtcNow.ToOffset(), user.LastLogin);
                }
            }
        }
    }
}