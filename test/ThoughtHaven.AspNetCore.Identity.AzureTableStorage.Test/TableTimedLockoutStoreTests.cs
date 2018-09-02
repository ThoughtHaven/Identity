using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Lockouts;
using ThoughtHaven.AspNetCore.Identity.AzureTableStorage.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.AzureTableStorage
{
    public class TableTimedLockoutStoreTests
    {
        public class Constructor
        {
            public class PrimaryOverload
            {
                [Fact]
                public void NullOptions_Throws()
                {
                    Assert.Throws<ArgumentNullException>("options", () =>
                    {
                        new TableTimedLockoutStore(options: null);
                    });
                }
            }
        }

        public class RetrieveMethod
        {
            public class KeyOverload
            {
                [Fact]
                public async Task NullKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("key", async () =>
                    {
                        await Store().Retrieve(key: null);
                    });
                }

                [Fact]
                public async Task EmptyKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("key", async () =>
                    {
                        await Store().Retrieve(key: "");
                    });
                }

                [Fact]
                public async Task WhiteSpaceKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("key", async () =>
                    {
                        await Store().Retrieve(key: " ");
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsRetrieveOnModelStore()
                {
                    var store = Store();

                    await store.Retrieve("key");

                    Assert.Equal("key", store.ModelStore.Retrieve_InputKey);
                }

                [Fact]
                public async Task RetrieveOnModelStoreReturnsNull_ReturnsNull()
                {
                    var store = Store();
                    store.ModelStore.Retrieve_Output = null;

                    var result = await store.Retrieve("key");

                    Assert.Null(result);
                }

                [Fact]
                public async Task WhenCalled_ReturnsLockout()
                {
                    var store = Store();

                    var result = await store.Retrieve("key");

                    Assert.Equal(store.ModelStore.Retrieve_Output.Key, result.Key);
                    Assert.Equal(store.ModelStore.Retrieve_Output.LastModified,
                        result.LastModified);
                    Assert.Equal(store.ModelStore.Retrieve_Output.FailedAccessAttempts,
                        result.FailedAccessAttempts);
                    Assert.Equal(store.ModelStore.Retrieve_Output.Expiration,
                        result.Expiration);
                }
            }
        }

        public class CreateMethod
        {
            public class KeyOverload
            {
                [Fact]
                public async Task NullLockout_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("lockout", async () =>
                    {
                        await Store().Create(lockout: null);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsCreateOnModelStore()
                {
                    var store = Store();
                    var lockout = Lockout();

                    await store.Create(lockout);

                    Assert.Equal(lockout.Key, store.ModelStore.Create_InputModel.Key);
                    Assert.Equal(lockout.LastModified,
                        store.ModelStore.Create_InputModel.LastModified);
                    Assert.Equal(lockout.FailedAccessAttempts,
                        store.ModelStore.Create_InputModel.FailedAccessAttempts);
                    Assert.Equal(lockout.Expiration,
                        store.ModelStore.Create_InputModel.Expiration);
                }

                [Fact]
                public async Task WhenCalled_ReturnsLockout()
                {
                    var store = Store();
                    var lockout = Lockout();

                    var result = await store.Create(lockout);

                    Assert.Equal(lockout, result);
                }
            }
        }

        public class UpdateMethod
        {
            public class KeyOverload
            {
                [Fact]
                public async Task NullLockout_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("lockout", async () =>
                    {
                        await Store().Update(lockout: null);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsUpdateOnModelStore()
                {
                    var store = Store();
                    var lockout = Lockout();

                    await store.Update(lockout);

                    Assert.Equal(lockout.Key, store.ModelStore.Update_InputModel.Key);
                    Assert.Equal(lockout.LastModified,
                        store.ModelStore.Update_InputModel.LastModified);
                    Assert.Equal(lockout.FailedAccessAttempts,
                        store.ModelStore.Update_InputModel.FailedAccessAttempts);
                    Assert.Equal(lockout.Expiration,
                        store.ModelStore.Update_InputModel.Expiration);
                }

                [Fact]
                public async Task WhenCalled_ReturnsLockout()
                {
                    var store = Store();
                    var lockout = Lockout();

                    var result = await store.Update(lockout);

                    Assert.Equal(lockout, result);
                }
            }
        }

        public class DeleteMethod
        {
            public class KeyOverload
            {
                [Fact]
                public async Task NullKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("key", async () =>
                    {
                        await Store().Delete(key: null);
                    });
                }

                [Fact]
                public async Task EmptyKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("key", async () =>
                    {
                        await Store().Delete(key: "");
                    });
                }

                [Fact]
                public async Task WhiteSpaceKey_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentException>("key", async () =>
                    {
                        await Store().Delete(key: " ");
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsDeleteOnModelStore()
                {
                    var store = Store();

                    await store.Delete("key");

                    Assert.Equal("key", store.ModelStore.Delete_InputKey);
                }
            }
        }

        public class CreateEntityKeysMethod
        {
            public class KeyOverload
            {
                [Fact]
                public void NullKey_Throws()
                {
                    Assert.Throws<ArgumentNullException>("key", () =>
                    {
                        FakeTableTimedLockoutStore.CreateEntityKeys(key: null);
                    });
                }

                [Fact]
                public void WhenCalled_ReturnsKeys()
                {
                    var keys = FakeTableTimedLockoutStore.CreateEntityKeys("key");

                    Assert.Equal("key", keys.PartitionKey);
                    Assert.Equal("Lockout", keys.RowKey);
                }
            }
        }
        
        private static TableStoreConfiguration Options() =>
            new TableStoreConfiguration("UseDevelopmentStorage=true;");
        private static FakeTableTimedLockoutStore Store() =>
            new FakeTableTimedLockoutStore(new FakeTimedLockoutCrudStore());
        private static TimedLockout Lockout() =>
            new TimedLockout("key", DateTimeOffset.UtcNow)
            {
                FailedAccessAttempts = 5,
                Expiration = DateTimeOffset.UtcNow.AddDays(1),
            };
    }
}