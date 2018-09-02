using System;
using Microsoft.WindowsAzure.Storage.Table;
using ThoughtHaven.AspNetCore.Identity.AzureTableStorage;
using ThoughtHaven.Security.SingleUseTokens.AzureTableStorage;
using Xunit;

namespace Microsoft.Extensions.DependencyInjection
{
    public class TableIdentityConfigurationTests
    {
        public class TableRequestProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsNotNull()
                {
                    var options = TableStorageIdentity();

                    Assert.NotNull(options.TableRequest);
                }

                [Fact]
                public void DefaultValue_ReturnsTableStoreRequestOptions()
                {
                    var options = TableStorageIdentity();

                    Assert.Equal(options.TableStore.TableRequest, options.TableRequest);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void NullValue_Throws()
                {
                    var options = TableStorageIdentity();

                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        options.TableRequest = null;
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var tableRequest = new TableRequestOptions();
                    var options = TableStorageIdentity();

                    options.TableRequest = tableRequest;

                    Assert.Equal(tableRequest, options.TableRequest);
                }

                [Fact]
                public void WhenCalled_SetsTableStoreTableRequest()
                {
                    var tableRequest = new TableRequestOptions();
                    var options = TableStorageIdentity();

                    options.TableRequest = tableRequest;

                    Assert.Equal(tableRequest, options.TableStore.TableRequest);
                }

                [Fact]
                public void WhenCalled_SetsSingleUseTokenTableRequest()
                {
                    var tableRequest = new TableRequestOptions();
                    var options = TableStorageIdentity();

                    options.TableRequest = tableRequest;

                    Assert.Equal(tableRequest, options.SingleUseToken.TableRequest);
                }
            }
        }

        public class TableStoreProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsNotNull()
                {
                    var options = TableStorageIdentity();

                    Assert.NotNull(options.TableStore);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void NullValue_Throws()
                {
                    var options = TableStorageIdentity();

                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        options.TableStore = null;
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var tableStore = TableStore();
                    var options = TableStorageIdentity();
                    options.TableStore = tableStore;

                    Assert.Equal(tableStore, options.TableStore);
                }
            }
        }

        public class SingleUseTokenProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsNotNull()
                {
                    var options = TableStorageIdentity();

                    Assert.NotNull(options.SingleUseToken);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void NullValue_Throws()
                {
                    var options = TableStorageIdentity();
                    
                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        options.SingleUseToken = null;
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var singleUseToken = SingleUseToken();
                    var options = TableStorageIdentity();

                    options.SingleUseToken = singleUseToken;

                    Assert.Equal(singleUseToken, options.SingleUseToken);
                }
            }
        }

        public class Constructor
        {
            public class StorageAccountConnectionStringOverload
            {
                [Fact]
                public void NullStorageAccountConnectionString_Throws()
                {
                    Assert.Throws<ArgumentNullException>("storageAccountConnectionString", () =>
                    {
                        new TableIdentityOptions(
                            storageAccountConnectionString: null);
                    });
                }

                [Fact]
                public void EmptyStorageAccountConnectionString_Throws()
                {
                    Assert.Throws<ArgumentException>("storageAccountConnectionString", () =>
                    {
                        new TableIdentityOptions(storageAccountConnectionString: "");
                    });
                }

                [Fact]
                public void WhiteSpaceStorageAccountConnectionString_Throws()
                {
                    Assert.Throws<ArgumentException>("storageAccountConnectionString", () =>
                    {
                        new TableIdentityOptions(storageAccountConnectionString: " ");
                    });
                }

                [Fact]
                public void WhenCalled_SetsTableStoreWithConnectionString()
                {
                    var options = new TableIdentityOptions("ConnectionString");

                    Assert.NotNull(options.TableStore);
                    Assert.Equal("ConnectionString",
                        options.TableStore.StorageAccountConnectionString);
                }

                [Fact]
                public void WhenCalled_SetsSingleUseTokenWithConnectionString()
                {
                    var options = new TableIdentityOptions("ConnectionString");

                    Assert.NotNull(options.SingleUseToken);
                    Assert.Equal("ConnectionString",
                        options.SingleUseToken.StorageAccountConnectionString);
                }
            }

            public class TableStoreAndSingleUseTokenOverload
            {
                [Fact]
                public void NullTableStore_Throws()
                {
                    Assert.Throws<ArgumentNullException>("tableStore", () =>
                    {
                        new TableIdentityOptions(
                            tableStore: null,
                            singleUseToken: SingleUseToken());
                    });
                }

                [Fact]
                public void NullSingleUseToken_Throws()
                {
                    Assert.Throws<ArgumentNullException>("singleUseToken", () =>
                    {
                        new TableIdentityOptions(
                            tableStore: TableStore(),
                            singleUseToken: null);
                    });
                }

                [Fact]
                public void WhenCalled_SetsTableStore()
                {
                    var tableStore = TableStore();
                    var singleUseToken = SingleUseToken();

                    var options = new TableIdentityOptions(tableStore,
                        singleUseToken);

                    Assert.Equal(tableStore, options.TableStore);
                }

                [Fact]
                public void WhenCalled_SetsSingleUseToken()
                {
                    var tableStore = TableStore();
                    var singleUseToken = SingleUseToken();

                    var options = new TableIdentityOptions(tableStore,
                        singleUseToken);

                    Assert.Equal(singleUseToken, options.SingleUseToken);
                }
            }
        }

        private static TableSingleUseTokenOptions SingleUseToken() =>
            new TableSingleUseTokenOptions("UseDevelopmentStorage=true;");
        private static TableStoreOptions TableStore() =>
            new TableStoreOptions("UseDevelopmentStorage=true;");
        private static TableIdentityOptions TableStorageIdentity() =>
            new TableIdentityOptions("UseDevelopmentStorage=true;");
    }
}