using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Emails;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.Internal;
using ThoughtHaven.Contacts;

namespace ThoughtHaven.AspNetCore.Identity
{
    public static partial class IdentityHelperServiceExtensions
    {
        public static Task<TUser> Retrieve<TUser>(this IIdentityService<TUser> identity,
            EmailAddress email)
            where TUser : IUserEmail
        {
            Guard.Null(nameof(identity), identity);
            Guard.Null(nameof(email), email);

            return identity.Helper.Retrieve<TUser>(email);
        }

        public static async Task<Result<VerificationCode, UserMessage>> UpdateEmail<TUser>(
            this IIdentityService<TUser> identity, TUser user, EmailAddress email)
            where TUser : IUserKey, IUserEmail
        {
            Guard.Null(nameof(identity), identity);
            Guard.Null(nameof(user), user);
            Guard.Null(nameof(email), email);

            var key = user.Key();
            if (key == null)
            {
                throw new ArgumentException(
                    paramName: nameof(user),
                    message: $"The {nameof(user)} argument's {nameof(user.Key)}() method cannot return null.");
            }

            var existing = await identity.Retrieve(email).ConfigureAwait(false);

            if (existing != null)
            {
                if (existing.Key() == key) { return identity.Helper.UserAlreadyOwnsEmail; }

                return identity.Helper.EmailNotAvailable;
            }

            await identity.Helper.SetEmail(user, email).ConfigureAwait(false);

            await identity.Update(user).ConfigureAwait(false);

            return await identity.Helper.CreateEmailVerificationCode(key)
                .ConfigureAwait(false);
        }

        public static async Task<Result<UserMessage>> ConfirmEmail<TUser>(
            this IIdentityService<TUser> identity, TUser user, VerificationCode code)
            where TUser : IUserKey, IUserEmail
        {
            Guard.Null(nameof(identity), identity);
            Guard.Null(nameof(user), user);
            Guard.Null(nameof(code), code);

            var key = user.Key();
            if (key == null)
            {
                throw new ArgumentException(
                    paramName: nameof(user),
                    message: $"The {nameof(user)} argument's {nameof(user.Key)}() method cannot return null.");
            }

            var tokenValid = await identity.Helper.ValidateEmailVerificationCode(key,
                code).ConfigureAwait(false);

            if (!tokenValid) { return identity.Helper.InvalidEmailVerificationCode; }

            await identity.Helper.ConfirmEmail(user).ConfigureAwait(false);

            await identity.Update(user).ConfigureAwait(false);

            return new Result<UserMessage>();
        }
    }
}