using System;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Stores
{
    public class TableStoreOptionsTests
    {
        public class UserStoreTableNameProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsIdentityUsers()
                {
                    var options = new TableStoreOptions();

                    Assert.Equal("IdentityUsers", options.UserStoreTableName);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void NullValue_Throws()
                {
                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        new TableStoreOptions().UserStoreTableName = null;
                    });
                }

                [Fact]
                public void EmptyValue_Throws()
                {
                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        new TableStoreOptions().UserStoreTableName = "";
                    });
                }

                [Fact]
                public void WhiteSpaceValue_Throws()
                {
                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        new TableStoreOptions().UserStoreTableName = " ";
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var options = new TableStoreOptions()
                    {
                        UserStoreTableName = "OtherName"
                    };

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
                    var options = new TableStoreOptions();

                    Assert.Equal("IdentityTimedLockouts", options.TimedLockoutStoreTableName);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void NullValue_Throws()
                {
                    Assert.Throws<ArgumentNullException>("value", () =>
                    {
                        new TableStoreOptions().TimedLockoutStoreTableName = null;
                    });
                }

                [Fact]
                public void EmptyValue_Throws()
                {
                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        new TableStoreOptions().TimedLockoutStoreTableName = "";
                    });
                }

                [Fact]
                public void WhiteSpaceValue_Throws()
                {
                    Assert.Throws<ArgumentException>("value", () =>
                    {
                        new TableStoreOptions().TimedLockoutStoreTableName = " ";
                    });
                }

                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var options = new TableStoreOptions()
                    {
                        TimedLockoutStoreTableName = "OtherName"
                    };

                    Assert.Equal("OtherName", options.TimedLockoutStoreTableName);
                }
            }
        }
    }
}