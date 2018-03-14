using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using ThoughtHaven.AspNetCore.Identity.Emails;
using ThoughtHaven.AspNetCore.Identity.Passwords;
using ThoughtHaven.Messages.Emails;

namespace ThoughtHaven.AspNetCore.Identity
{
    public static partial class IdentityServiceExtensions
    {
        public static async Task<Result<TUser, UserMessage>> Create<TUser>(
            this IIdentityService<TUser> identity, EmailAddress email, Password password)
            where TUser : IUserEmail, IUserPasswordHash, new()
        {
            Guard.Null(nameof(identity), identity);
            Guard.Null(nameof(email), email);
            Guard.Null(nameof(password), password);

            var existingUser = await identity.Retrieve(email).ConfigureAwait(false);

            if (existingUser != null) { return identity.Helper.EmailNotAvailable; }

            var user = new TUser();

            await identity.Helper.SetEmail(user, email).ConfigureAwait(false);

            var result = await identity.Helper.SetPasswordHash(user, password)
                .ConfigureAwait(false);

            if (!result.Success) { return result.Failure; }

            await identity.Create(user).ConfigureAwait(false);

            return user;
        }

        public static async Task<Result<TUser, UserMessage>> Login<TUser>(
            this IIdentityService<TUser> identity, EmailAddress email, Password password,
            AuthenticationProperties properties)
            where TUser : IUserEmail, IUserPasswordHash, new()
        {
            Guard.Null(nameof(identity), identity);
            Guard.Null(nameof(email), email);
            Guard.Null(nameof(password), password);
            Guard.Null(nameof(properties), properties);

            var lockedOut = await identity.Helper.IsLockedOut(email.Value)
                .ConfigureAwait(false);

            if (lockedOut) { return identity.Helper.LockedOut; }

            var user = await identity.Retrieve(email).ConfigureAwait(false);

            if (user == null) { return identity.Helper.InvalidCredentials; }

            var passwordResult = await identity.Helper.ValidatePassword(user, password)
                .ConfigureAwait(false);

            if (!passwordResult.Valid) { return identity.Helper.InvalidCredentials; }

            if (passwordResult.UpdateHash)
            {
                await identity.Helper.SetPasswordHash(user, password).ConfigureAwait(false);
            }

            await identity.Login(user, properties).ConfigureAwait(false);

            return user;
        }

        public static Task<Result<TUser, UserMessage>> Login<TUser>(
            this IIdentityService<TUser> identity, EmailAddress email, Password password,
            bool persistent = false)
            where TUser : IUserEmail, IUserPasswordHash, new() =>
            identity.Login(email, password, new AuthenticationProperties()
            {
                IsPersistent = persistent
            });
    }
}