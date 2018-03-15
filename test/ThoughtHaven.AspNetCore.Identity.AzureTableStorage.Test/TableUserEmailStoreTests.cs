using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ThoughtHaven.AspNetCore.Identity.Stores.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Stores
{
    public class TableUserEmailStoreTests
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
                        new TableUserEmailStore<User>(
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
                        new TableUserEmailStore<User>(
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
                        new TableUserEmailStore<User>(
                            account: Account(),
                            requestOptions: RequestOptions(),
                            storeOptions: null);
                    });
                }
            }
        }

        public class RetrieveMethod
        {
            public class EmailOverload
            {
                [Fact]
                public async Task NullEmail_Throws()
                {
                    await Assert.ThrowsAsync<ArgumentNullException>("email", async () =>
                    {
                        await Store().Retrieve(email: null);
                    });
                }

                [Fact]
                public async Task WhenCalled_CallsEnsureExistsOnExistenceTester()
                {
                    var store = Store();

                    await store.Retrieve(email: "some@email.com");

                    Assert.Equal(store.EntityStore.Table,
                        store.EntityStore.ExistenceTester.EnsureExists_InputTable);
                }

                [Fact]
                public async Task WhenCalled_CallsExecuteQuerySegmentedOnTable()
                {
                    var store = Store();

                    await store.Retrieve(email: "some@email.com");

                    Assert.Equal("Email eq 'some@email.com'",
                        store.EntityStore.Table
                            .ExecuteQuerySegmentedAsync_InputQuery.FilterString);
                    Assert.NotNull(store.EntityStore.Table
                        .ExecuteQuerySegmentedAsync_InputToken);
                    Assert.Equal(store.EntityStore.Options,
                        store.EntityStore.Table.ExecuteQuerySegmentedAsync_InputRequestOptions);
                    Assert.Null(store.EntityStore.Table
                        .ExecuteQuerySegmentedAsync_InputOperationContext);
                }

                [Fact]
                public async Task ExecuteQuerySegmentedOnTableReturnsNull_ReturnsNull()
                {
                    var store = Store();
                    store.EntityStore.Table.ExecuteQuerySegmentedAsync_Output = null;

                    var result = await store.Retrieve(email: "some@email.com");

                    Assert.Null(result);
                }

                [Fact]
                public async Task ExecuteQuerySegmentedOnTableReturnsZeroResults_ReturnsNull()
                {
                    var store = Store();
                    store.EntityStore.Table.ExecuteQuerySegmentedAsync_Output =
                        FakeCloudTable.CreateSegment(results: 0);

                    var result = await store.Retrieve(email: "some@email.com");

                    Assert.Null(result);
                }

                [Fact]
                public async Task ExecuteQuerySegmentedOnTableReturnsTwoResults_Throws()
                {
                    var store = Store();
                    store.EntityStore.Table.ExecuteQuerySegmentedAsync_Output =
                        FakeCloudTable.CreateSegment(results: 2);

                    await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                    {
                        await store.Retrieve(email: "some@email.com");
                    });
                }

                [Fact]
                public async Task WhenCalled_ReturnsUser()
                {
                    var store = Store();

                    var result = await store.Retrieve(email: "some@email.com");

                    Assert.Equal("some@email.com", result.Email);
                }
            }
        }

        private static CloudStorageAccount Account() =>
            CloudStorageAccount.DevelopmentStorageAccount;
        private static TableRequestOptions RequestOptions() => new TableRequestOptions();
        private static TableStoreOptions StoreOptions() => new TableStoreOptions();
        private static FakeTableUserEmailStore Store() =>
            new FakeTableUserEmailStore(new FakeTableEntityStore());
    }
}