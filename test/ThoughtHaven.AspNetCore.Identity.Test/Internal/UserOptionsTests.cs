using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Internal
{
    public class UserOptionsTests
    {
        public class RequireIdProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsTrue()
                {
                    var options = new UserOptions();

                    Assert.True(options.RequireId);
                }
            }
        }

        public class RequireCreatedProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsTrue()
                {
                    var options = new UserOptions();

                    Assert.True(options.RequireCreated);
                }
            }
        }

        public class RequireSecurityStampProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsTrue()
                {
                    var options = new UserOptions();

                    Assert.True(options.RequireSecurityStamp);
                }
            }
        }

        public class RequireEmailProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsTrue()
                {
                    var options = new UserOptions();

                    Assert.True(options.RequireEmail);
                }
            }
        }

        public class RequirePasswordProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsTrue()
                {
                    var options = new UserOptions();

                    Assert.True(options.RequirePassword);
                }
            }
        }
    }
}