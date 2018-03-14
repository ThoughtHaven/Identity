using System;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Internal
{
    public class VerificationCodeTests
    {
        public class Type
        {
            [Fact]
            public void InheritsValueObject()
            {
                Assert.True(typeof(ValueObject<int>).IsAssignableFrom(
                    typeof(VerificationCode)));
            }
        }

        public class Constructor
        {
            public class ValueOverload
            {
                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var valueObject = new VerificationCode(1234);

                    Assert.Equal(1234, valueObject.Value);
                }

                [Fact]
                public void ValueLessThanZero_Throws()
                {
                    Assert.Throws<ArgumentOutOfRangeException>("value", () =>
                    {
                        new VerificationCode(value: -1);
                    });
                }

                [Fact]
                public void ValueUnder4Characters_Throws()
                {
                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        new VerificationCode(value: 123);
                    });
                }
            }
        }

        public class VerificationCodeOperators
        {
            public class ValueOverload
            {
                [Fact]
                public void WhenCalled_ReturnsCode()
                {
                    VerificationCode code = 1234;

                    Assert.Equal(1234, code.Value);
                }
            }
        }
    }
}