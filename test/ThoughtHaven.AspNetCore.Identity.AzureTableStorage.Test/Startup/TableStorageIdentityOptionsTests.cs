using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Table;
using ThoughtHaven.AspNetCore.Identity.Stores;
using ThoughtHaven.Security.SingleUseTokens;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Startup
{
    public class TableStorageIdentityOptionsTests
    {
        public class TableStoreProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsNotNull()
                {
                    var options = new TableStorageIdentityOptions();

                    Assert.NotNull(options.TableStore);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void NullValue_Throws()
                {
                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        new TableStorageIdentityOptions().TableStore = null;
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var tableStore = new TableStoreOptions();
                    var options = new TableStorageIdentityOptions()
                    {
                        TableStore = tableStore
                    };

                    Assert.Equal(tableStore, options.TableStore);
                }
            }
        }

        public class TableRequestProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsNotNull()
                {
                    var options = new TableStorageIdentityOptions();

                    Assert.NotNull(options.TableRequest);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void NullValue_Throws()
                {
                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        new TableStorageIdentityOptions().TableRequest = null;
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var tableRequest = new TableRequestOptions();
                    var options = new TableStorageIdentityOptions()
                    {
                        TableRequest = tableRequest
                    };

                    Assert.Equal(tableRequest, options.TableRequest);
                }

                [Fact]
                public void WhenCalled_SetsSingleUseTokenTableRequest()
                {
                    var tableRequest = new TableRequestOptions();
                    var options = new TableStorageIdentityOptions()
                    {
                        TableRequest = tableRequest
                    };

                    Assert.Equal(tableRequest, options.SingleUseToken.TableRequest);
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
                    var options = new TableStorageIdentityOptions();

                    Assert.NotNull(options.SingleUseToken);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void NullValue_Throws()
                {
                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        new TableStorageIdentityOptions().SingleUseToken = null;
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var singleUseToken = new TableSingleUseTokenOptions();
                    var options = new TableStorageIdentityOptions()
                    {
                        SingleUseToken = singleUseToken
                    };

                    Assert.Equal(singleUseToken, options.SingleUseToken);
                }
            }
        }
    }
}