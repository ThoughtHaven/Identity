using System;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Lockouts
{
    public class TimedLockoutTests
    {
        public class Constructor
        {
            public class KeyAndLastModifiedOverload
            {
                [Fact]
                public void NullKey_Throws()
                {
                    Assert.Throws<ArgumentNullException>("key", () =>
                    {
                        new TimedLockout(
                            key: null,
                            lastModified: DateTimeOffset.UtcNow);
                    });
                }

                [Fact]
                public void EmptyKey_Throws()
                {
                    Assert.Throws<ArgumentException>("key", () =>
                    {
                        new TimedLockout(
                            key: string.Empty,
                            lastModified: DateTimeOffset.UtcNow);
                    });
                }

                [Fact]
                public void WhiteSpaceKey_Throws()
                {
                    Assert.Throws<ArgumentException>("key", () =>
                    {
                        new TimedLockout(
                            key: " ",
                            lastModified: DateTimeOffset.UtcNow);
                    });
                }

                [Fact]
                public void WhenCalled_SetsCorrectProperties()
                {
                    var key = "key";
                    var lastModified = DateTimeOffset.UtcNow;

                    var data = new TimedLockout(key, lastModified);

                    Assert.Equal(key, data.Key);
                    Assert.Equal(lastModified, data.LastModified);
                }
            }
        }

        public class LastModifiedProperty
        {
            public class SetOperator
            {
                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var data = new TimedLockout("key", DateTimeOffset.UtcNow);
                    var lastModified = DateTimeOffset.UtcNow.AddDays(1);

                    data.LastModified = lastModified;

                    Assert.Equal(lastModified, data.LastModified);
                }
            }
        }

        public class FailedAccessAttemptsProperty
        {
            public class GetOperator
            {
                [Fact]
                public void DefaultValue_Equals1()
                {
                    var data = new TimedLockout("key", DateTimeOffset.UtcNow);

                    Assert.Equal(1, data.FailedAccessAttempts);
                }
            }

            public class SetOperator
            {
                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var data = new TimedLockout("key", DateTimeOffset.UtcNow);
                    byte failedAttempts = 5;

                    data.FailedAccessAttempts = failedAttempts;

                    Assert.Equal(failedAttempts, data.FailedAccessAttempts);
                }
            }
        }

        public class ExpirationProperty
        {
            public class GetOperator
            {
                [Fact]
                public void DefaultValue_ReturnsNull()
                {
                    var data = new TimedLockout("key", DateTimeOffset.UtcNow);

                    Assert.Null(data.Expiration);
                }
            }

            public class SetOperator
            {
                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var data = new TimedLockout("key", DateTimeOffset.UtcNow);
                    var expiration = DateTimeOffset.UtcNow.AddDays(1);

                    data.Expiration = expiration;

                    Assert.Equal(expiration, data.Expiration);
                }
            }
        }
    }
}