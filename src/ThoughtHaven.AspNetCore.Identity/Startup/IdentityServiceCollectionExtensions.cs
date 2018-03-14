using System.Reflection;
using ThoughtHaven;
using ThoughtHaven.AspNetCore.Identity.Created;
using ThoughtHaven.AspNetCore.Identity.Emails;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.LastLogins;
using ThoughtHaven.AspNetCore.Identity.Lockouts;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;
using ThoughtHaven.Data;
using ThoughtHaven.Messages.Emails;
using ThoughtHaven.Security.SingleUseTokens;
using static ThoughtHaven.AspNetCore.Identity.Startup.IdentityServices;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServiceCollectionExtensions
    {
        public static IServiceCollection AddThoughtHavenIdentity<TUser, TUserStore, TSingleUseTokenService, TTimedLockoutStore>(
            this IServiceCollection services, IdentityOptions options)
            where TUser : class, IUserKey, IUserId, IUserEmail, IUserCreated,
            IUserSecurityStamp, IUserLastLogin
            where TUserStore : class, ICrudStore<UserKey, TUser>,
            IRetrieveOperation<EmailAddress, TUser>
            where TSingleUseTokenService : class, ISingleUseTokenService
            where TTimedLockoutStore : class, ICrudStore<string, TimedLockout>
        {
            Guard.Null(nameof(services), services);
            Guard.Null(nameof(options), options);

            AddOptions(services, options);
            AddInfrastructure(services);
            AddPasswords(services);
            AddUserHelpers<TUser>(services);
            AddUserValidators<TUser>(services, options.User);
            AddUserStore<TUser, TUserStore>(services);
            AddUserEmailStore<TUser, TUserStore>(services);
            AddSingleUseTokenService<TSingleUseTokenService>(services);
            AddTimedLockoutStore<TTimedLockoutStore>(services);
            AddUserClaims<TUser>(services);
            AddIdentityServices<TUser>(services);

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = options.AuthenticationScheme;
                auth.DefaultChallengeScheme = options.AuthenticationScheme;
            })
            .AddCookie(options.AuthenticationScheme, cookie =>
            {
                var type = cookie.GetType();

                foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (!property.CanRead || !property.CanWrite) { continue; }

                    property.SetValue(cookie, property.GetValue(options.Cookie));
                }

                cookie.Validate();
            });

            return services;
        }
    }
}