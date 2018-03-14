using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Keys
{
    public class UserIdHelperTests
    {
        public class AssignUserIdAsyncMethod
        {
            public class UserOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await new FakeUserHelper1().AssignUserId<User>(user: null);
                    });
                }

                [Fact]
                public async Task WhenCalled_SetsUserIdToGuidStringWithoutDashes()
                {
                    var user = new User();

                    await new FakeUserHelper1().AssignUserId(user);

                    Assert.True(Guid.TryParse(user.Id, out var guid));
                    Assert.DoesNotContain("-", user.Id);
                }
            }
        }
    }
}