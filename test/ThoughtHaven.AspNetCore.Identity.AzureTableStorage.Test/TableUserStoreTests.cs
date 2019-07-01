﻿using System;
using ThoughtHaven.AspNetCore.Identity.AzureTableStorage.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.AzureTableStorage
{
    public class TableUserStoreTests
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
                        new TableUserStore<User>(options: null!);
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
                        FakeTableUserStore.CreateEntityKeys(key: null!);
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
        
        private static TableStoreOptions StoreOptions() =>
            new TableStoreOptions("UseDevelopmentStorage=true;");
    }
}