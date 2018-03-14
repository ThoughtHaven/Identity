using System;
using ThoughtHaven.AspNetCore.Identity.Internal;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public class PasswordResetCodeTests
    {
        public class Type
        {
            [Fact]
            public void InheritsValueObject()
            {
                Assert.True(typeof(VerificationCode).IsAssignableFrom(
                    typeof(PasswordResetCode)));
            }
        }

        public class Constructor
        {
            public class ValueOverload
            {
                [Fact]
                public void ValueNegativeNumber_Throws()
                {
                    Assert.Throws<ArgumentOutOfRangeException>("value", () =>
                    {
                        new PasswordResetCode(value: -1);
                    });
                }

                [Fact]
                public void ValueUnder4Characters_Throws()
                {
                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        new PasswordResetCode(value: 123);
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var valueObject = new PasswordResetCode(1234);

                    Assert.Equal(1234, valueObject.Value);
                }
            }
        }

        public class PasswordResetCodeOperator
        {
            public class ValueOverload
            {
                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var valueObject = new PasswordResetCode(1234);

                    Assert.Equal(1234, valueObject.Value);
                }
            }
        }
    }
}