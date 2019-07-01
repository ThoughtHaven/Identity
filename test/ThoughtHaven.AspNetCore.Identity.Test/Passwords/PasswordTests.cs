using System;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public class PasswordTests
    {
        public class Type
        {
            [Fact]
            public void InheritsValueObjectOfString()
            {
                Assert.True(typeof(ValueObject<string>).IsAssignableFrom(typeof(Password)));
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
                        new Password(value: null!);
                    });
                }

                [Fact]
                public void EmptyValue_Throws()
                {
                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        new Password(value: "");
                    });
                }

                [Fact]
                public void WhiteSpaceValue_Throws()
                {
                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        new Password(value: " ");
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var valueObject = new Password("password");

                    Assert.Equal("password", valueObject.Value);
                }
            }
        }
        
        public class PasswordOperator
        {
            public class ValueOverload
            {
                [Fact]
                public void WhenCalled_ReturnsPassword()
                {
                    Password email = "pw";

                    Assert.Equal("pw", email);
                }
            }
        }
    }
}