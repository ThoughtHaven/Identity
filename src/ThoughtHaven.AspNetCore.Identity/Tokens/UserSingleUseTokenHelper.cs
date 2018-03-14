using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.Internal;
using ThoughtHaven.Security.Cryptography;
using ThoughtHaven.Security.SingleUseTokens;

namespace ThoughtHaven.AspNetCore.Identity
{
    public abstract partial class UserHelper
    {
        private readonly CryptoRandom _random = new CryptoRandom();

        protected abstract ISingleUseTokenService SingleUseTokenService { get; }

        protected virtual SingleUseToken CreateSingleUseToken(string prefix, UserKey userKey,
            VerificationCode code)
        {
            Guard.NullOrWhiteSpace(nameof(prefix), prefix);
            Guard.Null(nameof(userKey), userKey);
            Guard.Null(nameof(code), code);

            return new SingleUseToken($"{prefix}-{userKey.Value}-{code.Value}");
        }
    }
}