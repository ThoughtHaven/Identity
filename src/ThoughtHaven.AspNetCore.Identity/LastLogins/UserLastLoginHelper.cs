using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.LastLogins;

namespace ThoughtHaven.AspNetCore.Identity
{
    public abstract partial class UserHelper
    {
        public virtual Task SetLastLogin<TUser>(TUser user) where TUser : IUserLastLogin
        {
            Guard.Null(nameof(user), user);

            user.LastLogin = this.Clock.UtcNow;

            return Task.CompletedTask;
        }
    }
}