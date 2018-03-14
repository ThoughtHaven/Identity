using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using Xunit;
using System;

namespace ThoughtHaven.AspNetCore.Identity.SecurityStamps
{
    public class UserSecurityStampHelperTests
    {
        public class RefreshSecurityStampMethod
        {
            public class UserOverload
            {
                [Fact]
                public async Task NullUser_Throws()
                {
                    UserHelper helper = new FakeUserHelper1();

                    await Assert.ThrowsAsync<ArgumentNullException>("user", async () =>
                    {
                        await helper.RefreshSecurityStamp<User>(user: null);
                    });
                }

                [Fact]
                public async Task ValidUser_SetsSecurityStamp20CharactersLong()
                {
                    UserHelper helper = new FakeUserHelper1();
                    var user = new User();

                    await helper.RefreshSecurityStamp(user);

                    Assert.NotNull(user.SecurityStamp);
                    Assert.Equal(20, user.SecurityStamp.Length);
                }
            }
        }
    }
}