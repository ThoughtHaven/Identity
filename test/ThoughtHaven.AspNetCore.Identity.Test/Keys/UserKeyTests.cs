using System;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Keys
{
    public class UserKeyTests
    {
        public class Type
        {
            [Fact]
            public void InheritsValueObjectOfString()
            {
                Assert.True(typeof(ValueObject<string>)
                    .IsAssignableFrom(typeof(UserKey)));
            }
        }

        public class Constructor
        {
            public class ValueOverload
            {
                [Fact]
                public void NullValue_Throws()
                {
                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        new UserKey(value: null!);
                    });
                }

                [Fact]
                public void EmptyValue_Throws()
                {
                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        new UserKey(value: string.Empty);
                    });
                }

                [Fact]
                public void WhiteSpaceValue_Throws()
                {
                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        new UserKey(value: " ");
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var value = "key";

                    var key = new UserKey(value);

                    Assert.Equal(value, key.Value);
                }
            }
        }

        public class UserKeyOperator
        {
            public class ValueOverload
            {
                [Fact]
                public void WhenCalled_ReturnsUserKey()
                {
                    UserKey key = "key";

                    Assert.Equal("key", key.Value);
                }
            }
        }
    }
}