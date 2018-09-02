using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.AzureTableStorage.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.AzureTableStorage
{
    public class TableUserEmailStoreTests
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
                        new TableUserEmailStore<User>(options: null);
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
        
        private static TableStoreConfiguration Options() =>
            new TableStoreConfiguration("UseDevelopmentStorage=true;");
        private static FakeTableUserEmailStore Store() =>
            new FakeTableUserEmailStore(new FakeTableEntityStore());
    }
}