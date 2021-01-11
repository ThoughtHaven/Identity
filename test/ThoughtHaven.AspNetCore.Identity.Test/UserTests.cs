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
                    var user = new User() { Id = null! };

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

                    Assert.NotEqual(default, user.Created);
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

        public class Constructor
        {
            public class UserOverload
            {
                [Fact]
                public void NullUser_Throws()
                {
                    Assert.Throws<ArgumentNullException>("user", () =>
                    {
                        new User(user: null!);
                    });
                }

                [Theory]
                [InlineData("id1")]
                [InlineData("id2")]
                public void WhenCalled_SetsId(string value)
                {
                    var user = User(id: value);

                    Assert.Equal(value, user.Id);
                }

                [Theory]
                [InlineData("email1@email.com")]
                [InlineData("email2@email.com")]
                public void WhenCalled_SetsEmail(string value)
                {
                    var user = User(email: value);

                    Assert.Equal(value, user.Email);
                }

                [Theory]
                [InlineData(true)]
                [InlineData(false)]
                public void WhenCalled_SetsEmailConfirmed(bool value)
                {
                    var user = User(emailConfirmed: value);

                    Assert.Equal(value, user.EmailConfirmed);
                }

                [Theory]
                [InlineData("hash1")]
                [InlineData("hash2")]
                public void WhenCalled_SetsPasswordHash(string value)
                {
                    var user = User(passwordHash: value);

                    Assert.Equal(value, user.PasswordHash);
                }

                [Theory]
                [InlineData("stamp1")]
                [InlineData("stamp2")]
                public void WhenCalled_SetsSecurityStamp(string value)
                {
                    var user = User(securityStamp: value);

                    Assert.Equal(value, user.SecurityStamp);
                }

                [Theory]
                [InlineData(1000000)]
                [InlineData(2000000)]
                public void WhenCalled_SetsCreated(long value)
                {
                    var offset = new DateTimeOffset(ticks: value, TimeSpan.Zero);

                    var user = User(created: offset);

                    Assert.Equal(offset, user.Created);
                }

                [Theory]
                [InlineData(1000000)]
                [InlineData(2000000)]
                public void WhenCalled_SetsLastLogin(long value)
                {
                    var offset = new DateTimeOffset(ticks: value, TimeSpan.Zero);

                    var user = User(lastLogin: offset);

                    Assert.Equal(offset, user.LastLogin);
                }
            }
        }

        private static User User(string id = "id", string email = "some@email.com", bool emailConfirmed = false,
            string passwordHash = "hash", string securityStamp = "stamp", DateTimeOffset? created = null,
            DateTimeOffset? lastLogin = null) =>
            new User()
            {
                Id = id,
                Email = email,
                EmailConfirmed = emailConfirmed,
                PasswordHash = passwordHash,
                SecurityStamp = securityStamp,
                Created = created ?? DateTimeOffset.UtcNow,
                LastLogin = lastLogin,
            };
    }
}