using Microsoft.Extensions.DependencyInjection.Extensions;
using ThoughtHaven;
using ThoughtHaven.AspNetCore.Identity.Created;
using ThoughtHaven.AspNetCore.Identity.Emails;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.LastLogins;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;
using ThoughtHaven.Security.SingleUseTokens.AzureTableStorage;
using ThoughtHaven.AspNetCore.Identity.AzureTableStorage;
using ThoughtHaven.AspNetCore.Identity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TableStorageIdentityServiceCollectionExtensions
    {
        public static IServiceCollection AddThoughtHavenIdentity<TUser>(
            this IServiceCollection services, TableStorageIdentityOptions options)
            where TUser : class, IUserKey, IUserId, IUserEmail, IUserCreated,
            IUserSecurityStamp, IUserLastLogin, new()
        {
            Guard.Null(nameof(services), services);
            Guard.Null(nameof(options), options);

            services.TryAddSingleton(options);
            services.TryAddSingleton(options.TableStore);

            services.AddSingleUseTokens(options.SingleUseToken);

            return services.AddThoughtHavenIdentity<TUser, TableUserEmailStore<TUser>,
                TableSingleUseTokenService, TableTimedLockoutStore>(options);
        }
    }
}