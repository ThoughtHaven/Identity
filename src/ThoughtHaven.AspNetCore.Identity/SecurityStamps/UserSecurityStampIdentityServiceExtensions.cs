using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;

namespace ThoughtHaven.AspNetCore.Identity
{
    public static partial class IdentityHelperServiceExtensions
    {
        public static async Task Logout<TUser>(this IIdentityService<TUser> identity,
            bool logoutEverywhere)
            where TUser : class, IUserSecurityStamp
        {
            Guard.Null(nameof(identity), identity);

            if (logoutEverywhere)
            {
                var user = await identity.Authenticate().ConfigureAwait(false);

                if (user != null)
                {
                    await identity.Helper.RefreshSecurityStamp(user).ConfigureAwait(false);

                    await identity.Update(user).ConfigureAwait(false);
                }
            }

            await identity.Logout().ConfigureAwait(false);
        }
    }
}