using Microsoft.Extensions.DependencyInjection.Extensions;
using ThoughtHaven;
using ThoughtHaven.AspNetCore.Identity.AzureTableStorage;
using ThoughtHaven.AspNetCore.Identity.Created;
using ThoughtHaven.AspNetCore.Identity.Emails;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.LastLogins;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;
using ThoughtHaven.Security.SingleUseTokens.AzureTableStorage;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TableIdentityServiceCollectionExtensions
    {
        public static IServiceCollection AddThoughtHavenIdentity<TUser>(
            this IServiceCollection services, TableIdentityConfiguration configuration)
            where TUser : class, IUserKey, IUserId, IUserEmail, IUserCreated,
            IUserSecurityStamp, IUserLastLogin, new()
        {
            Guard.Null(nameof(services), services);
            Guard.Null(nameof(configuration), configuration);

            services.TryAddSingleton(configuration);
            services.TryAddSingleton(configuration.TableStore);

            services.AddSingleUseTokens(configuration.SingleUseToken);

            return services.AddThoughtHavenIdentity<TUser, TableUserEmailStore<TUser>,
                TableSingleUseTokenService, TableTimedLockoutStore>(configuration);
        }
    }
}