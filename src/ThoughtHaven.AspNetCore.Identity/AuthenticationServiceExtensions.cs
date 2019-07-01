using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace ThoughtHaven.AspNetCore.Identity
{
    public static class AuthenticationServiceExtensions
    {
        public static Task Login<T>(this IAuthenticationService<T> authentication, T login)
            where T : class
        {
            Guard.Null(nameof(authentication), authentication);
            Guard.Null(nameof(login), login);

            return authentication.Login(login, new AuthenticationProperties());
        }

        public static Task Login<T>(this IAuthenticationService<T> authentication, T login,
            bool persistent)
            where T : class
        {
            Guard.Null(nameof(authentication), authentication);
            Guard.Null(nameof(login), login);

            return authentication.Login(login, new AuthenticationProperties()
            {
                IsPersistent = persistent
            });
        }
    }
}