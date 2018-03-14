using System;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public class PasswordValidateResultTests
    {
        public class Constructor
        {
            public class EmptyOverload
            {
                [Fact]
                public void WhenCalled_SetsValidToFalse()
                {
                    var result = new PasswordValidateResult();

                    Assert.False(result.Valid);
                }

                [Fact]
                public void WhenCalled_SetsUpdateHashToFalse()
                {
                    var result = new PasswordValidateResult();
                    
                    Assert.False(result.UpdateHash);
                }
            }

            public class ValidAndUpdateHashOverload
            {
                [Fact]
                public void ValidFalseAndUpdateHashTrue_Throws()
                {
                    Assert.Throws<ArgumentException>("updateHash", () =>
                    {
                        new PasswordValidateResult(valid: false, updateHash: true);
                    });
                }

                [Fact]
                public void ValidFalse_SetsValidToFalse()
                {
                    var result = new PasswordValidateResult(valid: false);

                    Assert.False(result.Valid);
                }

                [Fact]
                public void ValidTrue_SetsValidToTrue()
                {
                    var result = new PasswordValidateResult(valid: true);

                    Assert.True(result.Valid);
                }

                [Fact]
                public void DefaultUpdateHash_SetsUpdateHashToFalse()
                {
                    var result = new PasswordValidateResult(valid: true);

                    Assert.False(result.UpdateHash);
                }

                [Fact]
                public void UpdateHashFalse_SetsUpdateHashToFalse()
                {
                    var result = new PasswordValidateResult(valid: true, updateHash: false);

                    Assert.False(result.UpdateHash);
                }

                [Fact]
                public void UpdateHashTrue_SetsUpdateHashToTrue()
                {
                    var result = new PasswordValidateResult(valid: true, updateHash: true);

                    Assert.True(result.UpdateHash);
                }
            }
        }
    }
}