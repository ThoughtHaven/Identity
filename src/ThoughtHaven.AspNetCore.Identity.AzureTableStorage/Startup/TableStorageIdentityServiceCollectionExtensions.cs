using Microsoft.Extensions.DependencyInjection.Extensions;
using ThoughtHaven;
using ThoughtHaven.AspNetCore.Identity.Created;
using ThoughtHaven.AspNetCore.Identity.Emails;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.LastLogins;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;
using ThoughtHaven.Security.SingleUseTokens;
using ThoughtHaven.AspNetCore.Identity.Stores;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TableStorageIdentityServiceCollectionExtensions
    {
        public static IServiceCollection AddThoughtHavenIdentity<TUser>(
            this IServiceCollection services, string storageAccountConnectionString,
            TableStorageIdentityOptions options = null)
            where TUser : class, IUserKey, IUserId, IUserEmail, IUserCreated,
            IUserSecurityStamp, IUserLastLogin, new()
        {
            Guard.Null(nameof(services), services);
            Guard.NullOrWhiteSpace(nameof(storageAccountConnectionString),
                storageAccountConnectionString);

            options = options ?? new TableStorageIdentityOptions();

            services.TryAddSingleton(options);
            services.TryAddSingleton(options.TableStore);
            services.TryAddSingleton(options.TableRequest);

            services.AddSingleUseTokens(storageAccountConnectionString, options.SingleUseToken);

            return services.AddThoughtHavenIdentity<TUser, TableUserEmailStore<TUser>,
                TableSingleUseTokenService, TableTimedLockoutStore>(options);
        }
    }
}