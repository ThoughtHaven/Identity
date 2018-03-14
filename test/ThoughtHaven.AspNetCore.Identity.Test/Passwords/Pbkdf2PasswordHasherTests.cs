using System;
using System.Linq;
using System.Security.Cryptography;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public class Pbkdf2PasswordHasherTests
    {
        public class Constructor
        {
            public class IterationsOverload
            {
                [Fact]
                public void IterationsBelow1_Throws()
                {
                    Assert.Throws<ArgumentOutOfRangeException>("iterations", () =>
                    {
                        new FakePbkdf2PasswordHasher(iterations: 0);
                    });
                }

                [Fact]
                public void DefaultIterations_SetsIterationsTo25000()
                {
                    var hasher = new FakePbkdf2PasswordHasher();

                    Assert.Equal(25_000, hasher.Iterations);
                }

                [Fact]
                public void WhenCalled_SetsIterations()
                {
                    var hasher = new FakePbkdf2PasswordHasher(iterations: 1);

                    Assert.Equal(1, hasher.Iterations);
                }
            }
        }

        public class HashMethod
        {
            public class PasswordOverload
            {
                [Fact]
                public void NullPassword_Throws()
                {
                    Assert.Throws<ArgumentNullException>("password", () =>
                    {
                        new FakePbkdf2PasswordHasher().Hash(password: null);
                    });
                }

                [Fact]
                public void IterationsEquals1_PrependsIterationsToHash()
                {
                    var hash = Hasher().Hash("password");

                    Assert.StartsWith("1.", hash);
                }

                [Fact]
                public void IterationsEquals25000_PrependsIterationsToHash()
                {
                    var hash = Hasher(iterations: 25000).Hash("password");

                    Assert.StartsWith("25000.", hash);
                }

                [Fact]
                public void IterationsEquals100000_PrependsIterationsToHash()
                {
                    var hash = Hasher(iterations: 100000).Hash("password");

                    Assert.StartsWith("100000.", hash);
                }

                [Fact]
                public void WhenCalled_ReturnsHash()
                {
                    var hash = Hasher().Hash("password");

                    var parsed = ParseHash(hash);

                    var expected = Hash("password", parsed.salt, parsed.iterations);

                    Assert.Equal(expected, hash);
                }
            }
        }

        public class ValidateMethod
        {
            public class HashAndPasswordOverload
            {
                [Fact]
                public void NullHash_Throws()
                {
                    Assert.Throws<ArgumentNullException>("hash", () =>
                    {
                        Hasher().Validate(
                            hash: null,
                            password: "password");
                    });
                }

                [Fact]
                public void EmptyHash_Throws()
                {
                    Assert.Throws<ArgumentException>("hash", () =>
                    {
                        Hasher().Validate(
                            hash: "",
                            password: "password");
                    });
                }

                [Fact]
                public void WhiteSpaceHash_Throws()
                {
                    Assert.Throws<ArgumentException>("hash", () =>
                    {
                        Hasher().Validate(
                            hash: " ",
                            password: "password");
                    });
                }

                [Fact]
                public void NullPassword_Throws()
                {
                    Assert.Throws<ArgumentNullException>("password", () =>
                    {
                        Hasher().Validate(
                            hash: "hash",
                            password: null);
                    });
                }

                [Fact]
                public void MalformedHash_ReturnsNotValid()
                {
                    var hasher = Hasher();
                    var hash = "bad";

                    Assert.False(hasher.Validate(hash, "password").Valid);
                }

                [Fact]
                public void MalformedIterationsInHash_ReturnsNotValid()
                {
                    var hasher = Hasher();
                    var hash = "notanumber.bad";

                    Assert.False(hasher.Validate(hash, "password").Valid);
                }

                [Fact]
                public void MalformedExpectedHashInHash_ReturnsNotValid()
                {
                    var hasher = Hasher();
                    var hash = "1.not48bytehash";

                    Assert.False(hasher.Validate(hash, "password").Valid);
                }

                [Fact]
                public void ValidPassword_ReturnsValid()
                {
                    var hasher = Hasher();
                    var hash = hasher.Hash("password");

                    Assert.True(hasher.Validate(hash, "password").Valid);
                }

                [Fact]
                public void ValidPasswordAndEqualIterations_ReturnsUpdateHashFalse()
                {
                    var hasher = Hasher();
                    var hash = hasher.Hash("password");

                    Assert.False(hasher.Validate(hash, "password").UpdateHash);
                }

                [Fact]
                public void ValidPasswordAndFewerIterations_ReturnsUpdateHashFalse()
                {
                    var hasher = Hasher(iterations: 2);
                    var hash = hasher.Hash("password");

                    hasher = Hasher(iterations: 1);

                    Assert.False(hasher.Validate(hash, "password").UpdateHash);
                }

                [Fact]
                public void ValidPasswordAndMoreIterations_ReturnsUpdateHashTrue()
                {
                    var hasher = Hasher(iterations: 1);
                    var hash = hasher.Hash("password");

                    hasher = Hasher(iterations: 2);

                    Assert.True(hasher.Validate(hash, "password").UpdateHash);
                }

                [Fact]
                public void InvalidPassword_ReturnsNotValid()
                {
                    var hasher = Hasher();
                    var hash = hasher.Hash("password");

                    Assert.False(hasher.Validate(hash, "not-password").Valid);
                }

                [Fact]
                public void InvalidPasswordAndEqualIterations_UpdateHashFalse()
                {
                    var hasher = Hasher();
                    var hash = hasher.Hash("password");

                    Assert.False(hasher.Validate(hash, "not-password").UpdateHash);
                }

                [Fact]
                public void InvalidPasswordAndFewerIterations_UpdateHashFalse()
                {
                    var hasher = Hasher(iterations: 2);
                    var hash = hasher.Hash("password");
                    hasher = Hasher(iterations: 1);

                    Assert.False(hasher.Validate(hash, "not-password").UpdateHash);
                }

                [Fact]
                public void InvalidPasswordAndMoreIterations_UpdateHashFalse()
                {
                    var hasher = Hasher(iterations: 1);
                    var hash = hasher.Hash("password");
                    hasher = Hasher(iterations: 2);

                    Assert.False(hasher.Validate(hash, "not-password").UpdateHash);
                }
            }
        }

        private static FakePbkdf2PasswordHasher Hasher(int iterations = 1) =>
            new FakePbkdf2PasswordHasher(iterations);
        private static (int iterations, byte[] salt, byte[] hash) ParseHash(string hash)
        {
            var split = hash.Split(new string[] { "." }, StringSplitOptions.None);

            var iterations = int.Parse(split[0]);

            var concat = Convert.FromBase64String(split[1]);
            var salt = concat.Take(16).ToArray();
            var cipher = concat.Skip(16).Take(32).ToArray();

            return (iterations, salt, cipher);
        }
        private static string Hash(Password password, byte[] salt, int iterations)
        {
            using (var cipher = new Rfc2898DeriveBytes(password.Value, salt, iterations))
            {
                var hash = cipher.GetBytes(32);

                var concat = salt.Concat(hash).ToArray();

                return $"{iterations}.{Convert.ToBase64String(concat)}";
            }
        }
    }
}