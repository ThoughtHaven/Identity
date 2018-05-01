using Microsoft.WindowsAzure.Storage.Table;
using System;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.AzureTableStorage
{
    public class TableStoreOptionsTests
    {
        public class TableRequestProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsValue()
                {
                    var options = Options();

                    Assert.NotNull(options.TableRequest);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void NullValue_Throws()
                {
                    var options = Options();

                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        options.TableRequest = null;
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var request = new TableRequestOptions();
                    var options = Options();

                    options.TableRequest = request;

                    Assert.Equal(request, options.TableRequest);
                }
            }
        }

        public class UserStoreTableNameProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsIdentityUsers()
                {
                    var options = Options();

                    Assert.Equal("IdentityUsers", options.UserStoreTableName);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void NullValue_Throws()
                {
                    var options = Options();

                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        options.UserStoreTableName = null;
                    });
                }

                [Fact]
                public void EmptyValue_Throws()
                {
                    var options = Options();

                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        options.UserStoreTableName = "";
                    });
                }

                [Fact]
                public void WhiteSpaceValue_Throws()
                {
                    var options = Options();

                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        options.UserStoreTableName = " ";
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var options = Options();
                    options.UserStoreTableName = "OtherName";

                    Assert.Equal("OtherName", options.UserStoreTableName);
                }
            }
        }

        public class TimedLockoutStoreTableNameProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsIdentityTimedLockouts()
                {
                    var options = Options();

                    Assert.Equal("IdentityTimedLockouts", options.TimedLockoutStoreTableName);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void NullValue_Throws()
                {
                    var options = Options();

                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        options.TimedLockoutStoreTableName = null;
                    });
                }

                [Fact]
                public void EmptyValue_Throws()
                {
                    var options = Options();

                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        options.TimedLockoutStoreTableName = "";
                    });
                }

                [Fact]
                public void WhiteSpaceValue_Throws()
                {
                    var options = Options();

                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        options.TimedLockoutStoreTableName = " ";
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var options = Options();

                    options.TimedLockoutStoreTableName = "OtherName";

                    Assert.Equal("OtherName", options.TimedLockoutStoreTableName);
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
                        new TableStoreOptions(storageAccountConnectionString: null);
                    });
                }

                [Fact]
                public void EmptyStorageAccountConnectionString_Throws()
                {
                    Assert.Throws<ArgumentException>("storageAccountConnectionString", () =>
                    {
                        new TableStoreOptions(storageAccountConnectionString: "");
                    });
                }

                [Fact]
                public void WhiteSpaceStorageAccountConnectionString_Throws()
                {
                    Assert.Throws<ArgumentException>("storageAccountConnectionString", () =>
                    {
                        new TableStoreOptions(storageAccountConnectionString: " ");
                    });
                }

                [Fact]
                public void WhenCalled_SetsStorageAccountConnectionString()
                {
                    var options = new TableStoreOptions("ConnectionString");

                    Assert.Equal("ConnectionString", options.StorageAccountConnectionString);
                }
            }
        }

        private static TableStoreOptions Options() =>
            new TableStoreOptions("UseDevelopmentStorage=true;");
    }
}