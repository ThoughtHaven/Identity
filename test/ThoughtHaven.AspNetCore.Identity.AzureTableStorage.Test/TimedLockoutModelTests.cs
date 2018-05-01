using System;
using ThoughtHaven.AspNetCore.Identity.Lockouts;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.AzureTableStorage
{
    public class TimedLockoutModelTests
    {
        public class Constructor
        {
            public class LockoutOverload
            {
                [Fact]
                public void NullLockout_Throws()
                {
                    Assert.Throws<ArgumentNullException>("lockout", () =>
                    {
                        new TimedLockoutModel(lockout: null);
                    });
                }

                [Fact]
                public void WhenCalled_SetsKey()
                {
                    var lockout = new TimedLockout("key", lastModified: DateTimeOffset.UtcNow);

                    var model = new TimedLockoutModel(lockout);

                    Assert.Equal("key", model.Key);
                }

                [Fact]
                public void WhenCalled_SetsLastModified()
                {
                    var lockout = new TimedLockout("key", lastModified: DateTimeOffset.UtcNow);

                    var model = new TimedLockoutModel(lockout);

                    Assert.Equal(lockout.LastModified, model.LastModified);
                }

                [Fact]
                public void WhenCalled_SetsFailedAccessAttempts()
                {
                    var lockout = new TimedLockout("key", lastModified: DateTimeOffset.UtcNow)
                    {
                        FailedAccessAttempts = 5
                    };

                    var model = new TimedLockoutModel(lockout);

                    Assert.Equal(5, model.FailedAccessAttempts);
                }

                [Fact]
                public void WhenCalled_SetsExpiration()
                {
                    var lockout = new TimedLockout("key", lastModified: DateTimeOffset.UtcNow)
                    {
                        Expiration = DateTimeOffset.UtcNow
                    };

                    var model = new TimedLockoutModel(lockout);

                    Assert.Equal(lockout.Expiration, model.Expiration);
                }
            }
        }
    }
}