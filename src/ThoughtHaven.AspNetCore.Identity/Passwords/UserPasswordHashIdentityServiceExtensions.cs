using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.Passwords;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;

namespace ThoughtHaven.AspNetCore.Identity
{
    public static partial class IdentityHelperServiceExtensions
    {
        public static async Task<Result<UserMessage>> ValidatePassword<TUser>(
            this IIdentityService<TUser> identity, TUser user, Password password)
            where TUser : IUserPasswordHash
        {
            Guard.Null(nameof(identity), identity);
            Guard.Null(nameof(user), user);
            Guard.Null(nameof(password), password);

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                throw new ArgumentException(
                    paramName: nameof(user),
                    message: $"The {nameof(user)} argument's {nameof(user.PasswordHash)} property cannot be null or white space.");
            }

            var passwordResult = await identity.Helper.ValidatePassword(user, password)
                .ConfigureAwait(false);

            if (!passwordResult.Valid) { return identity.Helper.InvalidPassword; }

            if (passwordResult.UpdateHash)
            {
                await identity.Helper.SetPasswordHash(user, password).ConfigureAwait(false);

                await identity.Update(user).ConfigureAwait(false);
            }

            return new Result<UserMessage>();
        }

        public static Task<PasswordResetCode> ForgotPassword<TUser>(
            this IIdentityService<TUser> identity, TUser user)
            where TUser : IUserKey, IUserPasswordHash
        {
            Guard.Null(nameof(identity), identity);
            Guard.Null(nameof(user), user);

            var key = user.Key();
            if (key == null)
            {
                throw new ArgumentException(
                    paramName: nameof(user),
                    message: $"The {nameof(user)} argument's {nameof(user.Key)}() method cannot return null.");
            }

            return identity.Helper.CreatePasswordResetCode(key);
        }

        public static async Task<Result<UserMessage>> UpdatePassword<TUser>(
            this IIdentityService<TUser> identity, TUser user, Password current,
            Password updated)
            where TUser : IUserPasswordHash, IUserSecurityStamp
        {
            Guard.Null(nameof(identity), identity);
            Guard.Null(nameof(user), user);
            Guard.Null(nameof(current), current);
            Guard.Null(nameof(updated), updated);

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                throw new ArgumentException(
                    paramName: nameof(user),
                    message: $"The {nameof(user)} argument's {nameof(user.PasswordHash)} property cannot be null or white space.");
            }

            var result = await identity.ValidatePassword(user, current)
                .ConfigureAwait(false);

            if (!result.Success) { return result.Failure; }

            result = await identity.Helper.SetPasswordHash(user, updated)
                .ConfigureAwait(false);

            if (result.Success)
            {
                await identity.Helper.RefreshSecurityStamp(user).ConfigureAwait(false);

                await identity.Update(user).ConfigureAwait(false);
            }

            return result;
        }

        public static async Task<Result<UserMessage>> UpdatePassword<TUser>(
            this IIdentityService<TUser> identity, TUser user, PasswordResetCode code,
            Password password)
            where TUser : IUserKey, IUserPasswordHash, IUserSecurityStamp
        {
            Guard.Null(nameof(identity), identity);
            Guard.Null(nameof(user), user);
            Guard.Null(nameof(code), code);
            Guard.Null(nameof(password), password);

            var key = user.Key();
            if (key == null)
            {
                throw new ArgumentException(
                    paramName: nameof(user),
                    message: $"The {nameof(user)} argument's {nameof(user.Key)}() method cannot return null.");
            }

            var validCode = await identity.Helper.ValidatePasswordResetCode(key, code)
                .ConfigureAwait(false);

            if (!validCode) { return identity.Helper.InvalidPasswordResetCode; }

            var result = await identity.Helper.SetPasswordHash(user, password)
                .ConfigureAwait(false);

            if (result.Success)
            {
                await identity.Helper.RefreshSecurityStamp(user).ConfigureAwait(false);

                await identity.Update(user).ConfigureAwait(false);
            }

            return result;
        }
    }
}