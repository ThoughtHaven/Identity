using System;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity
{
    public class UserTests
    {
        public class KeyMethod
        {
            public class EmptyOverload
            {
                [Fact]
                public void NoId_ReturnsNull()
                {
                    var user = new User() { Id = null };

                    Assert.Null(user.Key());
                }

                [Fact]
                public void EmptyId_ReturnsNull()
                {
                    var user = new User() { Id = "" };

                    Assert.Null(user.Key());
                }

                [Fact]
                public void WhiteSpaceId_ReturnsNull()
                {
                    var user = new User() { Id = " " };

                    Assert.Null(user.Key());
                }

                [Fact]
                public void WhenCalled_ReturnsValue()
                {
                    var user = new User() { Id = "id" };

                    Assert.Equal(user.Id, user.Key());
                }
            }
        }

        public class IdProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsNull()
                {
                    var user = new User();

                    Assert.Null(user.Id);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var user = new User { Id = "id" };

                    Assert.Equal("id", user.Id);
                }
            }
        }

        public class EmailProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsNull()
                {
                    var user = new User();

                    Assert.Null(user.Email);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var user = new User { Email = "some@email.com" };

                    Assert.Equal("some@email.com", user.Email);
                }
            }
        }

        public class EmailConfirmedProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsFalse()
                {
                    var user = new User();

                    Assert.False(user.EmailConfirmed);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var user = new User { EmailConfirmed = true };

                    Assert.True(user.EmailConfirmed);
                }
            }
        }

        public class PasswordHashProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsNull()
                {
                    var user = new User();

                    Assert.Null(user.PasswordHash);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var user = new User { PasswordHash = "hash" };

                    Assert.Equal("hash", user.PasswordHash);
                }
            }
        }

        public class SecurityStampProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsNull()
                {
                    var user = new User();

                    Assert.Null(user.SecurityStamp);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var user = new User { SecurityStamp = "stamp" };

                    Assert.Equal("stamp", user.SecurityStamp);
                }
            }
        }

        public class CreatedProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsNotDefault()
                {
                    var user = new User();

                    Assert.NotEqual(default(DateTimeOffset), user.Created);
                }

                [Fact]
                public void DefaultValue_ReturnsUtcNow()
                {
                    var user = new User();
                    var now = DateTimeOffset.UtcNow;

                    Assert.True(user.Created <= now);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var created = DateTimeOffset.UtcNow;

                    var user = new User { Created = created };

                    Assert.Equal(created, user.Created);
                }
            }
        }

        public class LastLoginProperty
        {
            public class GetAccessor
            {
                [Fact]
                public void DefaultValue_ReturnsNull()
                {
                    var user = new User();

                    Assert.Null(user.LastLogin);
                }
            }

            public class SetAccessor
            {
                [Fact]
                public void WhenCalled_SetsValue()
                {
                    var lastLogin = DateTimeOffset.UtcNow;

                    var user = new User { LastLogin = lastLogin };

                    Assert.Equal(lastLogin, user.LastLogin);
                }
            }
        }
    }
}