using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ThoughtHaven.AspNetCore.Identity.Claims;
using ThoughtHaven.AspNetCore.Identity.Created;
using ThoughtHaven.AspNetCore.Identity.Emails;
using ThoughtHaven.AspNetCore.Identity.Internal;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.LastLogins;
using ThoughtHaven.AspNetCore.Identity.Lockouts;
using ThoughtHaven.AspNetCore.Identity.Passwords;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;
using ThoughtHaven.Data;
using ThoughtHaven.Contacts;
using ThoughtHaven.Reflection;
using ThoughtHaven.Security.SingleUseTokens;

namespace ThoughtHaven.AspNetCore.Identity.Startup
{
    public static class IdentityServices
    {
        public static void AddOptions(IServiceCollection services, IdentityOptions options)
        {
            Guard.Null(nameof(services), services);
            Guard.Null(nameof(options), options);
            
            services.TryAddSingleton(options);
            services.TryAddSingleton(options.Claims);
            services.TryAddSingleton(options.User);
        }

        public static void AddInfrastructure(IServiceCollection services)
        {
            Guard.Null(nameof(services), services);

            services.TryAddTransient<SystemClock>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void AddPasswords(IServiceCollection services)
        {
            Guard.Null(nameof(services), services);

            services.AddPwnedPasswordHttpClient();

            services.AddTransient<IPasswordStrengthValidator,
                MinimumLengthPasswordStrengthValidator>();
            services.AddTransient<IPasswordStrengthValidator,
                PwnedPasswordStrengthValidator>();
            services.TryAddTransient<IPasswordHasher, Pbkdf2PasswordHasher>();
        }

        public static void AddUserHelpers<TUser>(IServiceCollection services)
        {
            Guard.Null(nameof(services), services);

            services.TryAddTransient<IUserHelper, UserHelper<TUser>>();
        }

        public static void AddUserValidators<TUser>(IServiceCollection services,
            UserOptions options)
        {
            Guard.Null(nameof(services), services);
            Guard.Null(nameof(options), options);

            var userType = typeof(TUser);

            if (options.RequireId && userType.Implements<IUserId>())
            {
                services.AddTransient(typeof(IUserValidator<>).MakeGenericType(userType),
                    typeof(UserIdRequiredValidator<>).MakeGenericType(userType));
            }

            if (options.RequireCreated && userType.Implements<IUserCreated>())
            {
                services.AddTransient(typeof(IUserValidator<>).MakeGenericType(userType),
                    typeof(UserCreatedRequiredValidator<>).MakeGenericType(userType));
            }

            if (options.RequireSecurityStamp && userType.Implements<IUserSecurityStamp>())
            {
                services.AddTransient(typeof(IUserValidator<>).MakeGenericType(userType),
                    typeof(UserSecurityStampRequiredValidator<>).MakeGenericType(userType));
            }

            if (options.RequireEmail && userType.Implements<IUserEmail>())
            {
                services.AddTransient(typeof(IUserValidator<>).MakeGenericType(userType),
                    typeof(UserEmailRequiredValidator<>).MakeGenericType(userType));
            }

            if (options.RequirePassword && userType.Implements<IUserPasswordHash>())
            {
                services.AddTransient(typeof(IUserValidator<>).MakeGenericType(userType),
                    typeof(UserPasswordHashRequiredValidator<>).MakeGenericType(userType));
            }
        }

        public static void AddUserStore<TUser, TUserStore>(IServiceCollection services)
            where TUserStore : class, ICrudStore<UserKey, TUser>
        {
            Guard.Null(nameof(services), services);

            services.TryAddTransient<IRetrieveOperation<UserKey, TUser>, TUserStore>();
            services.TryAddTransient<ICreateOperation<TUser>, TUserStore>();
            services.TryAddTransient<IUpdateOperation<TUser>, TUserStore>();
            services.TryAddTransient<IDeleteOperation<UserKey>, TUserStore>();
            services.TryAddTransient<ICrudStore<UserKey, TUser>, TUserStore>();
        }

        public static void AddUserEmailStore<TUser, TUserEmailStore>(
            IServiceCollection services)
            where TUserEmailStore : class, IRetrieveOperation<EmailAddress, TUser>
        {
            Guard.Null(nameof(services), services);

            services.TryAddTransient<IRetrieveOperation<EmailAddress, TUser>,
                TUserEmailStore>();
        }

        public static void AddSingleUseTokenService<TSingleUseTokenService>(
            IServiceCollection services)
            where TSingleUseTokenService : class, ISingleUseTokenService
        {
            Guard.Null(nameof(services), services);

            services.TryAddTransient<ISingleUseTokenService, TSingleUseTokenService>();
        }

        public static void AddTimedLockoutStore<TTimedLockoutStore>(
            IServiceCollection services)
            where TTimedLockoutStore : class, ICrudStore<string, TimedLockout>
        {
            Guard.Null(nameof(services), services);

            services.TryAddTransient<ICrudStore<string, TimedLockout>, TTimedLockoutStore>();
        }

        public static void AddUserClaims<TUser>(IServiceCollection services)
            where TUser : IUserKey, IUserSecurityStamp
        {
            Guard.Null(nameof(services), services);

            services.TryAddTransient<IUserClaimsConverter<TUser>,
                UserClaimsConverter<TUser>>();
            services.TryAddTransient<IAuthenticationService<ClaimsPrincipal>,
                ClaimsAuthenticationService<TUser>>();
        }

        public static void AddIdentityServices<TUser>(IServiceCollection services)
            where TUser : class, IUserKey, IUserId, IUserCreated, IUserSecurityStamp,
            IUserLastLogin
        {
            Guard.Null(nameof(services), services);

            services.TryAddTransient<IAuthenticationService<TUser>, IdentityService<TUser>>();
            services.TryAddTransient<IIdentityServiceBase<TUser>, IdentityService<TUser>>();
            services.TryAddTransient<IIdentityService<TUser>, IdentityService<TUser>>();
        }
    }
}