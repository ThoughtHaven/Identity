using ThoughtHaven.AspNetCore.Identity.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Credentials
{
    public class UserCredentialsHelperTests
    {
        public class InvalidCredentialsProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void WhenCalled_ReturnsExpectedMessage()
                {
                    UserHelper helper = new FakeUserHelper1();

                    Assert.Equal("Those credentials weren't right. Go ahead and try again.",
                        helper.InvalidCredentials.Message);
                }
            }
        }
    }
}