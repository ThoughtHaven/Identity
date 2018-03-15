using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ThoughtHaven.AspNetCore.Identity.Stores.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Stores
{
    public class TableUserStoreTests
    {
        public class Constructor
        {
            public class PrimaryOverload
            {
                [Fact]
                public void NullAccount_Throws()
                {
                    Assert.Throws<ArgumentNullException>("account", () =>
                    {
                        new TableUserStore<User>(
                            account: null,
                            requestOptions: RequestOptions(),
                            storeOptions: StoreOptions());
                    });
                }

                [Fact]
                public void NullRequestOptions_Throws()
                {
                    Assert.Throws<ArgumentNullException>("requestOptions", () =>
                    {
                        new TableUserStore<User>(
                            account: Account(),
                            requestOptions: null,
                            storeOptions: StoreOptions());
                    });
                }

                [Fact]
                public void NullStoreOptions_Throws()
                {
                    Assert.Throws<ArgumentNullException>("storeOptions", () =>
                    {
                        new TableUserStore<User>(
                            account: Account(),
                            requestOptions: RequestOptions(),
                            storeOptions: null);
                    });
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
                        FakeTableUserStore.CreateEntityKeys(key: null);
                    });
                }

                [Fact]
                public void WhenCalled_ReturnsKeys()
                {
                    var keys = FakeTableUserStore.CreateEntityKeys("key");

                    Assert.Equal("key", keys.PartitionKey);
                    Assert.Equal("User", keys.RowKey);
                }
            }
        }

        private static CloudStorageAccount Account() =>
            CloudStorageAccount.DevelopmentStorageAccount;
        private static TableRequestOptions RequestOptions() => new TableRequestOptions();
        private static TableStoreOptions StoreOptions() => new TableStoreOptions();
    }
}