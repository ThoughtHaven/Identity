using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Lockouts
{
    public class UserLockoutHelperTests
    {
        public class LockedOutProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void WhenCalled_ReturnsExpectedMessage()
                {
                    UserHelper helper = new FakeUserHelper1();

                    Assert.Equal("This account has been locked to protect it from possible hacking. Wait a few minutes to try again.",
                        helper.LockedOut.Message);
                }
            }
        }

        public class IsLockedOutMethod
        {
            public class KeyOverload
            {
                [Fact]
                public async Task NullKey_Throws()
                {
                    UserHelper helper = new FakeUserHelper1();

                    await Assert.ThrowsAsync<ArgumentNullException>("key", async () =>
                    {
                        await helper.IsLockedOut(key: null!);
                    });
                }

                [Fact]
                public async Task EmptyKey_Throws()
                {
                    UserHelper helper = new FakeUserHelper1();

                    await Assert.ThrowsAsync<ArgumentException>("key", async () =>
                    {
                        await helper.IsLockedOut(key: "");
                    });
                }

                [Fact]
                public async Task WhiteSpaceKey_Throws()
                {
                    UserHelper helper = new FakeUserHelper1();

                    await Assert.ThrowsAsync<ArgumentException>("key", async () =>
                    {
                        await helper.IsLockedOut(key: " ");
                    });
                }

                [Fact]
                public async Task KeyDoesNotExist_CallsRetrieveOnLockoutStore()
                {
                    var helper = new FakeUserHelper1();

                    _ = await helper.IsLockedOut("key");

                    Assert.Equal("key", helper.FakeTimedLockoutStore.Retrieve_KeyInput);
                }

                [Fact]
                public async Task KeyDoesNotExist_ReturnsFalse()
                {
                    var helper = new FakeUserHelper1();
                    helper.FakeTimedLockoutStore.Retrieve_Output = null;

                    var result = await helper.IsLockedOut("key");

                    Assert.False(result);
                }

                [Fact]
                public async Task KeyDoesNotExist_AddsData()
                {
                    var helper = new FakeUserHelper1();
                    helper.FakeTimedLockoutStore.Retrieve_Output = null;

                    _ = await helper.IsLockedOut("key");

                    var created = helper.FakeTimedLockoutStore.Create_InputData;

                    Assert.Equal("key", created!.Key);
                    Assert.Equal(helper.FakeClock.UtcNow, created.LastModified);
                    Assert.Equal(1, created.FailedAccessAttempts);
                    Assert.Null(created.Expiration);
                }

                [Fact]
                public async Task UnderMaxFailedAccessAttempts_ReturnsFalse()
                {
                    var helper = new FakeUserHelper1();

                    var lockout = new TimedLockout("key", helper.FakeClock.UtcNow)
                    {
                        FailedAccessAttempts = 1
                    };

                    helper.FakeTimedLockoutStore.Retrieve_Output = lockout;

                    // Act
                    var result = await helper.IsLockedOut("key");

                    Assert.False(result);
                }

                [Fact]
                public async Task UnderMaxFailedAccessAttempts_UpdatesData()
                {
                    var helper = new FakeUserHelper1();

                    var lockout = new TimedLockout("key", helper.FakeClock.UtcNow)
                    {
                        FailedAccessAttempts = 1
                    };

                    helper.FakeTimedLockoutStore.Retrieve_Output = lockout;

                    // Act
                    _ = await helper.IsLockedOut("key");

                    var updated = helper.FakeTimedLockoutStore.Update_InputData;

                    Assert.Equal("key", updated!.Key);
                    Assert.Equal(helper.FakeClock.UtcNow, updated.LastModified);
                    Assert.Equal(2, updated.FailedAccessAttempts);
                    Assert.Null(updated.Expiration);
                }

                [Fact]
                public async Task ReachesFiveFailedAccessAttempts_ReturnsTrue()
                {
                    var helper = new FakeUserHelper1();

                    var lockout = new TimedLockout("key", helper.FakeClock.UtcNow)
                    {
                        FailedAccessAttempts = 4
                    };

                    helper.FakeTimedLockoutStore.Retrieve_Output = lockout;

                    // Act
                    var result = await helper.IsLockedOut("key");

                    Assert.True(result);
                }

                [Fact]
                public async Task ReachesFiveFailedAccessAttempts_UpdatesData()
                {
                    var helper = new FakeUserHelper1();

                    var lockout = new TimedLockout("key", helper.FakeClock.UtcNow)
                    {
                        FailedAccessAttempts = 4,
                    };

                    helper.FakeTimedLockoutStore.Retrieve_Output = lockout;

                    // Act
                    _ = await helper.IsLockedOut("key");

                    var updated = helper.FakeTimedLockoutStore.Update_InputData;

                    Assert.Equal("key", updated!.Key);
                    Assert.Equal(helper.FakeClock.UtcNow, updated.LastModified);
                    Assert.Equal(5, updated.FailedAccessAttempts);
                    Assert.Equal(helper.FakeClock.UtcNow.Add(TimeSpan.FromMinutes(10)),
                        updated.Expiration!.Value);
                }

                [Fact]
                public async Task StaleLastModified_ReturnsFalse()
                {
                    var helper = new FakeUserHelper1();

                    var lockout = new TimedLockout("key", helper.FakeClock.UtcNow.AddDays(-1))
                    {
                        Expiration = null,
                        FailedAccessAttempts = 5,
                    };

                    helper.FakeTimedLockoutStore.Retrieve_Output = lockout;

                    // Act
                    var result = await helper.IsLockedOut("key");

                    Assert.False(result);
                }

                [Fact]
                public async Task StaleLastModified_ResetsData()
                {
                    var helper = new FakeUserHelper1();

                    var lockout = new TimedLockout("key", helper.FakeClock.UtcNow.AddDays(-1))
                    {
                        Expiration = null,
                        FailedAccessAttempts = 5,
                    };

                    helper.FakeTimedLockoutStore.Retrieve_Output = lockout;

                    // Act
                    _ = await helper.IsLockedOut("key");

                    var updated = helper.FakeTimedLockoutStore.Update_InputData;

                    Assert.Equal("key", updated!.Key);
                    Assert.Equal(helper.FakeClock.UtcNow, updated.LastModified);
                    Assert.Equal(1, updated.FailedAccessAttempts);
                    Assert.Null(updated.Expiration);
                }

                [Fact]
                public async Task OpenExpiration_ReturnsTrue()
                {
                    var helper = new FakeUserHelper1();
                    var expiration = helper.FakeClock.UtcNow.AddDays(1);
                    var lockout = new TimedLockout("key", helper.FakeClock.UtcNow)
                    {
                        Expiration = expiration
                    };

                    helper.FakeTimedLockoutStore.Retrieve_Output = lockout;

                    // Act
                    var result = await helper.IsLockedOut("key");

                    Assert.True(result);
                }

                [Fact]
                public async Task StaleExpiration_ReturnsFalse()
                {
                    var helper = new FakeUserHelper1();
                    var expiration = helper.FakeClock.UtcNow.AddDays(-1);
                    var lockout = new TimedLockout("key", helper.FakeClock.UtcNow)
                    {
                        Expiration = expiration,
                    };

                    helper.FakeTimedLockoutStore.Retrieve_Output = lockout;

                    // Act
                    var result = await helper.IsLockedOut("key");

                    Assert.False(result);
                }

                [Fact]
                public async Task StaleExpiration_ResetsData()
                {
                    var helper = new FakeUserHelper1();
                    var expiration = helper.FakeClock.UtcNow.AddDays(-1);
                    var lockout = new TimedLockout("key", helper.FakeClock.UtcNow)
                    {
                        Expiration = expiration,
                    };

                    helper.FakeTimedLockoutStore.Retrieve_Output = lockout;

                    // Act
                    _ = await helper.IsLockedOut("key");

                    var updated = helper.FakeTimedLockoutStore.Update_InputData;

                    Assert.Equal("key", updated!.Key);
                    Assert.Equal(helper.FakeClock.UtcNow, updated.LastModified);
                    Assert.Equal(1, updated.FailedAccessAttempts);
                    Assert.Null(updated.Expiration);
                }
            }
        }

        public class ResetLockedOutMethod
        {
            public class KeyOverload
            {
                [Fact]
                public async Task NullKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("key", async () =>
                    {
                        await new FakeUserHelper1().ResetLockedOut(key: null!);
                    });
                }

                [Fact]
                public async Task EmptyKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("key", async () =>
                    {
                        await new FakeUserHelper1().ResetLockedOut(key: "");
                    });
                }

                [Fact]
                public async Task WhiteSpaceKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("key", async () =>
                    {
                        await new FakeUserHelper1().ResetLockedOut(key: " ");
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsDeleteOnTimedLockoutStore()
                {
                    var helper = new FakeUserHelper1();

                    await helper.ResetLockedOut("key");

                    Assert.Equal("key", helper.FakeTimedLockoutStore.Delete_KeyInput);
                }
            }
        }
    }
}